// <copyright file="YahooStatusCode.cs" company="Engage Software">
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
    /// The possible values of status code returned from the Yahoo geocoding service
    /// </summary>
    public enum YahooStatusCode
    {
        /// <summary>
        /// A successful request
        /// </summary>
        Success = 200,

        /// <summary>
        /// The parameters passed to the service did not match as expected. The Message should tell you what was missing or incorrect.
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// You do not have permission to access this resource, or are over your rate limit.
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// An internal problem prevented us from returning data to you.
        /// </summary>
        ServiceUnavailable = 503
    }
}