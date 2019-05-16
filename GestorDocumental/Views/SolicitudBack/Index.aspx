<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="../../Styles/jquery-ui.css" />
    <script type="text/javascript" src="../../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.min.js"></script>
    <link href="../../Content/themes/controls.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>

    <style type="text/css">
        .divleft {
            float: left;
            display: inline;
            width: 27%;
        }

        legend.scheduler-border {
            font-size: 1.2em !important;
            font-weight: bold !important;
            text-align: left !important;
        }
    </style>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Entrega y Devolución de documentos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        $.noConflict();
        var _default = null;
        (function ($) {
            _default = {
                countRow: 1,
                _loadPage: function () {
                    $(document).ready(function () {
                        $('[data-toggle="tooltip"]').tooltip({
                            placement: 'top'
                        });
                    });
                },
            }
            _default._loadPage();
        })(jQuery);

        function confirmar(e) {
            var notiquet = e.dataItem["_noTiquet"];
            var negId = e.dataItem["_negId"];
            bootbox.dialog({
                message: "<h4>¿Confirmar la entrega del documento?</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            formularioEntrega(notiquet, negId);
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });
        }

        function recibir(e) {
            var negId = e.dataItem["_negId"];
            var array = [];

            if (negId == "") {
                var noTiquet = e.dataItem["_noTiquet"];
                var totalRegistros = $("#" + noTiquet).data('tGrid').total;

                for (var i = 0; i < totalRegistros; i++) {
                    array.push($("#" + noTiquet).data('tGrid').data[i]._negId);
                }
            }
            else {
                array.push(negId);
            }

            bootbox.dialog({
                message: "<h4>¿Confirmar el recibido?</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/SolicitudBack/confirmarDevolucion?negId=" + array, null,
                                function () {
                                    mensajeConfirmacion();
                                },
                                function (error) { console.log(error); });
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });
        }

        function formularioEntrega(noTiquet, negId) {
            if (negId == "")
                negId = 0;
            transact.ajaxGET("/SolicitudBack/obtenerCamposSolicitud?negId=" + negId, null,
                function (data) {
                    bootbox.dialog({
                        title: "<b>Formato de entrega del documento</b>",
                        message: "<h4>Entrega del documento</h4>" +
                                    "<form action='/SolicitudBack/actualizarFechaEntrega' method='post'>" +
                                    "<input type='hidden' id='noTiquet' name='noTiquet' value='" + noTiquet + "' />" +
                                    data + "<hr>" + 
                                    "<table><tr><td>" +
                                    "<b>Dias en prestamo</b>" +
                                    "</td></tr>" + "<tr><td>" +
                                    "<input type='text' id='txtDias' name='txtDias' required class='form-control' />" +
                                    "</td></tr></table>" +
                                    "<hr>" +
                                    "<input type='submit' class='btn btn-login' id='envio' value=':: Enviar ::' />" +
                                    "</form>"
                    });
                },
                function (error) { console.log(error) })
        }

        function mensajeConfirmacion() {
            bootbox.dialog({
                message: "<h4>Documento recibido correctamente!</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Aceptar ::",
                        className: "btn btn-login",
                        callback: function () {
                            window.location.href = "/SolicitudBack/Index";
                        }
                    }
                }
            });
        }

    </script>


    <h2>Entrega y Devolución de documentos</h2>
    <ul class="nav nav-tabs">
        <li class="active"><a data-toggle="tab" href="#home">Entrega de documentos</a></li>
        <li><a data-toggle="tab" href="#menu1">Devolución de documentos</a></li>
    </ul>

    <div class="tab-content">
        <div id="home" class="tab-pane fade in active">
            <br />
            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Documentos con solicitudes pendientes</legend>
                <%= Html.Telerik().StyleSheetRegistrar()
                    .DefaultGroup(group => group.Add("telerik.hay.css")) %>
                <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_SolicitudesPendientes>()
                        .Name("GrillaSolicitudesPendientes")
                        .Columns(columns =>
                        {
                            columns.Bound(o => o._noTiquet).Width(100).Title("No. Tiquet").HeaderHtmlAttributes(new { style = "height: 40px" });
                            columns.Bound(o => o._negId).Width(100).Title("No. de Negocio");
                            columns.Bound(o => o._documentoSolicitado).Width(100).Title("Documento Solicitado");
                            columns.Bound(o => o._fechaSolicitud).Format("{0:F}").Width(100).Title("Fecha de Solicitud");
                            columns.Bound(o => o._usuario).Width(100).Title("Usuario que solicito");
                            columns.Command(o => o.Custom("Entregar").Text("Entregar")
                                .SendState(false)
                                .Ajax(true)).Width(100);
                        })

                        .DetailView(details => details.ClientTemplate(
                            Html.Telerik().Grid<GestorDocumental.Controllers.grilla_SolicitudesPendientesDetalle>()
                            .Name("<#= _noTiquet #>")
                            .Columns(colums =>
                            {
                                colums.Bound(a => a._negId).Width(100).Title("Negocio").HeaderHtmlAttributes(new { style = "height: 40px" });
                                colums.Bound(a => a._noDocumento).Width(100).Title("Documento Solicitado");
                            })
                            .DataBinding(databinding => databinding.Ajax()
                            .Select("getInformacionDetalle", "SolicitudBack", new { noTiquet = "<#= _noTiquet #>" }))
                        .Pageable(pager => pager.PageSize(10, new int[] { 10,25,50 })
                            .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
                        .Sortable()
                        .Filterable()
                        .ToHtmlString()))

                    .DataBinding(d => d.Ajax().Select("getInformacionSPendientes", "SolicitudBack"))
                    .Pageable(paginas => paginas.PageSize(20))
                    .Filterable()
                    .ClientEvents(e => e.OnCommand("confirmar"))
                    .ToHtmlString()
                %>
            </fieldset>
        </div>
        <div id="menu1" class="tab-pane fade">
            <br />
            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Documentos prestados</legend>
                <%= Html.Telerik().StyleSheetRegistrar()
                    .DefaultGroup(group => group.Add("telerik.hay.css")) %>
                <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_SolicitudesDevolucion>()
                        .Name("GrillaSolicitudesDevolucion")
                        .Columns(columns => {
                            columns.Bound(o => o._noTiquet).Width(100).Title("No. Tiquet").HeaderHtmlAttributes(new { style = "height: 40px" });
                            columns.Bound(o => o._negId).Width(100).Title("No. de Negocio");
                            columns.Bound(o => o._documentoSolicitado).Width(100).Title("Documento Solicitado");
                            columns.Bound(o => o._fechaSolicitud).Format("{0:F}").Width(100).Title("Fecha límite Devolución");
                            columns.Bound(o => o._usuario).Width(100).Title("Usuario que solicitó");
                            columns.Bound(o => o._semaforo)
                                .ClientTemplate("<img src='<#= _semaforo #>'/>")
                                .Title("")
                                .Width(100)
                                .HtmlAttributes(new { style = "text-align:center" });
                                
                            columns.Command(o => o.Custom("ConfirmarRecibido").Text("Recibir")
                                .SendState(false)
                                .Ajax(true)).Width(100);
                        })
                        
                        .DetailView(details => details.ClientTemplate(
                            Html.Telerik().Grid<GestorDocumental.Controllers.grilla_SolicitudesPendientesDetalle>()
                            .Name("<#= _noTiquet #>")
                            .Columns(colums =>
                            {
                                colums.Bound(a => a._negId).Width(100).Title("Negocio").HeaderHtmlAttributes(new { style = "height: 40px" });
                                colums.Bound(a => a._noDocumento).Width(100).Title("Documento Solicitado");
                            })
                            .DataBinding(databinding => databinding.Ajax()
                            .Select("getInformacionDetalle", "SolicitudBack", new { noTiquet = "<#= _noTiquet #>" }))
                        .Pageable(pager => pager.PageSize(10, new int[] { 10,25,50 })
                            .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
                        .Sortable()
                        .Filterable()
                        .ToHtmlString()))
                        
                    .DataBinding(d => d.Ajax().Select("getInformacionSDevolucion", "SolicitudBack"))
                    .Pageable(paginas => paginas.PageSize(20))
                    .Filterable()
                    .ClientEvents(e => e.OnCommand("recibir"))
                    .ToHtmlString()
                %>
            </fieldset>
        </div>
    </div>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
</asp:Content>
