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
using Engage.Dnn.Locator.Data;

namespace Engage.Dnn.Locator
{
    public class Location
    {

        #region Properties

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _locationId = -1;
        public int LocationId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _locationId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _locationId = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _externalIdentifier;
        public string ExternalIdentifier
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _externalIdentifier; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _externalIdentifier = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _name;
        public string Name
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _name; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _name = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private double _latitude;
        public double Latitude
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _latitude; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _latitude = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private double _longitude;
        public double Longitude
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _longitude; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _longitude = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _countryId;
        public int CountryId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _countryId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _countryId = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _regionId;
        public int RegionId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _regionId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _regionId = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _stateName;
        public string StateName
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _stateName; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _stateName = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _city;
        public string City
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _city; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _city = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _address;
        public string Address
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _address; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _address = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _postalCode;
        public string PostalCode
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _postalCode; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _postalCode = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _phone;
        public string Phone
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _phone; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _phone = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _locationDetails;
        public string LocationDetails
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _locationDetails; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _locationDetails = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _locationTypeId;
        public int LocationTypeId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _locationTypeId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _locationTypeId = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _portalId;
        public int PortalId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _portalId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _portalId = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _website;
        public string Website
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _website; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _website = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
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
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _lastUpdateDate = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _csvLineNumber;
        public int CsvLineNumber
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _csvLineNumber; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _csvLineNumber = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _approved;
        public bool Approved
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _approved; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _approved = value; }
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
            Location location = Location.Create(DataProvider.Instance().GetLocation(locationId).Rows[0]);
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
        public static Location Create(DataRow row)
        {
            Location loc = new Location();

            loc.Address = row["Address"].ToString();
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

            return loc;
        }

        #endregion


    }
}
