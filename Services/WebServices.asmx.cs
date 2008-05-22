using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Engage.Dnn.Locator.Data;
using Engage.Dnn.Locator.Maps;
using Engage.Dnn.Locator.Providers.MapProviders;

namespace Engage.Dnn.Locator.Services
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class WebServices : System.Web.Services.WebService
    {

        [WebMethod]
        public DataTable GetLocationsByZip(string mapProviderTypeName, string apiKey, int radius, string postalCode, int locationTypeId, int portalId)
        {
            int[] locationTypeIds = { locationTypeId };

            return SearchLocationsByZip(mapProviderTypeName, apiKey, radius, postalCode, locationTypeIds, portalId);
        }

        public DataTable GetLocationsByZip(string mapProviderTypeName, string apiKey, int radius, string postalCode, int[] locationTypeIds, int portalId)
        {
            return SearchLocationsByZip(mapProviderTypeName, apiKey, radius, postalCode, locationTypeIds, portalId);
        }

        private DataTable SearchLocationsByZip(string mapProviderTypeName, string apiKey, int radius, string postalCode, int[] locationTypeIds, int portalId)
        {

            //use reflection to construct the provider class.
            MapProviderType providerType = (MapProviderType)MapProviderType.GetFromName(mapProviderTypeName, typeof(MapProviderType));
            //MapProvider provider = MapProvider.CreateInstance(providerType);

            DataTable matches = null;

            //figure out which method to call based on provider.
            if (providerType == MapProviderType.GoogleMaps)
            {
                GoogleGeocodeResult result = SearchUtility.SearchGoogle(postalCode, apiKey);
                matches = Data.DataProvider.Instance().GetClosestLocationsByRadius(result.latitude, result.longitude, radius, portalId, locationTypeIds);
            }
            else
            {
                YahooGeocodeResult result = SearchUtility.SearchYahoo("", "", "", "", postalCode, apiKey);
                matches = Data.DataProvider.Instance().GetClosestLocationsByRadius(result.latitude, result.longitude, radius, portalId, locationTypeIds);
            }

            return matches;
        }

        [WebMethod]
        public DataTable GetLocation(int locationId)
        {
            return Data.DataProvider.Instance().GetLocation(locationId);
        }

        [WebMethod]
        public DataTable GetLocationTypes()
        {
            return Data.DataProvider.Instance().GetLocationTypes();
        }

        [WebMethod]
        public DataTable GetLocations(int locationTypeId)
        {
            return Data.DataProvider.Instance().GetLocationType(locationTypeId);
        }
    }
}
