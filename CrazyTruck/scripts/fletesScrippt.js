$(document).ready(function() {

    //colorear tab
    $('#menuSection a').removeClass("menuSelected");
    $('#menuSection a:nth-child(1)').addClass("menuSelected");

    //set titulo seccion
    $('#titleSection h2').text("Fletes");

    //btnShowEditEscala();
    //btnShowDeleteEscala();

    //inicializar tabla de escalas
    //setDataTableEscalas();
});

$(function(){
    //dar formato a la tabla
    var table = $('#tableFletes').DataTable({
        language: dataTableLanguage,
        order: [[ 4, "desc" ]],
        
    });
});

//mostrar formulario agregar flete
$('#subtitleSection #btnShowAdd').on({
    click: function(){
        var section = "#addSection";
        var info = "#generalInfo";
        var scales = "#scales";

        if(!$(section).find(info).length){
            $(info).appendTo(section);
            $(info).removeClass('clearInfo');
        }

        if(!$(section).find(scales).length){
            $(scales).appendTo(section);
            $(scales).removeClass('clearInfo block');
        }

        $(this).hide();
        $('#tableSection').hide();
        $('#addSection').show();
        $('#btnShowSearch').show();
        $('#subtitleSection h3').text("Agregar flete");

        //vaciar formulario
        clearGeneralInfo();
    }
});

function clearGeneralInfo(){
    getTrailerList();
    getOperadorList();
    getRemolqueList();

    $('#generalInfo').show();
    $('#scales').hide();
    $('#remolque1').hide();
    $('#remolque2').hide();
    $('#selectTipoRemolque select').val("-1");

    if(typeof(cargas1) != 'undefined') {
        cargas1 = undefined;
    }
    if(typeof(cargas2) != 'undefined') {
        cargas2 = undefined;
    }
}

//mostrar tabla de fletes
$('#subtitleSection #btnShowSearch, #terminarFlete .btnShowSearch').on({
    click: function(){
        $(this).hide();
        $('#addSection').hide();
        $('#tableSection').show();
        $('#btnShowAdd').show();
        $('#subtitleSection h3').text("Lista de fletes");
    }
});

/*$('select').change(function(){
    if($(this).val() != "-1") {
        $(this).removeClass("noSelected");
    } else {
        $(this).addClass("noSelected");
    }
});*/

/* Obtener lista de elementos para setear en el formulario */
function getTrailerList(){
    $.get("/Fletes/listaTrailers", null, function (list) {
        
        //select
        var setData = $('#fleteTrailer');

        if(list.length != 0) {

            setData.empty();
            setData.append('<option value="-1" disabled selected>Ningun trailer seleccionado</option>');
            
            //recorrer lista obtenida del controlador
            $.each(list, function(i, val){
                var option = '<option value="'+list[i].id+'">'+list[i].matricula+": "+list[i].modelo+" "+list[i].anio+'</option>';

                setData.append(option);
            });
            
        } else {
            setData.html('<option value="-1" disabled selected>Ningun trailer seleccionado</option>');
        }
        setData.change();
    });
}

function getOperadorList(){
    $.get("/Fletes/listaOperadores", null, function(list){

        //select
        var setData = $('#fleteOperador');

        if(list.length != 0) {

            setData.empty();
            setData.append('<option value="-1" disabled selected>Ningun operador seleccionado</option>');
            
            //recorrer lista obtenida del controlador
            $.each(list, function(i, val){
                var option = '<option value="'+list[i].id+'">'+list[i].numOperador+": "+list[i].nombre+" "+list[i].apellido+'</option>';

                setData.append(option);
            });
            
        } else {
            setData.html('<option value="-1" disabled selected>Ningun operador seleccionado</option>');
        }
        setData.change();
    });
}

function getRemolqueList(){
    $.get("/Fletes/listaRemolques", null, function(list){

        //select
        var setData1 = $('#fleteRemolque1');
        var setData2 = $('#fleteRemolque2');

        if(list.length != 0) {

            setData1.empty();
            setData1.append('<option value="-1" disabled selected>Ningun remolque seleccionado</option>');

            setData2.empty();
            setData2.append('<option value="-1" disabled selected>Ningun remolque seleccionado</option>');
            
            //recorrer lista obtenida del controlador
            $.each(list, function(i, val){
                var option = '<option value="'+list[i].id+'">'+list[i].matricula+'</option>';

                setData1.append(option);
                setData2.append(option);
            });
            
        } else {
            setData1.html('<option value="-1" disabled selected>Ningun operador seleccionado</option>');
            setData2.html('<option value="-1" disabled selected>Ningun operador seleccionado</option>');
        }
        setData1.change();
        setData2.change();
    });
}

