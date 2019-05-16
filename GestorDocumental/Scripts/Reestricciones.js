//Para que solo ingrese numeros
function numbersonly(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8 && unicode != 9) {
        if (unicode < 47 || unicode > 57) //if not a number
        {
            return false
        } //disable key press    
    }
}

function decimales(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8 && unicode != 45 && unicode != 9) {
        if (unicode < 43 || unicode > 57) //if not a number
        { return false } //disable key press    
    }
}

function decimales_Coma(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8 && unicode != 45 && unicode != 9) {
        if (unicode < 43 || unicode > 57 || unicode == 46) //if not a number
        { return false } //disable key press    
    }
}

//Para que solo ingrese letras
function soloLetras(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
    especiales = [8, 37, 39, 46];

    tecla_especial = false
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}

//Funcion para bloquear todo el teclado (Para los campos que tienen calendarios
function bloqTeclado(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode > 1) //if not a number
    { return false } //disable key press
}
