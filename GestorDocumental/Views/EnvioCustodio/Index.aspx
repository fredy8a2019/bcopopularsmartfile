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

            .span:hover {
                background-color: rgb(250, 250, 250) !important;
            }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Envio a Custodio Final
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
                        _ui.eventClick($("#btnConfirmar"), _default.btnBuscarClick);
                    });
                },

                btnBuscarClick: function () {
                    //Validamos que al grilla de envio a custodio tenga al menos un documento
                    var totalRegistros = $("#GrillaSolicitudDocumentosContenido").find(".t-no-data").length;
                    if (totalRegistros == 1) {
                        bootbox.dialog({
                            message: "<h4>Aun no se ha agregado ningun registro</h4>",
                            title: "<b>Alerta</b>",
                            buttons: {
                                success: {
                                    label: ":: Aceptar ::",
                                    className: "btn-danger"
                                }
                            }
                        });
                    } else {
                        var _guia = $("#txtGuia").val();
                        var _currier = $("#txtCurrier").val();
                        var _precinto = $("#txtPrecinto").val();
                        var _idEnvioDisponible = $("#txtIdEnvio").val();

                        bootbox.dialog({
                            message: "<h4>¿Confirmar el envio de los documentos seleccionados?</h4>",
                            title: "<b>Confirmación</b>",
                            buttons: {
                                success: {
                                    label: ":: Si ::",
                                    className: "btn btn-login",
                                    callback: function () {
                                        transact.ajaxPOST("/EnvioCustodio/ConfirmarEnvioFinal?guia=" + _guia + "&currier=" + _currier + "&precinto=" + _precinto + "&idEnvioFinal=" + _idEnvioDisponible, null, _default.successbtnBuscar, _default.error);
                                    }
                                },
                                danger: {
                                    label: ":: No ::",
                                    className: "btn-danger"
                                }
                            }
                        });
                    }
                },

                successbtnBuscar: function () {
                    bootbox.dialog({
                        message: "<h4>Envio realizado correctamente</h4>",
                        title: "<b>Confirmación</b>",
                        buttons: {
                            success: {
                                label: ":: Aceptar ::",
                                className: "btn btn-login",
                                callback: function () {
                                    window.location.href = "/EnvioCustodio/Index";
                                }
                            }
                        }
                    });
                },

                error: function (error) {
                    console.log(error);
                }
            }
            _default._loadPage();
        })(jQuery);

        function agregarNegocio(e) {
            var _negId = e.dataItem["_negId"];
            var _idEnvioDisponible = $("#txtIdEnvio").val();

            bootbox.dialog({
                message: "<h4>Agregar el documento con el número de Negocio: <b>" + _negId + "</b></h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/EnvioCustodio/agregarNegocioEnvio?idEnvio=" + _idEnvioDisponible + "&negId=" + _negId, null,
                                function () { actualizarGrillas(); }, function (error) { console.error(error); });
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });
        }

        function eliminarNegocio(e) {
            var _negId = e.dataItem["_negId"];
            var _idEnvioDisponible = $("#txtIdEnvio").val();

            bootbox.dialog({
                message: "<h4>Eliminar el Negocio: <b>" + _negId + "</b></h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/EnvioCustodio/eliminarNegocioEnvio?idEnvio=" + _idEnvioDisponible + "&negId=" + _negId, null,
                                function () { actualizarGrillas(); }, function (error) { console.error(error); });
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });
        }

        function actualizarGrillas() {
            $("#GrillaSolicitudDocumentos.t-grid .t-refresh").trigger('click');
            $("#GrillaSolicitudDocumentosContenido.t-grid .t-refresh").trigger('click');
        }

    </script>

    <h1>Envio a Custodio</h1>
    <fieldset class="scheduler-border">
        <legend class="scheduler-border">Documentos listos para envio final</legend>

        <%= Html.Telerik().StyleSheetRegistrar()
                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
        <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_EnvioFinal>()
                .Name("GrillaSolicitudDocumentos")
                .Columns(columns => {
                    columns.Bound(o => o._negId).Width(100).Title("Negocio").HeaderHtmlAttributes(new { style = "height: 40px" });
                    columns.Bound(o => o._producto).Width(100).Title("Producto");
                    columns.Bound(o => o._subProducto).Width(100).Title("Sub Producto");
                    columns.Bound(o => o._codBarras).Width(100).Title("Codigo de Barras");
                    columns.Bound(o => o._campoUno).Width(100).Title(ViewData["CAMPO_1"].ToString());
                    columns.Bound(o => o._campoDos).Width(100).Title(ViewData["CAMPO_2"].ToString());
                    columns.Bound(o => o._campoTres).Width(100).Title(ViewData["CAMPO_3"].ToString());
                    columns.Command(o => o.Custom("Agregar").Text("Agregar")
                        .SendState(false)
                        .Ajax(true)).Width(100);
                })
                .DataBinding(d => d.Ajax().Select("getInformacionEnvio", "EnvioCustodio"))
                .Pageable(pager => pager.PageSize(10, new int[] { 10,25,50 })
                        .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
                .ClientEvents(e => e.OnCommand("agregarNegocio"))
                .Filterable()
                .ToHtmlString()
        %>
    </fieldset>

    <fieldset class="scheduler-border">
        <legend class="scheduler-border">Contenido guardado para envio a custodio</legend>

        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Información adicional para envio</legend>
            <div class="divleft">
                <label>Guia</label>
                <input type="text" id="txtGuia" name="txtGuia" value="" class="form-control" />
            </div>
            <div class="divleft">
                <label>Currier</label>
                <input type="text" id="txtCurrier" name="txtCurrier" value="" class="form-control" />
            </div>
            <div class="divleft">
                <label>Precinto</label>
                <input type="text" id="txtPrecinto" name="txtPrecinto" value="" class="form-control" />
            </div>
        </fieldset>

        <input type="hidden" id="txtIdEnvio" name="txtIdEnvio" value="<%= ViewData["_IdEnvioDisponible"] %>" class="form-control" />

        <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_EnvioFinal>()
                .Name("GrillaSolicitudDocumentosContenido")
                .Columns(columns => {
                    columns.Bound(o => o._negId).Width(100).Title("Negocio").HeaderHtmlAttributes(new { style = "height: 40px" });
                    columns.Bound(o => o._producto).Width(100).Title("Producto");
                    columns.Bound(o => o._subProducto).Width(100).Title("Sub Producto");
                    columns.Bound(o => o._codBarras).Width(100).Title("Codigo de Barras");
                    columns.Bound(o => o._campoUno).Width(100).Title(ViewData["CAMPO_1"].ToString());
                    columns.Bound(o => o._campoDos).Width(100).Title(ViewData["CAMPO_2"].ToString());
                    columns.Bound(o => o._campoTres).Width(100).Title(ViewData["CAMPO_3"].ToString());
                    columns.Command(o => o.Custom("Eliminar").Text("Eliminar")
                        .SendState(false)
                        .Ajax(true)).Width(100);
                })
                .DataBinding(d => d.Ajax().Select("getInformacionEnvioContenido", "EnvioCustodio"))
                .Pageable(pager => pager.PageSize(10, new int[] { 10,25,50 })
                        .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
                .ClientEvents(e => e.OnCommand("eliminarNegocio"))
                .Filterable()
                .ToHtmlString()
        %>
        <br />
        <input type="button" name="" value=":: Confirmar envio ::" class="btn btn-login" id="btnConfirmar" />
    </fieldset>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
</asp:Content>
