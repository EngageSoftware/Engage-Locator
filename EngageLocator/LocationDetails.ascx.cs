using System;
using System.Data;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Entities.Modules;

namespace Engage.Dnn.Locator
{
    public partial class LocationDetails : ModuleBase
    {
        private int _tabModuleId;

        #region Properties

        protected bool ShowLocationDetails
        {
            get
            {
                bool showDetails = false;
                ModuleController objModules = new ModuleController();

                if (objModules.GetTabModuleSettings(_tabModuleId)["ShowLocationDetails"] != null)
                {
                    if (objModules.GetTabModuleSettings(_tabModuleId)["ShowLocationDetails"].ToString() == "DetailsPage")
                        showDetails = true;
                }
                return showDetails;
            }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            btnBack.Click += btnBack_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                lblLocationId.Text = Request.QueryString["lid"];
                _tabModuleId = Convert.ToInt32(Request.QueryString["tmid"]);
                Location location = Location.GetLocation(Convert.ToInt32(lblLocationId.Text));

                lblLocationName.Text = location.Name;
                lnkLocationName.Text = location.Website;
                lnkLocationName.NavigateUrl = location.Website;
                lblLocationDetails.Text = location.LocationDetails;

                if (location.Address != String.Empty && location.Address.Contains(","))
                {
                    int length = location.Address.IndexOf(',');
                    lblLocationsAddress2.Text = location.Address.Remove(0, length);
                }
                lblLocationsAddress1.Text = location.Address;

                ListController controller = new ListController();
                ListEntryInfo region = controller.GetListEntryInfo(location.RegionId);
                lblLocationsAddress3.Text = location.City + ", " + region.Value + " " + location.PostalCode;
                lblPhoneNumber.Text = location.Phone;

                DataTable comments = location.GetLocationComments(true).Tables[0];
                if(comments.Rows.Count > 0)
                {
                    rptComments.DataSource = comments;
                    rptComments.DataBind();
                }
                else
                    rptComments.Visible = false;
            }

        }

        public void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if(!Page.IsValid) return;
            bool approved = false;
            if(Settings["ModerateComments"].ToString() == "False")
                approved = true;
            Location.InsertLocationComment(Convert.ToInt32(lblLocationId.Text), txtComment.Text, txtSubmittedBy.Text, approved);
            txtComment.Text = string.Empty;
            txtSubmittedBy.Text = string.Empty;
            Location location = Location.GetLocation(Convert.ToInt32(lblLocationId.Text));
            rptComments.DataSource = location.GetLocationComments(true);
            rptComments.DataBind();
            lblCommentSubmitted.Visible = true;
        }
    }
}