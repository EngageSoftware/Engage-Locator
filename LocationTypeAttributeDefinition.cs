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
    /// The LocationAttributeDefinition class provides a Business Layer entity for 
    /// property Definitions
    /// </summary>
    /// -----------------------------------------------------------------------------
    [XmlRoot("attributedefinition", IsNullable = false)]
    public class LocationTypeAttributeDefinition
    {

        #region "Private Members"

        private int _DataType = Null.NullInteger;
        private string _DefaultValue;
        private bool _IsDirty;
        private int _Length;
        private int _LocationTypeId = Null.NullInteger;
        private int _PortalId;
        private int _AttributeDefinitionId = Null.NullInteger;
        private string _AttributeName;
        private string _AttributeValue;
        private bool _Required;
        private string _ValidationExpression;
        private int _ViewOrder;
        private bool _Visible;
        private UserVisibilityMode _Visibility = UserVisibilityMode.AdminOnly;

        #endregion

        public LocationTypeAttributeDefinition()
        {
        }

        #region "Public Properties"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Data Type of the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [Editor("DotNetNuke.UI.WebControls.DNNListEditControl, DotNetNuke", typeof(DotNetNuke.UI.WebControls.EditControl)), List("DataType", "", ListBoundField.Id, ListBoundField.Value), IsReadOnly(true), Required(true), SortOrder(1)]
        [XmlIgnore()]
        public int DataType
        {
            get { return _DataType; }
            set
            {
                if (_DataType != value)
                    _IsDirty = true;
                _DataType = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Default Value of the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [SortOrder(4)]
        [XmlIgnore()]
        public string DefaultValue
        {
            get { return _DefaultValue; }
            set
            {
                if (_DefaultValue != value)
                    _IsDirty = true;
                _DefaultValue = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether the Definition has been modified since it has been retrieved
        /// </summary>
        /// -----------------------------------------------------------------------------
        [Browsable(false)]
        [XmlIgnore()]
        public bool IsDirty
        {
            get { return _IsDirty; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Length of the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [SortOrder(3)]
        [XmlElement("length")]
        public int Length
        {
            get { return _Length; }
            set
            {
                if (_Length != value)
                    _IsDirty = true;
                _Length = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the LocationTypeId
        /// </summary>
        /// -----------------------------------------------------------------------------
        [Browsable(false)]
        [XmlIgnore()]
        public int LocationTypeId
        {
            get { return _LocationTypeId; }
            set { _LocationTypeId = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the PortalId
        /// </summary>
        /// -----------------------------------------------------------------------------
        [Browsable(false)]
        [XmlIgnore()]
        public int PortalId
        {
            get { return _PortalId; }
            set { _PortalId = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Category of the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        //[Required(true), SortOrder(2)]
        //[XmlElement("attributecategory")]
        //public string AttributeCategory
        //{
        //    get { return _AttributeCategory; }
        //    set
        //    {
        //        if (_AttributeCategory != value)
        //            _IsDirty = true;
        //        _AttributeCategory = value;
        //    }
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Id of the LocationAttributeDefinition
        /// </summary>
        /// -----------------------------------------------------------------------------
        [Browsable(false)]
        [XmlIgnore()]
        public int AttributeDefinitionId
        {
            get { return _AttributeDefinitionId; }
            set { _AttributeDefinitionId = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Name of the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [Required(true), IsReadOnly(true), SortOrder(0), RegularExpressionValidator("^[a-zA-Z0-9._%\\-+']+$")]
        [XmlElement("attributename")]
        public string AttributeName
        {
            get { return _AttributeName; }
            set
            {
                if (_AttributeName != value)
                    _IsDirty = true;
                _AttributeName = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the Value of the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [Browsable(false)]
        [XmlIgnore()]
        public string AttributeValue
        {
            get { return _AttributeValue; }
            set
            {
                if (_AttributeValue != value)
                    _IsDirty = true;
                _AttributeValue = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets whether the property is required
        /// </summary>
        /// -----------------------------------------------------------------------------
        [SortOrder(6)]
        [XmlIgnore()]
        public bool Required
        {
            get { return _Required; }
            set
            {
                if (_Required != value)
                    _IsDirty = true;
                _Required = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets a Validation Expression (RegEx) for the Profile Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [SortOrder(5)]
        [XmlIgnore()]
        public string ValidationExpression
        {
            get { return _ValidationExpression; }
            set
            {
                if (_ValidationExpression != value)
                    _IsDirty = true;
                _ValidationExpression = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the View Order of the Property
        /// </summary>
        /// -----------------------------------------------------------------------------
        [Required(true), SortOrder(8)]
        [XmlIgnore()]
        public int ViewOrder
        {
            get { return _ViewOrder; }
            set
            {
                if (_ViewOrder != value)
                    _IsDirty = true;
                _ViewOrder = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets whether the property is visible
        /// </summary>
        /// -----------------------------------------------------------------------------
        [SortOrder(7)]
        [XmlIgnore()]
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                if (_Visible != value)
                    _IsDirty = true;
                _Visible = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets whether the property is visible
        /// </summary>
        /// -----------------------------------------------------------------------------
        [Browsable(false)]
        [XmlIgnore()]
        public UserVisibilityMode Visibility
        {
            get { return _Visibility; }
            set
            {
                if (_Visibility != value)
                    _IsDirty = true;
                _Visibility = value;
            }
        }

        #endregion

        #region "Public Methods"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Clears the IsDirty Flag
        /// </summary>
        /// -----------------------------------------------------------------------------
        public void ClearIsDirty()
        {
            _IsDirty = false;
        }


        public LocationTypeAttributeDefinition Clone()
        {

            LocationTypeAttributeDefinition objClone = new LocationTypeAttributeDefinition();
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
