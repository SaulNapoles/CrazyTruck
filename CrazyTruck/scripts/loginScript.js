//$(document).ready(function () {
//    if ($Session["nombre"] == "" && $Session["nombre"] == null) {
//        window.location.href = "/Login/Index";
//    }
//    });

function salir() {
    //metodo post
    $.ajax({
        url: '/Login/LogOut',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function () {
            alert("success");
            window.location.href = "/Login/Index";
            
         
        },
        error: function () {
            alert("error");
        }
    });
}


function validarCampos() {

    if ($('#userEmail').val() == null || $('#userEmail').val().lenght == 0) {
        
    } else if ($('#userPass').val() == null || $('#userPass').val().lenght == 0) {
      

    } else {

        iniciarSesion();
    }
}

function iniciarSesion() {
    
        var userEmail = $('#userEmail').val();
        var userPass = $('#userPass').val();

        var url = '/Login/startSession/';
        var $form = $(document.createElement('form')).css({ display: 'none' }).attr("method", "POST").attr("action", url);

        var $userEmail = $(document.createElement('input')).attr('name', 'userEmail').val(userEmail.trim());
        var $userPass = $(document.createElement('input')).attr('name', 'userPass').val(userPass.trim());
        $form.append($userEmail);
        $form.append($userPass);

        $("body").append($form);
        $form.submit();

    
}

