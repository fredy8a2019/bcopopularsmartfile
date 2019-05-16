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
    <%-- <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />--%>


    <script src="../../Scripts/jquery.maskedinput.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput.min.js" type="text/javascript"></script>

    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <script src="../../Scripts/Reestricciones.js" type="text/javascript"></script>

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

    
    

    <%--<link rel="stylesheet" href="../../Styles/menu-6.css" type="text/css" />
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>--%>

    <style type="text/css">
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

        .t-header {
            background-color: #205390;
            color: white;
            height: 30px;
        }

            .t-header .t-link {
                color: white;
                height: 100%;
                width: 100%;
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

        #mdialTamanio {
            width: 80% !important;
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
</asp:Content>

<%--javascript y HTML MainContent--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">


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

    <!-- Creado el 23/10/2017 Ivan Rene Rodriguez Bedoya-->
    <br />
    <br />

    <table style="width: 120%">
        <tr>
            <td>
                <h3>Validación Dactiloscopia</h3>
            </td>

        </tr>
    </table>


    <div id="zoom" style="overflow: scroll;" hidden="hidden" onmousemove="moverScrolls(event)"></div>
    <br />
    <div class="tab-content">
        <div class="row">
            <div class="col-lg-7">

                <%=ViewData["CausalesGeneradas"]%>
            </div>
            <div class="col-lg-5">
                <fieldset class="scheduler-border">
                    <legend class="scheduler-border" style="font-weight: bolder; font-size: large;">Informacion Cliente</legend>
                    <div id="tablaDatosCliente" style="width: 100%;">
                        <%=ViewData["CamposTablaGenerados"]%>
                    </div>

                </fieldset>

            </div>
        </div>

        <div class="row">
            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Historial Casos</legend>
                <div id="CausalesDev" style="width: 100%; overflow-x: scroll">
                    <%= Html.Telerik().StyleSheetRegistrar() 
                                                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
                    <%= Html.Telerik().Grid<GestorDocumental.Models.spValDoc_ConsultaCausales_Result>()
                                                    .Name("TablaCausales")
                                                    //.Filterable()
                                                    .Sortable()
                                                    .Localizable("es-ES")                                                    
                                                    .DataBinding(databinding => databinding.Ajax().Select("ActualizarCausales", "Dactiloscopia2"))
                                                    .Columns(colums =>
                                                    {
                                                        colums.Bound(o => o.DocDescripcion).Title("Documento Validado").HeaderHtmlAttributes(new { style = "center: 40px; height: 30px; " });
                                                        colums.Bound(o => o.nom_causal).Title("Política");
                                                        colums.Bound(o => o.desc_causal).Title("Descripción Política");
                                                        colums.Bound(o => o.fec_validacion).Format("{0:F}").Title("Fecha Validación");
                                                    })
                                                    .Pageable()
                                                    .TableHtmlAttributes(new {id="MyGrid2"})
                    %>
                </div>

            </fieldset>

        </div>


        <div class="row">
            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Historial Casos</legend>
                <div id="tablaConsulta" style="width: 100%; overflow-x: scroll">
                    <%= Html.Telerik().StyleSheetRegistrar() 
                                                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
                    <%= Html.Telerik().Grid<GestorDocumental.Models.sp_ValDac2_HistoricoCedula_Result>()
                                                    .Name("TablaHistorico")
                                                    //.Filterable()
                                                    .Sortable()
                                                    .Localizable("es-ES")                                                    
                                                    .DataBinding(databinding => databinding.Ajax().Select("_consultaHistorico", "Dactiloscopia2"))
                                                    .Columns(colums =>
                                                    {
                                                        colums.Bound(o => o.negId).HtmlAttributes(new { id = "IdNeg", style = "display:none" }).HeaderHtmlAttributes(new { style = "display:none" });
                                                        
                                                        colums.Bound(o => o.negId).Width(25).Title("Negocio").HeaderHtmlAttributes(new { style = " height: 25px" });;
                                                        colums.Bound(o => o.noIdentificacion).Width(25).Title("C.C.");
                                                        colums.Bound(o => o.nroBizagi).Width(25).Title("Radicado");
                                                        colums.Bound(o => o.resultado).Width(25).Title("Resultado");
                                                        colums.Bound(o => o.observacion).Width(25).Title("Observación");
                                                        colums.Command(o => o.Custom("PDF").Text("<span class=\"glyphicon glyphicon-file\" aria-hidden=\"true\"></span><span id=\"ocultar\">&nbsp;Archivo</span>")
                                                            .HtmlAttributes(new { onclick = "mostrarModalNegocio(this)"})
                                                            .SendState(true)
                                                            .DataRouteValues(route =>
                                                            {
                                                                route.Add(x => x.negId).RouteKey("idNeg");
                                                                //route.Add(x => x.idRadicacion).RouteKey("idRadicacion");
                                                            }))
                                                            .Title("")
                                                            .Width(25);
                                                    })
                                                    .Pageable()
                                                    .ClientEvents(events => events.OnDataBound("onComplete"))
                                                    .TableHtmlAttributes(new {id="MyGrid"})
                    %>
                </div>

            </fieldset>

        </div>
        <div class="row">
            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Observaciones</legend>
                <input type="text" style="width: 95%" id="txtObservacion" />
            </fieldset>
        </div>
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

                <input type="button" value="Finalizar" class="btn btn-login" style="width: 87.8px;" onclick="finValidacion()" />
                <%-- el siguiente boton llama al pop up en el cual se verifican los datos especificos para veridicar cual doccumento se
                    encuentra incorrecto Creado el 17/03/2016 William Eduardo Cicua--%>

                <input type="reset" name="btnRestablecer" id="btnRestablecer" value="Validar" class="btn btn-login" style="width: auto" <%=ViewData["URLDacti"]%> />
                <input type="button" value="Reiniciar" class="btn btn-login" style="width: 87.8px; margin-left: 75%;" onclick="reiniciarCausales()" />
            </form>
        </div>

    </div>
    <div class="modalLocal style-3" id="divRadicacion" hidden="hidden">
        <%--<div class="modalLocal style-3" id="divRadicacion">--%>
        <div class="modal-dialog" id="mdialTamanio">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="CerrarModal()">&times;</button>
                    <h3 class="modal-title">Carpeta</h3>
                </div>
                <div class="modal-body">
                    <div>
                        <form action="/CargaDocumental/Guardar" method="post">
                            <fieldset id="Contenido" class="scheduler-border" style="margin-top: 30px;">
                                <%--<legend class="scheduler-border">Documento</legend>--%>

                                <fieldset style="width: 100%; height: 100%">
                                    <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframe3" allowfullscreen></iframe>
                                </fieldset>

                            </fieldset>
                        </form>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var neg;
        neg = '<%=Session["_negId_"]%>';

        $(document).ready(function () {
            jsShowWindowLoadDac('');
            refrescaGrilla();
            setTimeout("jsRemoveWindowLoadDac()", 3000);
        });

        function jsRemoveWindowLoadDac() {
            // eliminamos el div que bloquea pantalla
            $("#WindowLoad").remove();

        }

        function jsShowWindowLoadDac(mensaje) {
            //eliminamos si existe un div ya bloqueando
            jsRemoveWindowLoadDac();

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


        // funcion la cual guenera los campos para realizar las validaciones de si se an validado 
        // o no ya que si esto no se realiza nos se puede validar
        function refrescaGrilla() {
            setTimeout("inicioPDF1()", 4000);
            $("#TablaHistorico.t-grid .t-refresh").trigger('click');
        }

        function refrescaGrillaCausales() {

            $("#TablaCausales.t-grid .t-refresh").trigger('click');
        }

        function CausalesImagen(docId) {
            var valLst;
            var idSelect;
            var nameSelect;
            var contador = 0;
            var validacion = 0;
            var _negId = '<%=ViewData["_negId"]%>';

            $("#Causales" + docId + " :input").each(function () {
                idSelect = $(this).attr("id");
                valLst = $("#" + idSelect + " option:selected").val();

                if (valLst == -1) {

                    validacion = 1;

                    alert("Validar todas las causales");
                    return false;
                }

            });
            if (validacion == 0) {
                $("#Causales" + docId + " :input").each(function () {
                    //se obtiene el name del select 
                    idSelect = $(this).attr("id");
                    nameSelect = document.getElementById(idSelect).name;

                    valLst = $("#" + idSelect + " option:selected").val();
                    //valLst = $(this).val();
                    var _snCausal = valLst;

                    transact.ajaxPOST("/Dactiloscopia2/CausalesEspecificas?_campId=" + nameSelect + "&_snCausal=" + _snCausal + "&_negId=" + _negId + "&doc=" + docId, null,
                    function () {
                        refrescaGrillaCausales();
                    },
                    function (error) { console.log(error) });
                });

                refrescaGrillaCausales();

            }

        }

        function onComplete(e) {
            setTimeout("inicioPDF1()", 4000);
            $(".t-button").removeAttr('href');
            var h = $("#MyGrid").outerWidth();

            console.log(<%=ViewData["MensajeError_"]%>)
        }
        function mostrarModalNegocio(e) {

            var columna = $(e).parent();
            var fila = $(columna).parent();

            idNeg = fila.children("#IdNeg").text();
            muestraRadicacion(idNeg)

            //window.open('../Contabilizar/index?negId=' + idNeg, 'toolbar=no,location=no,menubar=no,directories=no,channelmode=0,titlebar=no,addressbar=0,scrollbars=yes,resizable=yes,width=1000,height=800')();
        }


        function inicioPDF1() {

            document.getElementById('Iframe3').contentWindow.scaleInicial(1.5);

        }

        function CerrarModal() {
            $("#divRadicacion").attr('hidden', 'hidden');
            $("#tableContent").html("");
            $("#tableContentValores").html("");

            $("#attachments").parents(".t-upload").find(".t-upload-files").remove()
        }

        function RutaNegocio() {
            if (neg != '') {
                return neg;

            }
            else {
                return '';
            }
        }

        function muestraRadicacion(idNeg) {
            //oculataPreLoad();

            transact.ajaxPOST("/Dactiloscopia2/ActualizaNegocio?idNeg=" + idNeg, null,
                function (data) {
                    if (data == 1) {
                        $(".t-grid .t-refresh").trigger('click');
                    }
                }, function (error) { console.log(error) });

            neg = '../../../Content/ArchivosCliente/' + idNeg + '/' + idNeg + '.pdf';

            RutaNegocio(neg);

            $("#divRadicacion").removeAttr('hidden');

            $('#Iframe3')[0].contentWindow.location.reload(true);
            document.getElementById('Iframe3').contentWindow.scaleInicial(1.5);
            //window.setTimeout(function () { validarArchivo('Iframe3') }, 2000);
        }


        function popitup(url) {

            transact.ajaxPOST("/Dactiloscopia2/validarUsuario", null,
                function (data) {
                    console.log(data, "data");
                    if (data == 0) {
                        newwindow = window.open(url, 'name', 'width=1500,height=700,scrollbars=YES,left=400');

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

            transact.ajaxGET("/Dactiloscopia2/ValidarTotalCausales?_observaciones=" + txtObservaciones, null,
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
                                        transact.ajaxPOST("/Dactiloscopia2/finValidacion", null,
                                            function () {
                                                sigNeg();
                                            },
                                function (error) { console.log(error); });
                                    }
                                },
                                danger: {
                                    label: ".: NO :.",
                                    className: "btn-danger",
                                    callback: function () {
                                        window.location.href = '/Dactiloscopia2/Dactiloscopia2';
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
                                    window.location.href = '/Dactiloscopia2/Dactiloscopia2';
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
            }

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

            function reiniciarCausales() {
             
                transact.ajaxPOST("/Dactiloscopia2/reiniciarCausales?_negId=" + neg, null,
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
                                        callback: function () {
                                            window.location.href = '/Dactiloscopia2/Dactiloscopia2';
                                        }
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

    </script>
</asp:Content>
