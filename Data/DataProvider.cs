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
using System.Diagnostics;
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
        public const string ModuleQualifier = "EngageLocator_";

        // return the provider
    [DebuggerStepThroughAttribute]
        public static DataProvider Instance()
        {
            if (provider == null)
            {
                const string assembly = "Engage.Dnn.Locator.Data.SqlDataprovider,EngageLocator";
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
        public abstract DataTable GetAllLocations(int portalId, bool approved, string sortColumn, int index, int pageSize);
        public abstract void DeleteLocation(int locationId);
        public abstract string CopyData();
        public abstract void ClearLocations(int portalId);
        public abstract void ClearTempLocations();
        public abstract void InsertFileInfo(int fileId, int userId, int tabModuleId, int portalId, DateTime uploadDate, bool succeeded, bool processed);
        public abstract DataTable GetEmailByFileId(int fileId);
        public abstract int GetLastImportIndex();
        public abstract DataTable GetLatitudeLongitude(string address, string city);
        public abstract DataTable GetLocationTypes();
        public abstract DataTable GetLocations(int typeId, int portalId);
        public abstract int GetLocationTypeCount(string type);
        public abstract void UpdateLocationType(int locationTypeId, string locationTypeName);
        public abstract int InsertLocationType(string locationTypeName);
        public abstract void DeleteLocationType(int locationTypeId);
        public abstract int SaveLocation(Location loc);
        public abstract int SaveTempLocation(Location loc, bool successful);
        public abstract int UpdateLocation(Location loc);
        public abstract DataTable GetLocationTypeName(int id);
        public abstract DataTable GetLocationType(int id);
        public abstract DataTable GetCountriesList(int portalId);
        public abstract int GetTabModuleIdByFileId(int fileId);
        public abstract DataTable GetFilesToImport();
        public abstract void UpdateImportedLocationRow(int fileId);
        public abstract DataTable GetAllLocationsByDistance(double latitude, double longitude, int portalId, int[] locationTypeIds);
        public abstract DataTable GetAllLocationsByType(int portalId, string[] types);
        public abstract DataTable GetImportedLocationStatistics(int portalId);
        public abstract DataTable GetEngageLocatorTabModules(int portalId);
        public abstract void InsertComment(int locationId, string comment, string submittteBy, bool approved);
        public abstract void UpdateComment(int locationId, string comment, string submittedBy, bool approved, int userId);
        public abstract DataSet GetComments(int locationId, bool approved);
        public abstract DataTable GetNewSubmittedComments(int portalId, bool approved);
        public abstract Comment GetComment(int commentId);
        public abstract void SaveComment(Comment comment);
        public abstract void DeleteComment(int commentId);

        public abstract DataTable GetAttributeDefinitions(int locationTypeId);
        public abstract DataTable GetAttributeValues(int locationId);
        public abstract void AddAttribute(int attributeDefinitionId, int locationId, string attributeValue);
        public abstract void UpdateAttribute(int attributeDefinitionId, int locationId, string attributeValue);

        #region Custom Attributes Methods

        public abstract IDataReader GetAttributeDefinitionsById(int locationTypeId);
        public abstract int AddAttributeDefinition(int portalId, int LocationTypeId, int dataType, string defaultValue, string attributeName, bool required, string validationExpression, int viewOrder, bool visible, int length);
        public abstract void DeleteAttributeDefinition(int definitionId);
        public abstract IDataReader GetAttributeDefinition(int definitionId);
        public abstract IDataReader GetAttributeDefinitionByName(int locationTypeId,  string name);
        public abstract int UpdateAttributeDefinition(int definitionId, int dataType, string defaultValue, string attributeName, bool required, string validationExpression, int viewOrder, bool visible, int length);

        #endregion

    }
}
