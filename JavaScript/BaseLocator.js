///<reference name="MicrosoftAjax.js" />
///<reference name="jQuery.1.2.6.js" />

Type.registerNamespace('Engage.Dnn.Locator');

Engage.Dnn.Locator.BaseMap = function(mapDiv) {
    Engage.Dnn.Locator.BaseMap.initializeBase(this, [mapDiv]);
    this._mapDiv = jQuery(mapDiv);
    this._currentLocationSpan = this._noLocationSpan = this._instructionSpan = this._directionsLink = this._directionsSection = this._locationsArray = this._mapType = this._map = null;
};
Engage.Dnn.Locator.BaseMap.prototype = {
    initialize: function() {
        Engage.Dnn.Locator.BaseMap.callBaseMethod(this, "initialize");
		
        for (var i = 0; i < this._locationsArray.length; i++) {
            var locationMarker = this.createLocationMarker(this._locationsArray[i].Latitude, this._locationsArray[i].Longitude, i);
            this._locationsArray[i] = locationMarker;
        }
    },
    displayMap: function() {
        this._mapDiv.show();
		document.location = '#map';
    },
    hideMap: function() {
        this._mapDiv.hide();
    },
	displayInfo: function() {
        this._noLocationSpan.show();
        this._currentLocationSpan.hide();
        this._instructionSpan.hide();
        this._directionsSection.hide();
    },
	/*
	// TODO: convert methods into template methods
	showLocation: function(marker, locationSpan) 
	showAllLocations: function()
	*/
	createLocationMarker: function(latitude, longitude, index) {
	    var parentRow = jQuery('tr.locationEntryTR:eq(' + index + ')');
	    var address = jQuery('.locatorAddress', parentRow).text() + Engage.Dnn.Locator.BaseMap.htmlBreak + jQuery('.locatorCity', parentRow).text() + ' ' + jQuery('.locatorState', parentRow).text() + ' ' + jQuery('.locatorPostalCode', parentRow).text();
	    var title = jQuery('.locatorName', parentRow).text();

	    var marker = this.getNewMarker(latitude, longitude, index, title, address);
	    marker.engageLocatorIndex = index;
	    marker.engageLocatorAddress = address;
	    marker.engageLocatorTitle = title;
	    return marker;
	},
    dispose: function() {
        Engage.Dnn.Locator.BaseMap.callBaseMethod(this, "dispose");
    },
    get_currentLocationSpan: function() {
        return this._currentLocationSpan;
    },
    set_currentLocationSpan: function(value) {
        this._currentLocationSpan = value;
    },
    get_noLocationSpan: function() {
        return this._noLocationSpan;
    },
    set_noLocationSpan: function(value) {
        this._noLocationSpan = value;
    },
    get_instructionSpan: function() {
        return this._instructionSpan;
    },
    set_instructionSpan: function(value) {
        this._instructionSpan = value;
    },
    get_directionsLink: function() {
        return this._directionsLink;
    },
    set_directionsLink: function(value) {
        this._directionsLink = value;
    },
    get_directionsSection: function() {
        return this._directionsSection;
    },
    set_directionsSection: function(value) {
        this._directionsSection = value;
    },
    get_mapType: function() {
        return this._mapType;
    },
    set_mapType: function(value) {
        this._mapType = value;
    },
    get_locationsArray: function() {
        return this._locationsArray;
    },
    set_locationsArray: function(value) {
        this._locationsArray = value;
    }
};
Engage.Dnn.Locator.BaseMap.htmlBreak = "<br />";
Engage.Dnn.Locator.BaseMap.registerClass("Engage.Dnn.Locator.BaseMap", Sys.UI.Behavior);

Engage.Dnn.Locator.BaseMap.hideCurrentlyMappedLabels = function() {
    jQuery('td.mdDistance span.currentlyMapped').removeClass('currentlyMapped').addClass('hideCurrentlyMapped');
};
Engage.Dnn.Locator.BaseMap.getRowIndex = function(row) {
    var rowNumberText = jQuery("td.mdLocationNumber span", row).text();
    return parseInt(rowNumberText.substring(0, rowNumberText.length - 1), 10) - 1;
};