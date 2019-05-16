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
    Solicitud de Documentos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        function alerta(e) {
            var docSeleccionado = e.dataItem["_campoUno"];
            var negId = e.dataItem["_negId"];
            bootbox.dialog({
                message: "<h4>¿Confirmar la solicitud del documento?  " + "<b>" + docSeleccionado + "</b>" + "</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            formularioEnvio(docSeleccionado, negId);
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });
        }

        function formularioEnvio(cud, NegId) {
            bootbox.dialog({
                title: "<b>Formato de solicitud</b>",
                message: "<h4>Solicitud del documento: <b>" + cud + "</b></h4>" +
                            "<form action='/Solicitud/generarSolicitud' method='post'>" +
                            "<input type='hidden' id='NegId' name='NegId' value='" + NegId + "' />" +
                            "<input type='hidden' id='idCUD' name='idCUD' value='" + cud + "' />" +
                            "<%= ViewData["_camposEnvio"] %>" +
                            "<hr>" +
                            "<input type='submit' class='btn btn-login' id='envio' value=':: Enviar ::' />" +
                            "</form>"
            });
        }

        function completado() {
            bootbox.dialog({
                message: "<h4>Archivo subido correctamente",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Aceptar ::",
                        className: "btn btn-login",
                        callback: function () {
                            redireccion();
                        }
                    }
                }
            });
        }

        function redireccion() {
            $.ajax({
                type: "GET",
                url: "/Solicitud/obtenernoTiquet",
                dataType: "json",
                success: function (result) {
                    window.location.href = "/Solicitud/Confirmar?noTiquet=" + result;
                },
                error: function (result) {
                    console.log(result);
                }
            });
        }

        function refrescarGrillaSolicitud() {
            var valorFiltro = $("#txtfiltro").val();

            var link = "/Solicitud/getInformacionSolicitud?filtro=" + valorFiltro;
            $.ajax({
                type: "POST",
                url: link,
                data: {
                    dato: valorFiltro
                },
                dataType: "json",
                success: function (result) {
                    $("#GrillaSolicitudDocumentos").data("tGrid").dataBind(result.data);
                    if (result.total == 1) {
                        $("#panelGrilla").removeAttr("style");
                    }
                    else if (result.total == 0) {
                        $("#panelGrilla").css("display", "none");
                        bootbox.dialog({
                            message: "<h4>El documento no ha sido encontrado o no se encuentra disponible</h4>",
                            title: "<b>Alerta</b>",
                            buttons: {
                                success: {
                                    label: ":: Aceptar ::",
                                    className: "btn-danger"
                                }
                            }
                        });
                    }
                },
                error: function (result) {
                    alert(result.message);
                }
            });
        }

        $.noConflict();
        var _default = null;
        (function ($) {
            _default = {
                countRow: 1,
                _loadPage: function () {
                    $(document).ready(function () {
                        _ui.eventClick($("#btnBuscar"), _default.btnBuscarClick);
                    });
                },

                btnBuscarClick: function () {
                    refrescarGrillaSolicitud();
                }
            }
            _default._loadPage();
        })(jQuery);

    </script>

    <ul class="nav nav-tabs">
        <li class="active"><a data-toggle="tab" href="#home">Prestamo de documentos</a></li>
        <li><a data-toggle="tab" href="#menu1">Prestamo masivo de Documentos</a></li>
    </ul>

    <div class="tab-content">
        <div id="home" class="tab-pane fade in active">
            <br />
            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Solicitud de prestamo de documentos</legend>
                <div class="divleft">
                    <%= ViewData["CAMPO_1"].ToString() %>
                    <input type="text" id="txtfiltro" name="txtfiltro" class="form-control" />
                    <input type="button" class="btn btn-login" id="btnBuscar" onclick="" name="btnBuscar" value=":: Buscar ::" />
                </div>
            </fieldset>
            <%= Html.Telerik().StyleSheetRegistrar()
                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
            <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_SolicitudDocum>()
                .Name("GrillaSolicitudDocumentos")
                .Columns(columns => {
                    columns.Bound(o => o._negId).Width(100).Title("Negocio").HeaderHtmlAttributes(new { style = "height: 40px" });
                    columns.Bound(o => o._producto).Width(100).Title("Producto");
                    columns.Bound(o => o._subProducto).Width(100).Title("Sub Producto");
                    columns.Bound(o => o._codBarras).Width(100).Title("Codigo de Barras");
                    columns.Bound(o => o._campoUno).Width(100).Title(ViewData["CAMPO_1"].ToString());
                    columns.Bound(o => o._campoDos).Width(100).Title(ViewData["CAMPO_2"].ToString());
                    columns.Bound(o => o._campoTres).Width(100).Title(ViewData["CAMPO_3"].ToString());
                    columns.Command(o => o.Custom("Solicitar").Text("Solicitar")
                        .SendState(false)
                        .Ajax(true)).Width(100);
                })
                .ClientEvents(e => e.OnCommand("alerta"))
            %>
        </div>
        <div id="menu1" class="tab-pane fade">
            <br />
            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Prestamo masivo de documentos</legend>
                <%= Html.Telerik().Upload()
                        .Name("files")
                        .Multiple(false)
                        .Async(async => async
                            .Save("solicitudPrestamoMasivo", "Solicitud")
                            .AutoUpload(true))
                        .ClientEvents(events => events
                            .OnSuccess("completado"))
                %>
            </fieldset>
        </div>
    </div>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
</asp:Content>
