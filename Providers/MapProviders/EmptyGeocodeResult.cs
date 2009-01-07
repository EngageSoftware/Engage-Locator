// <copyright file="EmptyGeocodeResult.cs" company="Engage Software">
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
    /// Represents an empty geocoding result
    /// </summary>
    internal class EmptyGeocodeResult : GeocodeResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyGeocodeResult"/> class.
        /// </summary>
        public EmptyGeocodeResult() 
                : base(double.NaN, double.NaN)
        {
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GoogleGeocodeResult"/> is successful.
        /// </summary>
        /// <value><c>true</c> if successful; otherwise, <c>false</c>.</value>
        public override bool Successful
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the error message, or <c>null</c> if there is no error.
        /// </summary>
        /// <value>The error message, or <c>null</c> if there is no error.</value>
        public override string ErrorMessage
        {
            get { return null; }
        }
    }
}