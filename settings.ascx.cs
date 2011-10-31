// <copyright file="Settings.ascx.cs" company="Engage Software">
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
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;
    using Data;
    using DotNetNuke.Common.Lists;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Maps;

    /// <summary>
    /// A control for setting the options for this module
    /// </summary>
    public partial class Settings : ModuleSettingsBase
    {
        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    this.ddlLocatorCountry.DataSource = new ListController().GetListEntryInfoCollection("Country");
                    this.ddlLocatorCountry.DataTextField = "Text";
                    this.ddlLocatorCountry.DataValueField = "EntryId";
                    this.ddlLocatorCountry.DataBind();
                    this.ddlLocatorCountry.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), string.Empty));
                    this.ddlLocatorCountry.SelectedValue = Dnn.Utility.GetStringSetting(this.Settings, "DefaultCountry", string.Empty);

                    this.txtSearchTitle.Text = Dnn.Utility.GetStringSetting(this.Settings, "SearchTitle", string.Empty);

                    string selectedRestrictionOption = "None";
                    if (Dnn.Utility.GetBoolSetting(this.Settings, "Country", false))
                    {
                        selectedRestrictionOption = "Country";
                    }
                    else if (Dnn.Utility.GetBoolSetting(this.Settings, "Radius", false))
                    {
                        selectedRestrictionOption = "Radius";
                    }

                    this.SearchRestrictionsRadioButtonList.Items.FindByValue(selectedRestrictionOption).Selected = true;
                    this.SetRadiusOptionsRowVisibility();

                    // set Show Location Details
                    string locationDetailsSetting = Dnn.Utility.GetStringSetting(this.Settings, "ShowLocationDetails");
                    if (locationDetailsSetting != null)
                    {
                        switch (locationDetailsSetting.ToUpperInvariant())
                        {
                            case "FALSE":
                            case "NODETAILS":
                                this.NoLocationDetailsRadioButton.Checked = true;
                                break;
                            case "DETAILSPAGE":
                                this.LocationDetailSeparatePageRadioButton.Checked =
                                        this.ModerateCommentsCheckBox.Enabled =
                                        this.AllowCommentsCheckBox.Checked = 
                                        this.AllowRatingsCheckBox.Enabled = true;
                                break;
                            case "TRUE":
                            case "SAMEPAGE":
                                this.LocationDetailsSamePageRadioButton.Checked = true;
                                break;
                        }
                    }
                    else
                    {
                        this.NoLocationDetailsRadioButton.Checked = true;
                    }

                    this.LocationsPerPageTextBox.Text = Dnn.Utility.GetIntSetting(this.Settings, "LocationsPerPage", 10).ToString(CultureInfo.InvariantCulture);
                    this.AllowRatingsCheckBox.Checked = Utility.GetBooleanPortalSetting("LocatorAllowRatings", this.PortalId, false);
                    this.AllowCommentsCheckBox.Checked = Utility.GetBooleanPortalSetting("LocatorAllowComments", this.PortalId, false);
                    this.ModerateCommentsCheckBox.Checked = Utility.GetBooleanPortalSetting("LocatorCommentModeration", this.PortalId, false);

                    if (Dnn.Utility.GetBoolSetting(this.Settings, "ShowDefaultDisplay", false))
                    {
                        this.rbDisplayAll.Checked = true;
                        this.AlwaysShowSearchCheckBox.Enabled = true;
                    }
                    else if (Dnn.Utility.GetBoolSetting(this.Settings, "ShowMapDefaultDisplay", false))
                    {
                        this.rbShowMap.Checked = true;
                        this.AlwaysShowSearchCheckBox.Enabled = true;
                    }
                    else
                    {
                        this.rbSearch.Checked = true;
                    }

                    if (Dnn.Utility.GetBoolSetting(this.Settings, "AlwaysShowSearch", false) && this.AlwaysShowSearchCheckBox.Enabled)
                    {
                        this.AlwaysShowSearchCheckBox.Checked = true;
                    }


                    // set search parameters
                    this.chkAddress.Checked = Dnn.Utility.GetBoolSetting(this.Settings, "SearchAddress", false);
                    this.chkCityRegion.Checked = Dnn.Utility.GetBoolSetting(this.Settings, "SearchCityRegion", false);
                    this.chkPostalCode.Checked = Dnn.Utility.GetBoolSetting(this.Settings, "SearchPostalCode", true);
                    this.chkCountry.Checked = Dnn.Utility.GetBoolSetting(this.Settings, "SearchCountry", false);

                    this.IncludeUnlimitedMilesRadiusCheckBox.Checked = Dnn.Utility.GetBoolSetting(this.Settings, "IncludeUnlimitedMilesRadius", true);
                    foreach (ListItem li in this.DefaultRadiusDropDownList.Items)
                    {
                        // li.Value becomes li.Text if you don't explicitly reset it, as below.  BD
                        li.Value = li.Value;
                        li.Text = String.Format(CultureInfo.CurrentCulture, "{0} {1}", li.Value, Localization.GetString("Miles", this.LocalResourceFile));
                    }

                    if (this.IncludeUnlimitedMilesRadiusCheckBox.Checked)
                    {
                        this.DefaultRadiusDropDownList.Items.Add(new ListItem(Localization.GetString("UnlimitedMiles", Utility.LocalSharedResourceFile), string.Empty));
                    }

                    ListItem defaultItem = this.DefaultRadiusDropDownList.Items.FindByValue(Dnn.Utility.GetStringSetting(this.Settings, "DefaultRadius", string.Empty));
                    if (defaultItem != null)
                    {
                        this.DefaultRadiusDropDownList.ClearSelection();
                        defaultItem.Selected = true;
                    }

                    // set MapType
                    this.rblMapDisplayType.SelectedValue = Dnn.Utility.GetEnumSetting(this.Settings, "MapType", MapType.Normal).ToString();

                    // set Submission Settings
                    this.AllowLocationSubmissionsCheckBox.Checked = Utility.GetBooleanPortalSetting("LocatorAllowSubmissions", this.PortalId, false);
                    this.chkModerateLocations.Checked = Utility.GetBooleanPortalSetting("LocatorSubmissionModeration", this.PortalId, false);

                    // fill gridview with existing locator modules
                    DataTable modules = DataProvider.Instance().GetEngageLocatorTabModules(this.PortalId);
                    this.gvTabModules.DataSource = modules;
                    this.gvTabModules.DataBind();

                    // set existing module
                    string displayResultsTabIdValue = Dnn.Utility.GetIntSetting(this.Settings, "DisplayResultsTabId", this.TabId).ToString(CultureInfo.InvariantCulture);
                    foreach (GridViewRow tabModuleRow in this.gvTabModules.Rows)
                    {
                        RadioButton locatorModuleRadioButton = (RadioButton)tabModuleRow.FindControl("LocatorModuleRadioButton");
                        Label lblTabId = (Label)tabModuleRow.FindControl("lblTabId");
                        locatorModuleRadioButton.Checked = displayResultsTabIdValue.Equals(lblTabId.Text, StringComparison.Ordinal);
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

        /// <summary>
        /// Updates the settings.
        /// </summary>
        public override void UpdateSettings()
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            try
            {
                ModuleController objModules = new ModuleController();
                objModules.UpdateTabModuleSetting(this.TabModuleId, "SearchTitle", this.txtSearchTitle.Text);
                objModules.UpdateTabModuleSetting(this.TabModuleId, "Country", this.SearchRestrictionsRadioButtonList.Items.FindByValue("Country").Selected.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "Radius", this.SearchRestrictionsRadioButtonList.Items.FindByValue("Radius").Selected.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "DisplayProvider", this.ProviderTypeRadioButtonList.SelectedItem.Text);
                objModules.UpdateTabModuleSetting(this.TabModuleId, this.ProviderTypeRadioButtonList.SelectedValue + ".ApiKey", this.txtApiKey.Text);
                objModules.UpdateTabModuleSetting(this.TabModuleId, "DefaultCountry", this.ddlLocatorCountry.SelectedValue);
                objModules.UpdateTabModuleSetting(this.TabModuleId, "MapType", this.rblMapDisplayType.SelectedValue);
                objModules.UpdateTabModuleSetting(this.TabModuleId, "LocationsPerPage", int.Parse(this.LocationsPerPageTextBox.Text, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "DisplayTypes", this.GetLocationTypeList());
                objModules.UpdateTabModuleSetting(this.TabModuleId, "ShowDefaultDisplay", this.rbDisplayAll.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "ShowMapDefaultDisplay", this.rbShowMap.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "AlwaysShowSearch", this.AlwaysShowSearchCheckBox.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "SearchAddress", this.chkAddress.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "SearchCityRegion", this.chkCityRegion.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "SearchPostalCode", this.chkPostalCode.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "SearchCountry", this.chkCountry.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "IncludeUnlimitedMilesRadius", this.IncludeUnlimitedMilesRadiusCheckBox.Checked.ToString(CultureInfo.InvariantCulture));
                objModules.UpdateTabModuleSetting(this.TabModuleId, "DefaultRadius", this.DefaultRadiusDropDownList.SelectedValue.ToString(CultureInfo.InvariantCulture));

                if (this.NoLocationDetailsRadioButton.Checked)
                {
                    objModules.UpdateTabModuleSetting(this.TabModuleId, "ShowLocationDetails", "NoDetails");
                }
                else if (this.LocationDetailsSamePageRadioButton.Checked)
                {
                    objModules.UpdateTabModuleSetting(this.TabModuleId, "ShowLocationDetails", "SamePage");
                }
                else if (this.LocationDetailSeparatePageRadioButton.Checked)
                {
                    objModules.UpdateTabModuleSetting(this.TabModuleId, "ShowLocationDetails", "DetailsPage");
                }

                HostSettingsController hsc = new HostSettingsController();
                hsc.UpdateHostSetting("LocatorAllowSubmissions" + this.PortalId.ToString(CultureInfo.InvariantCulture), this.AllowLocationSubmissionsCheckBox.Checked.ToString(CultureInfo.InvariantCulture));
                hsc.UpdateHostSetting("LocatorSubmissionModeration" + this.PortalId.ToString(CultureInfo.InvariantCulture), this.chkModerateLocations.Checked.ToString(CultureInfo.InvariantCulture));
                hsc.UpdateHostSetting("LocatorAllowRatings" + this.PortalId.ToString(CultureInfo.InvariantCulture), this.AllowRatingsCheckBox.Checked.ToString(CultureInfo.InvariantCulture));
                hsc.UpdateHostSetting("LocatorAllowComments" + this.PortalId.ToString(CultureInfo.InvariantCulture), this.AllowCommentsCheckBox.Checked.ToString(CultureInfo.InvariantCulture));
                hsc.UpdateHostSetting("LocatorCommentModeration" + this.PortalId.ToString(CultureInfo.InvariantCulture), this.ModerateCommentsCheckBox.Checked.ToString(CultureInfo.InvariantCulture));

                foreach (GridViewRow dr in this.gvTabModules.Rows)
                {
                    RadioButton locatorModuleRadioButton = (RadioButton)dr.FindControl("LocatorModuleRadioButton");
                    Label lblTabId = (Label)dr.FindControl("lblTabId");
                    if (locatorModuleRadioButton.Checked)
                    {
                        objModules.UpdateTabModuleSetting(this.TabModuleId, "DisplayResultsTabId", lblTabId.Text);
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                // Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Raises the <see cref="System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (AJAX.IsInstalled())
            {
                AJAX.RegisterScriptManager();
            }

            this.ApiKeyValidator.ServerValidate += this.ApiKeyValidator_ServerValidate;
            this.ProviderTypeValidator.ServerValidate += this.ProviderTypeValidator_ServerValidate;
            this.SearchOptionsValidator.ServerValidate += this.SearchOptionsValidator_ServerValidate;
            this.LocatorCountryValidator.ServerValidate += this.LocatorCountryValidator_ServerValidate;
            this.SearchResultsModuleValidator.ServerValidate += this.SearchResultsModuleValidator_ServerValidate;
            this.AllowCommentsCheckBox.CheckedChanged += this.AllowCommentsCheckBox_CheckedChanged;
            this.AllowLocationSubmissionsCheckBox.CheckedChanged += this.AllowLocationSubmissionsCheckBox_CheckedChanged;
            this.IncludeUnlimitedMilesRadiusCheckBox.CheckedChanged += this.IncludeUnlimitedMilesRadiusCheckBox_CheckedChanged;
            this.LocationDetailsSamePageRadioButton.CheckedChanged += this.LocationDetailsRadioButtons_CheckChanged;
            this.rbSearch.CheckedChanged += this.DefaultDisplayRadioGroup_CheckedChanged;
            this.rbShowMap.CheckedChanged += this.DefaultDisplayRadioGroup_CheckedChanged;
            this.rbDisplayAll.CheckedChanged += this.DefaultDisplayRadioGroup_CheckedChanged;
            this.NoLocationDetailsRadioButton.CheckedChanged += this.LocationDetailsRadioButtons_CheckChanged;
            this.LocationDetailSeparatePageRadioButton.CheckedChanged += this.LocationDetailsRadioButtons_CheckChanged;
            this.SearchRestrictionsRadioButtonList.SelectedIndexChanged += this.SearchRestrictionsRadioButtonList_SelectedIndexChanged;
            this.ProviderTypeRadioButtonList.SelectedIndexChanged += this.ProviderTypeRadioButtonList_SelectedIndexChanged;
        }

        /// <summary>
        /// Handles the CheckChanged event of the LocatorModuleRadioButtons control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected void LocatorModuleRadioButtons_CheckChanged(object sender, EventArgs e)
        {
            RadioButton selectedRadioButton = (RadioButton)sender;

            foreach (GridViewRow dr in this.gvTabModules.Rows)
            {
                RadioButton currentModuleRadioButton = (RadioButton)dr.FindControl("LocatorModuleRadioButton");
                currentModuleRadioButton.Checked = currentModuleRadioButton == selectedRadioButton;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the SearchRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DefaultDisplayRadioGroup_CheckedChanged(object sender, EventArgs e)
        {
            this.AlwaysShowSearchCheckBox.Enabled = !this.rbSearch.Checked;
            if (!this.AlwaysShowSearchCheckBox.Enabled)
            {
                this.AlwaysShowSearchCheckBox.Checked = false;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the AllowCommentsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void AllowCommentsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.AllowCommentsCheckBox.Checked)
            {
                this.ModerateCommentsCheckBox.Enabled = this.AllowCommentsCheckBox.Checked;
            }
            else
            {
                this.ModerateCommentsCheckBox.Enabled = false;
                this.ModerateCommentsCheckBox.Checked = false;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the AllowLocationSubmissionsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void AllowLocationSubmissionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.AllowLocationSubmissionsCheckBox.Checked)
            {
                this.chkModerateLocations.Enabled = this.AllowLocationSubmissionsCheckBox.Checked;
            }
            else
            {
                this.chkModerateLocations.Checked = false;
                this.chkModerateLocations.Enabled = false;
            }
        }

        /// <summary>
        /// Handles the ServerValidate event of the apiKey control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs" /> instance containing the event data.</param>
        private void ApiKeyValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.ProviderTypeRadioButtonList.SelectedItem != null)
            {
                MapProvider mp = MapProvider.CreateInstance(this.ProviderTypeRadioButtonList.SelectedValue, this.txtApiKey.Text);
                args.IsValid = mp.IsKeyValid();
            }
            else
            {
                args.IsValid = false;
                this.dshMapProvider.IsExpanded = true;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the IncludeUnlimitedMilesRadiusCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void IncludeUnlimitedMilesRadiusCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IncludeUnlimitedMilesRadiusCheckBox.Checked)
            {
                this.DefaultRadiusDropDownList.Items.Add(new ListItem(Localization.GetString("UnlimitedMiles", Utility.LocalSharedResourceFile), string.Empty));
            }
            else
            {
                this.DefaultRadiusDropDownList.Items.Remove(this.DefaultRadiusDropDownList.Items.FindByValue(string.Empty));
            }
        }

        /// <summary>
        /// Handles the CheckChanged event of the radio button controls used to select where the location details are displayed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void LocationDetailsRadioButtons_CheckChanged(object sender, EventArgs e)
        {
            this.AllowCommentsCheckBox.Enabled =
                    this.ModerateCommentsCheckBox.Enabled =
                    this.AllowRatingsCheckBox.Enabled =
                    this.AllowCommentsCheckBox.Checked = this.ModerateCommentsCheckBox.Checked = this.LocationDetailSeparatePageRadioButton.Checked;
        }

        /// <summary>
        /// Handles the OnServerValidate event of the LocatorCountryValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs" /> instance containing the event data.</param>
        private void LocatorCountryValidator_ServerValidate(object source, ServerValidateEventArgs args)
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
        /// Handles the SelectedIndexChanged event of the ProviderTypeRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void ProviderTypeRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DisplayAPI();
        }

        /// <summary>
        /// Handles the ServerValidate event of the ProviderTypeValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs" /> instance containing the event data.</param>
        private void ProviderTypeValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.ProviderTypeRadioButtonList.SelectedItem == null)
            {
                args.IsValid = false;
                this.dshMapProvider.IsExpanded = true;
            }
        }

        /// <summary>
        /// Handles the ServerValidate event of the SearchOptionsValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs" /> instance containing the event data.</param>
        private void SearchOptionsValidator_ServerValidate(object source, ServerValidateEventArgs args)
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
        /// Handles the ServerValidate event of the SearchResultsModuleValidator control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs" /> instance containing the event data.</param>
        private void SearchResultsModuleValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string tabId = string.Empty;
            foreach (GridViewRow row in this.gvTabModules.Rows)
            {
                RadioButton rb = (RadioButton)row.FindControl("LocatorModuleRadioButton");
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
        /// Handles the SelectedIndexChanged event of the SearchRestrictionsRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SearchRestrictionsRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetRadiusOptionsRowVisibility();
        }

        /// <summary>
        /// Sets the visibility of <see cref="RadiusOptionsRow"/>.
        /// </summary>
        private void SetRadiusOptionsRowVisibility()
        {
            this.RadiusOptionsRow.Visible = this.SearchRestrictionsRadioButtonList.SelectedValue == "Radius";
        }

        private void DisplayAPI()
        {
            this.txtApiKey.Text = Dnn.Utility.GetStringSetting(this.Settings, this.ProviderTypeRadioButtonList.SelectedValue + ".ApiKey");
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
            if (displayTypes != null)
            {
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
            this.ProviderTypeRadioButtonList.DataSource = MapProviderType.GetMapProviderTypes();
            this.ProviderTypeRadioButtonList.DataTextField = "Name";
            this.ProviderTypeRadioButtonList.DataValueField = "ClassName";
            this.ProviderTypeRadioButtonList.DataBind();
            string displayProvider = Dnn.Utility.GetStringSetting(this.Settings, "DisplayProvider");
            ListItem li = this.ProviderTypeRadioButtonList.Items.FindByText(displayProvider);
            if (li != null)
            {
                this.ProviderTypeRadioButtonList.SelectedValue = li.Value;
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