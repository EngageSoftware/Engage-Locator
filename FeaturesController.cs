using System;
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace Engage.Dnn.Locator
{
    public class FeaturesController: ISearchable

    {
        public SearchItemInfoCollection GetSearchItems(ModuleInfo ModInfo)
        {
            SearchItemInfoCollection searchItemCollection = new SearchItemInfoCollection();
            List<Location> colLocations = Location.GetSearchLocations(ModInfo.PortalID, 1);
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
