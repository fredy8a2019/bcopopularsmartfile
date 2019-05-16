

function validarCampos() {
    //Se valida el campo numero de telefono
    var valorCamp = $("#telFijo").val();
    var valoCel = $("#txtCelular").val();
    if (valorCamp != "") {
        if (!(valorCamp.match(/^[0-9]{6,7}$/))) {
            $("#telFijo").after("<p class='error'>Ingrese un valor correcto</p>");
            return false;
        }
    }
    if (valoCel != "") {
        if (!(valoCel.match(/^[0-9]{9,10}$/))) {
            $("#txtCelular").after("<p class='error'>Ingrese un valor correcto</p>");
            return false;
        }
    }

    return true;
}

function remplazarComas(form) {
    var inputs = $(form).find("input");

    $.each(inputs, function (i, value) {
        var valorTexto = $(value).val();
        var valorTexto1 = valorTexto.replace(/[,]/gi, ' ');
        $(value).val(valorTexto1);
    });

    var inputs = $(form).find("textarea");

    $.each(inputs, function (i, value) {
        var valorTexto = $(value).val();
        var valorTexto1 = valorTexto.replace(/[,]/gi, ' ');
        $(value).val(valorTexto1);
    });
}