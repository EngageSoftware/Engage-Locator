//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Engage.Dnn.Locator.Data;

namespace Engage.Dnn.Locator
{
    public class Location
    {

        #region Properties

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _locationId = -1;
        public int LocationId
        {
            [DebuggerStepThroughAttribute]
            get { return _locationId; }
            [DebuggerStepThroughAttribute]
            set { _locationId = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _externalIdentifier;
        public string ExternalIdentifier
        {
            [DebuggerStepThroughAttribute]
            get { return _externalIdentifier; }
            [DebuggerStepThroughAttribute]
            set { _externalIdentifier = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _name;
        public string Name
        {
            [DebuggerStepThroughAttribute]
            get { return _name; }
            [DebuggerStepThroughAttribute]
            set { _name = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _latitude;
        public double Latitude
        {
            [DebuggerStepThroughAttribute]
            get { return _latitude; }
            [DebuggerStepThroughAttribute]
            set { _latitude = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _longitude;
        public double Longitude
        {
            [DebuggerStepThroughAttribute]
            get { return _longitude; }
            [DebuggerStepThroughAttribute]
            set { _longitude = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _countryId;
        public int CountryId
        {
            [DebuggerStepThroughAttribute]
            get { return _countryId; }
            [DebuggerStepThroughAttribute]
            set { _countryId = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _regionId;
        public int RegionId
        {
            [DebuggerStepThroughAttribute]
            get { return _regionId; }
            [DebuggerStepThroughAttribute]
            set { _regionId = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _stateName;
        public string StateName
        {
            [DebuggerStepThroughAttribute]
            get { return _stateName; }
            [DebuggerStepThroughAttribute]
            set { _stateName = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _city;
        public string City
        {
            [DebuggerStepThroughAttribute]
            get { return _city; }
            [DebuggerStepThroughAttribute]
            set { _city = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _address;
        public string Address
        {
            [DebuggerStepThroughAttribute]
            get { return _address; }
            [DebuggerStepThroughAttribute]
            set { _address = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _address2;
        public string Address2
        {
            [DebuggerStepThroughAttribute]
            get { return _address2; }
            [DebuggerStepThroughAttribute]
            set { _address2 = value; }
        }

        public string FullAddress
        {
            [DebuggerStepThroughAttribute]
            get 
            {
                string address = _address;
                if (_address2.Length > 0)
                {
                    address += " " + _address2;
                }
                return address;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _postalCode;
        public string PostalCode
        {
            [DebuggerStepThroughAttribute]
            get { return _postalCode; }
            [DebuggerStepThroughAttribute]
            set { _postalCode = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _phone;
        public string Phone
        {
            [DebuggerStepThroughAttribute]
            get { return _phone; }
            [DebuggerStepThroughAttribute]
            set { _phone = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _locationDetails;
        public string LocationDetails
        {
            [DebuggerStepThroughAttribute]
            get { return _locationDetails; }
            [DebuggerStepThroughAttribute]
            set { _locationDetails = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _locationTypeId;
        public int LocationTypeId
        {
            [DebuggerStepThroughAttribute]
            get { return _locationTypeId; }
            [DebuggerStepThroughAttribute]
            set { _locationTypeId = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _portalId;
        public int PortalId
        {
            [DebuggerStepThroughAttribute]
            get { return _portalId; }
            [DebuggerStepThroughAttribute]
            set { _portalId = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _website;
        public string Website
        {
            [DebuggerStepThroughAttribute]
            get { return _website; }
            [DebuggerStepThroughAttribute]
            set { _website = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime _lastUpdateDate;
        public DateTime LastUpdateDate
        {

            get
            {
                if (_lastUpdateDate == DateTime.MinValue)
                {
                    return DateTime.Now;
                }
                else
                {
                    return _lastUpdateDate;
                }
            }
            [DebuggerStepThroughAttribute]
            set { _lastUpdateDate = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _csvLineNumber;
        public int CsvLineNumber
        {
            [DebuggerStepThroughAttribute]
            get { return _csvLineNumber; }
            [DebuggerStepThroughAttribute]
            set { _csvLineNumber = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _approved;
        public bool Approved
        {
            [DebuggerStepThroughAttribute]
            get { return _approved; }
            [DebuggerStepThroughAttribute]
            set { _approved = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float _averageRating;
        public float AverageRating
        {
            [DebuggerStepThrough]
            get { return _averageRating; }
            [DebuggerStepThrough]
            private set { _averageRating = value; }
        }

        #endregion

        #region Methods

        public void Save()
        {
            if (_locationId == -1)
            {
                _locationId = DataProvider.Instance().SaveLocation(this);
            }
        }

        public void SaveTemp(bool successful)
        {
            if (_locationId != -1)
            {
                _locationId = DataProvider.Instance().SaveTempLocation(this, successful);
            }
        }

        public void Update()
        {
            if (_locationId != -1)
            {
                _locationId = DataProvider.Instance().UpdateLocation(this);
            }
        }

        public DataSet GetComments(bool approved)
        {
            return DataProvider.Instance().GetComments(_locationId, approved);
        }

        public List<Attribute> GetAttributes()
        {
            return Attribute.GetAttributes(_locationTypeId, _locationId);
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
            Location location = Location.Load(DataProvider.Instance().GetLocation(locationId).Rows[0]);
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
            DataTable dtLocations = DataProvider.Instance().GetAllLocations(portalId, approved, "Name", 0, 0);
            List<Location> locations = new List<Location>();
            foreach (DataRow row in dtLocations.Rows)
            {
                Location loc = GetLocation(Convert.ToInt32(row["LocationId"]));
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

        //This method should be used for all instance creation so that this code is in one place! hk
        public static Location Load(DataRow row)
        {
            Location loc = new Location();

            loc.Address = row["Address"].ToString();
            loc.Address2 = row["Address2"].ToString();
            loc.City = row["City"].ToString();
            loc.LocationDetails = row["LocationDetails"].ToString();
            loc.ExternalIdentifier = row["ExternalIdentifier"].ToString();
            loc.Latitude = Convert.ToDouble(row["Latitude"]);
            loc.LocationId = Convert.ToInt32(row["LocationId"]);
            loc.LocationTypeId = Convert.ToInt32(row["LocationTypeId"]);
            loc.Longitude = Convert.ToDouble(row["Longitude"]);
            loc.Name = row["Name"].ToString();
            loc.Website = row["Website"].ToString();
            loc.Phone = row["Phone"].ToString();
            loc.PostalCode = row["PostalCode"].ToString();
            loc.StateName = row["StateName"].ToString();
            loc.RegionId = Convert.ToInt32(row["RegionId"].ToString());
            loc.Approved = Convert.ToBoolean(row["Approved"].ToString());
            loc.AverageRating = row["AverageRating"] is DBNull ? 0 : Convert.ToSingle(row["AverageRating"]);

            return loc;
        }

        #endregion


    }
}