/* Manipulacion de cargas temporales */
function rowCarga(carga, index){
    return '<tr>'+
                '<td>'+carga[index].descripcion+'</td>'+
                '<td>'+carga[index].peso+'</td>'+
                '<td class="optionsTable"><button type="button" class="btnEdit btnShowEditCargaTemp"><i class="fas fa-pencil-alt"></i></button><button type="button" class="btnDelete btnShowDeleteCargaTemp"><i class="fas fa-trash-alt"></i></button></td>'+
            '</tr>';
}

var cargas1;
var cargas2;

$('#fleteTipoRemolque').change(function(){
    //obtener cantidad de remolques
    var cantidad = $(this).val();

    var table1 = "#tableCargas1 tbody";
    var table2 = "#tableCargas2 tbody";

    if(cantidad == "1"){
        
        createCargas1();

        $('#remolque2').hide();
        if(typeof(cargas2) == 'undefined' || cargas2.length >= 0){
            cargas2 = undefined;
        }
        $('#fleteRemolque2').val("-1").change();

    } else if(cantidad == "2"){
        
        createCargas1();
        createCargas2();

    }

    function createCargas1(){
        if(typeof(cargas1) == 'undefined') {
            cargas1 = [];
            $('#remolque1').show();
        }
        updateTable(table1, cargas1);

    }

    function createCargas2(){
        if(typeof(cargas2) == 'undefined') {
            cargas2 = [];
            $('#remolque2').show();
        }
        updateTable(table2, cargas2);
        
    }

});

function updateTable(table, cargas){
    if(cargas.length == 0) {
        $(table).html('<tr><td colspan="3">No hay cargas</td></tr>');
    } else {
        $(table).empty();
        $.each(cargas, function(i, val){
            $(table).append(rowCarga(cargas, i));
            console.log(cargas[i].descripcion+" "+cargas[i].peso);
        });
        setBtnEdit();
        setBtnDelete();
    }
}

var editFlete = false;
$('.btnShowAddCargaTemp').on({
    click: function(){

        //limpiar formulario
        $('.formCarga input').val("");
        $('.formCarga textarea').val("");

        var modal = "#modalFormCarga";

        //cerrar modal si se esta editando
        if(($('#modalFormFlete').data('bs.modal') || {}).isShown){
            editFlete = true;
            $('#modalFormFlete').modal('hide');

            //delay para abrir el modal
            setTimeout(function() {
                $(modal).modal({
                    backdrop: 'static'
                })
            }, 400); //delay in miliseconds
        } else {
            editFlete = false;
            //abrir modal
            $(modal).modal('show');
        }

        //cambiar titulo
        //var nombreRemolque = $(this).parents('.selectRemolque').find('select option:selected').text();
        $(modal+" .modal-title").text("Agregar carga");

        //agregar boton añadir
        $(modal+" .modal-footer button:first-child").remove();
        $(modal+" .modal-footer").prepend('<button type="button" class="btnAccept" id="btnAddCargaTemp"><span><i class="fas fa-check"></i></span><span>Agregar</span></button>');

        //habilitar boton añadir
        var idRemolque = $(this).parents('.selectRemolque').find('select').attr('id');
        enableBtnAddCargaTemp(idRemolque);

    }
});

function validateFormCarga() {
    $(".formCarga textarea, .formCarga input").removeClass("error");
    if ($("#cargaDescripcion").val() == "") {
        $("#cargaDescripcion").addClass("error");
        return false;
    }
    else if ($("#cargaPeso").val() == "" || $("#cargaPeso").val() <= 0) {
        $("#cargaPeso").addClass("error");
        return false;
    }
    else {
        return true;
    }
}

$("#cargaDescripcion, #cargaPeso").keyup(function () {
    if ($(this).val() != "") {
        $(this).removeClass("error");
    }
});
$("#cargaPeso").keyup(function () {
    var regex = /[0-9]+/;
    if ($(this).val() > 0 && regex.test($(this).val())) {
        $(this).removeClass("error");
    } else {
        $(this).addClass("error");
    }
});

