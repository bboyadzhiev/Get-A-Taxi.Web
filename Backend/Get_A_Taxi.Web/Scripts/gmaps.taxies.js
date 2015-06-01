// TAXIES
var taxies = function (districtId, operatorId) {
    var availableMarkers = [];
    var busyMarkers = [];
    var taxiesHub;

    function updateTaxiMarker(data) {
        if (data.isAvailable == true) {
            if (data.onDuty) {
                console.log("Available");
                gATMap.addMarker(data.taxiId, data.lat, data.lon, '/Content/Images/Map/taxi_available.png', data.plate + ' ' + data.phone, clkW, availableMarkers);
            } else {
                console.log("Unassigned");
            //    gATMap.removeMarker(data.taxiId, availableMarkers);
            }
        } else {
            if (data.onDuty) {
                console.log("Busy");
                gATMap.addMarker(data.taxiId, data.lat, data.lon, '/Content/Images/Map/taxi_busy.png', data.plate + ' ' + data.phone, clkP, busyMarkers);
            } else {
                console.log("OffDuty");
              //  gATMap.removeMarker(data.taxiId, busyMarkers);
            }
        }
    }

    function addTaxiesMarkers(data) {
        for (var i = 0; i < data.length; i++) {
            updateTaxiMarker(data[i]);
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
        console.log('Taxi ' + data.taxiId + ' on duty!');
        updateTaxiMarker(data);
    }

    taxiesHub.client.taxiOffDuty = function (taxiId) {
        console.log('Taxi ' + taxiId + ' is off duty!');
        var markerToRemove = gATMap.removeMarker(taxiId, availableMarkers);
        var busy = gATMap.removeMarker(taxiId, busyMarkers);
    }

    taxiesHub.client.taxiUpdated = function (data) {
        console.log('Taxi ' + data.taxiId + ' updated!');
        var busy = gATMap.removeMarker(data.taxiId, busyMarkers);
        var available = gATMap.removeMarker(data.taxiId, availableMarkers);
        if (busy != null) {
            console.log("Updating busy");
        }
        if (available != null) {
            console.log("Updating available");
        }
        updateTaxiMarker(data);
       
    }

    $.connection.hub.start().done(function () {
        console.log("Connecting to Taxies Hub...");
        taxiesHub.invoke("open", districtId);
    });

    $(window).unload(function () {
        taxiesHub.invoke("close", districtId);
    });

    return {
        availableMarkers: availableMarkers,
        busyMarkers: busyMarkers
    }

}(district, operator);