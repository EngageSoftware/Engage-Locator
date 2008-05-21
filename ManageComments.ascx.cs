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
using FileInfo=DotNetNuke.Services.FileSystem.FileInfo;
using Globals=DotNetNuke.Common.Globals;

namespace Engage.Dnn.Locator
{
    partial class ManageComments : ModuleBase
    {
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
            lbSettings.Visible = IsEditable;
            lbImportFile.Visible = IsEditable;
            lbManageLocations.Visible = IsEditable;
            lbLocationTypes.Visible = IsEditable;

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
                        Localization.LocalizeDataGrid(ref dgSubmittedComments, LocalResourceFile);
                        dgSubmittedComments.DataSource = location.GetComments(approved);
                        dgSubmittedComments.DataBind();
                    }
                    else
                    {
                        Localization.LocalizeDataGrid(ref dgSubmittedComments, LocalResourceFile);
                        lblConfigured.Visible = false;
                        divPanelTab.Visible = true;

                        BindData();
                        ClientAPI.AddButtonConfirm(btnDeleteComment, Localization.GetString("confirmDeleteComment", LocalResourceFile));
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

        private void BindData()
        {
            DataTable comments = Location.GetNewSubmittedComments(PortalId, false);
            dgSubmittedComments.DataSource = comments;
            dgSubmittedComments.DataBind();

            lblNoPending.Visible = (comments.Rows.Count == 0);
            btnAcceptComment.Visible = (comments.Rows.Count > 0);
            btnDeleteComment.Visible = (comments.Rows.Count > 0);
            dgSubmittedComments.PagerStyle.Visible = (comments.Rows.Count > 0);
        }
        
      
        protected void dgComments_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            Label lblCommentId = (Label)e.Item.FindControl("lblCommentId");
            if (lblCommentId.Text != "")
            {
                Comment.DeleteComment(Convert.ToInt32(lblCommentId.Text));
            }
            if (dgSubmittedComments.Items.Count > 0)
            {
                BindData();
            }
            else
            {
                Response.Redirect(Globals.NavigateURL());
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
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
            BindData();
        }

        protected void btnApprove_Click(object sender, EventArgs e)
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
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId));
        }

        protected void dgComments_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            //{
            //    TableCell cell = e.Item.Cells[e.Item.Cells.Count - 1];
            //    LinkButton button = (LinkButton)cell.Controls[0];
            //    ClientAPI.AddButtonConfirm(button, Localization.GetString("confirmDeleteComment", LocalResourceFile));
            //}
        }
    }
}
