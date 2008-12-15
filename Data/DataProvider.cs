// <copyright file="DataProvider.cs" company="Engage Software">
// Engage: Locator - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Locator.Data
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Framework;
    using DotNetNuke.Framework.Providers;

    /// <summary>
    /// An abstract representation of the data access for Engage: Locator
    /// </summary>
    public abstract class DataProvider
    {
        /// <summary>
        /// The prefix for all database objects in this module
        /// </summary>
        public const string ModuleQualifier = "EngageLocator_";

        /// <summary>
        /// The type of DNN provider that this provider is.
        /// </summary>
        protected const string ProviderType = "data";

        /// <summary>
        /// Backing field for <see cref="Instance"/>
        /// </summary>
        private static readonly DataProvider instance = ((DataProvider)Reflection.CreateObject("data", "Engage.Dnn.Locator.Data", string.Empty));

        /// <summary>
        /// Backing field for <see cref="ConnectionString"/>
        /// </summary>
        private static readonly string connectionString = GetConnectionString();

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        protected static string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }

        /// <summary>
        /// Gets a concrete instance of this provider.
        /// </summary>
        /// <returns>A concrete instance of <see cref="DataProvider"/></returns>
        [DebuggerStepThroughAttribute]
        public static DataProvider Instance()
        {
            return instance;
        }

        /// <summary>
        /// Gets the configured connection string for this site's data provider.
        /// </summary>
        /// <returns>The connection string for this site's data provider</returns>
        private static string GetConnectionString()
        {
            ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
            Provider objProvider = ((Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider]);

            string configuredConnectionString = Config.GetConnectionString();
            return !string.IsNullOrEmpty(configuredConnectionString) ? configuredConnectionString : objProvider.Attributes["connectionString"];
        }

        public abstract DataTable GetNClosestLocations(double latitude, double longitude, int count, int portalId);

        public abstract IDataReader GetClosestLocationsByRadius(double latitude, double longitude, int radius, int portalId, int[] locationTypeIds);
        public abstract IDataReader GetLocation(int locationId);
        public abstract IDataReader GetLocationsByCountry(int countryId, int portalId);
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
        public abstract IDataReader GetAllLocationsByDistance(double latitude, double longitude, int portalId, int[] locationTypeIds);
        public abstract IDataReader GetAllLocationsByType(int portalId, string[] types);
        public abstract DataTable GetImportedLocationStatistics(int portalId);
        public abstract DataTable GetEngageLocatorTabModules(int portalId);
        public abstract void InsertComment(int locationId, string comment, string submittedBy, bool approved);
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

        public abstract IDataReader GetAttributeDefinitionsById(int locationTypeId);
        public abstract int AddAttributeDefinition(int portalId, int locationTypeId, int dataType, string defaultValue, string attributeName, bool required, string validationExpression, int viewOrder, bool visible, int length);
        public abstract void DeleteAttributeDefinition(int definitionId);
        public abstract IDataReader GetAttributeDefinition(int definitionId);
        public abstract IDataReader GetAttributeDefinitionByName(int locationTypeId,  string name);
        public abstract int UpdateAttributeDefinition(int definitionId, int dataType, string defaultValue, string attributeName, bool required, string validationExpression, int viewOrder, bool visible, int length);
    }
}
