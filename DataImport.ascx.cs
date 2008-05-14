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

using FileInfo=DotNetNuke.Services.FileSystem.FileInfo;
using Globals=DotNetNuke.Common.Globals;

namespace Engage.Dnn.Locator
{
    partial class DataImport : ModuleBase
    {
        private static int tabModuleId;
        protected TextEditor teLocationDetails;
        private const int checkboxColumn = 15;
        private const int commentsColumn = 14;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (fileImport.PostedFile == null || fileImport.PostedFile.ContentLength < 1)
                {
                    lblMessage.Text = Localization.GetString("NoFileSelected", LocalResourceFile);
                    return;
                }
                if (!fileImport.PostedFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    lblMessage.Text = Localization.GetString("UseCSVFile", LocalResourceFile); 
                    return;
                }
                else
                {
                    lblMessage.Text = Localization.GetString("FileExists", LocalResourceFile);
                }

                VerifyFolders();
                UploadFile();
                DataImportScheduler import = new DataImportScheduler();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        protected void UploadFile()
        {
            System.Web.HttpPostedFile postedFile = fileImport.PostedFile;
            FolderInfo fi = FileSystemUtils.GetFolder(PortalId, "Location Import");
            ArrayList al = FileSystemUtils.GetFilesByFolder(PortalId, fi.FolderID);
            FileController fc = new FileController();
            DataProvider provider = DataProvider.Instance();
            string fileName = Path.GetFileName(postedFile.FileName);
            string newPath = fi.PhysicalPath + fileName;
            foreach (FileInfo s in al)
            {
                if (s.FileName == fileName)
                {
                    lblMessage.Text = Localization.GetString("FileExists", LocalResourceFile); 
                    return;
                }
            }
            
            postedFile.SaveAs(newPath);
            string ext = Path.GetExtension(postedFile.FileName).Substring(1);
            int fileId = fc.AddFile(PortalId, fileName,ext , postedFile.ContentLength, Null.NullInteger, Null.NullInteger, postedFile.ContentType, fi.FolderPath, fi.FolderID, true);

            provider.InsertFileInfo(fileId, UserId, TabModuleId, PortalId, DateTime.Now, 0, 0);
            lblMessage.Text = Localization.GetString("FileUploaded", LocalResourceFile); 

        }

