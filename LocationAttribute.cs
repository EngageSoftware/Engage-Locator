using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Engage.Dnn.Locator.Data;

namespace Engage.Dnn.Locator
{
    public class LocationAttribute
    {
        private int _locationAttributeId;
        private int _locationId;
        private int _attributeDefinitionId;
        private string _attributeName;
        private string _attributeValue;
        private DateTime _createDate;
        private DateTime _revisionDate;

        #region Properties

        public int LocationAttributeId
        {
            get { return _locationAttributeId; }
            set { _locationAttributeId = value; }
        }

        public int LocationId
        {
            get { return _locationId; }
            set { _locationId = value; }
        }

        public int AttributeDefinitionId
        {
            get { return _attributeDefinitionId; }
            set { _attributeDefinitionId = value; }
        }

        public string AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }

        public string AttributeValue
        {
            get { return _attributeValue; }
            set { _attributeValue = value; }
        }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public DateTime RevisionDate
        {
            get { return _revisionDate; }
            set { _revisionDate = value; }
        }

        #endregion

        public static List<LocationAttribute> GetLocationAttributes(int locationTypeId, int locationId)
        {
            DataTable dtLocationAttributeDefinitions = DataProvider.Instance().GetAttributeDefinitions(locationTypeId);
            DataTable dtLocationAttributeValues = DataProvider.Instance().GetAttributeValues(locationId);
            List<LocationAttribute> locationAttributes = new List<LocationAttribute>();
            foreach (DataRow row in dtLocationAttributeDefinitions.Rows)
            {
                LocationAttribute attribute = new LocationAttribute();
                attribute.AttributeDefinitionId = Convert.ToInt32(row["AttributeDefinitionId"]);
                attribute.AttributeName = row["AttributeName"].ToString();
                attribute.LocationId = locationId;
                foreach (DataRow valueRow in dtLocationAttributeValues.Rows)
                {
                    if(attribute.LocationId == Convert.ToInt32(valueRow["LocationId"]) && attribute.AttributeDefinitionId == Convert.ToInt32(valueRow["AttributeDefinitionId"]))
                    {
                        if (valueRow["LocationAttributeId"] != null)
                        {
                            attribute._locationAttributeId = Convert.ToInt32(valueRow["LocationAttributeId"]);
                            attribute._attributeValue = valueRow["AttributeValue"].ToString();
                            attribute._createDate = Convert.ToDateTime(valueRow["CreateDate"]);
                            attribute._revisionDate = Convert.ToDateTime(valueRow["RevisionDate"]);
                        }
                        else
                        {
                            attribute._locationAttributeId = -1;
                            attribute._attributeValue = string.Empty;
                        }
                    }
                }
                locationAttributes.Add(attribute);
            }
            return locationAttributes;
        }

        public static void AddLocationAttribute(int attributeDefinitionId, int locationId, string attributeValue)
        {
            DataProvider.Instance().AddAttribute(attributeDefinitionId, locationId, attributeValue);
        }

        public static void UpdateLocationAttribute(int locationAttributeId, int locationId, string attributeValue)
        {
            DataProvider.Instance().UpdateAttribute(locationAttributeId, locationId, attributeValue);
        }
    }
}
