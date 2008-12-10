//Engage: Locator - http://www.engagemodules.com
//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.Utilities;
using Engage.Dnn.Locator.Data;
using Engage.Dnn.Locator;
using Engage.Dnn.Locator.Maps;
using FileInfo = DotNetNuke.Services.FileSystem.FileInfo;
using Globals = DotNetNuke.Common.Globals;

namespace Engage.Dnn.Locator
{
    partial class ManageLocations : ModuleBase
    {
        protected TextEditor teLocationDetails;
        private const int checkboxColumn = 9;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }
            this.rbWaitingForApproval.CheckedChanged += new EventHandler(rbApproved_CheckChanged);
            this.rbApproved.CheckedChanged += new EventHandler(rbApproved_CheckChanged);

            lbSettings.Visible = IsEditable;
            lbImportFile.Visible = IsEditable;
            lbManageComments.Visible = IsEditable;
            lbLocationTypes.Visible = IsEditable;

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // Global navigation
            if (UserInfo.IsSuperUser)
            {
                lbManageLocations.Enabled = false;
                lbManageLocations.CssClass = "mnavDisabled";
                lbManageLocations.ImageUrl = "~/desktopmodules/EngageLocator/images/locationBtDisabled.gif";
            }

            string error = String.Empty;

            if (MainDisplay.IsConfigured(TabModuleId, ref error))
            {
                if (!Page.IsPostBack)
                {
                    Bind();
                }
            }
            else
            {
                lblConfigured.Visible = true;
                lblConfigured.Text = lblConfigured.Text + " " + error;
            }
        }

        private void Bind()
        {
            Localization.LocalizeDataGrid(ref dgLocations, LocalResourceFile);
            lblConfigured.Visible = false;
            divPanelTab.Visible = true;

            if (Approved)
            {
                rbApproved.Checked = true;
            }
            else
            {
                rbWaitingForApproval.Checked = true;
                btnReject.Visible = true;
                btnAccept.Visible = true;
            }

            BindData(Approved, "Name");
            ClientAPI.AddButtonConfirm(btnReject, Localization.GetString("confirmDeleteLocation"));
        }

        private void BindData(bool approved, string sortColumn)
        {
            DataTable locations = Location.GetLocations(PortalId, approved, sortColumn, CurrentPageIndex - 1, dgLocations.PageSize);
            dgLocations.DataSource = locations;
            dgLocations.DataBind();

            if (locations.Rows.Count > 0)
            {
                pager.TotalRecords = Convert.ToInt32(locations.Rows[0]["TotalRecords"], CultureInfo.InvariantCulture);
                pager.PageSize = dgLocations.PageSize;
                pager.CurrentPage = CurrentPageIndex;
                pager.TabID = TabId;
                pager.QuerystringParams = "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&ctl=ManageLocations";

                dgLocations.Attributes.Add("SortColumn", sortColumn);
            }

        }

        protected void dgLocations_EditCommand(object source, DataGridCommandEventArgs e)
        {
            Label locationId = (Label)e.Item.FindControl("lblLocationId");
            Response.Redirect(Globals.NavigateURL(TabId, "ManageLocation", "mid=" + ModuleId + "&lid=" + locationId.Text));
        }

        protected void dgLocations_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            dgLocations.EditItemIndex = -1;
            bool approved = rbApproved.Checked;
            BindData(approved, "Name");
        }

        //protected void dgLocations_DeleteCommand(object source, DataGridCommandEventArgs e)
        //{
        //    DataProvider provider = DataProvider.Instance();
        //    Label lbl = (Label)e.Item.FindControl("lblLocationId");
        //    if (lbl.Text != "")
        //    {
        //        provider.DeleteLocation(Convert.ToInt32(lbl.Text));
        //    }
        //    bool approved = rbApproved.Checked;
        //    BindData(approved, "Name");
        //}

        protected void btnAddLocation_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId, "ManageLocation", "mid=" + ModuleId + "&tmid=" + TabModuleId));
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId));
        }

        protected void rbApproved_CheckChanged(object sender, EventArgs e)
        {
            bool approved = rbApproved.Checked;
            string href = EditUrl("Approved", approved.ToString(), "ManageLocations");
            //have to redirect to get the currentpage off the querystring
            Response.Redirect(href, true);
        }

        protected void dgLocations_DataBind(object sender, EventArgs e)
        {
            if (rbApproved.Checked)
            {
                dgLocations.Columns[checkboxColumn].Visible = false;
                this.btnReject.Visible = false;
                btnAccept.Visible = false;
            }
            else if (rbWaitingForApproval.Checked)
            {
                dgLocations.Columns[checkboxColumn].Visible = true;
                this.btnReject.Visible = true;
                btnAccept.Visible = true;
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            foreach (DataGridItem row in dgLocations.Items)
            {
                CheckBox cbApproved = (CheckBox)row.FindControl("cbApproved");
                if (cbApproved.Checked)
                {
                    Label lbl = (Label)row.FindControl("lblLocationId");
                    Location.DeleteLocation(Convert.ToInt32(lbl.Text, CultureInfo.InvariantCulture));
                }
            }

            BindData(false, "Name");
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            foreach (DataGridItem row in dgLocations.Items)
            {
                CheckBox cbApproved = (CheckBox)row.FindControl("chkApproved");
                if (cbApproved.Checked)
                {
                    Label lblLocationId = (Label)row.FindControl("lblLocationId");
                    Location location = Location.GetLocation(Convert.ToInt32(lblLocationId.Text, CultureInfo.InvariantCulture));
                    location.Approved = true;
                    location.Update();
                }
            }
            BindData(false, "Name");
        }

        protected void dgLocations_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            //{
            //    TableCell cell = e.Item.Cells[e.Item.Cells.Count - 1];
            //    LinkButton button = (LinkButton)cell.Controls[0];
            //    ClientAPI.AddButtonConfirm(button, Localization.GetString("confirmDeleteLocation", LocalResourceFile));
            //}
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId));
        }


        protected void dgLocations_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                TableCell cell = e.Item.Cells[0];
                Label id = (Label)cell.FindControl("lblLocationId");

                // TODO: We should not be going back to the database here. If BindData() set the datasource to Location[] then this would not be needed. Change Location.GetLocations() to return a Location[] list.
                Location location = Location.GetLocation(Convert.ToInt32(id.Text, CultureInfo.InvariantCulture));
                DataSet approved = location.GetComments(true);
                DataSet waiting = location.GetComments(false);
                Label lbl = (Label)cell.FindControl("lblApprovedComments");
                lbl.Text = approved.Tables[0].Rows.Count + Localization.GetString("lblApprovedComments", LocalResourceFile);
                lbl = (Label)cell.FindControl("lblWaitingComments");
                lbl.Text = waiting.Tables[0].Rows.Count + Localization.GetString("lblWaitingComments", LocalResourceFile);

                Label lblAddress = (Label)cell.FindControl("lblAddress");
                lblAddress.Text = location.FullAddress;
            }
        }

        protected void dgLocations_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            string sort = dgLocations.Attributes["SortColumn"];
            string newSort;
            string direction = dgLocations.Attributes["SortDirection"];

            if (sort != null && sort == e.SortExpression)
            {
                newSort = e.SortExpression + " DESC";
            }
            else
            {
                newSort = e.SortExpression + " ASC";
            }

            BindData(true, newSort);

            dgLocations.Attributes.Add("SortColumn", e.SortExpression);
        }

        private int CurrentPageIndex
        {
            get
            {
                int index = 1;
                //Get the currentpage index from the url parameter
                if (Request.QueryString["currentpage"] != null)
                {
                    index = Convert.ToInt32(Request.QueryString["currentpage"], CultureInfo.InvariantCulture);
                }

                return index;
            }
        }

        private bool Approved
        {
            get
            {
                bool approved = true;
                //Get the currentpage index from the url parameter
                if (Request.QueryString["Approved"] != null)
                {
                    approved = Convert.ToBoolean(Request.QueryString["Approved"], CultureInfo.InvariantCulture);
                }

                return approved;
            }
        }
    }
}
