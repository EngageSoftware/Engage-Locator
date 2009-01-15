///<reference name="MicrosoftAjax.js" />
///<reference name="jQuery.1.2.6.js" />
Type.registerNamespace('Engage.Dnn.Locator');
Engage.Dnn.Locator.YahooMap = function(mapDiv) {
    Engage.Dnn.Locator.YahooMap.initializeBase(this, [mapDiv]);};
    Engage.Dnn.Locator.YahooMap.prototype = {
        initialize: function() {
            Engage.Dnn.Locator.YahooMap.callBaseMethod(this, "initialize");
            this._map = new YMap(this.get_element(), this._mapType);
            this._map.addTypeControl();
            this._map.addPanControl();
            this._map.addZoomLong();

            var self = this;
            jQuery('div.viewMapBt a').each(function() {
                var parentRow = jQuery(this).parents('tr.locationEntryTR');
                var locationPoint = self._locationsArray[Engage.Dnn.Locator.BaseMap.getRowIndex(parentRow)].YGeoPoint;
                if (!locationPoint || (!locationPoint.Lat && !locationPoint.Lon)) {
                    this.hide();
                }
            }).filter('a:visible').click(function(event) {
                event.preventDefault();
                var parentRow = jQuery(this).parents('tr.locationEntryTR');
                self.showLocation(self._locationsArray[Engage.Dnn.Locator.BaseMap.getRowIndex(parentRow)], jQuery('td.mdDistance span.hideCurrentlyMapped, td.mdDistance span.currentlyMapped', parentRow));
            });
        },
        showLocation: function(marker, locationSpan) {
            this.displayMap();
            this._map.removeMarkersAll();
            this._map.drawZoomAndCenter(marker.YGeoPoint, Engage.Dnn.Locator.YahooMap.defaultZoomLevel);
            this._map.addOverlay(this.cloneMarker(marker));
            this._noLocationSpan.hide();
            this._currentLocationSpan.show().html(marker.engageLocatorTitle + Engage.Dnn.Locator.BaseMap.htmlBreak + marker.engageLocatorAddress);
            this._instructionSpan.show();
            this._directionsSection.show();
            this._directionsLink.show().attr('href', 'http://maps.yahoo.com/broadband#q1=' + marker.engageLocatorAddress.replace(Engage.Dnn.Locator.BaseMap.htmlBreak, ', '));
            Engage.Dnn.Locator.BaseMap.hideCurrentlyMappedLabels();
            locationSpan.removeClass('hideCurrentlyMapped').addClass('currentlyMapped');
        },
        showAllLocations: function() {
            this.displayMap();
            this._map.removeMarkersAll();
            for (var i = 0; i < this._locationsArray.length; i++) {
                var marker = this._locationsArray[i];
                this._map.addOverlay(this.cloneMarker(marker));
            }
            var pointsArray = jQuery.map(this._locationsArray, function(marker) { return marker.YGeoPoint; });
            var bestZoom = this._map.getBestZoomAndCenter(pointsArray);
            this._map.drawZoomAndCenter(bestZoom.YGeoPoint, bestZoom.zoomLevel);
            this.displayInfo();
            Engage.Dnn.Locator.BaseMap.hideCurrentlyMappedLabels();
        },
        cloneMarker: function(marker) {
            return this.createLocationMarker(marker.YGeoPoint.Lat, marker.YGeoPoint.Lon, marker.engageLocatorIndex);
        },
        getNewMarker: function(latitude, longitude, index, title, address) {
            var marker = new YMarker(new YGeoPoint(latitude, longitude));
            marker.addLabel(index + 1);
            marker.addAutoExpand(title + Engage.Dnn.Locator.BaseMap.htmlBreak + address);
            return marker;
        },
        dispose: function() {
            Engage.Dnn.Locator.YahooMap.callBaseMethod(this, "dispose");
        }
    };
Engage.Dnn.Locator.YahooMap.defaultZoomLevel = 6;

Engage.Dnn.Locator.YahooMap.registerClass("Engage.Dnn.Locator.YahooMap", Engage.Dnn.Locator.BaseMap);