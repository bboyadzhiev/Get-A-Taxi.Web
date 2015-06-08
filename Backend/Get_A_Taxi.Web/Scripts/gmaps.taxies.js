// TAXIES
var taxies = function (districtId, operatorId) {
    var availableMarkers = [];
    var busyMarkers = [];
    var taxiesHub;

    function updateTaxiMarker(data) {
        var removed = gATMap.removeMarker(data.taxiId, busyMarkers);
        if(removed == null){
            removed = gATMap.removeMarker(data.taxiId, availableMarkers);
        }
        
        if (removed != null && data.plate == null) {
            data.plate = removed.plate;
            data.phone = removed.phone;
           
        }
        if (data.status == 0) { // Available
            gATMap.addMarker(data.taxiId, data.lat, data.lon, '/Content/Images/Map/taxi_available.png', data.plate + ' ' + data.phone, clkW, availableMarkers);
        }
        if (data.status == 1) { // Busy
            gATMap.addMarker(data.taxiId, data.lat, data.lon, '/Content/Images/Map/taxi_busy.png', data.plate + ' ' + data.phone, clkP, busyMarkers);
        }
        console.log(data.plate);
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
        //console.log(data);
        addTaxiesMarkers(data);
    };

    taxiesHub.client.taxiOnDuty = function (data) {
        console.log('Taxi ' + data.taxiId + ' on duty!');
        updateTaxiMarker(data);
    }

    taxiesHub.client.taxiOffDuty = function (taxiId) {
        console.log('Taxi ' + taxiId + ' is off duty!');
        var markerToRemove = gATMap.removeMarker(taxiId, availableMarkers);
        if (markerToRemove == null) {
            markerToRemove = gATMap.removeMarker(taxiId, busyMarkers);
        }
    }

    taxiesHub.client.taxiUpdated = function (data) {
        console.log('Taxi ' + data.taxiId + ' updated!');
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