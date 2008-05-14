//Copyright (c) 2004-2007
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Locator.Data;

namespace Engage.Dnn.Locator
{
    partial class ZipLocator : ModuleBase, IActionable
    {
        #region Properties

        protected string ShowLocationDetails
        {
            get
            {
                string showDetails = "False";
                ModuleController objModules = new ModuleController();

                if (objModules.GetTabModuleSettings(TabModuleId)["ShowLocationDetails"] != null)
                {
                    showDetails = objModules.GetTabModuleSettings(TabModuleId)["ShowLocationDetails"].ToString();
                }
                return showDetails;
            }
        }

        protected bool ShowCountry
        {
            get
            {
                ModuleController objModules = new ModuleController();
                return Convert.ToBoolean(objModules.GetTabModuleSettings(TabModuleId)["Country"].ToString());
            }
        }

        protected bool ShowRadius
        {
            get
            {
                ModuleController objModules = new ModuleController();
                return Convert.ToBoolean(objModules.GetTabModuleSettings(TabModuleId)["Radius"].ToString());
            }
        }

        private bool _loadDefault = false;
        protected bool ShowDefaultDisplay
        {
            get
            {
                bool showDefault = false;
                ModuleController objModules = new ModuleController();
                if (objModules.GetTabModuleSettings(TabModuleId)["ShowDefaultDisplay"] != null) 
                    showDefault = Convert.ToBoolean(objModules.GetTabModuleSettings(TabModuleId)["ShowDefaultDisplay"].ToString());
                return showDefault;
            }
        }

        protected bool ShowMapDefaultDisplay
        {
            get
            {
                bool showMap = false;
                ModuleController objModules = new ModuleController();

                if (objModules.GetTabModuleSettings(TabModuleId)["ShowMapDefaultDisplay"] != null)
                    showMap = Convert.ToBoolean(objModules.GetTabModuleSettings(TabModuleId)["ShowMapDefaultDisplay"].ToString());
                return showMap;
            }
        }

        protected int DisplayTabId
        {
            get
            {
                ModuleController objModules = new ModuleController();
                if (objModules.GetTabModuleSettings(TabModuleId)["DisplayResultsTabId"] != null)
                    return Convert.ToInt32(objModules.GetTabModuleSettings(TabModuleId)["DisplayResultsTabId"]);
                else
                    return TabId;
            }
        }

        #endregion

