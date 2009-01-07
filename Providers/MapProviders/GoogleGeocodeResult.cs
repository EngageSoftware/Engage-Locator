// <copyright file="GoogleGeocodeResult.cs" company="Engage Software">
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
    /// <summary>
    /// The result of a geocoding request to Google
    /// </summary>
    public class GoogleGeocodeResult : GeocodeResult
    {
        /// <summary>
        /// Backing field for <see cref="AccuracyCode"/>
        /// </summary>
        private readonly GoogleAccuracyCode accuracyCode;

        /// <summary>
        /// Backing field for <see cref="StatusCode"/>
        /// </summary>
        private readonly GoogleStatusCode statusCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleGeocodeResult"/> class.
        /// </summary>
        /// <param name="latitude">The latitude of the result.</param>
        /// <param name="longitude">The longitude of the result.</param>
        /// <param name="accuracyCode">The accuracy of the result.</param>
        /// <param name="statusCode">The status of the geocode request.</param>
        public GoogleGeocodeResult(double latitude, double longitude, GoogleAccuracyCode accuracyCode, GoogleStatusCode statusCode)
            : base(latitude, longitude)
        {
            this.accuracyCode = accuracyCode;
            this.statusCode = statusCode;
        }

        /// <summary>
        /// Gets the accuracy of the geocode result
        /// </summary>
        public GoogleAccuracyCode AccuracyCode
        {
            get { return this.accuracyCode; }
        }

        /// <summary>
        /// Gets the status of the geocode request
        /// </summary>
        public GoogleStatusCode StatusCode
        {
            get { return this.statusCode; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GoogleGeocodeResult"/> is successful.
        /// </summary>
        /// <value><c>true</c> if successful; otherwise, <c>false</c>.</value>
        public override bool Successful
        {
            get { return this.statusCode == GoogleStatusCode.Success; }
        }

        /// <summary>
        /// Gets the error message, or <c>null</c> if there is no error.
        /// </summary>
        /// <value>The error message, or <c>null</c> if there is no error.</value>
        public override string ErrorMessage
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}