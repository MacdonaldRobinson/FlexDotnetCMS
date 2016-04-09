<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoogleMaps.ascx.cs" Inherits="WebApplication.Controls.Widgets.GoogleMaps" %>

<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
<script>
    var geocoder;
    var map;
    var marker;
    var infowindow = new google.maps.InfoWindow();

    function initialize() {
        geocoder = new google.maps.Geocoder();
        var latlng = new google.maps.LatLng(-34.397, 150.644);
        var mapOptions = {
            zoom: 8,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        }
        map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

<%
    foreach (var addressWithInfoWindowContent in AddressesWithInfoWindowContent)
    {
        %>
        codeAddress('<%=addressWithInfoWindowContent.Key%>', '<%= addressWithInfoWindowContent.Value %>');
        <%
    }
%>
    }

    function codeAddress(address, infoWindowContent) {
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                map.setCenter(results[0].geometry.location);
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location
                });

                google.maps.event.addListener(marker, 'click', function () {
                    infowindow.setContent(infoWindowContent);
                    infowindow.open(map, marker);
                });

            } else {
                alert('Geocode was not successful for the following reason: ' + status);
            }
        });
    }

    google.maps.event.addDomListener(window, 'load', initialize);
</script>

<style type="text/css">
    #map-canvas {
        width: 400px;
        height: 300px;
    }

        #map-canvas img {
            max-width: none;
        }
</style>

<div id="map-canvas"></div>