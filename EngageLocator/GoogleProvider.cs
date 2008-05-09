using System;
using System.Data;
using System.Globalization;
using System.Text;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;

namespace Engage.Dnn.Locator
{
    public class GoogleProvider:MapProvider   
    {
        private int _locatorTabModuleId;

        public int LocatorTabModuleId
        {
            get { return _locatorTabModuleId; }
            set { _locatorTabModuleId = value; }
        }

        #region MapProvider Members

        public override string MapProviderUrl
        {
            get { return "http://maps.google.com/maps?f=q&hl=en&geocode=&q="; }
        }

        public override string GenerateMiniMapScriptCore()
        {
            StringBuilder script = new StringBuilder(3000);
            
            ScriptStart(script);
            DisplayMap(script);
            HideMap(script);
            MapLocation(script);
            CreateCenterMarker(script);
            CreateLocationMarker(script);
            ScriptEnd(script);

            return script.ToString();
        }

        public override string GenerateMapScriptCore(string mapType)
        {
            StringBuilder script = new StringBuilder(3000);
            
            ScriptStart(script);
            ScriptControls(script);
            DisplayMap(script);
            MapLocation(script);
            MapLocations(script);
            MapType(script, mapType);
            DisplayMapInfo(script);
            GetZoomLevel(script);
            CreateCenterMarker(script);
            CreateLocationMarker(script);
            GetElements(script);
            ScriptEnd(script);

            return script.ToString();
        }

