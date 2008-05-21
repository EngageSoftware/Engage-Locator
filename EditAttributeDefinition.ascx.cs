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
            base.OnInit(e);
            divDelete.Visible = ((bool)IsAddMode == false);
        }


        #region Private Members

        
        //private string ResourceFile = "~/Admin/Users/App_LocalResources/LocationType.ascx";

        #endregion

        #region Protected Members

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        protected bool IsAddMode
        {
            get { return Request.QueryString["AttributeDefinitionID"] == null; }
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
        private AttributeDefinition _attributeDefinition;
        protected AttributeDefinition AttributeDefinition
        {
            get
            {
                //_attributeDefinition = LocationType.GetAttributeDefinition(AttributeDefinitionId, LocationTypeId);
                if (_attributeDefinition == null)
                {
                    if ((bool)IsAddMode)
                    {
                        //Create New Attribute Definition
                        _attributeDefinition = new AttributeDefinition();
                        _attributeDefinition.PortalId = UsersPortalId;
                        _attributeDefinition.LocationTypeId = LocationTypeId;
                        ListController controller = new ListController();
                        ListEntryInfo info = controller.GetListEntryInfo("DataType", "Text");
                        _attributeDefinition.DataType = info.EntryID;
                        _attributeDefinition.AttributeName = txtName.Text;
                        _attributeDefinition.DefaultValue = txtDefaultValue.Text;
                        _attributeDefinition.ViewOrder = LocationType.GetAttributeDefinitions(LocationTypeId).Count;
                    }
                    else
                    {
                        //Get Attribute Definition from Data Store
                        _attributeDefinition = LocationType.GetAttributeDefinition(AttributeDefinitionId, LocationTypeId);
                    }
                }
                return _attributeDefinition;
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
                        cmdDelete.Visible = false;
                    }
                }


                if (!Page.IsPostBack)
                {
                    //Add Delete Confirmation
                    cmdDelete.Visible = true;
                    ClientAPI.AddButtonConfirm(cmdDelete, Localization.GetString("DeleteAttribute", LocalResourceFile));

                    BindData();
                }

            }

            catch (Exception exc)
            {
                //Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void cmdUpdate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                AttributeDefinition.AttributeName = txtName.Text;
                AttributeDefinition.DefaultValue = txtDefaultValue.Text;

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

        protected void cmdCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
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

        protected void cmdDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                if (AttributeDefinitionId != Null.NullInteger)
                {
                    //Delete the Attribute Definition
                    LocationType.DeleteAttributeDefinition(AttributeDefinition);
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

        private void BindData()
        {
            _attributeDefinition = LocationType.GetAttributeDefinition(AttributeDefinitionId, LocationTypeId);
            txtName.Text = AttributeDefinition.AttributeName;
            txtDefaultValue.Text = AttributeDefinition.DefaultValue;
        }

    }
}

