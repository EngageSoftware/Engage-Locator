using System;
using System.Data;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;

namespace Engage.Dnn.Locator
{
    public partial class Details : ModuleBase
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                BindData();
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if(!Page.IsValid) return;
            bool approved = false;
            if(Settings["ModerateComments"].ToString() == "False")
                approved = true;
            Location.InsertComment(Convert.ToInt32(lblLocationId.Text), txtComment.Text, txtSubmittedBy.Text, approved);
            txtComment.Text = string.Empty;
            txtSubmittedBy.Text = string.Empty;
            Location location = Location.GetLocation(Convert.ToInt32(lblLocationId.Text));
            rptComments.DataSource = location.GetComments(true);
            rptComments.DataBind();
            lblCommentSubmitted.Visible = true;
        }

        #endregion

        #region Methods

        private void BindData()
        {
                 lblLocationId.Text = Request.QueryString["lid"];
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

                DataTable comments = location.GetComments(true).Tables[0];
                if(comments.Rows.Count > 0)
                {
                    rptComments.DataSource = comments;
                    rptComments.DataBind();
                }
                else
                    rptComments.Visible = false;

            foreach (Attribute a in location.GetAttributes())
            {
                

                Literal lit = new Literal();
                lit.Text = "<div class=div_CustomAttribute" + a.AttributeId.ToString() + ">";
                plhCustomAttributes.Controls.Add(lit);

                Label l = new Label();
                string text = Localization.GetString(a.AttributeName, LocalResourceFile);
                if (text == null) text = a.AttributeName;
                l.Text = text;
                plhCustomAttributes.Controls.Add(l);

                lit = new Literal();
                lit.Text = "&nbsp;";
                plhCustomAttributes.Controls.Add(lit);


                l = new Label();
                l.Text = a.AttributeValue;
                plhCustomAttributes.Controls.Add(l);

                lit = new Literal();
                lit.Text = "<br />";
                plhCustomAttributes.Controls.Add(lit);

                lit = new Literal();
                lit.Text = "</div>";
                plhCustomAttributes.Controls.Add(lit);


            }
        }

        #endregion
        #region Properties

        protected bool ShowLocationDetails
        {
            get
            {
                
                bool showDetails = false;
                ModuleController controller = new ModuleController();
                if (controller.GetTabModuleSettings(TabModuleId)["ShowLocationDetails"] != null)
                {
                    if (controller.GetTabModuleSettings(TabModuleId)["ShowLocationDetails"].ToString() == "DetailsPage")
                        showDetails = true;
                }
                return showDetails;
            }
        }

        #endregion
         
    }
}