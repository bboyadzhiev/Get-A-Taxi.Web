
var gATMap = (function () {

    var map = new GMaps({
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
    function getAddress(latVal, lngVal, getAddressCallback) {
        GMaps.geocode({
            lat: latVal,
            lng: lngVal,
            callback: function (results, status) {
                if (status == 'OK') {
                   // var latlng = results[0].geometry.location;
                    var lat = latVal;
                    var lng = lngVal;
                    var formattedAddress = results[0].formatted_address;
                    map.setCenter(lat, lng);
                    getAddressCallback(lat, lng, formattedAddress);
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
                    getCoordinatesCallback(latlng.lat(), latlng.lng(), formattedAddress);
                } else {
                    alert("Address could not be found!");
                }
            }
        });
    }

    function addMarker(markerId, lat, lng, iconLink, content, clickCallback, arrayReference) {
        var newMarker = map.addMarker({
            id: markerId,
            lat: lat,
            lng: lng,
            icon: iconLink,
            infoWindow: {
                content: '<p>' + content + '</p>'
            },
            click: function () {
                clickCallback(markerId, lat, lng, content);
            },
        });

        arrayReference.push(newMarker);
        console.log('GATMAP: marker added!');
        return newMarker;
    }


    function updateMarker(markerId, lat, lng, iconLink, content, clickCallback, arrayReference) {
        var markerToUpdate = removeMarker(markerId, arrayReference);

        if (markerToUpdate != null) {
            markerToUpdate.lat = lat,
              markerToUpdate.lng = lng,
              markerToUpdate.icon = iconLink,
              markerToUpdate.infoWindow = { content: '<p>' + content + '</p>' },
              markerToUpdate.click = function () {
                  clickCallback(markerId, lat, lng, content);
              }

            map.addMarker(markerToUpdate);
            arrayReference.push(markerToUpdate);
            console.log('GATMAP: marker updated!');
        }

        //for (var marker in arrayReference) {
        //    if (marker.markerId == markerId) {
        //        markerToUpdate = marker;
        //        break;
        //    }
        //}

    }

    function removeMarker(markerId, arrayReference) {
        var markerToRemove;
        var arrayLength = arrayReference.length;
        var pos;
        for (var i = 0; i < arrayLength; i++) {
            if (arrayReference[i].id == markerId) {
                markerToRemove = arrayReference[i];
                pos = i;
                break;
            }
        }
        console.log(pos);
        if (pos > -1) {
            map.removeMarker(markerToRemove);
            arrayReference.splice(pos, 1);
            console.log('GATMAP: marker removed!');
            //  markerToRemove = null;
        }
        return markerToRemove;
    }

    return {
        map: map,
        addMarker: addMarker,
        getAddress: getAddress,
        getCoordinates: getCoordinates,
        updateMarker: updateMarker,
        removeMarker: removeMarker
    }

})();

