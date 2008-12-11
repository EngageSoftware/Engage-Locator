// <copyright file="MapProvider.cs" company="Engage Software">
// Copyright (c) 2004-2007
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
    using System.Collections.Generic;
    using System.Web.UI;

    public abstract class MapProvider
    {
        public static MapProvider CreateInstance(MapProviderType mapType)
        {
            return CreateInstance(mapType.ClassName);
        }

        public static MapProvider CreateInstance(string className)
        {
            Type objectType = Type.GetType(className, true, true);
            MapProvider mp = (MapProvider)Activator.CreateInstance(objectType);
            return mp;
        }

        public void RegisterMapScript(ScriptManager scriptManager, string apiKey, MapType mapType, string mapSectionId, string currentLocationSpanId, string noLocationSpanId, string instructionSpanId, string directionsLinkId, string directionsSpanId, List<Location> locations, bool showAllLocationsOnLoad)
        {
            this.GenerateMapScriptCore(scriptManager, apiKey, mapType, mapSectionId, currentLocationSpanId, noLocationSpanId, instructionSpanId, directionsLinkId, directionsSpanId, locations, showAllLocationsOnLoad);
        }

        public abstract void GenerateMapScriptCore(ScriptManager scriptManager, string apiKey, MapType mapType, string mapSectionId, string currentLocationSpanId, string noLocationSpanId, string instructionSpanId, string directionsLinkId, string directionsSectionId, List<Location> locations, bool showAllLocationsOnLoad);

        public string GenerateMiniMapScript()
        {
            return this.GenerateMiniMapScriptCore();
        }

        public abstract string GenerateMiniMapScriptCore();

        public abstract bool IsKeyValid(string apiKey);

        /// <summary>
        /// Returns the JavaScript to select the element with the given ID.
        /// </summary>
        /// <param name="elementId">The ID of the element to get.</param>
        /// <returns>The JavaScript to select the element with the given ID</returns>
        protected static string GetElementJavaScript(string elementId)
        {
            return "jQuery('#" + elementId + "')";
        }
    }

    /// <summary>
    /// The type of map to display
    /// </summary>
    public enum MapType
    {
        /// <summary>
        /// A normal map
        /// </summary>
        Normal,

        /// <summary>
        /// A hybrid map
        /// </summary>
        Hybrid,

        /// <summary>
        /// A satellite map
        /// </summary>
        Satellite
    }
}