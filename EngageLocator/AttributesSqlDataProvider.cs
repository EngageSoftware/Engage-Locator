using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DotNetNuke.Framework.Providers;
using DotNetNuke.Common.Utilities;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

namespace Engage.Dnn.Locator.DataTemp
{
    public class AttributesSqlDataProvider : AttributesDataProvider
    {
        #region Constants
        private const string providerType = "data";

        #endregion

        #region Private Members

        private readonly ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);
        private readonly string connectionString;
        private readonly string providerPath;
        private readonly string objectQualifier;
        private readonly string databaseOwner;
        private const string moduleQualifier = "EngageLocator_";

        #endregion

        #region Constructors
        public AttributesSqlDataProvider()
        {
            Provider provider = ((Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider]);

            connectionString = Config.GetConnectionString();

            if (String.IsNullOrEmpty(connectionString))
            {
                connectionString = provider.Attributes["connectionString"];
            }

            providerPath = provider.Attributes["providerPath"];

            objectQualifier = provider.Attributes["objectQualifier"];
            if (!String.IsNullOrEmpty(objectQualifier) & objectQualifier.EndsWith("_") == false)
            {
                objectQualifier += "_";
            }

            databaseOwner = provider.Attributes["databaseOwner"];
            if (!String.IsNullOrEmpty(databaseOwner) & databaseOwner.EndsWith(".") == false)
            {
                databaseOwner += ".";
            }
        }
        #endregion

        #region Properties
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }

        public string ProviderPath
        {
            get
            {
                return providerPath;
            }
        }

        public string ObjectQualifier
        {
            get
            {
                return objectQualifier;
            }
        }

        public string DatabaseOwner
        {
            get
            {
                return databaseOwner;
            }
        }

        public string NamePrefix
        {
            get
            {
                return databaseOwner + objectQualifier + moduleQualifier;
            }
        }
        #endregion

        public override IDataReader GetAttributeDefinitionsByLTID(int ltid)
        {
            return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetAttributeDefinitionsByLTID", new SqlParameter("@LocationTypeId", ltid));
        }

        public override int AddAttributeDefinition(int portalId, int LocationTypeId, int dataType, string defaultValue, string attributeName, bool required, string validationExpression, int viewOrder, bool visible, int length)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, NamePrefix + "spAddAttributeDefinition", new SqlParameter("@PortalId", portalId), new SqlParameter("@LocationTypeId", LocationTypeId), new SqlParameter("@DataType", dataType), new SqlParameter("@DefaultValue", defaultValue), new SqlParameter("@AttributeName", attributeName), new SqlParameter("@Required", required), new SqlParameter("@ValidationExpression", validationExpression), new SqlParameter("@ViewOrder", viewOrder), new SqlParameter("@Visible", visible), new SqlParameter("@Length", length));
        } 

        public override void DeleteAttributeDefinition(int attributeDefinitionId)
        {
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, NamePrefix + "spDeleteAttributeDefinition", new SqlParameter("@AttributeDefinitionId", attributeDefinitionId));
        }

        public override IDataReader GetAttributeDefinition(int attributeDefinitionId)
        {
            return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetAttributeDefinition", new SqlParameter("@AttributeDefinitionId", attributeDefinitionId));
        }

        public override IDataReader GetAttributeDefinitionByName(int portalId, string name)
        {
            return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetAttributeDefinitionByName", new SqlParameter("@PortalID", portalId), new SqlParameter("@Name", name));
        }

        public override int UpdateAttributeDefinition(int attributeDefinitionId, int dataType, string defaultValue, string attributeName, bool required, string validationExpression, int viewOrder, bool visible, int length)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, NamePrefix + "spUpdateAttributeDefinition", new SqlParameter("@AttributeDefinitionId", attributeDefinitionId), new SqlParameter("@DataType", dataType), new SqlParameter("@DefaultValue", defaultValue), new SqlParameter("@AttributeName", attributeName), new SqlParameter("@Required", required), new SqlParameter("@ValidationExpression", validationExpression), new SqlParameter("@ViewOrder", viewOrder), new SqlParameter("@Visible", visible), new SqlParameter("@Length", length));
        }
    }
}
