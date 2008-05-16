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
            cmdUpdate.Click += new EventHandler(cmdUpdate_Click);
            cmdCancel.Click += new EventHandler(cmdCancel_Click);
            cmdDelete.Click += new EventHandler(cmdDelete_Click);
        }

        #region Private Members

        
        //private string ResourceFile = "~/Admin/Users/App_LocalResources/LocationType.ascx";

        #endregion

        #region Protected Members

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _IsAddMode = Null.NullBoolean;
        protected object IsAddMode
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _IsAddMode; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
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
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private AttributeDefinition _AttributeDefinition;
        protected AttributeDefinition AttributeDefinition
        {
            get
            {
                if (_AttributeDefinition == null)
                {
                    if ((bool)IsAddMode)
                    {
                        //Create New Attribute Definition
                        _AttributeDefinition = new AttributeDefinition();
                        _AttributeDefinition.PortalId = UsersPortalId;
                        _AttributeDefinition.LocationTypeId = LocationTypeId;
                    }
                    else
                    {
                        //Get Attribute Definition from Data Store
                        _AttributeDefinition = LocationType.GetAttributeDefinition(AttributeDefinitionId, LocationTypeId);
                    }
                }
                return _AttributeDefinition;
            }
        }

        protected int AttributeDefinitionId
        {
            get
            {
                int id = Null.NullInteger;
                if ((ViewState["AttributeDefinitionID"] != null))
                {
                    id = Int32.Parse(ViewState["AttributeDefinitionID"].ToString());
                }
                if (Request.QueryString["AttributeDefinitionId"] != null)
                {
                    id = Convert.ToInt32(Request.QueryString["AttributeDefinitionId"]);
                }
                return id;
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

        protected int UsersPortalId
        {
            get
            {
                int id = PortalId;
                if (IsSuperUser)
                {
                    id = Null.NullInteger;
                }
                return id;
            }
        }

        #endregion

        #region Event Handlers

        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                //Get Attribute Definition Id from Querystring (unless its been stored in the Viewstate)
                if (AttributeDefinitionId == Null.NullInteger)
                {
                    if ((Request.QueryString["AttributeDefinitionId"] != null))
                    {
                        AttributeDefinitionId = Int32.Parse(Request.QueryString["AttributeDefinitionId"]);
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

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if ((bool)IsAddMode)
                {
                    LocationType.AddAttributeDefinition(AttributeDefinition);
                    Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "AttributeDefinitions", "mid=" + ModuleId.ToString() + "&ltid=" + LocationTypeId));
                }
                else
                {
                    LocationType.UpdateAttributeDefinition(AttributeDefinition);
                    Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "AttributeDefinitions", "mid=" + ModuleId.ToString() + "&ltid=" + LocationTypeId));
                }
            }
            catch (ModuleLoadException exc)
            {
                //Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
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

        protected void cmdDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (AttributeDefinitionId != Null.NullInteger)
                {
                    //Declare Definition and "retrieve" it from the Attribute Editor
                    AttributeDefinition attributeDefinition;
                    attributeDefinition = (AttributeDefinition)Attributes.DataSource;

                    //Delete the Attribute Definition
                    LocationType.DeleteAttributeDefinition(attributeDefinition);
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

