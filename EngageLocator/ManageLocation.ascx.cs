using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Lists;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.Utilities;
using Globals=DotNetNuke.Common.Globals;

namespace Engage.Dnn.Locator
{
    public partial class ManageLocation : ModuleBase
    {
        private static int tabModuleId;
        private static int locationId;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            tabModuleId = Convert.ToInt32(Request.QueryString["tmid"]);
            locationId = Convert.ToInt32(Request.QueryString["lid"]);

            if (!Page.IsPostBack)
            {
                FillCountry();
                FillState();
                DisplayLocationTypes(ddlType);
                if (locationId > 0)
                {
                    LoadLocation();
                    btnDelete.Visible = true;
                }
                else if (UserInfo.IsInRole(PortalSettings.ActiveTab.AdministratorRoles))
                {
                    ClientAPI.AddButtonConfirm(btnDelete, Localization.GetString("confirmDelete", LocalResourceFile));
                    rbApprove.Visible = false;
                    rbWaitingForApproval.Visible = false;
                    this.lblStatus.Visible = false;
                    btnDelete.Visible = false;
                    if (Settings["ModerateSubmissions"] != null && Settings["ModerateSubmissions"].ToString() == "True")
                    {
                        this.lblStatus.Visible = true;
                        rbApprove.Visible = true;
                        rbWaitingForApproval.Visible = true;
                        rbWaitingForApproval.Checked = true;
                    }
                    else
                    {
                        rbApprove.Visible = false;
                        rbWaitingForApproval.Visible = false;
                        this.lblStatus.Visible = false;
                    }
                }
                else
                {
                    btnDelete.Visible = false;
                    rbApprove.Visible = false;
                    rbWaitingForApproval.Visible = false;
                    this.lblStatus.Visible = false;
                }
                txtLocationId.Focus();
            }
        }

        private void LoadLocation()
        {
            Location location = Location.GetLocation(locationId);
            string address2 = string.Empty;

            txtLocationId.Text = location.ExternalIdentifier;
            txtName.Text = location.Name;
            txtWebsite.Text = location.Website;
            if (location.Address != String.Empty && location.Address.Contains(","))
            {
                int length = location.Address.IndexOf(',');
                address2 = location.Address.Remove(0, length);
            }
            txtAddress1.Text = location.Address;
            txtAddress2.Text = address2;
            txtCity.Text = location.City;
            txtZip.Text = location.PostalCode;
            txtPhone.Text = location.Phone;
            teLocationDetails.Text = location.LocationDetails;
            ddlType.SelectedValue = location.LocationTypeId.ToString();
            txtWebsite.Text = location.Website;
            txtLatitude.Text = location.Latitude.ToString();
            txtLongitude.Text = location.Longitude.ToString();

            if (UserInfo.IsInRole(PortalSettings.ActiveTab.AdministratorRoles))
            {
                ClientAPI.AddButtonConfirm(btnDelete, Localization.GetString("confirmDelete", LocalResourceFile));
                btnDelete.Visible = true;
                if (Settings["ModerateSubmissions"].ToString() == "True")
                {
                    this.lblStatus.Visible = true;
                    rbApprove.Visible = true;
                    rbWaitingForApproval.Visible = true;
                    if (location.Approved)
                        rbApprove.Checked = true;
                    else
                        rbWaitingForApproval.Checked = true;
                }
                else
                {
                    rbApprove.Visible = false;
                    rbWaitingForApproval.Visible = false;
                    this.lblStatus.Visible = false;
                }
            }
            else
            {
                btnDelete.Visible = false;
                rbApprove.Visible = false;
                rbWaitingForApproval.Visible = false;
                this.lblStatus.Visible = false;
            }

            ddlState.SelectedValue = location.RegionId.ToString();

            ListController controller = new ListController();
            ListEntryInfoCollection regions = controller.GetListEntryInfoCollection("Region");
            int parentId = 0;
            foreach (ListEntryInfo region in regions)
            {
                if (location.RegionId == region.EntryID)
                    parentId = region.ParentID;
            }
            ddlCountry.SelectedValue = parentId.ToString();

            rptCustomAttributes.DataSource = location.GetLocationAttributes();
            rptCustomAttributes.DataBind();

            if (rptCustomAttributes.Items.Count > 0)
            {
                rptCustomAttributes.Visible = true;
                lblAttributes.Visible = true;
            }
        }

