
gATMap.initMap('#map', lat, lon, zoomChanged, mapClicked);
var districtsManagement = function () {
    var districtMarkers = [];
    var lastZoom;
    function updateUI(lat, lng, formattedAddress) {
        $('#CenterLatitude').val(lat);
        $('#CenterLongitude').val(lng);
        $('#address').val(formattedAddress);
        $('#Title').val(formattedAddress);
        gATMap.addMarker(0, lat, lng, "/Content/Images/Map/orange_m.png", formattedAddress, clb, districtMarkers);
        console.log(lastZoom);
        if (lastZoom == null) {
            $('#MapZoom').val(gATMap.initialZoom());
        } else {
            $('#MapZoom').val(lastZoom);
        }
        
    }

    function setLastZoom(zoomValue){
        lastZoom = zoomValue;
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
        gATMap.getAddress($('#CenterLatitude').val(), $('#CenterLongitude').val(), updateUI)
    });

    return {
        updateUI: updateUI,
        setLastZoom: setLastZoom
    }

}();

$('#geocoding_form').submit(function (e) {
    e.preventDefault();
    gATMap.getCoordinates($('#address').val().trim(), districtsManagement.updateUI);
});

function mapClicked(e) {
    gATMap.clearMarkers();
    gATMap.setCenter(e.latLng.lat(), e.latLng.lng());
    gATMap.getAddress(e.latLng.lat(), e.latLng.lng(), districtsManagement.updateUI);
    
}
function zoomChanged(map) {
    districtsManagement.setLastZoom(map.zoom);
    $('#MapZoom').val(map.zoom);
}

