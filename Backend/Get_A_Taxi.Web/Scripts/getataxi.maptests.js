var map;

function getZoom() {
	$('#MapZoom').val(map.zoom);
}

google.maps.event.addListener(map, 'click', function (e) {
	map.setCenter(e.latLng.lat(), e.latLng.lng());
	map.addMarker({
		lat: latlng.lat(),
		lng: latlng.lng()
	});
});

$(document).ready(function () {
	map = new GMaps({
		div: '#map',
		lat: -12.043333,
		lng: -77.028333,
		zoom_changed: function (map) {
			$('#MapZoom').val(map.getZoom());
		},
		click: function (e) {
			map.setCenter(e.latLng.lat(), e.latLng.lng());
			map.addMarker({
				lat: latlng.lat(),
				lng: latlng.lng()
			});
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
					map.addMarker({
						lat: latlng.lat(),
						lng: latlng.lng()
					});
					$('#CenterLatitude').val(latlng.lat());
					$('#CenterLongitude').val(latlng.lng());
					$('#MapZoom').val(map.getZoom());
					$('#Title').val($('#address').val());
				}
			}
		});
	});
});