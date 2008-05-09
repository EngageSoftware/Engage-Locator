using System;
using System.Collections.Generic;
using System.Data;
using Engage.Dnn.Locator.Data;

namespace Engage.Dnn.Locator
{
    public class Location
    {
        private int locationId = -1;
        private string externalIdentifier;
        private string name;
        private double latitude;
        private double longitude;
        private int countryId;
        private int regionId;
        private string stateName;
        private string city;
        private string address;
        private string postalCode;
        private string phone;
        private string locationDetails;
        private int locationTypeId;
        private int portalId;
        private string website;
        private DateTime lastUpdateDate;
        private int csvLineNumber;
        private bool _approved;

        public void Save()
        {
            if (locationId == -1)
            {
                locationId = DataProvider.Instance().SaveLocation(this);
            }
        }

        public void SaveTemp(bool successful)
        {
            if (locationId != -1)
            {
                locationId = DataProvider.Instance().SaveTempLocation(this, successful);
            }
        }

        public void Update()
        {
            if (locationId != -1)
            {
                locationId = DataProvider.Instance().UpdateLocation(this);
            }
        }

        public DataSet GetLocationComments(bool approved)
        {
            return DataProvider.Instance().GetLocationComments(locationId, approved);
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
            Location location = Location.Create(DataProvider.Instance().GetLocation(locationId).Rows[0]);
            return location;
        }

        public static DataTable GetLocations(int portalId, int approved)
        {
            return DataProvider.Instance().GetAllLocations(portalId, Convert.ToInt32(approved));
        }

        public static void DeleteLocation(int locationId)
        {
            DataProvider.Instance().DeleteLocation(locationId);
        }

        public static List<Location> GetSearchLocations(int portalId, int approved)
        {
            DataTable dtLocations = DataProvider.Instance().GetAllLocations(portalId, Convert.ToInt32(approved));
            List<Location> locations = new List<Location>();
            foreach (DataRow row in dtLocations.Rows)
            {
                Location loc = GetLocation(Convert.ToInt32(row["LocationId"]));
                locations.Add(loc);
            }
            return locations;
        }

        public static void InsertLocationComment(int locationId, string comment, string submittedBy, bool approved)
        {
            DataProvider.Instance().InsertLocationComment(locationId, comment, submittedBy, approved);
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

        public DataTable GetLocationAttributes()
        {
            return DataProvider.Instance().GetLocationAttributes(this.LocationTypeId);
        }

        public static string GetLocationAttributeValue(int attributeDefinitionId, int locationId)
        {
            return DataProvider.Instance().GetLocationAttributeValue(attributeDefinitionId, locationId);
        }

        public void UpdateLocationAttribute(int locationAttributeId, string attributeValue)
        {
            DataProvider.Instance().UpdateLocationAttribute(locationAttributeId, this.locationId, attributeValue);
        }

        #region Properties
        public int LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }

        public string ExternalIdentifier
        {
            get { return externalIdentifier; }
            set { externalIdentifier = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public int CountryId
        {
            get { return countryId; }
            set { countryId = value; }
        }

        public int RegionId
        {
            get { return regionId; }
            set { regionId = value; }
        }

        public string StateName
        {
            get { return stateName; }
            set { stateName = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string LocationDetails
        {
            get { return locationDetails; }
            set { locationDetails = value; }
        }

        public int LocationTypeId
        {
            get { return locationTypeId; }
            set { locationTypeId = value; }
        }

        public int PortalId
        {
            get { return portalId; }
            set { portalId = value; }
        }

        public string Website
        {
            get { return website; }
            set { website = value; }
        }

        public DateTime LastUpdateDate
        {

            get
            {
                if (lastUpdateDate == DateTime.MinValue)
                {
                    return DateTime.Now;
                }
                else
                {
                    return lastUpdateDate;
                }
            }
            
            set { lastUpdateDate = value; }
        }

        public int CsvLineNumber
        {
            get { return csvLineNumber; }
            set { csvLineNumber = value; }
        }

        public bool Approved
        {
            get { return _approved; }
            set { _approved = value; }
        }

        #endregion
    }
}
