using System;
using System.Data;
using System.Text;
using System.Web.UI;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Tabs;
using Engage.Dnn.Locator.Data;

namespace Engage.Dnn.Locator
{
    partial class MiniLocator : PortalModuleBase
    {
        public Location location;
        protected void Page_Load(Object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["LocationId"] == "")
                {
                    Session.Add("LocationId", Request.QueryString["LocationId"]);
                }
                ShowData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void ShowData()
        {
            TabController tc = new TabController();

            if (Session["LocationId"] != null)
            {
                pnlZipCode.Visible = false;
                pnlAddress.Visible = true;
                lnkSubmit.Visible = false;
                lnkLocator.Visible = true;
                int selectedLocation = Convert.ToInt32(Session["LocationId"].ToString());
                
                lnkLocator.Text = "<a href=" +
                                         DotNetNuke.Common.Globals.NavigateURL(
                                             tc.GetTabByName("Engage Locator", PortalId).TabID, "") + "><b>Change Location</b></a>";

                if (selectedLocation >= 0)
                {
                    location = Location.Create(DataProvider.Instance().GetLocation(selectedLocation).Rows[0]);
                    lblLocationName.Text = location.Name;
                    lblAddress.Text = location.Address;
                    CreateLocationString();
                    BindData();
                    MapProvider mp = GetProvider();
                    mp.MapDiv = divMap;

                    ModuleController mc = new ModuleController();
                    string apiKey = mc.GetTabModuleSettings(mc.GetModuleByDefinition(PortalId, "EngageLocator").TabModuleID)[mp.GetType().FullName + ".ApiKey"].ToString();
                    RegisterMapScripts(mp, apiKey);


                }
            }
            else
            {
                pnlZipCode.Visible = true;
                pnlAddress.Visible = false;
                lnkLocator.Visible = false;
            }
        }

        private void CreateLocationString()
        {
            if (location.StateName != "")
            {
                if (location.Name != "")
                {
                    lblLocation.Text = location.Name + ", " + location.StateName;
                }
                else
                {
                    lblLocation.Text = location.StateName;    
                }
                            
            }
            else
            {
                lblLocation.Text = location.Name;
            }

            if (location.PostalCode != "")
            {
                if (lblLocation.Text != "")
                {
                    lblLocation.Text = lblLocation.Text + ", " + location.PostalCode;
                }
                else
                {
                    lblLocation.Text = location.PostalCode;
                }
            }
        }

        private void RegisterMapScripts(MapProvider mp, string apiKey)
        {
            Page.ClientScript.RegisterClientScriptInclude(divMap.GetType(), "includeMap", mp.GetMapUrl(apiKey));
            string s = mp.GenerateMiniMapScript();
            Page.ClientScript.RegisterStartupScript(divMap.GetType(), "drawMap", s, false);
        }

        private void BindData()
        {
            
            string searchCriteria = GetSearchCriteria();
            MapProvider mp = GetProvider();

            ModuleController mc = new ModuleController();
            string mapsApiKey = mc.GetTabModuleSettings(mc.GetModuleByDefinition(PortalId, "EngageLocator").TabModuleID)[mp.GetType().FullName + ".ApiKey"].ToString();
            

            YahooGeocodeResult yahooResult = SearchUtility.SearchYahoo(searchCriteria, "", "", "", txtZipCode.Text, mapsApiKey);
            GoogleGeocodeResult googleResult = SearchUtility.SearchGoogle(searchCriteria, mapsApiKey);

            if (yahooResult.statusCode == YahooStatusCode.Success || googleResult.statusCode == GoogleStatusCode.Success)
            {
                double latitude;
                double longitude;
                if (yahooResult.statusCode == YahooStatusCode.Success && googleResult.statusCode == GoogleStatusCode.Success)
                {
                    if (SearchUtility.IsYahooMoreAccurateThanGoogle(yahooResult.accuracyCode, googleResult.accuracyCode))
                    {
                        latitude = yahooResult.latitude;
                        longitude = yahooResult.longitude;
                    }
                    else
                    {
                        latitude = googleResult.latitude;
                        longitude = googleResult.longitude;
                    }
                }
                else if (yahooResult.statusCode == YahooStatusCode.Success)
                {
                    latitude = yahooResult.latitude;
                    longitude = yahooResult.longitude;
                }
                else //Google must be successful
                {
                    latitude = googleResult.latitude;
                    longitude = googleResult.longitude;
                }
                
                dvMapImage.Attributes.Add("onmouseover", "ShowEventInfo(event);showLocation(" + latitude + ", " + longitude + ", " + 1 + ", \"" + lblAddress.Text.Replace("'", "") + "%%" + location.City.Replace("'", "") + "\", \"" + lblLocationName.Text.Replace("'", "") + "\", '" + "" + "');");
                dvMapImage.Attributes.Add("onmouseout", "hideMap()");

                ViewState["UserLocation"] = new Pair(latitude, longitude);
            }
        }

        private MapProvider GetProvider()
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

        private string GetSearchCriteria()
        {
            StringBuilder criteria = new StringBuilder();
            if (!String.IsNullOrEmpty(location.City))
            {
                if (!String.IsNullOrEmpty(lblAddress.Text))
                {
                    criteria.Append(lblAddress.Text + ", " + location.City);
                }
                else
                {
                    criteria.Append(location.City);    
                }
            }

            return criteria.ToString();
        }

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            ModuleController mc = new ModuleController();
            MapProvider mp = GetProvider();
            string mapsApiKey = mc.GetTabModuleSettings(mc.GetModuleByDefinition(PortalId, "EngageLocator").TabModuleID)[mp.GetType().FullName + ".ApiKey"].ToString();
            
            DataTable dt = GetClosetLocation(mapsApiKey);

            if (dt.Rows.Count > 0)
            {
                Session.Add("LocationId", dt.Rows[0]["LocationId"].ToString());
                ShowData();
            }
            else
            {
                txtZipCode.Text = "No Locations Found";
            }

            
        }

        private DataTable GetClosetLocation(string mapsApiKey)
        {
            DataTable dt = new DataTable();
            GoogleGeocodeResult googleResult = SearchUtility.SearchGoogle(txtZipCode.Text, mapsApiKey);
            YahooGeocodeResult yahooResult =
                SearchUtility.SearchYahoo(txtZipCode.Text, "", "", "",
                                          txtZipCode.Text, mapsApiKey);
            if (googleResult.statusCode == GoogleStatusCode.Success)
            {
                dt = DataProvider.Instance().GetNClosestLocations(googleResult.latitude, googleResult.longitude, 1, PortalId);
            }
            else if (yahooResult.statusCode == YahooStatusCode.Success)
            {
                dt = DataProvider.Instance().GetNClosestLocations(yahooResult.latitude, yahooResult.longitude, 1, PortalId);
            }

            return dt;
        }

        protected void lnklocator_Click(object sender, EventArgs e)
        {
            Session.Remove("LocationId");
        }
    }


}