        protected void VerifyFolders()
        {
            string loc = "Location Import";
            string home = PortalSettings.HomeDirectoryMapPath;
            string homeImport = PortalSettings.HomeDirectoryMapPath + loc;
            string working = "Working";
            string completed = "Completed";
            string error = "Error";
            
            
            if (FileSystemUtils.GetFolder(PortalId, loc) == null)
            {
                FileSystemUtils.AddFolder(PortalSettings, home, loc);
            }

            if (FileSystemUtils.GetFolder(PortalId, loc + "/" + working) == null)
            {
                FileSystemUtils.AddFolder(PortalSettings, homeImport, working);
            }

            if (FileSystemUtils.GetFolder(PortalId, loc + "/" + completed) == null)
            {
                FileSystemUtils.AddFolder(PortalSettings, homeImport, completed);
            }

            if (FileSystemUtils.GetFolder(PortalId, loc + "/" + error) == null)
            {
                FileSystemUtils.AddFolder(PortalSettings, homeImport, error);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            tabModuleId = Convert.ToInt32(Request.QueryString["tmid"]);
            
            string error = String.Empty;

            if (IsConfigured(ref error))
            {

                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["lid"] != null && Request.QueryString["approved"] != null)
                    {
                        tabpnlLocations.Visible = false;
                        tabpnlLocations.Enabled = false;
                        tabPanelImport.Enabled = false;
                        tabPanelImport.Visible = false;
                        tabpnlComments.Visible = true;
                        tabpnlComments.Focus();

                        bool approved = Convert.ToBoolean(Request.QueryString["approved"]);
                        Location location = Location.GetLocation(Convert.ToInt32(Request.QueryString["lid"]));
                        if (approved)
                            lblLocationComments.Text = Localization.GetString("lblLocationComments_approved", LocalResourceFile);
                        else
                            lblLocationComments.Text = Localization.GetString("lblLocationComments_new", LocalResourceFile);
                        Localization.LocalizeDataGrid(ref dgSubmittedComments, LocalResourceFile);
                        dgSubmittedComments.DataSource = location.GetComments(approved);
                        dgSubmittedComments.DataBind();
                    }
                    else
                    {
                        Localization.LocalizeDataGrid(ref dgLocations, LocalResourceFile);
                        Localization.LocalizeDataGrid(ref dgSubmittedComments, LocalResourceFile);
                        lblConfigured.Visible = false;
                        divPanelTab.Visible = true;

                        if (Settings["ModerateSubmissions"] != null && Settings["ModerateSubmissions"].ToString() == "True")
                        {
                            int approved = 0;
                            if (rbApproved.Checked)
                                approved = 1;
                            BindDataGrid(approved);
                            BindComments();
                        }
                        else
                        {
                            rbLocations.Visible = false;
                            BindDataGrid(1);
                            BindComments();
                        }
                        ClientAPI.AddButtonConfirm(btnDeleteComment, Localization.GetString("confirmDeleteComment", LocalResourceFile));
                        ClientAPI.AddButtonConfirm(btnDelete, Localization.GetString("confirmDeleteLocation"));
                    }
                }
            }
            else
            {
                divPanelTab.Visible = false;
                lblConfigured.Visible = true;
                lblConfigured.Text = lblConfigured.Text + " " + error;
            }
        }

        public static bool IsConfigured(ref string error)
        {
            Hashtable settings = DotNetNuke.Entities.Portals.PortalSettings.GetTabModuleSettings(tabModuleId);
            string mapProvider = Convert.ToString(settings["DisplayProvider"]);
            if (mapProvider == String.Empty)
            {
                error = "No Map Provider Selected.";
                return false;
            }
            else
            {

                string className = MapProviderType.GetMapProviderClassByName(mapProvider);
                string apiKey = Convert.ToString(settings[className + ".ApiKey"]);
                if (apiKey == String.Empty)
                {
                    error = "No API Key Entered.";
                    return false;
                }
                else
                {
                    MapProvider mp = MapProvider.CreateInstance(className);
                    if (mp.IsKeyValid(apiKey))
                    {
                        error = "Success";
                        return true;
                    }
                    else
                    {
                        error = "API Key is not in the correct format.";
                        return false;
                    }
                }
            }
        }

        private void BindDataGrid(int approved)
        {
            DataTable locations = Location.GetLocations(PortalId, approved);
            tabContainer.Tabs[1].Visible = (locations.Rows.Count >= 0);
            dgLocations.DataSource = locations;
            dgLocations.DataBind();
        }
        
        private void BindComments()
        {
            DataTable comments = Location.GetNewSubmittedComments(PortalId, false);
            if (comments.Rows.Count > 0)
            {
                dgSubmittedComments.DataSource = comments;
                dgSubmittedComments.DataBind();
            }
            tabContainer.Tabs[2].Visible = (comments.Rows.Count > 0);
            lblLocationComments.Visible = (comments.Rows.Count > 0);
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
            Response.Redirect(Globals.NavigateURL(TabId, "ManageLocations", "mid=" + ModuleId + "&tmid=" + tabModuleId + "&lid=" + locationId.Text));
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
            Response.Redirect(Globals.NavigateURL(TabId, "ManageLocations", "mid=" + ModuleId + "&tmid=" + tabModuleId));
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

        protected void dgComments_EditCommand(object source, DataGridCommandEventArgs e)
        {
            Label commentId = (Label)e.Item.FindControl("lblCommentId");
            Response.Redirect(Globals.NavigateURL(TabId, "ManageComments", "mid=" + ModuleId + "&tmid=" + tabModuleId + "&cid=" + commentId.Text));
        }

        protected void dgComments_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            Label lblCommentId = (Label)e.Item.FindControl("lblCommentId");
            if (lblCommentId.Text != "")
            {
                Comment.DeleteComment(Convert.ToInt32(lblCommentId.Text));
            }
            DataTable comments = Location.GetNewSubmittedComments(PortalId, false);
            if (comments.Rows.Count > 0)
                BindComments();
            else
                Response.Redirect(Globals.NavigateURL(TabId, "Import", "mid=" + ModuleId + "&tmid=" + TabModuleId));
        }

