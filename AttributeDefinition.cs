//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using DotNetNuke.UI;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Entities.Modules;
using System.Xml.Serialization;
using System.ComponentModel;
using DotNetNuke.Entities.Users;
using DotNetNuke.Common.Utilities;

namespace Engage.Dnn.Locator
{
    /// <summary>
    /// The AttributeDefinition class provides a Business Layer entity for 
    /// property Definitions
    /// </summary>
    /// -----------------------------------------------------------------------------
    [XmlRoot("attributedefinition", IsNullable = false)]
    public class AttributeDefinition
    {

        #region Properties

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _dataType = Null.NullInteger;
        [Editor("DotNetNuke.UI.WebControls.DNNListEditControl, DotNetNuke", typeof(DotNetNuke.UI.WebControls.EditControl)), List("DataType", "", ListBoundField.Id, ListBoundField.Value), IsReadOnly(true), Required(true), SortOrder(1)]
        [XmlIgnore()]
        public int DataType
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _dataType; }
            set
            {
                if (_dataType != value)
                    _isDirty = true;
                _dataType = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Default Value of the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _defaultValue;
        [SortOrder(4)]
        [XmlIgnore()]
        public string DefaultValue
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _defaultValue; }
            set
            {
                if (_defaultValue != value)
                    _isDirty = true;
                _defaultValue = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether the Definition has been modified since it has been retrieved
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _isDirty;
        [Browsable(false)]
        [XmlIgnore()]
        public bool IsDirty
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _isDirty; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Length of the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _length;
        [SortOrder(3)]
        [XmlElement("length")]
        public int Length
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _length; }
            set
            {
                if (_length != value)
                    _isDirty = true;
                _length = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the LocationTypeId
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _locationTypeId = Null.NullInteger;
        [Browsable(false)]
        [XmlIgnore()]
        public int LocationTypeId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _locationTypeId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _locationTypeId = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the PortalId
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _portalId;
        [Browsable(false)]
        [XmlIgnore()]
        public int PortalId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _portalId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _portalId = value; }
        }
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Id of the LocationAttributeDefinition
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _attributeDefinitionId = Null.NullInteger;
        [Browsable(false)]
        [XmlIgnore()]
        public int AttributeDefinitionId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _attributeDefinitionId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _attributeDefinitionId = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Name of the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _name;
        [Required(true), IsReadOnly(true), SortOrder(0), RegularExpressionValidator("^[a-zA-Z0-9._%\\-+']+$")]
        [XmlElement("attributename")]
        public string AttributeName
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _name; }
            set
            {
                if (_name != value)
                    _isDirty = true;
                _name = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Value of the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _value;       
        [Browsable(false)]
        [XmlIgnore()]
        public string AttributeValue
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _value; }
            set
            {
                if (_value != value)
                    _isDirty = true;
                _value = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets whether the property is required
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _required;
        [SortOrder(6)]
        [XmlIgnore()]
        public bool Required
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _required; }
            set
            {
                if (_required != value)
                    _isDirty = true;
                _required = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets a Validation Expression (RegEx) for the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _validationExpression;
        [SortOrder(5)]
        [XmlIgnore()]
        public string ValidationExpression
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _validationExpression; }
            set
            {
                if (_validationExpression != value)
                    _isDirty = true;
                _validationExpression = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the View Order of the Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _viewOrder;
        [Required(true), SortOrder(8)]
        [XmlIgnore()]
        public int ViewOrder
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _viewOrder; }
            set
            {
                if (_viewOrder != value)
                    _isDirty = true;
                _viewOrder = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets whether the property is visible
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _visible;
        [SortOrder(7)]
        [XmlIgnore()]
        public bool Visible
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _visible; }
            set
            {
                if (_visible != value)
                    _isDirty = true;
                _visible = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets whether the property is visible
        /// </summary>
        /// -----------------------------------------------------------------------------
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private UserVisibilityMode _visibility = UserVisibilityMode.AdminOnly;
        [Browsable(false)]
        [XmlIgnore()]
        public UserVisibilityMode Visibility
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _visibility; }
            set
            {
                if (_visibility != value)
                    _isDirty = true;
                _visibility = value;
            }
        }

        #endregion

        #region Methods

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Clears the IsDirty Flag
        /// </summary>
        /// -----------------------------------------------------------------------------
        public void ClearIsDirty()
        {
            _isDirty = false;
        }


        public AttributeDefinition Clone()
        {

            AttributeDefinition objClone = new AttributeDefinition();
            objClone.DataType = this.DataType;
            objClone.DefaultValue = this.DefaultValue;
            objClone.Length = this.Length;
            objClone.LocationTypeId = this.LocationTypeId;
            objClone.PortalId = this.PortalId;
            //objClone.AttributeCategory = this.AttributeCategory;
            objClone.AttributeDefinitionId = this.AttributeDefinitionId;
            objClone.AttributeName = this.AttributeName;
            objClone.AttributeValue = this.AttributeValue;
            objClone.Required = this.Required;
            objClone.ValidationExpression = this.ValidationExpression;
            objClone.ViewOrder = this.ViewOrder;
            objClone.Visibility = this.Visibility;
            objClone.Visible = this.Visible;
            objClone.ClearIsDirty();

            return objClone;

        }
        #endregion

    }

}
