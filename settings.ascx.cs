using System;
using System.Data;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Locator.Data;
using DotNetNuke.Common;
using Engage.Dnn.Locator.Maps;

namespace Engage.Dnn.Locator
{
    partial class Settings : ModuleSettingsBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (AJAX.IsInstalled())
            {
                AJAX.RegisterScriptManager();
            }
        }

        #region Base Method Implementations

        public override void LoadSettings()
        {
            try
            {
                    if (!Page.IsPostBack)
                    {
                        ListController controller = new ListController();
                        ListEntryInfoCollection countries = controller.GetListEntryInfoCollection("Country");

                        ddlLocatorCountry.DataSource = countries;
                        ddlLocatorCountry.DataTextField = "Text";
                        ddlLocatorCountry.DataValueField = "EntryId";
                        ddlLocatorCountry.DataBind();
                        ddlLocatorCountry.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));
                        ddlLocatorCountry.SelectedIndex = 0;

                        //set Default Country
                        if (TabModuleSettings["DefaultCountry"] != null)
                        {
                            ddlLocatorCountry.SelectedValue = TabModuleSettings["DefaultCountry"].ToString();
                        }
                        //set Search Instructions
                        if (TabModuleSettings["SearchTitle"] != null)
                        {
                            txtSearchTitle.Text = TabModuleSettings["SearchTitle"].ToString();
                        }
                        //set Search Restrictions
                        if (TabModuleSettings["Country"] != null && TabModuleSettings["Country"].ToString() == "True")
                        {
                            rblRestrictions.Items.FindByValue("Country").Selected = true;
                        }
                        else if (TabModuleSettings["Radius"] != null && TabModuleSettings["Radius"].ToString() == "True")
                        {
                            this.rblRestrictions.SelectedValue = this.rblRestrictions.Items.FindByText("Radius").Value;
                        }
                        else 
                        {
                            this.rblRestrictions.SelectedValue = this.rblRestrictions.Items.FindByText("None").Value;
                        }
                        //set Show Location Details
                        if (TabModuleSettings["ShowLocationDetails"] != null)
                        {
                            if (TabModuleSettings["ShowLocationDetails"].ToString() == "NoDetails" || TabModuleSettings["ShowLocationDetails"].ToString() == "False")
                                rbNoDetails.Checked = true;
                            else if (TabModuleSettings["ShowLocationDetails"].ToString() == "DetailsPage")
                            {
                                rbDetailsPage.Checked = true;
                                cbLocationComments.Enabled = true;
                                cbModerateComments.Enabled = true;
                            }
                            else if (TabModuleSettings["ShowLocationDetails"].ToString() == "SamePage" || TabModuleSettings["ShowLocationDetails"].ToString() == "True")
                                rbSamePage.Checked = true;
                        }
                        else
                        {
                            rbNoDetails.Checked = true;
                        }
                        //set Allow Comments
                        if (TabModuleSettings["LocationComments"] != null)
                            cbLocationComments.Checked = Convert.ToBoolean(TabModuleSettings["LocationComments"].ToString());
                        else
                            cbLocationComments.Checked = false;

                        //set Moderate Comments
                        using (DotNetNuke.Entities.Host)
                        {
                            if (Null.IsNull(HostSettings.GetHostSetting("LocatorCommentModeration" + PortalId)
                                cbModerateComments.Checked = false;
                            else
                                cbModerateComments.Checked = Convert.ToBoolean(HostSettings.GetHostSetting("LocatorCommentModeration" + PortalId).ToString());
                        }
                        //if (TabModuleSettings["ModerateComments"] != null)
                        //    cbModerateComments.Checked = Convert.ToBoolean(TabModuleSettings["ModerateComments"].ToString());
                        //else
                        //    cbModerateComments.Checked = false;
                        //set Default Display

                        if (TabModuleSettings["ShowDefaultDisplay"] != null && TabModuleSettings["ShowDefaultDisplay"].ToString() == "True")
                        {
                            rbDisplayAll.Checked = true;
                        }
                        else if (TabModuleSettings["ShowMapDefaultDisplay"] != null && TabModuleSettings["ShowMapDefaultDisplay"].ToString() == "True")
                        {
                            rbShowMap.Checked = true;
                        }
                        else
                        {
                            rbSearch.Checked = true;
                        }
                        //set search parameters
                        if (TabModuleSettings["SearchAddress"] != null)
                        {
                            chkAddress.Checked = Convert.ToBoolean(TabModuleSettings["SearchAddress"].ToString());
                        }
                        if (TabModuleSettings["SearchCityRegion"] != null)
                        {
                            chkCityRegion.Checked = Convert.ToBoolean(TabModuleSettings["SearchCityRegion"].ToString());
                        }
                        if (TabModuleSettings["SearchPostalCode"] != null)
                        {
                            chkPostalCode.Checked = Convert.ToBoolean(TabModuleSettings["SearchPostalCode"].ToString());
                        }
                        if (TabModuleSettings["SearchCountry"] != null)
                        {
                            chkCountry.Checked = Convert.ToBoolean(TabModuleSettings["SearchCountry"].ToString());
                        }
                        //set MapType
                        if(TabModuleSettings["MapType"] != null)
                        {
                            ddlMapType.SelectedValue = TabModuleSettings["MapType"].ToString();
                        }
                        //set SubmisionSettings
                        if(TabModuleSettings["AllowSubmissions"] != null)
                        {
                            cbAllowSubmissions.Checked= Convert.ToBoolean(TabModuleSettings["AllowSubmissions"]);
                        }
                        if (TabModuleSettings["ModerateSubmissions"] != null)
                        {
                            cbSubmissionModeration.Checked = Convert.ToBoolean(TabModuleSettings["ModerateSubmissions"]);
                        }
                        //fill gridview with existing locator modules
                        DataTable modules = DataProvider.Instance().GetEngageLocatorTabModules(PortalId);
                        gvTabModules.DataSource = modules;
                        gvTabModules.DataBind();

                        //set existing module
                        if (TabModuleSettings["DisplayResultsTabId"] != null)
                        {
                            foreach (GridViewRow dr in gvTabModules.Rows)
                            {
                                RadioButton rb = (RadioButton)dr.FindControl("rbLocatorModule");
                                Label lblTabId = (Label)dr.FindControl("lblTabId");
                                if (lblTabId.Text == TabModuleSettings["DisplayResultsTabId"].ToString())
                                {
                                    rb.Checked = true;
                                }
                                else
                                {
                                    rb.Checked = false;
                                }
                            }
                        }
                        else
                        {
                            foreach (GridViewRow dr in gvTabModules.Rows)
                            {
                                RadioButton rb = (RadioButton)dr.FindControl("rbLocatorModule");
                                Label lblTabId = (Label)dr.FindControl("lblTabId");
                                if (lblTabId.Text == TabId.ToString())
                                {
                                    rb.Checked = true;
                                }
                            }
                        }

                        DisplayProviderType();
                        DisplayAPI();
                        DisplayLocationTypes();

                        //todo: put logic in to determine IsExpanded
                        dshMapProvider.IsExpanded = false;
                        dshSubmissionSettings.IsExpanded = false;
                        this.dshDisplaySetting.IsExpanded = false;
                        dshSearchSettings.IsExpanded = false;
                    }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void DisplayProviderType()
        {
            this.rblProviderType.DataSource = MapProviderType.GetMapProviderTypes();
            this.rblProviderType.DataTextField = "Name";
            this.rblProviderType.DataValueField = "ClassName";
            this.rblProviderType.DataBind();
            string displayProvider = Convert.ToString(TabModuleSettings["DisplayProvider"]);
            ListItem li = this.rblProviderType.Items.FindByText(displayProvider);
            if (li != null) { this.rblProviderType.SelectedValue = li.Value; }


        }

        private void DisplayAPI()
        {
            txtApiKey.Text = Convert.ToString(TabModuleSettings[this.rblProviderType.SelectedValue + ".ApiKey"]);
            txtApiKey.Focus();
        }

        private void DisplayLocationTypes()
        {
            DataTable dt = LocationType.GetLocationTypes();
            lbLocationType.DataSource = dt;
            lbLocationType.DataTextField = "LocationTypeName";
            lbLocationType.DataValueField = "LocationTypeId";
            lbLocationType.DataBind();

            string displayTypes = Convert.ToString(TabModuleSettings["DisplayTypes"]);
            string[] displayTypesArray = displayTypes.Split(',');

            //lbLocationType.SelectedIndex = 0;

            foreach (string s in displayTypesArray)
            {
                foreach (ListItem locationItems in lbLocationType.Items)
                {
                    if (locationItems.Value == s)
                    {
                        locationItems.Selected = true;
                    }
                } 
            }

            if (lbLocationType.SelectedValue == null || lbLocationType.SelectedValue == "")
            {
                if (lbLocationType.Items[0].Text == "Default")
                {
                    lbLocationType.SelectedIndex = 0;
                }

            }
        }

        public override void UpdateSettings()
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                ModuleController objModules = new ModuleController();
                objModules.UpdateTabModuleSetting(TabModuleId, "SearchTitle", txtSearchTitle.Text);
                objModules.UpdateTabModuleSetting(TabModuleId, "Country", this.rblRestrictions.Items.FindByValue("Country").Selected.ToString());
                objModules.UpdateTabModuleSetting(TabModuleId, "Radius", this.rblRestrictions.Items.FindByValue("Radius").Selected.ToString());
                objModules.UpdateTabModuleSetting(TabModuleId, "DisplayProvider", this.rblProviderType.SelectedItem.Text);
                objModules.UpdateTabModuleSetting(TabModuleId, this.rblProviderType.SelectedValue + ".ApiKey", txtApiKey.Text);
                objModules.UpdateTabModuleSetting(TabModuleId, "DefaultCountry", ddlLocatorCountry.SelectedValue);
                objModules.UpdateTabModuleSetting(TabModuleId, "MapType", this.ddlMapType.SelectedValue);
                objModules.UpdateTabModuleSetting(TabModuleId, "AllowSubmissions", this.cbAllowSubmissions.Checked.ToString());
                using (DotNetNuke.Entities.Host)
                {
                    DotNetNuke.Entities.Host.HostSettingsController hsc = new DotNetNuke.Entities.Host.HostSettingsController();
                    hsc.UpdateHostSetting("LocatorSubmissionModeration" + PortalId, cbModerateComments.Checked.ToString());
                }
                //objModules.UpdateTabModuleSetting(TabModuleId, "ModerateSubmissions", this.cbSubmissionModeration.Checked.ToString());

                string locationTypeList = GetLocationTypeList();


                objModules.UpdateTabModuleSetting(TabModuleId, "DisplayTypes", locationTypeList);

                if(rbNoDetails.Checked)
                    objModules.UpdateTabModuleSetting(TabModuleId, "ShowLocationDetails", "NoDetails");
                else if(rbSamePage.Checked)
                    objModules.UpdateTabModuleSetting(TabModuleId, "ShowLocationDetails", "SamePage");
                else if(rbDetailsPage.Checked)
                    objModules.UpdateTabModuleSetting(TabModuleId, "ShowLocationDetails", "DetailsPage");
                objModules.UpdateTabModuleSetting(TabModuleId, "LocationComments", cbLocationComments.Checked.ToString());
                using (DotNetNuke.Entities.Host)
                {
                    DotNetNuke.Entities.Host.HostSettingsController hsc = new DotNetNuke.Entities.Host.HostSettingsController();
                    hsc.UpdateHostSetting("LocatorCommentModeration" + PortalId, cbModerateComments.Checked.ToString());
                }
                //objModules.UpdateTabModuleSetting(TabModuleId, "ModerateComments", cbModerateComments.Checked.ToString());

                objModules.UpdateTabModuleSetting(TabModuleId, "ShowDefaultDisplay", rbDisplayAll.Checked.ToString());
                objModules.UpdateTabModuleSetting(TabModuleId, "ShowMapDefaultDisplay", rbShowMap.Checked.ToString());

                objModules.UpdateTabModuleSetting(TabModuleId, "SearchAddress", chkAddress.Checked.ToString());
                objModules.UpdateTabModuleSetting(TabModuleId, "SearchCityRegion", chkCityRegion.Checked.ToString());
                objModules.UpdateTabModuleSetting(TabModuleId, "SearchPostalCode", chkPostalCode.Checked.ToString());
                objModules.UpdateTabModuleSetting(TabModuleId, "SearchCountry", chkCountry.Checked.ToString());

                foreach (GridViewRow dr in gvTabModules.Rows)
                {
                    RadioButton rb = (RadioButton)dr.FindControl("rbLocatorModule");
                    Label lblTabId = (Label)dr.FindControl("lblTabId");
                    if (rb.Checked)
                    {
                        objModules.UpdateTabModuleSetting(TabModuleId, "DisplayResultsTabId", lblTabId.Text);
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private string GetLocationTypeList()
        {
            string locationTypeList = "";
            foreach (ListItem li in lbLocationType.Items)
            {
                if (li.Selected)
                {
                    if (locationTypeList.Length == 0)
                    {
                        locationTypeList = li.Value;
                    }
                    else
                    {
                        locationTypeList = locationTypeList + "," + li.Value; 
                    }
                }
            }

            if (locationTypeList.Length == 0)
            {
                locationTypeList = lbLocationType.Items[0].Value;
            }

            return locationTypeList;
        }

        #endregion

        protected void ddlProviderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList) sender;
            txtApiKey.Text = Convert.ToString(TabModuleSettings[ddl.SelectedValue + ".ApiKey"]);
        }

        protected void lbEditLocationType_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId, "AttributeDefinitions", "mid=" + ModuleId + "&ltid=" + lbLocationType.SelectedValue));
        }

        protected void lbUpdateLocationType_Click(object sender, EventArgs e)
        {
            if (lbUpdateLocationType.Text == "Update")
            {
                DataProvider.Instance().UpdateLocationType(Convert.ToInt32(lbLocationType.SelectedItem.Value), txtEditLocationType.Text);
            }
            else if (lbUpdateLocationType.Text == "Save")
            {
                DataProvider.Instance().InsertLocationType(txtEditLocationType.Text);
            }
            dvLocationType.Visible = true;
            dvLocationTypeEdit.Visible = false;
            lblLocationTypeNotInUse.Visible = false;
            lblLocationTypeInst.Visible = true;
            DisplayLocationTypes();
        }

        protected void lbAddLocationType_Click(object sender, EventArgs e)
        {
            dvLocationType.Visible = false;
            dvLocationTypeEdit.Visible = true;
            lblLocationTypeNotInUse.Visible = false;
            lblLocationTypeInst.Visible = false;
            lbUpdateLocationType.Text = "Save";
            txtEditLocationType.Text = String.Empty;
            txtEditLocationType.Focus();
        }

        protected void lbDeleteLocationType_Click(object sender, EventArgs e)
        {
            if(LocationType.GetLocationTypeInUse(lbLocationType.SelectedItem.Text))
            {
                lblLocationTypeNotInUse.Visible = true;
                lblLocationTypeNotInUse.Text = Localization.GetString("lblLocationTypeNotInUse", LocalResourceFile);
            }
            else
            {
                DataProvider.Instance().DeleteLocationType(Convert.ToInt32(lbLocationType.SelectedItem.Value));
                DisplayLocationTypes();
                lblLocationTypeNotInUse.Visible = false;                
            }
        }

        protected void rblProviderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayAPI();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void apiKey_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.rblProviderType.SelectedItem != null)
            {
                MapProvider mp = MapProvider.CreateInstance(this.rblProviderType.SelectedValue);
                args.IsValid = mp.IsKeyValid(txtApiKey.Text);
            }
            else
            {
                args.IsValid = false;
                dshMapProvider.IsExpanded = true;
            }
        }

        protected void lbCancelLocationType_Click(object sender, EventArgs e)
        {
            dvLocationType.Visible = true;
            dvLocationTypeEdit.Visible = false;
            lblLocationTypeNotInUse.Visible = false;
            lblLocationTypeInst.Visible = true;

        }

        protected void cvLocatorCountry_OnServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddlLocatorCountry.SelectedIndex > 0)
                args.IsValid = true;
            else
            {
                args.IsValid = false;
                dshMapProvider.IsExpanded = true;
            }
        }


        protected void cvSearchOptions_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (chkAddress.Checked || chkCityRegion.Checked || chkPostalCode.Checked || chkPostalCode.Checked || chkCountry.Checked)
                args.IsValid = true;
            else
            {
                args.IsValid = false;
                dshSearchSettings.IsExpanded = true;
            }
        }

        protected void cvProviderType_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (rblProviderType.SelectedItem == null)
            {
                args.IsValid = false;
                dshMapProvider.IsExpanded = true;
            }
        }

        protected void rbLocatorModules_CheckChanged(object sender, EventArgs e)
        {
            RadioButton rbselected = (RadioButton)sender;

            foreach (GridViewRow dr in gvTabModules.Rows)
            {
                RadioButton rb = (RadioButton)dr.FindControl("rbLocatorModule");
                if (rb == rbselected)
                {
                    rb.Checked = true;
                }
                else
                {
                    rb.Checked = false;
                }
            }
        }

        protected void cvLocatorModules_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string tabId = string.Empty;
            foreach (GridViewRow row in gvTabModules.Rows)
            {
                RadioButton rb = (RadioButton)row.FindControl("rbLocatorModule");
                Label lblTabId = (Label)row.FindControl("lblTabId");

                if (rb.Checked)
                {
                    tabId = lblTabId.Text;
                }
            }

            if (tabId != string.Empty)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
                dshSearchSettings.IsExpanded = true;
            }
        }

        protected void rbLoctionDetails_CheckChanged(object sender, EventArgs e)
        {
            cbLocationComments.Enabled = rbDetailsPage.Checked;
            cbModerateComments.Enabled = rbDetailsPage.Checked;
        }
    }
}


