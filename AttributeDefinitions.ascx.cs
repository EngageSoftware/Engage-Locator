//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using DotNetNuke.UI.WebControls;
using Globals = DotNetNuke.Common.Globals;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using DotNetNuke.Framework.Providers;
using Engage.Dnn.Locator.Data;

namespace Engage.Dnn.Locator
{
    partial class AttributeDefinitions : PortalModuleBase
    {
        #region Constants

        //const int COLUMN_REQUIRED = 7;
        //const int COLUMN_VISIBLE = 8;
        const int COLUMN_MOVE_DOWN = 3;
        const int COLUMN_MOVE_UP = 4;

        #endregion

        #region Protected Members
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

        protected int LocationTypeId
        {
            get
            {
                return Convert.ToInt32(lbLocType.SelectedValue);
            }
        }

        #endregion

        #region Private Methods

        private bool IsInEdit
        {
            get 
            {
                bool b = false;
                if ((ViewState["editMode"] != null))
                {
                    b = Convert.ToBoolean(ViewState["editMode"]);
                }
                return b;
            }
            set { ViewState["editMode"] = value; }

        }
        private void BindData()
        {
            foreach (DataGridColumn column in grdLocationTypeAttributes.Columns)
            {
                if (object.ReferenceEquals(column.GetType(), typeof(CheckBoxColumn)))
                {
                    if (SupportsRichClient() == false)
                    {
                        CheckBoxColumn cbColumn = (CheckBoxColumn)column;
                        cbColumn.CheckedChanged += grdLocationTypeAttributes_ItemCheckedChanged;
                    }
                }
                else if (object.ReferenceEquals(column.GetType(), typeof(ImageCommandColumn)))
                {
                    //Manage Delete Confirm JS
                    ImageCommandColumn imageColumn = (ImageCommandColumn)column;
                    switch (imageColumn.CommandName)
                    {
                        case "Delete":
                            imageColumn.OnClickJS = Localization.GetString("DeleteItem");
                            imageColumn.Text = Localization.GetString("Delete", this.LocalResourceFile);
                            break;
                        case "Edit":
                            //The Friendly URL parser does not like non-alphanumeric characters
                            //so first create the format string with a dummy value and then
                            //replace the dummy value with the FormatString place holder
                            string formatString = EditUrl("AttributeDefinitionID", "KEYFIELD", "EditAttributeDefinition", "ltid=" + LocationTypeId);
                            formatString = formatString.Replace("KEYFIELD", "{0}");
                            imageColumn.NavigateURLFormatString = formatString;
                            break;
                        case "MoveUp":
                            imageColumn.Text = Localization.GetString("MoveUp", this.LocalResourceFile);
                            break;
                        case "MoveDown":
                            imageColumn.Text = Localization.GetString("MoveDown", this.LocalResourceFile);
                            break;
                    }
                }

                BindGrid();
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the attributes 
        /// </summary>
        /// -----------------------------------------------------------------------------
        private AttributeDefinitionCollection GetAttributes()
        {
            return LocationType.GetAttributeDefinitionsById(LocationTypeId, false);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Helper function that determines whether the client-side functionality is possible
        /// </summary>
        /// -----------------------------------------------------------------------------
        private bool SupportsRichClient()
        {
            return DotNetNuke.UI.Utilities.ClientAPI.BrowserSupportsFunctionality(ClientAPI.ClientFunctionality.DHTML);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Deletes a property
        /// </summary>
        /// <param name="index">The index of the Property to delete</param>
        /// -----------------------------------------------------------------------------
        private void DeleteAttribute(int attributeDefinitionId)
        {
            AttributeDefinitionCollection locationTypeAttributes = GetAttributes();
            foreach (AttributeDefinition objAttribute in locationTypeAttributes)
            {
                if (objAttribute.AttributeDefinitionId == attributeDefinitionId)
                {
                    LocationType.DeleteAttributeDefinition(objAttribute);
                    break;
                }
            }

            BindGrid();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Moves a property
        /// </summary>
        /// <param name="index">The index of the Property to move</param>
        /// <param name="destIndex">The new index of the Property</param>
        /// -----------------------------------------------------------------------------
        private void MoveAttribute(int index, int destIndex)
        {
            AttributeDefinitionCollection locationTypeAttributes = GetAttributes();
            AttributeDefinition objAttribute = locationTypeAttributes[index];
            AttributeDefinition objNext = locationTypeAttributes[destIndex];

            int currentOrder = objAttribute.ViewOrder;
            int nextOrder = objNext.ViewOrder;

            //Swap ViewOrders
            objAttribute.ViewOrder = nextOrder;
            objNext.ViewOrder = currentOrder;

            //'Refresh Grid
            //locationTypeAttributes.Sort();
            BindGrid();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Moves a property down in the ViewOrder
        /// </summary>
        /// <param name="index">The index of the Property to move</param>
        /// -----------------------------------------------------------------------------
        private void MoveAttributeDown(int index)
        {
            MoveAttribute(index, index + 1);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Moves a property up in the ViewOrder
        /// </summary>
        /// <param name="index">The index of the Property to move</param>
        /// -----------------------------------------------------------------------------
        private void MoveAttributeUp(int index)
        {
            MoveAttribute(index, index - 1);
        }

        private void BindTypes()
        {
            DataTable dt= LocationType.GetLocationTypes();

            lbLocType.DataTextField = "LocationTypeName";
            lbLocType.DataValueField = "LocationTypeId";
            lbLocType.DataSource = dt;
            lbLocType.DataBind();

            if (dt.Rows.Count > 0) lbLocType.SelectedIndex = 0;

            SetButtons();
        }

        private void SetButtons()
        {

            bool inUse = LocationType.GetLocationTypeInUse(LocationTypeId.ToString());
            if (inUse)
            {
                btnDeleteLocationType.Enabled = false;
                btnDeleteLocationType.ImageUrl = "~/desktopmodules/EngageLocator/images/delete_disabled_bt.gif";
            }
            else
            {
                btnDeleteLocationType.Enabled = true;
                btnDeleteLocationType.ImageUrl = "~/desktopmodules/EngageLocator/images/cadelete.gif";
            }

            if (LocationType.GetLocationTypeName(LocationTypeId) == "Default")
            {
                btnDeleteLocationType.Enabled = false;
                btnDeleteLocationType.ImageUrl = "~/desktopmodules/EngageLocator/images/delete_disabled_bt.gif";
                btnEditLocationType.Enabled = false;
                btnEditLocationType.ImageUrl = "~/desktopmodules/EngageLocator/images/edit_disabled_bt.gif";
            }
            else if (inUse == false)
            {
                btnDeleteLocationType.Enabled = true;
                btnDeleteLocationType.ImageUrl = "~/desktopmodules/EngageLocator/images/cadelete.gif";
                btnEditLocationType.Enabled = true;
                btnEditLocationType.ImageUrl = "~/desktopmodules/EngageLocator/images/caedit.gif";
            }
            else
            {
                btnEditLocationType.Enabled = true;
                btnEditLocationType.ImageUrl = "~/desktopmodules/EngageLocator/images/caedit.gif";
            }
        }

        private void BindGrid()
        {
            AttributeDefinitionCollection attributes = GetAttributes();
            grdLocationTypeAttributes.DataSource = attributes;
            grdLocationTypeAttributes.DataBind();

        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Updates any "dirty" attributes
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void Updateattributes()
        {
            AttributeDefinitionCollection locationTypeAttributes = GetAttributes();
            foreach (AttributeDefinition objAttribute in locationTypeAttributes)
            {
                if (objAttribute.IsDirty)
                {
                    LocationType.UpdateAttributeDefinition(objAttribute);
                }
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// This method is responsible for taking in posted information from the grid and
        /// persisting it to the property definition collection
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void ProcessPostBack()
        {
            try
            {
                AttributeDefinitionCollection objAttributes = GetAttributes();
                string[] aryNewOrder = DotNetNuke.UI.Utilities.ClientAPI.GetClientSideReorder(this.grdLocationTypeAttributes.ClientID, this.Page);
                AttributeDefinition objAttribute;
                DataGridItem objItem;
                CheckBox chk;
                for (int i = 0; i <= this.grdLocationTypeAttributes.Items.Count - 1; i++)
                {
                    objItem = this.grdLocationTypeAttributes.Items[i];
                    objAttribute = objAttributes[i];
                    //chk = (CheckBox)objItem.Cells[COLUMN_REQUIRED].Controls[0];
                    //objAttribute.Required = chk.Checked;
                    //chk = (CheckBox)objItem.Cells[COLUMN_VISIBLE].Controls[0];
                    //objAttribute.Visible = chk.Checked;
                }
                //assign vieworder
                for (int i = 0; i <= aryNewOrder.Length - 1; i++)
                {
                    objAttributes[Int32.Parse(aryNewOrder[i])].ViewOrder = i;
                }
                objAttributes.Sort();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Public Methods

        public string DisplayDataType(AttributeDefinition definition)
        {
            string retValue = Null.NullString;
            ListController objListController = new ListController();
            ListEntryInfo definitionEntry = objListController.GetListEntryInfo(definition.DataType);

            if ((definitionEntry != null))
            {
                retValue = definitionEntry.Value;
            }

            return retValue;
        }
        #endregion

        #region Event Handlers

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Init runs when the control is initialised
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void Page_Init(object sender, System.EventArgs e)
        {
           Localization.LocalizeDataGrid(ref grdLocationTypeAttributes, this.LocalResourceFile);
           BindTypes();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                BindData();
                ClientAPI.AddButtonConfirm(btnDeleteLocationType, Localization.GetString("confirmDelete", LocalResourceFile));
            }
            catch (Exception exc)
            {
                //Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void grdLocationTypeAttributes_ItemCheckedChanged(object sender, DNNDataGridCheckChangedEventArgs e)
        {
            string propertyName = e.Field;
            bool propertyValue = e.Checked;
            bool isAll = e.IsAll;
            int index = e.Item.ItemIndex;

            AttributeDefinitionCollection attributes = GetAttributes();
            AttributeDefinition loctypeAttribute;

            if (isAll)
            {
                //Update All the attributes
                foreach (AttributeDefinition loctypeAttributeX in attributes)
                {
                    switch (propertyName)
                    {
                        case "Required":
                            loctypeAttributeX.Required = propertyValue;
                            break;
                        case "Visible":
                            loctypeAttributeX.Visible = propertyValue;
                            break;
                    }
                }
            }
            else
            {
                //Update the indexed property
                loctypeAttribute = attributes[index];
                switch (propertyName)
                {
                    case "Required":
                        loctypeAttribute.Required = propertyValue;
                        break;
                    case "Visible":
                        loctypeAttribute.Visible = propertyValue;
                        break;
                }
            }

            BindGrid();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// grdLocationTypeAttributes_ItemCommand runs when a Command event is raised in the
        /// Grid
        /// </summary>
        /// -----------------------------------------------------------------------------
        //protected void grdLocationTypeAttributes_ItemCommand(object source, DataGridCommandEventArgs e)
        //{
        //    string commandName = e.CommandName;
        //    //int commandArgument = (int)e.CommandArgument;
        //    int index = e.Item.ItemIndex;

        //    switch (commandName)
        //    {
        //        case "Delete":
        //            DeleteAttribute(index);
        //            break;
        //        case "MoveUp":
        //            MoveAttributeUp(index);
        //            break;
        //        case "MoveDown":
        //            MoveAttributeDown(index);
        //            break;
        //    }
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// When it is determined that the client supports a rich interactivity the grdLocationTypeAttributes_ItemCreated 
        /// event is responsible for disabling all the unneeded AutoPostBacks, along with assiging the appropriate
        ///	client-side script for each event handler
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected void grdLocationTypeAttributes_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (SupportsRichClient())
            {
                switch (e.Item.ItemType)
                {
                    case ListItemType.Header:
                        //we combined the header label and checkbox in same place, so it is control 1 instead of 0
                        //((WebControl)e.Item.Cells[COLUMN_REQUIRED].Controls[1]).Attributes.Add("onclick", "dnn.util.checkallChecked(this," + COLUMN_REQUIRED + ");");
                        //((CheckBox)e.Item.Cells[COLUMN_REQUIRED].Controls[1]).AutoPostBack = false;
                        //((WebControl)e.Item.Cells[COLUMN_VISIBLE].Controls[1]).Attributes.Add("onclick", "dnn.util.checkallChecked(this," + COLUMN_VISIBLE + ");");
                        //((CheckBox)e.Item.Cells[COLUMN_VISIBLE].Controls[1]).AutoPostBack = false;
                        break;
                    case ListItemType.AlternatingItem:
                    case ListItemType.Item:
                        //((CheckBox)e.Item.Cells[COLUMN_REQUIRED].Controls[0]).AutoPostBack = false;
                        //((CheckBox)e.Item.Cells[COLUMN_VISIBLE].Controls[0]).AutoPostBack = false;

                        DotNetNuke.UI.Utilities.ClientAPI.EnableClientSideReorder(e.Item.Cells[COLUMN_MOVE_DOWN].Controls[0], this.Page, false, this.grdLocationTypeAttributes.ClientID);
                        DotNetNuke.UI.Utilities.ClientAPI.EnableClientSideReorder(e.Item.Cells[COLUMN_MOVE_UP].Controls[0], this.Page, true, this.grdLocationTypeAttributes.ClientID);
                        break;
                }
            }
        }

        protected void grdLocationTypeAttributes_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            DataGridItem item = e.Item;

            if (item.ItemType == ListItemType.Item | item.ItemType == ListItemType.AlternatingItem | item.ItemType == ListItemType.SelectedItem)
            {

                System.Web.UI.Control imgColumnControl = item.Controls[1].Controls[0];
                if (imgColumnControl is ImageButton)
                {
                    ImageButton delImage = (ImageButton)imgColumnControl;
                    AttributeDefinition loctypeAttribute = (AttributeDefinition)item.DataItem;

                    switch (loctypeAttribute.AttributeName.ToLower())
                    {
                        case "lastname":
                        case "firstname":
                        case "timezone":
                        case "preferredlocale":
                            delImage.Visible = false;
                            break;
                        default:
                            delImage.Visible = true;
                            break;
                    }
                }
            }
        }

        protected void grdLocationTypeAttributes_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string commandName = e.CommandName;
            //int commandArgument = (int)e.CommandArgument;
            int index = e.Item.ItemIndex;

            switch (commandName)
            {
                case "Delete":
                    Label l = ((Label)grdLocationTypeAttributes.Items[e.Item.ItemIndex].Cells[0].Controls[1]);
                    int attributeDefinitionId = Convert.ToInt32(l.Text);
                    DeleteAttribute(attributeDefinitionId);
                    break;
                case "MoveUp":
                    MoveAttributeUp(index);
                    break;
                case "MoveDown":
                    MoveAttributeDown(index);
                    break;
            }
        }

        protected void lbLocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
            SetButtons();
        }

        protected void btnCAAdd_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string href = EditUrl("ltid", LocationTypeId.ToString(), "EditAttributeDefinition");
            Response.Redirect(href, true);
        }


        protected void cmdUpdate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ProcessPostBack();
            try
            {
                Updateattributes();

                BindGrid();
            }
            catch (Exception exc)
            {
                //Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }

        protected void cmdCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(TabId), true);
        }

        protected void btnCreateLocationType_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            lbLocType.Enabled = false;
            btnCreateLocationType.Visible = false;
            dvLocationType.Visible = true;
            txtEditLocationType.Text = String.Empty;
            txtEditLocationType.Focus();
        }

        protected void btnEditLocationType_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            dvLocationType.Visible = true;
            txtEditLocationType.Text = LocationType.GetLocationTypeName(LocationTypeId);
            txtEditLocationType.Focus();
            lbLocType.Enabled = false;
            IsInEdit = true;
        }

        protected void btnDeleteLocationType_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            DataProvider.Instance().DeleteLocationType(Convert.ToInt32(lbLocType.SelectedItem.Value));
            BindTypes();
            BindData();
        }

        protected void btnUpdateLocationType_Click(object sender, EventArgs e)
        {
            if (IsInEdit)
            {
                DataProvider.Instance().UpdateLocationType(Convert.ToInt32(lbLocType.SelectedItem.Value), txtEditLocationType.Text);
            }
            else
            {
                DataProvider.Instance().InsertLocationType(txtEditLocationType.Text);
            }
            dvLocationType.Visible = false;
            BindTypes();
            btnCreateLocationType.Visible = true;
            lbLocType.Enabled = true;
            IsInEdit = false;
        }

        protected void btnCancelLocationType_Click(object sender, EventArgs e)
        {
            btnCreateLocationType.Visible = true;
            dvLocationType.Visible = false;
            lbLocType.Enabled = true;
            IsInEdit = false;
        }


        #endregion
    }

}
