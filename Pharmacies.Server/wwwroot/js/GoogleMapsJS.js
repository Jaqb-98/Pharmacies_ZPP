



IsInArea = (lat, lng, area) => {
    var location = new google.maps.LatLng(lat, lng);

    var paths = [];

    for (var i = 0; i < area.length; i++) {
        paths.push(new google.maps.LatLng(area[i].lat, area[i].lng));
    }

    var polygon = new google.maps.Polygon({
        paths: paths
    });

    return google.maps.geometry.poly.containsLocation(location, polygon);

}

