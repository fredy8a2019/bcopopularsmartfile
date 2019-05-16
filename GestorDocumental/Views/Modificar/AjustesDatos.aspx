<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Scripts/Reestricciones.js"></script>
    <script type="text/javascript">
        var _default = null;
        (function () {
            _default = {
                loagPage: function () {
                    $(document).ready(function () {
                        _ui.eventClick($("#btnBuscar"), _default.btnBuscarDatos);
                    });
                },

                btnBuscarDatos: function () {
                    transact.ajaxPOST("/Modificar/_GetAllAjusteDatos/", $("#datos").serialize(), _default.sucessGrilla, _default.error);
                },

                sucessGrilla: function (data) {
                    console.log(data);
                    var grid = $("#GrillaAjustarDatos").data("tGrid");
                    grid.dataBind(data);
                },

                error: function (error) {
                    console.log(error);
                },

            }

            _default.loagPage();
        })();

        function onSave(e) {

            setTimeout('window.location.reload()', 2000);

        }

        function onEdit(e) {

        }

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    AjustesDatos
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        GridEditMode mode = (GridEditMode)ViewData["mode"];
        GridButtonType type = (GridButtonType)ViewData["type"];
        GridInsertRowPosition insertRowPosition = (GridInsertRowPosition)ViewData["insertRowPosition"];
    %>
    <form id="datos">
        <fieldset class="scheduler-border">
            <legend><h2>AjustesDatos</h2></legend>
            <div>
                <label>
                    Negocio:</label>
                <input type="text" onkeypress="return numbersonly(event);" name="txtNegocio" id="txtNegocio" class="form-control" style="width: 200px;" />
                <br />
            </div>
            <input type="button" name="name" value=" : :  Buscar  : : " id="btnBuscar" class="btn btn-login" />
            <br />
            <br />
            <%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.hay.css")) %>
            <%=Html.Telerik().Grid<GestorDocumental.Models.AjusteDatosNegocioPropiedades>()
         .Name("GrillaAjustarDatos")
          .DataKeys(keys =>
            {
                keys.Add(p => p.IdCampo);
                keys.Add(p => p.Negocio);
            })
          .DataBinding(dataBinding =>
          {
               dataBinding.Ajax()
              .Update("_EditRowData", "Modificar");
        })
         .Columns(columns => {
                    columns.Bound(p => p.Negocio).Hidden();
                    columns.Bound(p => p.IdCampo).Hidden();
                    columns.Bound(p => p.tipoCampos).Hidden();
                    columns.Bound(p => p.Captura).Hidden();
                    columns.Bound(p => p.TipDoc).Hidden();
                    columns.Bound(p => p.DescripcionCampo).ReadOnly(true).HeaderHtmlAttributes(new { style = "height: 40px" });
                    columns.Bound(p => p.Indice);
                    columns.Bound(p => p.Valor);
                    columns.Command(Command =>
                    {
                        Command.Edit().ButtonType(type);

                    });
                }).ClientEvents(events => events.OnEdit("onEdit").OnSave("onSave"))
        .Pageable(c => c.PageSize(300))
        .Scrollable(scrolling => scrolling.Height(500))
        .Sortable()
        .Filterable()
        .Resizable(resizing => resizing.Columns(true))  


            %>
        </fieldset>
    </form>
</asp:Content>
