///<reference name="MicrosoftAjax.js" />
///<reference name="jQuery.1.2.6.js" />
google.load('maps', '2');
google.load('jquery', '1.2');

Type.registerNamespace('Engage.Dnn.Locator');

Engage.Dnn.Locator.GoogleMap = function(mapDiv) {
    Engage.Dnn.Locator.GoogleMap.initializeBase(this, [mapDiv]);
};

Engage.Dnn.Locator.GoogleMap.prototype = {
    initialize: function() {
        Engage.Dnn.Locator.GoogleMap.callBaseMethod(this, "initialize");

        this._map = new GMap2(this.get_element());
        this._map.addControl(new GSmallMapControl());
        this._map.addControl(new GMapTypeControl());

        // setCenter _must_ be called before any other operations, to initialize the map.
        this._map.setCenter(new GLatLng(0, 0), Engage.Dnn.Locator.GoogleMap.defaultZoomLevel, this._mapType);

        var self = this;
        jQuery('div.viewMapBt a')
			.each(function() {
			    var parentRow = jQuery(this).parents('tr');
			    var locationPoint = self._locationsArray[Engage.Dnn.Locator.BaseMap.getRowIndex(parentRow)].getLatLng();
			    if (!locationPoint || (!locationPoint.lat() && !locationPoint.lng())) {
			        this.hide();
			    }
			})
			.filter('a:visible')
			.click(function(event) {
			    event.preventDefault();

			    var parentRow = jQuery(this).parents('tr');
			    self.showLocation(self._locationsArray[Engage.Dnn.Locator.BaseMap.getRowIndex(parentRow)], jQuery('td.mdDistance span.hideCurrentlyMapped, td.mdDistance span.currentlyMapped', parentRow));
			});
    },
    showLocation: function(marker, locationSpan) {
        this._map.clearOverlays();
        this._map.addOverlay(marker);

        this._noLocationSpan.hide();
        this._currentLocationSpan.show().html(marker.engageLocatorTitle + Engage.Dnn.Locator.BaseMap.htmlBreak + marker.engageLocatorAddress);

        this._instructionSpan.show();
        this._directionsSection.show();
        this._directionsLink.show().attr('href', 'http://maps.google.com/maps?f=q&hl=en&geocode=&q=' + marker.engageLocatorAddress.replace(Engage.Dnn.Locator.BaseMap.htmlBreak, ', '));

        Engage.Dnn.Locator.BaseMap.hideCurrentlyMappedLabels();
        locationSpan.removeClass('hideCurrentlyMapped').addClass('currentlyMapped');

        this.displayMap();
        this._map.checkResize();
        this._map.setCenter(marker.getPoint(), Engage.Dnn.Locator.GoogleMap.defaultZoomLevel);
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

        Engage.Dnn.Locator.BaseMap.hideCurrentlyMappedLabels();
    },
    getNewMarker: function(latitude, longitude, index, title, address) {
        var marker = new GMarker(new GLatLng(latitude, longitude));
        GEvent.addListener(marker, "click", function() {
            marker.openInfoWindowHtml((index + 1).toString(10) + ") " + title + Engage.Dnn.Locator.BaseMap.htmlBreak + address);
        });
        return marker;
    },
    dispose: function() {
        Engage.Dnn.Locator.GoogleMap.callBaseMethod(this, "dispose");
    }
};

Engage.Dnn.Locator.GoogleMap.defaultZoomLevel = 13;

Engage.Dnn.Locator.GoogleMap.registerClass("Engage.Dnn.Locator.GoogleMap", Engage.Dnn.Locator.BaseMap);

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