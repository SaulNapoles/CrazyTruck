function agregarOperador() {
    var operadorNombre = $('#operadorNombre').val();
    var operadorApellido = $('#operadorApellido').val();
    var operadorDireccion = $('#operadorDireccion').val();
    var operadorCurp = $('#operadorCurp').val();
    var operadorNumLicencia = $('#operadorNumLicencia').val();
    var operadorTelefono = $('#operadorTelefono').val();
    var operadorNumOperador = $('#operadorNumOperador').val();
    var operadorNss = $('#operadorNss').val();

    var url = '/Operadores/agregar/';
    var $form = $(document.createElement('form')).css({ display: 'none' }).attr("method", "POST").attr("action", url);

    var $operadorNombre = $(document.createElement('input')).attr('name', 'operadorNombre').val(operadorNombre);
    var $operadorApellido = $(document.createElement('input')).attr('name', 'operadorApellido').val(operadorApellido); 
    var $operadorDireccion = $(document.createElement('input')).attr('name', 'operadorDireccion').val(operadorDireccion);
    var $operadorCurp = $(document.createElement('input')).attr('name', 'operadorCurp').val(operadorCurp);
    var $operadorNumLicencia = $(document.createElement('input')).attr('name', 'operadorNumLicencia').val(operadorNumLicencia);
    var $operadorTelefono = $(document.createElement('input')).attr('name', 'operadorTelefono').val(operadorTelefono);
    var $operadorNumOperador = $(document.createElement('input')).attr('name', 'operadorNumOperador').val(operadorNumOperador);
    var $operadorNss = $(document.createElement('input')).attr('name', 'operadorNss').val(operadorNss);

   
    $form.append($operadorNombre);
    $form.append($operadorApellido);
    $form.append($operadorDireccion);
    $form.append($operadorCurp);
    $form.append($operadorNumLicencia);
    $form.append($operadorTelefono);
    $form.append($operadorNumOperador);
    $form.append($operadorNss);

    $("body").append($form);
    $form.submit();

}
