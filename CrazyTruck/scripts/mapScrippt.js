var mapAdd;
var gMarker;
var gInfoW;

function initMapAdd(){
    gMarker = null;

    var idMap = document.getElementById('mapAdd');
    var objMap = {
        center: {
            lat: 23.877435,
            lng: -102.620500
        },
        zoom: 6,
        streetViewControl: false
    }
    mapAdd = new google.maps.Map(idMap, objMap);

    google.maps.event.addListener(mapAdd, "click", function(event){
        //obtener lat y lng del click
        var coordenadas = event.latLng;

        //crear marcador si no existe
        createMaker();

        //setear marcador
        moveMarker(mapAdd, gMarker, coordenadas);
        console.log(coordenadas.lat() + " " +coordenadas.lng());
    });

}

//input on key Enter
$('#searchAddress').keypress(function(event){
    //13 = key Enter
    if (event.keyCode === 13) {
        //crear geocoder para obtener lugares con direcciones
        var gCoder = new google.maps.Geocoder();

        //buscar por direccion
        var objDirection = {
            address: this.value
        }
        gCoder.geocode(objDirection, fn_coder);

        function fn_coder(data, status){
            if(status === "OK"){
                //obtener coordenadas
                var coordenadas = data[0].geometry.location; //objLatLng

                //crear marcador si no existe
                createMaker();

                //setear marcador
                moveMarker(mapAdd, gMarker, coordenadas);
                mapAdd.setCenter(coordenadas);
                mapAdd.setZoom(18);
            }
            
        }
    }
    
});

function createMaker(){
    if(gMarker == undefined) {
        var objMarker = {
            map: mapAdd,
            animation: google.maps.Animation.DROP,
            draggable: true
        }
        gMarker = new google.maps.Marker(objMarker);

        console.log("se creo");
    } else {
        console.log("ya esta creado")
    }
}

function moveMarker(map, marker, coordenadas){
    //actualizar marker
    marker.setPosition(coordenadas);

    //actualizar nombre escala
    getAddress(marker, function(location){
        document.getElementById("escalaDescripcion").value = location;
        document.getElementById("escalaLat").value = round(coordenadas.lat(), 8);
        document.getElementById("escalaLng").value = round(coordenadas.lng(), 8);

        //actualizar info window
        var objWindow = {
            content: "<div><p>"+location+"</p></div>"
        }
        gInfoW = new google.maps.InfoWindow(objWindow);
        setListeners();

    });
    
}

function setListeners(){
    //addListener(objeto asociado, evento, funcion)
    google.maps.event.addListener(gMarker, "click", function(){
        gInfoW.close();
        gInfoW.open(mapAdd, gMarker);
    });

    google.maps.event.addListener(gMarker, "dragend", function(){
        var coordenadas = gMarker.getPosition();

        console.log(coordenadas.lat()+" "+coordenadas.lng());

        getAddress(gMarker, function(location){
            document.getElementById("escalaDescripcion").value = location;
            document.getElementById("escalaLat").value = round(coordenadas.lat(), 8);
            document.getElementById("escalaLng").value = round(coordenadas.lng(), 8);
    
            //actualizar info window
            var objWindow = {
                content: "<div><p>"+location+"</p></div>"
            }
            gInfoW = new google.maps.InfoWindow(objWindow);
    
        });

    });
}


function getAddress(marker, fn){
    var markerLatLgn = marker.getPosition();
    
    var gCoder = new google.maps.Geocoder();

    var objDirection = {
        location: markerLatLgn
    }
    gCoder.geocode(objDirection, function(data){
        fn(data[1].formatted_address);
        
    });

}

function resetMarker(marker){
    marker.setMap(null);
    marker = undefined;
    
}



/*((-111.0219734 + -110.9083503) / 2)=-110.965162
((27.92516586 + 29.09746723) /2)=28.511317
*/