function enableBtnAddCargaTemp(idRemolque){
    $('#btnAddCargaTemp').on({
        click: function () {

            if (validateFormCarga()) {
                
                var modal = "#modalFormCarga";
                var table1 = "#tableCargas1 tbody";
                var table2 = "#tableCargas2 tbody";

                //crear carga temporal
                var carga = { descripcion: $('#cargaDescripcion').val(), peso: $('#cargaPeso').val() };

                //agregar a correspondiente arreglo
                if (idRemolque == "fleteRemolque1") {
                    cargas1.push(carga);
                    updateTable(table1, cargas1);
                } else if (idRemolque == "fleteRemolque2") {
                    cargas2.push(carga);
                    updateTable(table2, cargas2);
                }

                //cerrar modal
                alert("Carga agregada");
                $(modal).modal('hide');

                if (editFlete == true) {
                    //delay para abrir el modal
                    setTimeout(function () {
                        $('#modalFormFlete').modal({
                            backdrop: 'static'
                        })
                    }, 400); //delay in miliseconds
                    editFlete = false;
                }

            }

        }
    });
    
}

function setBtnEdit(){
    $('.btnShowEditCargaTemp').on({
        click: function(){
            //limpiar formulario
            $('.formCarga input').val("");
            $('.formCarga textarea').val("");

            //obtener valores de la tabla y los guarda en variables
            var row = $(this).parents("tr").find("td");

            var descripcion = row.eq(0).text(); //descripcion
            var peso = row.eq(1).text(); //peso

            //llenar formulario
            $('#cargaDescripcion').val(descripcion);
            $('#cargaPeso').val(peso);

            //abrir modal
            var modal = "#modalFormCarga";
            $(modal).modal('show');

            //cambiar titulo
            $(modal+" .modal-title").text("Editar carga");

            //agregar boton editar
            $(modal+" .modal-footer button:first-child").remove();
            $(modal+" .modal-footer").prepend('<button type="button" class="btnAccept" id="btnEditCargaTemp"><span><i class="fas fa-check"></i></span><span>Editar</span></button>');

            //habilitar boton editar
            var idRemolque = $(this).parents('.selectRemolque').find('select').attr('id');
            var rowIndex = $(this).parents('tr').index();
            //console.log(rowIndex);
            enableBtnEditCargaTemp(idRemolque, rowIndex);

        }
    });
}

function enableBtnEditCargaTemp(idRemolque, rowIndex, carga){
    $('#btnEditCargaTemp').on({
        click: function () {

            if (validateFormCarga()) {
                
                var modal = "#modalFormCarga";
                var table1 = "#tableCargas1 tbody";
                var table2 = "#tableCargas2 tbody";

                //crear carga temporal
                var carga = { descripcion: $('#cargaDescripcion').val(), peso: $('#cargaPeso').val() };

                //editar de correspondiente arreglo
                if (idRemolque == "fleteRemolque1") {
                    cargas1[rowIndex] = carga;
                    updateTable(table1, cargas1);
                } else if (idRemolque == "fleteRemolque2") {
                    cargas2[rowIndex] = carga;
                    updateTable(table2, cargas2);
                }

                //cerrar modal
                alert("Se edito " + carga);
                $(modal).modal('hide');

            }
        }
    });
}


function setBtnDelete(){
    $('.btnShowDeleteCargaTemp').on({
        click: function(){

            //abrir modal
            var modal = "#modalDeleteCarga";
            $(modal).modal('show');
            
            //cambiar titulo
            $(modal+" .modal-title").text("Eliminar carga");

            //agregar boton eliminar
            $(modal+" .modal-footer button:first-child").remove();
            $(modal+" .modal-footer").prepend('<button type="button" class="btnCancel" id="btnDeleteCargaTemp"><span><i class="fas fa-times"></i></span><span>Eliminar</span></button>');

            //habilitar boton editar
            var idRemolque = $(this).parents('.selectRemolque').find('select').attr('id');
            var rowIndex = $(this).parents('tr').index();
           
            enableBtnDeleteCargaTemp(idRemolque, rowIndex);

        }
    })
}

function enableBtnDeleteCargaTemp(idRemolque, rowIndex){
    $('#btnDeleteCargaTemp').on({
        click: function(){
            var modal = "#modalDeleteCarga";
            var table1 = "#tableCargas1 tbody";
            var table2 = "#tableCargas2 tbody";

            //eliminar de correspondiente arreglo
            if(idRemolque == "fleteRemolque1"){
                cargas1.splice(rowIndex, 1);
                updateTable(table1, cargas1);
            } else if(idRemolque == "fleteRemolque2"){
                cargas2.splice(rowIndex, 1);
                updateTable(table2, cargas2);
            }

            //cerrar modal
            alert("Se elimino "+rowIndex);
            $(modal).modal('hide');
        }
    });
}

