<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html>

<html>
<head runat="server" title="Visores">

    <meta http-equiv="expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="-1">
    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/themes/responsive/css/bootstrap.min.css" rel="stylesheet" />

    <style type="text/css">
        body {
            background: #ffffff;
        }

        .modalLocal {
            background-color: rgba(0,0,0,.5);
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            /*display: block;*/
            z-index: 1;
            overflow-y: scroll;
        }

        .divleft {
            float: left;
            display: inline;
            width: 27%;
        }

        .t-header .t-link {
            color: white;
            height: 100%;
            width: 100%;
        }

        .divleft label {
            display: inline;
        }

        #cotBuscar {
            float: left;
            display: inline;
            width: 80%;
        }

            #cotBuscar input[type=text], #contentSearch input[type=text] {
                width: 31%;
                margin-right: 25px;
                margin-left: 20px;
            }

        textarea {
            width: 86%;
            border-radius: 5px;
            margin-right: 25px;
        }

        .span {
            background-color: rgb(238, 238, 238) !important;
            color: rgb(102, 102, 102) !important;
            border: none !important;
            cursor: pointer !important;
            padding: 2px 12px 3px 12px !important;
            text-decoration: none !important;
            border: 1px solid rgb(128,128,128) !important;
            border-radius: 4px !important;
        }

        #arriba {
            position: relative;
            z-index: 1;
        }

        #abajo {
            position: relative;
            z-index: 0;
        }

        .span:hover {
            background-color: rgb(250, 250, 250) !important;
        }

        h2 {
            font-size: 30px;
        }

        .modal {
            background-color: rgba(0,0,0,.8);
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            display: block;
        }

        #MyGrid {
            /*border-left: none;
            border-right: none;*/
            border: none;
        }

        #GridConsulta {
            width: 100%;
        }

        .t-grid tbody .t-button {
            background-color: #9AAE04;
            color: black;
        }

        .t-last {
            text-align: center;
            width: 200px;
        }

        #txtDocumento, #txtCodConsultor {
            z-index: -1;
        }

        @media(max-width:600px) {
            #ocultar {
                display: none;
            }

            #MyGrid {
                font-size: 13px;
            }

            #ColEstado {
                display: none;
            }

            #WindowLoad {
                position: fixed;
                top: 0px;
                left: 0px;
                z-index: 3200;
                filter: alpha(opacity=65);
                -moz-opacity: 65;
                opacity: 0.65;
                background: #999;
            }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            //dimencionesFullScreen();
            jsShowWindowLoadVMult('')
            setTimeout("inicioPDF1()", 4000);
            setTimeout("inicioPDF2()", 6000);
            setTimeout("inicioPDF3()", 8000);
            setTimeout("inicioPDF4()", 10000);
            setTimeout("inicioPDF5()", 12000);
            setTimeout("jsRemoveWindowLoadVMul()", 3000);
        });

        function jsRemoveWindowLoadVMul() {
            // eliminamos el div que bloquea pantalla
            $("#WindowLoad").remove();

        }

        function jsShowWindowLoadVMult(mensaje) {
            //eliminamos si existe un div ya bloqueando
            jsRemoveWindowLoadVMul();

            //si no enviamos mensaje se pondra este por defecto
            if (mensaje === undefined) mensaje = "Procesando la información<br>Espere por favor";

            //centrar imagen gif
            height = 20;//El div del titulo, para que se vea mas arriba (H)
            var ancho = 0;
            var alto = 0;

            //obtenemos el ancho y alto de la ventana de nuestro navegador, compatible con todos los navegadores
            if (window.innerWidth == undefined) ancho = window.screen.width;
            else ancho = window.innerWidth;
            if (window.innerHeight == undefined) alto = window.screen.height;
            else alto = window.innerHeight;

            //operación necesaria para centrar el div que muestra el mensaje
            var heightdivsito = alto / 2 - parseInt(height) / 2;//Se utiliza en el margen superior, para centrar

            //imagen que aparece mientras nuestro div es mostrado y da apariencia de cargando
            imgCentro = "<div style='text-align:center;height:" + alto + "px;'><div  style='color:#000;margin-top:" + heightdivsito + "px; font-size:20px;font-weight:bold'></div><img  src='../../Images/Loading2.gif' width='100' height='100' ></div>";

            //creamos el div que bloquea grande------------------------------------------
            div = document.createElement("div");
            div.id = "WindowLoad"
            div.style.width = ancho + "px";
            div.style.height = alto + "px";
            $("body").append(div);

            //creamos un input text para que el foco se plasme en este y el usuario no pueda escribir en nada de atras
            input = document.createElement("input");
            input.id = "focusInput";
            input.type = "text"

            //asignamos el div que bloquea
            $("#WindowLoad").append(input);

            //asignamos el foco y ocultamos el input text
            $("#focusInput").focus();
            $("#focusInput").hide();

            //centramos el div del texto
            $("#WindowLoad").html(imgCentro);

        }

        var Frames = window.frames;

        var neg;
        neg = '../../../Content/ArchivosCliente/<%=Session["negVisorMUL"]%>/<%=Session["negVisorMUL"]%>.pdf';

        // creacion de los cuatro visores de pdf que se usaran en el modulo

        function inicioPDF1() {

            if ('<%=Session["Pagina20"]%>' == '0') {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/<%=ViewData["negVisorMUL"]%>.pdf';
            } else {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/Pagina-<%=Session["Pagina20"]%>.pdf';
            }

            document.getElementById('Iframep1').contentWindow.scaleInicial('auto');
            document.getElementById('Iframep1').contentWindow.GotoPages('1');
            var pag1 = Frames[0].document.getElementById("pageContainer1");
            document.getElementById('Iframep1').contentWindow.documento(neg);

            if ('<%=Session["Pagina20"]%>' != '0') {
                document.getElementById('Iframep1').contentWindow.cordenadasPDFValDacti(pag1, 170, 50, 1.5);
            }
        }

        function inicioPDF2() {

            if ('<%=Session["Pagina260"]%>' == '0') {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/<%=ViewData["negVisorMUL"]%>.pdf';
            } else {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/Pagina-<%=Session["Pagina260"]%>.pdf';
            }

            document.getElementById('Iframep2').contentWindow.scaleInicial('auto');
            document.getElementById('Iframep2').contentWindow.GotoPages('1');
            var pag1 = Frames[1].document.getElementById("pageContainer1");
            document.getElementById('Iframep2').contentWindow.documento(neg);
            /*si el producto es LD pone posiciones del pagare LD*/

            //document.getElementById('Iframep2').contentWindow.cordenadasPDFValDacti(pag1, 550, 650, 2.2);
            if ('<%=Session["Pagina260"]%>' != '0') {
                document.getElementById('Iframep2').contentWindow.cordenadasPDFValDacti(pag1, 270, 790, 2.2);
            }

        }

        function inicioPDF3() {

            if ('<%=Session["Pagina280"]%>' == '0') {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/<%=ViewData["negVisorMUL"]%>.pdf';
            } else {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/Pagina-<%=Session["Pagina280"]%>.pdf';
            }

            document.getElementById('Iframep3').contentWindow.scaleInicial('auto');
            document.getElementById('Iframep3').contentWindow.GotoPages('1');
            var pag1 = Frames[2].document.getElementById("pageContainer1");
            document.getElementById('Iframep3').contentWindow.documento(neg);

            /*si el producto es LD pone posiciones del pagare LD*/
            if ('<%=Session["Pagina280"]%>' != '0') {
                document.getElementById('Iframep3').contentWindow.cordenadasPDFValDacti(pag1, 230, 680, 2.4);
                //document.getElementById('Iframep3').contentWindow.cordenadasPDFValDacti(pag1, 50, 700, 2.4);
                //document.getElementById('Iframep2').contentWindow.cordenadasPDFValDacti(pag1, 140, 680, 2.4);
            }
            
        }
        function inicioPDF4() {

            if ('<%=Session["Pagina260"]%>' == '0') {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/<%=ViewData["negVisorMUL"]%>.pdf';
            } else {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/Pagina-<%=Session["Pagina260"]%>.pdf';
            }

            document.getElementById('Iframep4').contentWindow.scaleInicial('auto');
            document.getElementById('Iframep4').contentWindow.GotoPages('1');
            var pag1 = Frames[3].document.getElementById("pageContainer1");
            document.getElementById('Iframep4').contentWindow.documento(neg);

            /*si el producto es LD pone posiciones del pagare LD*/

            if ('<%=Session["Pagina260"]%>' != '0') {
                //document.getElementById('Iframep3').contentWindow.cordenadasPDFValDacti(pag1, 230, 680, 2.4);
                document.getElementById('Iframep4').contentWindow.cordenadasPDFValDacti(pag1, 50, 700, 2.4);
                //document.getElementById('Iframep3').contentWindow.cordenadasPDFValDacti(pag1, 270, 790, 2.2);
            }                

        }
        function inicioPDF5() {


            if ('<%=Session["Pagina280"]%>' == '0') {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/<%=ViewData["negVisorMUL"]%>.pdf';
            } else {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/Pagina-<%=Session["Pagina280"]%>.pdf';
            }

            document.getElementById('Iframep5').contentWindow.scaleInicial('auto');
            document.getElementById('Iframep5').contentWindow.GotoPages('1');
            var pag1 = Frames[4].document.getElementById("pageContainer1");
            document.getElementById('Iframep5').contentWindow.documento(neg);

            /*si el producto es LD pone posiciones del pagare LD*/
            if ('<%=Session["Pagina280"]%>' != '0') {
                document.getElementById('Iframep5').contentWindow.cordenadasPDFValDacti(pag1, 270, 790, 2.2);   
            }

        }

        function alerta(pag, docId) {

            transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + docId, null,
                function (data) {
                    $("#CausalesImagen").html(data);
                }, function (error) { console.log(error) });
        }

        //INICIO: Funciones Para el visor PDF (creadas por Carlos Torres)

        var contador = 1;
        function RutaNegocio() {

            if (contador == 1) {
                if ('<%=Session["Pagina20"]%>' != '0') {
                    contador++;
                    return '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/Pagina-<%=Session["Pagina20"]%>.pdf';
                }
                else {
                    Alternar(Visor1Cedula);
                    contador++;
                    return '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/<%=ViewData["negVisorMUL"]%>.pdf';
                }
            } else if (contador == 3 || contador == 5) {
                if ('<%=Session["Pagina280"]%>' != '0') {
                    contador++;
                    return '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/Pagina-<%=Session["Pagina280"]%>.pdf';
                }
                else {

                    Alternar(Visor3PagareLD);
                    Alternar(Visor5PagareLD);
                    contador++;
                    return '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/<%=ViewData["negVisorMUL"]%>.pdf';
                }
            } else if (contador == 2 || contador == 4) {
                if ('<%=Session["Pagina260"]%>' != '0') {
                    contador++;
                    return '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/Pagina-<%=Session["Pagina260"]%>.pdf';
                }
                else {
                    Alternar(Visor2PagareTDC);
                    Alternar(Visor4PagareTDC);
                    contador++;
                    return '../../../Content/ArchivosCliente/<%=ViewData["negVisorMUL"]%>/<%=ViewData["negVisorMUL"]%>.pdf';
                }
            } else {
                contador++;
                return '';
            }
}

