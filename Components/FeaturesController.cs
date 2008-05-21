//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace Engage.Dnn.Locator.Components
{
    public class FeaturesController: ISearchable
    {
        public SearchItemInfoCollection GetSearchItems(ModuleInfo ModInfo)
        {
            SearchItemInfoCollection searchItemCollection = new SearchItemInfoCollection();
            List<Location> colLocations = Location.GetSearchLocations(ModInfo.PortalID, true);
            foreach (Location location in colLocations)
            {
                SearchItemInfo searchItem =
                    new SearchItemInfo(ModInfo.ModuleTitle, location.Name, 1, DateTime.Now, ModInfo.ModuleID,
                        location.Address + " " + location.City + " " + location.StateName + " " + location.PostalCode,
                        location.Address + " " + location.City + " " + location.StateName + " " + location.PostalCode + " " + location.LocationDetails);
                searchItemCollection.Add(searchItem);
            }
            return searchItemCollection;
        }
    }
}