function validateFormFlete() {
    $(".formFlete select").removeClass("error");
    if ($("#fleteTrailer option:selected").val() == "-1") {
        $("#fleteTrailer").addClass("error");
        return false;
    }
    else if ($("#fleteOperador option:selected").val() == "-1") {
        $("#fleteOperador").addClass("error");
        return false;
    }
    else if ($("#fleteTipoRemolque option:selected").val() == "-1") {
        $("#fleteTipoRemolque").addClass("error");
        return false;
    }
    else if ($("#fleteTipoRemolque option:selected").val() == "1") {
        if ($("#fleteRemolque1 option:selected").val() == "-1") {
            $("#fleteRemolque1").addClass("error");
            return false;
        }
        else {
            return true;
        }
    }
    else if ($("#fleteTipoRemolque option:selected").val() == "2") {
        if ($("#fleteRemolque1 option:selected").val() == "-1") {
            $("#fleteRemolque1").addClass("error");
            return false;
        }
        else if ($("#fleteRemolque2 option:selected").val() == "-1") {
            $("#fleteRemolque2").addClass("error");
            return false;
        }
        else {
            return true;
        }
    } else {
        return true;
    }
}

$("#formFlete select").change(function () {
    if ($(this).val() != "-1") {
        $(this).removeClass("error");
    }
});

/* Agregar flete */
var idFlete;
$('#btnAddFlete').on({
    click: function () {
        //validar formulario
        if(validateFormFlete()){

            var cargasList = [];

            //recorrer cargas
            $.each(cargas1, function (i, val) {
                var c = { idGandola: $('#fleteRemolque1').val(), descripcion: cargas1[i].descripcion, peso: cargas1[i].peso };
                cargasList.push(c);
            });
            $.each(cargas2, function (i, val) {
                var c = { idGandola: $('#fleteRemolque2').val(), descripcion: cargas2[i].descripcion, peso: cargas2[i].peso };
                cargasList.push(c);
            });

            //console.log(cargasList);

            //metodo post
            $.ajax({
                url: '/Fletes/agregarFlete',
                data: JSON.stringify({
                    Flete: {
                        idTrailer: $('#fleteTrailer').val(),
                        idOperador: $('#fleteOperador').val(),
                        idUsuario: "",
                        Carga: cargasList
                    }
                }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    alert("success");

                    $('#generalInfo').hide();
                    $('#scales').show();
                    //setDataTableEscalas();

                    idFlete = data;
                    console.log(idFlete);

                    getEscalaList(idFlete);
                },
                error: function () {
                    alert("error");

                }
            });

            /*console.log(JSON.stringify({
                Flete: {
                    idTrailer: $('#fleteTrailer').val(),
                    idOperador: $('#fleteOperador').val(),
                    idUsuario: 1,
                    Carga: cargasList
                }})
            );*/
        }
    }
});

/* Edit generalInfo in table */
$('.btnShowEditFlete').on({
    click: function(){
        var modal = "#modalFormFlete";
        var info = "#generalInfo";
        var table1 = "#tableCargas1 tbody";
        var table2 = "#tableCargas2 tbody";

        if(!$(modal).find(info).length){
            $(info).appendTo(modal+' #modalContent');
            $(info).addClass('clearInfo');
        }
        
        //vaciar formulario
        clearGeneralInfo();

        //obtener valores de la tabla y los guarda en variables
        var row = $(this).parents("tr").find("td");

        idFlete = row.eq(0).text(); //id

        //hacer metodo post para obtener flete
        $.ajax({
            url: '/Fletes/obtenerInfoFlete?idFlete='+idFlete,
            type: 'GET',
            success: function(){
                alert("success");

                //leer archivo
                $.getJSON("../LocalJSONFile.json", function(data) {
                    //var flete = JSON.parse(data);
                    var flete = data;

                    $('#fleteTrailer').val(flete.idTrailer).change();
                    $('#fleteOperador').val(flete.idOperador).change();

                    //obtener cargas
                    var cargasList = flete.Carga;
                    console.log(cargasList);

                    //verificar cuando son 2 remolques
                    var corte;
                    $.each(cargasList, function(index, val){
                        if(index != 0){
                            var actual = cargasList[index].idGandola;
                            var anterior = cargasList[index - 1].idGandola;
                            console.log("ac" + actual + "an" + anterior);
                            if (actual != anterior) {
                                corte = index;
                                console.log("x" + corte);
                                $('#fleteTipoRemolque').val("2").change();
                                cargas2 = [];
                                return false;
                            } else {
                                $('#fleteTipoRemolque').val("1").change();
                                cargas1 = [];
                            }
                        
                        }
                    });

                    if (cargasList.length != 0) {
                        //distribuir remolques
                        $.each(cargasList, function (index, val) {
                            console.log(index + " " + corte);
                            if (typeof (corte) == 'undefined') {
                                cargas1.push(val);
                            } else {
                                if(index < corte){
                                    cargas1.push(val);
                                } else {
                                    cargas2.push(val);
                                }
                            }
                        });

                        //setear remolques
                        if(typeof(corte) == 'undefined'){
                            //si no existe (solo hay 1 remolque)
                            $('#remolque1').show();
                            $('#fleteRemolque1').val(cargas1[0].idGandola).change();
                            updateTable(table1, cargas1);
                        } else {
                            $('#remolque1').show();
                            $('#fleteRemolque1').val(cargas1[0].idGandola).change();
                            updateTable(table1, cargas1);

                            $('#remolque2').show();
                            $('#fleteRemolque2').val(cargas2[0].idGandola).change();
                            updateTable(table2, cargas2);
                        }
                    }
                });
                

            },
            error: function(){
                alert("error");
            }
        });


        //abrir modal
        $(modal).modal('show');

        //cambiar titulo
        $(modal+" .modal-title").text("Editar flete");

        //agregar boton editar
        $(modal+" .modal-footer button:first-child").remove();
        $(modal+" .modal-footer").prepend('<button type="button" class="btnAccept" id="btnEditFlete"><span><i class="fas fa-check"></i></span><span>Editar</span></button>');

        enableBtnEditInfo(idFlete);
    }
});

