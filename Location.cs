// <copyright file="Location.cs" company="Engage Software">
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Locator
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using Data;
    using DotNetNuke.Common.Lists;
    using DotNetNuke.Common.Utilities;

    /// <summary>
    /// Represents a location, the main component in Engage: Locator
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Backing field for <see cref="Address"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string address;

        /// <summary>
        /// Backing field for <see cref="Address2"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string address2;

        /// <summary>
        /// Backing field for <see cref="Approved"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool approved;

        /// <summary>
        /// Backing field for <see cref="AverageRating"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float averageRating;

        /// <summary>
        /// Backing field for <see cref="City"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string city;

        /// <summary>
        /// Backing field for <see cref="CountryId"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int countryId;

        /// <summary>
        /// Backing field for <see cref="CsvLineNumber"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int csvLineNumber;

        /// <summary>
        /// Backing field for <see cref="ExternalIdentifier"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string externalIdentifier;

        /// <summary>
        /// Backing field for <see cref="LastUpdateDate"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime lastUpdateDate;

        /// <summary>
        /// Backing field for <see cref="Latitude"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double latitude;

        /// <summary>
        /// Backing field for <see cref="LocationDetails"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string locationDetails;

        /// <summary>
        /// Backing field for <see cref="LocationId"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int locationId = -1;

        /// <summary>
        /// Backing field for <see cref="LocationTypeId"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int locationTypeId;

        /// <summary>
        /// Backing field for <see cref="Longitude"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double longitude;

        /// <summary>
        /// Backing field for <see cref="Name"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string name;

        /// <summary>
        /// Backing field for <see cref="Phone"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string phone;

        /// <summary>
        /// Backing field for <see cref="PortalId"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int portalId;

        /// <summary>
        /// Backing field for <see cref="PostalCode"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string postalCode;

        /// <summary>
        /// Backing field for <see cref="RegionId"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int regionId;

        /// <summary>
        /// Backing field for <see cref="RegionAbbreviation"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string regionAbbreviation;

        /// <summary>
        /// Backing field for <see cref="RegionName"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string regionName;

        /// <summary>
        /// Backing field for <see cref="Website"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string website;

        private double? distance;

        /// <summary>
        /// Gets or sets the location id.
        /// </summary>
        /// <value>The location id.</value>
        public int LocationId
        {
            [DebuggerStepThrough]
            get { return this.locationId; }
            [DebuggerStepThrough]
            set { this.locationId = value; }
        }

        /// <summary>
        /// Gets or sets the public-facing ID for this location.
        /// </summary>
        /// <value>The external identifier.</value>
        public string ExternalIdentifier
        {
            [DebuggerStepThrough]
            get { return this.externalIdentifier; }
            [DebuggerStepThrough]
            set { this.externalIdentifier = value; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name of this location.</value>
        public string Name
        {
            [DebuggerStepThrough]
            get { return this.name; }
            [DebuggerStepThrough]
            set { this.name = value; }
        }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude
        {
            [DebuggerStepThrough]
            get { return this.latitude; }
            [DebuggerStepThrough]
            set { this.latitude = value; }
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude
        {
            [DebuggerStepThrough]
            get { return this.longitude; }
            [DebuggerStepThrough]
            set { this.longitude = value; }
        }

        /// <summary>
        /// Gets or sets the country id.
        /// </summary>
        /// <value>The country id.</value>
        public int CountryId
        {
            [DebuggerStepThrough]
            get { return this.countryId; }
            [DebuggerStepThrough]
            set { this.countryId = value; }
        }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>The region id.</value>
        public int RegionId
        {
            [DebuggerStepThrough]
            get { return this.regionId; }
            [DebuggerStepThrough]
            set { this.regionId = value; }
        }

        /// <summary>
        /// Gets the region abbreviation.
        /// </summary>
        /// <value>The region abbreviation.</value>
        public string RegionAbbreviation
        {
            [DebuggerStepThrough]
            get { return this.regionAbbreviation; }
        }

        /// <summary>
        /// Gets the region name.
        /// </summary>
        /// <value>The region name.</value>
        public string RegionName
        {
            [DebuggerStepThrough]
            get { return this.regionName; }
        }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city in which this location resides.</value>
        public string City
        {
            [DebuggerStepThrough]
            get { return this.city; }
            [DebuggerStepThrough]
            set { this.city = value; }
        }

        /// <summary>
        /// Gets or sets the first line of the address.
        /// </summary>
        /// <value>The first line of the address.</value>
        public string Address
        {
            [DebuggerStepThrough]
            get { return this.address; }
            [DebuggerStepThrough]
            set { this.address = value; }
        }

        /// <summary>
        /// Gets or sets the second line of the address.
        /// </summary>
        /// <value>The second line of the address.</value>
        public string Address2
        {
            [DebuggerStepThrough]
            get { return this.address2; }
            [DebuggerStepThrough]
            set { this.address2 = value; }
        }

        /// <summary>
        /// Gets the third line of an address, containing city, state (if applicable) and postal code.
        /// </summary>
        /// <value>The third line of the address.</value>
        public string Address3
        {
            get
            {
                ListEntryInfo region = new ListController().GetListEntryInfo(this.RegionId);
                return this.City + ", " 
                    + (!Null.IsNull(region) 
                        ? region.Value + " " 
                        : string.Empty) 
                    + this.PostalCode;
            }
        }

        /// <summary>
        /// Gets the full address, a combination of <see cref="Address"/> and <see cref="Address2"/>.
        /// </summary>
        /// <value>The full address.</value>
        public string FullAddress
        {
            get
            {
                return this.address + (this.address2.Length > 0 ? " " + this.address2 : string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>The postal code.</value>
        public string PostalCode
        {
            [DebuggerStepThrough]
            get { return this.postalCode; }
            [DebuggerStepThrough]
            set { this.postalCode = value; }
        }

        /// <summary>
        /// Gets or sets the phone number for this location.
        /// </summary>
        /// <value>The phone number.</value>
        public string Phone
        {
            [DebuggerStepThrough]
            get { return this.phone; }
            [DebuggerStepThrough]
            set { this.phone = value; }
        }

        /// <summary>
        /// Gets or sets the location details.
        /// </summary>
        /// <value>The location details.</value>
        public string LocationDetails
        {
            [DebuggerStepThrough]
            get { return this.locationDetails; }
            [DebuggerStepThrough]
            set { this.locationDetails = value; }
        }

        /// <summary>
        /// Gets or sets the location type id.
        /// </summary>
        /// <value>The location type id.</value>
        public int LocationTypeId
        {
            [DebuggerStepThrough]
            get { return this.locationTypeId; }
            [DebuggerStepThrough]
            set { this.locationTypeId = value; }
        }

        /// <summary>
        /// Gets or sets the portal id.
        /// </summary>
        /// <value>The portal id.</value>
        public int PortalId
        {
            [DebuggerStepThrough]
            get { return this.portalId; }
            [DebuggerStepThrough]
            set { this.portalId = value; }
        }

        /// <summary>
        /// Gets or sets the website URL.
        /// </summary>
        /// <value>The website URL.</value>
        public string Website
        {
            [DebuggerStepThrough]
            get { return this.website; }
            [DebuggerStepThrough]
            set { this.website = value; }
        }

        /// <summary>
        /// Gets or sets the last update date.
        /// </summary>
        /// <value>The last update date.</value>
        public DateTime LastUpdateDate
        {
            get
            {
                return this.lastUpdateDate == DateTime.MinValue ? DateTime.Now : this.lastUpdateDate;
            }

            [DebuggerStepThrough]
            set 
            { 
                this.lastUpdateDate = value; 
            }
        }

        /// <summary>
        /// Gets or sets the line number on which this location was located when imported.
        /// </summary>
        /// <value>The CSV line number.</value>
        public int CsvLineNumber
        {
            [DebuggerStepThrough]
            get { return this.csvLineNumber; }
            [DebuggerStepThrough]
            set { this.csvLineNumber = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Location"/> is approved.
        /// </summary>
        /// <value><c>true</c> if approved; otherwise, <c>false</c>.</value>
        public bool Approved
        {
            [DebuggerStepThrough]
            get { return this.approved; }
            [DebuggerStepThrough]
            set { this.approved = value; }
        }

        /// <summary>
        /// Gets the average rating.
        /// </summary>
        /// <value>The average rating.</value>
        public float AverageRating
        {
            [DebuggerStepThrough]
            get { return this.averageRating; }
            [DebuggerStepThrough]
            private set { this.averageRating = value; }
        }

        /// <summary>
        /// Gets the distance of this location from the search query, or <c>null</c> if this location was not returned by a search query.
        /// </summary>
        /// <value>The distance of this location from the search query.</value>
        public double? Distance
        {
            [DebuggerStepThrough]
            get { return this.distance; }
        }

        public static DataTable GetCountriesList(int portalId)
        {
            return DataProvider.Instance().GetCountriesList(portalId);
        }

        public static DataTable GetFilesToImport()
        {
            return DataProvider.Instance().GetFilesToImport();
        }

        public static void UpdateImportedLocationRow(int fileId)
        {
            DataProvider.Instance().UpdateImportedLocationRow(fileId);
        }

        public static Location GetLocation(int locationId)
        {
            using (IDataReader reader = DataProvider.Instance().GetLocation(locationId))
            {
                if (reader.Read())
                {
                    return Load(reader);
                }
            }

            return null;
        }

        public static DataTable GetLocations(int portalId, bool approved, string sortColumn, int index, int pageSize)
        {
            return DataProvider.Instance().GetAllLocations(portalId, approved, sortColumn, index, pageSize);
        }

        public static List<Location> GetAllLocationsByDistance(double latitude, double longitude, int portalId, int[] locationTypeIds)
        {
            using (IDataReader reader = DataProvider.Instance().GetAllLocationsByDistance(latitude, longitude, portalId, locationTypeIds))
            {
                List<Location> locations = new List<Location>();
                while (reader.Read())
                {
                    locations.Add(Load(reader));
                }

                return locations;
            }
        }

        public static List<Location> GetClosestLocationsByRadius(double latitude, double longitude, int radius, int portalId, int[] locationTypeIds)
        {
            using (IDataReader reader = DataProvider.Instance().GetClosestLocationsByRadius(latitude, longitude, radius, portalId, locationTypeIds))
            {
                List<Location> locations = new List<Location>();
                while (reader.Read())
                {
                    locations.Add(Load(reader));
                }

                return locations;
            }
        }

        public static List<Location> GetAllLocationsByType(int portalId, string[] types)
        {
            using (IDataReader reader = DataProvider.Instance().GetAllLocationsByType(portalId, types))
            {
                List<Location> locations = new List<Location>();
                while (reader.Read())
                {
                    locations.Add(Load(reader));
                }

                return locations;
            }
        }

        public static List<Location> GetLocationsByCountry(int countryId, int portalId)
        {
            using (IDataReader reader = DataProvider.Instance().GetLocationsByCountry(countryId, portalId))
            {
                List<Location> locations = new List<Location>();
                while (reader.Read())
                {
                    locations.Add(Load(reader));
                }

                return locations;
            }
        }

        public static void DeleteLocation(int locationId)
        {
            DataProvider.Instance().DeleteLocation(locationId);
        }

        public static List<Location> GetSearchLocations(int portalId, bool approved)
        {
            DataTable locationsTable = DataProvider.Instance().GetAllLocations(portalId, approved, "Name", 0, 0);
            List<Location> locations = new List<Location>();
            foreach (DataRow row in locationsTable.Rows)
            {
                Location loc = GetLocation(Convert.ToInt32(row["LocationId"], CultureInfo.InvariantCulture));
                locations.Add(loc);
            }

            return locations;
        }

        public static void InsertComment(int locationId, string comment, string submittedBy, bool approved)
        {
            DataProvider.Instance().InsertComment(locationId, comment, submittedBy, approved);
        }

        public static DataTable GetNewSubmittedComments(int portalId, bool approved)
        {
            return DataProvider.Instance().GetNewSubmittedComments(portalId, approved);
        }

        /// <summary>
        /// Loads the <see cref="Location"/> instance contained in the specified <paramref name="row"/>.
        /// </summary>
        /// <param name="row">The data reader for the <see cref="Location"/> instance.</param>
        /// <returns>A <see cref="Location"/> instance instantiated from the given <paramref name="row"/></returns>
        public static Location Load(IDataReader row)
        {
            return Load(row, row.GetSchemaTable().Columns.Contains("Distance"));
        }

        /// <summary>
        /// Loads the <see cref="Location"/> instance contained in the specified <paramref name="row"/>.
        /// </summary>
        /// <param name="row">The data record for the <see cref="Location"/> instance.</param>
        /// <param name="containsDistanceColumn">if set to <c>true</c> <paramref name="row"/> contains the Distance column.</param>
        /// <returns>A <see cref="Location"/> instance instantiated from the given <paramref name="row"/></returns>
        public static Location Load(IDataRecord row, bool containsDistanceColumn)
        {
            Location loc = new Location();

            loc.address = row["Address"].ToString();
            loc.address2 = row["Address2"].ToString();
            loc.city = row["City"].ToString();
            loc.locationDetails = row["LocationDetails"].ToString();
            loc.externalIdentifier = row["ExternalIdentifier"].ToString();
            loc.latitude = Convert.ToDouble(row["Latitude"], CultureInfo.InvariantCulture);
            loc.locationId = Convert.ToInt32(row["LocationId"], CultureInfo.InvariantCulture);
            loc.locationTypeId = Convert.ToInt32(row["LocationTypeId"], CultureInfo.InvariantCulture);
            loc.longitude = Convert.ToDouble(row["Longitude"], CultureInfo.InvariantCulture);
            loc.name = row["Name"].ToString();
            loc.website = row["Website"].ToString();
            loc.phone = row["Phone"].ToString();
            loc.postalCode = row["PostalCode"].ToString();
            loc.regionName = row["StateName"].ToString();
            loc.regionAbbreviation = row["Abbreviation"].ToString();
            loc.regionId = Convert.ToInt32(row["RegionId"].ToString(), CultureInfo.InvariantCulture);
            loc.approved = Convert.ToBoolean(row["Approved"].ToString(), CultureInfo.InvariantCulture);
            loc.averageRating = row["AverageRating"] is DBNull ? 0 : Convert.ToSingle(row["AverageRating"], CultureInfo.InvariantCulture);
            loc.distance = containsDistanceColumn ? (double?)row["Distance"] : null;

            return loc;
        }

        public void Save()
        {
            if (this.locationId == -1)
            {
                this.locationId = DataProvider.Instance().SaveLocation(this);
            }
        }

        public void SaveTemp(bool successful)
        {
            if (this.locationId != -1)
            {
                this.locationId = DataProvider.Instance().SaveTempLocation(this, successful);
            }
        }

        public void Update()
        {
            if (this.locationId != -1)
            {
                this.locationId = DataProvider.Instance().UpdateLocation(this);
            }
        }

        public DataSet GetComments(bool onlyApproved)
        {
            return DataProvider.Instance().GetComments(this.locationId, onlyApproved);
        }

        public List<Attribute> GetAttributes()
        {
            return Attribute.GetAttributes(this.locationTypeId, this.locationId);
        }
    }
}