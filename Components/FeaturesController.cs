// <copyright file="FeaturesController.cs" company="Engage Software">
// Engage: Locator - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Locator.Components
{
    using System;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Search;

    /// <summary>
    /// Controls which DNN features are available for this module.
    /// </summary>
    public class FeaturesController : ISearchable
    {
        /// <summary>
        /// Gets a list of search items to populate the DNN search index for the given module instance
        /// </summary>
        /// <param name="ModInfo">The mod info.</param>
        /// <returns>A list of search result information for the DNN search index</returns>
        public SearchItemInfoCollection GetSearchItems(ModuleInfo ModInfo)
        {
            SearchItemInfoCollection searchItemCollection = new SearchItemInfoCollection();
            LocationCollection colLocations = Location.GetLocations(ModInfo.PortalID, true);
            foreach (Location location in colLocations)
            {
                searchItemCollection.Add(new SearchItemInfo(
                     ModInfo.ModuleTitle, 
                     location.Name, 
                     1, 
                     DateTime.Now, 
                     ModInfo.ModuleID,
                     location.Address + " " + location.City + " " + location.RegionName + " " + location.PostalCode,
                     location.Address + " " + location.City + " " + location.RegionName + " " + location.PostalCode + " " + location.LocationDetails));
            }

            return searchItemCollection;
        }
    }
}
