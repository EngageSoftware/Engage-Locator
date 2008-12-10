// <copyright file="Location.cs" company="Engage Software">
// Engage: Locator - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Locator.JavaScript
{
    /// <summary>
    /// A JSON-friendly representation of a location
    /// </summary>
    public struct Location
    {
        /// <summary>
        /// The latitude of this location
        /// </summary>
        public double? Latitude;

        /// <summary>
        /// The longitude of this location
        /// </summary>
        public double? Longitude;

        /// <summary>
        /// The address of this location
        /// </summary>
        public string Address;

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> struct.
        /// </summary>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        /// <param name="address">The address of the location.</param>
        public Location(double? latitude, double? longitude, string address)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Address = address;
        }
    }
}