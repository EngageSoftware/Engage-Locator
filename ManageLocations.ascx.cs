// <copyright file="ManageLocations.ascx.cs" company="Engage Software">
// Engage: Locator - http://www.engagemodules.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Locator
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.UserControls;
    using DotNetNuke.UI.Utilities;

    partial class ManageLocations : ModuleBase
    {
        private const int checkboxColumn = 9;

        protected TextEditor teLocationDetails;

        private bool Approved
        {
            get
            {
                bool approved = true;
                if (this.Request.QueryString["Approved"] != null)
                {
                    approved = Convert.ToBoolean(this.Request.QueryString["Approved"], CultureInfo.InvariantCulture);
                }

                return approved;
            }
        }

        private int CurrentPageIndex
        {
            get
            {
                int index = 1;
                if (this.Request.QueryString["currentpage"] != null)
                {
                    index = Convert.ToInt32(this.Request.QueryString["currentpage"], CultureInfo.InvariantCulture);
                }

                return index;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (AJAX.IsInstalled())
            {
                AJAX.RegisterScriptManager();
            }
            this.rbWaitingForApproval.CheckedChanged += this.rbApproved_CheckChanged;
            this.rbApproved.CheckedChanged += this.rbApproved_CheckChanged;

            this.lbSettings.Visible = this.IsEditable;
            this.lbImportFile.Visible = this.IsEditable;
            this.lbManageComments.Visible = this.IsEditable;
            this.lbLocationTypes.Visible = this.IsEditable;
        }


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Global navigation
            if (this.UserInfo.IsSuperUser)
            {
                this.lbManageLocations.Enabled = false;
                this.lbManageLocations.CssClass = "mnavDisabled";
                this.lbManageLocations.ImageUrl = "~/desktopmodules/EngageLocator/images/locationBtDisabled.gif";
            }

            string error = String.Empty;

            if (MainDisplay.IsConfigured(this.TabModuleId, ref error))
            {
                if (!this.IsPostBack)
                {
                    this.Bind();
                }
            }
            else
            {
                this.lblConfigured.Visible = true;
                this.lblConfigured.Text = this.lblConfigured.Text + " " + error;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnAccept control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            foreach (DataGridItem row in this.dgLocations.Items)
            {
                CheckBox cbApproved = (CheckBox)row.FindControl("chkApproved");
                if (cbApproved.Checked)
                {
                    Label lblLocationId = (Label)row.FindControl("lblLocationId");
                    Location location = Location.GetLocation(Convert.ToInt32(lblLocationId.Text, CultureInfo.InvariantCulture));
                    location.Approved = true;
                    location.Save();
                }
            }

            this.BindData(false, "Name");
        }

        /// <summary>
        /// Handles the Click event of the btnAddLocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnAddLocation_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId, "ManageLocation", "mid=" + this.ModuleId + "&tmid=" + this.TabModuleId));
        }

        /// <summary>
        /// Handles the Click event of the btnBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId));
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId));
        }

        /// <summary>
        /// Handles the Click event of the btnReject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnReject_Click(object sender, EventArgs e)
        {
            foreach (DataGridItem row in this.dgLocations.Items)
            {
                CheckBox cbApproved = (CheckBox)row.FindControl("cbApproved");
                if (cbApproved.Checked)
                {
                    Label lbl = (Label)row.FindControl("lblLocationId");
                    Location.DeleteLocation(Convert.ToInt32(lbl.Text, CultureInfo.InvariantCulture));
                }
            }

            this.BindData(false, "Name");
        }

        /// <summary>
        /// Handles the CancelCommand event of the dgLocations control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void dgLocations_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgLocations.EditItemIndex = -1;
            bool approved = this.rbApproved.Checked;
            this.BindData(approved, "Name");
        }

        /// <summary>
        /// Handles the DataBind event of the dgLocations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void dgLocations_DataBind(object sender, EventArgs e)
        {
            if (this.rbApproved.Checked)
            {
                this.dgLocations.Columns[checkboxColumn].Visible = false;
                this.btnReject.Visible = false;
                this.btnAccept.Visible = false;
            }
            else if (this.rbWaitingForApproval.Checked)
            {
                this.dgLocations.Columns[checkboxColumn].Visible = true;
                this.btnReject.Visible = true;
                this.btnAccept.Visible = true;
            }
        }

        /// <summary>
        /// Handles the EditCommand event of the dgLocations control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void dgLocations_EditCommand(object source, DataGridCommandEventArgs e)
        {
            Label locationId = (Label)e.Item.FindControl("lblLocationId");
            this.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId, "ManageLocation", "mid=" + this.ModuleId + "&lid=" + locationId.Text));
        }

        /// <summary>
        /// Handles the ItemDataBound event of the dgLocations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
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
                lbl.Text = approved.Tables[0].Rows.Count + Localization.GetString("lblApprovedComments", this.LocalResourceFile);
                lbl = (Label)cell.FindControl("lblWaitingComments");
                lbl.Text = waiting.Tables[0].Rows.Count + Localization.GetString("lblWaitingComments", this.LocalResourceFile);

                Label lblAddress = (Label)cell.FindControl("lblAddress");
                lblAddress.Text = location.FullAddress;
            }
        }

        /// <summary>
        /// Handles the SortCommand event of the dgLocations control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridSortCommandEventArgs"/> instance containing the event data.</param>
        protected void dgLocations_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            string newSort;
            string sort = this.dgLocations.Attributes["SortColumn"];

            if (sort != null && sort == e.SortExpression)
            {
                newSort = e.SortExpression + " DESC";
            }
            else
            {
                newSort = e.SortExpression + " ASC";
            }

            this.BindData(true, newSort);

            this.dgLocations.Attributes.Add("SortColumn", e.SortExpression);
        }

        /// <summary>
        /// Handles the CheckChanged event of the rbApproved control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void rbApproved_CheckChanged(object sender, EventArgs e)
        {
            bool approved = this.rbApproved.Checked;
            string href = this.EditUrl("Approved", approved.ToString(), "ManageLocations");
            
            // have to redirect to get the currentpage off the querystring
            this.Response.Redirect(href, true);
        }

        private void Bind()
        {
            // Localization.LocalizeDataGrid has a bug when ShowMissingKeys is on, so it localizes twice (shows "RESX:[L]Edit.Text").
            // This doesn't affect users, and doesn't have an easy fix.  Don't worry about it.
            Localization.LocalizeDataGrid(ref this.dgLocations, this.LocalResourceFile);
            this.lblConfigured.Visible = false;
            this.divPanelTab.Visible = true;

            if (this.Approved)
            {
                this.rbApproved.Checked = true;
            }
            else
            {
                this.rbWaitingForApproval.Checked = true;
                this.btnReject.Visible = true;
                this.btnAccept.Visible = true;
            }

            this.BindData(this.Approved, "Name");
            ClientAPI.AddButtonConfirm(this.btnReject, Localization.GetString("confirmDeleteLocation"));
        }

        private void BindData(bool approved, string sortColumn)
        {
            DataTable locations = Location.GetLocations(this.PortalId, approved, sortColumn, this.CurrentPageIndex - 1, this.dgLocations.PageSize);
            this.dgLocations.DataSource = locations;
            this.dgLocations.DataBind();

            if (locations.Rows.Count > 0)
            {
                this.pager.TotalRecords = Convert.ToInt32(locations.Rows[0]["TotalRecords"], CultureInfo.InvariantCulture);
                this.pager.PageSize = this.dgLocations.PageSize;
                this.pager.CurrentPage = this.CurrentPageIndex;
                this.pager.TabID = this.TabId;
                this.pager.QuerystringParams = "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&ctl=ManageLocations";

                this.dgLocations.Attributes.Add("SortColumn", sortColumn);
            }
        }
    }
}