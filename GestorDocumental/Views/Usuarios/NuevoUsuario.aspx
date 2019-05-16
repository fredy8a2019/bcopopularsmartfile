<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"> 
    Administración Usuarios
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        GridEditMode mode = (GridEditMode)ViewData["mode"];
        GridButtonType type = (GridButtonType)ViewData["type"];
        GridInsertRowPosition insertRowPosition = (GridInsertRowPosition)ViewData["insertRowPosition"];
    
    %>
    <br />
    <br />
    <h3>Administración Usuarios</h3>

    <%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.hay.css")) %>
    <%= Html.Telerik().Grid<GestorDocumental.Models.UsuariosPropiedades>()
        .Name("Grid")
        .DataKeys(keys =>
        {
            keys.Add(p => p.idUsusario);
        })
        .ToolBar(commands => commands.Insert())
        .DataBinding(dataBinding =>
        {
            dataBinding.Ajax()
                .Select("_SelectAjaxEditing", "Usuarios")
                .Insert("_InsertAjaxEditing", "Usuarios")
                .Update("_SaveAjaxEditing", "Usuarios");
        })
        .Columns(columns =>
        {
            columns.Bound(p => p.idUsusario).Width(120).Title("Usuario").HeaderHtmlAttributes(new { style = "height: 40px" });
            columns.Bound(p => p.usuarioDominio).Width(70).Title("Usuario LDAP");
            columns.Bound(p => p.Nombre).Width(170);
            columns.Bound(p => p.nitCliente).Width(190).Title("Cliente");
            columns.Bound(p => p.rol).Width(160).Title("Rol");
            columns.Bound(p => p.activo)
                .ClientTemplate("<input type='checkbox' disabled='disabled'name='activo' <#= activo? checked='checked' : '' #> />").Width(60).Title("Activo");
            columns.Bound(p => p.impresora).Width(150);
            columns.Bound(p => p.FechaRegistro).Format("{0:dd/MM/yyyy}").Width(100); 
            columns.Command(commands =>
            { commands.Edit().ButtonType(type); }).Width(90);
        }).ClientEvents(events => events.OnEdit("onEdit"))
          .Editable(editing => editing
                .InsertRowPosition(insertRowPosition)
                .Mode(mode))
        //.Pageable(pager => pager.PageSize(75, new int[] { 100,150,175 }) //JFPancho; se cambia cantidad de paginas
        .Pageable(pager => pager.PageSize(10, new int[] { 15,20,30 })
                .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
        .Scrollable(scrolling => scrolling.Height(500))
        .Sortable()
        .Filterable()
        .Resizable(resizing => resizing.Columns(true))
    %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../../Content/Scripts/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script type="text/javascript">
        var datos = {};
        var datosCliente = {};
        (function () {

            var _default = {
                init: function () {
                    transact.ajaxPOST("/Listas/_GetDropDownRolesApp", null, _default._successRoles, _default._error);
                    transact.ajaxPOST("/Listas/_GetDropDownListClientes", null, _default._sucessCliente, _default._error);
                },

                _successRoles: function (data) {
                    datos = data;
                },

                _sucessCliente: function (data) {
                    datosCliente = data;
                },

                _error: function (error) {
                    console.log(error);
                }
            }

            _default.init();
        })();
        function onEdit(e) {
            //Manipula el rol
            $(e.form).find('#rol').parent().append("<select id='rol' name='rol'></select>");

            var resultado = $(e.form).find('#rol').parent().find("input[type='text']").val();
            $(e.form).find('#rol').parent().find("input[type='text']").remove();
            _ui.fillCombo($(e.form).find('#rol'), datos);
            ($("#rol").children()).each(function (i, item) {
                if ($(item).text() == resultado) {
                    $(item).attr("selected", true);
                }
            });

            //Manipula el rol
            $(e.form).find('#nitCliente').parent().append("<select id='nitCliente' name='nitCliente'></select>");
            var resultado = $(e.form).find('#nitCliente').parent().find("input[type='text']").val();
            $(e.form).find('#nitCliente').parent().find("input[type='text']").remove();
            _ui.fillCombo($(e.form).find('#nitCliente'), datosCliente);
            ($("#nitCliente").children()).each(function (i, item) {
                if ($(item).text() == resultado) {
                    $(item).attr("selected", true);
                }
            });

            _ui.builDatepicker($(e.form).find('#FechaRegistro'));

            if ($(e.form).find('#idUsusario').parent().find("input[type='text']").val() == 0) {
                $(e.form).find('#idUsusario').parent().find("input[type='text']").remove("disabled", "disabled");
            } else {
                $(e.form).find('#idUsusario').parent().find("input[type='text']").attr("disabled", "disabled");
            }

        }
    </script>
    <style type="text/css">
        .field-validation-error {
            position: absolute;
            display: block;
        }

        * html .field-validation-error {
            position: relative;
        }

        * + html .field-validation-error {
            position: relative;
        }

            .field-validation-error span {
                position: absolute;
                white-space: nowrap;
                color: red;
                padding: 17px 5px 3px;
                background: transparent url('<%= Url.Content("~/Content/Common/validation-error-message.png") %>') no-repeat 0 0;
            }

        /* in-form editing */
        .t-edit-form-container {
            width: 350px;
            margin: 1em;
        }

            .t-edit-form-container .editor-label, .t-edit-form-container .editor-field {
                padding-bottom: 1em;
                float: left;
            }

            .t-edit-form-container .editor-label {
                width: 30%;
                text-align: right;
                padding-right: 3%;
                clear: left;
            }

            .t-edit-form-container .editor-field {
                width: 60%;
            }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