function enableBtnEditInfo(idFlete){
    $('#btnEditFlete').on({
        click: function () {

            if (validateFormFlete()) {
                
                var modal = "#modalFormFlete";
                var cargasList = [];
                

                //recorrer cargas
                $.each(cargas1, function (i, val) {
                    console.log(cargas1[i].id + " carga1");
                    var idCarga;
                    if (typeof(cargas1[i].id) == 'undefined') {
                        idCarga = "null";
                    } else {
                        idCarga = cargas1[i].id;
                    }
                    var c = { id: idCarga, idGandola: $('#fleteRemolque1').val(), descripcion: cargas1[i].descripcion, peso: cargas1[i].peso };
                    cargasList.push(c);
                });

                $.each(cargas2, function (i, val) {
                    console.log(cargas2[i].id + " carga2");
                    var idCarga;
                    if (typeof(cargas2[i].id) == 'undefined') {
                        idCarga = "null";
                    } else {
                        idCarga = cargas2[i].id;
                    }
                    console.log();
                    var c = { id: idCarga, idGandola: $('#fleteRemolque2').val(), descripcion: cargas2[i].descripcion, peso: cargas2[i].peso };
                    cargasList.push(c);
                });

                //metodo post
                $.ajax({
                    url: '/Fletes/editarFlete',
                    data: JSON.stringify({
                        Flete: {
                            id: idFlete,
                            idTrailer: $('#fleteTrailer').val(),
                            idOperador: $('#fleteOperador').val(),
                            Carga: cargasList
                        }
                    }),
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    success: function () {
                        alert("success");
                        window.location.href = "/Fletes/Lista";

                        //cerrar modal
                        $(modal).modal('hide');
                    },
                    error: function () {
                        alert("error");
                    }
                });

            }

        }
    });
}

/* Edit scales in table */
$('.btnShowEditFleteEscala').on({
    click: function(){
        var modal = "#modalFormFlete2";
        var scales = "#scales";

        if(!$(modal).find(scales).length){
            $(scales).appendTo(modal+' #modalContent2');
            $(scales).addClass('clearInfo block');
        }

        //inicializar tabla de escalas
        //setDataTableEscalas();

        //obtener valores de la tabla y los guarda en variables
        var row = $(this).parents("tr").find("td");
        idFlete = row.eq(0).text(); //id

        getEscalaList(idFlete);

        //actualizar mapa
        //initMapRutas();

        console.log("el idFlete es: "+idFlete);
        
        //abrir modal
        $(modal).modal('show');

        //cambiar titulo
        $(modal+" .modal-title").text("Editar escalas");

        //agregar boton editar
        $(modal+" .modal-footer button:first-child").remove();
        $(modal+" .modal-footer").prepend('<button type="button" class="btnAccept none" id="btnEditFlete"><span><i class="fas fa-check"></i></span><span>Editar</span></button>');

    }
});

/* Show more info flete */
$('.btnShowInfoFlete').on({
    click: function(){

    }
});

/* Delete flete */
$('.btnShowDeleteFlete').on({
    click: function(){
        //obtener id
        var idFlete = $(this).parents("tr").find("td").eq(0).text();

        //abrir modal
        var modal = "#modalDeleteFlete";
        $(modal).modal('show');
        
        //cambiar titulo
        $(modal+" .modal-title").text("Eliminar flete");

        //agregar boton eliminar
        $(modal+" .modal-footer button:first-child").remove();
        $(modal+" .modal-footer").prepend('<button type="button" class="btnCancel" id="btnDeleteFlete"><span><i class="fas fa-times"></i></span><span>Eliminar</span></button>');

        enableBtnDeleteFlete(idFlete);
    
    }
});

