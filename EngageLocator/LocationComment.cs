using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Engage.Dnn.Locator.Data;

namespace Engage.Dnn.Locator
{
    public class LocationComment
    {
        private int _commentId;
        private string _comment;
        private string _submittedBy;
        private bool _approved;
        private string _locationName;

        #region

        public int CommentId
        {
            get { return _commentId; }
            set { _commentId = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public string SubmittedBy
        {
            get { return _submittedBy; }
            set { _submittedBy = value; }
        }

        public bool Approved
        {
            get { return _approved; }
            set { _approved = value; }
        }

        public string LocationName
        {
            get { return _locationName; }
            set { _locationName = value; }
        }

        #endregion

        public static LocationComment GetComment(int commentId)
        {
            return DataProvider.Instance().GetLocationComment(commentId);
        }

        public static void DeleteComment(int commentId)
        {
            DataProvider.Instance().DeleteLocatinComment(commentId);
        }

        public void Update()
        {           
            DataProvider.Instance().SaveLocationComment(this);
        }

        public void SaveComment()
        {
            DataProvider.Instance().SaveLocationComment(this);
        }

    }
}
