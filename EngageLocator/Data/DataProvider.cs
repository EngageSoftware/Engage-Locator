//Engage: Locator - http://www.engagemodules.com
//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace Engage.Dnn.Locator.Data
{
    public abstract class DataProvider
    {
        #region Shared/Static Methods
        // singleton reference to the instantiated object 
        //private static DataProvider provider = ((DataProvider)DotNetNuke.Framework.Reflection.CreateObject("data", "Engage.Dnn.Publish.Data", ""));

        private static DataProvider provider;

        // return the provider
        public static DataProvider Instance()
        {
            if (provider == null)
            {
                string assembly = "Engage.Dnn.Locator.Data.SqlDataprovider,EngageLocator";
                Type objectType = Type.GetType(assembly, true, true);

                provider = (DataProvider)Activator.CreateInstance(objectType);
                DataCache.SetCache(objectType.FullName, provider);
            }

            return provider;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not returning class state information")]
        public static IDbConnection GetConnection()
        {
            const string providerType = "data";
            ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);

            Provider objProvider = ((Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);
            string _connectionString;
            if (!String.IsNullOrEmpty(objProvider.Attributes["connectionStringName"]) && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]]))
            {
                _connectionString = System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]];
            }
            else
            {
                _connectionString = objProvider.Attributes["connectionString"];
            }

            IDbConnection newConnection = new System.Data.SqlClient.SqlConnection();
            newConnection.ConnectionString = _connectionString;
            newConnection.Open();
            return newConnection;
        }

        #endregion

        public abstract DataTable GetNClosestLocations(double latitude, double longitude, int N, int portalId);

        public abstract DataTable GetClosestLocationsByRadius(double latitude, double longitude, int radius, int portalId, int[] locationTypeIds);
        public abstract DataTable GetLocation(int locationId);
        public abstract DataTable GetLocationsByCountry(int countryId, int portalId);
        public abstract DataTable GetAllLocations(int portalId, int approved);
        public abstract void DeleteLocation(int locationId);
        public abstract string CopyData();
        public abstract void ClearLocations(int portalId);
        public abstract void ClearTempLocations();
        public abstract void InsertFileInfo(int fileId, int userId, int tabModuleId, int portalId, DateTime uploadDate, int succeeded, int processed);
        public abstract DataTable GetEmailByFileId(int fileId);
        public abstract int GetLastImportIndex();
        public abstract DataTable GetLatitudeLongitude(string address, string city);
        public abstract DataTable GetLocationTypes();
        public abstract int GetLocationTypeCount(string type);
        public abstract void UpdateLocationType(int locationTypeId, string locationTypeName);
        public abstract int InsertLocationType(string locationTypeName);
        public abstract void DeleteLocationType(int locationTypeId);
        public abstract int SaveLocation(Location loc);
        public abstract int SaveTempLocation(Location loc, bool successful);
        public abstract int UpdateLocation(Location loc);
        public abstract DataTable GetLocationTypeName(int id);
        public abstract DataTable GetCountriesList(int portalId);
        public abstract int GetTabModuleIdByFileId(int fileId);
        public abstract DataTable GetFilesToImport();
        public abstract void UpdateImportedLocationRow(int fileId);
        public abstract DataTable GetAllLocationsByDistance(double latitude, double longitude, int portalId, int[] locationTypeIds);
        public abstract DataTable GetAllLocationsByType(int portalId, string[] types);
        public abstract DataTable GetImportedLocationStatistics(int portalId);
        public abstract DataTable GetEngageLocatorTabModules(int portalId);
        public abstract void InsertLocationComment(int locationId, string comment, string submittteBy, bool approved);
        public abstract void UpdateLocationComment(int locationId, string comment, string submittedBy, bool approved, int userId);
        public abstract DataSet GetLocationComments(int locationId, bool approved);
        public abstract DataTable GetNewSubmittedComments(int portalId, bool approved);
        public abstract LocationComment GetLocationComment(int commentId);
        public abstract void SaveLocationComment(LocationComment comment);
        public abstract void DeleteLocatinComment(int commentId);
    }
}