function enableBtnDeleteFlete(idFlete){
    $('#btnDeleteFlete').on({
        click: function(){
            var modal = "#modalDeleteFlete";

            //metodo post
            $.ajax({
                url: '/Fletes/eliminarFlete?idFlete='+idFlete,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function(){
                    alert("success");
                    window.location.href = "/Fletes/Lista";

                    //cerrar modal
                    $(modal).modal('hide');
                },
                error: function(){
                    alert("error");
                }
            });

            //alert(idFlete);
        }
    });
}



/* Manipulacion de escalas */

//fuction para actualizar la tabla de escalas
function getEscalaList(idFlete) {
    var table;
    if ( $.fn.DataTable.isDataTable('#tableEscalas') ) {
        $('#tableEscalas').DataTable().destroy();
    }
    //$('#tableEscalas tbody').empty();
    //$('#tableEscalas').DataTable().clear();
    

    function getList(callback){
        $.get("/Escala/listaEscalas?idFlete="+idFlete, null, function(list){

            //obtener tbody
            var setData = $('#escalaList');
            
            if(list.length != 0) {
                var rutasList = [];

                setData.empty();
                //recorrer lista obtenida del controlador
                $.each(list, function(i, val){
                    var tr = '<tr>' +
                                '<td class="details-control"><i class="fas fa-plus-square"></i></td>'+
                                '<td class="none">'+list[i].id+'</td>'+
                                '<td class="none">'+myTrim(list[i].latitud)+'</td>'+
                                '<td class="none">'+myTrim(list[i].longitud)+'</td>'+
                                '<td>'+list[i].nombre+'</td>'+
                                '<td class="none">'+list[i].descripcion+'</td>'+
                                '<td>'+parseDate(list[i].fecha)+'</td>'+
                                '<td><button type="button" class="btnEdit btnShowEditEscala"><i class="fas fa-pencil-alt"></i></button><button type="button" class="btnDelete btnShowDeleteEscala"><i class="fas fa-trash-alt"></i></button></td>'+
                            '</tr>';
                    
                    setData.append(tr);

                    var position = {lat: Number(myTrim(list[i].latitud)), lng: Number(myTrim(list[i].longitud)), nombre: list[i].nombre};
                    rutasList.push(position);

                });

                callback(rutasList);

            } else {
                $(setData).html('<tr><td colspan="8">No hay escalas</td></tr>');

                callback(null);
            }
            
            //habilitar botones de añadir y eliminar
            btnShowEditEscala();
            btnShowDeleteEscala();
            
        });

    }
    
    getList(function(rutasList){
        //actualizar mapa
        initMapRutas(rutasList);
    });

    //dar formato a la tabla
    table = $('#tableEscalas').DataTable({
        language: dataTableLanguage,
        //order: [[ 6, "desc" ]],
        paging: false,
        ordering: false,
        info: false,
        searching: false,

        responsive: {
            details: {
                renderer: function ( api, rowIdx, columns ) {
                    var data = $.map( columns, function ( col, i ) {
                        return col.hidden ?
                            '<div class="row table-details">'+
                                '<div class="col-xs-12">'+
                                    '<p>'+
                                    '<span>'+col.title+': '+'</span> '+col.data+'</span>'+
                                    '</p>'+
                                '</div>'+
                            '</div>' :
                            '';
                    } ).join('');
 
                    return data ?
                        $('<div class="rowEscalas"/>').append( data ) :
                        false;
                },
                type: 'column'
            }
        },

        columnDefs: [
            //dar prioridad a la columna opciones y mas informacion
            { responsivePriority: 1, targets: -1 },
            { responsivePriority: 1, targets: 0 }
        ]

    });

    //table.draw();

    //listener para abrir y cerrar detalles
    $('#tableEscalas').on('click', 'td.details-control', function(){
                
        var tr = $(this).parents('tr');
        var row = table.row(tr);

        if (row.child.isShown()) {
            tr.addClass('shown');

            $(this).empty();
            $(this).append('<i class="fas fa-minus-square"></i>');
        }
        else {
            tr.removeClass('shown');

            $(this).empty();
            $(this).append('<i class="fas fa-plus-square"></i>');
        
        }
    });

    //actualizar mapa
    //initMapRutas();
}

