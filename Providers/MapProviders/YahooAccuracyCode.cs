// <copyright file="YahooAccuracyCode.cs" company="Engage Software">
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
    /// The accuracy of a <see cref="YahooGeocodeResult"/>
    /// </summary>
    public enum YahooAccuracyCode
    {
        /// <summary>
        /// Could not be found
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Could only find country
        /// </summary>
        Country = 1,

        /// <summary>
        /// Accurate to the region level
        /// </summary>
        State = 2,

        /// <summary>
        /// Accurate to the city level
        /// </summary>
        City = 4,

        /// <summary>
        /// Accurate to the postal code
        /// </summary>
        Zip = 5,

        /// <summary>
        /// Accurate to the postal code+2
        /// </summary>
        ZipPlus2 = -1,

        /// <summary>
        /// Accurate to the postal code +4
        /// </summary>
        ZipPlus4 = -2,

        /// <summary>
        /// Accurate to the street
        /// </summary>
        Street = 6,

        /// <summary>
        /// Accurate to the address
        /// </summary>
        Address = 8
    }
}