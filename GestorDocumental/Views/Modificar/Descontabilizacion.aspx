<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        (function () {
            var _default = {
                loagPage: function () {
                    $(document).ready(function () {
                        _ui.eventClick($("#btnBuscar"), _default.btnBuscarDatos);
                    });
                },

                btnBuscarDatos: function () {
                    if ($("##txtNumeroSAP").val() != "") {
                        transact.ajaxPOST("/Modificar/_GetAllDescontabilizar/", $("#fromDescontabilizar").serialize(), _default.sucessGrilla, _default.error);
                    }
                },

                sucessGrilla: function (data) {
                    console.log(data);
                    var grid = $("#GrillaDescontabilizar").data("tGrid");
                    grid.dataBind(data);
                },

                error: function (error) {
                    console.log(error);
                },

            }

            _default.loagPage();
        })();

        function onEdit(e) {
            if ($("input#idCampo").val() == "1162") {
                $(e.form).find('#valor').parent().append("<select id='valor' name='valor'>" +
                                " <option value='Contabilizado'>Contabilizado</option>" +
                                "<option value='Rechazado'>Rechazado</option>" +
                                "</select>");
                var resultado = $(e.form).find('#valor').parent().find("input[type='text']").val();
                $(e.form).find('#valor').parent().find("input[type='text']").remove();
            }

        }

    </script>
    <link href="../../Content/themes/controls.css" rel="stylesheet" type="text/css" />
    <style>
        #txtNumeroSAP {
            width: 30%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Repoceso Contabilizar
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        GridEditMode mode = (GridEditMode)ViewData["mode"];
        GridButtonType type = (GridButtonType)ViewData["type"];
        GridInsertRowPosition insertRowPosition = (GridInsertRowPosition)ViewData["insertRowPosition"];
    %>
    <form id="fromDescontabilizar">
        <fieldset class="scheduler-border">
            <legend><h2>Reproceso Contabilización</h2></legend>
            <div>
                <label>
                    Código Barras:</label>
                <input type="text" name="txtNumeroSAP" value="" id="txtNumeroSAP" required="required" />
            </div>
            <input type="button" name="name" value=" : :  Buscar  : : " id="btnBuscar" class="btn btn-login" />
            <br /><br />
            <%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.hay.css")) %>
            <%= Html.Telerik().Grid<GestorDocumental.Models.DescontabilizarPropiedades>()
         .Name("GrillaDescontabilizar")
          .DataKeys(keys =>
            {
              keys.Add(p => p.negocio);
              keys.Add(p => p.captura);
              keys.Add(p => p.idCampo);
            })
          .DataBinding(dataBinding =>
          {
              dataBinding.Ajax()
              .Update("_EditRowDes", "Modificar");
        })
         .Columns(columns => {
                    columns.Bound(p => p.negocio).Hidden();
                    columns.Bound(p => p.captura).Hidden();
                    columns.Bound(p => p.idCampo).Hidden();
                    columns.Bound(p => p.usuario).Title("Usuario").ReadOnly(true).HeaderHtmlAttributes(new { style = "height: 40px" });
                    columns.Bound(p => p.NomCampo).ReadOnly(true);
                    columns.Bound(p => p.valor).Title("Valor");
                    columns.Bound(p => p.FechaRegistro).Format("{0:dd/MM/yyyy}").ReadOnly(true);
                    columns.Command(Command =>
                    {
                        Command.Edit().ButtonType(type);

                    });
                }).ClientEvents(events => events.OnEdit("onEdit"))
        .Pageable(c => c.PageSize(1000))
        .Scrollable(scrolling => scrolling.Height(300))
        .Sortable()
        .Filterable()
        .Resizable(resizing => resizing.Columns(true))    
            %>
        </fieldset>
    </form>
</asp:Content>
