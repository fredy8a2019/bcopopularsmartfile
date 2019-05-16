<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    .::Documentos Cargados::.
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>

    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../../Content/Scripts/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.button.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.menu.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.tooltip.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.datepicker.js" type="text/javascript"></script>

    <script src="../../Scripts/jquery.maskedinput.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput.min.js" type="text/javascript"></script>

    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <script src="../../Scripts/Reestricciones.js" type="text/javascript"></script>

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
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <div id="divContenerdor">
        <hr />
        <h3>Documentos Cargados.
        </h3>
        <br />
        <fieldset id="DatosPrinciples" class="scheduler-border">

            <legend class="scheduler-border">
                <%--<input type="checkbox" aria-label="...">--%>
                Filtros Principales</legend>
            <div class="row">
                <div class="col-lg-1" style="margin: 5px 0px 15px 15px;">
                    <span class="lblMedio"  id="lbNCodConsultor" ><b>Negocio:</b></span>
                </div>
                <div class="col-lg-4" style="margin: 0px 0px 0px 0px;">
                    <input type="text" class="form-control" id="txtnegocio" pattern="^[0-9]{0,100}$"  />                    
                </div>
            </div>

            <div class="row">
                <div class="col-lg-1" style="margin: 10px 0px 0px 10px;">
                    
                </div>
                <div class="col-lg-3" style="margin: 10px 0px 5px 5px;">
                    <button class="btn btn-login" style="width: auto;" id="btnBuscar" onclick="CrearReporte()">
                        <span class="glyphicon glyphicon-search" aria-hidden="true"></span>&nbsp;Buscar
                    </button>
                    <button class="btn btn-login" style="width: auto;" id="btnLimpiar" onclick="Limpiar()">
                        <span class="glyphicon glyphicon-trash" aria-hidden="true"></span>&nbsp;Limpiar
                    </button>
                </div>
            </div>
        </fieldset>
        <br />
        <br />
        <fieldset id="ListaEvaluaciones" class="scheduler-border">
            <legend class="scheduler-border">Listado de Evaluaciones</legend>
            <div id="tablaConsulta" style="width: 100%; overflow-x: scroll" >
                <%= Html.Telerik().StyleSheetRegistrar() 
                                                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
                <%= Html.Telerik().Grid<GestorDocumental.Models.sp_RepDocCar_ObtenerNegocios_Result>()
                                                    .Name("GridConsulta")
                                                    //.Filterable()
                                                    .Sortable()
                                                    .Localizable("es-ES")                                                    
                                                    .DataBinding(databinding => databinding.Ajax().Select("_consultaRadicaciones", "ReporteDocumentosCargados"))
                                                    .Columns(colums =>
                                                    {
                                                        colums.Bound(o => o.negId).HtmlAttributes(new { id = "IdNeg", style = "display:none" }).HeaderHtmlAttributes(new { style = "display:none" });
                                                        
                                                        colums.Bound(o => o.negId).Width(25).Title("Negocio");
                                                        colums.Bound(o => o.numeroBizagi).Width(25).Title("Numero Bizagi");

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

        <div class="modalLocal style-3" id="divRadicacion" hidden="hidden">
            <%--<div class="modalLocal style-3" id="divRadicacion">--%>
            <div class="modal-dialog">
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

    </div>

    <script type="text/javascript">

        $(document).ready(function () {

            //CrearReporte();
        });

        function _CargaListaConsul(data) {
            _ui.fillCombo($("#SelCodconsultor"), data);
            $("#SelCodconsultor" + " " + "option[value='-1']").remove();
        }

        function _CargaListaConsulError() {

        }

        $("#btFechaFin").on("click", function () {
            _ui.showDatepicker($("#txtFechaFin"));
        });

        $("#btFechaInicial").on("click", function () {
            _ui.showDatepicker($("#txtFechaInicial"));
        });

        function onComplete(e) {
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

    function CerrarModal() {
        $("#divRadicacion").attr('hidden', 'hidden');
        $("#tableContent").html("");
        $("#tableContentValores").html("");

        $("#attachments").parents(".t-upload").find(".t-upload-files").remove()
    }

    function CrearReporte() {

        var negocio = document.getElementById("txtnegocio").value;

        transact.ajaxPOST("/ReporteDocumentosCargados/CrearReporte?negocio=" + negocio, null,
                function (data) {
                    if (data == 1) {
                        $(".t-grid .t-refresh").trigger('click');
                    }
                }, function (error) { console.log(error) });
    }

    function Activar(a, b) {
        if (document.getElementById(b).checked == true) {
            $("#" + a).removeAttr("disabled");
        } else {
            $("#" + a).attr("disabled", "disabled");
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
    function habilitaCampo(id, campo) {

        if ($('#' + id).is(':checked')) {
            $('#' + campo).removeAttr('disabled');

            document.getElementById(campo).style.zIndex = 0;

        }
        else {
            $('#' + campo).attr('disabled', 'disabled');
            document.getElementById(campo).style.zIndex = -1;
        }
    }
    function Limpiar() {
        document.getElementById("txtnegocio").value = '';


        CrearReporte();
    }

    $(document).ready(function () {

        setTimeout("inicioPDF1()", 4000);

    });

    function inicioPDF1() {

        document.getElementById('Iframe3').contentWindow.scaleInicial('auto');

    }

    var neg;
    neg = '';

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
        document.getElementById("txtnegocio").style.zIndex = 0;


        transact.ajaxPOST("/ReporteDocumentosCargados/ActualizaNegocio?idNeg=" + idNeg, null,
            function (data) {
                if (data == 1) {
                    $(".t-grid .t-refresh").trigger('click');
                }
            }, function (error) { console.log(error) });

        RutaNegocio(idNeg);

        neg = '../../../Content/ArchivosCliente/' + idNeg + '/' + idNeg + '.pdf';


        $("#divRadicacion").removeAttr('hidden');

        $('#Iframe3')[0].contentWindow.location.reload(true);
        //window.setTimeout(function () { validarArchivo('Iframe3') }, 2000);
    }

    </script>

</asp:Content>
