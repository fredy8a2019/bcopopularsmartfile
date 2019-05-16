<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Crear Actas Destruccion
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
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
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var paginaDestruccionIni = null;
        (function () {
            paginaDestruccionIni = {
                loadPageDestruccionIni: function () {
                    $(document).ready(function () {
                        $("#GrillaContenidoActa.t-grid .t-refresh").trigger('click');
                        transact.ajaxGET("/IniDestruccion/ObtNroActa", null, function (data) {
                            $("#txtNroActa").val(data);
                            $("#txtNroActa2").val(data);
                        }, function (error) {
                            console.log(error);
                        });
                        //_ui.eventClick($("#btnAgregarAGrid"), paginaCrearArchivo.btnAgregarGrid);
                        //_ui.eventClick($("#btnAceptar"), paginaCrearArchivo.btnAceptarClick);
                    });
                },

            },
            paginaDestruccionIni.loadPageDestruccionIni();

        })();

        //Para que solo ingrese numeros
        function numbersonly(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode != 8 && unicode != 45 && unicode != 9) {
                if (unicode < 43 || unicode > 57) //if not a number
                { return false } //disable key press    
            }
        }

        function refrescarGrillas() {
            $("#GrillaNegDestruccion.t-grid .t-refresh").trigger('click');
            $("#GrillaContenidoActa.t-grid .t-refresh").trigger('click');
        }

        function radNegCilck() {
            var valor = $('input[name=radDocs]:checked').val();
            if (valor == "rdNegocio") {
                $("#_panelNegocio").removeAttr("style");
                $("#_panelGrillaDOC").css("display", "none");
                $("#_panelCodBarras").css("display", "none");
            }
        };
        function radCodBarrasCilck() {
            var valor = $('input[name=radDocs]:checked').val();
            if (valor == "rdCodBarras") {
                $("#_panelCodBarras").removeAttr("style");
                $("#_panelNegocio").css("display", "none");
                $("#_panelGrillaDOC").css("display", "none");
            }
        };
        function radGrillaDocCilck() {
            var valor = $('input[name=radDocs]:checked').val();
            if (valor == "rdTodosDOC") {
                $("#_panelGrillaDOC").removeAttr("style");
                $("#_panelNegocio").css("display", "none");
                $("#_panelCodBarras").css("display", "none");
            }

            $("#_panelGrillaDOC").removeAttr("style");
        };

        function AgregaPorDoc(campoH) {
            var codProceso = $(campoH).val();
            var codActa = $("#txtNroActa").val();

            if (codProceso == 1) {
                var codDocumento = $("#txtNroNEG").val();
            } else if (codProceso == 2) {
                var codDocumento = $("#txtCodBarras").val();
            }

            transact.ajaxGET("/IniDestruccion/GrillaPorDoc?_codProceso=" + codProceso + "&_codDocumento=" + codDocumento + "&_nroActa=" + codActa, null, function (data) {
                if (data == 1) { //proceso ok
                    bootbox.dialog({
                        message: "<h4>Registro agregado al Acta #" + $("#txtNroActa").val() + "</h4>",
                        title: "",
                        buttons: {
                            success: {
                                label: ".: Aceptar :.",
                                className: "btn btn-login",
                                callback: function () {
                                    refrescarGrillas();
                                }
                            }
                        }
                    });
                } else if (data == 0) { //no encuentra registro o éste no cumple con las reglas
                    bootbox.dialog({
                        message: "<h4>El registro no se encuentra Disponible o ya fue agregado al Acta</h4>",
                        title: "<b>Alerta</b>",
                        buttons: {
                            success: {
                                label: ".: Aceptar :.",
                                className: "btn-danger"
                            }
                        }
                    });
                    $("#txtNroNEG").val("");
                    $("#txtCodBarras").val("");
                };
            });
        };

        //Refresca la grilla de los documentos listos para almacenar por las reglas
        function refrescarGrillaDestruc(e) {
            if (e.name = "Almacenar") {
                bootbox.dialog({
                    title: "<b>Confirmación</b>",
                    message: "<h4>Documento agregado al Acta de Destrucción</h4>",
                    buttons: {
                        success: {
                            label: ":: Aceptar ::",
                            className: "btn btn-login"
                        }
                    }
                });
                $("#GrillaNegDestruccion.t-grid .t-refresh").trigger('click');
                $("#GrillaContenidoActa.t-grid .t-refresh").trigger('click');
            }
        };
        function agregaActa() {
            var array = []
            $.each($("#GrillaNegDestruccion input[type='checkbox']:checked"), function (i, item) {
                array.push($(item).val());
            });

            if (array.length > 0) {
                transact.ajaxPOST("/IniDestruccion/AgregarAlActaDestruc?nroActa=" + $("#txtNroActa").val() + "&negID=" + array, null, function () { }, function error() { console.log(error) });
                bootbox.dialog({
                    message: "<h4>Registro agregado al Acta #" + $("#txtNroActa").val() + "</h4>",
                    title: "<b>Confirmación</b>",
                    buttons: {
                        success: {
                            label: ".: Aceptar :.",
                            className: "btn btn-login",
                            callback: function () {
                                refrescarGrillas();
                            }
                        }
                    }
                });
            } else {
                bootbox.dialog({
                    message: "<h4>No se han seleccionado registros para agregar al Acta</h4>",
                    title: "<b>Alerta</b>",
                    buttons: {
                        success: {
                            label: ".: Aceptar :.",
                            className: "btn-danger"
                        }
                    }
                });
            };
        }
        function eliminaDocGrilla(e) {
            var negId = e.dataItem["_negID"];
            bootbox.dialog({
                message: "<h4>¿Seguro que desea eliminar el registro?</h4>",
                title: "<b>Eliminar</b>",
                buttons: {
                    success: {
                        label: ".: SI :.",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/IniDestruccion/EliminaDocActa?_negID=" + negId, null, function () { }, function error() { console.log(error) });
                            refrescarGrillas();
                        }
                    },
                    danger: {
                        label: ".: NO :.",
                        className: "btn-danger"
                    }
                }
            });
        }

        function CreaActa() {
            var nroActa = $("#txtNroActa").val();
            bootbox.dialog({
                message: "<h4>¿Confirma  que desea Crear el Acta Nro. " + nroActa + "</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ".: SI :.",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/IniDestruccion/CreaActaDestruc?_nroActa=" + nroActa, null, function () { }, function error() { console.log(error) });
                            location.href = "/IniDestruccion/IniDestruccion";
                        }
                    },
                    danger: {
                        label: ".: NO :.",
                        className: "btn-danger"
                    }
                }
            });
        }
        function completado() {
            var _acta = $("#txtNroActa").val();
            transact.ajaxPOST("/IniDestruccion/CreaActaDestruc?_nroActa=" + _acta, null, function () { }, function error() { console.log(error) });

            transact.ajaxGET("/IniDestruccion/cantActas", null, function (data) {
                bootbox.dialog({
                    message: "<h4>Se cargaron correctamente " + data + " archivos al Acta #" + _acta + "</h4>",
                    title: "<b>Confirmación</b>",
                    buttons: {
                        success: {
                            label: ".: Aceptar :.",
                            className: "btn btn-login",
                            callback: function () {
                                location.href = "/IniDestruccion/IniDestruccion";
                            }
                        }
                    }
                });

            }, function error() { console.log(error) });
        }

        function obtNroActa(e) {
            e.data = { _nroActa: $("#txtNroActa2").val() };
        }

    </script>

    <h2>Creación de Actas para Destrucción Documental</h2>
    <ul class="nav nav-tabs">
        <li class="active"><a data-toggle="tab" href="#home">Creación de Actas</a></li>
        <li><a data-toggle="tab" href="#menu1">Destrucción Masiva</a></li>
    </ul>

    <div class="tab-content">
        <div id="home" class="tab-pane fade in active">
            <br />

            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Documentos Listos para Destrucción</legend>
                <div class="row">
                    <div class="span2">
                        <label>Nro. Acta:</label>
                        <input type="text" style="width: 50px; background-color: #D2C9C9" id="txtNroActa" name="name" value="" readonly="readonly" />
                    </div>
                </div>
                <div class="row">
                    <div class="span5">
                        <div id="panelNegocio" style="display: block">
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border">
                                    <input type="radio" id="consultaNEG" name="radDocs" value="rdNegocio" onclick="radNegCilck()" />
                                    Agregar a Destrucción por Documento
                                </legend>
                                <div id="_panelNegocio" style="display: none">
                                    <label>Número Documento:</label>
                                    <input type="text" style="width: 300px;" id="txtNroNEG" name="name" value="" />
                                    <input type="hidden" id="txtProc1" name="name" value="1" />
                                    <br />
                                    <input type="button" name="name" value=".: Buscar :." class="btn btn-Comando" onclick="AgregaPorDoc(document.getElementById('txtProc1'))" id="btnNEGOCIO" />
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <div class="span5">
                        <div id="panelCodigoBarras" style="display: block">
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border">
                                    <input type="radio" id="consultaCODBARRAS" name="radDocs" value="rdCodBarras" onclick="radCodBarrasCilck()" />
                                    Agregar a Destrucción por Código de Barras
                                </legend>
                                <div id="_panelCodBarras" style="display: none">
                                    <label>Código de Barras:</label>
                                    <input type="text" style="width: 300px;" id="txtCodBarras" name="name" value="" onkeypress="return numbersonly(event)" />
                                    <input type="hidden" id="txtProc2" name="name" value="2" />
                                    <br />
                                    <input type="button" name="name" value=".: Buscar :." class="btn btn-Comando" onclick="AgregaPorDoc(document.getElementById('txtProc2'))" id="btnCodBARRAS" />
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span10">
                        <div id="panelGrillaDoc" style="display: block">
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border">
                                    <input type="radio" id="consultaTodosDOc" name="radDocs" value="rdTodosDOC" onclick="radGrillaDocCilck()" />
                                    Listado de Negocios a Destruir
                                </legend>
                                <div id="_panelGrillaDOC" style="display: none">
                                    <%= Html.Telerik().StyleSheetRegistrar()
                                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
                                    <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_ListDestruc>()
                            .Name("GrillaNegDestruccion")
                            .DataBinding(d => d.Ajax()
                            .Select("grillaListDestruc", "IniDestruccion"))
                            .Columns(columns =>
                            {
                                columns.Bound(o => o._negId).Width(100).Title("Negocio").HeaderHtmlAttributes(new { style = "height: 40px" });
                                columns.Bound(o => o._producto).Width(100).Title("Producto");
                                columns.Bound(o => o._subProducto).Width(100).Title("Sub Producto");
                                columns.Bound(o => o._codBarras).Width(100).Title("Código de Barras");
                                columns.Bound(o => o._campo1).Width(100).Title(ViewData["CAMPO_1"].ToString());
                                columns.Bound(o => o._campo2).Width(100).Title(ViewData["CAMPO_2"].ToString());
                                columns.Bound(o => o._campo3).Width(100).Title(ViewData["CAMPO_3"].ToString());

                                columns.Bound(o => o._negId)
                                    .ClientTemplate("<input type='checkbox' id='chkMessage' name='checkedRecords' value='<#= _negId #>' />")
                                    .Title("Destruccion")
                                    .Width(100).Title("")
                                    .HtmlAttributes(new { style = "text-aling:center" });
                            })
                            .ClientEvents(q => { 
                                q.OnCommand("refrescarGrillaDestruc"); 
                            })
                            
                            .Pageable(pager => pager.PageSize(25, new int[] { 10,25,50 })
                                .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
                            .Filterable()
                            .ToHtmlString()
                                    %>
                                    <br />
                                    <input type="button" value=".: Agregar al Acta :." id="btnAgregaActa" class="btn btn-Comando" onclick="agregaActa()" />
                                </div>

                            </fieldset>
                        </div>
                    </div>
                </div>
            </fieldset>

            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Contenido del Acta de Destrucción</legend>
                <div id="GrillaActa">
                    <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_ContenidoActa>()
                    .Name("GrillaContenidoActa")
                    .Columns(columns =>
                    {
                        columns.Bound(o => o._negID).Width(100).Title("Negocio").HeaderHtmlAttributes(new { style = "height: 40px" });
                        columns.Bound(o => o._fecCreacion).Width(100).Title("Fecha Creación Acta");
                        columns.Command(o => o.Custom("Eliminar").Text("Eliminar")
                            .SendState(false)
                            .Ajax(true)).Width(100);
                    })
                    .DataBinding(d => d.Ajax().Select("grillaContenidoActa", "IniDestruccion", new { _codActa = ViewData["_codActa"].ToString() }))
                    .Pageable(paginas => paginas.PageSize(10))
                    .ClientEvents(o => o.OnCommand("eliminaDocGrilla"))
                    .Sortable()
                    .Filterable()
                    .ToHtmlString()
                    %>
                </div>
                <br />
                <input type="button" value=".: Crear Acta :." id="btnCreaActa" class="btn btn-login" onclick="CreaActa()" />

            </fieldset>
        </div>
        <div id="menu1" class="tab-pane fade">
            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Cargue de Archivo</legend>
                <div class="row">
                    <div class="span2">
                        <label>Nro. Acta:</label>
                        <input type="text" style="width: 50px; background-color: #D2C9C9" id="txtNroActa2" name="name" value="" readonly="readonly" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="span10">
                        <%= Html.Telerik().Upload()
                        .Name("files")
                        .Multiple(false)
                        .Async(async => async
                            .Save("CrearDestruccionMasiva", "IniDestruccion")
                            .AutoUpload(true))
                            .ClientEvents(events => events
                                .OnUpload("obtNroActa")
                                .OnSuccess("completado"))
                        %>
                    </div>
                </div>
                <br />
            </fieldset>
        </div>
    </div>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
</asp:Content>
