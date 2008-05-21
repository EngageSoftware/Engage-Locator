//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


using System;
using System.Globalization;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;


namespace Engage.Dnn.Locator
{
    public class ModuleBase : PortalModuleBase
    {
        #region Properties
        public bool RatingsEnabled
        {
            get
            {
                if (Null.IsNull(HostSettings.GetHostSetting("LocatorAllowRatings" + PortalId.ToString(CultureInfo.InvariantCulture))))
                    return false;
                return Convert.ToBoolean(HostSettings.GetHostSetting("LocatorAllowRatings" + PortalId.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
            }
        }

        public bool CommentsEnabled
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                if (Null.IsNull(HostSettings.GetHostSetting("LocatorAllowComments" + PortalId.ToString(CultureInfo.InvariantCulture))))
                    return false;
                return Convert.ToBoolean(HostSettings.GetHostSetting("LocatorAllowComments" + PortalId.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
            }
        }

        public bool CommentModerationEnabled
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                if (Null.IsNull(HostSettings.GetHostSetting("LocatorCommentModeration" + PortalId.ToString(CultureInfo.InvariantCulture))))
                    return false;
                return Convert.ToBoolean(HostSettings.GetHostSetting("LocatorCommentModeration" + PortalId.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
            }
        }

        public bool SubmissionsEnabled
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                if (Null.IsNull(HostSettings.GetHostSetting("LocatorAllowSubmissions" + PortalId.ToString(CultureInfo.InvariantCulture))))
                    return false;
                else
                    return Convert.ToBoolean(HostSettings.GetHostSetting("LocatorAllowSubmissions" + PortalId.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
            }
        }

        public bool SubmissionModerationEnabled
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                if (Null.IsNull(HostSettings.GetHostSetting("LocatorSubmissionModeration" + PortalId.ToString(CultureInfo.InvariantCulture))))
                    return false;
                else
                    return Convert.ToBoolean(HostSettings.GetHostSetting("LocatorSubmissionModeration" + PortalId.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
            }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }
        }
        
    }
}