        private static void ScriptEnd(StringBuilder script)
        {
            script.AppendFormat(CultureInfo.InvariantCulture, "//]]>{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "</script>");
        }

        private void ScriptStart(StringBuilder script)
        {
            script.AppendFormat(CultureInfo.InvariantCulture, @"<script type=""text/javascript"">{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "//<![CDATA[{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, @"var map = new GMap2(document.getElementById(""{1}""));{0}", Environment.NewLine, MapDiv.ClientID);
        }

        private static void ScriptControls(StringBuilder script)
        {
            script.AppendFormat(CultureInfo.InvariantCulture, "map.addControl(new GSmallMapControl());{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.addControl(new GMapTypeControl());{0}", Environment.NewLine);
        }

        private static void GetElements(StringBuilder script)
        {
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
        }

        private static void GetZoomLevel(StringBuilder script)
        {
//Get Best Zoom Level
            script.AppendFormat(CultureInfo.InvariantCulture, "function getBounds(mapType, size, arr){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var south, west, east, north;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "south = west = 180;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "east = north = -180;{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "for (var i = 0; i < arr.length; i++){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "if (arr[i].getPoint().lat() > north){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "north = arr[i].getPoint().lat();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "if (arr[i].getPoint().lat() < south){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "south = arr[i].getPoint().lat();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "if (arr[i].getPoint().lng() > east){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "east = arr[i].getPoint().lng();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "if (arr[i].getPoint().lng() < west){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "west = arr[i].getPoint().lng();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "var southWest = new GLatLng(south, west);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var northEast = new GLatLng(north, east);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var bounds = new GLatLngBounds(southWest, northEast);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var zoomLevel = mapType.getBoundsZoomLevel(bounds, map.getSize());{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "var minZoom = mapType.getMinimumResolution(arr[0]);{0}", Environment.NewLine);
            //script.AppendFormat(CultureInfo.InvariantCulture, "var maxZoom = mapType.getMaximumResolution(arr[0]);{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "if (zoomLevel < minZoom){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "zoomLevel = minZoom;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
            //script.AppendFormat(CultureInfo.InvariantCulture, "if (zoomLevel > maxZoom){0}", Environment.NewLine);
            //script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            //script.AppendFormat(CultureInfo.InvariantCulture, "zoomLevel = maxZoom;{0}", Environment.NewLine);
            //script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "return bounds;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
        }

        private void DisplayMapInfo(StringBuilder script)
        {
//Display Visual Labels
            script.AppendFormat(CultureInfo.InvariantCulture, "function displayInfo(){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
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
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
        }

        private static void CreateCenterMarker(StringBuilder script)
        {
            script.AppendFormat(CultureInfo.InvariantCulture, "function createCenterMarker(latitude, longitude){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var myPoint = new GLatLng(latitude, longitude);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var myImage = new GIcon();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, @"myImage.image = ""{1}"";{0}", Environment.NewLine, Globals.ResolveUrl("~/images/ratingminus.gif"));
            script.AppendFormat(CultureInfo.InvariantCulture, "myImage.iconSize = new GSize(18, 18);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "myImage.iconAnchor = new GPoint(0, 0);{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "var marker = new GMarker(myPoint, myImage);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "return marker;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
        }

        private static void CreateLocationMarker(StringBuilder script)
        {
            script.AppendFormat(CultureInfo.InvariantCulture, "function createLocationMarker(latitude, longitude, index, address){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var myPoint = new GLatLng(latitude, longitude);{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, "var marker = new GMarker(myPoint);{0}", Environment.NewLine);

            script.AppendFormat(CultureInfo.InvariantCulture, @"GEvent.addListener(marker, ""click"", function() {{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, @"marker.openInfoWindowHtml(index + "") "" + address.replace(""%%"", ""<br />""));{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}});{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "return marker;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
        }

        private void DisplayMap(StringBuilder script)
        {
            script.AppendFormat(CultureInfo.InvariantCulture, "function displayMap(){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var mapDiv = document.getElementById('{1}');{0}", Environment.NewLine, MapDiv.ClientID);
            script.AppendFormat(CultureInfo.InvariantCulture, "mapDiv.style.display = '';{0}", Environment.NewLine);
            //script.AppendFormat(CultureInfo.InvariantCulture, "document.location = '#map';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
        }

        private void HideMap(StringBuilder script)
        {
            script.AppendFormat(CultureInfo.InvariantCulture, "function hideMap(){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var mapDiv = document.getElementById('{1}');{0}", Environment.NewLine, MapDiv.ClientID);
            script.AppendFormat(CultureInfo.InvariantCulture, "mapDiv.style.display = 'none';{0}", Environment.NewLine);
            //script.AppendFormat(CultureInfo.InvariantCulture, "document.location = '#map';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
        }

        private void MapLocations(StringBuilder script)
        {
            //Show All Locations
            script.AppendFormat(CultureInfo.InvariantCulture, "function showAllLocations(){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var arr = new Array();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var marker;{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.clearOverlays();{0}", Environment.NewLine);
            int arrIndex = 0;
            if (RptLocations.DataSource != null)
            {
                foreach (DataRowView row in (DataView)RptLocations.DataSource)
                {
                    if (row.DataView.Table.Columns.Contains("Latitude") && row.DataView.Table.Columns.Contains("Longitude") && !row["Latitude"].Equals(DBNull.Value) && !row["Longitude"].Equals(DBNull.Value))
                    {
                        script.AppendFormat(CultureInfo.InvariantCulture, "marker = createLocationMarker({1}, {2}, {3}, '{4}<br/>{5}<br/>{6}');{0}", Environment.NewLine, row["Latitude"], row["Longitude"], arrIndex + 1, row["Name"].ToString().Replace("'", "\\'"), row["Address"].ToString().Replace("'", "\\'"), row["City"].ToString().Replace("'", "\\'"));
                        script.AppendFormat(CultureInfo.InvariantCulture, "var myPoint = marker.getPoint();{0}", Environment.NewLine);
                        script.AppendFormat(CultureInfo.InvariantCulture, "map.setCenter(myPoint, 13);{0}", Environment.NewLine);
                        script.AppendFormat(CultureInfo.InvariantCulture, "map.addOverlay(marker);{0}", Environment.NewLine);
                        script.AppendFormat(CultureInfo.InvariantCulture, "arr[{1}] = marker;{0}", Environment.NewLine, arrIndex++);
                        LocationCount++;
                    }
                }
            }
            script.AppendFormat(CultureInfo.InvariantCulture, "displayInfo();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "displayMap();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.checkResize();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var bounds = getBounds(map.getCurrentMapType(), map.getSize(), arr);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.setCenter(bounds.getCenter(),map.getBoundsZoomLevel(bounds) - 1);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "for (var i = 0; i < length; i++) {{{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "currentlyMappedLabels[i].className = 'hideCurrentlyMapped';{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
        }

        private void MapLocation(StringBuilder script)
        {
            script.AppendFormat(CultureInfo.InvariantCulture, "function showLocation(latitude, longitude, index, address, title, labelId){0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "{{ {0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.clearOverlays();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var marker = createLocationMarker(latitude, longitude, index + 1, title + '<br />' + address);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "var myPoint = marker.getPoint();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.setCenter(myPoint, 13);{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.addOverlay(marker);{0}", Environment.NewLine);
            if (LocatorMapLabel != null)
            {
                script.AppendFormat(CultureInfo.InvariantCulture, "var allLabel = document.getElementById('{1}');{0}",
                                    Environment.NewLine, LocatorMapLabel.ClientID);
                script.AppendFormat(CultureInfo.InvariantCulture, "allLabel.style.display = 'none';{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "var label = document.getElementById('locatorMapLabel');{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "label.style.display = '';{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "label.innerHTML = title + '<br />' + address.replace(\"%%\", \"<br />\");{0}", Environment.NewLine);
            }

            if (LocatorMapLabel != null)
            {
                script.AppendFormat(CultureInfo.InvariantCulture, "var scrollLabel = document.getElementById('{1}');{0}",
                                    Environment.NewLine, ScrollToViewMore.ClientID);
                script.AppendFormat(CultureInfo.InvariantCulture, "scrollLabel.style.display = '';{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "var mapLink = document.getElementById('lblMapLink');{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "mapLink.style.display = '';{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "var lnkDrivingDirections = document.getElementById('lnkDrivingDirections');{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "var mapLoc = 'http://maps.google.com/maps?f=q&hl=en&geocode=&q=' + address;{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "lnkDrivingDirections.href = mapLoc.replace(\"%%\", \" \");{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "var currentlyMappedLabel = document.getElementById(labelId);{0}", Environment.NewLine);
            }
            if (LocatorMapLabel != null)
            {
                script.AppendFormat(CultureInfo.InvariantCulture,
                                    "var currentlyMappedLabels = getElementsByClassName('currentlyMapped', 'span', document.getElementById('{1}'));{0}",
                                    Environment.NewLine, RptLocations.ClientID);
                script.AppendFormat(CultureInfo.InvariantCulture, "var length = currentlyMappedLabels.length;{0}", Environment.NewLine);
            }
            
            script.AppendFormat(CultureInfo.InvariantCulture, "displayMap();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.checkResize();{0}", Environment.NewLine);
            script.AppendFormat(CultureInfo.InvariantCulture, "map.setCenter(myPoint, 13);{0}", Environment.NewLine);
            if (LocatorMapLabel != null)
            {
                script.AppendFormat(CultureInfo.InvariantCulture, "for (var i = 0; i < length; i++) {{{0}",
                                    Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture,
                                    "currentlyMappedLabels[i].className = 'hideCurrentlyMapped';{0}",
                                    Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
                script.AppendFormat(CultureInfo.InvariantCulture,
                                    "currentlyMappedLabel.className = 'currentlyMapped';{0}", Environment.NewLine);
            }
            script.AppendFormat(CultureInfo.InvariantCulture, "}}{0}", Environment.NewLine);
        }

        private void MapType(StringBuilder script, string mapType)
        {
            if (mapType == "Normal") 
                script.AppendFormat(CultureInfo.InvariantCulture, "map.setMapType(G_NORMAL_MAP);{0}", Environment.NewLine);
            else if (mapType == "Satellite")
                script.AppendFormat(CultureInfo.InvariantCulture, "map.setMapType(G_SATELLITE_MAP);{0}", Environment.NewLine);
            else if (mapType == "Hybrid")
                script.AppendFormat(CultureInfo.InvariantCulture, "map.setMapType(G_HYBRID_MAP);{0}", Environment.NewLine);
        }



        #endregion

        #region GetMapUrl

        public override string GetMapUrl(string apiKey)
        {
            if (String.IsNullOrEmpty("apiKey")) {throw new ArgumentNullException("apiKey", "ApiKey is Invalid");}
            return "http://maps.google.com/maps?file=api&v=2&key=" + apiKey;
        }

        public override bool IsKeyValid(string apiKey)
        {
            return (apiKey != null && apiKey.Length == 86);
        }

        #endregion
    }
}