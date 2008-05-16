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
using FileInfo=DotNetNuke.Services.FileSystem.FileInfo;
using Globals=DotNetNuke.Common.Globals;

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
        }



        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            string error = String.Empty;

            if (MainDisplay.IsConfigured(TabModuleId, ref error))
            {

                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["lid"] != null && Request.QueryString["approved"] != null)
                    {
                        bool approved = Convert.ToBoolean(Request.QueryString["approved"]);
                        Location location = Location.GetLocation(Convert.ToInt32(Request.QueryString["lid"]));
                    }
                    else
                    {
                        Localization.LocalizeDataGrid(ref dgLocations, LocalResourceFile);
                        lblConfigured.Visible = false;
                        divPanelTab.Visible = true;

                        if (Settings["ModerateSubmissions"] != null && Settings["ModerateSubmissions"].ToString() == "True")
                        {
                            int approved = 0;
                            if (rbApproved.Checked)
                                approved = 1;
                            BindDataGrid(approved);
                        }
                        else
                        {
                            rbLocations.Visible = false;
                            BindDataGrid(1);
                        }
                        ClientAPI.AddButtonConfirm(btnDelete, Localization.GetString("confirmDeleteLocation"));
                    }
                }
            }
            else
            {
                
                lblConfigured.Visible = true;
                lblConfigured.Text = lblConfigured.Text + " " + error;
            }
        }

       
        private void BindDataGrid(int approved)
        {
            DataTable locations = Location.GetLocations(PortalId, approved);
            dgLocations.DataSource = locations;
            dgLocations.DataBind();
        }
        
        protected void dgLocations_PageChange(Object sender, DataGridPageChangedEventArgs e)
        {
            dgLocations.CurrentPageIndex = e.NewPageIndex;
            int approved = 0;
            if (rbApproved.Checked)
                approved = 1;
            BindDataGrid(approved);  
        }

        protected void dgLocations_EditCommand(object source, DataGridCommandEventArgs e)
        {
            Label locationId = (Label) e.Item.FindControl("lblLocationId");
            Response.Redirect(Globals.NavigateURL(TabId, "ManageLocation", "mid=" + ModuleId + "&lid=" + locationId.Text));
        }

        protected void dgLocations_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            dgLocations.EditItemIndex = -1;
            int approved = 0;
            if (rbApproved.Checked)
                approved = 1;
            BindDataGrid(approved);  
        }

        protected void dgLocations_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            DataProvider provider = DataProvider.Instance();
            Label lbl = (Label)e.Item.FindControl("lblLocationId");
            if (lbl.Text != "")
            {
                provider.DeleteLocation(Convert.ToInt32(lbl.Text));
            }
            int approved = 0;
            if (rbApproved.Checked)
                approved = 1;
            BindDataGrid(approved);  
        }
       
        protected void btnAddLocation_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId, "ManageLocation", "mid=" + ModuleId + "&tmid=" + TabModuleId));
        }

        protected void rbLocations_CheckChanged(object sender, EventArgs e)
        {
            if (dgLocations.CurrentPageIndex != 0)
                dgLocations.CurrentPageIndex = 0;
            int approved = 0;
            if (rbApproved.Checked)
                approved = 1;
            BindDataGrid(approved);
            if (approved == 0)
            {
                btnDelete.Visible = dgLocations.Items.Count > 0;
                btnAccept.Visible = dgLocations.Items.Count > 0;
            }
        }

        protected void dgLocations_DataBind(object sender, EventArgs e)
        {
            if (rbApproved.Checked)
            {
                dgLocations.Columns[checkboxColumn].Visible = false;
                this.btnDelete.Visible = false;
                btnAccept.Visible = false;
            }
            else if (rbWaitingForApproval.Checked)
            {
                dgLocations.Columns[checkboxColumn].Visible = true;
                this.btnDelete.Visible = true;
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
                    Location.DeleteLocation(Convert.ToInt32(lbl.Text));
                }
            }
            //int approved = 0;
            //if (rbApproved.Checked)
            //    approved = 1;
            BindDataGrid(0);
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            foreach (DataGridItem row in dgLocations.Items)
            {
                CheckBox cbApproved = (CheckBox)row.FindControl("cbApproved");
                if(cbApproved.Checked)
                {
                    Label lblLocationId = (Label)row.FindControl("lblLocationId");
                    Location location = Location.GetLocation(Convert.ToInt32(lblLocationId.Text));
                    location.Approved = true;
                    location.Update();
                }
            }
            BindDataGrid(0);
        }

        protected void dgLocations_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                TableCell cell = e.Item.Cells[e.Item.Cells.Count - 1];
                LinkButton button = (LinkButton)cell.Controls[0];
                ClientAPI.AddButtonConfirm(button, Localization.GetString("confirmDeleteLocation", LocalResourceFile));
            }
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
                Location location = Location.GetLocation(Convert.ToInt32(id.Text));
                DataSet approved = location.GetComments(true);
                DataSet waiting = location.GetComments(false);
                Label lbl = (Label)cell.FindControl("lblApprovedComments");
                lbl.Text = approved.Tables[0].Rows.Count + Localization.GetString("lblApprovedComments", LocalResourceFile);
                lbl = (Label)cell.FindControl("lblWaitingComments");
                lbl.Text = waiting.Tables[0].Rows.Count + Localization.GetString("lblWaitingComments", LocalResourceFile);
            }
        }
    }
}