var mapRutas;
var dr;
function initMapRutas(rutas) {

    var idMap = document.getElementById('mapRutas');

    var objMap = {
        center: {
            lat: 23.877435,
            lng: -102.620500
        },
        zoom: 6,
        streetViewControl: false
    }
    
    if (typeof (mapRutas) == 'undefined') {
        mapRutas = new google.maps.Map(idMap, objMap);

    }

    //limpiar rutas y markers
    if (dr != null) {
        dr.setMap(null);
        dr = null;

        $.each(markers, function (i, val) {
            //console.log(markers[i]);
            markers[i].setMap(null);
            console.log(markers[i]);
        });
        markers = [];
        console.log("limpiar");
    }
    
    //generar rutas objetos
    var objDR = {
        map: mapRutas,
        suppressMarkers: true,
        //markerOptions: {icon: "images/miniMarker.png"}
    }
    
    var ds = new google.maps.DirectionsService(); //obtener coordenadas
    dr = new google.maps.DirectionsRenderer(objDR); //traducir coordenadas
    

    //dr.setMap(null);
    //dr.setOptions(objDR);

    if (rutas != null) {

        createMarkers(mapRutas, rutas, function (markers) {

            //inicializar cluster de markers
            var markerCluster = new MarkerClusterer(mapRutas, markers, {
                //imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
                imagePath: "../images/m"
            });

            //guardar wayPoints
            var points = markers.slice(1, -1);
            var wayPoints = [];
            for (var i = 0; i < points.length; i++) {
                var o = {
                    location: {
                        lat: points[i].position.lat(),
                        lng: points[i].position.lng()
                    },
                    stopover: true
                }
                wayPoints.push(o);
            }

            console.log(wayPoints);

            //generar ruta
            var origin;
            var destination;

            if (rutas.length == 1) {
                origin = markers[0].position;
                destination = markers[0].position;
                console.log("es 1");
            }
            else {
                origin = markers[0].position;
                destination = markers[markers.length - 1].position;
                console.log("es 2 o mas");
            }

            var objDS = {
                origin: origin,
                destination: destination,
                waypoints: wayPoints,
                optimizeWaypoints: true,
                provideRouteAlternatives: true,
                travelMode: "DRIVING"
            }
            ds.route(objDS, function (routes, status) {
                if (status === "OK") {
                    dr.setDirections(routes);
                } else {
                    alert(status);
                }

            });

        });

    }
    else {
        var position = {
            lat: 23.877435, lng: -102.620500
        };

        mapRutas.setCenter(position);
        mapRutas.setZoom(6);
        console.log("es 0");
    }

    
}

//function para generar los markers
var markers = [];
function createMarkers(map, routes, fn){
    //obtener rutas de la tabla
    //var routes = getRutas(idTable);

    //recorrer arreglo de rutas
    $.each(routes, function(index, val){
        //console.log(routes[index].lat+" aqui si "+ routes[index].lng+" n "+routes[index].nombre);
        
        //crear marcador
        var objMarker = {
            map: map,
            position: {
                //obtener posicion
                lat: routes[index].lat,
                lng: routes[index].lng
            }
        }
        console.log(routes[index].lat+" "+routes[index].lng);
        var gMarker = new google.maps.Marker(objMarker);
        
        //set listener click
        var gInfoW = new google.maps.InfoWindow();

        google.maps.event.addListener(gMarker, "click", (function(gMarker){
            
            return function(){
                getAddress(gMarker, function(location){
                    
                    gInfoW.setContent("<div style='width:200px'><span>"+routes[index].nombre+"</span></div>");
                                        
                    //console.log(location);

                    gInfoW.close();
                    gInfoW.open(map, gMarker);
                    
                });

            }
            
        })(gMarker));

        //guardar marker
        markers.push(gMarker);
    });

    fn(markers);

}

//function para redondear
function round(value, decimals) {
    return Number(Math.round(value+'e'+decimals)+'e-'+decimals);
}  