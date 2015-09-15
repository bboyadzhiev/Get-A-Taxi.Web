var getTaxiStands = (function () {
    var taxistands = [];
    function clkW(id, lat, lng, content) {

    }
    $.ajax({
        url: '/Operator/Main/GetTaxiStands',
        type: 'GET',
        contentType: 'application/json',
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                gATMap.addMarker(i, data[i].lat, data[i].lon, '/Content/Images/Map/taxistandMarker.png', data[i].alias, clkW, taxistands);
            }
        },
        error: function () {

        }
    });
})();