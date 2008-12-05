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
        /// Backing field for <see cref="StateName"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string stateName;

        /// <summary>
        /// Backing field for <see cref="Website"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string website;
        
        public int LocationId
        {
            [DebuggerStepThrough]
            get { return this.locationId; }
            [DebuggerStepThrough]
            set { this.locationId = value; }
        }

        public string ExternalIdentifier
        {
            [DebuggerStepThrough]
            get { return this.externalIdentifier; }
            [DebuggerStepThrough]
            set { this.externalIdentifier = value; }
        }

        public string Name
        {
            [DebuggerStepThrough]
            get { return this.name; }
            [DebuggerStepThrough]
            set { this.name = value; }
        }

        public double Latitude
        {
            [DebuggerStepThrough]
            get { return this.latitude; }
            [DebuggerStepThrough]
            set { this.latitude = value; }
        }

        public double Longitude
        {
            [DebuggerStepThrough]
            get { return this.longitude; }
            [DebuggerStepThrough]
            set { this.longitude = value; }
        }

        public int CountryId
        {
            [DebuggerStepThrough]
            get { return this.countryId; }
            [DebuggerStepThrough]
            set { this.countryId = value; }
        }

        public int RegionId
        {
            [DebuggerStepThrough]
            get { return this.regionId; }
            [DebuggerStepThrough]
            set { this.regionId = value; }
        }

        public string StateName
        {
            [DebuggerStepThrough]
            get { return this.stateName; }
            [DebuggerStepThrough]
            set { this.stateName = value; }
        }

        public string City
        {
            [DebuggerStepThrough]
            get { return this.city; }
            [DebuggerStepThrough]
            set { this.city = value; }
        }

        public string Address
        {
            [DebuggerStepThrough]
            get { return this.address; }
            [DebuggerStepThrough]
            set { this.address = value; }
        }

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

        public string FullAddress
        {
            get
            {
                return this.address + (this.address2.Length > 0 ? " " + this.address2 : string.Empty);
            }
        }

        public string PostalCode
        {
            [DebuggerStepThrough]
            get { return this.postalCode; }
            [DebuggerStepThrough]
            set { this.postalCode = value; }
        }

        public string Phone
        {
            [DebuggerStepThrough]
            get { return this.phone; }
            [DebuggerStepThrough]
            set { this.phone = value; }
        }

        public string LocationDetails
        {
            [DebuggerStepThrough]
            get { return this.locationDetails; }
            [DebuggerStepThrough]
            set { this.locationDetails = value; }
        }

        public int LocationTypeId
        {
            [DebuggerStepThrough]
            get { return this.locationTypeId; }
            [DebuggerStepThrough]
            set { this.locationTypeId = value; }
        }

        public int PortalId
        {
            [DebuggerStepThrough]
            get { return this.portalId; }
            [DebuggerStepThrough]
            set { this.portalId = value; }
        }

        public string Website
        {
            [DebuggerStepThrough]
            get { return this.website; }
            [DebuggerStepThrough]
            set { this.website = value; }
        }

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

        public int CsvLineNumber
        {
            [DebuggerStepThrough]
            get { return this.csvLineNumber; }
            [DebuggerStepThrough]
            set { this.csvLineNumber = value; }
        }

        public bool Approved
        {
            [DebuggerStepThrough]
            get { return this.approved; }
            [DebuggerStepThrough]
            set { this.approved = value; }
        }

        public float AverageRating
        {
            [DebuggerStepThrough]
            get { return this.averageRating; }
            [DebuggerStepThrough]
            private set { this.averageRating = value; }
        }

        #region Methods

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

        #endregion

        #region Static Methods

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
            Location location = Load(DataProvider.Instance().GetLocation(locationId).Rows[0]);
            return location;
        }

        public static DataTable GetLocations(int portalId, bool approved, string sortColumn, int index, int pageSize)
        {
            return DataProvider.Instance().GetAllLocations(portalId, approved, sortColumn, index, pageSize);
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

        public static Location Load(DataRow row)
        {
            Location loc = new Location();

            loc.Address = row["Address"].ToString();
            loc.Address2 = row["Address2"].ToString();
            loc.City = row["City"].ToString();
            loc.LocationDetails = row["LocationDetails"].ToString();
            loc.ExternalIdentifier = row["ExternalIdentifier"].ToString();
            loc.Latitude = Convert.ToDouble(row["Latitude"], CultureInfo.InvariantCulture);
            loc.LocationId = Convert.ToInt32(row["LocationId"], CultureInfo.InvariantCulture);
            loc.LocationTypeId = Convert.ToInt32(row["LocationTypeId"], CultureInfo.InvariantCulture);
            loc.Longitude = Convert.ToDouble(row["Longitude"], CultureInfo.InvariantCulture);
            loc.Name = row["Name"].ToString();
            loc.Website = row["Website"].ToString();
            loc.Phone = row["Phone"].ToString();
            loc.PostalCode = row["PostalCode"].ToString();
            loc.StateName = row["StateName"].ToString();
            loc.RegionId = Convert.ToInt32(row["RegionId"].ToString(), CultureInfo.InvariantCulture);
            loc.Approved = Convert.ToBoolean(row["Approved"].ToString(), CultureInfo.InvariantCulture);
            loc.AverageRating = row["AverageRating"] is DBNull ? 0 : Convert.ToSingle(row["AverageRating"], CultureInfo.InvariantCulture);

            return loc;
        }

        #endregion
    }
}