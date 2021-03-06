//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Data;
using System.Configuration;
using System.Web;
using Engage.Dnn.Locator.Data;

namespace Engage.Dnn.Locator
{
    public class Comment
    {
        #region Properties

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _commentId;
        public int CommentId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _commentId; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _commentId = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _text;
        public string Text
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _text; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _text = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _submittedBy;
        public string SubmittedBy
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _submittedBy; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _submittedBy = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _approved;
        public bool Approved
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _approved; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _approved = value; }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _locationName;

        public string LocationName
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get { return _locationName; }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set { _locationName = value; }
        }

        #endregion

        #region Methods

        public void Update()
        {           
            DataProvider.Instance().SaveComment(this);
        }

        public void SaveComment()
        {
            DataProvider.Instance().SaveComment(this);
        }

        #endregion

        #region Static Methods

        public static Comment GetComment(int commentId)
        {
            return DataProvider.Instance().GetComment(commentId);
        }

        public static void DeleteComment(int commentId)
        {
            DataProvider.Instance().DeleteComment(commentId);
        }

        #endregion

    }
}
