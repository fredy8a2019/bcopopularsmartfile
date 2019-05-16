<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
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
    <link href="../../Content/themes/controls.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <style type="text/css">
        body {
            font-family: Calibri !important;
        }

        input[type="text"] {
            margin-top: 8px;
        }

        .custom-combobox {
            width: 300%;
            font-size: 70%;
        }

        div#contentImg {
            position: absolute;
            margin: -13% 0px 0px 50%;
        }

        #ulSubproductos, #ulSubproductosSelected {
            list-style-type: none;
            margin: 0;
            padding: 0 0 2.5em;
            float: left;
            margin-right: 10px;
            background-color: rgb(235, 235, 228);
            border-radius: 5px;
            margin: 7px 0px 0px 0px;
            width: 230px;
        }

            #ulSubproductos li, #ulSubproductosSelected li {
                margin: 5px 5px 5px 5px;
                padding: 5px;
                font-size: smaller;
                width: 95%;
                box-shadow: 2px 2px 8px rgba(0,0,0,0.8);
            }

        input[name=sub] {
            float: left;
            display: inline !important;
            width: 14%;
            margin-top: 10px;
        }

        li input[type="text"] {
            background-color: rgb(235, 235, 228);
            margin: 0px 0px 0px 0px !important;
            border: solid 0px rgb(235, 235, 228);
            color: Black;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    CrearUnidad
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Crear Unidad Documental</h2>
    <form action="/Almacenar/Guardar" method="post">
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Datos Principales</legend>
            <table>
                <tr>
                    <td>
                        <label>Cliente</label>
                    </td>
                    <td>
                        <input type="text" id="Text2" name="Cliente" value="<%= ViewData["Cliente"] %>" disabled="disabled" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Oficina</label>
                    </td>
                    <td>
                        <select id="sleOficina" name="oficina" data-work="oficina">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Tipo Contenedor</label>
                    </td>
                    <td>
                        <select id="sleTipoContenedor" name="tipoContenedor" data-work="tipoContenedor">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Destino</label>
                    </td>
                    <td>
                        <select id="sleDestino" name="destinoAlmacenamiento" data-work="destinoAlmacenamiento">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Producto</label>
                    </td>
                    <td>
                        <select id="sleProductoAlmacenamiento" name="ProductoAlmacenamiento" data-work="ProductoAlmacenamiento">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Subproductos</label>
                    </td>
                    <td>
                        <div>
                            <ul id="ulSubproductos" class="connectedSortable">
                                <li class=""></li>
                            </ul>
                            <input type="button" id="btnSelect" name="sub" value=">" class="btn btn-login" />
                            <input type="button" id="btnDesSelect" name="sub" value="<" class="btn btn-login" />
                            <input type="button" id="btnTodo" name="sub" value="Todo" class="btn btn-login" />
                        </div>
                    </td>
                    <td>
                        <div>
                            <ul id="ulSubproductosSelected" class="connectedSortable">
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="contentImg">
                <img src="../../Content/Images/caja.png" alt="Caja" />
            </div>
        </fieldset>
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Datos Complementarios</legend>
            <table id="tblContentCampos">
                <tbody>
                </tbody>
            </table>
        </fieldset>
        <input type="submit" name="btnGuardar" id="btnGuardar" value=":: Guardar ::" class="btn btn-login" style="width: auto" />
        <input type="button" name="name" id="btnCancelar" value=":: Cancelar ::" class="btn btn-login" />
    </form>
    <script type="text/javascript">
        $.noConflict();
        var _default = null;
        (function ($) {
            _default = {
                loadData: function () {
                    transact.ajaxPOST("/Listas/_GetTipoContenedor", null, _default.successTipoContenedorUD, _default.error);
                    transact.ajaxPOST("/Listas/_GetDestino", null, _default.successDestinoUD, _default.error);
                    transact.ajaxPOST("/Radicacion/_GetDropDownListProductos/", null, _default._successProductos, _default._error);
                    transact.ajaxPOST("/Radicacion/_GetDropDownList_Oficinas/", null, _default._successOficinas, _default._error);
                },

                loadPages: function () {
                    $(document).ready(function () {
                        _ui.buildCombobox();
                        $("#sleTipoContenedor").combobox();
                        $("#sleDestino").combobox();
                        $("#sleProductoAlmacenamiento").combobox();
                        $("#sleOficina").combobox();
                        $("#sleTipoContenedor").parent().find("input[autocomplete='off']").attr("placeholder", "Seleccioné un tipo");
                        $("#sleDestino").parent().find("input[autocomplete='off']").attr("placeholder", "Seleccioné un destino");
                        $("#sleProductoAlmacenamiento").parent().find("input[autocomplete='off']").attr("placeholder", "Seleccioné un producto");

                        //BTN 
                        _ui.eventClick($("#btnSelect"), _default.clickSelecionarSubProdc);
                        _ui.eventClick($("#btnDesSelect"), _default.clickDesSelecionarSubProdc);
                        _ui.eventClick($("#btnTodo"), _default.chickTodo);
                    });
                },

                successTipoContenedorUD: function (data) {
                    _ui.fillCombo($("#sleTipoContenedor"), data);
                },

                successDestinoUD: function (data) {
                    _ui.fillCombo($("#sleDestino"), data);
                },

                _successProductos: function (data) {
                    _ui.fillCombo($("#sleProductoAlmacenamiento"), data);
                },

                _successOficinas: function (data) {
                    _ui.fillCombo($("#sleOficina"), data);
                },

                error: function (error) {
                    console.log(error)
                },

                clickSelecionarSubProdc: function (e) {

                    var liSelected = $("#ulSubproductos li[data-selected='true']");

                    $.each(liSelected, function (i, values) {
                        values = $(values);
                        var texto = values.text();
                        values.text("");
                        values.append(_ui.createElement("input", {
                            "value": texto,
                            "name": values.attr("name"),
                            "readonly": "true",
                            "type": "text"
                        }, ""));
                        values.removeAttr("style");
                    });
                    $("#ulSubproductosSelected").append($("#ulSubproductos li[data-selected='true']"));
                    $("#ulSubproductosSelected li").attr("data-selected", "falso");
                },

                clickDesSelecionarSubProdc: function (e) {
                    var liSelected = $("#ulSubproductosSelected li[data-selected='true']");

                    $.each(liSelected, function (i, values) {
                        values = $(values);
                        var texto = values.find("input").val();
                        values.find("input").remove();
                        values.removeAttr("style");
                        values.text(texto);

                    });

                    $("#ulSubproductos").append($("#ulSubproductosSelected li[data-selected='true']"));
                    $("#ulSubproductos li").attr("data-selected", "falso");
                },

                chickTodo: function (e) {
                    var liSelected = $("#ulSubproductos li");

                    $.each(liSelected, function (i, values) {
                        values = $(values);

                        values.css({
                            "background-color": "#9AAE04",
                            "color": "White"
                        });

                        var texto = values.text();
                        values.text("");
                        values.append(_ui.createElement("input", {
                            "value": texto,
                            "name": values.attr("name"),
                            "readonly": "true",
                            "type": "text"
                        }, ""));


                        values.removeAttr("style");
                    });

                    $("#ulSubproductosSelected").append($("#ulSubproductos li"));
                    $("#ulSubproductosSelected li").attr("data-selected", "falso");
                }
            }

            _default.loadData();
            _default.loadPages();

        })(jQuery);
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
