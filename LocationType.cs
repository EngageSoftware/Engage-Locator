using System.Data;
using Engage.Dnn.Locator.Data;

namespace Engage.Dnn.Locator
{
    public class LocationType
    {
        private int _locationId;
        private string _locationTypeName;

        public static DataTable GetLocationTypes()
        {
            return DataProvider.Instance().GetLocationTypes();
        }

        public static bool GetLocationTypeInUse(string location)
        {
            bool inuse = false;
            int count;

            count = DataProvider.Instance().GetLocationTypeCount(location);
            if (count > 0) inuse = true;

            return inuse;
        }

        public static string GetLocationTypeName(int id)
        {
            DataTable dt = DataProvider.Instance().GetLocationTypeName(id);
            string name = string.Empty;
            if(dt.Rows.Count == 1)
            {
                name = dt.Rows[0]["LocationTypeName"].ToString();
                
            }

            return name;
        }

        #region Properties

        public int LocationId
        {
            get { return _locationId; }
            set { _locationId = value; }
        }

        public string LocationTypeName
        {
            get { return _locationTypeName; }
            set { _locationTypeName = value; }
        }

        #endregion

    }
}