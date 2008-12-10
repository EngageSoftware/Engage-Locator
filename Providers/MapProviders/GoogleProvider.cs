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
    using System.Web.Script.Serialization;
    using System.Web.UI;

    using Maps;

    /// <summary>
    /// A <see cref="MapProvider"/> for Google Maps
    /// </summary>
    public class GoogleProvider : MapProvider
    {
        /// <summary>
        /// Backing field for <see cref="LocatorTabModuleId"/>
        /// </summary>
        private int locatorTabModuleId;

        public int LocatorTabModuleId
        {
            get { return this.locatorTabModuleId; }
            set { this.locatorTabModuleId = value; }
        }

        public static string SearchUrl
        {
            get {return  "http://maps.google.com/maps/geo?output=csv";}
        }

        public override string Url
        {
            get { return SearchUrl; }
        }

        public override string MapProviderUrl
        {
            get { return "http://maps.google.com/maps?f=q&hl=en&geocode=&q="; }
        }

        public override string GenerateMiniMapScriptCore()
        {
            throw new NotImplementedException();
        }

        public override void GenerateMapScriptCore(ScriptManager scriptManager, string apiKey, MapType mapType, string mapSectionId, string currentLocationSpanId, string noLocationSpanId, string instructionSpanId, string directionsLinkId, string directionsSectionId, List<Location> locations, bool showAllLocationsOnLoad)
        {
            List<JavaScript.Location> locationsAsJson = locations.ConvertAll(delegate(Location location)
                {
                    return new JavaScript.Location(location.Latitude, location.Longitude, location.FullAddress + Environment.NewLine + location.Address3);
                });
            string mapParameters = string.Format(CultureInfo.InvariantCulture, "currentLocationSpan: {0}, noLocationSpan: {1}, instructionSpan: {2}, directionsLink: {3}, directionsSection: {4}, mapType: {5}, locationsArray: {6}", GetElementJavaScript(currentLocationSpanId), GetElementJavaScript(noLocationSpanId), GetElementJavaScript(instructionSpanId), GetElementJavaScript(directionsLinkId), GetElementJavaScript(directionsSectionId), ConvertMapType(mapType), new JavaScriptSerializer().Serialize(locationsAsJson));

            scriptManager.Scripts.Add(new ScriptReference(GetLoaderUrl(apiKey)));
            scriptManager.Scripts.Add(new ScriptReference("Engage.Dnn.Locator.JavaScript.GoogleLocator.js", "EngageLocator"));
            ScriptManager.RegisterStartupScript(
                    scriptManager.Page,
                    typeof(GoogleProvider),
                    "Initialize",
                    "google.setOnLoadCallback(function(){ jQuery.noConflict(); $create(Engage.Dnn.Locator.GoogleMap, {" + mapParameters + "}, {}, {}, $get('" + mapSectionId + "')); });",
                    true);

            if (showAllLocationsOnLoad)
            {
                ScriptManager.RegisterStartupScript(
                        scriptManager.Page,
                        typeof(GoogleProvider),
                        "showAllLocations",
                        "google.setOnLoadCallback(function(){ $find('" + mapSectionId + "$GoogleMap').showAllLocations(); });",
                        true);
            }
        }

        public override bool IsKeyValid(string apiKey)
        {
            return apiKey != null && apiKey.Length == 86;
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
        /// Returns the JavaScript to select the element with the given ID.
        /// </summary>
        /// <param name="elementId">The ID of the element to get.</param>
        /// <returns>The JavaScript to select the element with the given ID</returns>
        private static string GetElementJavaScript(string elementId)
        {
            return "jQuery('#" + elementId + "')";
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

    /// <summary>
    /// The type of Google map to initially display
    /// </summary>
    internal enum GoogleMapType
    {
        /// <summary>
        /// normal Map
        /// </summary>
        G_NORMAL_MAP,

        /// <summary>
        /// Sattelite Map
        /// </summary>
        G_SATELLITE_MAP,

        /// <summary>
        /// Hybrid Map
        /// </summary>
        G_HYBRID_MAP
    }

    public struct GoogleGeocodeResult
    {
        public double latitude;
        public double longitude;

        public GoogleAccuracyCode accuracyCode;
        public GoogleStatusCode statusCode;
    }

    public enum GoogleStatusCode
    {
        /// <summary>
        /// No errors occurred; the address was successfully parsed and its geocode has been returned.
        /// </summary>
        Success = 200,
        /// <summary>
        /// A geocoding request could not be successfully processed, yet the exact reason for the failure is not known.
        /// </summary>
        ServerError = 500,
        /// <summary>
        /// The HTTP q parameter was either missing or had no value.
        /// </summary>
        MissingAddress = 601,
        /// <summary>
        /// No corresponding geographic location could be found for the specified address. This may be due to the fact that the address is relatively new, or it may be incorrect.
        /// </summary>
        UnknownAddress = 602,
        /// <summary>
        /// The geocode for the given address cannot be returned due to legal or contractual reasons.
        /// </summary>
        UnavailableAddress = 603,
        /// <summary>
        /// The given key is either invalid or does not match the domain for which it was given.
        /// </summary>
        BadKey = 610
    }

    public enum GoogleAccuracyCode
    {
        Unknown = 0,
        Country = 1,
        Region = 2,
        Subregion = 3,
        Town = 4,
        PostalCode = 5,
        Street = 6,
        Intersection = 7,
        Address = 8
    }
}