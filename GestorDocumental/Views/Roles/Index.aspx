<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Administración de Roles y paginas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    
    <meta http-equiv="expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="-1">
    
    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />

    <link href="../../Styles/JQueryUiBase/jquery-ui.css" rel="stylesheet" />
    <script src="../../Scripts/jquery.min.js"></script>
    <script src="../../Scripts/jquery-ui.min.js"></script>
    <link href="../../Content/themes/controls.css" rel="stylesheet" type="text/css" />
    <%--<script src="../../Scripts/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>

    <link href="../../Styles/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Styles/bootstrap-theme.min.css" rel="stylesheet" />

    <script src="../../Styles/BootsTrap/js/bootstrap-tooltip.js"></script>
    <script src="../../Styles/BootsTrap/js/bootstrap-popover.js"></script>

    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>--%>

    <style type="text/css">
        .divleft {
            float: left;
            display: inline;
            width: 27%;
        }

        /*legend.scheduler-border {
            font-size: 1.2em !important;
            font-weight: bold !important;
            text-align: left !important;
        }*/
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        $.noConflict();
        var _default = null;
        (function ($) {
            _default = {
                loadData: function () {
                    transact.ajaxPOST("/Listas/_GetDropDownRolesApp", null, _default.successTipoContenedorUD, _default.error);
                },

                loadPages: function () {
                    $(document).ready(function () {
                        _ui.buildCombobox();
                    });
                },

                successTipoContenedorUD: function (data) {
                    _ui.fillCombo($("#dp_Roles"), data);
                },

                error: function (error) {
                    console.log(error)
                },
            }

            _default.loadData();
            _default.loadPages();
        })(jQuery);

        function crearRoles() {
            var nombreRol = $("#txtNombreRol").val();
            bootbox.dialog({
                message: "<h4>¿Desea crear el rol: " + nombreRol + "?</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/Roles/crearRol?nombreRol=" + nombreRol, null,
                                function (data) {
                                    if (data == "Correcto") {
                                        mensajeCorrecto();
                                        $("#dp_Roles").empty();
                                        _default.loadData();
                                    }
                                    else {
                                        mensajeIncorrecto();
                                        $("#dp_Roles").empty();
                                        _default.loadData();
                                    }
                                },
                                function (error) {
                                    console.log(error);
                                });
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });
        }

        function desactivar(e) {
            var nombreRol = e.dataItem["nombreRol"];
            var idRol = e.dataItem["idRol"];

            bootbox.dialog({
                message: "<h4>¿Desea desactivar el rol: " + nombreRol + "?</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/Roles/desactivarRol?idRol=" + idRol + "&nombreRol=" + nombreRol, null,
                                function (data) {
                                    if (data == "Correcto") {
                                        mensajeCorrecto();
                                        $("#dp_Roles").empty();
                                        _default.loadData();
                                    }
                                    else {
                                        mensajeIncorrecto();
                                        $("#dp_Roles").empty();
                                        _default.loadData();
                                    }
                                },
                                function (error) {
                                    console.log(error);
                                });
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });
        }

        function activar(e) {
            var nombreRol = e.dataItem["nombreRol"];
            var idRol = e.dataItem["idRol"];

            bootbox.dialog({
                message: "<h4>¿Desea activar el rol: " + nombreRol + "?</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/Roles/activarRol?idRol=" + idRol + "&nombreRol=" + nombreRol, null,
                                function (data) {
                                    if (data == "Correcto") {
                                        mensajeCorrecto();
                                        $("#dp_Roles").empty();
                                        _default.loadData();
                                    }
                                    else {
                                        mensajeIncorrecto();
                                        $("#dp_Roles").empty();
                                        _default.loadData();
                                    }
                                },
                                function (error) {
                                    console.log(error);
                                });
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });
        }

        //Eliminar el Menu del rol seleccionado
        function eliminar(e) {
            var idRol = $("#dp_Roles").val();
            var nombreModulo = e.dataItem["descripcionMenu"];
            var idMenu = e.dataItem["idMenu"];
            var nombreRol = $('#dp_Roles option:selected').text();

            bootbox.dialog({
                message: "<h4>¿Eliminar el módulo: " + nombreModulo + " del rol seleccionado?</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/Roles/eliminarMenuRol?idRol=" + idRol + "&idMenu=" + idMenu + "&nombreRol=" + nombreRol + "&nombreModulo=" + nombreModulo, null,
                                function (data) {
                                    if (data == "Correcto") {
                                        mensajeCorrecto();
                                        cargarRoles();
                                    }
                                    else {
                                        mensajeIncorrecto();
                                    }
                                },
                                function (error) {
                                    console.log(error);
                                });
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });
        }

        //Asociar un nuevo Menu al Rol seleccionado
        function asociar(e) {
            var idRol = $("#dp_Roles").val();
            var nombreModulo = e.dataItem["descripcionMenu"];
            var idMenu = e.dataItem["idMenu"];
            var nombreRol = $('#dp_Roles option:selected').text();

            bootbox.dialog({
                message: "<h4>¿Asociar el módulo: " + nombreModulo + " al rol seleccionado?</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/Roles/asociarMenuRol?idRol=" + idRol + "&idMenu=" + idMenu + "&nombreRol=" + nombreRol + "&nombreModulo=" + nombreModulo, null,
                                function (data) {
                                    if (data == "Correcto") {
                                        mensajeCorrecto();
                                        cargarRoles();
                                    }
                                    else {
                                        mensajeIncorrecto();
                                    }
                                },
                                function (error) {
                                    console.log(error);
                                });
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });
        }

        function mensajeCorrecto() {
            $("#GrillaRolesActivados.t-grid .t-refresh").trigger('click');
            $("#GrillaRolesDesactivados.t-grid .t-refresh").trigger('click');
            bootbox.dialog({
                message: "<h4>Proceso terminado correctamente</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Aceptar ::",
                        className: "btn btn-login",
                    }
                }
            });
        }

        function mensajeIncorrecto() {
            $("#GrillaRolesActivados.t-grid .t-refresh").trigger('click');
            $("#GrillaRolesDesactivados.t-grid .t-refresh").trigger('click');
            bootbox.dialog({
                message: "<h4>Ha ocurrido un error durante la ejecución del proceso</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Aceptar ::",
                        className: "btn btn-login",
                    }
                }
            });
        }

        function cargarRoles() {
            var valorFiltro = $("#dp_Roles").val();

            var link = "/Roles/getRolesMenu?idRol=" + valorFiltro;
            $.ajax({
                type: "POST",
                url: link,
                data: { dato: valorFiltro },
                dataType: "json",
                success: function (result) {
                    $("#GrillaRolesMenu").data("tGrid").dataBind(result.data);
                },
                error: function (result) {
                    alert(result.message);
                }
            });

            var linkFaltantes = "/Roles/getRolesMenuFaltantes?idRol=" + valorFiltro;
            $.ajax({
                type: "POST",
                url: linkFaltantes,
                data: { dato: valorFiltro },
                dataType: "json",
                success: function (result) {
                    $("#GrillaRolesMenuFaltantes").data("tGrid").dataBind(result.data);
                },
                error: function (result) {
                    alert(result.message);
                }
            });
        }
    </script>
    <br />
    <br />
    <br />
    <ul class="nav nav-tabs">
        <li class="active"><a data-toggle="tab" href="#home">Administración de Roles</a></li>
        <li><a data-toggle="tab" href="#menu1">Administración de Roles - Menú</a></li>
    </ul>

    <div class="tab-content">
        <div id="home" class="tab-pane fade in active">
            <br />
            <fieldset class="scheduler-border">
                <legend>Creación de Roles</legend>

                <label>Nombre del Rol</label>
                <input type="text" id="txtNombreRol" name="txtNombreRol" class="text-box form-control" style="width: 300px;" />
                <input type="button" id="btnAgregar" onclick="crearRoles();" value=":: Agregar ::" class="btn btn-login" />
            </fieldset>

            <div class="row">
                <div class="col-lg-6">
                    <fieldset class="scheduler-border">
                        <legend>Roles Activos</legend>
                        <%= Html.Telerik().StyleSheetRegistrar()
                .DefaultGroup(group => group.Add("telerik.hay.css")) %>

                        <%= Html.Telerik().Grid<GestorDocumental.Controllers.grillaPRoles>()
                .Name("GrillaRolesActivados")
                .DataBinding(d => d.Ajax().Select("getInfoRolesActivados", "Roles"))
                .Columns(columns => {
                    columns.Bound(o => o.idRol).Width(100).Title("ID").HeaderHtmlAttributes(new { style = "height: 40px" });
                    columns.Bound(o => o.nombreRol).Width(100).Title("Nombre del Rol");
                    columns.Command(o => o.Custom("Desactivar").Text("Desactivar Rol")
                        .SendState(false)
                        .Ajax(true)).Width(20);
                })
                
                .Pageable(pager => pager.PageSize(10, new int[] { 10,25,50 })
                        .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
                .Sortable()
                .Filterable()
                .ClientEvents(e => e.OnCommand("desactivar"))
                        %>
                    </fieldset>
                </div>
                <div class="col-lg-6">
                    <fieldset class="scheduler-border">
                        <legend>Roles Inactivos</legend>
                        <%= Html.Telerik().Grid<GestorDocumental.Controllers.grillaPRoles>()
                                .Name("GrillaRolesDesactivados")
                                .DataBinding(d => d.Ajax().Select("getInfoRolesDesactivados", "Roles"))
                                .Columns(columns => {
                                    columns.Bound(o => o.idRol).Width(100).Title("ID").HeaderHtmlAttributes(new { style = "height: 40px" });
                                    columns.Bound(o => o.nombreRol).Width(100).Title("Nombre del Rol");
                                    columns.Command(o => o.Custom("Desactivar").Text("Activar Rol")
                                        .SendState(false)
                                        .Ajax(true)).Width(20);
                                })
                
                                .Pageable(pager => pager.PageSize(10, new int[] { 10,25,50 })
                                        .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
                                .Sortable()
                                .Filterable()
                                .ClientEvents(e => e.OnCommand("activar"))
                        %>
                    </fieldset>
                </div>
            </div>
        </div>

        <div id="menu1" class="tab-pane fade">
            <br />
            <fieldset class="scheduler-border">
                <legend>Roles disponibles</legend>
                <select id="dp_Roles" onchange="cargarRoles();" class="form-control" on style="width: 30%">
                </select>
            </fieldset>

            <div class="row">
                <div class="col-lg-6">
                    <fieldset class="scheduler-border">
                        <legend>Módulos asociados</legend>
                        <%= Html.Telerik().Grid<GestorDocumental.Controllers.grillaRolesMenu>()
                            .Name("GrillaRolesMenu")
                            .Columns(columns => {
                                columns.Bound(o => o.idMenu).Width(100).Title("ID").HeaderHtmlAttributes(new { style = "height: 40px" });
                                columns.Bound(o => o.descripcionMenu).Width(100).Title("Nombre Modulo");
                                columns.Command(o => o.Custom("Eliminar").Text("Eliminar")
                                            .SendState(false)
                                            .Ajax(true)).Width(20);
                            })
             
                            .Pageable(pager => pager.PageSize(100))
                            .Sortable()
                            .Filterable()
                            .ClientEvents(e => e.OnCommand("eliminar"))
                        %>
                    </fieldset>
                </div>

                <div class="col-lg-6">
                    <fieldset class="scheduler-border">
                        <legend>Módulos no asociados</legend>
                        <%= Html.Telerik().Grid<GestorDocumental.Controllers.grillaRolesMenu>()
                        .Name("GrillaRolesMenuFaltantes")
                        .Columns(columns => {
                            columns.Bound(o => o.idMenu).Width(100).Title("ID").HeaderHtmlAttributes(new { style = "height: 40px" });
                            columns.Bound(o => o.descripcionMenu).Width(100).Title("Nombre Modulo");
                            columns.Command(o => o.Custom("Asociar").Text("Asociar")
                                        .SendState(false)
                                        .Ajax(true)).Width(20);
                        })
             
                        .Pageable(pager => pager.PageSize(100))
                        .Sortable()
                        .Filterable()
                        .ClientEvents(e => e.OnCommand("asociar"))
                        %>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>
</asp:Content>