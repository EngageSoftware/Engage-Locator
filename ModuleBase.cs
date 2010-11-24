// <copyright file="ModuleBase.cs" company="Engage Software">
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
    using System.Diagnostics;
    using System.Globalization;

#if TRIAL
    using System.Web.UI;

    using Engage.Dnn.Locator.Components;
    using Engage.Licensing;
#endif

    using Maps;

    /// <summary>
    /// The base class for all control in the Engage: Locator module
    /// </summary>
    public class ModuleBase : Framework.ModuleBase
    {
        /// <summary>
        /// Gets the name of the desktop module.
        /// </summary>
        /// <value>The name of the desktop module.</value>
        public override string DesktopModuleName
        {
            get { return Utility.DesktopModuleName; }
        }

        /// <summary>
        /// Raises the <see cref="EventArgs"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.AddJQueryReference();
        }
        
        /// <summary>
        /// Gets a value indicating whether ratings are enabled for the module on this portal.
        /// </summary>
        /// <value><c>true</c> if ratings are enabled; otherwise, <c>false</c>.</value>
        public bool RatingsEnabled
        {
            get 
            {
                return Utility.GetBooleanPortalSetting("LocatorAllowRatings", this.PortalId, false);
            }
        }

        /// <summary>
        /// Gets a value indicating whether comments are enabled for the module on this portal.
        /// </summary>
        /// <value><c>true</c> if comments are enabled; otherwise, <c>false</c>.</value>
        public bool CommentsEnabled
        {
            [DebuggerStepThrough]
            get
            {
                return Utility.GetBooleanPortalSetting("LocatorAllowComments", PortalId, false);
            }
        }

        /// <summary>
        /// Gets a value indicating whether new comment submissions on this portal should be moderated.
        /// </summary>
        /// <value>
        /// <c>true</c> if new comment submissions should be moderated; otherwise, <c>false</c>.
        /// </value>
        public bool CommentModerationEnabled
        {
            [DebuggerStepThrough]
            get
            {
                return Utility.GetBooleanPortalSetting("LocatorCommentModeration", PortalId, false);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the module accepts new location submissions on this portal.
        /// </summary>
        /// <value><c>true</c> if new location submissions are enabled; otherwise, <c>false</c>.</value>
        public bool SubmissionsEnabled
        {
            [DebuggerStepThroughAttribute]
            get
            {
                return Utility.GetBooleanPortalSetting("LocatorAllowSubmissions", PortalId, false);
            }
        }

        /// <summary>
        /// Gets a value indicating whether new location submissions on this portal should be moderated.
        /// </summary>
        /// <value>
        /// <c>true</c> if new location submissions should be moderated; otherwise, <c>false</c>.
        /// </value>
        public bool SubmissionModerationEnabled
        {
            [DebuggerStepThroughAttribute]
            get
            {
                return Utility.GetBooleanPortalSetting("LocatorSubmissionModeration", PortalId, false);
            }
        }

        /// <summary>
        /// Gets the map provider for this module.
        /// </summary>
        /// <returns>The map provider for this module</returns>
        public MapProvider GetMapProvider()
        {
            MapProviderType providerType = Dnn.Utility.GetStringSetting(this.Settings, "DisplayProvider").ToUpperInvariant() == "YAHOO"
                                                   ? MapProviderType.YahooMaps
                                                   : MapProviderType.GoogleMaps;

            string apiKey = Dnn.Utility.GetStringSetting(this.Settings, providerType.ClassName + ".ApiKey");
            return MapProvider.CreateInstance(providerType, apiKey);
        }

#if TRIAL
        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.LicenseProvider = new TrialLicenseProvider(FeaturesController.ModuleLicenseKey);

            base.OnInit(e);
        }
#endif

        /// <summary>
        /// Handles the OnClick event of the lbSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbSettings_OnClick(object sender, EventArgs e)
        {
            this.Response.Redirect(this.EditUrl("ModuleId", this.ModuleId.ToString(CultureInfo.InvariantCulture), "Module"), true);
        }

        /// <summary>
        /// Handles the OnClick event of the lbManageLocations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbManageLocations_OnClick(object sender, EventArgs e)
        {
            this.Response.Redirect(this.EditUrl("ManageLocations"), true);
        }

        /// <summary>
        /// Handles the OnClick event of the lbImportFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbImportFile_OnClick(object sender, EventArgs e)
        {
            this.Response.Redirect(this.EditUrl("Import"), true);
        }

        /// <summary>
        /// Handles the OnClick event of the lbManageComments control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbManageComments_OnClick(object sender, EventArgs e)
        {
            this.Response.Redirect(this.EditUrl("ManageComments"), true);
        }

        /// <summary>
        /// Handles the OnClick event of the lbManageTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbManageTypes_OnClick(object sender, EventArgs e)
        {
            this.Response.Redirect(this.EditUrl("AttributeDefinitions"), true);
        }
    }
}
