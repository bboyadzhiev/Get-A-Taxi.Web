var markers = [];

$(document).ready(function () {
    map = new GMaps({
        div: '#map',
        lat: defaultLat,
        lng: defaultLon,
        zoom_changed: function (map) {
            $('#MapZoom').val(map.getZoom());
        },
        click: function (e) {
            mapClicked(e);
        }
    });

    $('#geocoding_form').submit(function (e) {
        e.preventDefault();
        GMaps.geocode({
            address: $('#address').val().trim(),
            callback: function (results, status) {
                if (status == 'OK') {
                    var latlng = results[0].geometry.location;
                    map.setCenter(latlng.lat(), latlng.lng());
                    geocodeComplete(latlng, map.getZoom());
                }
            }
        });

    });

    $('#getAddress').click(function (e) {
        GMaps.geocode({
            lat: $('#CenterLattitude').val(),
            lng: $('#CenterLongitude').val(),
            callback: function (results, status) {
                if (status == 'OK') {
                    var address = results[0].formatted_address;
                    geoDecodeComplete(address);
                } 
            }
        });
    });
});