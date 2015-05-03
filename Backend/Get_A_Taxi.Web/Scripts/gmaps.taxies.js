// TAXIES
var taxies = function (districtId, operatorId) {
    var availableMarkers = [];
    var busyMarkers = [];
    var taxiesHub;

    function addNewTaxiMarker(data) {
        if (data.isAvailable == true) {
            gATMap.addMarker(data.taxiId, data.lat, data.lon, '/Content/Images/Map/taxi_available.png', data.plate + ' ' + data.phone, clkW, availableMarkers);
        } else {
            gATMap.addMarker(data.taxiId, data.lat, data.lon, '/Content/Images/Map/taxi_busy.png', data.plate + ' ' + data.phone, clkP, busyMarkers);
        }
    }

    function addTaxiesMarkers(data) {
        for (var i = 0; i < data.length; i++) {
            addNewTaxiMarker(data[i]);
        }
    }

    function clkW(markerId, lat, lng, content) {

    }

    function clkP(markerId, lat, lng, content) {

    }

    // Taxies hub
    taxiesHub = $.connection.taxiesHub;
    console.log(taxiesHub);

    taxiesHub.client.populateTaxies = function (data) {
        console.log('Taxies Hub Connected!')
        console.log(data);
        addTaxiesMarkers(data);
    };

    taxiesHub.client.taxiOnDuty = function (data) {
        console.log('Taxi ' + taxiId + ' on duty!');
        addNewTaxiMarker(data);
    }

    taxiesHub.client.taxiOffDuty = function (taxiId) {
        console.log('Taxi ' + taxiId + ' is off duty!');
        var markerToRemove = gATMap.removeMarker(taxiId, availableMarkers);
    }

    taxiesHub.client.taxiUpdated = function (data) {
        console.log('Taxi ' + data.taxiId + ' updated!');

        var taxiMarkerToUpdate = gATMap.removeMarker(data.taxiId, availableMarkers);
        taxiMarkerToUpdate.lat = data.lat;
        taxiMarkerToUpdate.lon = data.lon;
        addNewTaxiMarker(taxiMarkerToUpdate);
    }

    $.connection.hub.start().done(function () {
        console.log("Connecting to Taxies Hub...");
        taxiesHub.invoke("open", districtId);
    });

    $(window).unload(function () {
        taxiesHub.invoke("close", districtId);
    });
}(district, operator);