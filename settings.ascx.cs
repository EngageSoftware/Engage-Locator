namespace Engage.Dnn.Locator
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;
    using Data;
    using DotNetNuke.Common.Lists;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Maps;

    public partial class Settings : ModuleSettingsBase
    {
        public override void LoadSettings()
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    ListController controller = new ListController();
                    ListEntryInfoCollection countries = controller.GetListEntryInfoCollection("Country");

                    this.ddlLocatorCountry.DataSource = countries;
                    this.ddlLocatorCountry.DataTextField = "Text";
                    this.ddlLocatorCountry.DataValueField = "EntryId";
                    this.ddlLocatorCountry.DataBind();
                    this.ddlLocatorCountry.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), "-1"));
                    this.ddlLocatorCountry.SelectedIndex = 0;

                    //set Default Country
                    if (this.TabModuleSettings["DefaultCountry"] != null)
                    {
                        this.ddlLocatorCountry.SelectedValue = this.TabModuleSettings["DefaultCountry"].ToString();
                    }
                    //set Search Instructions
                    if (this.TabModuleSettings["SearchTitle"] != null)
                    {
                        this.txtSearchTitle.Text = this.TabModuleSettings["SearchTitle"].ToString();
                    }
                    //set Search Restrictions
                    if (this.TabModuleSettings["Country"] != null && this.TabModuleSettings["Country"].ToString() == "True")
                    {
                        this.rblRestrictions.Items.FindByValue("Country").Selected = true;
                    }
                    else if (this.TabModuleSettings["Radius"] != null && this.TabModuleSettings["Radius"].ToString() == "True")
                    {
                        this.rblRestrictions.SelectedValue = this.rblRestrictions.Items.FindByText("Radius").Value;
                    }
                    else
                    {
                        this.rblRestrictions.SelectedValue = this.rblRestrictions.Items.FindByText("None").Value;
                    }
                    //set Show Location Details
                    if (this.TabModuleSettings["ShowLocationDetails"] != null)
                    {
                        if (this.TabModuleSettings["ShowLocationDetails"].ToString() == "NoDetails"
                            || this.TabModuleSettings["ShowLocationDetails"].ToString() == "False")
                        {
                            this.rbNoDetails.Checked = true;
                        }
                        else if (this.TabModuleSettings["ShowLocationDetails"].ToString() == "DetailsPage")
                        {
                            this.rbDetailsPage.Checked = true;
                            this.chkAllowComments.Enabled = true;
                            this.chkModerateComments.Enabled = this.chkAllowComments.Checked;
                            this.cbLocationRating.Enabled = true;
                        }
                        else if (this.TabModuleSettings["ShowLocationDetails"].ToString() == "SamePage"
                                 || this.TabModuleSettings["ShowLocationDetails"].ToString() == "True")
                        {
                            this.rbSamePage.Checked = true;
                        }
                    }
                    else
                    {
                        this.rbNoDetails.Checked = true;
                    }

                    this.cbLocationRating.Checked = Utility.GetBooleanPortalSetting("LocatorAllowRatings", this.PortalId, false);

                    // set Comments Settings
                    this.chkAllowComments.Checked = Utility.GetBooleanPortalSetting("LocatorAllowComments", this.PortalId, false);
                    this.chkModerateComments.Checked = Utility.GetBooleanPortalSetting("LocatorCommentModeration", this.PortalId, false);

                    if (this.TabModuleSettings["ShowDefaultDisplay"] != null && this.TabModuleSettings["ShowDefaultDisplay"].ToString() == "True")
                    {
                        this.rbDisplayAll.Checked = true;
                    }
                    else if (this.TabModuleSettings["ShowMapDefaultDisplay"] != null && this.TabModuleSettings["ShowMapDefaultDisplay"].ToString() == "True")
                    {
                        this.rbShowMap.Checked = true;
                    }
                    else
                    {
                        this.rbSearch.Checked = true;
                    }

                    // set search parameters
                    if (this.TabModuleSettings["SearchAddress"] != null)
                    {
                        this.chkAddress.Checked = Convert.ToBoolean(this.TabModuleSettings["SearchAddress"].ToString(), CultureInfo.InvariantCulture);
                    }

                    if (this.TabModuleSettings["SearchCityRegion"] != null)
                    {
                        this.chkCityRegion.Checked = Convert.ToBoolean(this.TabModuleSettings["SearchCityRegion"].ToString(), CultureInfo.InvariantCulture);
                    }

                    if (this.TabModuleSettings["SearchPostalCode"] != null)
                    {
                        this.chkPostalCode.Checked = Convert.ToBoolean(this.TabModuleSettings["SearchPostalCode"].ToString(), CultureInfo.InvariantCulture);
                    }

                    if (this.TabModuleSettings["SearchCountry"] != null)
                    {
                        this.chkCountry.Checked = Convert.ToBoolean(this.TabModuleSettings["SearchCountry"].ToString(), CultureInfo.InvariantCulture);
                    }

                    // set MapType
                    if (!Null.IsNull(this.TabModuleSettings["MapType"]))
                    {
                        this.rblMapDisplayType.SelectedValue = this.TabModuleSettings["MapType"].ToString();
                    }

                    // set Submission Settings
                    if (!Null.IsNull(HostSettings.GetHostSetting("LocatorAllowSubmissions" + this.PortalId.ToString(CultureInfo.InvariantCulture))))
                    {
                        this.chkAllowLocations.Checked = Convert.ToBoolean(HostSettings.GetHostSetting("LocatorAllowSubmissions" + this.PortalId.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
                    }

                    if (!Null.IsNull(HostSettings.GetHostSetting("LocatorSubmissionModeration" + this.PortalId.ToString(CultureInfo.InvariantCulture))))
                    {
                        this.chkModerateLocations.Checked = Convert.ToBoolean(HostSettings.GetHostSetting("LocatorSubmissionModeration" + this.PortalId.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
                    }

                    // fill gridview with existing locator modules
                    DataTable modules = DataProvider.Instance().GetEngageLocatorTabModules(this.PortalId);
                    this.gvTabModules.DataSource = modules;
                    this.gvTabModules.DataBind();

                    // set existing module
                    if (this.TabModuleSettings["DisplayResultsTabId"] != null)
                    {
                        foreach (GridViewRow dr in this.gvTabModules.Rows)
                        {
                            RadioButton rb = (RadioButton)dr.FindControl("rbLocatorModule");
                            Label lblTabId = (Label)dr.FindControl("lblTabId");
                            if (lblTabId.Text == this.TabModuleSettings["DisplayResultsTabId"].ToString())
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
                        foreach (GridViewRow dr in this.gvTabModules.Rows)
                        {
                            RadioButton rb = (RadioButton)dr.FindControl("rbLocatorModule");
                            Label lblTabId = (Label)dr.FindControl("lblTabId");
                            if (lblTabId.Text == this.TabId.ToString(CultureInfo.InvariantCulture))
                            {
                                rb.Checked = true;
                            }
                        }
                    }

                    this.DisplayProviderType();
                    this.DisplayAPI();
                    this.DisplayLocationTypes();

                    // todo: put logic in to determine IsExpanded
                    this.dshMapProvider.IsExpanded = false;
                    this.dshSubmissionSettings.IsExpanded = false;
                    this.dshDisplaySetting.IsExpanded = false;
                    this.dshSearchSettings.IsExpanded = false;
                }
            }
            catch (Exception exc) 
            {
                // Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            try
            {
                ModuleController objModules = new ModuleController();
                HostSettingsController hsc = new HostSettingsController();
                objModules.UpdateTabModuleSetting(this.TabModuleId, "SearchTitle", this.txtSearchTitle.Text);
                objModules.UpdateTabModuleSetting(this.TabModuleId, "Country", this.rblRestrictions.Items.FindByValue("Country").Selected.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "Radius", this.rblRestrictions.Items.FindByValue("Radius").Selected.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "DisplayProvider", this.rblProviderType.SelectedItem.Text);
                objModules.UpdateTabModuleSetting(this.TabModuleId, this.rblProviderType.SelectedValue + ".ApiKey", this.txtApiKey.Text);
                objModules.UpdateTabModuleSetting(this.TabModuleId, "DefaultCountry", this.ddlLocatorCountry.SelectedValue);
                objModules.UpdateTabModuleSetting(this.TabModuleId, "MapType", this.rblMapDisplayType.SelectedValue);

                hsc.UpdateHostSetting("LocatorAllowSubmissions" + this.PortalId, this.chkAllowLocations.Checked.ToString(CultureInfo.InvariantCulture));
                hsc.UpdateHostSetting("LocatorSubmissionModeration" + this.PortalId, this.chkModerateLocations.Checked.ToString(CultureInfo.InvariantCulture));

                objModules.UpdateTabModuleSetting(this.TabModuleId, "DisplayTypes", this.GetLocationTypeList());

                if (this.rbNoDetails.Checked)
                {
                    objModules.UpdateTabModuleSetting(this.TabModuleId, "ShowLocationDetails", "NoDetails");
                }
                else if (this.rbSamePage.Checked)
                {
                    objModules.UpdateTabModuleSetting(this.TabModuleId, "ShowLocationDetails", "SamePage");
                }
                else if (this.rbDetailsPage.Checked)
                {
                    objModules.UpdateTabModuleSetting(this.TabModuleId, "ShowLocationDetails", "DetailsPage");
                }

                hsc.UpdateHostSetting(
                        "LocatorAllowRatings" + this.PortalId.ToString(CultureInfo.InvariantCulture),
                        this.cbLocationRating.Checked.ToString(CultureInfo.InvariantCulture));
                hsc.UpdateHostSetting(
                        "LocatorAllowComments" + this.PortalId.ToString(CultureInfo.InvariantCulture),
                        this.chkAllowComments.Checked.ToString(CultureInfo.InvariantCulture));
                hsc.UpdateHostSetting(
                        "LocatorCommentModeration" + this.PortalId.ToString(CultureInfo.InvariantCulture),
                        this.chkModerateComments.Checked.ToString(CultureInfo.InvariantCulture));

                objModules.UpdateTabModuleSetting(
                        this.TabModuleId, "ShowDefaultDisplay", this.rbDisplayAll.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(
                        this.TabModuleId, "ShowMapDefaultDisplay", this.rbShowMap.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "SearchAddress", this.chkAddress.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(
                        this.TabModuleId, "SearchCityRegion", this.chkCityRegion.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(
                        this.TabModuleId, "SearchPostalCode", this.chkPostalCode.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "SearchCountry", this.chkCountry.Checked.ToString(CultureInfo.InvariantCulture));

                foreach (GridViewRow dr in this.gvTabModules.Rows)
                {
                    RadioButton rb = (RadioButton)dr.FindControl("rbLocatorModule");
                    Label lblTabId = (Label)dr.FindControl("lblTabId");
                    if (rb.Checked)
                    {
                        objModules.UpdateTabModuleSetting(this.TabModuleId, "DisplayResultsTabId", lblTabId.Text);
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Raises the.<see cref="!:!:E:System.Web.UI.Control.Init" />Event.
        /// </summary>
        /// <param name="e">An. <see cref="!:!:T:System.EventArgs" /> Object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (AJAX.IsInstalled())
            {
                AJAX.RegisterScriptManager();
            }
        }

        /// <summary>
        /// Handles the ServerValidate event of the apiKey control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The. <see cref="!:T:System.Web.UI.WebControls.ServerValidateEventArgs" /> Instance containing the event data.</param>
        protected void apiKey_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.rblProviderType.SelectedItem != null)
            {
                MapProvider mp = MapProvider.CreateInstance(this.rblProviderType.SelectedValue);
                args.IsValid = mp.IsKeyValid(this.txtApiKey.Text);
            }
            else
            {
                args.IsValid = false;
                this.dshMapProvider.IsExpanded = true;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkAllowComments control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The. <see cref="!:T:System.EventArgs" /> Instance containing the event data.</param>
        protected void chkAllowComments_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllowComments.Checked)
            {
                this.chkModerateComments.Enabled = this.chkAllowComments.Checked;
            }
            else
            {
                this.chkModerateComments.Enabled = false;
                this.chkModerateComments.Checked = false;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkAllowLocations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The. <see cref="!:T:System.EventArgs" /> Instance containing the event data.</param>
        protected void chkAllowLocations_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllowLocations.Checked)
            {
                this.chkModerateLocations.Enabled = this.chkAllowLocations.Checked;
            }
            else
            {
                this.chkModerateLocations.Checked = false;
                this.chkModerateLocations.Enabled = false;
            }
        }

        /// <summary>
        /// Handles the OnServerValidate event of the LocatorCountryValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The. <see cref="System.Web.UI.WebControls.ServerValidateEventArgs" /> Instance containing the event data.</param>
        protected void LocatorCountryValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.ddlLocatorCountry.SelectedIndex > 0)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
                this.dshMapProvider.IsExpanded = true;
            }
        }

        /// <summary>
        /// Handles the ServerValidate event of the SearchResultsModuleValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The. <see cref="System.Web.UI.WebControls.ServerValidateEventArgs" /> Instance containing the event data.</param>
        protected void SearchResultsModuleValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string tabId = string.Empty;
            foreach (GridViewRow row in this.gvTabModules.Rows)
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
                this.dshSearchSettings.IsExpanded = true;
            }
        }

        /// <summary>
        /// Handles the ServerValidate event of the ProviderTypeValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The. <see cref="!:T:System.Web.UI.WebControls.ServerValidateEventArgs" /> Instance containing the event data.</param>
        protected void ProviderTypeValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.rblProviderType.SelectedItem == null)
            {
                args.IsValid = false;
                this.dshMapProvider.IsExpanded = true;
            }
        }

        /// <summary>
        /// Handles the ServerValidate event of the SearchOptionsValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The. <see cref="!:T:System.Web.UI.WebControls.ServerValidateEventArgs" /> Instance containing the event data.</param>
        protected void SearchOptionsValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.chkAddress.Checked || this.chkCityRegion.Checked || this.chkPostalCode.Checked || this.chkPostalCode.Checked
                || this.chkCountry.Checked)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
                this.dshSearchSettings.IsExpanded = true;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlProviderType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ddlProviderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            this.txtApiKey.Text = Dnn.Utility.GetStringSetting(this.Settings, ddl.SelectedValue + ".ApiKey");
        }

        /// <summary>
        /// Handles the CheckChanged event of the rbLocatorModules control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The. <see cref="System.EventArgs" /> Instance containing the event data.</param>
        protected void rbLocatorModules_CheckChanged(object sender, EventArgs e)
        {
            RadioButton rbselected = (RadioButton)sender;

            foreach (GridViewRow dr in this.gvTabModules.Rows)
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

        /// <summary>
        /// Handles the CheckChanged event of the rbLoctionDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The. <see cref="System.EventArgs" /> Instance containing the event data.</param>
        protected void rbLoctionDetails_CheckChanged(object sender, EventArgs e)
        {
            this.chkAllowComments.Enabled = this.chkModerateComments.Enabled = this.cbLocationRating.Enabled = this.rbDetailsPage.Checked;
            this.chkModerateComments.Enabled = this.chkAllowComments.Checked = this.chkModerateComments.Checked = this.rbDetailsPage.Checked;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the rblProviderType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The. <see cref="System.EventArgs" /> Instance containing the event data.</param>
        protected void rblProviderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DisplayAPI();
        }

        private void DisplayAPI()
        {
            this.txtApiKey.Text = Dnn.Utility.GetStringSetting(this.Settings, this.rblProviderType.SelectedValue + ".ApiKey");
            this.txtApiKey.Focus();
        }

        private void DisplayLocationTypes()
        {
            DataTable dt = LocationType.GetLocationTypes();
            this.lbLocationType.DataSource = dt;
            this.lbLocationType.DataTextField = "LocationTypeName";
            this.lbLocationType.DataValueField = "LocationTypeId";
            this.lbLocationType.DataBind();

            string displayTypes = Dnn.Utility.GetStringSetting(this.Settings, "DisplayTypes");
            string[] displayTypesArray = displayTypes.Split(',');

            ////lbLocationType.SelectedIndex = 0;

            foreach (string s in displayTypesArray)
            {
                foreach (ListItem locationItems in this.lbLocationType.Items)
                {
                    if (locationItems.Value == s)
                    {
                        locationItems.Selected = true;
                    }
                }
            }

            if (string.IsNullOrEmpty(this.lbLocationType.SelectedValue))
            {
                if (this.lbLocationType.Items[0].Text == "Default")
                {
                    this.lbLocationType.SelectedIndex = 0;
                }
            }
        }

        private void DisplayProviderType()
        {
            this.rblProviderType.DataSource = MapProviderType.GetMapProviderTypes();
            this.rblProviderType.DataTextField = "Name";
            this.rblProviderType.DataValueField = "ClassName";
            this.rblProviderType.DataBind();
            string displayProvider = Dnn.Utility.GetStringSetting(this.Settings, "DisplayProvider");
            ListItem li = this.rblProviderType.Items.FindByText(displayProvider);
            if (li != null)
            {
                this.rblProviderType.SelectedValue = li.Value;
            }
        }

        /// <summary>
        /// Gets a comma-delimited list of the IDs of the selected location types.
        /// </summary>
        /// <returns>A comma-delimited list of the IDs of the selected location types.</returns>
        private string GetLocationTypeList()
        {
            List<string> locationTypeIds = new List<string>();
            foreach (ListItem li in this.lbLocationType.Items)
            {
                if (li.Selected)
                {
                    locationTypeIds.Add(li.Value);
                }
            }

            if (locationTypeIds.Count == 0)
            {
                locationTypeIds.Add(this.lbLocationType.Items[0].Value);
            }

            return string.Join(",", locationTypeIds.ToArray());
        }
    }
}