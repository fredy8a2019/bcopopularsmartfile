<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Cambio de Contraseña
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="formPass" action="/Usuarios/CambioContrasena" method="post" onsubmit=" return comprobarPass(this)">
    <br />
        <br />
        <h3>
        Cambio de Contraseña</h3>
    <fieldset class="scheduler-border">
        <legend class="scheduler-border"></legend>
        <label>
            Contraseña Actual</label>
        <br />
        <input type="password" class="form-control" style="width: 30%;" name="passActual" /><br />
        <label>
            Nueva Contraseña</label><br />
        <input type="password" id="pswNuevo1" class="form-control" style="width: 30%;" name="passNuevo" /><br />
        <label>
            Repita La Constraseña</label><br />
        <input type="password" id="pswNuevo2" class="form-control" style="width: 30%;" name="confPass" />
        <div id="pass-info" style="width:30%"></div>
        <input type="hidden" id="validacionValor" />
        <br />
        <br />
        <input type="submit" class="btn btn-login" name="btn_Guardar" id="btnGuardar" value="Guardar Cambios" />
    </fieldset>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script src="../../Scripts/jquery.passwordstrength.js" type="text/javascript"></script>
    <link href="../../Styles/jquery.passwordstrength.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function comprobarPass(formulario) {
            var campo1 = formulario.passNuevo.value;
            var campo2 = formulario.confPass.value;

            if (campo1 == "" || campo2 == "") {
                return false;
            }

            if (campo1 == campo2) {
                return true;
            }
            else {
                alert("Las contraseñas no coinciden")
                return false;
            }
        }
    </script>
    <style type="text/css">
        #passwordTest
        {
            width: 400px;
            margin-left: auto;
            margin-right: auto;
            background: #F0F0F0;
            padding: 20px;
            border: 1px solid #DDD;
            border-radius: 4px;
        }
        #passwordTest input[type="password"]
        {
            width: 97.5%;
            height: 25px;
            margin-bottom: 5px;
            border: 1px solid #DDD;
            border-radius: 4px;
            line-height: 25px;
            padding-left: 5px;
            font-size: 25px;
            color: #829CBD;
        }
        #pass-info
        {
            width: 97.5%;
            height: 25px;
            border: 1px solid #DDD;
            border-radius: 4px;
            color: #829CBD;
            text-align: center;
            font: 12px/25px Arial, Helvetica, sans-serif;
        }
        #pass-info.weakpass
        {
            border: 1px solid #FF9191;
            background: #FFC7C7;
            color: #94546E;
            text-shadow: 1px 1px 1px #FFF;
        }
        #pass-info.stillweakpass
        {
            border: 1px solid #FBB;
            background: #FDD;
            color: #945870;
            text-shadow: 1px 1px 1px #FFF;
        }
        #pass-info.goodpass
        {
            border: 1px solid #C4EEC8;
            background: #E4FFE4;
            color: #51926E;
            text-shadow: 1px 1px 1px #FFF;
        }
        #pass-info.strongpass
        {
            border: 1px solid #6ED66E;
            background: #79F079;
            color: #348F34;
            text-shadow: 1px 1px 1px #FFF;
        }
        #pass-info.vrystrongpass
        {
            border: 1px solid #379137;
            background: #48B448;
            color: #CDFFCD;
            text-shadow: 1px 1px 1px #296429;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var password1 = $('#pswNuevo1'); //id of first password field
            var password2 = $('#pswNuevo2'); //id of second password field
            var passwordsInfo = $('#pass-info'); //id of indicator element
            $('#btnGuardar').css('display', 'none');

            passwordStrengthCheck(password1, password2, passwordsInfo); //call password check function
        });

        function passwordStrengthCheck(password1, password2, passwordsInfo) {
            //Debe contener 4 o mas caracteres
            var WeakPass = /(?=.{4,}).*/;
            //Debe contener letras minusculas y al menos un numero
            var MediumPass = /^(?=\S*?[a-z])(?=\S*?[0-9])\S{5,}$/;
            //Debe contener una letra Mayuscula, letras minusculas y al menos un numero
            var StrongPass = /^(?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9])\S{5,}$/;
            //Debe contener una Mayuscula, letras minusculas, un numero, y un caracter especial
            var VryStrongPass = /^(?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9])(?=\S*?[^\w\*])\S{5,}$/;

            $(password1).on('keyup', function (e) {
                if (VryStrongPass.test(password1.val())) {
                    passwordsInfo.removeClass().addClass('vrystrongpass').html("Contraseña segura");
                    $('#validacionValor').val('1');
                }
                else if (StrongPass.test(password1.val())) {
                    passwordsInfo.removeClass().addClass('strongpass').html("Ingrese al menos un caracter especial");
                    $('#validacionValor').val('0');
                    $('#btnGuardar').css('display', 'none');
                }
                else if (MediumPass.test(password1.val())) {
                    passwordsInfo.removeClass().addClass('goodpass').html("Ingrese al menos una letra mayuscúla");
                    $('#validacionValor').val('0');
                    $('#btnGuardar').css('display', 'none');
                }
                else if (WeakPass.test(password1.val())) {
                    passwordsInfo.removeClass().addClass('stillweakpass').html("Ingrese un número");
                    $('#validacionValor').val('0');
                    $('#btnGuardar').css('display', 'none');
                }
                else {
                    passwordsInfo.removeClass().addClass('weakpass').html("La contraseña debe ser de mas de 4 caracteres");
                    $('#validacionValor').val('0');
                    $('#btnGuardar').css('display', 'none');
                }
            });

            $(password2).on('keyup', function (e) {
                if (password1.val() !== password2.val()) {
                    passwordsInfo.removeClass().addClass('weakpass').html("Las contraseñas no coinciden");
                } else {
                    var validacion = $('#validacionValor').val();
                    if (validacion == '1') {
                        $('#btnGuardar').css('display', 'block');
                        passwordsInfo.removeClass().addClass('goodpass').html("Las contraseñas coinciden");
                    }
                    else {
                        passwordsInfo.removeClass().addClass('goodpass').html("Las contraseñas coinciden pero no cumplen con los estandares de seguridad");
                        $('#btnGuardar').css('display', 'none');
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>