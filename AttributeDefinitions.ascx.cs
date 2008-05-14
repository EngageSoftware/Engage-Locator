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

namespace Engage.Dnn.Locator
{
    partial class AttributeDefinitions : PortalModuleBase, IActionable
    {
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
            //this.Load += new EventHandler(this.Page_Load);
            this.cmdUpdate.Click += new EventHandler(cmdUpdate_Click);
            this.cmdRefresh.Click += new EventHandler(cmdRefresh_Click);
        }

        #region Constants

        const int COLUMN_REQUIRED = 9;
        const int COLUMN_VISIBLE = 10;
        const int COLUMN_MOVE_DOWN = 2;
        const int COLUMN_MOVE_UP = 3;

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

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the Return Url for the page
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string ReturnUrl
        {
            get
            {
                string[] FilterParams = new string[(Request.QueryString["filterproperty"] == "" ? 2 : 3) + 1];

                if ((Request.QueryString["filterProperty"] == ""))
                {
                    FilterParams.SetValue("filter=" + Request.QueryString["filter"], 0);
                    FilterParams.SetValue("currentpage=" + Request.QueryString["currentpage"], 1);
                }
                else
                {
                    FilterParams.SetValue("filter=" + Request.QueryString["filter"], 0);
                    FilterParams.SetValue("filterProperty=" + Request.QueryString["filterProperty"], 1);
                    FilterParams.SetValue("currentpage=" + Request.QueryString["currentpage"], 2);
                }

                return DotNetNuke.Common.Globals.NavigateURL(TabId, "", FilterParams);
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

        #endregion

        #region Private Members
        //private LocationTypeAttributeDefinitionCollection m_objAttributes;
        private DataTable _objAttributes;
        #endregion

        #region Private Methods

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
        private void DeleteAttribute(int index)
        {
            AttributeDefinitionCollection locationTypeAttributes = GetAttributes();
            AttributeDefinition objAttribute = locationTypeAttributes[index];

            LocationType.DeleteAttributeDefinition(objAttribute);

            RefreshGrid();
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

        private void FillLocTypeName()
        {
            ////Fill textbox with LocType name here.
            DataTable dt = LocationType.GetLocationTypes();
            dt.Select("LocationTypeId = " + LocationTypeId);
            txtLocationTypeName.Text = dt.Rows[0][1].ToString();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Binds the Property Collection to the Grid
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void BindGrid()
        {
            FillLocTypeName();

            AttributeDefinitionCollection attributes = GetAttributes();
            bool allRequired = true;
            bool allVisible = true;

            //Check whether the checkbox column headers are true or false
            foreach (AttributeDefinition loctypeAttribute in attributes)
            {
                if (loctypeAttribute.Required == false)
                {
                    allRequired = false;
                }
                if (loctypeAttribute.Visible == false)
                {
                    allVisible = false;
                }

                if (!allRequired & !allVisible)
                {
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            foreach (DataGridColumn column in grdLocationTypeAttributes.Columns)
            {
                if (object.ReferenceEquals(column.GetType(), typeof(CheckBoxColumn)))
                {
                    //Manage CheckBox column events
                    CheckBoxColumn cbColumn = (CheckBoxColumn)column;
                    if (cbColumn.DataField == "Required")
                    {
                        cbColumn.Checked = allRequired;
                    }
                    if (cbColumn.DataField == "Visible")
                    {
                        cbColumn.Checked = allVisible;
                    }
                }
            }
            grdLocationTypeAttributes.DataSource = attributes;
            grdLocationTypeAttributes.DataBind();

        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Refresh the Property Collection to the Grid
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void RefreshGrid()
        {
            _objAttributes = null;
            BindGrid();
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
                    chk = (CheckBox)objItem.Cells[COLUMN_REQUIRED].Controls[0];
                    objAttribute.Required = chk.Checked;
                    chk = (CheckBox)objItem.Cells[COLUMN_VISIBLE].Controls[0];
                    objAttribute.Visible = chk.Checked;
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
            }
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
                if (!Page.IsPostBack)
                {
                    Localization.LocalizeDataGrid(ref grdLocationTypeAttributes, this.LocalResourceFile);
                    BindGrid();
                }
                else
                {
                    ProcessPostBack();
                }
            }
            catch (Exception exc)
            {
                //Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// cmdRefresh_Click runs when the refresh button is clciked
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void cmdRefresh_Click(object sender, System.EventArgs e)
        {
            RefreshGrid();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// cmdUpdate_Click runs when the update button is clciked
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void cmdUpdate_Click(object sender, System.EventArgs e)
        {
            try
            {
                Updateattributes();

                RefreshGrid();
            }
            catch (Exception exc)
            {
                //Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// grdLocationTypeAttributes_ItemCheckedChanged runs when a checkbox in the grid
        /// is clicked
        /// </summary>
        /// -----------------------------------------------------------------------------
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
                        ((WebControl)e.Item.Cells[COLUMN_REQUIRED].Controls[1]).Attributes.Add("onclick", "dnn.util.checkallChecked(this," + COLUMN_REQUIRED + ");");
                        ((CheckBox)e.Item.Cells[COLUMN_REQUIRED].Controls[1]).AutoPostBack = false;
                        ((WebControl)e.Item.Cells[COLUMN_VISIBLE].Controls[1]).Attributes.Add("onclick", "dnn.util.checkallChecked(this," + COLUMN_VISIBLE + ");");
                        ((CheckBox)e.Item.Cells[COLUMN_VISIBLE].Controls[1]).AutoPostBack = false;
                        break;
                    case ListItemType.AlternatingItem:
                    case ListItemType.Item:
                        ((CheckBox)e.Item.Cells[COLUMN_REQUIRED].Controls[0]).AutoPostBack = false;
                        ((CheckBox)e.Item.Cells[COLUMN_VISIBLE].Controls[0]).AutoPostBack = false;

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

        #endregion

        #region Optional Interfaces

        ModuleActionCollection IActionable.ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();
                Actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, LocalResourceFile), ModuleActionType.AddContent, "", "add.gif", EditUrl("ltid", LocationTypeId.ToString(), "EditAttributeDefinition"), false, DotNetNuke.Security.SecurityAccessLevel.Admin, true, false);

                Actions.Add(GetNextActionID(), Localization.GetString("Cancel.Action", LocalResourceFile), ModuleActionType.AddContent, "", "lt.gif", ReturnUrl, false, DotNetNuke.Security.SecurityAccessLevel.Admin, true, false );
                return Actions;
            }
        }
        
        #endregion

        protected void grdLocationTypeAttributes_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string commandName = e.CommandName;
            //int commandArgument = (int)e.CommandArgument;
            int index = e.Item.ItemIndex;

            switch (commandName)
            {
                case "Delete":
                    DeleteAttribute(index);
                    break;
                case "MoveUp":
                    MoveAttributeUp(index);
                    break;
                case "MoveDown":
                    MoveAttributeDown(index);
                    break;
            }
        }

    }

}
