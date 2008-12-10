//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Locator.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Framework.Providers;

    using Microsoft.ApplicationBlocks.Data;

    public class SqlDataProvider : DataProvider
    {
        public const string CacheKey = "AttributeDefinitions{0}";

        public const int CacheTimeOut = 20;

        private const string providerType = "data";

        private readonly string connectionString;

        private readonly string databaseOwner;

        private readonly string objectQualifier;

        private readonly ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);

        private readonly string providerPath;

        public SqlDataProvider()
        {
            Provider provider = ((Provider)this.providerConfiguration.Providers[this.providerConfiguration.DefaultProvider]);

            this.connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(this.connectionString))
            {
                this.connectionString = provider.Attributes["connectionString"];
            }

            this.providerPath = provider.Attributes["providerPath"];

            this.objectQualifier = provider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(this.objectQualifier) && !this.objectQualifier.EndsWith("_", StringComparison.Ordinal))
            {
                this.objectQualifier += "_";
            }

            this.databaseOwner = provider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(this.databaseOwner) && !this.databaseOwner.EndsWith(".", StringComparison.Ordinal))
            {
                this.databaseOwner += ".";
            }
        }

        public string ConnectionString
        {
            get { return this.connectionString; }
        }

        public string DatabaseOwner
        {
            get { return this.databaseOwner; }
        }

        public string NamePrefix
        {
            get { return this.databaseOwner + this.objectQualifier + ModuleQualifier; }
        }

        public string ObjectQualifier
        {
            get { return this.objectQualifier; }
        }

        public string ProviderPath
        {
            get { return this.providerPath; }
        }

        public override void AddAttribute(int attributeDefinitionId, int locationId, string attributeValue)
        {
            SqlHelper.ExecuteNonQuery(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spAddLocationAttribute",
                    Engage.Utility.CreateIntegerParam("@attributeDefinitionId", attributeDefinitionId),
                    Engage.Utility.CreateIntegerParam("@locationId", locationId),
                    Engage.Utility.CreateTextParam("@attributeValue", attributeValue));
        }

        public override int AddAttributeDefinition(
                int portalId,
                int LocationTypeId,
                int dataType,
                string defaultValue,
                string attributeName,
                bool required,
                string validationExpression,
                int viewOrder,
                bool visible,
                int length)
        {
            return SqlHelper.ExecuteNonQuery(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spAddAttributeDefinition",
                    new SqlParameter("@PortalId", portalId),
                    new SqlParameter("@LocationTypeId", LocationTypeId),
                    new SqlParameter("@DataType", dataType),
                    new SqlParameter("@DefaultValue", defaultValue),
                    new SqlParameter("@AttributeName", attributeName),
                    new SqlParameter("@Required", required),
                    new SqlParameter("@ValidationExpression", validationExpression),
                    new SqlParameter("@ViewOrder", viewOrder),
                    new SqlParameter("@Visible", visible),
                    new SqlParameter("@Length", length));
        }

        public override void ClearLocations(int portalId)
        {
            StringBuilder sql = new StringBuilder(250);
            sql.AppendFormat(CultureInfo.InvariantCulture, "EXEC {0}spDeleteLocations ", this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, "@PortalId");

            SqlHelper.ExecuteNonQuery(
                    this.ConnectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@PortalId", portalId));
        }

        public override void ClearTempLocations()
        {
            StringBuilder sql = new StringBuilder(250);
            sql.AppendFormat(CultureInfo.InvariantCulture, "Truncate table {0}TempLocation ", this.NamePrefix);

            SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql.ToString());
        }

        public override string CopyData()
        {
            try
            {
                StringBuilder sql = new StringBuilder(500);
                sql.AppendFormat(CultureInfo.InvariantCulture, "EXEC {0}spCopyLocation", this.NamePrefix);
                SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql.ToString());
                return "Data Import Successful";
            }
            catch
            {
                return "Critical Error: Staged Data not Copied";
            }
        }

        public override void DeleteAttributeDefinition(int attributeDefinitionId)
        {
            SqlHelper.ExecuteNonQuery(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spDeleteAttributeDefinition",
                    new SqlParameter("@AttributeDefinitionId", attributeDefinitionId));
        }

        public override void DeleteComment(int commentId)
        {
            SqlHelper.ExecuteNonQuery(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spDeleteComment",
                    Engage.Utility.CreateIntegerParam("@commentId", commentId));
        }

        public override void DeleteLocation(int locationId)
        {
            StringBuilder sql = new StringBuilder(98);
            sql.AppendFormat(CultureInfo.InvariantCulture, "Delete from {0}Location where LocationId = @locationId", this.NamePrefix);

            SqlHelper.ExecuteNonQuery(
                    this.connectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@locationId", locationId));
        }

        public override void DeleteLocationType(int locationTypeId)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, "Delete from {0}LocationType where LocationTypeId = @locationTypeId", this.NamePrefix);

            SqlHelper.ExecuteNonQuery(
                    this.ConnectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@locationTypeId", locationTypeId));
        }

        public override DataTable GetAllLocations(int portalId, bool approved, string sortColumn, int index, int pageSize)
        {
            return
                    SqlHelper.ExecuteDataset(
                            this.connectionString,
                            CommandType.StoredProcedure,
                            this.NamePrefix + "spGetAllLocations",
                            Engage.Utility.CreateIntegerParam("@PortalId", portalId),
                            Engage.Utility.CreateBitParam("@approved", approved),
                            Engage.Utility.CreateVarcharParam("@sortColumn", sortColumn, 200),
                            Engage.Utility.CreateIntegerParam("@index", index),
                            Engage.Utility.CreateIntegerParam("@pagesize", pageSize)).Tables[0];
        }

        public override IDataReader GetAllLocationsByDistance(double latitude, double longitude, int portalId, int[] locationTypeIds)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "SELECT LocationId, LocationTypeId, ExternalIdentifier, Name, Website, Latitude, Longitude, Abbreviation, CountryName, City, Address, Address2, RegionId, StateName, Phone, LocationDetails, [Type], PostalCode, Approved, AverageRating, {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) AS Distance ",
                    this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " FROM {0}vLocations ", this.NamePrefix);
            ////sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) <= @Radius ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE PortalId = @PortalId ");
            ////sql.AppendFormat(CultureInfo.InvariantCulture, " AND IsSearchable = 1 ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND Latitude IS NOT NULL ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND Longitude IS NOT NULL ");
            int i = 0;

            if (locationTypeIds != null)
            {
                foreach (int id in locationTypeIds)
                {
                    if (i == 0)
                    {
                        sql.AppendFormat(
                                CultureInfo.InvariantCulture, " AND ( Type = '" + LocationType.GetLocationTypeName(id).Replace("'", "''") + "'");
                    }
                    else
                    {
                        sql.AppendFormat(CultureInfo.InvariantCulture, " OR Type = '" + LocationType.GetLocationTypeName(id).Replace("'", "''") + "'");
                    }

                    i++;
                }

                sql.AppendFormat(CultureInfo.InvariantCulture, ")");
            }

            sql.AppendFormat(
                    CultureInfo.InvariantCulture, " ORDER BY {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) ", this.NamePrefix);

            return SqlHelper.ExecuteReader(
                    this.connectionString,
                    CommandType.Text,
                    sql.ToString(),
                    Engage.Utility.CreateDoubleParam("@Latitude", latitude),
                    Engage.Utility.CreateDoubleParam("@Longitude", longitude),
                    Engage.Utility.CreateIntegerParam("@PortalId", portalId));
        }

        public override IDataReader GetAllLocationsByType(int portalId, string[] types)
        {
            StringBuilder sql = new StringBuilder(98);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "SELECT LocationId, vl.LocationTypeId, ExternalIdentifier, Name, WebSite, Abbreviation, StateName, CountryName as Country, CountryId, RegionId, City, Address, Address2, Latitude, Longitude, Phone, LocationDetails, Approved, AverageRating, lt.LocationTypeName, PostalCode ");
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    " FROM {0}vLocations vl join {0}LocationType lt on (vl.LocationTypeId = lt.LocationTypeId) ",
                    this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE vl.PortalId = @PortalId and Approved = 1 ");

            int i = 0;
            if (types[0] != String.Empty)
            {
                foreach (string s in types)
                {
                    if (i == 0)
                    {
                        sql.AppendFormat(
                                CultureInfo.InvariantCulture,
                                " AND ( Type = '"
                                + LocationType.GetLocationTypeName(Convert.ToInt32(s, CultureInfo.InvariantCulture)).Replace("'", "''") + "'");
                    }
                    else
                    {
                        sql.AppendFormat(
                                CultureInfo.InvariantCulture,
                                " OR Type = '" + LocationType.GetLocationTypeName(Convert.ToInt32(s, CultureInfo.InvariantCulture)).Replace("'", "''")
                                + "'");
                    }

                    i++;
                }

                sql.AppendFormat(CultureInfo.InvariantCulture, ")");
            }

            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY LocationId ");

            return SqlHelper.ExecuteReader(
                    this.connectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@PortalId", portalId));
        }

        public override IDataReader GetAttributeDefinition(int attributeDefinitionId)
        {
            return SqlHelper.ExecuteReader(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spGetAttributeDefinition",
                    new SqlParameter("@AttributeDefinitionId", attributeDefinitionId));
        }

        public override IDataReader GetAttributeDefinitionByName(int locationTypeId, string name)
        {
            return SqlHelper.ExecuteReader(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spGetAttributeDefinitionByName",
                    new SqlParameter("@locationTypeId", locationTypeId),
                    new SqlParameter("@Name", name));
        }

        public override DataTable GetAttributeDefinitions(int locationTypeId)
        {
            return
                    SqlHelper.ExecuteDataset(
                            this.connectionString,
                            CommandType.StoredProcedure,
                            this.NamePrefix + "spGetLocationAttributeDefinitions",
                            Engage.Utility.CreateIntegerParam("@locationTypeId", locationTypeId)).Tables[0];
        }

        public override IDataReader GetAttributeDefinitionsById(int locationTypeId)
        {
            return SqlHelper.ExecuteReader(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spGetAttributeDefinitionsById",
                    new SqlParameter("@LocationTypeId", locationTypeId));
        }

        public override DataTable GetAttributeValues(int locationId)
        {
            return
                    SqlHelper.ExecuteDataset(
                            this.connectionString,
                            CommandType.StoredProcedure,
                            this.NamePrefix + "spGetLocationAttributeValues",
                            Engage.Utility.CreateIntegerParam("@locationId", locationId)).Tables[0];
        }

        /// <summary>
        /// Gets the closest locations within a given radius.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="radius">The radius in miles.</param>
        /// <param name="portalId">The portal id.</param>
        /// <param name="locationTypeIds">IDs of the types of locations available.</param>
        /// <returns>
        /// A list of location records as well as their distance from the given location.
        /// </returns>
        public override IDataReader GetClosestLocationsByRadius(double latitude, double longitude, int radius, int portalId, int[] locationTypeIds)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "SELECT LocationId, LocationTypeId, ExternalIdentifier, Name, StateName, Website, Latitude, Longitude, Abbreviation, CountryName, City, Address, Address2, Phone, RegionId, LocationDetails, [Type], PostalCode, Approved, AverageRating, {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) AS Distance ",
                    this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " FROM {0}vLocations ", this.NamePrefix);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    " WHERE {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) <= @Radius ",
                    this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND PortalId = @PortalId ");
            ////sql.AppendFormat(CultureInfo.InvariantCulture, " AND IsSearchable = 1 ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND Latitude IS NOT NULL ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND Longitude IS NOT NULL ");
            int i = 0;

            if (locationTypeIds != null)
            {
                foreach (int id in locationTypeIds)
                {
                    if (i == 0)
                    {
                        sql.AppendFormat(
                                CultureInfo.InvariantCulture, " AND ( Type = '" + LocationType.GetLocationTypeName(id).Replace("'", "''") + "'");
                    }
                    else
                    {
                        sql.AppendFormat(CultureInfo.InvariantCulture, " OR Type = '" + LocationType.GetLocationTypeName(id).Replace("'", "''") + "'");
                    }

                    i++;
                }

                sql.AppendFormat(CultureInfo.InvariantCulture, ")");
            }

            sql.AppendFormat(
                    CultureInfo.InvariantCulture, " ORDER BY {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) ", this.NamePrefix);

            return SqlHelper.ExecuteReader(
                    this.connectionString,
                    CommandType.Text,
                    sql.ToString(),
                    Engage.Utility.CreateDoubleParam("@Latitude", latitude),
                    Engage.Utility.CreateDoubleParam("@Longitude", longitude),
                    Engage.Utility.CreateIntegerParam("@Radius", radius),
                    Engage.Utility.CreateIntegerParam("@PortalId", portalId));
        }

        public override Comment GetComment(int commentId)
        {
            DataTable comment =
                    SqlHelper.ExecuteDataset(
                            this.connectionString,
                            CommandType.StoredProcedure,
                            this.NamePrefix + "spGetComment",
                            Engage.Utility.CreateIntegerParam("@commentId", commentId)).Tables[0];
            Comment c = new Comment();
            c.CommentId = Convert.ToInt32(comment.Rows[0]["CommentId"], CultureInfo.InvariantCulture);
            c.Text = comment.Rows[0]["Text"].ToString();
            c.Approved = Convert.ToBoolean(comment.Rows[0]["Approved"], CultureInfo.InvariantCulture);
            c.SubmittedBy = comment.Rows[0]["Author"].ToString();
            c.LocationName = comment.Rows[0]["Name"].ToString();

            return c;
        }

        public override DataSet GetComments(int locationId, bool approved)
        {
            return SqlHelper.ExecuteDataset(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spGetComments",
                    Engage.Utility.CreateIntegerParam("@locationId", locationId),
                    Engage.Utility.CreateIntegerParam("@approved", Convert.ToInt32(approved)));
        }

        public override DataTable GetCountriesList(int portalId)
        {
            StringBuilder sql = new StringBuilder(100);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture, "SELECT DISTINCT c.EntryId, c.[Text] FROM {0}Lists c ", this.DatabaseOwner + this.ObjectQualifier);
            sql.AppendFormat(CultureInfo.InvariantCulture, " JOIN {0}Location l ON (c.EntryId = l.CountryId) ", this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND l.PortalId = @PortalId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY c.[Text] ");

            return
                    SqlHelper.ExecuteDataset(
                            this.connectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@PortalId", portalId)).Tables[
                            0];
        }

        public override DataTable GetEmailByFileId(int fileId)
        {
            StringBuilder sql = new StringBuilder(500);

            sql.AppendFormat(CultureInfo.InvariantCulture, " Select U.FirstName, U.LastName, KF.UploadDate, U.Email ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " From {0}Files KF ", this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Join {0}Users u on U.UserID = KF.UserID ", this.DatabaseOwner + this.ObjectQualifier);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Where FileId = @FileId ");

            return
                    SqlHelper.ExecuteDataset(
                            this.connectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@FileId", fileId)).Tables[0];
        }

        public override DataTable GetEngageLocatorTabModules(int portalId)
        {
            return
                    SqlHelper.ExecuteDataset(
                            this.connectionString,
                            CommandType.StoredProcedure,
                            this.NamePrefix + "spGetEngageLocatorTabModules",
                            Engage.Utility.CreateIntegerParam("@portalId", portalId)).Tables[0];
        }

        public override DataTable GetFilesToImport()
        {
            return SqlHelper.ExecuteDataset(this.connectionString, CommandType.StoredProcedure, this.NamePrefix + "spGetFilesToImport").Tables[0];
        }

        public override DataTable GetImportedLocationStatistics(int portalId)
        {
            return
                    SqlHelper.ExecuteDataset(
                            this.connectionString,
                            CommandType.StoredProcedure,
                            this.NamePrefix + "spGetImportedLocationStatistics",
                            Engage.Utility.CreateIntegerParam("@portalId", portalId)).Tables[0];
        }

        public override int GetLastImportIndex()
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Select max(CSVLineNumber) ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " From {0}TempLocation", this.NamePrefix);

            object value = SqlHelper.ExecuteScalar(this.connectionString, CommandType.Text, sql.ToString());
            if (value == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        public override DataTable GetLatitudeLongitude(string address, string city)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    " Select Latitude, Longitude from {0}Location where Address = @address AND City = @city",
                    this.NamePrefix);

            return
                    SqlHelper.ExecuteDataset(
                            this.connectionString,
                            CommandType.Text,
                            sql.ToString(),
                            Engage.Utility.CreateVarcharParam("@address", address, 255),
                            Engage.Utility.CreateVarcharParam("@city", city, 255)).Tables[0];
        }

        public override IDataReader GetLocation(int locationId)
        {
            StringBuilder sql = new StringBuilder(98);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "SELECT LocationId, LocationTypeId, ExternalIdentifier, Name, Website, Latitude, Longitude, Abbreviation, CountryName, City, Address, Address2, Phone, StateName, RegionId, LocationDetails, [Type], PostalCode, Approved, AverageRating ");
            sql.AppendFormat(CultureInfo.InvariantCulture, "FROM {0}vLocations ", this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE LocationId = @LocationId ");

            return SqlHelper.ExecuteReader(
                    this.connectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@LocationId", locationId));
        }

        public override DataTable GetLocations(int typeId, int portalId)
        {
            StringBuilder sql = new StringBuilder(98);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "SELECT LocationId, LocationTypeId, ExternalIdentifier, Name, Website, Latitude, Longitude, Abbreviation, CountryName, City, Address, Address2, Phone, StateName, RegionId, LocationDetails, [Type], PostalCode, Approved, AverageRating ");
            sql.AppendFormat(CultureInfo.InvariantCulture, "FROM {0}vLocations ", this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE LocationTypeId = @LocationTypeId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND PortalId = @PortalId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY Name ");

            return
                    SqlHelper.ExecuteDataset(
                            this.connectionString,
                            CommandType.Text,
                            sql.ToString(),
                            Engage.Utility.CreateIntegerParam("@LocationTypeId", typeId),
                            Engage.Utility.CreateIntegerParam("@PortalId", portalId)).Tables[0];
        }

        public override IDataReader GetLocationsByCountry(int countryId, int portalId)
        {
            StringBuilder sql = new StringBuilder(98);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "SELECT LocationId, LocationTypeId, ExternalIdentifier, Name, Website, Latitude, Longitude, Abbreviation, CountryName, City, Address, Address2, Phone, RegionId, LocationDetails, [Type], PostalCode, Approved, AverageRating FROM {0}vLocations ",
                    this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE CountryId = @CountryId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND PortalId = @PortalId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY City ");

            return SqlHelper.ExecuteReader(
                    this.connectionString,
                    CommandType.Text,
                    sql.ToString(),
                    Engage.Utility.CreateIntegerParam("@CountryId", countryId),
                    Engage.Utility.CreateIntegerParam("@PortalId", portalId));
        }

        public override DataTable GetLocationType(int id)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@LocationTypeId";
            param.Value = id;

            return
                    SqlHelper.ExecuteDataset(this.connectionString, CommandType.StoredProcedure, this.NamePrefix + "spGetLocationType", param).Tables[
                            0];
        }

        public override int GetLocationTypeCount(string type)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@LocationType";
            param.Value = type;

            int count =
                    Convert.ToInt32(
                            SqlHelper.ExecuteScalar(
                                    this.connectionString, CommandType.StoredProcedure, this.NamePrefix + "spGetLocationTypeCount", param),
                            CultureInfo.InvariantCulture);
            return count;
        }

        public override DataTable GetLocationTypeName(int id)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@LocationTypeId";
            param.Value = id;

            return
                    SqlHelper.ExecuteDataset(this.connectionString, CommandType.StoredProcedure, this.NamePrefix + "spGetLocationType", param).Tables[
                            0];
        }

        public override DataTable GetLocationTypes()
        {
            StringBuilder sql = new StringBuilder(500);

            sql.AppendFormat(CultureInfo.InvariantCulture, " Select LocationTypeID, LocationTypeName ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " From {0}LocationType ", this.NamePrefix);

            return SqlHelper.ExecuteDataset(this.connectionString, CommandType.Text, sql.ToString()).Tables[0];
        }

        /// <summary>
        /// Gets the N closest locations.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="N">The number of locations to return.</param>
        /// <param name="portalId">The portal id.</param>
        /// <returns>
        /// A list of location records as well as their distance from the given location.
        /// </returns>
        public override DataTable GetNClosestLocations(double latitude, double longitude, int N, int portalId)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "SELECT TOP {1} LocationId, Id, LocationTypeId, Name, Latitude, Longitude, Abbreviation, CountryName, City, Address, Address2, PostalCode, Phone, LocationDetails, [Type], {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) AS Distance ",
                    this.NamePrefix,
                    N);
            sql.AppendFormat(CultureInfo.InvariantCulture, " FROM {0}vLocations ", this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE PortalId = @PortalId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND IsSearchable = 1 ");
            ////sql.AppendFormat(CultureInfo.InvariantCulture, " AND [Type] = 1 ");
            sql.AppendFormat(
                    CultureInfo.InvariantCulture, " ORDER BY {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude)", this.NamePrefix);

            return
                    SqlHelper.ExecuteDataset(
                            this.connectionString,
                            CommandType.Text,
                            sql.ToString(),
                            Engage.Utility.CreateDoubleParam("@Latitude", latitude),
                            Engage.Utility.CreateDoubleParam("@Longitude", longitude),
                            Engage.Utility.CreateIntegerParam("@PortalId", portalId)).Tables[0];
        }

        public override DataTable GetNewSubmittedComments(int portalId, bool approved)
        {
            DataSet comments = SqlHelper.ExecuteDataset(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spGetNewSubmittedComments",
                    Engage.Utility.CreateIntegerParam("@portalId", portalId),
                    Engage.Utility.CreateIntegerParam("@approved", Convert.ToInt32(approved)));
            return comments.Tables[0];
        }

        public override int GetTabModuleIdByFileId(int fileId)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Select TabModuleId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " From  {0}Files", this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Where FileId = @FileId ");

            return
                    Convert.ToInt32(
                            SqlHelper.ExecuteScalar(
                                    this.connectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@FileId", fileId)),
                            CultureInfo.InvariantCulture);
        }

        public override void InsertComment(int locationId, string text, string submittteBy, bool approved)
        {
            SqlHelper.ExecuteNonQuery(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spInsertComment",
                    Engage.Utility.CreateIntegerParam("@locationId", locationId),
                    Engage.Utility.CreateTextParam("@text", text),
                    Engage.Utility.CreateVarcharParam("@submittedBy", submittteBy, 50),
                    Engage.Utility.CreateIntegerParam("@approved", Convert.ToInt32(approved)));
        }

        public override void InsertFileInfo(
                int fileId, int userId, int tabModuleId, int portalId, DateTime uploadDate, bool succeeded, bool processed)
        {
            StringBuilder sql = new StringBuilder(500);

            SqlHelper.ExecuteScalar(
                    this.ConnectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spInsertFileInfo",
                    Engage.Utility.CreateIntegerParam("@FileId", fileId),
                    Engage.Utility.CreateIntegerParam("@UserId", userId),
                    Engage.Utility.CreateIntegerParam("@TabModuleId", tabModuleId),
                    Engage.Utility.CreateIntegerParam("@PortalId", portalId),
                    Engage.Utility.CreateDateTimeParam("@UploadDate", uploadDate),
                    Engage.Utility.CreateBitParam("@Succeeded", succeeded),
                    Engage.Utility.CreateBitParam("@Processed", processed));
        }

        public override int InsertLocationType(string locationTypeName)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture, "insert into {0}LocationType (LocationTypeName) values (@locationTypeName) ", this.NamePrefix);
            //sql.AppendFormat(CultureInfo.InvariantCulture, "SELECT @@IDENTITY");

            return
                    Convert.ToInt32(
                            SqlHelper.ExecuteNonQuery(
                                    this.ConnectionString,
                                    CommandType.Text,
                                    sql.ToString(),
                                    Engage.Utility.CreateVarcharParam("@locationTypeName", locationTypeName, 50)));
        }

        public override void SaveComment(Comment myComment)
        {
            SqlHelper.ExecuteNonQuery(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spSaveComment",
                    Engage.Utility.CreateIntegerParam("@commentId", myComment.CommentId),
                    Engage.Utility.CreateTextParam("@text", myComment.Text),
                    Engage.Utility.CreateVarcharParam("@submittedBy", myComment.SubmittedBy, 50),
                    Engage.Utility.CreateIntegerParam("@approved", Convert.ToInt32(myComment.Approved)));
        }

        public override int SaveLocation(Location loc)
        {
            return
                    Convert.ToInt32(
                            SqlHelper.ExecuteScalar(
                                    this.ConnectionString,
                                    CommandType.StoredProcedure,
                                    this.NamePrefix + "spInsertLocation",
                                    Engage.Utility.CreateVarcharParam("@ExternalIdentifier", loc.ExternalIdentifier, 100),
                                    Engage.Utility.CreateVarcharParam("@Name", loc.Name, 100),
                                    Engage.Utility.CreateVarcharParam("@Website", loc.Website, 255),
                                    Engage.Utility.CreateDoubleParam("@Latitude", loc.Latitude),
                                    Engage.Utility.CreateDoubleParam("@Longitude", loc.Longitude),
                                    Engage.Utility.CreateIntegerParam("@CountryId", loc.CountryId),
                                    Engage.Utility.CreateIntegerParam("@RegionId", loc.RegionId),
                                    Engage.Utility.CreateVarcharParam("@City", loc.City, 100),
                                    Engage.Utility.CreateVarcharParam("@Address", loc.Address, 510),
                                    Engage.Utility.CreateVarcharParam("@Address2", loc.Address2, 510),
                                    Engage.Utility.CreateVarcharParam("@PostalCode", loc.PostalCode, 100),
                                    Engage.Utility.CreateVarcharParam("@Phone", loc.Phone, 100),
                                    Engage.Utility.CreateTextParam("@LocationDetails", loc.LocationDetails),
                                    Engage.Utility.CreateIntegerParam("@LocationTypeId", loc.LocationTypeId),
                                    Engage.Utility.CreateIntegerParam("@CsvLineNumber", 0),
                                    Engage.Utility.CreateIntegerParam("@PortalId", loc.PortalId),
                                    Engage.Utility.CreateBitParam("@approved", loc.Approved)),
                            CultureInfo.InvariantCulture);
        }

        public override int SaveTempLocation(Location loc, bool successful)
        {
            return SqlHelper.ExecuteNonQuery(
                    this.ConnectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spInsertTempLocation",
                    Engage.Utility.CreateVarcharParam("@ExternalIdentifier", loc.ExternalIdentifier, 100),
                    Engage.Utility.CreateVarcharParam("@Name", loc.Name, 100),
                    Engage.Utility.CreateVarcharParam("@Website", loc.Website, 255),
                    Engage.Utility.CreateDoubleParam("@Latitude", loc.Latitude),
                    Engage.Utility.CreateDoubleParam("@Longitude", loc.Longitude),
                    Engage.Utility.CreateIntegerParam("@CountryId", loc.CountryId),
                    Engage.Utility.CreateIntegerParam("@RegionId", loc.RegionId),
                    Engage.Utility.CreateVarcharParam("@City", loc.City, 510),
                    Engage.Utility.CreateVarcharParam("@Address", loc.Address, 510),
                    Engage.Utility.CreateVarcharParam("@Address2", loc.Address2, 510),
                    Engage.Utility.CreateVarcharParam("@PostalCode", loc.PostalCode, 100),
                    Engage.Utility.CreateVarcharParam("@Phone", loc.Phone, 100),
                    Engage.Utility.CreateTextParam("@LocationDetails", loc.LocationDetails),
                    Engage.Utility.CreateIntegerParam("@LocationTypeId", loc.LocationTypeId),
                    Engage.Utility.CreateIntegerParam("@PortalId", loc.PortalId),
                    Engage.Utility.CreateIntegerParam("@CSVLineNumber", loc.CsvLineNumber),
                    // TODO: change this stored procedure to just take a bit parameter and remove this useless casting
                    Engage.Utility.CreateVarcharParam(
                            "@successful", Convert.ToInt32(successful, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture), 25),
                    Engage.Utility.CreateBitParam("@approved", loc.Approved));
        }

        public override void UpdateAttribute(int locationAttributeId, int locationId, string attributeValue)
        {
            SqlHelper.ExecuteNonQuery(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spUpdateLocationAttribute",
                    Engage.Utility.CreateIntegerParam("@locationAttributeId", locationAttributeId),
                    Engage.Utility.CreateIntegerParam("@locationId", locationId),
                    Engage.Utility.CreateTextParam("@attributeValue", attributeValue));
        }

        public override int UpdateAttributeDefinition(
                int attributeDefinitionId,
                int dataType,
                string defaultValue,
                string attributeName,
                bool required,
                string validationExpression,
                int viewOrder,
                bool visible,
                int length)
        {
            return SqlHelper.ExecuteNonQuery(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spUpdateAttributeDefinition",
                    new SqlParameter("@AttributeDefinitionId", attributeDefinitionId),
                    new SqlParameter("@DataType", dataType),
                    new SqlParameter("@DefaultValue", defaultValue),
                    new SqlParameter("@AttributeName", attributeName),
                    new SqlParameter("@Required", required),
                    new SqlParameter("@ValidationExpression", validationExpression),
                    new SqlParameter("@ViewOrder", viewOrder),
                    new SqlParameter("@Visible", visible),
                    new SqlParameter("@Length", length));
        }

        public override void UpdateComment(int locationId, string comment, string submittedBy, bool approved, int userId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void UpdateImportedLocationRow(int fileId)
        {
            SqlHelper.ExecuteNonQuery(
                    this.connectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spUpdateImportedLocationRow",
                    Engage.Utility.CreateIntegerParam("@FileId", fileId));
        }

        public override int UpdateLocation(Location loc)
        {
            return SqlHelper.ExecuteNonQuery(
                    this.ConnectionString,
                    CommandType.StoredProcedure,
                    this.NamePrefix + "spUpdateLocation",
                    Engage.Utility.CreateIntegerParam("@LocationId", loc.LocationId),
                    Engage.Utility.CreateVarcharParam("@ExternalIdentifier", loc.ExternalIdentifier, 100),
                    Engage.Utility.CreateIntegerParam("@LocationTypeId", loc.LocationTypeId),
                    Engage.Utility.CreateVarcharParam("@Name", loc.Name, 100),
                    Engage.Utility.CreateVarcharParam("@Website", loc.Website, 255),
                    Engage.Utility.CreateDoubleParam("@Latitude", loc.Latitude),
                    Engage.Utility.CreateDoubleParam("@Longitude", loc.Longitude),
                    Engage.Utility.CreateIntegerParam("@CountryId", loc.CountryId),
                    Engage.Utility.CreateIntegerParam("@RegionId", loc.RegionId),
                    Engage.Utility.CreateVarcharParam("@City", loc.City, 100),
                    Engage.Utility.CreateVarcharParam("@Address", loc.Address, 510),
                    Engage.Utility.CreateVarcharParam("@Address2", loc.Address2, 510),
                    Engage.Utility.CreateVarcharParam("@PostalCode", loc.PostalCode, 100),
                    Engage.Utility.CreateVarcharParam("@Phone", loc.Phone, 100),
                    Engage.Utility.CreateTextParam("@LocationDetails", loc.LocationDetails),
                    Engage.Utility.CreateIntegerParam("@PortalId", loc.PortalId),
                    Engage.Utility.CreateDateTimeParam("@LastUpdatedDate", loc.LastUpdateDate),
                    Engage.Utility.CreateIntegerParam("@approved", Convert.ToInt32(loc.Approved)));
        }

        public override void UpdateLocationType(int locationTypeId, string locationTypeName)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "update {0}LocationType set LocationTypeName = @locationTypeName where LocationTypeId = @locationTypeId",
                    this.NamePrefix);

            SqlHelper.ExecuteNonQuery(
                    this.ConnectionString,
                    CommandType.Text,
                    sql.ToString(),
                    Engage.Utility.CreateIntegerParam("@locationTypeId", locationTypeId),
                    Engage.Utility.CreateVarcharParam("@locationTypeName", locationTypeName, 50));
        }
    }
}