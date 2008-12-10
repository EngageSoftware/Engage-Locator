///<reference name="MicrosoftAjax.js" />
///<reference name="jQuery.1.2.6.js" />
google.load('maps', '2');
google.load('jquery', '1.2'); 

Type.registerNamespace('Engage.Dnn.Locator');

Engage.Dnn.Locator.GoogleMap = function(mapDiv) {
    Engage.Dnn.Locator.GoogleMap.initializeBase(this, [mapDiv]);
    this._mapDiv = jQuery(mapDiv);
    this._currentLocationSpan = this._noLocationSpan = this._instructionSpan = this._directionsLink = this._directionsSection = this._locationsArray = this._mapType = this._map = null;
};

Engage.Dnn.Locator.GoogleMap.prototype = {
    initialize: function() {
        Engage.Dnn.Locator.GoogleMap.callBaseMethod(this, "initialize");

        this._map = new GMap2(this.get_element());
        this._map.addControl(new GSmallMapControl());
        this._map.addControl(new GMapTypeControl());
        
        // setCenter _must_ be called before any other operations, to initialize the map.
        this._map.setCenter(new GLatLng(0, 0), Engage.Dnn.Locator.GoogleMap.defaultZoomLevel, this._mapType);

        for (var i = 0; i < this._locationsArray.length; i++) {
            var locationMarker = Engage.Dnn.Locator.GoogleMap.createLocationMarker(this._locationsArray[i].Latitude, this._locationsArray[i].Longitude, i);
            this._locationsArray[i] = locationMarker;
        }

        var self = this;
        jQuery('div.viewMapBt a')
			.each(function() {
			    var parentRow = jQuery(this).parents('tr');
			    var locationPoint = self._locationsArray[Engage.Dnn.Locator.GoogleMap.getRowIndex(parentRow)].getLatLng();
			    if (!locationPoint || (!locationPoint.lat() && !locaitonPoint.lng())) {
			        this.hide();
			    }
			})
			.filter('a:visible')
			.click(function(event) {
			    event.preventDefault();

			    var parentRow = jQuery(this).parents('tr');
			    self.showLocation(self._locationsArray[Engage.Dnn.Locator.GoogleMap.getRowIndex(parentRow)], jQuery('td.mdDistance span.hideCurrentlyMapped, td.mdDistance span.currentlyMapped', parentRow));
			});
    },
    displayMap: function() {
        this._mapDiv.show();
    },
    hideMap: function() {
        this._mapDiv.hide();
    },
    showLocation: function(marker, locationSpan) {
        this._map.clearOverlays();
        this._map.addOverlay(marker);

        this._noLocationSpan.hide();
        this._currentLocationSpan.show().html(marker.engageLocatorTitle + Engage.Dnn.Locator.GoogleMap.htmlBreak + marker.engageLocatorAddress);

        this._instructionSpan.show();
        this._directionsSection.show();
        this._directionsLink.show().attr('href', 'http://maps.google.com/maps?f=q&hl=en&geocode=&q=' + marker.engageLocatorAddress.replace(Engage.Dnn.Locator.GoogleMap.htmlBreak, ', '));

        Engage.Dnn.Locator.GoogleMap.hideCurrentlyMappedLabels();
        locationSpan.removeClass('hideCurrentlyMapped').addClass('currentlyMapped');

        this.displayMap();
        this._map.checkResize();
        this._map.setCenter(marker.getPoint(), Engage.Dnn.Locator.GoogleMap.defaultZoomLevel);
    },
    displayInfo: function() {
        this._noLocationSpan.show();
        this._currentLocationSpan.show();
        this._instructionSpan.show();
        this._directionsSection.show();
    },
    showAllLocations: function() {
        this._map.clearOverlays();

        for (var i = 0; i < this._locationsArray.length; i++) {
            this._map.addOverlay(this._locationsArray[i]);
        }

        this.displayInfo();
        this.displayMap();
        this._map.checkResize();
        var bounds = Engage.Dnn.Locator.GoogleMap.getBounds(this._map.getCurrentMapType(), this._map.getSize(), this._locationsArray);
        this._map.setCenter(bounds.getCenter(), this._map.getBoundsZoomLevel(bounds) - 1);

        Engage.Dnn.Locator.GoogleMap.hideCurrentlyMappedLabels();
    },
    dispose: function() {
        Engage.Dnn.Locator.GoogleMap.callBaseMethod(this, "dispose");
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

Engage.Dnn.Locator.GoogleMap.htmlBreak = "<br />";
Engage.Dnn.Locator.GoogleMap.defaultZoomLevel = 13;

Engage.Dnn.Locator.GoogleMap.registerClass("Engage.Dnn.Locator.GoogleMap", Sys.UI.Behavior);

Engage.Dnn.Locator.GoogleMap.createLocationMarker = function(latitude, longitude, index) {
    var myPoint = new GLatLng(latitude, longitude);
    var marker = new GMarker(myPoint);

    var parentRow = jQuery('tr.locationEntryTR:eq(' + index + ')');
    var address = jQuery('.locatorAddress', parentRow).text() + Engage.Dnn.Locator.GoogleMap.htmlBreak + jQuery('.locatorCity', parentRow).text() + ' ' + jQuery('.locatorState', parentRow).text() + ' ' + jQuery('.locatorPostalCode', parentRow).text();
    var title = jQuery('.locatorName', parentRow).text();

    GEvent.addListener(marker, "click", function() {
        marker.openInfoWindowHtml((index + 1).toString(10) + ") " + address.replace("%%", Engage.Dnn.Locator.GoogleMap.htmlBreak));
    });

    marker.engageLocatorAddress = address;
    marker.engageLocatorTitle = title;
    return marker;
};

Engage.Dnn.Locator.GoogleMap.getBounds = function(mapType, size, markers) {
    var south, west, east, north;
    south = west = 180;
    east = north = -180;
    for (var i = 0; i < markers.length; i++) { 
		if (markers[i].getPoint().lat() > north) {
            north = markers[i].getPoint().lat();
        }
        if (markers[i].getPoint().lat() < south) {
            south = markers[i].getPoint().lat();
        }
        if (markers[i].getPoint().lng() > east) {
            east = markers[i].getPoint().lng();
        }
        if (markers[i].getPoint().lng() < west) {
            west = markers[i].getPoint().lng();
        }
    }
	
    var southWest = new GLatLng(south, west);
    var northEast = new GLatLng(north, east);
    var bounds = new GLatLngBounds(southWest, northEast);
    var zoomLevel = mapType.getBoundsZoomLevel(bounds, size);
    var minZoom = mapType.getMinimumResolution(markers[0]);
    if (zoomLevel < minZoom) {
        zoomLevel = minZoom;
    }
	
    return bounds;
};

Engage.Dnn.Locator.GoogleMap.createCenterMarker = function(latitude, longitude) {
    var myPoint = new GLatLng(latitude, longitude);
    var myImage = new GIcon();
    myImage.image = "/images/ratingminus.gif";
    myImage.iconSize = new GSize(18, 18);
    myImage.iconAnchor = new GPoint(0, 0);
    return new GMarker(myPoint, myImage);
};

Engage.Dnn.Locator.GoogleMap.hideCurrentlyMappedLabels = function() {
	jQuery('td.mdDistance span.currentlyMapped').removeClass('currentlyMapped').addClass('hideCurrentlyMapped');
}

Engage.Dnn.Locator.GoogleMap.getRowIndex = function(row) {
    var rowNumberText = jQuery("td.mdLocationNumber span", row).text();
    return parseInt(rowNumberText.substring(0, rowNumberText.length - 1), 10) - 1;
}