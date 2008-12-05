// <copyright file="Details.ascx.cs" company="Engage Software">
// Engage: Locator - http://www.engagemodules.com
// Copyright (c) 2004-2008
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
    using System.Globalization;
    using AjaxControlToolkit;
    using Data;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Localization;
    using Rating = UserFeedback.Rating;

    /// <summary>
    /// A control which displays details about a particular location
    /// </summary>
    public partial class Details : ModuleBase
    {
        /// <summary>
        /// Gets a value indicating whether this module instance is set to display the location details field.
        /// </summary>
        /// <value><c>true</c> if this module instance is set to display the location details field; otherwise, <c>false</c>.</value>
        protected bool ShowLocationDetails
        {
            get
            {
                object setting = new ModuleController().GetTabModuleSettings(this.TabModuleId)["ShowLocationDetails"];
                return setting != null && setting.ToString() == "DetailsPage";
            }
        }

        /// <summary>
        /// Gets the ID of the location being displayed.
        /// </summary>
        /// <value>The location ID.</value>
        private int LocationId
        {
            get { return Convert.ToInt32(this.Request.QueryString["lid"], CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Raises the <see cref="ModuleBase.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += this.Page_Load;
            this.BackButton.Click += this.BackButton_Click;
            this.SubmitButton.Click += this.SubmitButton_Click;
            this.RatingControl.Changed += this.RatingControl_Changed;

            string commentSubmittedLocalizationKey = this.CommentModerationEnabled ? "lblCommentSubmittedModerated" : "lblCommentSubmitted";
            this.CommentSubmittedLabel.Text = Localization.GetString(commentSubmittedLocalizationKey, this.LocalResourceFile);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.BindData();
            }

            this.lbSettings.Visible
                = this.lbImportFile.Visible
                = this.lbManageLocations.Visible
                = this.lbLocationTypes.Visible
                = this.lbManageComments.Visible
                = this.IsEditable;

            this.ShowCommentEntryButton.Visible = this.CommentsEnabled;
            this.RatingUpdatePanel.Visible = this.RatingsEnabled;
        }

        /// <summary>
        /// Handles the Click event of the BackButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(Globals.NavigateURL(this.TabId));
        }

        /// <summary>
        /// Handles the Click event of the SubmitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            bool commentApproved = !this.CommentModerationEnabled;
            Location.InsertComment(this.LocationId, this.CommentTextBox.Text, this.SubmittedByTextBox.Text, commentApproved);
            this.CommentTextBox.Text = this.SubmittedByTextBox.Text = string.Empty;

            Location location = Location.GetLocation(this.LocationId);
            this.CommentsRepeater.DataSource = location.GetComments(true);
            this.CommentsRepeater.DataBind();
            this.CommentSubmittedLabel.Visible = true;
        }

        /// <summary>
        /// Handles the Changed event of the RatingControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AjaxControlToolkit.RatingEventArgs"/> instance containing the event data.</param>
        private void RatingControl_Changed(object sender, RatingEventArgs e)
        {
            Rating.AddRating(
                this.LocationId,
                this.UserId == -1 ? null : (int?)this.UserId,
                int.Parse(e.Value, CultureInfo.InvariantCulture),
                DataProvider.ModuleQualifier);
            this.RatingControl.ReadOnly = true;
        }

        /// <summary>
        /// Fills this control with information about this location
        /// </summary>
        private void BindData()
        {
            Location location = Location.GetLocation(this.LocationId);

            this.LocationNameLabel.Text = location.Name;
            this.LocationNameLink.Text = location.Website;
            this.LocationNameLink.NavigateUrl = location.Website;
            this.LocationDetailsLabel.Text = location.LocationDetails;
            this.LocationAddress1Label.Text = location.FullAddress;
            this.PhoneNumberLabel.Text = location.Phone;
            this.LocationAddress3Label.Text = location.Address3;

            this.RatingControl.CurrentRating = (int)Math.Round(location.AverageRating);

            this.CommentsRepeater.DataSource = location.GetComments(true).Tables[0];
            this.CommentsRepeater.DataBind();
            if (this.CommentsRepeater.Items.Count == 0)
            {
                this.CommentsRepeater.Visible = false;
            }

            this.CustomAttributeRepeater.DataSource = location.GetAttributes();
            this.CustomAttributeRepeater.DataBind();
        }
    }
}