//function para obtener las rutas de la tabla
function getRutas(){
    var routes = [];

    if($("#tableEscalas tbody tr").length != 0) {

        //recorrer tabla de escalas
        $("#tableEscalas tbody tr").each(function(index){
            //redondear a 8
            var lat = round($(this).find("td").eq(2).text(), 8); //lat
            var lng = round($(this).find("td").eq(3).text(), 8); //lng
            var nombre = $(this).find("td").eq(4).text(); //nombre

            //guardar posicion
            console.log("lat"+lat+"lng"+lng);
            var position = {lat: lat, lng: lng, nombre: nombre}
            routes.push(position);
        });

    }

    //console.log(routes);
    return routes;
}

$('#btnShowAddEscala').on({
    click: function(){

        //limpiar formulario
        $('.formEscala input').val("");
        $('.formEscala textarea').val("");

        var modal = "#modalFormEscala";
        
        //cerrar modal si se esta editando
        if(($('#modalFormFlete2').data('bs.modal') || {}).isShown){
            editFlete = true;
            $('#modalFormFlete2').modal('hide');

            //delay para abrir el modal
            setTimeout(function() {
                $(modal).modal({
                    backdrop: 'static'
                })
            }, 400); //delay in miliseconds
        } else {
            editFlete = false;
            //abrir modal
            $(modal).modal('show');
        }

        //cambiar titulo
        $(modal+" .modal-title").text("Agregar escala");

        //agregar boton añadir
        $(modal+" .modal-footer button:first-child").remove();
        $(modal+" .modal-footer").prepend('<button type="button" class="btnAccept" id="btnAddEscala"><span><i class="fas fa-check"></i></span><span>Agregar</span></button>');

        enableBtnAdd();
        initMapAdd();

    }
});

function validateFormEscala() {
    $(".formEscala input, .formEscala textarea").removeClass("error");
    if ($("#escalaLat").val() == "" || $("#escalaLng").val() == "") {
        alert("Selecciona una dirección");
        return false;
    }
    else if ($("#escalaNombre").val() == "") {
        $("#escalaNombre").addClass("error");
        return false;
    }
    else if ($("#escalaDescripcion").val() == "") {
        $("#escalaDescripcion").addClass("error");
        return false;
    }
    else if ($("#escalaFecha").val() == "") {
        $("#escalaFecha").addClass("error");
        return false;
    }
    else {
        return true;
    }
}
$("#escalaNombre, #escalaDescripcion, #escalaFecha").keyup(function() {
    if ($(this).val() != "") {
        $(this).removeClass("error");
    }
});


function enableBtnAdd(){
    $('#btnAddEscala').on({
        click: function () {

            if (validateFormEscala()) {

                var modal = "#modalFormEscala";

                //metodo post
                $.ajax({
                    url: '/Escala/agregarEscala',
                    data: JSON.stringify({
                        Escala: {
                            idFlete: idFlete,
                            latitud: $('#escalaLat').val(),
                            longitud: $('#escalaLng').val(),
                            nombre: $('#escalaNombre').val(),
                            descripcion: $('#escalaDescripcion').val(),
                            fecha: $('#escalaFecha').val()
                        }
                    }),
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    success: function () {
                        alert("success");

                        //actualizar tabla
                        getEscalaList(idFlete);

                        //cerrar modal
                        $(modal).modal('hide');

                        if (editFlete == true) {
                            //delay para abrir el modal
                            setTimeout(function () {
                                $('#modalFormFlete2').modal({
                                    backdrop: 'static'
                                })
                            }, 500); //delay in miliseconds
                            editFlete = false;
                        }

                    },
                    error: function () {
                        alert("error");

                    }
                });

            }
            
        }
    });
}

