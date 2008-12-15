// <copyright file="SqlDataProvider.cs" company="Engage Software">
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
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using DotNetNuke.Framework.Providers;
    using Microsoft.ApplicationBlocks.Data;

    /// <summary>
    /// A concrete implementation of <see cref="DataProvider"/> for SQL Server (2000+ compatible)
    /// </summary>
    public class SqlDataProvider : DataProvider
    {
        public const string CacheKey = "AttributeDefinitions{0}";

        public const int CacheTimeOut = 20;

        /// <summary>
        /// The default size of a varchar parameter in the database
        /// </summary>
        private const int DefaultVarcharSize = 255;

        private readonly string databaseOwner;

        private readonly string objectQualifier;

        private readonly ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);

        private readonly string providerPath;

        public SqlDataProvider()
        {
            Provider provider = ((Provider)this.providerConfiguration.Providers[this.providerConfiguration.DefaultProvider]);

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
            this.ExecuteNonQuery(
                    "AddLocationAttribute",
                    Engage.Utility.CreateIntegerParam("@attributeDefinitionId", attributeDefinitionId),
                    Engage.Utility.CreateIntegerParam("@locationId", locationId),
                    Engage.Utility.CreateTextParam("@attributeValue", attributeValue));
        }

        public override int AddAttributeDefinition(
                int portalId,
                int locationTypeId,
                int dataType,
                string defaultValue,
                string attributeName,
                bool required,
                string validationExpression,
                int viewOrder,
                bool visible,
                int length)
        {
            return this.ExecuteNonQuery(
                    "AddAttributeDefinition",
                    Engage.Utility.CreateIntegerParam("@PortalId", portalId),
                    Engage.Utility.CreateIntegerParam("@LocationTypeId", locationTypeId),
                    Engage.Utility.CreateIntegerParam("@DataType", dataType),
                    Engage.Utility.CreateTextParam("@DefaultValue", defaultValue),
                    Engage.Utility.CreateVarcharParam("@AttributeName", attributeName, DefaultVarcharSize),
                    Engage.Utility.CreateBitParam("@Required", required),
                    Engage.Utility.CreateVarcharParam("@ValidationExpression", validationExpression, DefaultVarcharSize),
                    Engage.Utility.CreateIntegerParam("@ViewOrder", viewOrder),
                    Engage.Utility.CreateBitParam("@Visible", visible),
                    Engage.Utility.CreateIntegerParam("@Length", length));
        }

        public override void ClearLocations(int portalId)
        {
            this.ExecuteNonQuery("DeleteLocations", Engage.Utility.CreateIntegerParam("@PortalId", portalId));
        }

        public override void ClearTempLocations()
        {
            StringBuilder sql = new StringBuilder(250);
            sql.AppendFormat(CultureInfo.InvariantCulture, "Truncate table {0}TempLocation ", this.NamePrefix);

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql.ToString());
        }

        public override string CopyData()
        {
            try
            {
                this.ExecuteNonQuery("CopyLocation");
                return "Data Import Successful";
            }
            catch
            {
                return "Critical Error: Staged Data not Copied";
            }
        }

        public override void DeleteAttributeDefinition(int attributeDefinitionId)
        {
            this.ExecuteNonQuery("DeleteAttributeDefinition", Engage.Utility.CreateIntegerParam("@AttributeDefinitionId", attributeDefinitionId));
        }

        public override void DeleteComment(int commentId)
        {
            this.ExecuteNonQuery("DeleteComment", Engage.Utility.CreateIntegerParam("@commentId", commentId));
        }

        public override void DeleteLocation(int locationId)
        {
            this.ExecuteNonQuery("DeleteLocation", Engage.Utility.CreateIntegerParam("@locationId", locationId));
        }

        public override void DeleteLocationType(int locationTypeId)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, "Delete from {0}LocationType where LocationTypeId = @locationTypeId", this.NamePrefix);

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@locationTypeId", locationTypeId));
        }

        public override DataTable GetAllLocations(int portalId, bool approved, string sortColumn, int index, int pageSize)
        {
            return this.ExecuteDataset(
                "GetAllLocations",
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
                        sql.AppendFormat(CultureInfo.InvariantCulture, " AND ( Type = '" + LocationType.GetLocationTypeName(id).Replace("'", "''") + "'");
                    }
                    else
                    {
                        sql.AppendFormat(CultureInfo.InvariantCulture, " OR Type = '" + LocationType.GetLocationTypeName(id).Replace("'", "''") + "'");
                    }

                    i++;
                }

                sql.AppendFormat(CultureInfo.InvariantCulture, ")");
            }

            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) ", this.NamePrefix);

            return SqlHelper.ExecuteReader(
                    ConnectionString,
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
                                " AND ( Type = '{0}'",
                                LocationType.GetLocationTypeName(Convert.ToInt32(s, CultureInfo.InvariantCulture)).Replace("'", "''"));
                    }
                    else
                    {
                        sql.AppendFormat(
                                CultureInfo.InvariantCulture,
                                " OR Type = '{0}'", 
                                LocationType.GetLocationTypeName(Convert.ToInt32(s, CultureInfo.InvariantCulture)).Replace("'", "''"));
                    }

                    i++;
                }

                sql.AppendFormat(CultureInfo.InvariantCulture, ")");
            }

            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY LocationId ");

            return SqlHelper.ExecuteReader(
                    ConnectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@PortalId", portalId));
        }

        public override IDataReader GetAttributeDefinition(int attributeDefinitionId)
        {
            return this.ExecuteReader("GetAttributeDefinition", Engage.Utility.CreateIntegerParam("@AttributeDefinitionId", attributeDefinitionId));
        }

        public override IDataReader GetAttributeDefinitionByName(int locationTypeId, string name)
        {
            return this.ExecuteReader(
                    "GetAttributeDefinitionByName",
                    Engage.Utility.CreateIntegerParam("@locationTypeId", locationTypeId),
                    Engage.Utility.CreateVarcharParam("@Name", name));
        }

        public override DataTable GetAttributeDefinitions(int locationTypeId)
        {
            return this.ExecuteDataset("GetLocationAttributeDefinitions", Engage.Utility.CreateIntegerParam("@locationTypeId", locationTypeId)).Tables[0];
        }

        public override IDataReader GetAttributeDefinitionsById(int locationTypeId)
        {
            return this.ExecuteReader("GetAttributeDefinitionsById", Engage.Utility.CreateIntegerParam("@LocationTypeId", locationTypeId));
        }

        public override DataTable GetAttributeValues(int locationId)
        {
            return this.ExecuteDataset("GetLocationAttributeValues", Engage.Utility.CreateIntegerParam("@locationId", locationId)).Tables[0];
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
                        sql.AppendFormat(CultureInfo.InvariantCulture, " AND ( Type = '" + LocationType.GetLocationTypeName(id).Replace("'", "''") + "'");
                    }
                    else
                    {
                        sql.AppendFormat(CultureInfo.InvariantCulture, " OR Type = '" + LocationType.GetLocationTypeName(id).Replace("'", "''") + "'");
                    }

                    i++;
                }

                sql.AppendFormat(CultureInfo.InvariantCulture, ")");
            }

            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) ", this.NamePrefix);

            return SqlHelper.ExecuteReader(
                    ConnectionString,
                    CommandType.Text,
                    sql.ToString(),
                    Engage.Utility.CreateDoubleParam("@Latitude", latitude),
                    Engage.Utility.CreateDoubleParam("@Longitude", longitude),
                    Engage.Utility.CreateIntegerParam("@Radius", radius),
                    Engage.Utility.CreateIntegerParam("@PortalId", portalId));
        }

        public override Comment GetComment(int commentId)
        {
            using (IDataReader comment = this.ExecuteReader("GetComment", Engage.Utility.CreateIntegerParam("@commentId", commentId)))
            {
                if (comment.Read())
                {
                    Comment c = new Comment();
                    c.CommentId = Convert.ToInt32(comment["CommentId"], CultureInfo.InvariantCulture);
                    c.Text = comment["Text"].ToString();
                    c.Approved = Convert.ToBoolean(comment["Approved"], CultureInfo.InvariantCulture);
                    c.SubmittedBy = comment["Author"].ToString();
                    c.LocationName = comment["Name"].ToString();
                    return c;
                }

                return null;
            }
        }

        public override DataSet GetComments(int locationId, bool approved)
        {
            return this.ExecuteDataset(
                    "GetComments",
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
                            ConnectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@PortalId", portalId)).Tables[0];
        }

        public override DataTable GetEmailByFileId(int fileId)
        {
            StringBuilder sql = new StringBuilder(500);

            sql.AppendFormat(CultureInfo.InvariantCulture, " Select U.FirstName, U.LastName, KF.UploadDate, U.Email ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " From {0}Files KF ", this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Join {0}Users u on U.UserID = KF.UserID ", this.DatabaseOwner + this.ObjectQualifier);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Where FileId = @FileId ");

            return
                    SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@FileId", fileId)).Tables[0];
        }

        public override DataTable GetEngageLocatorTabModules(int portalId)
        {
            return this.ExecuteDataset("GetEngageLocatorTabModules", Engage.Utility.CreateIntegerParam("@portalId", portalId)).Tables[0];
        }

        public override DataTable GetFilesToImport()
        {
            return this.ExecuteDataset("GetFilesToImport").Tables[0];
        }

        public override DataTable GetImportedLocationStatistics(int portalId)
        {
            return this.ExecuteDataset("GetImportedLocationStatistics", Engage.Utility.CreateIntegerParam("@portalId", portalId)).Tables[0];
        }

        public override int GetLastImportIndex()
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Select max(CSVLineNumber) ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " From {0}TempLocation", this.NamePrefix);

            object value = SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, sql.ToString());
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
                    SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateVarcharParam("@address", address, DefaultVarcharSize), Engage.Utility.CreateVarcharParam("@city", city, DefaultVarcharSize)).Tables[0];
        }

        public override IDataReader GetLocation(int locationId)
        {
            StringBuilder sql = new StringBuilder(98);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "SELECT LocationId, LocationTypeId, ExternalIdentifier, Name, Website, Latitude, Longitude, Abbreviation, CountryName, City, Address, Address2, Phone, StateName, RegionId, LocationDetails, [Type], PostalCode, Approved, AverageRating ");
            sql.AppendFormat(CultureInfo.InvariantCulture, "FROM {0}vLocations ", this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE LocationId = @LocationId ");

            return SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@LocationId", locationId));
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

            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@LocationTypeId", typeId), Engage.Utility.CreateIntegerParam("@PortalId", portalId)).Tables[0];
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
                    ConnectionString,
                    CommandType.Text,
                    sql.ToString(),
                    Engage.Utility.CreateIntegerParam("@CountryId", countryId),
                    Engage.Utility.CreateIntegerParam("@PortalId", portalId));
        }

        public override DataTable GetLocationType(int id)
        {
            return this.ExecuteDataset("GetLocationType", Engage.Utility.CreateIntegerParam("@LocationTypeId", id)).Tables[0];
        }

        public override int GetLocationTypeCount(string type)
        {
            return Convert.ToInt32(
                    this.ExecuteScalar(
                        "GetLocationTypeCount", 
                        Engage.Utility.CreateVarcharParam("@LocationType", type)), 
                    CultureInfo.InvariantCulture);
        }

        public override DataTable GetLocationTypeName(int id)
        {
            return this.ExecuteDataset("GetLocationType", Engage.Utility.CreateIntegerParam("@LocationTypeId", id)).Tables[0];
        }

        public override DataTable GetLocationTypes()
        {
            StringBuilder sql = new StringBuilder(500);

            sql.AppendFormat(CultureInfo.InvariantCulture, " Select LocationTypeID, LocationTypeName ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " From {0}LocationType ", this.NamePrefix);

            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql.ToString()).Tables[0];
        }

        /// <summary>
        /// Gets the <paramref name="count"/> closest locations.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="count">The number of locations to return.</param>
        /// <param name="portalId">The portal id.</param>
        /// <returns>
        /// A list of location records as well as their distance from the given location.
        /// </returns>
        public override DataTable GetNClosestLocations(double latitude, double longitude, int count, int portalId)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "SELECT TOP {1} LocationId, Id, LocationTypeId, Name, Latitude, Longitude, Abbreviation, CountryName, City, Address, Address2, PostalCode, Phone, LocationDetails, [Type], {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) AS Distance ",
                    this.NamePrefix,
                    count);
            sql.AppendFormat(CultureInfo.InvariantCulture, " FROM {0}vLocations ", this.NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE PortalId = @PortalId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND IsSearchable = 1 ");
            ////sql.AppendFormat(CultureInfo.InvariantCulture, " AND [Type] = 1 ");
            sql.AppendFormat(
                    CultureInfo.InvariantCulture, " ORDER BY {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude)", this.NamePrefix);

            return
                    SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateDoubleParam("@Latitude", latitude), Engage.Utility.CreateDoubleParam("@Longitude", longitude), Engage.Utility.CreateIntegerParam("@PortalId", portalId)).Tables[0];
        }

        public override DataTable GetNewSubmittedComments(int portalId, bool approved)
        {
            return this.ExecuteDataset(
                "GetNewSubmittedComments",
                Engage.Utility.CreateIntegerParam("@portalId", portalId),
                Engage.Utility.CreateIntegerParam("@approved", Convert.ToInt32(approved))).Tables[0];
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
                                    ConnectionString, CommandType.Text, sql.ToString(), Engage.Utility.CreateIntegerParam("@FileId", fileId)),
                            CultureInfo.InvariantCulture);
        }

        public override void InsertComment(int locationId, string text, string submittedBy, bool approved)
        {
            this.ExecuteNonQuery(
                    "InsertComment",
                    Engage.Utility.CreateIntegerParam("@locationId", locationId),
                    Engage.Utility.CreateTextParam("@text", text),
                    Engage.Utility.CreateVarcharParam("@submittedBy", submittedBy, 50),
                    Engage.Utility.CreateIntegerParam("@approved", Convert.ToInt32(approved)));
        }

        public override void InsertFileInfo(int fileId, int userId, int tabModuleId, int portalId, DateTime uploadDate, bool succeeded, bool processed)
        {
            this.ExecuteNonQuery(
                    "InsertFileInfo",
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
            ////sql.AppendFormat(CultureInfo.InvariantCulture, "SELECT @@IDENTITY");

            return Convert.ToInt32(
                SqlHelper.ExecuteNonQuery(
                    ConnectionString,
                    CommandType.Text,
                    sql.ToString(),
                    Engage.Utility.CreateVarcharParam("@locationTypeName", locationTypeName, 50)),
                CultureInfo.InvariantCulture);
        }

        public override void SaveComment(Comment myComment)
        {
            this.ExecuteNonQuery(
                    "SaveComment",
                    Engage.Utility.CreateIntegerParam("@commentId", myComment.CommentId),
                    Engage.Utility.CreateTextParam("@text", myComment.Text),
                    Engage.Utility.CreateVarcharParam("@submittedBy", myComment.SubmittedBy, 50),
                    Engage.Utility.CreateIntegerParam("@approved", Convert.ToInt32(myComment.Approved)));
        }

        public override int SaveLocation(Location loc)
        {
            return Convert.ToInt32(
                    this.ExecuteScalar(
                            "InsertLocation",
                            Engage.Utility.CreateVarcharParam("@ExternalIdentifier", loc.ExternalIdentifier, 100),
                            Engage.Utility.CreateVarcharParam("@Name", loc.Name, 100),
                            Engage.Utility.CreateVarcharParam("@Website", loc.Website, DefaultVarcharSize),
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
            return this.ExecuteNonQuery(
                    "InsertTempLocation",
                    Engage.Utility.CreateVarcharParam("@ExternalIdentifier", loc.ExternalIdentifier, 100),
                    Engage.Utility.CreateVarcharParam("@Name", loc.Name, 100),
                    Engage.Utility.CreateVarcharParam("@Website", loc.Website, DefaultVarcharSize),
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
                    Engage.Utility.CreateVarcharParam("@successful", Convert.ToInt32(successful, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture), 25),
                    Engage.Utility.CreateBitParam("@approved", loc.Approved));
        }

        public override void UpdateAttribute(int locationAttributeId, int locationId, string attributeValue)
        {
            this.ExecuteNonQuery(
                    "UpdateLocationAttribute",
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
            return this.ExecuteNonQuery(
                    "UpdateAttributeDefinition",
                    Engage.Utility.CreateIntegerParam("@AttributeDefinitionId", attributeDefinitionId),
                    Engage.Utility.CreateIntegerParam("@DataType", dataType),
                    Engage.Utility.CreateTextParam("@DefaultValue", defaultValue),
                    Engage.Utility.CreateVarcharParam("@AttributeName", attributeName, DefaultVarcharSize),
                    Engage.Utility.CreateBitParam("@Required", required),
                    Engage.Utility.CreateVarcharParam("@ValidationExpression", validationExpression, DefaultVarcharSize),
                    Engage.Utility.CreateIntegerParam("@ViewOrder", viewOrder),
                    Engage.Utility.CreateBitParam("@Visible", visible),
                    Engage.Utility.CreateIntegerParam("@Length", length));
        }

        /// <exception cref="NotImplementedException">The method or operation is not implemented.</exception>
        public override void UpdateComment(int locationId, string comment, string submittedBy, bool approved, int userId)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public override void UpdateImportedLocationRow(int fileId)
        {
            this.ExecuteNonQuery("UpdateImportedLocationRow", Engage.Utility.CreateIntegerParam("@FileId", fileId));
        }

        public override int UpdateLocation(Location loc)
        {
            return this.ExecuteNonQuery(
                    "UpdateLocation",
                    Engage.Utility.CreateIntegerParam("@LocationId", loc.LocationId),
                    Engage.Utility.CreateVarcharParam("@ExternalIdentifier", loc.ExternalIdentifier, 100),
                    Engage.Utility.CreateIntegerParam("@LocationTypeId", loc.LocationTypeId),
                    Engage.Utility.CreateVarcharParam("@Name", loc.Name, 100),
                    Engage.Utility.CreateVarcharParam("@Website", loc.Website, DefaultVarcharSize),
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
                    ConnectionString,
                    CommandType.Text,
                    sql.ToString(),
                    Engage.Utility.CreateIntegerParam("@locationTypeId", locationTypeId),
                    Engage.Utility.CreateVarcharParam("@locationTypeName", locationTypeName, 50));
        }

        /// <summary>
        /// Executes a SQL stored procedure without returning any value.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.  Does not include any prefix, for example "InsertLocation" is translated to "dnn_EngageLocator_spInsertLocation."</param>
        /// <param name="parameters">The parameters for this query.</param>
        /// <returns>The number of rows affected by the given stored procedure</returns>
        private int ExecuteNonQuery(string storedProcedureName, params SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                this.NamePrefix + "sp" + storedProcedureName,
                parameters);
        }

        /// <summary>
        /// Executes a SQL stored procedure, returning the results as a <see cref="DataSet"/>.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.  Does not include any prefix, for example "GetLocations" is translated to "dnn_EngageEmployment_spGetLocations."</param>
        /// <param name="parameters">The parameters for this query.</param>
        /// <returns>The result set of the given stored procedure as a <see cref="DataSet"/>.</returns>
        private DataSet ExecuteDataset(string storedProcedureName, params SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteDataset(
                ConnectionString,
                CommandType.StoredProcedure,
                this.NamePrefix + "sp" + storedProcedureName,
                parameters);
        }

        /// <summary>
        /// Executes a SQL stored procedure, returning the results as a <see cref="SqlDataReader"/>.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.  Does not include any prefix, for example "GetLocations" is translated to "dnn_EngageEmployment_spGetLocations."</param>
        /// <param name="parameters">The parameters for this query.</param>
        /// <returns>The result set of the given stored procedure as a <see cref="SqlDataReader"/>.</returns>
        private SqlDataReader ExecuteReader(string storedProcedureName, params SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                this.NamePrefix + "sp" + storedProcedureName,
                parameters);
        }

        /// <summary>
        /// Executes a SQL stored procedure, returning a single value.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.  Does not include any prefix, for example "InsertLocation" is translated to "dnn_EngageEmployment_spInsertLocation."</param>
        /// <param name="parameters">The parameters for this query.</param>
        /// <returns>The single return value from the execution of the given stored procedure</returns>
        private object ExecuteScalar(string storedProcedureName, params SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteScalar(
                ConnectionString,
                CommandType.StoredProcedure,
                this.NamePrefix + "sp" + storedProcedureName,
                parameters);
        }
    }
}