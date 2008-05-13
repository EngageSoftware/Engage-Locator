using System;
using System.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace Engage.Dnn.Locator.DataTemp
{
    public abstract class AttributesDataProvider
    {
        #region Shared/Static Methods
        // singleton reference to the instantiated object 
        //private static DataProvider provider = ((DataProvider)DotNetNuke.Framework.Reflection.CreateObject("data", "Engage.Dnn.Publish.Data", ""));

        private static AttributesDataProvider provider;

        // return the provider
        public static AttributesDataProvider Instance()
        {
            if (provider == null)
            {
                string assembly = "Engage.Dnn.Locator.DataTemp.AttributesSqlDataProvider,EngageLocator";
                Type objectType = Type.GetType(assembly, true, true);

                provider = (AttributesDataProvider)Activator.CreateInstance(objectType);
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
        
        public abstract IDataReader GetAttributeDefinitionsByLTID(int ltid);
        public abstract int AddAttributeDefinition(int portalId, int LocationTypeId, int dataType, string defaultValue, string attributeName, bool required, string validationExpression, int viewOrder, bool visible, int length);
        public abstract void DeleteAttributeDefinition(int definitionId);
        public abstract IDataReader GetAttributeDefinition(int definitionId);
        public abstract IDataReader GetAttributeDefinitionByName(int portalId, string name);
        public abstract int UpdateAttributeDefinition(int definitionId, int dataType, string defaultValue, string attributeName, bool required, string validationExpression, int viewOrder, bool visible, int length);
    }
}
