// <copyright file="GoogleProvider.cs" company="Engage Software">
// Engage: Locator - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

// Because someone is storing the full class name in the TabModuleSettings we can exceed 50 char for the fully
// qualified name. So we can't use Engage.Dnn.Locator.Providers.MapProviders as it should be. hk!
namespace Engage.Dnn.Locator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Web.UI;

    using DotNetNuke.Common.Lists;

    using Maps;

    /// <summary>
    /// A <see cref="MapProvider"/> for Google Maps
    /// </summary>
    public class GoogleProvider : MapProvider
    {
        /// <summary>
        /// The base URL to use to geocode an address.  Add QueryString parameters <c>&amp;q</c> for the location address and <c>&amp;key</c> for the API key.
        /// </summary>
        private const string SearchUrl = "http://maps.google.com/maps/geo?output=csv&sensor=false";

        /// <summary>
        /// Registers the JavaScript to display the map.
        /// </summary>
        /// <param name="scriptManager">The page's script manager.</param>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="mapSectionId">The ID of the section (div) on the page in which the map should be created.</param>
        /// <param name="currentLocationSpanId">The ID of the span showing the current location text.</param>
        /// <param name="noLocationSpanId">The ID of the span shown when no location is selected.</param>
        /// <param name="instructionSpanId">The ID of the span with driving directions, etc.</param>
        /// <param name="directionsLinkId">The ID of the link to driving directions.</param>
        /// <param name="directionsSectionId">The ID of the section (div) with driving directions text.</param>
        /// <param name="locations">The list of locations to display.</param>
        /// <param name="showAllLocationsOnLoad">if set to <c>true</c> shows the map with all locations on it by default.</param>
        public override void GenerateMapScriptCore(ScriptManager scriptManager, MapType mapType, string mapSectionId, string currentLocationSpanId, string noLocationSpanId, string instructionSpanId, string directionsLinkId, string directionsSectionId, LocationCollection locations, bool showAllLocationsOnLoad)
        {
            ICollection<JavaScript.Location> locationsAsJson = locations.AsJson();
            string mapParameters = String.Format(CultureInfo.InvariantCulture, "currentLocationSpan: {0}, noLocationSpan: {1}, instructionSpan: {2}, directionsLink: {3}, directionsSection: {4}, mapType: {5}, locationsArray: {6}", GetElementJavaScript(currentLocationSpanId), GetElementJavaScript(noLocationSpanId), GetElementJavaScript(instructionSpanId), GetElementJavaScript(directionsLinkId), GetElementJavaScript(directionsSectionId), ConvertMapType(mapType), new JavaScriptSerializer().Serialize(locationsAsJson));

            scriptManager.Scripts.Add(new ScriptReference(GetLoaderUrl(this.ApiKey)));
            scriptManager.Scripts.Add(new ScriptReference("Engage.Dnn.Locator.JavaScript.BaseLocator.js", "EngageLocator"));
            scriptManager.Scripts.Add(new ScriptReference("Engage.Dnn.Locator.JavaScript.GoogleLocator.js", "EngageLocator"));
            ScriptManager.RegisterStartupScript(
                    scriptManager.Page,
                    typeof(GoogleProvider),
                    "Initialize",
                    "google.setOnLoadCallback(jQuery(function(){ jQuery.noConflict(); $create(Engage.Dnn.Locator.GoogleMap, {" + mapParameters + "}, {}, {}, $get('" + mapSectionId + "')); }));",
                    true);

            if (showAllLocationsOnLoad)
            {
                ScriptManager.RegisterStartupScript(
                        scriptManager.Page,
                        typeof(GoogleProvider),
                        "showAllLocations",
                        "google.setOnLoadCallback(jQuery(function(){ $find('" + mapSectionId + "$GoogleMap').showAllLocations(); }));",
                        true);
            }
        }

        /// <summary>
        /// Determines whether this provider's <see name="ApiKey"/> is in a valid format.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this provider's <see name="ApiKey"/> is in a valid format; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsKeyValid()
        {
            return this.ApiKey != null && this.ApiKey.Length == 86;
        }

        /// <summary>
        /// Gets the latitude and longitude of the given location.
        /// </summary>
        /// <param name="street">The street of the address.</param>
        /// <param name="city">The city of the address.</param>
        /// <param name="regionId">The ID of the region of the address.</param>
        /// <param name="zip">The zip code of the address.</param>
        /// <param name="countryId">The ID of the country of the address</param>
        /// <returns>The geocoding result</returns>
        public override GeocodeResult GeocodeLocation(string street, string city, int? regionId, string zip, int? countryId)
        {
            if (this.IsKeyValid())
            {
                try
                {
                    StringBuilder queryUrl = new StringBuilder();
                    queryUrl.Append(SearchUrl)
                        .Append("&q=")
                        .Append(HttpUtility.UrlEncode(street))
                        .Append(HttpUtility.UrlEncode(" "))
                        .Append(HttpUtility.UrlEncode(city))
                        .Append(HttpUtility.UrlEncode(" "))
                        .Append(HttpUtility.UrlEncode(GetRegionAbbreviation(regionId)))
                        .Append(HttpUtility.UrlEncode(" "))
                        .Append(HttpUtility.UrlEncode(zip))
                        .Append("&key=")
                        .Append(HttpUtility.UrlEncode(this.ApiKey));

                    string countryAbbreviation = GetCountryAbbreviation(countryId);
                    if (!string.IsNullOrEmpty(countryAbbreviation))
                    {
                        queryUrl.Append("&gl=")
                            .Append(countryAbbreviation);
                    }

                    string[] csvResults = new WebClient().DownloadString(queryUrl.ToString()).Split(',');

                    return new GoogleGeocodeResult(
                        double.Parse(csvResults[2], CultureInfo.InvariantCulture),
                        double.Parse(csvResults[3], CultureInfo.InvariantCulture),
                        (GoogleAccuracyCode)int.Parse(csvResults[1], CultureInfo.InvariantCulture),
                        (GoogleStatusCode)int.Parse(csvResults[0], CultureInfo.InvariantCulture));
                }
                catch (WebException)
                {
                    return new GoogleGeocodeResult(double.NaN, double.NaN, GoogleAccuracyCode.Unknown, GoogleStatusCode.ServerError);
                }
            }

            return GeocodeResult.Empty;
        }

        /// <summary>
        /// Converts <paramref name="mapType"/> into its Google Maps enumeration value.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <returns>The value of the map type for Google Maps</returns>
        private static string ConvertMapType(MapType mapType)
        {
            switch (mapType)
            {
                case MapType.Hybrid:
                    return GoogleMapType.G_HYBRID_MAP.ToString();
                case MapType.Satellite:
                    return GoogleMapType.G_SATELLITE_MAP.ToString();
                default:
                    return GoogleMapType.G_NORMAL_MAP.ToString();
            }
        }

        /// <summary>
        /// Gets the ccTLD (country top-level domain) abbreviation for the country with the given ID.
        /// </summary>
        /// <param name="countryId">The country ID.</param>
        /// <returns>The abbreviation for the given country</returns>
        private static string GetCountryAbbreviation(int? countryId)
        {
            if (countryId.HasValue)
            {
                ListEntryInfo countryEntry = new ListController().GetListEntryInfo(countryId.Value);

                if (countryEntry != null)
                {
                    return countryEntry.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the URL for the Google Ajax API Loader.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>A URL to the Google Loader script</returns>
        /// <exception cref="ArgumentNullException">API Key is required.</exception>
        /// <exception cref="ArgumentException">API Key is required.</exception>
        private static string GetLoaderUrl(string apiKey)
        {
            if (String.IsNullOrEmpty(apiKey))
            {
                if (apiKey == null)
                {
                    throw new ArgumentNullException("apiKey", "API Key is required");
                }

                throw new ArgumentException("API Key is required", "apiKey");
            }

            return "http://www.google.com/jsapi?key=" + apiKey;
        }
    }
}