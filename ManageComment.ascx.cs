using System;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using Globals=DotNetNuke.Common.Globals;

namespace Engage.Dnn.Locator
{
    public partial class ManageComment : ModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    int commentId = Convert.ToInt32(Request.QueryString["cid"]);
                    LocationComment comment = LocationComment.GetComment(commentId);

                    lblLocationCommentId.Text = comment.CommentId.ToString();
                    lblLocationTitle.Text = comment.LocationName;
                    txtComment.Text = comment.Comment;
                    txtSubmittedBy.Text = comment.SubmittedBy;
                    if (comment.Approved)
                        rbApproved.Checked = true;
                    else
                        rbWaitingForApproval.Checked = true;
                    ClientAPI.AddButtonConfirm(btnDelete, Localization.GetString("confirmDelete", LocalResourceFile));
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId, "Import", "mid=" + ModuleId + "&tmid=" + TabModuleId));
        }

        protected void btnSaveComment_Click(object sender, EventArgs e)
        {
            LocationComment comment = new LocationComment();
            comment.CommentId = Convert.ToInt32(lblLocationCommentId.Text);
            comment.Comment = txtComment.Text;
            comment.SubmittedBy = txtSubmittedBy.Text;
            comment.Approved = rbApproved.Checked;

            comment.SaveComment();

            Response.Redirect(Globals.NavigateURL(TabId, "Import", "mid=" + ModuleId + "&tmid=" + TabModuleId));
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LocationComment.DeleteComment(Convert.ToInt32(lblLocationCommentId.Text));
            Response.Redirect(Globals.NavigateURL(TabId, "Import", "mid=" + ModuleId + "&tmid=" + TabModuleId));
        }
    }
}