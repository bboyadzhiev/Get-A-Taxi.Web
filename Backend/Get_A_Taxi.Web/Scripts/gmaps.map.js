﻿//var markers = [];
$(document).ready(function () {
    gATMap = (function () {

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
                console.log('marker updated');
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
                if (arrayReference[i].markerId == markerId) {
                    markerToRemove = arrayReference[i];
                    pos = i;
                    break;
                }
            }

            if (pos > -1) {
                map.removeMarker(markerToRemove);
                arrayReference.splice(pos, 1);
                console.log('marker removed');
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
   
});

