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
    Almacenar
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Almacenar documentos</h2>
    <fieldset class="scheduler-border">
        <legend class="scheduler-border">Datos Unidad de Almacenamiento
            <label id="lbl_Label" style="color: #6785C1; font-size: xx-large;"></label>
        </legend>
        <div id="cotBuscar" class="divleft">
            <label>
                Numero CUD:</label>
            <input type="text" name="" id="txtNumeroCUD" value="" />
            <input type="button" name="" value=": : Buscar : :" onclick="ejecutaGrillas()" class="btn btn-login" id="btnBuscarCUD" />
        </div>
        <div class="divleft">
            <label>
                CUD:</label>
            <input type="text" id="txtCUD" disabled="disabled" name="name" value="" />
        </div>
        <div class="divleft">
            <label>
                Tipo contenedor:</label>
            <input type="text" id="txtTipoContenedor" disabled="disabled" name="name" value="" />
        </div>
        <div class="divleft">
            <label>
                Fecha de creación:</label>
            <input type="text" id="txtFechaCreacion" disabled="disabled" name="name" value="" />
        </div>
        <div class="divleft">
            <label>
                Cliente:</label>
            <input type="text" id="txtCliente" disabled="disabled" name="name" value="" />
        </div>
        <div class="divleft">
            <label>
                Oficina:</label>
            <input type="text" id="txtOficina" disabled="disabled" name="name" value="" />
        </div>
        <div class="divleft">
            <label>
                Destino:</label>
            <input type="text" id="txtDestino" disabled="disabled" name="name" value="" />
        </div>
        <div class="divleft">
            <label>
                Subproducto:</label><br />
            <textarea id="txtSubProductos" disabled="disabled" style="width: 242px;"></textarea>
        </div>
    </fieldset>

    <div class="row">
        <div class="span5">
            <div id="panelSecundario" style="display: none">
                <fieldset class="scheduler-border">
                    <legend class="scheduler-border">
                        <input type="radio" id="verBLote" name="rOpciones" value="bLote" />
                        Busqueda por Lote
                    </legend>
                    <div class="divleft" id="_almLote">
                        <label>
                            Lote:</label>
                        <input type="text" style="width: 300px;" id="txtNoLote" name="name" value="" />
                        <input type="button" name="name" value=": : Buscar : : " class="btn btn-login" onclick="refrescarGrilla()" id="btnBuscarFiltro" />
                    </div>
                </fieldset>
            </div>
        </div>
        <div class="span5">
            <div id="panelManualPistoleo" style="display: none">
                <fieldset class="scheduler-border">
                    <legend class="scheduler-border">
                        <input type="radio" id="verAPistoleo" name="rOpciones" value="pistoleo" />
                        Almacenamiento Manual
                    </legend>
                    <div class="divleft" id="_almPistoleo">
                        <label>
                            Codigo de Barras:</label>
                        <input type="text" style="width: 300px;" id="txtNoNegocio" name="name" value="" />
                        <input type="button" name="name" value=": : Almacenar : : " class="btn btn-login" id="btnAlmacenarPistoleo" />
                    </div>
                </fieldset>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        //Para evitar que la grilla se LLene al momento en que la pagina se cargue
        var initialLoad = true;
        function Grid_onDataBinding(e) {
            if (initialLoad) {
                e.preventDefault();
                initialLoad = false;
            }
        }

        function ejecutaGrillas() {
            refrescarGrillaUD();
            refrescarGrillaReglas();
        }

        //Refresca la grilla de mostrar la informacion de los documentos por lotes.
        function refrescarGrilla() {
            var valor = $("#txtNoLote").val();
            if (valor == "") {
                bootbox.dialog({
                    message: "<h4>Ingrese un valor a buscar</h4>",
                    title: "<b>Alerta</b>",
                    buttons: {
                        success: {
                            label: ":: Aceptar ::",
                            className: "btn-danger"
                        }
                    }
                });
            }
            else {
                var link = "/Almacenar/getAlmacenarLote?NoLote=" + valor;
                $.ajax({
                    type: "POST",
                    url: link,
                    data: { dato: valor },
                    dataType: "json",
                    success: function (result) {
                        $("#Grid").data("tGrid").dataBind(result.data);
                    },
                    error: function (result) {
                        alert(result.message);
                    }
                })

                refrescarGrillaUD();
                $("#GrillaBmanual").removeAttr("style");
                $("#GridReglasDocumentos.t-grid .t-refresh").trigger('click');
            }
        }

        //Refresca la grilla de los documentos listos para almacenar por las reglas
        function refrescarGrillaReglas(e) {
            $("#GridReglasDocumentos.t-grid .t-refresh").trigger('click');
            $("#GridContenidoUD.t-grid .t-refresh").trigger('click');

            if (e.name = "Almacenar") {
                bootbox.dialog({
                    message: "<h4>Documento almacenado</h4>",
                    title: "<b>Confirmación</b>",
                    buttons: {
                        success: {
                            label: ":: Aceptar ::",
                            className: "btn btn-login"
                        }
                    }
                });
            }
        }

        function ejecutarBoton() {
            $('#btnBuscarFiltro').trigger('click');
        }

        //Refresca la grilla de mostrar la informacion de los documentos almacenados en la Unidad Documental.
        function refrescarGrillaUD() {
            $("#GridReglasDocumentos.t-grid .t-refresh").trigger('click');
            var cUD = $("#txtNumeroCUD").val();
            if (cUD == "")
                cUD = 0;
            else
                cUD = $("#txtNumeroCUD").val();
            var link = "/Almacenar/getContenidoUD?ud=" + cUD;
            $.ajax({
                type: "POST",
                url: link,
                data: { dato: cUD },
                dataType: "json",
                success: function (result) {
                    $("#GridContenidoUD").data("tGrid").dataBind(result.data);
                },
                error: function (result) {
                    alert(result.message);
                }
            });
        }

        //Funcion que trae los registros de la grilla de detalles en los lotes consultados.
        function prueba_detallesResultado(e) {
            var grid = $(this).data('tGrid');
            //expandFirstRow(grid, e.row);
        }

        function expandFirstRow(grid, row) {
            if (grid.$rows().index(row) == 0) {
                grid.expandRow(row);
            }
        }

        function refrescaPistoleo() {
            $("#txtNoNegocio").focus();
            $("#txtNoNegocio").val("");
            $("#GridContenidoUD.t-grid .t-refresh").trigger('click');
            $("#GridReglasDocumentos.t-grid .t-refresh").trigger('click');
        }

        function refrescaGrillas() {
            $("#GridContenidoUD.t-grid .t-refresh").trigger('click');
            $("#GridReglasDocumentos.t-grid .t-refresh").trigger('click');
            //ejecutarBoton();
        }

        function refrescaGrillaReglas() {
            $("#GridReglasDocumentos.t-grid .t-refresh").trigger('click');
        }

        function ocultarPaneles() {
            $("#panelSecundario").css("display", "none");
            $("#panelManualPistoleo").css("display", "none");
            $("#GrillaDocReglas").css("display", "none");
            $("#panelGrilla").css("display", "none");
            limpiarCampos();
        }

        function limpiarCampos() {
            $("#txtCUD").val("");
            $("#txtTipoContenedor").val("");
            $("#txtFechaCreacion").val("");
            $("#txtCliente").val("");
            $("#txtOficina").val("");
            $("#txtDestino").val("");
            $("#txtSubProductos").text("");
        }

        function eliminarDocumento(e) {
            var _cud = e.dataItem["_cud"];
            var _negId = e.dataItem["_negId"];

            bootbox.dialog({
                message: "<h4>¿Desea eliminar el documento seleccionado?</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/Almacenar/EliminarDocumentoUD?negID=" + _negId + "&ud=" + _cud, null,
                                function () { },
                                function (error) { console.log(error) });
                            refrescaGrillas();
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });

        }

        //Funcion para generar la planilla
        function printGrid() {
            var gridElement = $('#GridContenidoUD'),
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

                printableContent = printableContent.clone()[0].outerHTML;
            }

            doc.write(htmlStart + printableContent + htmlEnd);
            doc.close();
            win.print();
        }

        $.noConflict();
        var _default = null;
        (function ($) {
            _default = {
                countRow: 1,
                _loadPage: function () {
                    $(document).ready(function () {
                        //Foco en el primer campo
                        $("#txtNumeroCUD").focus();
                        //BTN
                        _ui.eventClick($("#btnBuscarCUD"), _default.btnBuscarClick);
                        _ui.eventClick($("#btnLiberar"), _default.btnLiberarUDClick);
                        _ui.eventClick($("#btnCerrar"), _default.btnCerrarUDClick);
                        _ui.eventClick($("#btnAlmacenarPistoleo"), _default.btnAlmacenaPistoleoClick);
                        _ui.eventClick($("#btnAlmacenarMasivo"), _default.btnAlmacenarMasivoClick);

                        //RadioButtons
                        _ui.eventClick($("#verBLote"), _default.radVerLoteClick);
                        _ui.eventClick($("#verAPistoleo"), _default.radVerAPistoleoClick);
                        _ui.eventClick($("#verGrillaReglas"), _default.radVerGrillaReglas);
                        _ui.eventClick($("#verGrillaBManual"), _default.radVerGrillaBManual)
                    });
                },

                btnBuscarClick: function () {
                    var CUD = $("#txtNumeroCUD").val();
                    $("#wait").removeAttr("style");
                    transact.ajaxGET("/Almacenar/_validarCUD?numeroCud=" + CUD, null, _default.successValidarUD, _default.error);
                },

                btnLiberarUDClick: function () {
                    var ud = $("#txtCUD").val();
                    transact.ajaxPOST("/Almacenar/liberarCajaUD?numeroCUD=" + ud, null, _default.successLiberarUD, _default.error);
                },

                btnCerrarUDClick: function () {
                    bootbox.dialog({
                        message: "<h4>¿Esta seguro de cerrar por completo la Unidad Documental?</h4>",
                        title: "<b>Confirmación</b>",
                        buttons: {
                            success: {
                                label: ":: Si ::",
                                className: "btn btn-login",
                                callback: function () {
                                    var ud = $("#txtCUD").val();
                                    transact.ajaxPOST("/Almacenar/cerrarCajaUD?numeroCUD=" + ud, null, _default.successCerrarUD, _default.error);
                                }
                            },
                            danger: {
                                label: ":: No ::",
                                className: "btn-danger"
                            }
                        }
                    });
                },

                btnAlmacenaPistoleoClick: function () {
                    var noDocumento = $("#txtNoNegocio").val();
                    transact.ajaxGET("/Almacenar/almacenarPistoleo?numeroDocumento=" + noDocumento, null, _default.successAlmacenarPistoleo, _default.error);
                },

                btnAlmacenarMasivoClick: function () {
                    var array = []
                    $.each($("#GridReglasDocumentos input[type='checkbox']:checked"), function (i, item) {
                        array.push($(item).val());
                    });

                    if (array.length > 0) {
                        transact.ajaxPOST("/Almacenar/AlmacenarDocumentoReglas?negID=" + array, null, _default.successAlmacenarMasivo, _default.error);
                    } else {
                        bootbox.dialog({
                            message: "<h4>No ha seleccionado ningún negocio para almacenar</h4>",
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

                radVerLoteClick: function () {
                    var valor = $('input[name=rOpciones]:checked').val();
                    //Si esta seleccionado BLote se oculta el de Almacenamiento Manual.
                    if (valor == "bLote") {
                        $("#_almLote").removeAttr("style");
                        $("#_almPistoleo").css("display", "none");
                        $("#_pnlDocListos").css("display", "none");
                    }
                    else if (valor == "pistoleo") {
                        $("#_almPistoleo").removeAttr("style");
                        $("#_almLote").css("display", "none");
                    }
                },

                radVerAPistoleoClick: function () {
                    var valor = $('input[name=rOpciones]:checked').val();
                    //Si esta seleccionado el valor del pistoleo se oculta la busqueda por lote.
                    if (valor == "pistoleo") {
                        $("#_almPistoleo").removeAttr("style");
                        $("#_almLote").css("display", "none");
                        $("#_pnlDocListos").css("display", "none");
                    } else if (valor == "bLote") {
                        $("#_almLote").removeAttr("style");
                        $("#_almPistoleo").css("display", "none");
                    }
                },

                radVerGrillaReglas: function () {
                    var valor = $('input[name=rOpciones]:checked').val();
                    //Si esta seleccionado el valor de la grilla se ocultan los otros dos opciones
                    if (valor == "gridReglas") {
                        $("#_pnlDocListos").removeAttr("style");
                        $("#_almPistoleo").css("display", "none");
                    }
                },

                radVerGrillaBManual: function () {
                    var valor = $('input[name=rOpciones]:checked').val();
                    //Si esta seleccionado el valor de la grilla se ocultan los otros dos opciones
                    if (valor == "gridBManual") {
                        $("#_pnlDocListos").css("display", "none");
                        $("#_almPistoleo").css("display", "none");
                    }
                },

                //Valida si la Unidad documental consultada esta disponible o no
                successValidarUD: function (data) {
                    var resultado = data;
                    var CUD = $("#txtNumeroCUD").val();

                    limpiarCampos();
                    $("#panelSecundario").css("display", "none");
                    $("#panelManualPistoleo").css("display", "none");
                    $("#panelGrilla").css("display", "none");
                    $("#GrillaDocReglas").css("display", "none");

                    if (resultado == 1) {
                        bootbox.dialog({
                            message: "<h4>La unidad Documental No. " + CUD + " no se encuentra disponible</h4>",
                            title: "<b>Alerta</b>",
                            buttons: {
                                success: {
                                    label: ":: Aceptar ::",
                                    className: "btn-danger"
                                }
                            }
                        });
                    }
                    else {
                        //Crea la Asignacion de tareas para bloquear la Unidad Documental
                        $.ajax({
                            type: "POST",
                            url: "/Almacenar/_CrearEtapaBloqueo?numeroCud=" + CUD,
                            data: { numeroCud: CUD },
                            dataType: "json",
                            success: function (result) {
                                if (result[0].Resultado == 0) {
                                    bootbox.dialog({
                                        message: "<h4>La unidad Documental No. " + CUD + " esta en Uso</h4>",
                                        title: "<b>Alerta</b>",
                                        buttons: {
                                            success: {
                                                label: ":: Aceptar ::",
                                                className: "btn btn-login",
                                                callback: function () {
                                                    ocultarPaneles();
                                                }
                                            }
                                        }
                                    });
                                }
                                else {
                                    transact.ajaxGET("/Almacenar/_getDataCUD?numeroCud=" + CUD, null, _default.successTipoContenedorUD, _default.error);
                                }
                            },
                            error: function () {
                            }
                        });
                    }
                },

                //Cuando la busqueda es exitosa, se cargan los datos de UD en los campos del formulario
                successTipoContenedorUD: function (data) {
                    if (data == "") {
                        var _CUD = $("#txtNumeroCUD").val();
                        bootbox.dialog({
                            message: "<h4>La unidad Documental No. " + _CUD + " no existe</h4>",
                            title: "<b>Alerta</b>",
                            buttons: {
                                success: {
                                    label: ":: Aceptar ::",
                                    className: "btn-danger"
                                }
                            }
                        });
                    }
                    else {
                        $("#txtCUD").val(data[0].CUD);
                        $("#txtTipoContenedor").val(data[0].Tipo);
                        $("#txtCliente").val(data[0].CliNombre);
                        $("#txtOficina").val(data[0].Oficina);
                        $("#txtDestino").val(data[0].Destino);
                        $("#lbl_Label").text(" - " + data[0].Destino);
                        $("#txtSubProductos").text(data[0].SubProductos);

                        var fecha = new Date(parseInt(data[0].FechaCreacion.substr(6)));
                        var fechaFinal = formatDate(fecha);
                        $("#txtFechaCreacion").val(fechaFinal);
                        $("#panelSecundario").css("display", "block");
                        $("#panelManualPistoleo").css("display", "block");
                        $("#panelGrilla").removeAttr("style");
                        $("#GrillaDocReglas").removeAttr("style");

                        refrescaGrillaReglas();
                        $("#wait").css("display", "none");
                    }
                },

                //Se ejecuta el codigo cuando se selecciona la opcion de liberar la Unidad documental
                successLiberarUD: function () {
                    var ud = $("#txtCUD").val();
                    bootbox.dialog({
                        message: "<h4>La unidad documental " + ud + " se ha liberado</h4>",
                        title: "<b>Confirmación</b>",
                        buttons: {
                            success: {
                                label: ":: Aceptar ::",
                                className: "btn btn-login",
                                callback: function () {
                                    var pagina = "/Almacenar/CrearUnidad";
                                    location.href = pagina;
                                }
                            }
                        }
                    });
                },

                //Se ejecuta el codigo cuando se selecciona la opcion de cerrar la unidad documental
                successCerrarUD: function () {
                    var ud = $("#txtCUD").val();
                    bootbox.dialog({
                        message: "<h4>La unidad documental " + ud + " se ha cerrado</h4>",
                        title: "<b>Confirmación</b>",
                        buttons: {
                            success: {
                                label: ":: Aceptar ::",
                                className: "btn btn-login",
                                callback: function () {
                                    var pagina = "/Almacenar/CrearUnidad";
                                    location.href = pagina;
                                }
                            }
                        }
                    });
                },

                //Se ejecuta para realizar la función del pistoleo para guardar automaticamente el documento.
                successAlmacenarPistoleo: function (data) {
                    if (data == 0) {
                        bootbox.dialog({
                            message: "<h4>El documento no se encuentra disponible</h4>",
                            title: "<b>Alerta</b>",
                            buttons: {
                                success: {
                                    label: ":: Aceptar ::",
                                    className: "btn-danger"
                                }
                            }
                        });
                    }
                    else {
                        bootbox.dialog({
                            message: "<h4>Documento almacenado</h4>",
                            title: "<b>Confirmación</b>",
                            buttons: {
                                success: {
                                    label: ":: Aceptar ::",
                                    className: "btn btn-login",
                                    callback: function () {
                                        refrescaPistoleo();
                                    }
                                }
                            }
                        });
                    }
                },

                successAlmacenarMasivo: function () {
                    bootbox.dialog({
                        message: "<h4>Documento almacenado</h4>",
                        title: "<b>Confirmación</b>",
                        buttons: {
                            success: {
                                label: ":: Aceptar ::",
                                className: "btn btn-login",
                                callback: function () {
                                    refrescaGrillas();
                                }
                            }
                        }
                    });
                },

                btnCancelarClick: function () {
                    location.reload();
                },

                error: function (error) {
                    console.log(error);
                }
            }
            _default._loadPage();

        })(jQuery);

        function formatDate(fecha) {
            var _fecha = new Date(fecha);
            var fechaFinal = (_fecha.getMonth() + 1) + '/' + _fecha.getDate() + '/' + _fecha.getFullYear() + ":" + _fecha.getHours() + ":" + _fecha.getMinutes() + ":" + _fecha.getSeconds();
            return fechaFinal;
        }

        var rowNumber = 0;
        function resetRowNumber(e) {
            rowNumber = 0;
        }

        function renderNumber(data) {
            return ++rowNumber;
        }

    </script>

    <div id="GrillaBmanual" style="display: none">
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">
                <input type="radio" id="verGrillaBManual" name="rOpciones" value="gridBManual" />
                Busqueda Manual
            </legend>
            <%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.hay.css")) %>
            <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_AlmacenarLote>()
           .Name("Grid")
           .Columns(colums =>
           {
               colums.Bound(o => o._numeroLote).Width(100).Title("No. Lote").HeaderHtmlAttributes(new { style = "height: 40px" });
               colums.Bound(o => o._tipo).Width(100).Title("Tipo");
               colums.Bound(o => o._fechaCargue).Format("{0:F}").Width(100).Title("Fecha de Cargue");
           })
                .ClientEvents(events => events.OnRowDataBound("prueba_detallesResultado"))
                .DetailView(details => details.ClientTemplate(
                    Html.Telerik().Grid<GestorDocumental.Controllers.grilla_DetalleLote>()
                    .Name("<#= _numeroLote #>")
                    .Columns(columns =>
                    {
                        columns.Bound(a => a._negID).Width(100).Title("Negocio").HeaderHtmlAttributes(new { style = "height: 40px" });
                        columns.Bound(a => a._codBarras).Width(100).Title("Código de Barras");
                        columns.Bound(a => a._paginas).Width(100).Title("Paginas");
                        columns.Command(a => a.Custom("Almacenar").Text("Almacenar")
                            .SendState(false)
                            .DataRouteValues(route =>
                            {
                                route.Add(x => x._negID).RouteKey("negID");
                            })
                            .Ajax(true)
                            .Action("AlmacenarDocumento", "Almacenar"))
                            .Width(100);
                    })
                    .DataBinding(databinding => databinding.Ajax()
                    .Select("getDetalle", "Almacenar", new { loteONumeriUnico = "<#= _numeroLote #>" }))
                    .ClientEvents(q =>
                    {
                        q.OnCommand("refrescarGrilla");
                    })
                .Pageable(paginas => paginas.PageSize(10))
                .Sortable()
                .Filterable()
                .ToHtmlString()
                ))
           .Pageable(paging => paging.PageSize(20))
           .ClientEvents(events2 => events2.OnDataBinding("Grid_onDataBinding"))
            %>
        </fieldset>
    </div>

    <div id="GrillaDocReglas" style="display: none">
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">
                <input type="radio" id="verGrillaReglas" name="rOpciones" value="gridReglas" />
                Documentos listos para almacenar
            </legend>
            <div id="_pnlDocListos" style="display: none">
                <input type="button" value=":: Almacenar Documentos ::" id="btnAlmacenarMasivo" class="btn btn-Comando" /><br />
                <br />
                <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_ReglasDocumentos>()
                    .Name("GridReglasDocumentos")
                    .DataKeys(k => { k.Add(a => a._negID); })
                    .DataBinding(d => d.Ajax()
                        .Select("getGrillaReglas", "Almacenar", new { ud = "0" }))
                    .Columns(colums =>
                    {
                        colums.Bound(o => o._negID).Width(100).Title("Negocio").HeaderHtmlAttributes(new { style = "height: 40px" });
                        colums.Bound(o => o._paginas).Width(100).Title("Páginas");
                        colums.Bound(o => o._codBarras).Width(100).Title("Código de Barras");
                        colums.Bound(o => o._producto).Width(100).Title("Producto");
                        colums.Bound(o => o._subProducto).Width(100).Title("Sub Producto");
                        colums.Bound(o => o._campo1).Width(100).Title(ViewData["CAMPO_1"].ToString());
                        colums.Bound(o => o._campo2).Width(100).Title(ViewData["CAMPO_2"].ToString());
                        colums.Bound(o => o._campo3).Width(100).Title(ViewData["CAMPO_3"].ToString());

                        colums.Bound(o => o._negID)
                         .ClientTemplate("<input type='checkbox' id='chkMessage' name='checkedRecords' value='<#= _negID #>' />")
                         .Title("Almacenar")
                         .Width(100)
                         .HtmlAttributes(new { style = "text-align:center" });
                    })
                    .ClientEvents(q => { 
                        //q.OnCommand("refrescarGrillaReglas");
                    })
                    .Pageable(pager => pager.PageSize(10, new int[] { 10,25,50 })
                        .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
                    .Filterable()
                    .Groupable()
                    .ToHtmlString()
                %>
            </div>
        </fieldset>
    </div>

    <div id="panelGrilla" style="display: none">
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Contenido de la unidad documental</legend>
            <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_contenidoUD>()
                    .Name("GridContenidoUD")
                    .DataKeys(k => { k.Add(a => a._negId); k.Add(a => a._cud); })
                    .DataBinding(d => d.Ajax()
                        .Select("getContenidoUD", "Almacenar", new { ud = "0" }))
                    .Columns(colums =>
                    {
                        colums.Template(t => { }).Width(100).Title("Posición").ClientTemplate("<#= renderNumber(data) #>");
                        colums.Bound(o => o._cud).Width(100).Title("Número CUD").HeaderHtmlAttributes(new { style = "height: 40px" });
                        colums.Bound(o => o._negId).Width(100).Title("Negocio");
                        colums.Bound(o => o._codBarras).Width(100).Title("Código de Barras"); 
                        colums.Bound(o => o._fecha).Format("{0:F}").Width(100).Title("Fecha de almacenamiento");
                        colums.Command  (a =>
                            a.Custom("Almacenar")
                            .Text("Eliminar").Ajax(true)
                            .SendState(false))
                            .Width(100);
                    })
                    .ClientEvents(q => {
                        q.OnComplete("refrescarGrillaUD");
                        q.OnDataBound("resetRowNumber");
                        q.OnCommand("eliminarDocumento");
                    })
                    .Pageable(a => a.PageSize(200))
            %>
            <br />
            <input type="button" name="" value=":: Guardar :: " class="btn btn-login" style="display: none" id="btnGuardar" />
            <input type="button" name="" value=":: Liberar Caja :: " class="btn btn-login" id="btnLiberar" />
            <input type="button" name="" value=":: Cerrar Caja :: " class="btn btn-login" id="btnCerrar" />
            <input type="button" name="" value=":: Generar Planilla :: " onclick="printGrid()" class="btn btn-login" id="btnGenerarPlanilla" />
            <input type="button" name="" value=":: Cancelar :: " class="btn btn-login" style="display: none" id="btnCancelar" />
        </fieldset>
    </div>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
    <div id="content">
        <div id="wait" style="display: none; width: 69px; height: 89px; position: absolute; top: 50%; left: 50%; padding: 2px;">
            <img src="../../Images/cargando.gif" /><br />
        </div>
    </div>
</asp:Content>
