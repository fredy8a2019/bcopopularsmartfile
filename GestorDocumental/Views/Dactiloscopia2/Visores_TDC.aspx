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
        }

        #WindowLoad
        {
            position:fixed;
            top:0px;
            left:0px;
            z-index:3200;
            filter:alpha(opacity=65);
           -moz-opacity:65;
            opacity:0.65;
            background:#999;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            //dimencionesFullScreen();
            jsShowWindowLoadVTDC('');
            setTimeout("inicioPDF1()", 4000);
            setTimeout("inicioPDF2()", 6000);
            setTimeout("inicioPDF3()", 8000);
            setTimeout("jsRemoveWindowLoadTDC()", 3000);
        });
        function jsRemoveWindowLoadTDC() {
            // eliminamos el div que bloquea pantalla
            $("#WindowLoad").remove();

        }

        function jsShowWindowLoadVTDC(mensaje) {
            //eliminamos si existe un div ya bloqueando
            jsRemoveWindowLoadTDC();

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
            $("doctype").append(div);

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
        neg = '../../../Content/ArchivosCliente/<%=Session["negVisorTDC"]%>/<%=Session["negVisorTDC"]%>.pdf';

        // creacion de los cuatro visores de pdf que se usaran en el modulo

        function inicioPDF1() {
            
            if ('<%=Session["Pagina20"]%>' == '0') {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorTDC"]%>/<%=ViewData["negVisorTDC"]%>.pdf';
            } else {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorTDC"]%>/Pagina-<%=Session["Pagina20"]%>.pdf';
            }

            document.getElementById('Iframep1').contentWindow.documento(neg);
            document.getElementById('Iframep1').contentWindow.scaleInicial('auto');
            document.getElementById('Iframep1').contentWindow.GotoPages('1');
            var pag1 = Frames[0].document.getElementById("pageContainer1");

            document.getElementById('Iframep1').contentWindow.cordenadasPDFValDacti(pag1, 150, 150, 2.0);
        }

        function inicioPDF2() {

            if ('<%=Session["Pagina280"]%>' == '0') {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorTDC"]%>/<%=ViewData["negVisorTDC"]%>.pdf';
            } else {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorTDC"]%>/Pagina-<%=Session["Pagina280"]%>.pdf';
            }
            
            document.getElementById('Iframep2').contentWindow.documento(neg);
            document.getElementById('Iframep2').contentWindow.scaleInicial('auto');
            document.getElementById('Iframep2').contentWindow.GotoPages('1');
            var pag1 = Frames[1].document.getElementById("pageContainer1");

            /*si el producto es LD pone posiciones del pagare LD*/            
            document.getElementById('Iframep2').contentWindow.cordenadasPDFValDacti(pag1, 20, 725, 2.4);
        }

        function inicioPDF3() {

            if ('<%=Session["Pagina260"]%>' == '0') {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorTDC"]%>/<%=ViewData["negVisorTDC"]%>.pdf';
            } else {
                neg = '../../../Content/ArchivosCliente/<%=ViewData["negVisorTDC"]%>/Pagina-<%=Session["Pagina260"]%>.pdf';
            }
            document.getElementById('Iframep3').contentWindow.documento(neg);
            document.getElementById('Iframep3').contentWindow.scaleInicial('auto');
            document.getElementById('Iframep3').contentWindow.GotoPages('1');
            var pag1 = Frames[2].document.getElementById("pageContainer1");

            /*si el producto es LD pone posiciones del pagare LD*/
            document.getElementById('Iframep3').contentWindow.cordenadasPDFValDacti(pag1, 260, 790, 2.2);
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
                    return '../../../Content/ArchivosCliente/<%=ViewData["negVisorTDC"]%>/Pagina-<%=Session["Pagina20"]%>.pdf';
                }
                else {
                    contador++;
                    Alternar(NoCedula);
                    return '../../../Content/ArchivosCliente/<%=ViewData["negVisorTDC"]%>/<%=ViewData["negVisorTDC"]%>.pdf';
                }
            } else if (contador == 2 || contador == 3) {
                if ('<%=Session["Pagina260"]%>' != '0') {
                    contador++;
                    return  '../../../Content/ArchivosCliente/<%=ViewData["negVisorTDC"]%>/Pagina-<%=Session["Pagina260"]%>.pdf';
                }
                else {
                    contador++;
                    Alternar(NoPagare1);
                    Alternar(NoPagare2);
                    return '../../../Content/ArchivosCliente/<%=ViewData["negVisorTDC"]%>/<%=ViewData["negVisorTDC"]%>.pdf';
                }
            } else {
                contador++;
                return '';
            }
        }

        function Alternar(Seccion) {
            if (Seccion.style.display == "none") {
                Seccion.style.display = "";
            }
        }

function ObtenerIdFrame(idfra) {
    iframeIdDac = idfra;
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
            <div class="col-lg-9">
                <div class="row">
                    <div style="position: relative;">
                        <div class="alert alert-danger" id="NoCedula" style="position: absolute; margin-left: 40%; margin-top: 5%; z-index: 1; display: none">
                            <strong>Atención!</strong>No se indexo la Cedula del Cliente
                        </div>
                    </div>
                    <div class="span6">
                        <div style="position: inherit!important; z-index: 0">
                            <fieldset style="width: 1160px; height: 500px">
                                <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframep1"></iframe>
                            </fieldset>
                        </div>
                    </div>
                    <div style="position: relative;">
                        <div class="alert alert-danger" id="NoPagare1" style="position: absolute; margin-left: 40%; margin-top: 5%; z-index: 1; display: none">
                            <strong>Atención!</strong>No se indexo el Pagare TDC
                        </div>
                    </div>
                    <div class="span6">
                        <div style="position: inherit!important; z-index: 0 /*top: -42px!important; height: 550px!important*/">
                            <fieldset style="width: 1160px; height: 260px">
                                <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframep2"></iframe>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-2">
                <div style="position: relative;">
                    <div class="alert alert-danger" id="NoPagare2" style="position: absolute; margin-left: 90%; margin-top: 10%; z-index: 1; display: none">
                        <strong>Atención! </strong>No se indexo el Pagare TDC
                    </div>
                </div>
                <div>
                    <fieldset style="width: 380px; height: 765px">
                        <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframep3"></iframe>
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
