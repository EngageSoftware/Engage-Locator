// <copyright file="ManualGeocodeResult.cs" company="Engage Software">
// Engage: Locator - http://www.engagemodules.com
// Copyright (c) 2004-2009
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Locator
{
    using DotNetNuke.Services.Localization;

    /// <summary>
    /// A provided geocode result, not obtained from a web service.
    /// </summary>
    internal class ManualGeocodeResult : GeocodeResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManualGeocodeResult"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public ManualGeocodeResult(double? latitude, double? longitude)
                : base(latitude ?? double.NaN, longitude ?? double.NaN)
        {
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GoogleGeocodeResult"/> is successful.
        /// </summary>
        /// <value><c>true</c> if successful; otherwise, <c>false</c>.</value>
        public override bool Successful
        {
            get { return !double.IsNaN(this.Latitude) && !double.IsNaN(this.Longitude); }
        }

        /// <summary>
        /// Gets the error message, or <c>null</c> if there is no error.
        /// </summary>
        /// <value>The error message, or <c>null</c> if there is no error.</value>
        public override string ErrorMessage
        {
            get
            {
                return this.Successful ? null : Localization.GetString("BadManualResult.Text", Utility.LocalSharedResourceFile);
            }
        }
    }
}