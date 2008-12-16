namespace Engage.Dnn.Locator.Services
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Web.Services;
    using Maps;

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class WebServices : WebService
    {
        [WebMethod]
        public List<Location> GetLocationsByZip(string mapProviderTypeName, string apiKey, int radius, string postalCode, int locationTypeId, int portalId)
        {
            int[] locationTypeIds = { locationTypeId };

            return SearchLocationsByZip(mapProviderTypeName, apiKey, radius, postalCode, locationTypeIds, portalId);
        }

        public List<Location> GetLocationsByZip(string mapProviderTypeName, string apiKey, int radius, string postalCode, int[] locationTypeIds, int portalId)
        {
            return SearchLocationsByZip(mapProviderTypeName, apiKey, radius, postalCode, locationTypeIds, portalId);
        }

        private static List<Location> SearchLocationsByZip(string mapProviderTypeName, string apiKey, int radius, string postalCode, int[] locationTypeIds, int portalId)
        {
            // use reflection to construct the provider class.
            MapProviderType providerType = MapProviderType.GetFromName(mapProviderTypeName, typeof(MapProviderType));
            //// MapProvider provider = MapProvider.CreateInstance(providerType);

            List<Location> matches;

            // figure out which method to call based on provider.
            if (providerType == MapProviderType.GoogleMaps)
            {
                GoogleGeocodeResult result = SearchUtility.SearchGoogle(postalCode, apiKey);
                matches = Location.GetAllLocationsByDistance(result.latitude, result.longitude, radius, portalId, locationTypeIds, null, null);
            }
            else
            {
                YahooGeocodeResult result = SearchUtility.SearchYahoo(string.Empty, string.Empty, string.Empty, string.Empty, postalCode, apiKey);
                matches = Location.GetAllLocationsByDistance(result.Latitude, result.Longitude, radius, portalId, locationTypeIds, null, null);
            }

            return matches;
        }

        [WebMethod]
        public Location GetLocation(int locationId)
        {
            return Location.GetLocation(locationId);
        }

        [WebMethod]
        public DataTable GetLocationTypes()
        {
            return Data.DataProvider.Instance().GetLocationTypes();
        }

        [WebMethod]
        public DataTable GetLocations(int locationTypeId, int portalId)
        {
            return Data.DataProvider.Instance().GetLocations(locationTypeId, portalId);
        }
    }
}
