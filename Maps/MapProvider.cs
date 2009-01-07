// <copyright file="MapProvider.cs" company="Engage Software">
// Engage: Locator - http://www.engagemodules.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Locator.Maps
{
    using System;
    using System.Web.UI;

    using DotNetNuke.Common.Lists;

    /// <summary>
    /// An abstract base class for map providers, including the ability to display maps in HTML and JavaScript, and the ability to geocode locations.
    /// </summary>
    public abstract class MapProvider
    {
        /// <summary>
        /// Backing field for <see cref="ApiKey"/>
        /// </summary>
        private string apiKey;

        /// <summary>
        /// Gets the API key for this provider.
        /// </summary>
        /// <value>The API key.</value>
        public string ApiKey
        {
            get { return this.apiKey; }
        }

        /// <summary>
        /// Creates a concrete instance of a <see cref="MapProvider"/> with the given <paramref name="providerType"/>.
        /// </summary>
        /// <param name="providerType">Type of the map provider.</param>
        /// <param name="apiKey">The API key for this provider.</param>
        /// <returns>
        /// An instantiated <see cref="MapProvider"/>
        /// </returns>
        public static MapProvider CreateInstance(MapProviderType providerType, string apiKey)
        {
            return CreateInstance(providerType.ClassName, apiKey);
        }

        /// <summary>
        /// Creates a concrete instance of a <see cref="MapProvider"/> with the given <paramref name="className"/>.
        /// </summary>
        /// <param name="className">The fully qualified name of the <see cref="MapProvider"/> type to instantiate.</param>
        /// <param name="apiKey">The API key for this provider.</param>
        /// <returns>
        /// An instantiated <see cref="MapProvider"/>
        /// </returns>
        public static MapProvider CreateInstance(string className, string apiKey)
        {
            Type objectType = Type.GetType(className, true, true);
            MapProvider mapProvider = (MapProvider)Activator.CreateInstance(objectType);
            mapProvider.apiKey = apiKey;
            return mapProvider;
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
        public abstract GeocodeResult GeocodeLocation(string street, string city, int? regionId, string zip, int? countryId);

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
        public void RegisterMapScript(ScriptManager scriptManager, MapType mapType, string mapSectionId, string currentLocationSpanId, string noLocationSpanId, string instructionSpanId, string directionsLinkId, string directionsSectionId, LocationCollection locations, bool showAllLocationsOnLoad)
        {
            this.GenerateMapScriptCore(scriptManager, mapType, mapSectionId, currentLocationSpanId, noLocationSpanId, instructionSpanId, directionsLinkId, directionsSectionId, locations, showAllLocationsOnLoad);
        }

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
        public abstract void GenerateMapScriptCore(ScriptManager scriptManager, MapType mapType, string mapSectionId, string currentLocationSpanId, string noLocationSpanId, string instructionSpanId, string directionsLinkId, string directionsSectionId, LocationCollection locations, bool showAllLocationsOnLoad);

        /// <summary>
        /// Determines whether this provider's <see name="ApiKey"/> is in a valid format.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this provider's <see name="ApiKey"/> is in a valid format; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsKeyValid();

        /// <summary>
        /// Returns the JavaScript to select the element with the given ID.
        /// </summary>
        /// <param name="elementId">The ID of the element to get.</param>
        /// <returns>The JavaScript to select the element with the given ID</returns>
        protected static string GetElementJavaScript(string elementId)
        {
            return "jQuery('#" + elementId + "')";
        }

        /// <summary>
        /// Gets the abbreviation for the region with the given ID.
        /// </summary>
        /// <param name="regionId">The region id.</param>
        /// <returns>The abbreviation for the region, or <c>null</c> if <paramref name="regionId"/> is <c>null</c></returns>
        protected static string GetRegionAbbreviation(int? regionId)
        {
            if (regionId.HasValue)
            {
                ListEntryInfo regionEntry = new ListController().GetListEntryInfo(regionId.Value);
                if (regionEntry != null)
                {
                    return regionEntry.Value;
                }
            }
            
            return null;
        }
    }
}