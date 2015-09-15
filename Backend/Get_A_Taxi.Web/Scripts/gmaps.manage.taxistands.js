
var taxiStandsManagement = function () {
var markers = [];
    function updateUI(lat,lng, formattedAddress) {
        $('#Latitude').val(lat);
        $('#Longitude').val(lng);
        $('#Address').val(formattedAddress);
        $('#Alias').val(formattedAddress);
        gATMap.addMarker(0, lat, lng, "/Content/Images/Map/purple_m.png", formattedAddress, clb, markers);
    }

    function clb(markerId, lat, lng, content) {
        // alert(content);
    }

    $("#Address").keyup(function (event) {
        if (event.keyCode == 13) {
            gATMap.getCoordinates($('#Address').val().trim(), updateUI);
        }
    });

    function getInputAddress(latInput, lngInput, updateUI) {
        gATMap.getAddress($(latInput).val(), $(lngInput).val(), updateUI)
    }

    $('#getAddress').click(function (e) {
        getInputAddress('#CenterLatitude', '#CenterLongitude', '#Alias')

    });
    return {
        updateUI: updateUI
    }

}();

gATMap.initMap('#map', lat, lon, zoomChanged, mapClicked);

function mapClicked(e) {
    gATMap.clearMarkers();
    gATMap.setCenter(e.latLng.lat(), e.latLng.lng());
    gATMap.getAddress(e.latLng.lat(), e.latLng.lng(), taxiStandsManagement.updateUI);
}

function zoomChanged(map) {

}