function btnShowEditEscala(){
    $('.btnShowEditEscala').on({
        click: function(){
            //limpiar formulario
            $('.formEscala input').val("");
            $('.formEscala textarea').val("");

            //obtener valores de la tabla y los guarda en variables
            var row = $(this).parents("tr").find("td");

            var idEscala = row.eq(1).text(); //id

            //hacer metodo post para obtener flete
            $.ajax({
                url: '/Escala/obtenerEscala?idEscala=' + idEscala,
                type: 'GET',
                success: function (escala) {
                    alert("success");
                    //var escala = JSON.parse(data);

                    //iniciar mapa
                    initMapAdd();

                    //crear marcador
                    createMaker();

                    //setear marcador
                    var coordenadas = new google.maps.LatLng({lat: parseFloat(escala[0].latitud), lng: parseFloat(escala[0].longitud)}); 
                    moveMarker(mapAdd, gMarker, coordenadas);
                    mapAdd.setCenter(coordenadas);
                    mapAdd.setZoom(18);

                    //llenar formulario
                    $('#escalaLat').val(escala[0].latitud);
                    $('#escalaLng').val(escala[0].longitud);
                    $('#escalaNombre').val(escala[0].nombre);
                    $('#escalaDescripcion').val(escala[0].descripcion);
                    $('#escalaFecha').val(parseDate(escala[0].fecha));

                }
            });
            
            var modal = "#modalFormEscala";

            //cerrar modal si se esta editando
            if(($('#modalFormFlete2').data('bs.modal') || {}).isShown){
                editFlete = true;
                $('#modalFormFlete2').modal('hide');

                //delay para abrir el modal
                setTimeout(function() {
                    $(modal).modal({
                        backdrop: 'static'
                    })
                }, 400); //delay in miliseconds
            } else {
                editFlete = false;
                //abrir modal
                $(modal).modal('show');
            }

            //cambiar titulo
            $(modal+" .modal-title").text("Editar escala");

            //agregar boton editar
            $(modal+" .modal-footer button:first-child").remove();
            $(modal+" .modal-footer").prepend('<button type="button" class="btnAccept" id="btnEditEscala"><span><i class="fas fa-check"></i></span><span>Editar</span></button>');

            enableBtnEdit(idEscala);
        }
    });
}

function enableBtnEdit(idEscala){
    $('#btnEditEscala').on({
        click: function () {

            if (validateFormEscala()) {

                var modal = "#modalFormEscala";

                //metodo post
                $.ajax({
                    url: '/Escala/editarEscala',
                    data: JSON.stringify({
                        Escala: {
                            id: idEscala,
                            latitud: $('#escalaLat').val(),
                            longitud: $('#escalaLng').val(),
                            nombre: $('#escalaNombre').val(),
                            descripcion: $('#escalaDescripcion').val(),
                            fecha: $('#escalaFecha').val()
                        }
                    }),
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    success: function () {
                        alert("success");

                        //actualizar tabla
                        getEscalaList(idFlete);

                        //cerrar modal
                        $(modal).modal('hide');

                        if (editFlete == true) {
                            //delay para abrir el modal
                            setTimeout(function () {
                                $('#modalFormFlete2').modal({
                                    backdrop: 'static'
                                })
                            }, 500); //delay in miliseconds
                            editFlete = false;
                        }
                    },
                    error: function () {
                        alert("error");
                    }
                });

            }
        }
    });
}

function btnShowDeleteEscala(){
    $('.btnShowDeleteEscala').on({
        click: function(){
    
            //obtener id
            var idEscala = $(this).parents("tr").find("td").eq(1).text();
            
            var modal = "#modalDeleteEscala";
            
            //cerrar modal si se esta editando
            if(($('#modalFormFlete2').data('bs.modal') || {}).isShown){
                editFlete = true;
                $('#modalFormFlete2').modal('hide');

                //delay para abrir el modal
                setTimeout(function() {
                    $(modal).modal({
                        backdrop: 'static'
                    })
                }, 400); //delay in miliseconds
            } else {
                editFlete = false;
                //abrir modal
                $(modal).modal('show');
            }

            //cambiar titulo
            $(modal+" .modal-title").text("Eliminar escala");
    
            //agregar boton eliminar
            $(modal+" .modal-footer button:first-child").remove();
            $(modal+" .modal-footer").prepend('<button type="button" class="btnCancel" id="btnDeleteEscala"><span><i class="fas fa-times"></i></span><span>Eliminar</span></button>');
    
            enableBtnDeleteEscala(idEscala);
        }
    });
}

function enableBtnDeleteEscala(idEscala){
    $('#btnDeleteEscala').on({
        click: function(){
            var modal = "#modalDeleteEscala";

            //metodo post
            $.ajax({
                url: '/Escala/eliminarEscala?idEscala='+idEscala,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function(){
                    alert("success");
                    
                    //actualizar tabla
                    getEscalaList(idFlete);

                    //cerrar modal
                    $(modal).modal('hide');

                    if (editFlete == true) {
                        //delay para abrir el modal
                        setTimeout(function () {
                            $('#modalFormFlete2').modal({
                                backdrop: 'static'
                            })
                        }, 500); //delay in miliseconds
                        editFlete = false;
                    }
                },
                error: function(){
                    alert("error");
                }
            });

            //alert(idEscala);
        }
    });
}

function parseDate(date){
    var regex = /-?\d+/;
    var result = regex.exec(date);
    var date = new Date(parseInt(result[0]));
    return moment(date).format('DD/MM/YYYY hh:mm a')
}

function myTrim(string) {
    return string.replace(/^\s+|\s+$/gm,'');
}