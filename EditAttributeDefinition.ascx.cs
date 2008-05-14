using System.IO;
using System.Xml;

using Globals = DotNetNuke.Common.Globals;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common.Lists;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;

using DotNetNuke.Services.Localization;

using DotNetNuke.UI.Utilities;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Common.Utilities;
using System;

namespace Engage.Dnn.Locator
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditLocationTypeDefinition PortalModuleBase is used to manage a LocationType Attribute
    /// for a portal
    /// </summary>
    /// -----------------------------------------------------------------------------
    partial class EditAttributeDefinition : DotNetNuke.Entities.Modules.PortalModuleBase
    {

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
            //cmdUpdate.Click += new EventHandler(cmdUpdate_Click);
            //cmdCancel.Click += new EventHandler(cmdCancel_Click);
            //cmdDelete.Click += new EventHandler(cmdDelete_Click);
        }

        #region "Private Members"

        private bool _IsAddMode = Null.NullBoolean;
        private LocationTypeAttributeDefinition _AttributeDefinition;
        //private string ResourceFile = "~/Admin/Users/App_LocalResources/LocationType.ascx";

        #endregion

        #region "Protected Members"

        protected object IsAddMode
        {
            get { return _IsAddMode; }
            set { _IsAddMode = (bool)value; }
        }

        protected bool IsList
        {
            get
            {
                bool _IsList = false;
                ListController objListController = new ListController();
                ListEntryInfo dataType = objListController.GetListEntryInfo(AttributeDefinition.DataType);

                if (((dataType != null)) && (dataType.ListName == "DataType") && (dataType.Value == "List"))
                {
                    _IsList = true;
                }

                return _IsList;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether we are dealing with SuperUsers
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected bool IsSuperUser
        {
            get
            {
                if (PortalSettings.ActiveTab.ParentId == PortalSettings.SuperTabId)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected LocationTypeAttributeDefinition AttributeDefinition
        {
            get
            {
                if (_AttributeDefinition == null)
                {
                    if ((bool)IsAddMode)
                    {
                        //Create New Attribute Definition
                        _AttributeDefinition = new LocationTypeAttributeDefinition();
                        _AttributeDefinition.PortalId = UsersPortalId;
                        _AttributeDefinition.LocationTypeId = LocationTypeId;
                    }
                    else
                    {
                        //Get Attribute Definition from Data Store
                        _AttributeDefinition = LocationTypeController.GetAttributeDefinition(AttributeDefinitionID, PortalId);
                    }
                }
                return _AttributeDefinition;
            }
        }

        protected int AttributeDefinitionID
        {
            get
            {
                int _DefinitionID = Null.NullInteger;
                if ((ViewState["AttributeDefinitionID"] != null))
                {
                    _DefinitionID = Int32.Parse(ViewState["AttributeDefinitionID"].ToString());
                }
                return _DefinitionID;
            }
            set { ViewState["AttributeDefinitionID"] = value; }
        }

        protected int LocationTypeId
        {
            get
            {
                int id = Null.NullInteger;
                if ((Request.QueryString["ltid"] != null))
                {
                    id = Int32.Parse(Request.QueryString["ltid"].ToString());
                }
                return id;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the Portal Id whose Users we are managing
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected int UsersPortalId
        {
            get
            {
                int intPortalId = PortalId;
                if (IsSuperUser)
                {
                    intPortalId = Null.NullInteger;
                }
                return intPortalId;
            }
        }

        #endregion

        #region "Event Handlers"

        private void Page_Init(object sender, System.EventArgs e)
        {
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                //Get Attribute Definition Id from Querystring (unless its been stored in the Viewstate)
                if (AttributeDefinitionID == Null.NullInteger)
                {
                    if ((Request.QueryString["AttributeDefinitionId"] != null))
                    {
                        AttributeDefinitionID = Int32.Parse(Request.QueryString["AttributeDefinitionId"]);
                    }
                    else
                    {
                        IsAddMode = true;
                    }
                }

                if ((Request.QueryString["ltid"] != null))
                {
                    cmdUpdate.Text = "Save";
                }

                if (!Page.IsPostBack)
                {
                    //Localization.LoadCultureDropDownList(cboLocales, CultureDropDownTypes.NativeName, ((DotNetNuke.Framework.PageBase)Page).PageCulture.Name);
                    //lblLocales.Text = cboLocales.SelectedItem.Text;
                    //cboLocales.Visible = !(cboLocales.Items.Count == 1);
                    //lblLocales.Visible = (cboLocales.Items.Count == 1);
                }

                //Add Delete Confirmation
                cmdDelete.Visible = true;
                ClientAPI.AddButtonConfirm(cmdDelete, Localization.GetString("DeleteAttribute"));

                //Bind Attribute Definition to Data Store
                Attributes.LocalResourceFile = this.LocalResourceFile;
                Attributes.DataSource = AttributeDefinition;
                Attributes.DataBind();
            }

            catch (Exception exc)
            {
                //Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// cmdUpdate_Click runs when the Delete button is clicked
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if ((bool)IsAddMode)
                {
                    LocationTypeController.AddAttributeDefinition(AttributeDefinition);
                    Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "AttributeDefinitions", "mid=" + ModuleId.ToString() + "&ltid=" + LocationTypeId));
                }
                else
                {
                    LocationTypeController.UpdateAttributeDefinition(AttributeDefinition);
                    Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "AttributeDefinitions", "mid=" + ModuleId.ToString() + "&ltid=" + LocationTypeId));
                }
            }
            catch (ModuleLoadException exc)
            {
                //Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// cmdCancel_Click runs when the Delete button is clicked
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "AttributeDefinitions", "mid=" + ModuleId.ToString() + "&ltid=" + LocationTypeId));
            }
            catch (ModuleLoadException exc)
            {
                //Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// cmdDelete_Click runs when the Delete button is clicked
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void cmdDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (AttributeDefinitionID != Null.NullInteger)
                {
                    //Declare Definition and "retrieve" it from the Attribute Editor
                    LocationTypeAttributeDefinition attributeDefinition;
                    attributeDefinition = (LocationTypeAttributeDefinition)Attributes.DataSource;

                    //Delete the Attribute Definition
                    LocationTypeController.DeleteAttributeDefinition(attributeDefinition);
                }

                //Redirect to Definitions page
                Response.Redirect(Globals.NavigateURL(TabId, "ManageLocationType", "mid=" + ModuleId), true);
            }
            catch (ModuleLoadException exc)
            {
                //Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

    }
}