        protected void Page_Load(Object sender, EventArgs e)
        {
            try
            {
                string error = String.Empty;
                lnkViewMap.OnClientClick = "showAllLocations(); return false;";
                if (!IsConfigured(TabModuleId, ref error))
                {
                    mvwLocator.SetActiveView(vwSetup);
                    if (UserInfo.IsInRole(PortalSettings.ActiveTab.AdministratorRoles))
                    {
                        lblSetupText.Text = Localization.GetString("SetupText_Admin", LocalResourceFile) + error; // "Please setup module through the Module Settings page.<br>Required Items: <br/><br/>" + error;
                    }
                    else
                    {
                        lblSetupText.Text = Localization.GetString("SetupText_NonAdmin", LocalResourceFile); // "This page is not configured. Please contact an Administrator.";
                    }
                }
                else
                {
                    mvwLocator.SetActiveView(vwLocator);
                    if (!IsPostBack)
                    {
                        FillDropDowns();

                        if (Request.QueryString["Search"] != null)
                        {
                            DoWork(Request.QueryString["Search"]);
                        }
                        else
                        {
                            if (ShowDefaultDisplay || ShowMapDefaultDisplay)
                            {
                                DoWork(null);
                            }
                        }

                        SetSearchDisplay();

                        if (Settings["Country"] != null && Settings["Country"].ToString() == "False" && Settings["Radius"] != null &&
                            Settings["Radius"].ToString() == "False")
                            btn_ShowAll.Visible = false;

                        if(Settings["AllowSubmissions"] != null)
                        {
                            if (Settings["AllowSubmissions"].ToString() == "True")
                            {                                
                                lnkSubmitLocation.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetSearchDisplay()
        {
            if (Settings["SearchAddress"] != null)
                addressFirstLine.Visible = Convert.ToBoolean(Settings["SearchAddress"].ToString());
            else
                ltCity.Visible = false;
            if (Settings["SearchCityRegion"] != null)
            {
                ltCity.Visible = Convert.ToBoolean(Settings["SearchCityRegion"].ToString());
                ltRegion.Visible = Convert.ToBoolean(Settings["SearchCityRegion"].ToString());
            }
            else
            {
                ltCity.Visible = false;
                ltRegion.Visible = false;
            }
            if (Settings["SearchPostalCode"] != null)
                ltPostalcode.Visible = Convert.ToBoolean(Settings["SearchPostalCode"].ToString());
            else
                ltPostalcode.Visible = true;
            if (Settings["SearchCountry"] != null)
                ltCountry.Visible = Convert.ToBoolean(Settings["SearchCountry"].ToString());
            else
                ltCountry.Visible = false;

        }

        private void DoWork(string parameters)
        {
            if (BindData(parameters)) //only register map script if binding was successful and we are moving to the map view
            {
                MapProvider mp = GetProvider();
                mp.MapDiv = divMap;
                mp.LocatorMapLabel = lblLocatorMapLabel;
                mp.RptLocations = rptLocations;
                mp.ScrollToViewMore = lblScrollToViewMore;
                string apiKey = Convert.ToString(Settings[mp.GetType().FullName + ".ApiKey"]);
                RegisterMapScripts(mp, apiKey);
                ShowMaps();
            }
            if (ShowMapDefaultDisplay || this._loadDefault)
            {
                ClientScriptManager cs = Page.ClientScript;
                if (!cs.IsStartupScriptRegistered(GetType(), "showAllLocations"))
                {
                    cs.RegisterStartupScript(GetType(), "showAllLocations", "showAllLocations();", true);
                }
            }
            lnkViewMap.Visible = false;

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (DisplayTabId == TabId)
            {                
                if (BindData(string.Empty)) //only register map script if binding was successful and we are moving to the map view
                {
                    MapProvider mp = GetProvider();
                    mp.MapDiv = divMap;
                    mp.LocatorMapLabel = lblLocatorMapLabel;
                    mp.RptLocations = rptLocations;
                    mp.ScrollToViewMore = lblScrollToViewMore;
                    string apiKey = Convert.ToString(Settings[mp.GetType().FullName + ".ApiKey"]);
                    RegisterMapScripts(mp, apiKey);
                    ShowMaps();
                }
            }
            else Response.Redirect(Globals.NavigateURL(DisplayTabId, "", "?search=" + txtLocationPostalCode.Text));

        }

        public MapProvider GetProvider()
        {
            MapProvider mp;
            switch (Convert.ToString(Settings["DisplayProvider"]))
            {
                case "Google":
                    mp = MapProvider.CreateInstance(MapProviderType.GoogleMaps);
                    break;
                case "Yahoo":
                    mp = MapProvider.CreateInstance(MapProviderType.YahooMaps);
                    mp.LatLong = ViewState["UserLocation"] as Pair;
                    mp.SearchCriteria = GetSearchCriteria();
                    break;
                default:
                    mp = MapProvider.CreateInstance(MapProviderType.GoogleMaps);
                    break;
            }
            return mp;
        }

        private void ShowMaps()
        {
            divMap.Visible = true;
            lnkViewMap.Visible = true;
        }

        private bool BindData(string parameters)
        {
            DataTable locations = null;
            string searchCriteria = string.Empty;
            if (parameters == string.Empty )
            {
                searchCriteria = GetSearchCriteria();
            }
            else if (parameters != string.Empty )
            {
                searchCriteria = parameters;
            }
            MapProvider mp = GetProvider();

            //Hide/Show Location Details column based on TabModuleSettings

            string mapsApiKey = Convert.ToString(Settings[mp.GetType().FullName + ".ApiKey"]);

            if (ShowCountry && ddlCountry.SelectedIndex != 0 && _loadDefault == false)
            {
                if (ddlCountry.Items.Count > 0)
                {
                    if ((ddlCountry.SelectedValue != "-1") || (ddlCountry.SelectedValue == String.Empty))
                    {
                        int countryId = int.Parse(ddlCountry.SelectedValue);
                        locations = DataProvider.Instance().GetLocationsByCountry(countryId, PortalId);
                    }
                    else
                    {
                        lblErrorMessage.Text = Localization.GetString("NoCountrySelected", LocalResourceFile);
                        pnlError.Visible = true;
                        return false;
                    }
                }
            }
            else if (!String.IsNullOrEmpty(searchCriteria))
            {
                if (ddlDistance.SelectedValue != Localization.GetString("ChooseOne", LocalResourceFile))                    
                {
                    YahooGeocodeResult yahooResult = new YahooGeocodeResult();
                    GoogleGeocodeResult googleResult = new GoogleGeocodeResult();

                    if (mp.GetType().FullName.Contains("Yahoo"))
                    {
                         yahooResult = SearchUtility.SearchYahoo(searchCriteria, "", "", "", "", mapsApiKey);    
                    }

                    if (mp.GetType().FullName.Contains("Google"))
                    {
                        googleResult = SearchUtility.SearchGoogle(searchCriteria, mapsApiKey);    
                    }

                    locations = CompareProviders(yahooResult, googleResult, locations);
                }
                else
                {
                    lblErrorMessage.Text = Localization.GetString("NoSelection", LocalResourceFile);
                    pnlError.Visible = true;
                    return false;
                }
            }
            else if (ShowDefaultDisplay || this._loadDefault || ShowMapDefaultDisplay)
            {
                locations = GetDefaultLocations();
            }
            else
            {
                lblErrorMessage.Text = Localization.GetString("lblErrorMessage", LocalResourceFile);

            }

            if (locations == null || locations.Rows.Count < 1)
            {
                if (locations == null)
                {
                    lblErrorMessage.Text = Localization.GetString("NoLocationsFound", LocalResourceFile);
                    pnlError.Visible = true;
                    return false;
                }
                if (locations.Rows.Count < 1)
                {
                    lblErrorMessage.Text = Localization.GetString("NoLocationsFound", LocalResourceFile);
                }

                pnlError.Visible = true;
                return false;
            }
            else
            {
                mvwLocator.SetActiveView(vwResults);
                
                lblNumClosest.Text = String.Format(CultureInfo.CurrentCulture, Localization.GetString("lblNumClosest", LocalResourceFile),
                                      locations.Rows.Count);    
                //lblNumClosest.Visible = (locations.Rows.Count == 1);
                rptLocations.DataSource = locations.DefaultView;
                rptLocations.DataBind();
                return true;
            }
        }

        private DataTable GetDefaultLocations()
        {
            ModuleController objModules = new ModuleController();
            string displayTypes = Convert.ToString(objModules.GetTabModuleSettings(TabModuleId)["DisplayTypes"]);
            string[] displayTypesArray = displayTypes.Split(',');
            return DataProvider.Instance().GetAllLocationsByType(PortalId, displayTypesArray);
        }

        private DataTable CompareProviders(YahooGeocodeResult yahooResult, GoogleGeocodeResult googleResult, DataTable locations)
        {
            if (yahooResult.statusCode == YahooStatusCode.Success ||
                googleResult.statusCode == GoogleStatusCode.Success)
            {
                double latitude = 0;
                double longitude = 0;
                if (yahooResult.statusCode == YahooStatusCode.Success)
                {
                        latitude = yahooResult.latitude;
                        longitude = yahooResult.longitude;
                }
                else if (googleResult.statusCode == GoogleStatusCode.Success)
                {
                    latitude = googleResult.latitude;
                    longitude = googleResult.longitude;
                }

                ModuleController objModules = new ModuleController();
                string locationTypes = Convert.ToString(objModules.GetTabModuleSettings(TabModuleId)["DisplayTypes"]);
                string[] values = locationTypes.Split(',');
                int[] locationTypeIds = Array.ConvertAll<string, int>(values, delegate(string s) {return int.Parse(s); });
               
                if(ddlDistance.SelectedValue == Localization.GetString("UnlimitedMiles", LocalResourceFile))
                {
                    locations = DataProvider.Instance().GetAllLocationsByDistance(latitude, longitude, PortalId, locationTypeIds);
                }
                else
                {
                    locations = DataProvider.Instance().GetClosestLocationsByRadius(latitude, longitude, int.Parse(ddlDistance.SelectedValue), PortalId, locationTypeIds);}

                ViewState["UserLocation"] = new Pair(latitude, longitude);
            }
            else
            {
                lblErrorMessage.Text = "An unknown error occurred.";
                switch (yahooResult.statusCode)
                {
                    case YahooStatusCode.BadRequest: //400
                        lblErrorMessage.Text = "The entered location could not be found.";
                        break;
                    case YahooStatusCode.Forbidden: //403
                        lblErrorMessage.Text =
                            "The provided Yahoo! Maps API key is not working, or the rate limit has been reached.  Please try again later.";
                        break;
                    case YahooStatusCode.ServiceUnavailable: //503
                        lblErrorMessage.Text =
                            "The locator service is temporarily unavailable.  We apologize for the inconvenience.  Please try again.";
                        break;
                }
            }
            return locations;
        }

        //protected void dgLocations_ItemDataBound(object sender, DataGridItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        //    {
        //        DataRowView row = e.Item.DataItem as DataRowView;
        //        HyperLink lnkMapIt = (HyperLink) e.Item.FindControl("lnkMapIt");
        //        Label lblLocationsGridDistance = (Label) e.Item.FindControl("lblLocationsGridDistance");
        //        Label lblCurrentlyMapped = (Label) e.Item.FindControl("lblCurrentlyMapped");
        //        if (row != null) lnkMapIt.Visible = !row["latitude"].Equals(DBNull.Value);
        //        if (row != null) lblLocationsGridDistance.Visible = row.DataView.Table.Columns.IndexOf("Distance") > -1;

        //        if (lnkMapIt.Visible)
        //        {
        //            if (row != null)
        //            {
        //                lnkMapIt.Attributes.Add("onclick", "showLocation(" + row["latitude"] + ", " + 
        //                    row["longitude"] + ", " + (e.Item.ItemIndex + 1) + ", \"" +
        //                    Server.HtmlEncode(row["Address"].ToString().Replace("'", "")) + "%%" +
        //                    Server.HtmlEncode(row["City"].ToString().Replace("'", "")) + ", " +
        //                    Server.HtmlEncode(row["Abbreviation"].ToString().Replace("'", "")) + " " +
        //                    Server.HtmlEncode(row["PostalCode"].ToString().Replace("'", "")) +
        //                    "\", \"" + ")" + "Test" + 
        //                    Server.HtmlEncode(row["LocationName"].ToString().Replace("'", "")) + "\", '" + lblCurrentlyMapped.ClientID + "')");
                        
        //            }
        //        }

        //        if (lblLocationsGridDistance.Visible)
        //        {
        //            if (row != null)
        //                lblLocationsGridDistance.Text =
        //                    String.Format(CultureInfo.CurrentCulture, "{0:#.00}", (double) row["Distance"]) +
        //                    Localization.GetString("Miles", LocalResourceFile);
        //        }
        //    }
        //}

        protected void btnBack_Click(object sender, EventArgs e)
        {
            mvwLocator.SetActiveView(vwLocator);

            this.txtLocationAddress.Text = string.Empty;
            this.txtLocationCity.Text = string.Empty;
            this.ddlLocationRegion.SelectedIndex = 0;
            this.txtLocationPostalCode.Text = string.Empty;
            this.ddlLocatorCountry.SelectedIndex = 0;
            if (ShowCountry)
            {
                ddlCountry.SelectedIndex = 0;
                ddlDistance.SelectedIndex = 0;
            }
            else ddlCountry.SelectedValue = Localization.GetString("UnlimitedMiles", LocalResourceFile);
            pnlError.Visible = false;

            divMap.Style[HtmlTextWriterStyle.Display] = "none";
        }


        private void RegisterMapScripts(MapProvider mp, string apiKey)
        {
            
            Page.ClientScript.RegisterClientScriptInclude(divMap.GetType(), "includeMap", mp.GetMapUrl(apiKey));

            string mapType = Settings["MapType"].ToString();
            string s = mp.GenerateMapScript(mapType);

            if (mp.LocationCount > 0)
            {
                Page.ClientScript.RegisterStartupScript(divMap.GetType(), "drawMap", s, false);
            }
        }

        /// <summary>
        /// This method generates a string formatted like ...... and returns it!!!! John
        /// </summary>
        /// <returns></returns>
        private string GetSearchCriteria()
        {
            StringBuilder criteria = new StringBuilder();
            if(this.txtLocationAddress.Text != string.Empty)
            {
                criteria.AppendFormat(txtLocationAddress.Text.Replace(' ', '+'));
            }
            if(this.txtLocationCity.Text != string.Empty)
            {
                if (criteria.ToString() != string.Empty) criteria.AppendFormat("+");
                 criteria.AppendFormat(this.txtLocationCity.Text.Replace(' ', '+'));
            }
            if (this.ddlLocationRegion.SelectedIndex > 0)
            {
                if (criteria.ToString() != string.Empty) criteria.AppendFormat("+");
                criteria.AppendFormat(this.ddlLocationRegion.SelectedItem.Text.Replace(' ', '+'));
            }
            if (this.txtLocationPostalCode.Text != string.Empty)
            {
                if (criteria.ToString() != string.Empty) criteria.AppendFormat("+");
                criteria.AppendFormat(this.txtLocationPostalCode.Text.Replace(' ', '+'));
            }
            if (this.ddlLocatorCountry.SelectedIndex > 0)
            {
                if (criteria.ToString() != string.Empty) criteria.AppendFormat("+");
                criteria.AppendFormat(this.ddlLocatorCountry.SelectedItem.Text.Replace(' ', '+'));
            }
            return criteria.ToString();
        }

        private void FillDropDowns()
        {
            ModuleController objModules = new ModuleController();
            Hashtable settings = objModules.GetTabModuleSettings(TabModuleId);

            if (settings.Contains("SearchTitle")) lblSearchTitle.Text = settings["SearchTitle"].ToString();
            if (settings.Contains("Address")) pnlAddress.Visible = Convert.ToBoolean(settings["Address"]);
            if (settings.Contains("Country")) pnlCountry.Visible = Convert.ToBoolean(settings["Country"]);
            if (settings.Contains("Radius")) pnlDistance.Visible = Convert.ToBoolean(settings["Radius"]);

            foreach (ListItem li in ddlDistance.Items)
            {
                li.Value = li.Value;
                li.Text =
                    String.Format(CultureInfo.CurrentCulture, "{0} {1}", li.Value,
                                  Localization.GetString("Miles", LocalResourceFile));
            }

            ddlDistance.Items.Add(Localization.GetString("UnlimitedMiles", LocalResourceFile));
            ddlDistance.DataBind();
            ddlDistance.SelectedIndex = ddlDistance.Items.IndexOf(ddlDistance.Items.FindByText(Localization.GetString("UnlimitedMiles", LocalResourceFile)));

            //Load the state list based on country
            ListController controller = new ListController();
            ListEntryInfoCollection states = controller.GetListEntryInfoCollection("Region");

            ddlLocationRegion.DataSource = states;
            ddlLocationRegion.DataTextField = "Text";
            ddlLocationRegion.DataValueField = "EntryID";
            ddlLocationRegion.DataBind();
            ddlLocationRegion.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));

            //fill the country dropdown
            ListController countryController = new ListController();
            ListEntryInfoCollection countries = countryController.GetListEntryInfoCollection("Country");

            ddlLocatorCountry.DataSource = countries;
            ddlLocatorCountry.DataTextField = "Text";
            ddlLocatorCountry.DataValueField = "EntryId";
            ddlLocatorCountry.DataBind();
            ddlLocatorCountry.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));
            ddlLocatorCountry.SelectedIndex = 0;

            if(ShowRadius)
            {                
                if(ShowCountry)
                {
                    ddlDistance.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));
                    ddlDistance.SelectedIndex = 0;
                }
            }

            if(ShowCountry)
            {
                FillCountry();
            }
         }

