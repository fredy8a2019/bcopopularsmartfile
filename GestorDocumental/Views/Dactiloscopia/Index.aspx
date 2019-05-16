<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Dactiloscopia
</asp:Content>
<%--referencias a librerias y estilos HeadContent--%>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <meta http-equiv="expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="-1">

    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />

    <%--<link rel="stylesheet" href="http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css">
    <link href="http://www.jqueryscript.net/css/jquerysctipttop.css" rel="stylesheet" type="text/css">
    <script src="https://code.jquery.com/jquery-1.12.2.js"></script>
    <script src="http://netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js"></script>

    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../../Content/Scripts/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.button.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.menu.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.tooltip.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.datepicker.js" type="text/javascript"></script>
    <link href="../../Content/themes/controls.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput.js" type="text/javascript"></script>

    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>

    <%--<link rel="stylesheet" href="../../Styles/menu-6.css" type="text/css" />
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>--%>

    <style type="text/css">
        body {
            background: #ffffff;
        }

        .divleft {
            float: left;
            display: inline;
            width: 27%;
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
    </style>
</asp:Content>

<%--javascript y HTML MainContent--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            dimencionesFullScreen();
            setTimeout("inicioPDF1()", 4000);
            setTimeout("inicioPDF2()", 5000);
            setTimeout("inicioPDF3()", 6000);
            setTimeout("inicioPDF4()", 8000);
            
        });

        var cordenadasY = '<%=ViewData["CORDENADAS_Y"]%>'.split(',');
        var cordenadasX = '<%=ViewData["CORDENADAS_X"]%>'.split(',');
        var Frames = window.frames;

        var iframeIdDac = "";

        var paginaValDocumental = null;
        var _negId = '<%=ViewData["_negId"]%>';
        (function () {
            paginaValDactiloscopia = {
                loadValDocumental: function () {
                    $(document).ready(function () {
                        $("GridConsultaCausales.t-grid .t-refresh").trigger('click');

                        var negId = '<%=ViewData["_negId"]%>';
                        if (negId == 0) {
                            $("#alerta2").removeAttr("Style");
                            $("#CausalesImg").prop("disabled", true);
                            $("#CausalesG").prop("disabled", true);
                            $("#LstCausales").prop("disabled", true);
                        }
                    });
                }
            },
            paginaValDactiloscopia.loadValDocumental();
        })();

        var neg;
        var neg2;

        neg2 = '../../../Content/ArchivosCliente/<%=ViewData["_negId"]%>/Devuelto/<%=ViewData["_negId"]%>.pdf';
        neg = '../../../Content/ArchivosCliente/<%=ViewData["_negId"]%>/<%=ViewData["_negId"]%>.pdf';

        // creacion de los cuatro visores de pdf que se usaran en el modulo

        function inicioPDF1() {

            document.getElementById('Iframe1').contentWindow.scaleInicial('auto');
            document.getElementById('Iframe1').contentWindow.GotoPages('<%=ViewData["Hoja 2"]%>');
        }

        function inicioPDF2() {

            document.getElementById('Iframe2').contentWindow.scaleInicial('auto');
            document.getElementById('Iframe2').contentWindow.GotoPages('<%=ViewData["Hoja 1"]%>');
            var pag = Frames[1].document.getElementById("pageContainer<%=ViewData["Hoja 1"]%>");
            //funcion con la cual se mueve a la pagina indicada en las cordenadasn
            document.getElementById('Iframe2').contentWindow.cordenadas(pag, cordenadasX[0], cordenadasY[0]);
        }

        function inicioPDF3() {

            document.getElementById('Iframe3').contentWindow.scaleInicial('auto');
            document.getElementById('Iframe3').contentWindow.GotoPages('<%=ViewData["Hoja 3"]%>');
            var pag = Frames[2].document.getElementById("pageContainer<%=ViewData["Hoja 3"]%>");
            //funcion con la cual se mueve a la pagina indicada en las cordenadas
            document.getElementById('Iframe3').contentWindow.cordenadas(pag, cordenadasX[1], cordenadasY[1]);
        }
        function inicioPDF4() {

            document.getElementById('Iframe4').contentWindow.scaleInicial('auto');
            document.getElementById('Iframe4').contentWindow.GotoPages('<%=ViewData["Hoja 4"]%>');
            var pag = Frames[3].document.getElementById("pageContainer<%=ViewData["Hoja 4"]%>");
            //funcion con la cual se mueve a la pagina indicada en las cordenadas
            document.getElementById('Iframe4').contentWindow.cordenadas(pag, cordenadasX[2], cordenadasY[2]);
        }

        $(document).ready(documentoscargados);


        function documentoscargados() {

            if ('<%=ViewData["Hoja 1"]%>' == '0') {
                document.getElementById("NoFUV").style.display = "block";
            }

            if ('<%=ViewData["Hoja 2"]%>' == '0') {
                document.getElementById("NoCedula").style.display = "block";
            }
            if ('<%=ViewData["Hoja 3"]%>' == '0') {
                document.getElementById("NoPagare").style.display = "block";
            }

            if ('<%=ViewData["Hoja 4"]%>' == '0') {
                document.getElementById("NoCarta").style.display = "block";
            }
        }

        function alerta(pag, docId) {

            transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + docId, null,
                function (data) {
                    $("#CausalesImagen").html(data);
                }, function (error) { console.log(error) });
        }


        //INICIO: Funciones Para el visor PDF (creadas por Carlos Torres)

        function RutaNegocio() {
            if ('<%=ViewData["_negId"]%>' != '') {
                if ('<%=ViewData["_Nuevo"]%>' == '2') {
                    return neg2;
                }
                return neg;

            }
            else {
                return '';
            }
        }

        window.addEventListener('keydown', function keydown(evt) {
            if (document.getElementById("zoom").className == 'modal') {

                switch (evt.keyCode) {
                    case 27: // esc key
                        //document.getElementById(iframeIdDac).contentWindow.salirZoomFullScreen();
                        document.getElementById("Iframe1").contentWindow.salirZoomFullScreen();
                        break;
                    case 61: // FF/Mac '='
                    case 107: // FF '+' and '='
                    case 187: // Chrome '+'
                    case 171: // FF with German keyboard
                        document.getElementById(iframeId).contentWindow.ZoomInFull();
                        break;
                    case 173: // FF/Mac '-'
                    case 109: // FF '-'
                    case 189: // Chrome '-'
                        document.getElementById(iframeId).contentWindow.ZoomOutFull();
                        break;
                }
            }
        });

        function moverScrolls(e) {
            var x = e.clientX;
            var y = e.clientY;

            var zoom = document.getElementById('Iframe1').contentWindow.retornaZoom();

            if (isNaN(zoom)) {
                zoom = 0;
            }

            var elmnt = document.getElementById("zoom");
            elmnt.scrollLeft = (x * (zoom + 1));
            elmnt.scrollTop = (y * (zoom + 2));
        }

        function dimencionesFullScreen() {
            var browserWidth = $(window).width(); //document.documentElement.clientWidth;
            var browserHeight = $(window).height(); //document.documentElement.clientHeight;
            document.getElementById("zoom").style.width = browserWidth;
            document.getElementById("zoom").style.height = browserHeight;
        }

        function ObtenerIdFrame(idfra) {
            iframeIdDac = idfra;
        }
        //FIN: Funciones Para el visor PDF 

    </script>

    <!-- Creado el 14/03/2016 por William Eduardo cicua ruiz -->
    <br />
    <br />

    <table style="width: 120%">
        <tr>
            <td>
                <h3>Validación Dactiloscopia</h3>
                    <h4>Negocio: <%=ViewData["TITULO"]%></h4>
            </td>

        </tr>
    </table>

    <div id="zoom" style="overflow: scroll;" hidden="hidden" onmousemove="moverScrolls(event)"></div>

    <div id="MySplitter">
        <fieldset class="scheduler-border" style="width: 1150px;">
            <legend class="scheduler-border">Datos Complementarios</legend>

            <div class="alert alert-danger" id="alerta2" style="display: none;">
                <strong>Atención! </strong><%=ViewData["_mensajeInformacion"] %>
            </div>
            <div class="row">
                <div class="col-lg-7">
                    <div style="position: relative;">
                        <div class="alert alert-danger" id="NoCedula" style="position: absolute; margin-left: 20%; margin-top: 50%; z-index: 1; display: none">
                            <strong>Atención!</strong>No se indexo la Cedula del Cliente
                        </div>
                    </div>
                    <div class="span6">
                        <div id="abajo" style="position: inherit!important; z-index: 0 /*top: -42px!important; height: 550px!important*/">
                            <fieldset style="width: 650px; height: 800px">
                                <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframe1" allowfullscreen></iframe>
                            </fieldset>
                        </div>
                    </div>
                </div>
                <div class="col-lg-5">
                    <div class="">
                        <div style="position: relative;">
                            <div class="alert alert-danger" id="NoFUV" style="position: absolute; margin-left: 20%; margin-top: 50%; z-index: 1; display: none">
                                <strong>Atención! </strong>No se indexo el FUV-4
                            </div>
                        </div>
                        <div id="abajo" style="position: inherit!important; /*top: -42px!important; height: 550px!important*/">
                            <fieldset style="width: 470px; height: 270px">
                                <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframe2" allowfullscreen></iframe>
                            </fieldset>
                        </div>
                    </div>

                    <div class="">
                        <div style="position: relative;">
                            <div class="alert alert-danger" id="NoPagare" style="position: absolute; margin-left: 20%; margin-top: 50%; z-index: 1; display: none">
                                <strong>Atención! </strong>No se indexo el Pagare
                            </div>
                        </div>
                        <div id="abajo" style="position: inherit!important; /*top: -42px!important; height: 550px!important*/">
                            <fieldset style="width: 470px; height: 270px">
                                <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframe3" allowfullscreen></iframe>
                            </fieldset>
                        </div>
                    </div>

                    <div class="">
                        <div style="position: relative;">
                            <div class="alert alert-danger" id="NoCarta" style="position: absolute; margin-left: 20%; margin-top: 50%; z-index: 1; display: none">
                                <strong>Atención! </strong>No se indexo la carta
                            </div>
                        </div>
                        <div id="abajo" style="position: inherit!important; /*top: -42px!important; height: 550px!important*/">
                            <fieldset style="width: 470px; height: 270px">
                                <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframe4" onclick="ObtenerIdFrame('Iframe4')" allowfullscreen></iframe>
                            </fieldset>
                        </div>
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
        <div id="BottomPane2">
            <form method="post">
                
                <input type="button" value="Finalizar" class="btn btn-login" style="width: 87.8px;" onclick="finValidacion()" <%=ViewData["Block"]%> />
                <%-- el siguiente boton llama al pop up en el cual se verifican los datos especificos para veridicar cual doccumento se
                    encuentra incorrecto Creado el 17/03/2016 William Eduardo Cicua--%>

                <input type="reset" name="btnRestablecer" id="btnRestablecer" value="Validar" class="btn btn-login" style="width: auto" onclick="popitup('Noaprobo')" <%=ViewData["Block"]%> />
                <input type="button" value="Reiniciar" class="btn btn-login" style="width: 87.8px; margin-left: 75%;"  onclick="reiniciarCausales()"  />
            </form>
        </div>
    </div>

    <script  type="text/javascript">

        $(Document).ready(alerta());

        // funcion la cual guenera los campos para realizar las validaciones de si se an validado 
        // o no ya que si esto no se realiza nos se puede validar
        function alerta() {

            transact.ajaxPOST("/Dactiloscopia/Noaprobo", null,
                function (data) {
                    $("#CausalesFuv").html(data);
                }, function (error) { console.log(error) });

            transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 1"]%>', null,
                function (data) {
                    $("#CausalesFuv").html(data);
                }, function (error) { console.log(error) });

            transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 2"]%>', null,
                function (data) {
                    $("#CausalesCC").html(data);
                }, function (error) { console.log(error) });

            if ('<%=Session["Tajeta"]%>' == '0') {
                transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 3"]%>', null,
                function (data) {
                    $("#CausalesPagare").html(data);
                }, function (error) { console.log(error) });

                transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 4"]%>', null,
                function (data) {
                    $("#CausalesCart").html(data);
                }, function (error) { console.log(error) });
            } else {
                transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 3"]%>', null,
                function (data) {
                    $("#CausalesPagare&Carta").html(data);
                }, function (error) { console.log(error) });
            }

            ActualizarDataTable();
        }

        function reiniciarCausales() {

            console.log(_negId, "_negId");
            transact.ajaxPOST("/Dactiloscopia/reiniciarCausales?_negId=" + _negId , null,
                function (data) {
                    console.log(data, "data");
                    if (data == 1) {
                        bootbox.dialog({
                            message: "<h4>El caso se a reiniciado de forma correcta</h4>",
                            title: "Reinicio caso",
                            buttons: {
                                succes: {
                                    label: ".: Cerrar :.",
                                    className: "btn-success",
                                }
                            }
                        });
                    } else {
                        Alternar(alerta4);
                    } 
                },
                function (error) {
                    console.log(error);
                });

        }

        // esta funcion se ejecuta cuando se le da finalizar a Dactiloscopia entrando a una funcion ajax que le retorna
        // 1, 2 o 3 con lo cual en el caso 1 termina la dactiloscopia por que ya valido todas las causales el 2 cuando
        // hay algunas validadas pero no todas y 3 que no a validado ninguna en los dos ultimos casos activa un strong por 
        // 15  segundos que indica el error
        function finValidacion() {

            var valChck = 0;
            var txtObservaciones = $("#txtObservacion").val();

            if (txtObservaciones == null) {

                txtObservaciones = "";
            }

            $("#SN_Indexacion").find(function () {

                if ($('input[name="indxSN"]').is(':checked')) {
                    valChck = 1
                }
            });

            console.log(_negId);

            transact.ajaxGET("/Dactiloscopia/AutoCausal?_negId=" + _negId + "&_snIndx=" + valChck + "&_observaciones=" + txtObservaciones, null,
                function (data) {
                    console.log(data, "data");
                    if (data == 1) {
                        bootbox.dialog({
                            message: "<h4>Desea finalizar la Validación Dactiloscopia para el Negocio:" + <%=ViewData["_negId"]%> + "</h4>",
                            title: "Confirmación",
                            buttons: {
                                succes: {
                                    label: ".: SI :.",
                                    className: "btn-success",
                                    callback: function () {
                                        transact.ajaxPOST("/Dactiloscopia/finValidacion", null,
                                            function () {
                                                sigNeg();
                                                var _neg_vis_b = '<%=Session["_neg_"] = ""%>';
                                            },
                                function (error) { console.log(error); });

                                    }
                                },
                                danger: {
                                    label: ".: NO :.",
                                    className: "btn-danger",
                                    callback: function () {
                                        window.location.href = '/Dactiloscopia/Index';
                                    }
                                }
                            }
                        });
                        } else if ((data == 2)) {
                            Alternar(alerta1);
                        } else if ((data == 3)) {
                            Alternar(alerta3);
                        }
                },
                function (error) {
                    console.log(error);
                });

            // funcion que indica si se quiere pasar al siguiente negocio o no
                    function sigNeg() {
                        bootbox.dialog({
                            message: "<h4>¿Continuar con el siguiente negocio?</h4>",
                            title: "Confirmación",
                            buttons: {
                                success: {
                                    label: ":: Continuar ::",
                                    className: "btn-success",
                                    callback: function () {
                                        window.location.href = '/Dactiloscopia/Index';
                                    }
                                },
                                danger: {
                                    label: ":: Cancelar ::",
                                    className: "btn-danger",
                                    callback: function () {
                                        window.location.href = '/Home/Index';
                                    }
                                }
                            }
                        });
                    }


                    function formularioEnvio(cud, NegId) {
                        bootbox.dialog({
                            title: "<b>Fin Validación</b>",
                            message: "<h4>Validación del Negocio: <b>" + cud + "</b></h4>" +
                                        "<form action='/Solicitud/generarSolicitud' method='post'>" +
                                        "<input type='hidden' id='NegId' name='NegId' value='" + NegId + "' />" +
                                        "<input type='hidden' id='idCUD' name='idCUD' value='" + cud + "' />" +
                                        "<%= ViewData["_camposEnvio"] %>" +
                            "<hr>" +
                            "<input type='submit' class='btn btn-login' id='envio' value=':: Enviar ::' />" +
                            "</form>"
                        });
                    }

                }

                // funcion la cual crea el pop pu con la url del la vista la cual va a cargar y internamente se configura el tamaño de ka ventana y el nombre
                // amenos que ya lo tenga en  el html de la vista, luego un string el cuan no se modifica y por ultimo es el style que tendra la pagina desde 
                // el tamaño que tendra hasta el fondo pero normalmente solo se configura el tamaño de resto se hace por css en la vista
                // creado el 17/03/2016 William Eduardo Cicua

                function popitup(url) {

                    transact.ajaxPOST("/Dactiloscopia/validarUsuario", null,
                        function (data) {
                            console.log(data, "data");
                            if (data == 0) {
                                newwindow = window.open(url, 'name', 'width=550,height=800,scrollbars=YES,left=400');

                                if (window.focus) { newwindow.focus() }
                                return false;
                            } if (data == 2) {
                                window.locationf = "../Seguridad/Login";
                            }
                        }, function (error) { console.log(error) });

                    //newwindow = window.open(url, 'name', 'width=550,height=1000,scrollbars=YES,left=400');

                    //if (window.focus) { newwindow.focus() }
                    //return false;
                }


                //funcion que limita los 15 segundos que duran los strong
                function Alternar(Seccion) {
                    if (Seccion.style.display == "none") {
                        Seccion.style.display = ""
                        window.setTimeout(function () {
                            $(".alert-danger").fadeTo(500, 0).slideUp(500, function () {
                                $(this).removeAttr("style");
                                $(this).css("display", "none");
                            });
                        }, 3000);
                    }
                }
                function ActualizarDataTable() {
                    var a;
                    transact.ajaxPOST("/Dactiloscopia/getDatatable?_negId=" + _negId, null,
                       function (data) {
                           $("#causales").html(data);

                       },
                       function (error) { console.log(error) });
                }

    </script>
</asp:Content>