        private void FillState()
        {
            //Load the state list based on country
            ListController controller = new ListController();
            ListEntryInfoCollection states = controller.GetListEntryInfoCollection("Region");

            ddlState.DataSource = states;
            ddlState.DataTextField = "Text";
            ddlState.DataValueField = "EntryID";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));
        }

        private void FillCountry()
        {
            ListController controller = new ListController();
            ListEntryInfoCollection countries = controller.GetListEntryInfoCollection("Country");

            ddlCountry.DataSource = countries;
            ddlCountry.DataTextField = "Text";
            ddlCountry.DataValueField = "EntryId";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));
            ListItem liUs = ddlCountry.Items.FindByText("United States");
            ddlCountry.Items.Remove(liUs);//remove US and add it back at the top of the list
            ddlCountry.Items.Insert(1, liUs);
            ddlCountry.SelectedIndex = 1;
        }

        private void DisplayLocationTypes(ListControl ddl)
        {
            ddl.DataSource = LocationType.GetLocationTypes();
            ddl.DataTextField = "LocationTypeName";
            ddl.DataValueField = "LocationTypeId";
            ddl.DataBind();

            string displayTypes = Convert.ToString((new ModuleController()).GetModuleSettings(ModuleId)["DisplayTypes"]);
            string[] displayTypesArray = displayTypes.Split(',');

            foreach (string s in displayTypesArray)
            {
                foreach (ListItem locationItems in ddl.Items)
                {
                    if (locationItems.Value == s)
                    {
                        locationItems.Selected = true;
                    }
                }
            }
            ddl.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));

            if (ddl.Items.Count == 0)
            {
                singleError.Visible = true;
            }
            else
            {
                singleError.Visible = false;
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListController controller = new ListController();
            int parentid = controller.GetListEntryInfo(Convert.ToInt32(ddlState.SelectedValue)).ParentID;

            ddlCountry.SelectedValue = parentid.ToString();
            //ScriptManager.GetCurrent(Page).SetFocus(ddlState);
            Page.SetFocus(ddlState);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == false) return;

            try
            {
                if (txtLocationId.Text != String.Empty && locationId > 0)
                {
                    Location currentLocation = Location.GetLocation(locationId);
                    string error;
                    string address;
                    string city = txtCity.Text;

                    currentLocation.ExternalIdentifier = txtLocationId.Text;
                    currentLocation.Name = txtName.Text;
                    currentLocation.Website = txtWebsite.Text;

                    address = txtAddress1.Text;

                    if (txtAddress2.Text != String.Empty)
                    {
                        address = address + ", " + txtAddress2.Text;
                    }
                    currentLocation.Address = address;
                    currentLocation.City = city;
                    currentLocation.RegionId = Convert.ToInt32(ddlState.SelectedValue);
                    currentLocation.StateName = ddlState.SelectedItem.ToString();
                    currentLocation.CountryId = Convert.ToInt32(ddlCountry.SelectedValue);
                    currentLocation.PostalCode = txtZip.Text;
                    currentLocation.Phone = txtPhone.Text;
                    currentLocation.LocationDetails = teLocationDetails.Text;
                    currentLocation.LocationTypeId = Convert.ToInt32(ddlType.SelectedValue);
                    currentLocation.PortalId = PortalId;
                    currentLocation.Website = txtWebsite.Text;
                    currentLocation.LastUpdateDate = DateTime.Now;
                    string location = txtAddress1.Text + ", " + currentLocation.City + ", " + currentLocation.StateName + ", " + currentLocation.PostalCode;
                    if (currentLocation.Latitude.ToString() != txtLatitude.Text || currentLocation.Longitude.ToString() != txtLongitude.Text)
                    {
                        currentLocation.Latitude = Convert.ToDouble(txtLatitude.Text);
                        currentLocation.Longitude = Convert.ToDouble(txtLongitude.Text);
                        error = "Success";
                    }
                    else
                    {
                        Nullable<double> latitude;
                        Nullable<double> longitude;
                        GetGeoCodeResults(city, ddlState.SelectedItem.ToString(), txtZip.Text, txtAddress1.Text, out latitude, out longitude, out error, location);
                        currentLocation.Latitude = Convert.ToDouble(latitude);
                        currentLocation.Longitude = Convert.ToDouble(longitude);
                    }
                    if (Settings["ModerateSubmissions"].ToString() == "True")
                        currentLocation.Approved = rbApprove.Checked;
                    else
                        currentLocation.Approved = true;
                    if (error == "Success")
                    {
                        currentLocation.Update();
                        Response.Redirect(EditUrl("", "", "Import", "tmid=" + TabModuleId));
                    }
                    else
                    {
                        lblError.Text = error + " - Please try submitting your location again.";
                    }
                }
                else
                {
                    Location newLocation = new Location();
                    string error;
                    string address;
                    string city = txtCity.Text;

                    newLocation.ExternalIdentifier = txtLocationId.Text;
                    newLocation.Name = txtName.Text;
                    newLocation.Website = txtWebsite.Text;

                    address = txtAddress1.Text;

                    if (txtAddress2.Text != String.Empty)
                    {
                        address = address + ", " + txtAddress2.Text;
                    }

                    if (Settings["ModerateSubmissions"].ToString() == "True")
                    {
                        if (UserInfo.IsInRole(PortalSettings.ActiveTab.AdministratorRoles))
                            newLocation.Approved = rbApprove.Checked;
                        else
                            newLocation.Approved = false;
                    }

                    newLocation.Address = address;
                    newLocation.City = city;
                    newLocation.RegionId = Convert.ToInt32(ddlState.SelectedValue);
                    newLocation.StateName = ddlState.SelectedItem.ToString();
                    newLocation.CountryId = Convert.ToInt32(ddlCountry.SelectedValue);
                    newLocation.PostalCode = txtZip.Text;
                    newLocation.Phone = txtPhone.Text;
                    newLocation.LocationDetails = teLocationDetails.Text;
                    newLocation.LocationTypeId = Convert.ToInt32(ddlType.SelectedValue);
                    newLocation.PortalId = PortalId;
                    newLocation.Website = txtWebsite.Text;
                    newLocation.LastUpdateDate = DateTime.Now;
                    string location = txtAddress1.Text + ", " + newLocation.City + ", " + newLocation.StateName + ", " + newLocation.PostalCode;
                    if (txtLatitude.Text != "" || txtLongitude.Text != "")
                    {
                        newLocation.Latitude = Convert.ToDouble(txtLatitude.Text);
                        newLocation.Longitude = Convert.ToDouble(txtLongitude.Text);
                        error = "Success";
                    }
                    else
                    {
                        Nullable<double> latitude;
                        Nullable<double> longitude;
                        GetGeoCodeResults(city, ddlState.SelectedItem.ToString(), txtZip.Text, txtAddress1.Text, out latitude, out longitude, out error, location);
                        newLocation.Latitude = Convert.ToDouble(latitude);
                        newLocation.Longitude = Convert.ToDouble(longitude);
                    }
                    lblError.Visible = true;
                    if (error == "Success")
                    {
                        newLocation.Save();
                        if (UserInfo.IsInRole(PortalSettings.ActiveTab.AdministratorRoles))
                            Response.Redirect(EditUrl("", "", "Import", "tmid=" + TabModuleId));
                        else
                            Response.Redirect(Globals.NavigateURL());
                    }
                    else
                    {
                        lblError.Text = error + " - Please try submitting your location again.";
                        singleDivError.Visible = true;
                        lblError.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        private static void GetGeoCodeResults(string city, string state, string zip, string address, out double? latitude, out double? longitude, out string error, string location)
        {
            string apiKey = String.Empty;
            Hashtable ht = DotNetNuke.Entities.Portals.PortalSettings.GetTabModuleSettings(tabModuleId);
            string displayProvider = Convert.ToString(ht["DisplayProvider"]);
            ReadOnlyCollection<MapProviderType> mpType = MapProviderType.GetMapProviderTypes();
            foreach (MapProviderType type in mpType)
            {
                if (type.ClassName.Contains(displayProvider))
                {
                    apiKey = type.GetApiKey(ht);
                    break;
                }
            }

            if (displayProvider.Contains("Google"))
            {
                GoogleGeocodeResult google = SearchUtility.SearchGoogle(location, apiKey);
                latitude = google.latitude;
                longitude = google.longitude;
                error = google.statusCode.ToString();
            }
            else if (displayProvider.Contains("Yahoo"))
            {
                YahooGeocodeResult yahoo;
                yahoo.accuracyCode = YahooAccuracyCode.Unknown;
                yahoo = SearchUtility.SearchYahoo(location, address, city, state, zip, apiKey);
                latitude = yahoo.latitude;
                longitude = yahoo.longitude;
                error = yahoo.statusCode.ToString();
            }
            else
            {
                latitude = null;
                longitude = null;
                error = "No Geo Code Information Available";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId, "Import", "mid=" + ModuleId + "&tmid=" + tabModuleId));
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Location.DeleteLocation(locationId);
            Response.Redirect(EditUrl("", "", "Import", "tmid=" + TabModuleId));
        }

        protected void rptCustomAttributes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                Label label = (Label)e.Item.FindControl("lblCustomAttribute");
                TextBox textBox = (TextBox)e.Item.FindControl("txtCustomAttribute");
                label.Text = row["AttributeName"].ToString();
                textBox.Text = row["AttributeValue"].ToString();
            }
        }
    }
}