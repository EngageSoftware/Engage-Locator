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
                    Comment comment = Comment.GetComment(commentId);

                    lblCommentId.Text = comment.CommentId.ToString();
                    lblLocationTitle.Text = comment.LocationName;
                    txtComment.Text = comment.Text;
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
            Comment comment = new Comment();
            comment.CommentId = Convert.ToInt32(lblCommentId.Text);
            comment.Text = txtComment.Text;
            comment.SubmittedBy = txtSubmittedBy.Text;
            comment.Approved = rbApproved.Checked;

            comment.SaveComment();

            Response.Redirect(Globals.NavigateURL(TabId, "Import", "mid=" + ModuleId + "&tmid=" + TabModuleId));
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Comment.DeleteComment(Convert.ToInt32(lblCommentId.Text));
            Response.Redirect(Globals.NavigateURL(TabId, "Import", "mid=" + ModuleId + "&tmid=" + TabModuleId));
        }
    }
}