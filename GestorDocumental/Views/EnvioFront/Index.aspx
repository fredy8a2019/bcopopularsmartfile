<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
    Recepción Documental
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        //Para evitar que la grilla se LLene al momento en que la pagina se cargue
        var initialLoad = true;
        function Grid_onDataBinding(e) {
            if (initialLoad) {
                e.preventDefault();
                initialLoad = false;
            }
        }

        function refrescarGrillaRecepcion() {
            var cUD = $("#txtCUD").val();
            var nitCliente = "<%= Session["nit_cliente"] %>";

            if (cUD == "")
                cUD = 0;
            else
                cUD = $("#txtCUD").val();
            var link = "/EnvioFront/getInformacionRecepcion?cud=" + cUD + "&nitCliente=" + nitCliente;
            $.ajax({
                type: "POST",
                url: link,
                data: {
                    dato: cUD,
                    dato1: nitCliente
                },
                dataType: "json",
                success: function (result) {
                    $("#GrillaRecepcionPadre").data("tGrid").dataBind(result.data);
                    console.log(result.total);
                    if (result.total == 1) {
                        $("#panelGrilla").removeAttr("style");
                    }
                    else if (result.total == 0) {
                        $("#panelGrilla").css("display", "none");
                        bootbox.dialog({
                            message: "<h4>La unidad documental no se encuentra disponible</h4>",
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
            refrescarGrillaDocumentosProcesados();
        }

        function refrescarGrillaDocumentosProcesados() {
            var cUD = $("#txtCUD").val();
            if (cUD == "")
                cUD = 0;
            else
                cUD = $("#txtCUD").val();
            var link = "/EnvioFront/getGrillaDocumentosProcesados?CUD=" + cUD;
            $.ajax({
                type: "POST",
                url: link,
                data: {
                    dato: cUD
                },
                dataType: "json",
                success: function (result) {
                    $("#GrillaDocumentosProcesados").data("tGrid").dataBind(result.data);
                },
                error: function (result) {
                    alert(result.message);
                }
            });
        }

        //Funcion que trae los registros de la grilla de detalles en los lotes consultados.
        function prueba_detallesResultado(e) {
            var grid = $(this).data('tGrid');
            expandFirstRow(grid, e.row);
        }

        function expandFirstRow(grid, row) {
            if (grid.$rows().index(row) == 0) {
                grid.expandRow(row);
            }
        }

        function refrescaGrilla() {
            $("#GrillaRecepcionPadre.t-grid .t-refresh").trigger('click');
            refrescarGrillaDocumentosProcesados();
        }

        //Funcion para generar la planilla
        function printGrid() {
            var gridElement = $("#GrillaDocumentosProcesados"),
                printableContent,
                win = window.open('', '', 'width=800, height=500'),
                doc = win.document.open();

            var htmlStart =
                    '<!DOCTYPE html>' +
                    '<html>' +
                    '<head>' +
                    '<meta charset="utf-8" />' +
                    '<title>Planilla de datos</title>' +
                    '<style>' +
                    'table, th, td { border: 1px solid black; }' +
                    'html { font: 11pt sans-serif; }' +
                    '.t-grid { border-top-width: 0; }' +
                    '.t-grid, .t-grid-content { height: auto !important; }' +
                    '.t-grid-content { overflow: visible !important; }' +
                    'div.t-grid table { table-layout: auto; width: 100% !important; }' +
                    '.t-grid .t-grid-header th { border-top: 1px solid; }' +
                    '.t-grid-toolbar, .t-grid-pager > .t-link { display: none; }' +
                    '</style>' +
                    '</head>' +
                    '<body>';

            var htmlEnd =
                    '</body>' +
                    '</html>';

            var gridHeader = gridElement.children('.t-grid-header');
            if (gridHeader[0]) {
                var thead = gridHeader.find('thead').clone().addClass('t-grid-header');
                printableContent = gridElement
                    .clone()
                        .children('.t-grid-header').remove()
                    .end()
                        .children('.t-grid-content')
                            .find('table')
                                .first()
                                    .children('tbody').before(thead)
                                .end()
                            .end()
                        .end()
                    .end()[0].outerHTML;
            } else {
                printableContent = gridElement.clone();
                printableContent.find('th:last-child, td:last-child').remove();
                printableContent.find('a').remove();
                printableContent.find('input').remove();

                printableContent = printableContent.clone()[0].outerHTML;
            }

            doc.write(htmlStart + printableContent + htmlEnd);
            doc.close();
            win.print();
        }

        function eliminarDocumento(e) {
            var negID = e.dataItem["negId"];
            bootbox.dialog({
                message: "<h4>¿Desea eliminar el documento seleccionado?</h4>",
                title: "<b>Confirmar</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/EnvioFront/eliminarDocumentoRecepcionado?negID=" + negID,
                                null,
                                function () { refrescaGrilla(); },
                                function (error) { console.log(error)  });
                        }
                    },

                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
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
                        _ui.eventClick($("#btnProcesar"), _default.btnProcesarClick);
                        _ui.eventClick($("#btnConfirmar"), _default.btnConfirmarRecibidoClick)
                    });
                },

                btnProcesarClick: function () {
                    var array = [];
                    $.each($("#GrillaRecepcionPadre input[type='checkbox']"), function (i, item) {
                        if ($(item).is(':checked')) {
                            array.push($(item).val() + "_1");
                        } else {
                            array.push($(item).val() + "_0");
                        }
                    });

                    bootbox.dialog({
                        message: "<h4>¿Desea procesar los archivos?</h4>",
                        title: "<b>Confirmar</b>",
                        buttons: {
                            success: {
                                label: ":: Si ::",
                                className: "btn btn-login",
                                callback: function () {
                                    transact.ajaxPOST("/EnvioFront/procesarArchivos?negID=" + array, null, _default.successProcesar, _default.error);
                                }
                            },

                            danger: {
                                label: ":: No ::",
                                className: "btn-danger"
                            }
                        }
                    });
                },

                successProcesar: function () {
                    bootbox.dialog({
                        message: "<h4>Negocios procesados correctamente</h4>",
                        title: "<b>Confirmación</b>",
                        buttons: {
                            success: {
                                label: ":: Aceptar ::",
                                className: "btn btn-login",
                                callback: function () {
                                    refrescaGrilla();
                                }
                            }
                        }
                    });
                },

                btnConfirmarRecibidoClick: function () {
                    var cud = $("#txtCUD").val();
                    var observaciones = $("#txtObservaciones").val();

                    if (observaciones == "") {
                        $("#valObservaciones").attr("class", "form-group has-error");
                        return false;
                    }
                    else {
                        $("#valObservaciones").attr("class", "form-group has-success");
                        transact.ajaxGET("/EnvioFront/confirmarRecibido?cud=" + cud, null, _default.succesConfirmarRecibido, _default.error);
                    }
                },

                succesConfirmarRecibido: function (data) {
                    var resultado = data;
                    var cud = $("#txtCUD").val();
                    var txtObservaciones = $("#txtObservaciones").val();
                    if (resultado == 1) {
                        bootbox.dialog({
                            message: "<h4>Recepción de la Unidad documental terminada correctamente</h4>",
                            title: "<b>Confirmación</b>",
                            buttons: {
                                success: {
                                    label: ":: Aceptar ::",
                                    className: "btn btn-login",
                                    callback: function () {
                                        transact.ajaxPOST("/EnvioFront/crearAsignacionTareaUD?cud=" + cud + "&txtObservaciones=" + txtObservaciones, null, _default.succesConfirmarCerradoUD, _default.error);
                                    }
                                }
                            }
                        });
                    }
                    else if (resultado == 0) {
                        bootbox.dialog({
                            message: "<h4>Aun faltan documentos por procesar</h4>",
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

                succesConfirmarCerradoUD: function () {
                    location.href = "Index";
                },

                error: function (error) {
                    console.log(error);
                }
            }
            _default._loadPage();
        })(jQuery);

    </script>

    <h1>Recepción documental</h1>
    <fieldset class="scheduler-border">
        <legend class="scheduler-border">Filtro de Busqueda</legend>
        <div class="divleft">
            <label>CUD</label>
            <input type="text" class="form-control" id="txtCUD" name="txtCUD" />
            <input type="button" class="btn btn-login" id="btnBuscar" onclick="refrescarGrillaRecepcion()" name="btnBuscar" value=":: Buscar ::" />
        </div>
    </fieldset>
    <div id="panelGrilla" style="display: none">
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Unidad documental recibida</legend>
            <input type="button" value=":: Procesar ::" id="btnProcesar" class="btn btn-Comando" /><br />
            <br />
            <%= Html.Telerik().StyleSheetRegistrar()
                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
            <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_RecepcionPadre>()
                    .Name("GrillaRecepcionPadre")
                    .Columns(columns =>
                    {
                        columns.Bound(o => o.cud).Width(100).Title("CUD").HeaderHtmlAttributes(new { style = "height: 40px" });
                        columns.Bound(o => o.tipoContenedor).Width(100).Title("Tipo de Contenedor");
                        columns.Bound(o => o.nomOficina).Width(100).Title("Nombre Oficina");
                        columns.Bound(o => o.subProductos).Width(100).Title("SubProductos");
                        columns.Bound(o => o.fechaEnvio).Width(100).Format("{0:F}").Title("Fecha de Envio");
                    })

                        .DetailView(details => details.ClientTemplate(
                            Html.Telerik().Grid<GestorDocumental.Controllers.grilla_RecepcionHijo>()
                            .Name("<#= cud #>")
                            .Columns(columns =>
                            {
                                columns.Bound(a => a.posicion).Width(5).Title("Posición").HeaderHtmlAttributes(new { style = "height: 40px" });
                                columns.Bound(a => a.negId).Width(100).Title("Negocio");
                                columns.Bound(a => a.codBarras).Width(100).Title("Codigo de Barras");
                                columns.Bound(a => a.paginas).Width(100).Title("Paginas");
                                columns.Bound(a => a.campo1).Width(100).Title(ViewData["CAMPO_1"].ToString());
                                columns.Bound(a => a.campo2).Width(100).Title(ViewData["CAMPO_2"].ToString());
                                columns.Bound(a => a.campo3).Width(100).Title(ViewData["CAMPO_3"].ToString());

                                columns.Bound(o => o.negId)
                                    .ClientTemplate("<input type='checkbox' id='chkMessage' name='checkedRecords' value='<#= negId #>' />")
                                    .Title("Recibido")
                                    .Width(100)
                                    .HtmlAttributes(new { style = "text-align:center" });
                            })
                            .DataBinding(databinding => databinding.Ajax()
                                .Select("getDetalle", "EnvioFront", new { cud = "<#= cud #>" }))
                            .Pageable(paginas => paginas.PageSize(20))
                            .ToHtmlString()))

                    .Pageable(paginas => paginas.PageSize(10))
                    .ClientEvents(q =>
                    {
                        q.OnComplete("refrescarGrillaRecepcion");
                        q.OnRowDataBound("prueba_detallesResultado");
                    })
                    .Sortable()
                    .Filterable()
            %>
            <br />

            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Documentos ya procesados</legend>

                <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_RecepcionHijo>()
                    .Name("GrillaDocumentosProcesados")
                    .DataBinding(databinding => databinding.Ajax().Select("getGrillaDocumentosProcesados", "EnvioFront", new { CUD = "0" }))
                    .Columns(columns =>
                    {
                        columns.Bound(o => o.posicion).Width(5).Title("Posición").HeaderHtmlAttributes(new { style = "height: 40px" });
                        columns.Bound(a => a.negId).Width(100).Title("Negocio");
                        columns.Bound(a => a.codBarras).Width(100).Title("Codigo de Barras");
                        columns.Bound(a => a.paginas).Width(100).Title("Estado");
                        columns.Bound(a => a.campo1).Width(100).Title(ViewData["CAMPO_1"].ToString());
                        columns.Bound(a => a.campo2).Width(100).Title(ViewData["CAMPO_2"].ToString());
                        columns.Bound(a => a.campo3).Width(100).Title(ViewData["CAMPO_3"].ToString());
                        columns.Command(o => o.Custom("Eliminar").Text("Eliminar")
                            .SendState(false)
                            .Ajax(true)).Width(100);
                    })
                    .ClientEvents(e => {
                        e.OnCommand("eliminarDocumento");
                        e.OnComplete("refrescaGrilla");
                    })
                %>
            </fieldset>
            <br />
            <div id="valObservaciones">
                <textarea class="form-control" rows="3" id="txtObservaciones" placeholder="Observaciones"></textarea>
            </div>
            <br />
            <input type="button" value=":: Confirmar Recibido ::" id="btnConfirmar" class="btn btn-login" />
            <input type="button" value=":: Generar Planilla ::" id="btnPlanilla" onclick="printGrid()" class="btn btn-login" />
        </fieldset>
    </div>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
</asp:Content>
