//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

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
    using System.Globalization;

    public class Attribute
    {
        #region Properties

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _attributeId;
        public int AttributeId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _attributeId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _attributeId = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _locationId;
        public int LocationId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _locationId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _locationId = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _attributeDefinitionId;
        public int AttributeDefinitionId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _attributeDefinitionId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _attributeDefinitionId = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _attributeName;
        public string AttributeName
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _attributeName; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _attributeName = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _attributeValue;
        public string AttributeValue
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _attributeValue; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _attributeValue = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private DateTime _createDate;
        public DateTime CreateDate
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _createDate; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _createDate = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private DateTime _revisionDate;
        public DateTime RevisionDate
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _revisionDate; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _revisionDate = value; }
        }

        #endregion

        public static List<Attribute> GetAttributes(int locationTypeId, int locationId)
        {
            DataTable dtLocationAttributeDefinitions = DataProvider.Instance().GetAttributeDefinitions(locationTypeId);
            DataTable dtLocationAttributeValues = DataProvider.Instance().GetAttributeValues(locationId);
            List<Attribute> locationAttributes = new List<Attribute>();
            foreach (DataRow row in dtLocationAttributeDefinitions.Rows)
            {
                Attribute attribute = new Attribute();
                attribute.AttributeDefinitionId = Convert.ToInt32(row["AttributeDefinitionId"], CultureInfo.InvariantCulture);
                attribute.AttributeName = row["AttributeName"].ToString();
                attribute.LocationId = locationId;
                attribute.AttributeValue = row["DefaultValue"].ToString(); //default this and change later if needed.hkh
                foreach (DataRow valueRow in dtLocationAttributeValues.Rows)
                {
                    if (attribute.LocationId == Convert.ToInt32(valueRow["LocationId"], CultureInfo.InvariantCulture) && attribute.AttributeDefinitionId == Convert.ToInt32(valueRow["AttributeDefinitionId"], CultureInfo.InvariantCulture))
                    {
                        if (valueRow["LocationAttributeId"] != null)
                        {
                            attribute._attributeId = Convert.ToInt32(valueRow["LocationAttributeId"], CultureInfo.InvariantCulture);
                            attribute._attributeValue = valueRow["AttributeValue"].ToString();
                            attribute._createDate = Convert.ToDateTime(valueRow["CreateDate"], CultureInfo.InvariantCulture);
                            attribute._revisionDate = Convert.ToDateTime(valueRow["RevisionDate"], CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            attribute._attributeId = -1;
                            attribute._attributeValue = string.Empty;
                        }
                    }
                }
                locationAttributes.Add(attribute);
            }
            return locationAttributes;
        }

        public static void AddAttribute(int attributeDefinitionId, int locationId, string attributeValue)
        {
            DataProvider.Instance().AddAttribute(attributeDefinitionId, locationId, attributeValue);
        }

        public static void UpdateAttribute(int locationAttributeId, int locationId, string attributeValue)
        {
            DataProvider.Instance().UpdateAttribute(locationAttributeId, locationId, attributeValue);
        }
    }
}
