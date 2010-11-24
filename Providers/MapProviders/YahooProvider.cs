// <copyright file="YahooProvider.cs" company="Engage Software">
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
    using System.Xml;

    using DotNetNuke.Common.Lists;

    using Maps;

    /// <summary>
    /// A <see cref="MapProvider"/> for Yahoo! Maps
    /// </summary>
    public class YahooProvider : MapProvider
    {
        /// <summary>
        /// The base URL to use to geocode an address.  Add QueryString parameters <c>&amp;location</c> for the location address and <c>&amp;appid</c> for the API key.  
        /// Alternatively, you can specify <c>&amp;street</c>, <c>&amp;city</c>, <c>&amp;state</c>, and <c>&amp;zip</c> parameters in the place of <c>&amp;location</c>.
        /// </summary>
        public const string SearchUrl = "http://local.yahooapis.com/MapsService/V1/geocode?output=xml";

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

            scriptManager.Scripts.Add(new ScriptReference("http://api.maps.yahoo.com/ajaxymap?v=3.8&appid=" + this.ApiKey));
            ////scriptManager.Scripts.Add(new ScriptReference("http://ajax.googleapis.com/ajax/libs/jquery/1.2.6/jquery.min.js"));
            scriptManager.Scripts.Add(new ScriptReference("Engage.Dnn.Locator.JavaScript.BaseLocator.js", "EngageLocator"));
            scriptManager.Scripts.Add(new ScriptReference("Engage.Dnn.Locator.JavaScript.YahooLocator.js", "EngageLocator"));
            ScriptManager.RegisterStartupScript(
                    scriptManager.Page,
                    typeof(GoogleProvider),
                    "Initialize",
                    "jQuery(function(){ jQuery.noConflict(); $create(Engage.Dnn.Locator.YahooMap, {" + mapParameters + "}, {}, {}, $get('" + mapSectionId + "')); });",
                    true);

            if (showAllLocationsOnLoad)
            {
                ScriptManager.RegisterStartupScript(
                        scriptManager.Page,
                        typeof(GoogleProvider),
                        "showAllLocations",
                        "jQuery(function(){ $find('" + mapSectionId + "$YahooMap').showAllLocations(); });",
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
            return this.ApiKey != null;
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
                string regionAbbreviation = GetRegionAbbreviation(regionId);
                string countryName = GetCountryName(countryId);

                YahooGeocodeResult parameterizedResult = GeocodeLocation(GetCityStateParams(street, city, regionAbbreviation, zip), this.ApiKey);
                if (parameterizedResult.Successful && parameterizedResult.AccuracyCode >= YahooAccuracyCode.Address)
                {
                    return parameterizedResult;
                }

                YahooGeocodeResult freeformResult = GeocodeLocation(GetFreeformParam(street, city, regionAbbreviation, zip, countryName), this.ApiKey);
                if (!freeformResult.Successful || !parameterizedResult.Successful)
                {
                    if (freeformResult.Successful)
                    {
                        return freeformResult;
                    }

                    return parameterizedResult;
                }

                if (freeformResult.AccuracyCode > parameterizedResult.AccuracyCode)
                {
                    return freeformResult;
                }

                return parameterizedResult;
            }

            return GeocodeResult.Empty;
        }

        /// <summary>
        /// Gets the geocode result for the given <paramref name="queryParams"/>
        /// </summary>
        /// <param name="queryParams">The query paramters for the location to geocode.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>The geocode result from the request to </returns>
        private static YahooGeocodeResult GeocodeLocation(string queryParams, string apiKey)
        {
            double latitude = double.NaN;
            double longitude = double.NaN;
            YahooAccuracyCode accuracyCode;
            YahooStatusCode statusCode;

            try
            {
                using (XmlReader resultsReader = XmlReader.Create(SearchUrl + queryParams + "&appid=" + apiKey))
                {
                    resultsReader.Read();
                    if (resultsReader.IsStartElement("ResultSet"))
                    {
                        resultsReader.ReadStartElement("ResultSet");

                        accuracyCode = GetYahooAccuracyCode(resultsReader.GetAttribute("precision"));
                        resultsReader.ReadStartElement("Result");

                        resultsReader.ReadStartElement("Latitude");
                        latitude = resultsReader.ReadContentAsDouble();
                        resultsReader.ReadEndElement();

                        resultsReader.ReadStartElement("Longitude");
                        longitude = resultsReader.ReadContentAsDouble();
                    }
                    else
                    {
                        accuracyCode = YahooAccuracyCode.Unknown;
                    }

                    statusCode = YahooStatusCode.Success;
                }
            }
            catch (WebException exc)
            {
                accuracyCode = YahooAccuracyCode.Unknown;
                statusCode = YahooStatusCode.ServiceUnavailable;
                if (exc.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = (HttpWebResponse)exc.Response;
                    switch (response.StatusCode)
                    {
                            // 400
                        case HttpStatusCode.BadRequest:
                            statusCode = YahooStatusCode.BadRequest;
                            break;

                            // 403
                        case HttpStatusCode.Forbidden:
                            statusCode = YahooStatusCode.Forbidden;
                            break;

                            // 503
                        case HttpStatusCode.ServiceUnavailable: 
                            statusCode = YahooStatusCode.ServiceUnavailable;
                            break;
                    }
                }
            }

            return new YahooGeocodeResult(latitude, longitude, accuracyCode, statusCode);
        }

        /// <summary>
        /// Converts <paramref name="mapType"/> into its Yahoo! Maps enumeration value.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <returns>The value of the map type for Google Maps</returns>
        private static string ConvertMapType(MapType mapType)
        {
            switch (mapType)
            {
                case MapType.Hybrid:
                    return YahooMapType.YAHOO_MAP_HYB.ToString();
                case MapType.Satellite:
                    return YahooMapType.YAHOO_MAP_SAT.ToString();
                default:
                    return YahooMapType.YAHOO_MAP_REG.ToString();
            }
        }

        /// <summary>
        /// Gets the name of the country with the given ID.
        /// </summary>
        /// <param name="countryId">The country ID.</param>
        /// <returns>The name of the country, or <c>null</c> if <paramref name="countryId"/> is <c>null</c></returns>
        private static string GetCountryName(int? countryId)
        {
            return countryId.HasValue ? new ListController().GetListEntryInfo(countryId.Value).Text : null;
        }

        /// <summary>
        /// Converts the given accuracy code into a <see cref="YahooAccuracyCode"/> value
        /// </summary>
        /// <param name="accuracyValue">The accuracy value returned from the web service.</param>
        /// <returns>The <see cref="YahooAccuracyCode"/> value represented by <paramref name="accuracyValue"/></returns>
        private static YahooAccuracyCode GetYahooAccuracyCode(string accuracyValue)
        {
            YahooAccuracyCode accuracyCode;
            switch (accuracyValue)
            {
                case "country":
                    accuracyCode = YahooAccuracyCode.Country;
                    break;
                case "state":
                    accuracyCode = YahooAccuracyCode.State;
                    break;
                case "city":
                    accuracyCode = YahooAccuracyCode.City;
                    break;
                case "zip":
                    accuracyCode = YahooAccuracyCode.Zip;
                    break;
                case "zip+2":
                    accuracyCode = YahooAccuracyCode.ZipPlus2;
                    break;
                case "zip+4":
                    accuracyCode = YahooAccuracyCode.ZipPlus4;
                    break;
                case "street":
                    accuracyCode = YahooAccuracyCode.Street;
                    break;
                case "address":
                    accuracyCode = YahooAccuracyCode.Address;
                    break;
                default:
                    accuracyCode = YahooAccuracyCode.Unknown;
                    break;
            }

            return accuracyCode;
        }

        /// <summary>
        /// Gets the QueryString parameters for the given location as a collection of structured parameters.
        /// </summary>
        /// <param name="street">The street address of the location.</param>
        /// <param name="city">The city of the location.</param>
        /// <param name="regionAbbreviation">The abbreviation of the region for the location.</param>
        /// <param name="zip">The postal code of the location.</param>
        /// <returns>The QueryString parameters for the given location</returns>
        private static string GetCityStateParams(string street, string city, string regionAbbreviation, string zip)
        {
            StringBuilder queryParams = new StringBuilder();

            if (!string.IsNullOrEmpty(street))
            {
                queryParams.Append("&street=" + HttpUtility.UrlEncode(street));
            }

            if (!string.IsNullOrEmpty(city))
            {
                queryParams.Append("&city=" + HttpUtility.UrlEncode(city));
            }

            if (!string.IsNullOrEmpty(regionAbbreviation))
            {
                queryParams.Append("&state=" + HttpUtility.UrlEncode(regionAbbreviation));
            }

            if (!string.IsNullOrEmpty(zip))
            {
                queryParams.Append("&zip=" + HttpUtility.UrlEncode(zip));
            }

            return queryParams.ToString();
        }

        /// <summary>
        /// Gets the QueryString parameter for the given location as a single, unstructured value.
        /// </summary>
        /// <param name="street">The street address of the location.</param>
        /// <param name="city">The city of the location.</param>
        /// <param name="regionAbbreviation">The abbreviation of the region for the location.</param>
        /// <param name="zip">The postal code of the location.</param>
        /// <param name="countryName">The country of the location.</param>
        /// <returns>The QueryString parameter for the given location</returns>
        private static string GetFreeformParam(string street, string city, string regionAbbreviation, string zip, string countryName)
        {
            StringBuilder queryParam = new StringBuilder("&location=");

            if (!string.IsNullOrEmpty(street))
            {
                queryParam.Append(HttpUtility.UrlEncode(street + ", "));
            }

            if (!string.IsNullOrEmpty(city))
            {
                queryParam.Append(HttpUtility.UrlEncode(city + ", "));
            }

            if (!string.IsNullOrEmpty(regionAbbreviation))
            {
                queryParam.Append(HttpUtility.UrlEncode(regionAbbreviation + ", "));
            }

            if (!string.IsNullOrEmpty(zip))
            {
                queryParam.Append(HttpUtility.UrlEncode(zip + ", "));
            }

            if (!string.IsNullOrEmpty(countryName))
            {
                queryParam.Append(HttpUtility.UrlEncode(countryName));
            }

            return queryParam.ToString();
        }
    }
}
