var createTaxiStand = function () {
var markers = [];
    function updateUI(lat,lng, formattedAddress) {
        $('#Lattitude').val(lat);
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
        getInputAddress('#CenterLattitude', '#CenterLongitude', '#Alias')

    });
    return {
        updateUI: updateUI
    }

}();

function mapClicked(e) {
    gATMap.map.removeMarkers();
    gATMap.map.setCenter(e.latLng.lat(), e.latLng.lng());
    gATMap.getAddress(e.latLng.lat(), e.latLng.lng(), createTaxiStand.updateUI);
}

function zoomChanged(map) {

}