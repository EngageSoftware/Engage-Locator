// <copyright file="WebServices.asmx.cs" company="Engage Software">
// Engage: Locator - http://www.engagemodules.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Locator.Services
{
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
        public LocationCollection GetLocationsByZip(string mapProviderTypeName, string apiKey, int radius, string postalCode, int locationTypeId, int portalId)
        {
            int[] locationTypeIds = { locationTypeId };

            return SearchLocationsByZip(mapProviderTypeName, apiKey, radius, postalCode, locationTypeIds, portalId);
        }

        public LocationCollection GetLocationsByZip(string mapProviderTypeName, string apiKey, int radius, string postalCode, int[] locationTypeIds, int portalId)
        {
            return SearchLocationsByZip(mapProviderTypeName, apiKey, radius, postalCode, locationTypeIds, portalId);
        }

        private static LocationCollection SearchLocationsByZip(string mapProviderTypeName, string apiKey, int radius, string postalCode, int[] locationTypeIds, int portalId)
        {
            // use reflection to construct the provider class.
            MapProviderType providerType = MapProviderType.GetFromName(mapProviderTypeName, typeof(MapProviderType));
            //// MapProvider provider = MapProvider.CreateInstance(providerType);

            LocationCollection matches;

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
