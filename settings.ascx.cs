using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Locator.Data;
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
                                chkAllowComments.Enabled = true;
                                chkModerateComments.Enabled = chkAllowComments.Checked;
                                cbLocationRating.Enabled = true;
                            }
                            else if (TabModuleSettings["ShowLocationDetails"].ToString() == "SamePage" || TabModuleSettings["ShowLocationDetails"].ToString() == "True")
                                rbSamePage.Checked = true;
                        }
                        else
                        {
                            rbNoDetails.Checked = true;
                        }

                        if (Null.IsNull(HostSettings.GetHostSetting("LocatorAllowRatings" + PortalId.ToString(CultureInfo.InvariantCulture))))
                            cbLocationRating.Checked = false;
                        else
                            cbLocationRating.Checked = Convert.ToBoolean(HostSettings.GetHostSetting("LocatorAllowRatings" + PortalId.ToString(CultureInfo.InvariantCulture)).ToString());

                        //set Comments Settings
                        if (Null.IsNull(HostSettings.GetHostSetting("LocatorAllowComments" + PortalId.ToString(CultureInfo.InvariantCulture))))
                            chkAllowComments.Checked = false;
                        else
                            chkAllowComments.Checked = Convert.ToBoolean(HostSettings.GetHostSetting("LocatorAllowComments" + PortalId.ToString(CultureInfo.InvariantCulture)).ToString());
                        if (Null.IsNull(HostSettings.GetHostSetting("LocatorCommentModeration" + PortalId.ToString(CultureInfo.InvariantCulture))))
                            chkModerateComments.Checked = false;
                        else
                            chkModerateComments.Checked = Convert.ToBoolean(HostSettings.GetHostSetting("LocatorCommentModeration" + PortalId.ToString(CultureInfo.InvariantCulture)).ToString());

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
                        if (!Null.IsNull(TabModuleSettings["MapType"]))
                        {
                            rblMapDisplayType.SelectedValue = TabModuleSettings["MapType"].ToString();
                        }

                        //set Submission Settings
                        if (!Null.IsNull(HostSettings.GetHostSetting("LocatorAllowSubmissions" + PortalId.ToString(CultureInfo.InvariantCulture))))
                        {
                            chkAllowLocations.Checked = Convert.ToBoolean(HostSettings.GetHostSetting("LocatorAllowSubmissions" + PortalId.ToString(CultureInfo.InvariantCulture)));
                        }
                        if (!Null.IsNull(HostSettings.GetHostSetting("LocatorSubmissionModeration" + PortalId.ToString(CultureInfo.InvariantCulture))))
                        {
                            chkModerateLocations.Checked = Convert.ToBoolean(HostSettings.GetHostSetting("LocatorSubmissionModeration" + PortalId.ToString(CultureInfo.InvariantCulture)).ToString(CultureInfo.InvariantCulture));
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

            if (string.IsNullOrEmpty(lbLocationType.SelectedValue))
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
                HostSettingsController hsc = new HostSettingsController();
                objModules.UpdateTabModuleSetting(TabModuleId, "SearchTitle", txtSearchTitle.Text);
                objModules.UpdateTabModuleSetting(TabModuleId, "Country", this.rblRestrictions.Items.FindByValue("Country").Selected.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(TabModuleId, "Radius", this.rblRestrictions.Items.FindByValue("Radius").Selected.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(TabModuleId, "DisplayProvider", this.rblProviderType.SelectedItem.Text);
                objModules.UpdateTabModuleSetting(TabModuleId, this.rblProviderType.SelectedValue + ".ApiKey", txtApiKey.Text);
                objModules.UpdateTabModuleSetting(TabModuleId, "DefaultCountry", ddlLocatorCountry.SelectedValue);
                objModules.UpdateTabModuleSetting(TabModuleId, "MapType", this.rblMapDisplayType.SelectedValue);

                hsc.UpdateHostSetting("LocatorAllowSubmissions" + PortalId, this.chkAllowLocations.Checked.ToString(CultureInfo.InvariantCulture));
                hsc.UpdateHostSetting("LocatorSubmissionModeration" + PortalId, this.chkModerateLocations.Checked.ToString(CultureInfo.InvariantCulture));
                
                string locationTypeList = GetLocationTypeList();

                objModules.UpdateTabModuleSetting(TabModuleId, "DisplayTypes", locationTypeList);

                if(rbNoDetails.Checked)
                    objModules.UpdateTabModuleSetting(TabModuleId, "ShowLocationDetails", "NoDetails");
                else if(rbSamePage.Checked)
                    objModules.UpdateTabModuleSetting(TabModuleId, "ShowLocationDetails", "SamePage");
                else if(rbDetailsPage.Checked)
                    objModules.UpdateTabModuleSetting(TabModuleId, "ShowLocationDetails", "DetailsPage");

                hsc.UpdateHostSetting("LocatorAllowRatings" + PortalId.ToString(CultureInfo.InvariantCulture), cbLocationRating.Checked.ToString(CultureInfo.InvariantCulture));
                hsc.UpdateHostSetting("LocatorAllowComments" + PortalId.ToString(CultureInfo.InvariantCulture), chkAllowComments.Checked.ToString(CultureInfo.InvariantCulture));
                hsc.UpdateHostSetting("LocatorCommentModeration" + PortalId.ToString(CultureInfo.InvariantCulture), chkModerateComments.Checked.ToString(CultureInfo.InvariantCulture));

                objModules.UpdateTabModuleSetting(TabModuleId, "ShowDefaultDisplay", rbDisplayAll.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(TabModuleId, "ShowMapDefaultDisplay", rbShowMap.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(TabModuleId, "SearchAddress", chkAddress.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(TabModuleId, "SearchCityRegion", chkCityRegion.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(TabModuleId, "SearchPostalCode", chkPostalCode.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(TabModuleId, "SearchCountry", chkCountry.Checked.ToString(CultureInfo.InvariantCulture));

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
            chkAllowComments.Enabled = chkModerateComments.Enabled = cbLocationRating.Enabled = rbDetailsPage.Checked;
            chkModerateComments.Enabled = chkAllowComments.Checked = chkModerateComments.Checked = rbDetailsPage.Checked;
        }

        protected void chkAllowComments_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllowComments.Checked)
            {
                chkModerateComments.Enabled = chkAllowComments.Checked;
            }
            else
            {
                chkModerateComments.Enabled = false;
                chkModerateComments.Checked = false;
            }
        }

        protected void chkAllowLocations_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllowLocations.Checked)
            {
                chkModerateLocations.Enabled = chkAllowLocations.Checked;
            }
            else
            {
                chkModerateLocations.Checked = false;
                chkModerateLocations.Enabled = false;

            }
        }
    }
}


