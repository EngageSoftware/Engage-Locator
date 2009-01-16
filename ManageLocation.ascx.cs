// <copyright file="ManageLocation.ascx.cs" company="Engage Software">
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
    using System.Globalization;
    using System.Web.UI.WebControls;
    using DotNetNuke.Common.Lists;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Utilities;
    using Globals = DotNetNuke.Common.Globals;

    public partial class ManageLocation : ModuleBase
    {
        private int LocationId
        {
            get
            {
                int locationId = -1;
                object o = this.Request.QueryString["lid"];
                if (o != null)
                {
                    locationId = Convert.ToInt32(this.Request.QueryString["lid"], CultureInfo.InvariantCulture);
                }

                return locationId;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (AJAX.IsInstalled())
            {
                AJAX.RegisterScriptManager();
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lbSettings.Visible = this.IsEditable;
            this.lbImportFile.Visible = this.IsEditable;
            this.lbManageLocations.Visible = this.IsEditable;
            this.lbLocationTypes.Visible = this.IsEditable;
            this.lbManageComments.Visible = this.IsEditable;

            if (!this.Page.IsPostBack)
            {
                this.FillCountry();
                this.FillState();
                this.DisplayLocationTypes(this.ddlType);
                if (this.LocationId > 0)
                {
                    this.LoadLocation();
                    this.btnDelete.Visible = true;
                }
                else if (this.UserInfo.IsInRole(this.PortalSettings.ActiveTab.AdministratorRoles))
                {
                    ClientAPI.AddButtonConfirm(this.btnDelete, Localization.GetString("confirmDelete", this.LocalResourceFile));
                    this.rbApprove.Visible = false;
                    this.rbWaitingForApproval.Visible = false;
                    this.lblStatus.Visible = false;
                    this.btnDelete.Visible = false;
                    if (this.SubmissionModerationEnabled)
                    {
                        this.lblStatus.Visible = true;
                        this.rbApprove.Visible = true;
                        this.rbWaitingForApproval.Visible = true;
                        this.rbWaitingForApproval.Checked = true;
                    }
                    else
                    {
                        this.rbApprove.Visible = false;
                        this.rbWaitingForApproval.Visible = false;
                        this.lblStatus.Visible = false;
                    }
                }
                else
                {
                    this.btnDelete.Visible = false;
                    this.rbApprove.Visible = false;
                    this.rbWaitingForApproval.Visible = false;
                    this.lblStatus.Visible = false;
                }

                // Only Host users can make changes to lists
                this.AddingRegionsLabel.Visible = this.UserInfo.IsSuperUser;

                this.LoadCustomAttributes();

                this.txtLocationId.Focus();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.IsEditable)
            {
                this.Response.Redirect(this.EditUrl("ManageLocations"));
            }
            else
            {
                this.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Location.DeleteLocation(this.LocationId);
            this.Response.Redirect(this.EditUrl("ManageLocations"));
        }

        /// <summary>
        /// Handles the Click event of the btnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            try
            {
                string errorMessage;
                Location location = this.LocationId > 0 ? Location.GetLocation(this.LocationId) : new Location();

                location.ExternalIdentifier = this.txtLocationId.Text;
                location.Name = this.txtName.Text;
                location.Website = this.txtWebsite.Text;
                location.Address = this.txtAddress1.Text;
                location.Address2 = this.txtAddress2.Text;
                location.City = this.txtCity.Text;
                location.PostalCode = this.txtZip.Text;
                location.Phone = this.txtPhone.Text;
                location.LocationDetails = this.teLocationDetails.Text;
                location.Website = this.txtWebsite.Text;
                location.PortalId = this.PortalId;

                location.Approved = !this.SubmissionModerationEnabled || (this.rbApprove.Visible && this.rbApprove.Checked);
                location.CountryId = Convert.ToInt32(this.CountryDropDownList.SelectedValue, CultureInfo.InvariantCulture);
                location.LocationTypeId = Convert.ToInt32(this.ddlType.SelectedValue, CultureInfo.InvariantCulture);

                int regionId;
                if (int.TryParse(this.RegionDropDownList.SelectedValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out regionId))
                {
                    location.RegionId = regionId;
                }
                else
                {
                    location.RegionId = null;
                }

                // if latitude and longitude are provided (their real numbers, and they've changed from when the page loaded), just use those values
                // otherwise (re)check the address
                double latitude, longitude;
                if (double.TryParse(txtLongitude.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out longitude)
                    && double.TryParse(txtLatitude.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out latitude)
                    && (latitude != location.Latitude || longitude != location.Longitude))
                {
                    location.Latitude = latitude;
                    location.Longitude = longitude;
                    errorMessage = "Success";
                }
                else
                {
                    GeocodeResult geocodeResult = this.GetMapProvider().GeocodeLocation(
                            location.Address, location.City, location.RegionId, location.PostalCode, location.CountryId);

                    if (geocodeResult.Successful)
                    {
                        location.Latitude = geocodeResult.Latitude;
                        location.Longitude = geocodeResult.Longitude;
                        errorMessage = "Success";
                    }
                    else
                    {
                        errorMessage = geocodeResult.ErrorMessage;
                    }
                }

                if (errorMessage == "Success")
                {
                    this.lblError.Visible = false;

                    location.Save();

                    foreach (RepeaterItem item in this.rptCustomAttributes.Items)
                    {
                        HiddenField hdnLocationAttributeID = (HiddenField)item.FindControl("hdnLocationAttributeID");
                        HiddenField hdnAttributeDefinitionId = (HiddenField)item.FindControl("hdnAttributeDefinitionId");
                        TextBox txtLocationAttributeValue = (TextBox)item.FindControl("txtCustomAttribute");
                        int locationAttributeId = Convert.ToInt32(hdnLocationAttributeID.Value, CultureInfo.InvariantCulture);
                        if (locationAttributeId > 0)
                        {
                            Attribute.UpdateAttribute(locationAttributeId, location.LocationId, txtLocationAttributeValue.Text);
                        }
                        else
                        {
                            int attributeDefinitionId = Convert.ToInt32(hdnAttributeDefinitionId.Value, CultureInfo.InvariantCulture);
                            Attribute.AddAttribute(attributeDefinitionId, location.LocationId, txtLocationAttributeValue.Text);
                        }
                    }
                    if (this.IsEditable)
                    {
                        this.Response.Redirect(this.EditUrl("ManageLocations"), true);
                    }
                    else
                    {
                        this.Response.Redirect(Globals.NavigateURL(), true);
                    }
                }
                else
                {
                    this.singleDivError.Visible = true;
                    this.lblError.Text = errorMessage + Localization.GetString("Please try submitting your location again.Text", this.LocalResourceFile);
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadCustomAttributes();
        }

        /// <summary>
        /// Handles the ItemDataBound event of the rptCustomAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void rptCustomAttributes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Attribute attribute = (Attribute)e.Item.DataItem;
                HiddenField locationAttributeId = (HiddenField)e.Item.FindControl("hdnLocationAttributeID");
                HiddenField attributeDefinitionId = (HiddenField)e.Item.FindControl("hdnAttributeDefinitionId");
                Label label = (Label)e.Item.FindControl("lblCustomAttribute");
                TextBox textBox = (TextBox)e.Item.FindControl("txtCustomAttribute");
                locationAttributeId.Value = attribute.AttributeId.ToString(CultureInfo.InvariantCulture);
                attributeDefinitionId.Value = attribute.AttributeDefinitionId.ToString(CultureInfo.InvariantCulture);
                label.Text = attribute.AttributeName;
                textBox.Text = attribute.AttributeValue;
            }
        }

        private void DisplayLocationTypes(ListControl ddl)
        {
            ddl.DataSource = LocationType.GetLocationTypes();
            ddl.DataTextField = "LocationTypeName";
            ddl.DataValueField = "LocationTypeId";
            ddl.DataBind();

            string displayTypes = this.Settings["DisplayTypes"].ToString();
            string[] displayTypesArray = displayTypes.Split(',');

            ddl.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), "-1"));

            if (ddl.Items.Count == 0)
            {
                this.singleError.Visible = true;
            }
            else
            {
                this.singleError.Visible = false;
            }

            // If this module has been configured to display only one location and a user is adding a new entry
            // we are defaulting the locationtype based on the setting. hk
            if (displayTypes.Length == 1)
            {
                string id = displayTypesArray[0];
                ListItem li = ddl.Items.FindByValue(id);
                if (li != null)
                {
                    li.Selected = true;
                }
            }
        }

        private void FillCountry()
        {
            ListController controller = new ListController();
            ListEntryInfoCollection countries = controller.GetListEntryInfoCollection("Country");

            this.CountryDropDownList.DataSource = countries;
            this.CountryDropDownList.DataTextField = "Text";
            this.CountryDropDownList.DataValueField = "EntryId";
            this.CountryDropDownList.DataBind();
            this.CountryDropDownList.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), string.Empty));
            ListItem defaultItem = this.CountryDropDownList.Items.FindByValue(Dnn.Utility.GetStringSetting(this.Settings, "DefaultCountry", string.Empty));
            this.CountryDropDownList.Items.Remove(defaultItem);
            this.CountryDropDownList.Items.Insert(1, defaultItem);
            this.CountryDropDownList.SelectedIndex = 1;
        }

        private void FillState()
        {
            // Load the state list based on country
            ListController controller = new ListController();
            ListEntryInfoCollection states = controller.GetListEntryInfoCollection("Region");

            this.RegionDropDownList.DataSource = states;
            this.RegionDropDownList.DataTextField = "Text";
            this.RegionDropDownList.DataValueField = "EntryID";
            this.RegionDropDownList.DataBind();
            this.RegionDropDownList.Items.Insert(0, new ListItem(Localization.GetString("None", this.LocalResourceFile), string.Empty));
        }

        private void LoadCustomAttributes()
        {
            this.rptCustomAttributes.DataSource = Attribute.GetAttributes(Convert.ToInt32(this.ddlType.SelectedValue, CultureInfo.InvariantCulture), this.LocationId);
            this.rptCustomAttributes.DataBind();

            if (this.rptCustomAttributes.Items.Count > 0)
            {
                this.rptCustomAttributes.Visible = true;
                this.lblAttributes.Visible = true;
            }
        }

        private void LoadLocation()
        {
            Location location = Location.GetLocation(this.LocationId);

            this.txtLocationId.Text = location.ExternalIdentifier;
            this.txtName.Text = location.Name;
            this.txtWebsite.Text = location.Website;
            this.txtAddress1.Text = location.Address;
            this.txtAddress2.Text = location.Address2;
            this.txtCity.Text = location.City;
            this.txtZip.Text = location.PostalCode;
            this.txtPhone.Text = location.Phone;
            this.teLocationDetails.Text = location.LocationDetails;
            this.ddlType.SelectedValue = location.LocationTypeId.ToString(CultureInfo.InvariantCulture);
            this.txtWebsite.Text = location.Website;
            this.txtLatitude.Text = location.Latitude.ToString(CultureInfo.CurrentCulture);
            this.txtLongitude.Text = location.Longitude.ToString(CultureInfo.CurrentCulture);

            if (this.UserInfo.IsInRole(this.PortalSettings.ActiveTab.AdministratorRoles))
            {
                ClientAPI.AddButtonConfirm(this.btnDelete, Localization.GetString("confirmDelete", this.LocalResourceFile));
                this.btnDelete.Visible = true;
                if (this.SubmissionModerationEnabled)
                {
                    this.lblStatus.Visible = true;
                    this.rbApprove.Visible = true;
                    this.rbWaitingForApproval.Visible = true;
                    if (location.Approved)
                    {
                        this.rbApprove.Checked = true;
                    }
                    else
                    {
                        this.rbWaitingForApproval.Checked = true;
                    }
                }
                else
                {
                    this.rbApprove.Visible = false;
                    this.rbWaitingForApproval.Visible = false;
                    this.lblStatus.Visible = false;
                }
            }
            else
            {
                this.btnDelete.Visible = false;
                this.rbApprove.Visible = false;
                this.rbWaitingForApproval.Visible = false;
                this.lblStatus.Visible = false;
            }

            this.RegionDropDownList.SelectedValue = location.RegionId.HasValue ? location.RegionId.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;

            ListEntryInfoCollection regions = new ListController().GetListEntryInfoCollection("Region");
            int parentId = 0;
            foreach (ListEntryInfo region in regions)
            {
                if (location.RegionId == region.EntryID)
                {
                    parentId = region.ParentID;
                }
            }

            this.CountryDropDownList.SelectedValue = parentId.ToString(CultureInfo.InvariantCulture);

            this.rptCustomAttributes.DataSource = Attribute.GetAttributes(Convert.ToInt32(this.ddlType.SelectedValue, CultureInfo.InvariantCulture), location.LocationId);
            this.rptCustomAttributes.DataBind();

            if (this.rptCustomAttributes.Items.Count > 0)
            {
                this.rptCustomAttributes.Visible = true;
                this.lblAttributes.Visible = true;
            }
        }
    }
}