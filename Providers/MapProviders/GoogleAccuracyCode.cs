// <copyright file="GoogleAccuracyCode.cs" company="Engage Software">
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
    /// The accuracy of the geocode request to Google
    /// </summary>
    public enum GoogleAccuracyCode
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
        /// Accurate to the region (state, province, prefecture, etc.) level
        /// </summary>
        Region = 2,

        /// <summary>
        /// Accurate to the sub-region (county, municipality, etc.) level
        /// </summary>
        Subregion = 3,

        /// <summary>
        /// Accurate to the city level
        /// </summary>
        Town = 4,

        /// <summary>
        /// Accurate to the postal code
        /// </summary>
        PostalCode = 5,

        /// <summary>
        /// Accurate to the street
        /// </summary>
        Street = 6,

        /// <summary>
        /// Accurate to the intersection
        /// </summary>
        Intersection = 7,

        /// <summary>
        /// Accurate to the address
        /// </summary>
        Address = 8,

        /// <summary>
        /// Accurate to the premise (building name, property name, shopping center, etc.) level
        /// </summary>
        Premise = 9
    }
}