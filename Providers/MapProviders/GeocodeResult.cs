// <copyright file="GeocodeResult.cs" company="Engage Software">
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
    /// Abstract base class representing the results of a geocoding request
    /// </summary>
    public abstract class GeocodeResult
    {
        /// <summary>
        /// Represents an empty, unfulfilled geocode request.
        /// </summary>
        /// <value>The empty, unfulfilled geocode request.</value>
        public static readonly GeocodeResult Empty = new EmptyGeocodeResult();

        /// <summary>
        /// Backing field for <see cref="Latitude"/>
        /// </summary>
        private readonly double latitude;

        /// <summary>
        /// Backing field for <see cref="Longitude"/>
        /// </summary>
        private readonly double longitude;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodeResult"/> class.
        /// </summary>
        /// <param name="latitude">The latitude of the result.</param>
        /// <param name="longitude">The longitude of the result.</param>
        protected GeocodeResult(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        /// <summary>
        /// Gets the latitude of the result
        /// </summary>
        public double Latitude
        {
            get { return this.latitude; }
        }

        /// <summary>
        /// Gets the longitude of the result
        /// </summary>
        public double Longitude
        {
            get { return this.longitude; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GoogleGeocodeResult"/> is successful.
        /// </summary>
        /// <value><c>true</c> if successful; otherwise, <c>false</c>.</value>
        public abstract bool Successful
        {
            get;
        }

        /// <summary>
        /// Gets the error message, or <c>null</c> if there is no error.
        /// </summary>
        /// <value>The error message, or <c>null</c> if there is no error.</value>
        public abstract string ErrorMessage
        {
            get;
        }
    }
}