        protected void btnDeleteComment_Click(object sender, EventArgs e)
        {
            foreach (DataGridItem row in dgSubmittedComments.Items)
            {
                CheckBox cbApproved = (CheckBox)row.FindControl("cbCommentApproved");
                if (cbApproved.Checked)
                {
                    Label lblCommentId = (Label)row.FindControl("lblCommentId");
                    Comment.DeleteComment(Convert.ToInt32(lblCommentId.Text));
                }
            }
            DataTable comments = Location.GetNewSubmittedComments(PortalId, false);
            if (comments.Rows.Count > 0)
                BindComments();
            else
                Response.Redirect(Globals.NavigateURL(TabId, "Import", "mid=" + ModuleId + "&tmid=" + TabModuleId));
        }

        protected void btnAcceptComment_Click(object sender, EventArgs e)
        {
            foreach (DataGridItem row in dgSubmittedComments.Items)
            {
                CheckBox cbApproved = (CheckBox)row.FindControl("cbCommentApproved");
                if (cbApproved.Checked)
                {
                    Label lblCommentId = (Label)row.FindControl("lblCommentId");
                    Comment comment = Comment.GetComment(Convert.ToInt32(lblCommentId.Text));
                    comment.Approved = true;
                    comment.Update();
                }
            }
            DataTable comments = Location.GetNewSubmittedComments(PortalId, false);
            if(comments.Rows.Count > 0)
                BindComments();
            else
                Response.Redirect(Globals.NavigateURL(TabId, "Import", "mid=" + ModuleId + "&tmid=" + TabModuleId));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId));
        }

        protected void dgComments_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                TableCell cell = e.Item.Cells[e.Item.Cells.Count - 1];
                LinkButton button = (LinkButton)cell.Controls[0];
                ClientAPI.AddButtonConfirm(button, Localization.GetString("confirmDeleteComment", LocalResourceFile));
            }
        }

        protected void btnApprovedComments_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("", "", "Import", "tmid=" + TabModuleId + "&lid=1186" + "&approved=true"));
        }

        protected void btnNewComents_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("", "", "Import", "tmid=" + TabModuleId + "&lid=1186" + "&approved=false"));
        }

        protected void dgLocations_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {    
                TableCell commentsCell = e.Item.Cells[0];
                Label id = (Label)commentsCell.FindControl("lblLocationId");
                Location location = Location.GetLocation(Convert.ToInt32(id.Text));
                DataSet approved = location.GetComments(true);
                DataSet waiting = location.GetComments(false);
                LinkButton approvedComments = (LinkButton)commentsCell.FindControl("btnApprovedComments");
                if(approved.Tables[0].Rows.Count > 0)
                {
                    approvedComments.Visible = true;
                    approvedComments.Text = approved.Tables[0].Rows.Count + Localization.GetString("btnApprovedComments", LocalResourceFile);
                }
                else
                    approvedComments.Visible = false;
                LinkButton newComments = (LinkButton)commentsCell.FindControl("btnNewComments");
                if(waiting.Tables[0].Rows.Count > 0)
                {
                    newComments.Visible = true;
                    newComments.Text = waiting.Tables[0].Rows.Count + Localization.GetString("btnNewComments", LocalResourceFile);
                }
                else
                    newComments.Visible = false;
                
            }
        }
    }
}
