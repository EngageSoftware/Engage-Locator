using System;
using System.Data;
using System.Globalization;
using System.Text;
using DotNetNuke.Common;

namespace Engage.Dnn.Locator
{
    public class YahooProvider:MapProvider
    {
        #region MapProvider Members

        public static string SearchUrl
        {
            get { return "http://api.local.yahoo.com/MapsService/V1/geocode?output=xml"; }
        }

        public override string Url
        {
            get{ return SearchUrl;}
        }

        public override string MapProviderUrl
        {
            get { return "http://maps.yahoo.com/broadband#q1="; }
        }

        public override string GenerateMapScriptCore(string mapType)
        {
            if (SearchCriteria == null) { throw new InvalidOperationException("Search Criteria is not defined"); }
            if (LatLong == null) { throw new InvalidOperationException("Latitude and Longitude is not defined"); }

            StringBuilder script = new StringBuilder(2000);
            script.AppendFormat(CultureInfo.InvariantCulture, @"<script type=""text/javascript"">{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "function createCenterMarker(latitude, longitude) {{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var myPoint = new YGeoPoint(latitude, longitude);{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "var myImage = new YImage();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "myImage.src = '{1}';{0}", Environment.NewLine, Globals.ResolveUrl("~/images/ratingminus.gif"));
            script.AppendFormat(CultureInfo.InvariantCulture, "myImage.size = new YSize(18,18);{0}", Environment.NewLine);

            // Create a marker positioned at a lat/lon
            script.AppendFormat(CultureInfo.InvariantCulture, "var marker = new YMarker(myPoint, myImage);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "return marker;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "function createLocationMarker(latitude, longitude, index, address) {{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var myPoint = new YGeoPoint(latitude, longitude);{0}", Environment.NewLine);
            // Create a marker positioned at a lat/lon
            script.AppendFormat(CultureInfo.InvariantCulture, "var marker = new YMarker(myPoint);{0}", Environment.NewLine);
            // Add a label to the marker
            script.AppendFormat(CultureInfo.InvariantCulture, "marker.addLabel(index);{0}", Environment.NewLine);
            // Add auto expand
            script.AppendFormat(CultureInfo.InvariantCulture, "marker.addAutoExpand(address.replace(\"%%\", \"<br />\"));{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "return marker;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);

            //function to show all locations on one map
            script.AppendFormat(CultureInfo.InvariantCulture, "function showAllLocations() {{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "displayMap(); {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var marker;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var arr = new Array();{0}", Environment.NewLine);

            int arrIndex = 0;
            if (LatLong != null)
            {
                script.AppendFormat(CultureInfo.InvariantCulture, "marker = createCenterMarker({1}, {2});{0}", Environment.NewLine, LatLong.First, LatLong.Second);
                script.AppendFormat(CultureInfo.InvariantCulture, "map.addOverlay(marker);{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "arr[{1}] = marker.YGeoPoint;{0}", Environment.NewLine, arrIndex++);
            }

            if (RptLocations.DataSource != null)
            {
                foreach (DataRowView row in (DataView)RptLocations.DataSource)
                {
                    if (row.DataView.Table.Columns.Contains("Latitude") && row.DataView.Table.Columns.Contains("Longitude") && !row["Latitude"].Equals(DBNull.Value) && !row["Longitude"].Equals(DBNull.Value))
                    {
                        script.AppendFormat(CultureInfo.InvariantCulture, "marker = createLocationMarker({1}, {2}, {3}, '{4}<br/>{5}<br/>{6}');{0}", Environment.NewLine, row["Latitude"], row["Longitude"], arrIndex, row["Name"].ToString().Replace("'", "\\'"), row["Address"].ToString().Replace("'", "\\'"), row["City"].ToString().Replace("'", "\\'"));
                        script.AppendFormat(CultureInfo.InvariantCulture, "map.addOverlay(marker);{0}", Environment.NewLine);
                        script.AppendFormat(CultureInfo.InvariantCulture, "arr[{1}] = marker.YGeoPoint;{0}", Environment.NewLine, arrIndex++);
                        LocationCount++;
                    }
                }
            }

            // Display the map centered on a latitude and longitude
            if (LatLong != null)
            {
                script.AppendFormat(CultureInfo.InvariantCulture, "var zoomLevel = map.getZoomLevel(arr);{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "map.drawZoomAndCenter(arr[0], zoomLevel + 1);{0}", Environment.NewLine);
            }
            else if (arrIndex > 0)
            {
                script.AppendFormat(CultureInfo.InvariantCulture, "var bestZoom = map.getBestZoomAndCenter(arr);{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "map.drawZoomAndCenter(bestZoom.YGeoPoint, bestZoom.zoomLevel);{0}", Environment.NewLine);
            }
            else
            {
                script.AppendFormat(CultureInfo.InvariantCulture, "map.drawZoomAndCenter('{1}', 10);{0}", Environment.NewLine, SearchCriteria);
            }
            script.AppendFormat(CultureInfo.InvariantCulture, "var allLabel = document.getElementById('{1}');{0}", Environment.NewLine, LocatorMapLabel.ClientID);
            script.AppendFormat(CultureInfo.InvariantCulture, "allLabel.style.display = '';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var label = document.getElementById('locatorMapLabel');{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "label.style.display = 'none';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var scrollLabel = document.getElementById('{1}');{0}", Environment.NewLine, ScrollToViewMore.ClientID);
            script.AppendFormat(CultureInfo.InvariantCulture, "scrollLabel.style.display = 'none';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var mapLink = document.getElementById('lblMapLink');{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "mapLink.style.display = 'none';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var currentlyMappedLabels = getElementsByClassName('currentlyMapped', 'span', document.getElementById('{1}'));{0}", Environment.NewLine, RptLocations.ClientID);
            script.AppendFormat(CultureInfo.InvariantCulture, "var length = currentlyMappedLabels.length;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "for (var i = 0; i < length; i++) {{{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "currentlyMappedLabels[i].className = 'hideCurrentlyMapped';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);

            //function for individual location map
            script.AppendFormat(CultureInfo.InvariantCulture, "function showLocation(latitude, longitude, index, address, title, labelId) {{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "displayMap(); {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.removeMarkersAll(); {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var marker;{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "marker = createLocationMarker(latitude, longitude, index, title + '<br />' + address);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.addOverlay(marker);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var myPoint = marker.YGeoPoint;{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "map.drawZoomAndCenter(myPoint, 6);{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "var allLabel = document.getElementById('{1}');{0}", Environment.NewLine, LocatorMapLabel.ClientID);
            script.AppendFormat(CultureInfo.InvariantCulture, "allLabel.style.display = 'none';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var label = document.getElementById('locatorMapLabel');{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "label.style.display = '';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "label.innerHTML = title + '<br />' + address.replace(\"%%\", \"<br />\");{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var scrollLabel = document.getElementById('{1}');{0}", Environment.NewLine, ScrollToViewMore.ClientID);
            script.AppendFormat(CultureInfo.InvariantCulture, "scrollLabel.style.display = '';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var mapLink = document.getElementById('lblMapLink');{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "mapLink.style.display = '';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var lnkDrivingDirections = document.getElementById('lnkDrivingDirections');{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var mapLoc = 'http://maps.yahoo.com/broadband#q1=' + address;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "lnkDrivingDirections.href = mapLoc.replace(\"%%\", \" \");{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var currentlyMappedLabel = document.getElementById(labelId);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var currentlyMappedLabels = getElementsByClassName('currentlyMapped', 'span', document.getElementById('{1}'));{0}", Environment.NewLine, RptLocations.ClientID);
            script.AppendFormat(CultureInfo.InvariantCulture, "var length = currentlyMappedLabels.length;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "for (var i = 0; i < length; i++) {{{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "currentlyMappedLabels[i].className = 'hideCurrentlyMapped';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "currentlyMappedLabel.className = 'currentlyMapped';{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);

            //function for individual location map
            script.AppendFormat(CultureInfo.InvariantCulture, "function displayMap() {{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var mapDiv = document.getElementById('{1}');{0}", Environment.NewLine, MapDiv.ClientID);
            //script.AppendFormat(CultureInfo.InvariantCulture, "var mapLink = document.getElementById('{1}');{0}", Environment.NewLine, lblViewMap.ClientID);
            script.AppendFormat(CultureInfo.InvariantCulture, "mapDiv.style.display = '' ;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "document.location = '#map';{0}", Environment.NewLine);
            //script.AppendFormat(CultureInfo.InvariantCulture, "mapLink.style.display = 'none' ;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "function getElementsByClassName(className, tag, elm){{{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var testClass = new RegExp(\"(^|\\s)\" + className + \"(\\s|$)\");{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var tag = tag || \"*\";{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var elm = elm || document;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var elements = (tag == \"*\" && elm.all)? elm.all : elm.getElementsByTagName(tag);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var returnElements = [];{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var current;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var length = elements.length;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "for(var i=0; i<length; i++){{{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "current = elements[i];{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "if(testClass.test(current.className)){{{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "returnElements.push(current);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "return returnElements;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}");

            // Create a map object
            script.AppendFormat(CultureInfo.InvariantCulture, "var map = new YMap(document.getElementById('{1}'));{0}", Environment.NewLine, MapDiv.ClientID);
            // Add map type control
            script.AppendFormat(CultureInfo.InvariantCulture, "map.addTypeControl();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.addPanControl();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.addZoomLong();{0}", Environment.NewLine);

            // Set map type to either of: YAHOO_MAP_SAT YAHOO_MAP_HYB YAHOO_MAP_REG
            if (mapType == "Normal") 
                script.AppendFormat(CultureInfo.InvariantCulture, "map.setMapType(YAHOO_MAP_REG);{0}", Environment.NewLine);
            else if (mapType == "Satellite")
                script.AppendFormat(CultureInfo.InvariantCulture, "map.setMapType(YAHOO_MAP_SAT);{0}", Environment.NewLine);
            if (mapType == "Hybrid")
                script.AppendFormat(CultureInfo.InvariantCulture, "map.setMapType(YAHOO_MAP_HYB);{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "</script>");

            return script.ToString();
        }

        public override string GenerateMiniMapScriptCore()
        {
            StringBuilder script = new StringBuilder(3000);
            script.AppendFormat(CultureInfo.InvariantCulture, @"<script type=""text/javascript"">{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "//<![CDATA[{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, @"var map = new GMap2(document.getElementById(""{1}""));{0}", Environment.NewLine, MapDiv.ClientID);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.addControl(new GSmallMapControl());{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.addControl(new GMapTypeControl());{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "//]]>{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "</script>");
            return script.ToString();
        }

        public override string GetMapUrl(string apiKey)
        {
            if (String.IsNullOrEmpty("apiKey")) {throw new ArgumentNullException("apiKey", "ApiKey is Invalid");}
            return "http://api.maps.yahoo.com/ajaxymap?v=3.0&appid=" + apiKey;
        }

        public override bool IsKeyValid(string apiKey)
        {
            return (apiKey != null && (apiKey.Length > 64 && apiKey.Length < 74));
        }

        #endregion
    }

    public enum YahooStatusCode
    {
        Success = 200,
        /// <summary>
        /// The parameters passed to the service did not match as expected. The Message should tell you what was missing or incorrect.
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// You do not have permission to access this resource, or are over your rate limit.
        /// </summary>
        Forbidden = 403,
        /// <summary>
        /// An internal problem prevented us from returning data to you.
        /// </summary>
        ServiceUnavailable = 503
    }

    public enum YahooAccuracyCode
    {
        Unknown = 0,
        Country = 1,
        State = 2,
        City = 4,
        Zip = 5,
        ZipPlus2 = -1,
        ZipPlus4 = -2,
        Street = 6,
        Address = 8
    }

    public struct YahooGeocodeResult
    {
        public double latitude;
        public double longitude;

        public YahooAccuracyCode accuracyCode;
        public YahooStatusCode statusCode;
    }
}