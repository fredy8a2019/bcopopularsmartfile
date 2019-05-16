/*La funciones de validacion del lado cliente deben quedar consignadas en este archivo*/
/*=============================================================================================================*/
/*Funcion  para validacion de e-mail */
/*Parametros: <sender: ><args: Objeto con los el valor  y la propiedad isValid> */

function validateEmail(sender, args) {
    if (args.Value == "") {
        args.IsValid = false;
    }
    else {
        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!filter.test(args.Value)) {
            args.IsValid = false;
        }
    }
}
/*=============================================================================================================*/