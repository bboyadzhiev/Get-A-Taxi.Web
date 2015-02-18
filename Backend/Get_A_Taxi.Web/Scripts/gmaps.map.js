var markers = [];

$(document).ready(function () {
    map = new GMaps({
        div: '#map',
        lat: defaultLat,
        lng: defaultLon,
        zoom_changed: function (map) {
            zoomChanged(map);
        },
        click: function (e) {
            mapClicked(e);
        }
    });

});

function getAddress(latVal, lngVal, getAddressCallback) {
    GMaps.geocode({
        lat: latVal,
        lng: lngVal,
        callback: function (results, status) {
            if (status == 'OK') {
                var latlng = results[0].geometry.location;
                var formattedAddress = results[0].formatted_address;
                getAddressCallback(latlng, formattedAddress);
            }
        }
    });
}

function getCoordinates(address, getCoordinatesCallback) {
    GMaps.geocode({
        address: address,
        callback: function (results, status) {
            if (status == 'OK') {
                //console.log(results[0]);
                var latlng = results[0].geometry.location;
                var formattedAddress = results[0].formatted_address;
                map.setCenter(latlng.lat(), latlng.lng());
                getCoordinatesCallback(latlng, formattedAddress);
            } else {
                alert("Address could not be found!");
            }
        }
    });
}

function addMarker(markerId, latlng, iconLink, content, clickCallback, arrayReference) {
    var newMarker = map.addMarker({
        id: markerId,
        lat: latlng.lat(),
        lng: latlng.lng(),
        icon: iconLink,
        infoWindow: {
            content: '<p>' + content + '</p>'
        },
        click: function () {
            clickCallback(markerId, latlng, content);
        },
    });
    
    arrayReference.push(newMarker);
    return newMarker;
}