        #region Optional Interfaces

        ModuleActionCollection IActionable.ModuleActions
        {
            get
            {
                ModuleActionCollection mActions = new ModuleActionCollection();

                mActions.Add(GetNextActionID(), Localization.GetString("Manage", LocalResourceFile),
                             ModuleActionType.AddContent, "", "", EditUrl("Import"), false, SecurityAccessLevel.Edit,
                             true, false);
                return mActions;
            }
        }

        #endregion

        private void FillCountry()
        {
            //Check if we have any countries yet
            DataTable dt = DataProvider.Instance().GetCountriesList(PortalId);
            if (dt.Rows.Count > 0)
            {
                ddlCountry.DataSource = dt;
                ddlCountry.DataTextField = "Text";
                ddlCountry.DataValueField = "EntryId";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));
                
                //if we don't have US don't remove and add
                ModuleController objModules = new ModuleController();

                string countryId = objModules.GetTabModuleSettings(TabModuleId)["ShowLocationDetails"].ToString();

                ListItem liDefaultCountry = ddlCountry.Items.FindByValue(countryId);
                if (liDefaultCountry != null)
                {
                    ddlCountry.Items.Remove(liDefaultCountry);//remove US and add it back at the top of the list
                    ddlCountry.Items.Insert(1, liDefaultCountry);

                }


            }

        }

        public bool IsConfigured(int tabModuleId, ref string error)
        {
            Hashtable settings = DotNetNuke.Entities.Portals.PortalSettings.GetTabModuleSettings(tabModuleId);
            string mapProvider = Convert.ToString(settings["DisplayProvider"]);
            if (mapProvider == String.Empty)
            {
                error = "No Map Provider Selected.";
                return false;
            }
            else
            {
                
                string className = MapProviderType.GetMapProviderClassByName(mapProvider);
                string apiKey = Convert.ToString(settings[className + ".ApiKey"]);
                if (apiKey == String.Empty)
                {
                    error = "No API Key Entered.";
                    return false;
                }
                else
                {
                    MapProvider mp = MapProvider.CreateInstance(className);
                    if (mp.IsKeyValid(apiKey))
                    {
                        error = "Success";
                        return true;    
                    }
                    else
                    {
                        error = "API Key is not in the correct format.";
                        return false;   
                    }
                }
            }

            if (Convert.ToString(settings["DisplayProvider"]) == String.Empty)
            {
                error = "Search Setting \"Results Display Module\" not set.";
                return false;
            }

            if (Convert.ToString(settings["ShowLocationDetails"]) == String.Empty)
            {
                error = "Display Setting \"Show Location Details\" not set.";
                return false;
            }
        }

        protected void rptLocations_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                LinkButton lnkMapIt = (LinkButton)e.Item.FindControl("lnkMapIt");
                Label lblLocationsGridDistance = (Label) e.Item.FindControl("lblLocationsGridDistance");
                Label lblCurrentlyMapped = (Label) e.Item.FindControl("lblCurrentlyMapped");
                Label lblLocationDetails = (Label)e.Item.FindControl("lblLocationDetails");
                lblLocationDetails.Visible = (ShowLocationDetails == "SamePage" || ShowLocationDetails == "True");
                Label lblLocationMapNumber = (Label)e.Item.FindControl("lblLocationMapNumber");
                lblLocationMapNumber.Text = Convert.ToString(e.Item.ItemIndex + 1) + ") ";
                if(ShowLocationDetails == "DetailsPage") if (row != null)
                {
                    HyperLink lnkShowLocationDetails = (HyperLink)e.Item.FindControl("lnkShowLocationDetails");
                    lnkShowLocationDetails.Visible = true;
                    lnkShowLocationDetails.NavigateUrl = Globals.NavigateURL(TabId, "Details", "mid=" + ModuleId + "&lid=" + row["LocationId"]);
                    lnkShowLocationDetails.Text = Localization.GetString("lnkShowLocationDetails", LocalResourceFile);
                }
                if (row != null) lnkMapIt.Visible = !row["latitude"].Equals(DBNull.Value);
                if (row != null) lblLocationsGridDistance.Visible = row.DataView.Table.Columns.IndexOf("Distance") > -1;
                if (row != null)
                {
                    string[] attrib = new string[2];
                    attrib[0] = "LocationId";
                    attrib[1] = row["LocationId"].ToString();

                    if (lnkMapIt.Visible)
                    {
                        lnkMapIt.Attributes.Add("onclick",
                                                "showLocation(" + row["latitude"] + ", " + row["longitude"] + ", " +
                                                (e.Item.ItemIndex) + ", \"" +
                                                Server.HtmlEncode(row["Address"].ToString().Replace("'", "")) + "%%" +
                                                Server.HtmlEncode(row["City"].ToString().Replace("'", "")) + ", " +
                                                Server.HtmlEncode(row["Abbreviation"].ToString().Replace("'", "")) + " " +
                                                Server.HtmlEncode(row["PostalCode"].ToString().Replace("'", "")) +
                                                "\", \"" + 
                                                Server.HtmlEncode(row["Name"].ToString().Replace("'", "")) +
                                                "\", '" + lblCurrentlyMapped.ClientID + "'); return false;");
                    }

                    if (lblLocationsGridDistance.Visible)
                    {
                        lblLocationsGridDistance.Text =
                                String.Format(CultureInfo.CurrentCulture, "{0:#.00}", (double) row["Distance"]) +
                                Localization.GetString("Miles", LocalResourceFile);
                    }
                }
            }
        }


        protected void ddlDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ShowCountry)
            {ddlCountry.SelectedIndex = 0;}
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ShowRadius)
            {ddlDistance.SelectedIndex = 0;}
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            this.txtLocationAddress.Text = string.Empty;
            this.txtLocationCity.Text = string.Empty;
            this.ddlLocationRegion.SelectedIndex = 0;
            this.txtLocationPostalCode.Text = string.Empty;
            this.ddlLocatorCountry.SelectedIndex = 0;

            ddlCountry.SelectedIndex = -1;
            _loadDefault = true;
            DoWork(null);
            _loadDefault = false;
        }

        protected void lnkSubmitLocations_Click(object sender, EventArgs e)
        {
             Response.Redirect(Globals.NavigateURL(TabId, "ManageLocations", "mid=" + ModuleId));
        }
    }
}