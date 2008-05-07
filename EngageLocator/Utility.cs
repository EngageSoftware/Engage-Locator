//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Engage.Dnn.Locator.Util
{
    /// <summary>
    /// Summary description for Utility
    /// </summary>
    public static class Utility
    {
        public const string EmailRegEx = @"^[a-zA-Z0-9._%\-+']+@(?:[a-zA-Z0-9\-]+\.)+(?:[a-zA-Z]{2}|com|org|net|biz|info|name|aero|jobs|museum)$";
        public const string EmailsRegEx = @"^[a-zA-Z0-9._%\-+']+@(?:[a-zA-Z0-9\-]+\.)+(?:[a-zA-Z]{2}|com|org|net|biz|info|name|aero|jobs|museum)(?:,\s?[a-zA-Z0-9._%\-+']+@(?:[a-zA-Z0-9\-]+\.)+(?:[a-zA-Z]{2}|com|org|net|biz|info|name|aero|jobs|museum))*$";
        public const string NullString = "null";
        public const string Nbsp = "&nbsp;";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification="Compiler doesn't understand HasValue(string)")]
        public static string AddQuotes(string s)
        {
            if (HasValue(s) == false)
            {
                return NullString;
            }

            //wrap s with single quotes,
            //parse the passed string and find any single quotes and if
            //found add another one after it
            StringBuilder sb = new StringBuilder(s.Length + 16);
            sb.Append("'");
            sb.Append(s.Replace("'", "''"));

            //add the last quote
            sb.Append("'");

            return sb.ToString();
        }

        public static bool HasValue(string s)
        {
            if (s == null)
            {
                return false;
            }
            else if (s.Trim().Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Validates the email address.
        /// </summary>
        /// <param name="emailAddress">The email address to validate.</param>
        /// <returns>Whether the specified email address is in a valid format.</returns>
        public static bool ValidateEmailAddress(string emailAddress)
        {
            return ValidateEmailAddress(emailAddress, false);
        }

        /// <summary>
        /// Validates an email address, or multiple comma-delimited email addresses.
        /// </summary>
        /// <param name="emailAddress">The email address(es) to validate.</param>
        /// <param name="commaDelimited">
        /// if set to <c>true</c>, <paramref name="emailAddress"/> could be multiple, comma-delimited email addresses,
        /// otherwise it must be a single email address.
        /// </param>
        /// <returns>Whether the specified email address(es) are in a valid format.</returns>
        public static bool ValidateEmailAddress(string emailAddress, bool commaDelimited)
        {
            if (commaDelimited)
            {
                return Regex.IsMatch(emailAddress, EmailRegEx);
            }
            else
            {
                return Regex.IsMatch(emailAddress, EmailsRegEx);
            }
        }

        public static bool IsInteger(string s)
        {
            if (HasValue(s) == false)
                return false;

            try
            {
                Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }

            return true;
        }

        public static bool IsLoggedIn
        {
            get
            {
                return HttpContext.Current.Request.IsAuthenticated;
            }
        }

        public static bool EqualsIgnoreCase(string s1, string s2)
        {
            return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        }

        public static int GetRandomSeed()
        {
            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            return (randomBytes[0] & 0x7f) << 24 | randomBytes[1] << 16 | randomBytes[2] << 8 | randomBytes[3];
        }

        /// <summary>
        /// Registers a script to display error messages from server-side validation as the specified <see cref="UserControl"/> or <see cref="Page"/> loads from a postback.
        /// </summary>
        /// <remarks>
        /// Must be called in the PreRender if used to validate against the Text property of DNNTextEditor controls, otherwise Text will not be populated.
        /// Must set the ErrorMessage manually if using a resourcekey, otherwise the resourcekey will not have overridden the ErrorMessage property.
        /// </remarks>
        /// <param name="ctrl">The <see cref="UserControl"/> or <see cref="Page"/> which is being posted back.</param>
        public static void RegisterServerValidationMessageScript(TemplateControl ctrl)
        {
            RegisterServerValidationMessageScript(ctrl, string.Empty);
        }

        /// <summary>
        /// Registers a script to display error messages from server-side validation as the specified <see cref="UserControl"/> or <see cref="Page"/> loads from a postback.
        /// </summary>
        /// <remarks>
        /// Must be called in the PreRender if used to validate against the Text property of DNNTextEditor controls, otherwise Text will not be populated.
        /// Must set the ErrorMessage manually if using a resourcekey, otherwise the resourcekey will not have overridden the ErrorMessage property.
        /// </remarks>
        /// <param name="ctrl">The <see cref="UserControl"/> or <see cref="Page"/> which is being posted back.</param>
        /// <param name="validationGroup">The validation group against which to validate.</param>
        public static void RegisterServerValidationMessageScript(TemplateControl ctrl, string validationGroup)
        {
            if (ctrl.Page.IsPostBack)
            {
                ctrl.Page.Validate(validationGroup);
                if (!ctrl.Page.IsValid)
                {
                    StringBuilder errorMessage = new StringBuilder("<script language='javascript'>alert('");
                    for (int i = 0; i < ctrl.Page.Validators.Count; i++)
                    {
                        IValidator validator = ctrl.Page.Validators[i];
                        if (!validator.IsValid)
                        {
                            errorMessage.Append("- " + validator.ErrorMessage);
                            if (i < ctrl.Page.Validators.Count - 1)
                            {
                                errorMessage.Append(@"\r\n");
                            }
                        }
                    }
                    errorMessage.Append("');</script>");
                    ctrl.Page.ClientScript.RegisterStartupScript(typeof(IValidator), "validationAlert", errorMessage.ToString(), false);
                }
            }
        }
    }
}
