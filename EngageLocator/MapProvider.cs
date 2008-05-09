using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;


namespace Engage.Dnn.Locator
{
    public abstract class MapProvider
    {
        private int _locationCount;
        private HtmlGenericControl _mapDiv;
        private WebControl _locatorMapLabel;
        private WebControl _scrollToViewMore;
        private Repeater _rptLocations;
        private string _searchCriteria;
        private Pair _latLong;

        public abstract string GetMapUrl(string apiKey);
        public abstract bool IsKeyValid(string apiKey);
        public abstract string GenerateMapScriptCore(string mapType);
        public abstract string GenerateMiniMapScriptCore();
        public string GenerateMapScript(string mapType)
        {
            Validate();
            return GenerateMapScriptCore(mapType);            
        }

        public string GenerateMiniMapScript()
        {
            return GenerateMiniMapScriptCore();
        }

        protected void Validate()
        {
            if (MapDiv == null){throw new InvalidOperationException("Map Div is not defined");}
            if (LocatorMapLabel == null) { throw new InvalidOperationException("Locator Map is not defined"); }
            if (RptLocations == null) { throw new InvalidOperationException("Location Repeater is not defined"); }
            if (ScrollToViewMore == null) { throw new InvalidOperationException("Scroll Label is not defined"); }
        }

        public static MapProvider CreateInstance(MapProviderType mapType)
        {
            return CreateInstance(mapType.ClassName);
        }

        public static MapProvider CreateInstance(string className)
        {
            Type objectType = Type.GetType(className, true, true);
            MapProvider mp = (MapProvider)Activator.CreateInstance(objectType);
            return mp;
        }

        #region Properties

        public int LocationCount
        {
            get { return _locationCount; }
            protected set { _locationCount = value; }
        }

        public HtmlGenericControl MapDiv
        {
            set { _mapDiv = value; }
            get { return _mapDiv; }
        }
        public WebControl LocatorMapLabel
        {
            set { _locatorMapLabel = value; }
            get { return _locatorMapLabel; }
        }

        public WebControl ScrollToViewMore
        {
            set { _scrollToViewMore = value; }
            get { return _scrollToViewMore; }
        }

        public Repeater RptLocations
        {
            set { _rptLocations = value; }
            get { return _rptLocations; }
        }

        public string SearchCriteria
        {
            set { _searchCriteria = value; }
            get { return _searchCriteria; }
        }

        public Pair LatLong
        {
            set { _latLong = value; }
            get { return _latLong; }
        }

        #endregion

        #region

        public abstract string MapProviderUrl { get; }

        #endregion

    }

    
}