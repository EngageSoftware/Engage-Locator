// <copyright file="Utility.cs" company="Engage Software">
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
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;

    /// <summary>
    /// A class containing utilities for Engage: Locator
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// The name of the desktop module
        /// </summary>
        public const string DesktopModuleName = "Engage: Locator";

        /// <summary>
        /// Gets the portal setting with the given key as a <see cref="bool"/>, returning <paramref name="defaultSetting"/> if the setting doesn't exist.
        /// </summary>
        /// <param name="settingKey">The key of the setting to retrieve.</param>
        /// <param name="portalId">The ID of the portal of the setting.</param>
        /// <param name="defaultSetting">The default value of the setting</param>
        /// <returns>The value of the setting with the given key in the given portal, or <paramref name="defaultSetting"/> if it is not defined.</returns>
        public static bool GetBooleanPortalSetting(string settingKey, int portalId, bool defaultSetting)
        {
            string setting = HostSettings.GetHostSetting(settingKey + portalId.ToString(CultureInfo.InvariantCulture));
            return Null.IsNull(setting) ? defaultSetting : Convert.ToBoolean(setting, CultureInfo.InvariantCulture);
        }
    }
}
