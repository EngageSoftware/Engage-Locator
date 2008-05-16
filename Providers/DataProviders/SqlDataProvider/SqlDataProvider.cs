//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace Engage.Dnn.Locator.Data
{
    public class SqlDataProvider : DataProvider
    {
        #region Constants
        private const string providerType = "data";
        public const string CacheKey = "AttributeDefinitions{0}";
        public const int CacheTimeOut = 20;

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
        public SqlDataProvider()
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

        /// <summary>
        /// Gets the N closest locations.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="N">The number of locations to return.</param>
        /// <param name="portalId">The portal id.</param>
        /// <returns>A <see cref="DataTable"/> containing the location records as well as their distance from the given location.</returns>
        public override DataTable GetNClosestLocations(double latitude, double longitude, int N, int portalId)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, "SELECT TOP {1} LocationId, Id, LocationTypeId, Name, Latitude, Longitude, Abbreviation, CountryName, City, Address, PostalCode, Phone, LocationDetails, [Type], {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) AS Distance ", NamePrefix, N);
            sql.AppendFormat(CultureInfo.InvariantCulture, " FROM {0}vLocations ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE PortalId = @PortalId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND IsSearchable = 1 ");
            //sql.AppendFormat(CultureInfo.InvariantCulture, " AND [Type] = 1 ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude)", NamePrefix);

            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql.ToString(), CreateDoubleParam("@Latitude", latitude), CreateDoubleParam("@Longitude", longitude), CreateIntegerParam("@PortalId", portalId)).Tables[0];
        }

        /// <summary>
        /// Gets the closest locations within a given radius.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="radius">The radius in miles.</param>
        /// <param name="portalId">The portal id.</param>
        /// <returns>A <see cref="DataTable"/> containing the location records as well as their distance from the given location.</returns>
        /// <param name="types"></param>
        public override DataTable GetClosestLocationsByRadius(double latitude, double longitude, int radius, int portalId, int[] locationTypeIds)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, "SELECT LocationId, LocationTypeId, ExternalIdentifier, Name, StateName, Website, Latitude, Longitude, Abbreviation, CountryName, City, Address, Phone, RegionId, LocationDetails, [Type], PostalCode, {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) AS Distance ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " FROM {0}vLocations ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) <= @Radius ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND PortalId = @PortalId ");
            //sql.AppendFormat(CultureInfo.InvariantCulture, " AND IsSearchable = 1 ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND Latitude IS NOT NULL ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND Longitude IS NOT NULL ");
            int i = 0;

            if (locationTypeIds != null)
            {
                foreach (int id in locationTypeIds)
                {
                    if (i == 0)
                    {
                        sql.AppendFormat(CultureInfo.InvariantCulture, " AND ( Type = '" + LocationType.GetLocationTypeName(id) + "'");
                    }
                    else
                    {
                        sql.AppendFormat(CultureInfo.InvariantCulture, " OR Type = '" + LocationType.GetLocationTypeName(id) + "'");
                    }

                    i++;
                }

                sql.AppendFormat(CultureInfo.InvariantCulture, ")");
                
            }

            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) ", NamePrefix);

            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql.ToString(), CreateDoubleParam("@Latitude", latitude), CreateDoubleParam("@Longitude", longitude), CreateIntegerParam("@Radius", radius), CreateIntegerParam("@PortalId", portalId)).Tables[0];
        }

        public override DataTable GetLocation(int locationId)
        {
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetLocation", CreateIntegerParam("@locationId", locationId)).Tables[0];
            //Location loc = new Location();
            
            //loc.Address = dt.Rows[0]["Address"].ToString();
            //loc.City = dt.Rows[0]["City"].ToString();
            //loc.LocationDetails = dt.Rows[0]["LocationDetails"].ToString();
            //loc.ExternalIdentifier = dt.Rows[0]["ExternalIdentifier"].ToString();
            //loc.Latitude = Convert.ToDouble(dt.Rows[0]["Latitude"]);
            //loc.LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"]);
            //loc.LocationTypeId = Convert.ToInt32(dt.Rows[0]["LocationTypeId"]);
            //loc.Longitude = Convert.ToDouble(dt.Rows[0]["Longitude"]);
            //loc.Name = dt.Rows[0]["Name"].ToString();
            //loc.Website = dt.Rows[0]["Website"].ToString();
            //loc.Phone = dt.Rows[0]["Phone"].ToString();
            //loc.PostalCode = dt.Rows[0]["PostalCode"].ToString();
            //loc.StateName = dt.Rows[0]["State"].ToString();
            //loc.RegionId = Convert.ToInt32(dt.Rows[0]["RegionId"].ToString());
            //loc.Approved = Convert.ToBoolean(dt.Rows[0]["Approved"].ToString());
        }

        public override DataTable GetLocationsByCountry(int countryId, int portalId)
        {
            StringBuilder sql = new StringBuilder(98);
            sql.AppendFormat(CultureInfo.InvariantCulture, "SELECT LocationId, LocationTypeId, ExternalIdentifier, Name, Website, Latitude, Longitude, Abbreviation, CountryName, City, Address, Phone, RegionId, LocationDetails, [Type], PostalCode FROM {0}vLocations ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE CountryId = @CountryId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND PortalId = @PortalId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY City ");

            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql.ToString(), CreateIntegerParam("@CountryId", countryId), CreateIntegerParam("@PortalId", portalId)).Tables[0];
        }

        public override DataTable GetAllLocations(int portalId, bool approved, string sortColumn, int index, int pageSize)
        {
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetAllLocations", 
                CreateIntegerParam("@PortalId", portalId), 
                CreateBitParam("@approved", approved),
                CreateVarcharParam("@sortColumn", sortColumn, 200),
                CreateIntegerParam("@index", index),
                CreateIntegerParam("@pagesize", pageSize)).Tables[0];
        }

        public override void DeleteLocation(int locationId)
        {
            StringBuilder sql = new StringBuilder(98);
            sql.AppendFormat(CultureInfo.InvariantCulture, "Delete from {0}Location where LocationId = @locationId", NamePrefix);

            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql.ToString(), CreateIntegerParam("@locationId", locationId));
        }

        public override string CopyData()
        {
            try
            {
                StringBuilder sql = new StringBuilder(500);
                sql.AppendFormat(CultureInfo.InvariantCulture, "EXEC {0}spCopyLocation", NamePrefix);
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql.ToString());
                return "Data Import Successful";
            }
            catch
            {
                return "Critical Error: Staged Data not Copied";
            }
        }

        public override void ClearLocations(int portalId)
        {
            StringBuilder sql = new StringBuilder(250);
            sql.AppendFormat(CultureInfo.InvariantCulture, "EXEC {0}spDeleteLocations ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, "@PortalId");

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql.ToString(), CreateIntegerParam("@PortalId", portalId));
        }

        public override void ClearTempLocations()
        {
            StringBuilder sql = new StringBuilder(250);
            sql.AppendFormat(CultureInfo.InvariantCulture, "Truncate table {0}TempLocation ", NamePrefix);

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql.ToString());
        }

        public override void InsertFileInfo(int fileId, int userId, int tabModuleId, int portalId, DateTime uploadDate, int succeeded, int processed)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, "Insert into {0}Files (FileId, UserId, TabModuleId, PortalId, UploadDate, Succeeded, Processed)  values (@FileId, @UserId, @TabModuleId,  @PortalId, @UploadDate, @Succeeded, @Processed)", NamePrefix);

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql.ToString(), CreateIntegerParam("@FileId", fileId), CreateIntegerParam("@UserId", userId), CreateIntegerParam("@TabModuleId", tabModuleId), CreateIntegerParam("@PortalId", portalId), CreateDateTimeParam("@UploadDate", uploadDate), CreateIntegerParam("@Succeeded", succeeded), CreateIntegerParam("@Processed", processed));
        }

        public override DataTable GetEmailByFileId(int fileId)
        {
            StringBuilder sql = new StringBuilder(500);
            

            sql.AppendFormat(CultureInfo.InvariantCulture, " Select U.FirstName, U.LastName, KF.UploadDate, U.Email ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " From {0}Files KF ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Join {0}Users u on U.UserID = KF.UserID ", DatabaseOwner + ObjectQualifier);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Where FileId = @FileId ");

            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql.ToString(), CreateIntegerParam("@FileId", fileId)).Tables[0];
        }

        public override int GetLastImportIndex()
        {
            StringBuilder sql = new StringBuilder(500);

            sql.AppendFormat(CultureInfo.InvariantCulture, " Select max(CSVLineNumber) ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " From {0}TempLocation", NamePrefix);
            Object value = SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sql.ToString());
            if (value == DBNull.Value)
                return 0;
            else
                return Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sql.ToString()));
        }

        #region Private Helper Methods
        /// <summary>
        /// Creates a String (varchar) SQL param, setting and checking the bounds of the value within the parameter.
        /// Sets the value to DBNull if the string is <see cref="Null.NullString"/> or <c>Nothing</c>.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the field in the database.</param>
        /// <returns>A SqlParameter with the correct value, type, and capacity.</returns>
        private static SqlParameter CreateVarcharParam(string parameterName, string value, int size) {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.VarChar, size);
            if (value == null || value.Equals(Null.NullString))
            {
                param.Value = DBNull.Value;
            }
            else
            {
                if (value.Length > size)
                {
                    value = value.Substring(0, size);
                }
                param.Value = value;
            }
            return param;
        }

        /// <summary>
        /// Creates a Bit SQL param, setting the value to DBNull if <paramref name="value"/> is <see cref="Nullable<bool>"/> without a value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A SqlParameter with the correct value and type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledpublicCode")]
        public static SqlParameter CreateBitParam(string parameterName, bool? value)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.Bit);
            if (!value.HasValue)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = value.Value;
            }
            return param;
        }

    
        // <summary>
        // Creates a DateTime SQL param, setting the value to DBNull if the DateTime is its initial value, <see cref="Null.NullDate"/>,
        // <see cref="Date.MinValue"/>, <see cref="Date.MaxValue"/>, or <see cref="Nullable(Of DateTime)"/> without a value.
        // </summary>
        // <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        // <param name="value">The value of the parameter.</param>
        // <returns>A SqlParameter with the correct value and type.</returns>
        private static SqlParameter CreateDateTimeParam(string parameterName, DateTime? value)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.DateTime);
            if (!value.HasValue || value.Equals(Null.NullDate) || value.Equals(new DateTime()) || value.Equals(DateTime.MaxValue) || value.Equals(DateTime.MinValue))
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = value;
            }
            return param;
        }
    
        // <summary>
        // Creates an Integer SQL param, setting the value to DBNull if the value is <see cref="Null.NullInteger"/>, 
        // <see cref="Integer.MaxValue"/>, <see cref="Integer.MinValue"/>, or <see cref="Nullable(Of Integer)"/> without a value.
        // </summary>
        // <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        // <param name="value">The value of the parameter.</param>
        // <returns>A SqlParameter with the correct value and type.</returns>
        private static SqlParameter CreateIntegerParam(string parameterName, int? value)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.Int);
            if (!value.HasValue || value.Equals(Null.NullInteger) || value.Equals(int.MaxValue) || value.Equals(int.MinValue))
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = value;
            }
            return param;
        }

        // <summary>
        // Creates an Real SQL param, setting the value to DBNull if the value is <see cref="Null.NullSingle"/>, 
        // <see cref="Single.MaxValue"/>, <see cref="Single.MinValue"/>, or <see cref="Nullable(Of Single)"/> without a value.
        // </summary>
        // <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        // <param name="value">The value of the parameter.</param>
        // <returns>A <see cref="SqlParameter" /> with the correct value and type.</returns>
        private static SqlParameter CreateDoubleParam(string parameterName, double? value)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.Float);
            if (!value.HasValue || value.Equals(Null.NullSingle) || value.Equals(double.MaxValue) || value.Equals(double.MinValue))
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = value;
            }
            return param;
        }
        #endregion

        public override DataTable GetLatitudeLongitude(string address, string city)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Select Latitude, Longitude from {0}Location where Address = @address AND City = @city", NamePrefix);

            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql.ToString(), CreateVarcharParam("@address", address, 255), CreateVarcharParam("@city", city, 255)).Tables[0];
        }

        public override DataTable GetLocationTypes()
        {
            StringBuilder sql = new StringBuilder(500);

            sql.AppendFormat(CultureInfo.InvariantCulture, " Select LocationTypeID, LocationTypeName ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " From {0}LocationType ", NamePrefix);
            
            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql.ToString()).Tables[0];
        }

        public override int GetLocationTypeCount(string type)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@LocationType";
            param.Value = type;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetLocationTypeCount", param));
            return count;
        }

        public override DataTable GetLocationTypeName(int id)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@LocationTypeId";
            param.Value = id;

            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetLocationTypeName", param).Tables[0];
        }            

        public override void UpdateLocationType(int locationTypeId, string locationTypeName)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, "update {0}LocationType set LocationTypeName = @locationTypeName where LocationTypeId = @locationTypeId", NamePrefix);

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql.ToString(), CreateIntegerParam("@locationTypeId", locationTypeId), CreateVarcharParam("@locationTypeName", locationTypeName, 50));
        }

        public override int InsertLocationType(string locationTypeName)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, "insert into {0}LocationType (LocationTypeName) values (@locationTypeName) ", NamePrefix);
            //sql.AppendFormat(CultureInfo.InvariantCulture, "SELECT @@IDENTITY");

            return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql.ToString(), CreateVarcharParam("@locationTypeName", locationTypeName, 50)));
        }

        public override void DeleteLocationType(int locationTypeId)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, "Delete from {0}LocationType where LocationTypeId = @locationTypeId", NamePrefix);

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql.ToString(), CreateIntegerParam("@locationTypeId", locationTypeId));
        }

        public override int SaveLocation(Location loc)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, NamePrefix + "spInsertLocation", CreateVarcharParam("@ExternalIdentifier", loc.ExternalIdentifier, 100), CreateVarcharParam("@Name", loc.Name, 100), CreateVarcharParam("@Website", loc.Website, 255), CreateDoubleParam("@Latitude", loc.Latitude), CreateDoubleParam("@Longitude", loc.Longitude), CreateIntegerParam("@CountryId", loc.CountryId), CreateIntegerParam("@RegionId", loc.RegionId), CreateVarcharParam("@City", loc.City, 100), CreateVarcharParam("@Address", loc.Address, 100), CreateVarcharParam("@PostalCode", loc.PostalCode, 100), CreateVarcharParam("@Phone", loc.Phone, 100), CreateVarcharParam("@LocationDetails", loc.LocationDetails, 100), CreateIntegerParam("@LocationTypeId", loc.LocationTypeId), CreateIntegerParam("@CsvLineNumber", 0), CreateIntegerParam("@PortalId", loc.PortalId), CreateIntegerParam("@approved", Convert.ToInt32(loc.Approved))));
        }

        public override int SaveTempLocation(Location loc, bool successful)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, NamePrefix + "spInsertTempLocation", CreateVarcharParam("@ExternalIdentifier", loc.ExternalIdentifier, 100), CreateVarcharParam("@Name", loc.Name, 100), CreateVarcharParam("@Website", loc.Website, 255), CreateDoubleParam("@Latitude", loc.Latitude), CreateDoubleParam("@Longitude", loc.Longitude), CreateIntegerParam("@CountryId", loc.CountryId), CreateIntegerParam("@RegionId", loc.RegionId), CreateVarcharParam("@City", loc.City, 100), CreateVarcharParam("@Address", loc.Address, 100), CreateVarcharParam("@PostalCode", loc.PostalCode, 100), CreateVarcharParam("@Phone", loc.Phone, 100), CreateVarcharParam("@LocationDetails", loc.LocationDetails, 100), CreateIntegerParam("@LocationTypeId", loc.LocationTypeId), CreateIntegerParam("@PortalId", loc.PortalId), CreateIntegerParam("@CSVLineNumber", loc.CsvLineNumber), CreateVarcharParam("@successful", Convert.ToInt32(successful).ToString(), 25), CreateIntegerParam("@approved", Convert.ToInt32(loc.Approved)));
        }

        public override int UpdateLocation(Location loc)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, NamePrefix + "spUpdateLocation", CreateIntegerParam("@LocationId", loc.LocationId), CreateVarcharParam("@ExternalIdentifier", loc.ExternalIdentifier, 100), CreateIntegerParam("@LocationTypeId", loc.LocationTypeId), CreateVarcharParam("@Name", loc.Name, 100), CreateVarcharParam("@Website", loc.Website, 255), CreateDoubleParam("@Latitude", loc.Latitude), CreateDoubleParam("@Longitude", loc.Longitude), CreateIntegerParam("@CountryId", loc.CountryId), CreateIntegerParam("@RegionId", loc.RegionId), CreateVarcharParam("@City", loc.City, 100), CreateVarcharParam("@Address", loc.Address, 100), CreateVarcharParam("@PostalCode", loc.PostalCode, 100), CreateVarcharParam("@Phone", loc.Phone, 100), CreateVarcharParam("@LocationDetails", loc.LocationDetails, 500), CreateIntegerParam("@PortalId", loc.PortalId), CreateDateTimeParam("@LastUpdatedDate", loc.LastUpdateDate), CreateIntegerParam("@approved", Convert.ToInt32(loc.Approved)));
        }

        public override DataTable GetCountriesList(int portalId)
        {
            StringBuilder sql = new StringBuilder(100);
            sql.AppendFormat(CultureInfo.InvariantCulture, "SELECT DISTINCT c.EntryId, c.[Text] FROM {0}Lists c ", DatabaseOwner + ObjectQualifier);
            sql.AppendFormat(CultureInfo.InvariantCulture, " JOIN {0}Location l ON (c.EntryId = l.CountryId) ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND l.PortalId = @PortalId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY c.[Text] ");

            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql.ToString(), CreateIntegerParam("@PortalId", portalId)).Tables[0];
        }

        public override int GetTabModuleIdByFileId(int fileId)
        {
            StringBuilder sql = new StringBuilder(500);


            sql.AppendFormat(CultureInfo.InvariantCulture, " Select TabModuleId ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " From  {0}Files", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " Where FileId = @FileId ");

            return Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sql.ToString(), CreateIntegerParam("@FileId", fileId)));
        }

        public override DataTable GetFilesToImport()
        {
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetFilesToImport").Tables[0];
        }

        public override void UpdateImportedLocationRow(int fileId)
        {
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, NamePrefix + "spUpdateImportedLocationRow", CreateIntegerParam("@FileId", fileId));
        }

        public override DataTable GetAllLocationsByDistance(double latitude, double longitude, int portalId, int[] locationTypeIds)
        {
            StringBuilder sql = new StringBuilder(500);
            sql.AppendFormat(CultureInfo.InvariantCulture, "SELECT LocationId, LocationTypeId, ExternalIdentifier, Name, Website, Latitude, Longitude, Abbreviation, CountryName, City, Address, RegionId, Phone, LocationDetails, [Type], PostalCode, {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) AS Distance ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " FROM {0}vLocations ", NamePrefix);
            //sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) <= @Radius ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE PortalId = @PortalId ");
            //sql.AppendFormat(CultureInfo.InvariantCulture, " AND IsSearchable = 1 ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND Latitude IS NOT NULL ");
            sql.AppendFormat(CultureInfo.InvariantCulture, " AND Longitude IS NOT NULL ");
            int i = 0;

            if (locationTypeIds != null)
            {
                foreach (int id in locationTypeIds)
                {
                    if (i == 0)
                    {
                        sql.AppendFormat(CultureInfo.InvariantCulture, " AND ( Type = '" + LocationType.GetLocationTypeName(id) + "'");
                    }
                    else
                    {
                        sql.AppendFormat(CultureInfo.InvariantCulture, " OR Type = '" + LocationType.GetLocationTypeName(id) + "'");
                    }

                    i++;
                }

                sql.AppendFormat(CultureInfo.InvariantCulture, ")");

            }

            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY {0}fnDistanceBetween(@Latitude, @Longitude, Latitude, Longitude) ", NamePrefix);

            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql.ToString(), CreateDoubleParam("@Latitude", latitude), CreateDoubleParam("@Longitude", longitude), CreateIntegerParam("@PortalId", portalId)).Tables[0];
        }

        public override DataTable GetAllLocationsByType(int portalId, string[] types)
        {
            StringBuilder sql = new StringBuilder(98);
            sql.AppendFormat(CultureInfo.InvariantCulture, "SELECT LocationId, vl.LocationTypeId, ExternalIdentifier, Name, WebSite, Abbreviation, StateName, ");
            sql.AppendFormat(CultureInfo.InvariantCulture, "CountryName as Country, CountryId, RegionId, City, Address, Latitude, Longitude, Phone, LocationDetails, ");
            sql.AppendFormat(CultureInfo.InvariantCulture, "lt.LocationTypeName, PostalCode FROM {0}vLocations vl join {0}LocationType lt on (vl.LocationTypeId = lt.LocationTypeId) ", NamePrefix);
            sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE vl.PortalId = @PortalId and Approved = 1");

            int i = 0;
            if (types[0] != String.Empty)
            {
                foreach (string s in types)
                {
                    if (i == 0)
                    {
                        sql.AppendFormat(CultureInfo.InvariantCulture, " AND ( Type = '" + LocationType.GetLocationTypeName(Convert.ToInt32(s)) + "'");
                    }
                    else
                    {
                        sql.AppendFormat(CultureInfo.InvariantCulture, " OR Type = '" + LocationType.GetLocationTypeName(Convert.ToInt32(s)) + "'");
                    }

                    i++;
                }

                sql.AppendFormat(CultureInfo.InvariantCulture, ")");

            }

            sql.AppendFormat(CultureInfo.InvariantCulture, " ORDER BY LocationId ");

            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql.ToString(), CreateIntegerParam("@PortalId", portalId)).Tables[0];
        }

        public override DataTable GetImportedLocationStatistics(int portalId)
        {
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetImportedLocationStatistics", CreateIntegerParam("@portalId", portalId)).Tables[0];
        }

        public override DataTable GetEngageLocatorTabModules(int portalId)
        {
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetEngageLocatorTabModules", CreateIntegerParam("@portalId", portalId)).Tables[0];
        }

        public override DataTable GetAttributeDefinitions(int locationTypeId)
        {
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetLocationAttributeDefinitions", CreateIntegerParam("@locationTypeId", locationTypeId)).Tables[0];
        }

        public override void UpdateAttribute(int locationAttributeId, int locationId, string attributeValue)
        {
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, NamePrefix + "spUpdateLocationAttribute", CreateIntegerParam("@locationAttributeId", locationAttributeId), CreateIntegerParam("@locationId", locationId), CreateVarcharParam("@attributeValue", attributeValue, 255));
        }

        public override DataTable GetAttributeValues(int locationId)
        {
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetLocationAttributeValues", CreateIntegerParam("@locationId", locationId)).Tables[0];
        }

        public override void InsertComment(int locationId, string text, string submittteBy, bool approved)
        {
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, NamePrefix + "spInsertComment",
                CreateIntegerParam("@locationId", locationId), CreateVarcharParam("@text", text, 200), CreateVarcharParam("@submittedBy", submittteBy, 50),
                CreateIntegerParam("@approved", Convert.ToInt32(approved)));
        }

        public override void UpdateComment(int locationId, string comment, string submittedBy, bool approved, int userId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override DataSet GetComments(int locationId, bool approved)
        {
            return
                SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetComments",
                    CreateIntegerParam("@locationId", locationId), CreateIntegerParam("@approved", Convert.ToInt32(approved)));
        }

        public override DataTable GetNewSubmittedComments(int portalId, bool approved)
        {
            DataSet comments =
                SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetNewSubmittedComments",
                    CreateIntegerParam("@portalId", portalId), CreateIntegerParam("@approved", Convert.ToInt32(approved)));
            return comments.Tables[0];
        }

        public override Comment GetComment(int commentId)
        {
            DataTable comment =
                SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetComment",
                    CreateIntegerParam("@commentId", commentId)).Tables[0];
            Comment c = new Comment();
            c.CommentId = Convert.ToInt32(comment.Rows[0]["CommentId"]);
            c.Text = comment.Rows[0]["Text"].ToString();
            c.Approved = Convert.ToBoolean(comment.Rows[0]["Approved"].ToString());
            c.SubmittedBy = comment.Rows[0]["Author"].ToString();
            c.LocationName = comment.Rows[0]["Name"].ToString();

            return c;
        }

        public override void SaveComment(Comment myComment)
        {
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, NamePrefix + "spSaveComment", CreateIntegerParam("@commentId", myComment.CommentId), CreateVarcharParam("@text", myComment.Text, 255), CreateVarcharParam("@submittedBy", myComment.SubmittedBy, 50), CreateIntegerParam("@approved", Convert.ToInt32(myComment.Approved)));
        }

        public override void DeleteComment(int commentId)
        {
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, NamePrefix + "spDeleteComment", CreateIntegerParam("@commentId", commentId));
        }

        public override void AddAttribute(int attributeDefinitionId, int locationId, string attributeValue)
        {
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, NamePrefix + "spAddLocationAttribute", CreateIntegerParam("@attributeDefinitionId", attributeDefinitionId), CreateIntegerParam("@locationId", locationId), CreateVarcharParam("@attributeValue", attributeValue, 255));
        }

        #region Custom Attributes Methods

        public override IDataReader GetAttributeDefinitionsById(int locationTypeId)
        {
            return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetAttributeDefinitionsById", new SqlParameter("@LocationTypeId", locationTypeId));
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

        public override IDataReader GetAttributeDefinitionByName(int locationTypeId, string name)
        {
            return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, NamePrefix + "spGetAttributeDefinitionByName", new SqlParameter("@locationTypeId", locationTypeId), new SqlParameter("@Name", name));
        }

        public override int UpdateAttributeDefinition(int attributeDefinitionId, int dataType, string defaultValue, string attributeName, bool required, string validationExpression, int viewOrder, bool visible, int length)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, NamePrefix + "spUpdateAttributeDefinition", new SqlParameter("@AttributeDefinitionId", attributeDefinitionId), new SqlParameter("@DataType", dataType), new SqlParameter("@DefaultValue", defaultValue), new SqlParameter("@AttributeName", attributeName), new SqlParameter("@Required", required), new SqlParameter("@ValidationExpression", validationExpression), new SqlParameter("@ViewOrder", viewOrder), new SqlParameter("@Visible", visible), new SqlParameter("@Length", length));
        }

        #endregion
    }
}
