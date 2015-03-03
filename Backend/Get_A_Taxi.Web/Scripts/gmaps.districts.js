var districtMap = function () {
    var districtMarkers = [];
    function updateUI(latlng, formattedAddress) {
        $('#CenterLattitude').val(latlng.lat());
        $('#CenterLongitude').val(latlng.lng());
        $('#address').val(formattedAddress);
        $('#Title').val(formattedAddress);
        gATMap.addMarker(0, latlng.lat(), latlng.lng(), "/Content/Images/Map/orange_m.png", formattedAddress, clb, districtMarkers);
        $('#MapZoom').val(gATMap.map.zoom);
    }

    function clb(markerId, lat, lng, content) {
        // alert(content);
    }

    $("#address").keyup(function (event) {
        if (event.keyCode == 13) {
            gATMap.getCoordinates($('#address').val().trim(), updateUI);
        }
    });

    function getInputAddress(latInput, lngInput, updateUI) {
        gATMap.getAddress($(latInput).val(), $(lngInput).val(), updateUI)
    }

    $('#getAddress').click(function (e) {
        gATMap.getAddress($('#CenterLattitude').val(), $('#CenterLongitude').val(), updateUI)
    });

    return {
        updateUI: updateUI,
    }

}();

$('#geocoding_form').submit(function (e) {
    e.preventDefault();
    gATMap.getCoordinates($('#address').val().trim(), createDistrict.updateUI);
});

function mapClicked(e) {
    gATMap.map.removeMarkers();
    gATMap.map.setCenter(e.latLng.lat(), e.latLng.lng());
    gATMap.getAddress(e.latLng.lat(), e.latLng.lng(), createDistrict.updateUI);
}

function zoomChanged(map) {
    $('#MapZoom').val(map.zoom);
}

