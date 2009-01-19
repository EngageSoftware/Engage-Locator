// <copyright file="YahooGeocodeResult.cs" company="Engage Software">
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
    /// The result of a geocoding request to Yahoo!
    /// </summary>
    public class YahooGeocodeResult : GeocodeResult
    {
        /// <summary>
        /// The accuracy of the geocode result
        /// </summary>
        private readonly YahooAccuracyCode accuracyCode;

        /// <summary>
        /// The status of the geocode request
        /// </summary>
        private readonly YahooStatusCode statusCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooGeocodeResult"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="accuracyCode">The accuracy code.</param>
        /// <param name="statusCode">The status code.</param>
        public YahooGeocodeResult(double latitude, double longitude, YahooAccuracyCode accuracyCode, YahooStatusCode statusCode)
            : base(latitude, longitude)
        {
            this.accuracyCode = accuracyCode;
            this.statusCode = statusCode;
        }

        /// <summary>
        /// Gets the accuracy of the geocode result
        /// </summary>
        public YahooAccuracyCode AccuracyCode
        {
            get { return this.accuracyCode; }
        }

        /// <summary>
        /// Gets the status of the geocode request
        /// </summary>
        public YahooStatusCode StatusCode
        {
            get { return this.statusCode; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GoogleGeocodeResult"/> is successful.
        /// </summary>
        /// <value><c>true</c> if successful; otherwise, <c>false</c>.</value>
        public override bool Successful
        {
            get { return this.statusCode == YahooStatusCode.Success && this.accuracyCode > YahooAccuracyCode.Unknown; }
        }

        /// <summary>
        /// Gets the error message, or <c>null</c> if there is no error.
        /// </summary>
        /// <value>The error message, or <c>null</c> if there is no error.</value>
        public override string ErrorMessage
        {
            get {
                switch (this.statusCode)
                {
                    case YahooStatusCode.BadRequest:
                    case YahooStatusCode.Forbidden:
                    case YahooStatusCode.ServiceUnavailable:
                        return Localization.GetString(this.statusCode.ToString(), Utility.LocalSharedResourceFile);
                    default:
                        switch (this.accuracyCode)
                        {
                            case YahooAccuracyCode.Unknown:
                                return Localization.GetString("UnknownAddress", Utility.LocalSharedResourceFile);
                            default:
                                return null;
                        }
                }
            }
        }
    }
}