function ObtenerIdFrame(idfra) {
    iframeIdDac = idfra;
}

function Alternar(Seccion) {
    if (Seccion.style.display == "none") {
        Seccion.style.display = "";
    }
}
//FIN: Funciones Para el visor PDF 

    </script>

    <script type="text/javascript" charset="utf8" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js"></script>
    <script src="../../Scripts/jquery.dataTables.min.js"></script>

</head>

<body>
    <fieldset class="scheduler-border" style="width: 1500px;">

        <div class="alert alert-danger" id="alerta2" style="display: none;">
            <strong>Atención! </strong><%=ViewData["_mensajeInformacion"] %>
        </div>
        <div class="row">
            <div class="col-lg-7D" >
                <div style="position: relative;">
                    <div class="alert alert-danger" id="Visor1Cedula" style="position: absolute; margin-left: 10%; margin-top: 10%; z-index: 1; display: none">
                        <strong>Atención!</strong>No se indexo la Cedula del Cliente
                    </div>
                </div>

                <div>
                    <fieldset style="width: 935px; height: 600px">
                        <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframep1"></iframe>
                    </fieldset>
                </div>

            </div>
            <div class="col-lg-2D">
                <div style="position: relative;">
                    <div class="alert alert-danger" id="Visor2PagareTDC" style="position: absolute; margin-left: 10%; margin-top: 10%; z-index: 1; display: none">
                        <strong>Atención! </strong>No se indexo el Pagare TDC
                    </div>
                </div>
                <div>
                    <fieldset style="width: 300px; height: 600px">
                        <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframep2"></iframe>
                    </fieldset>
                </div>
            </div>
            <div class="col-lg-2D">
                <div class="row">
                    <div style="position: relative;">
                        <div class="alert alert-danger" id="Visor3PagareLD" style="position: absolute; margin-left: 10%; margin-top: 10%; z-index: 1; display: none">
                            <strong>Atención! </strong>No se indexo el Pagare LD
                        </div>
                    </div>
                    <div>
                        <fieldset style="width: 300px; height: 600px">
                            <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframep3"></iframe>
                        </fieldset>
                    </div>
                </div>

            </div>

        </div>
        <div class="row">
            <div class="col-lg-6">
                <div style="position: relative;">
                    <div class="alert alert-danger" id="Visor4PagareTDC" style="position: absolute; margin-left: 10%; margin-top: 10%; z-index: 1; display: none">
                        <strong>Atención!</strong>No se indexo el Pagare TDC
                    </div>
                </div>
                <div class="span6">
                    <div style="position: inherit!important; z-index: 0 /*top: -42px!important; height: 550px!important*/">
                        <fieldset style="width: 765px; height: 280px">
                            <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframep4"></iframe>
                        </fieldset>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div style="position: relative;">
                    <div class="alert alert-danger" id="Visor5PagareLD" style="position: absolute; margin-left: 10%; margin-top: 10%; z-index: 1; display: none">
                        <strong>Atención! </strong>No se indexo El pagare TDC
                    </div>
                </div>
                <div style="position: inherit!important; /*top: -42px!important; height: 550px!important*/">
                    <fieldset style="width: 750px; height: 280px">
                        <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframep5"></iframe>
                    </fieldset>
                </div>
            </div>
        </div>

    </fieldset>
    <div class="alert alert-danger" id="alerta1" style="display: none">
        <strong>Atención!</strong> Recuerda tienes causales pendientes por validar.
    </div>
    <div class="alert alert-danger" id="alerta3" style="display: none;">
        <strong>Atención! </strong>No se ha validado ninguna causal.
    </div>
    <div class="alert alert-danger" id="alerta4" style="display: none;">
        <strong>Atención! </strong>Error reiniciando causales contacte al administrador.
    </div>
    <br />

</body>
</html>
