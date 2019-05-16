<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Content/themes/controls.css" rel="stylesheet" type="text/css" />
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script type="text/javascript">
        (function () {
            var _default = {
                loagPage: function () {
                    $(document).ready(function () {
                        _ui.eventClick($("#btnBuscar"), _default.btnBuscarDatos);
                        _ui.eventClick($("input[type='checkbox']"), _default._accionCheck);
                        $("#txtNegocio").attr("disabled", "disabled");
                        $("#chkNumLote").attr("checked", true);
                    });
                },

                btnBuscarDatos: function () {
                    if ($("#NumeroLote").val() != "" || $("#Negocio").val() != "") {
                        transact.ajaxPOST("/Modificar/_GetAll/", $("#FormId").serialize(), _default.sucessGrilla, _default.error);
                    } else {
                    }

                },

                sucessGrilla: function (data) {
                    console.log(data);
                    var grid = $("#GrillaCodigos").data("tGrid");
                    grid.dataBind(data);
                },

                error: function (error) {
                    console.log(error);
                },

                _accionCheck: function (e) {
                    var chek = e.currentTarget.checked;
                    var idCampo = e.currentTarget.id;
                    if (chek == true) {
                        switch (idCampo) {
                            case "chkNumLote":
                                $("#txtLote").removeAttr("disabled");
                                $("#txtNegocio").attr("disabled", "disabled");
                                $("#txtNegocio").val("");
                                $("#chkNegocio").attr("checked", false);
                                break;
                            case "chkNegocio":
                                $("#txtNegocio").removeAttr("disabled");
                                $("#txtLote").attr("disabled", "disabled");
                                $("#txtLote").val("");
                                $("#chkNumLote").attr("checked", false);
                                break;
                            default:

                        }
                    } else {
                        switch (idCampo) {
                            case "chkNumLote":
                                $("#txtLote").attr("disabled", "disabled");
                                $("#txtNegocio").removeAttr("disabled");
                                break;
                            case "chkNegocio":
                                $("#txtNegocio").attr("disabled", "disabled");
                                $("#txtLote").removeAttr("disabled");
                                break;
                            default:

                        }
                    }
                }

            }

            _default.loagPage();
        })();
    </script>
    <style type="text/css">
        .divCampos {
            width: 50%;
            float: left;
        }

            .divCampos input[type='text'] {
                float: right;
                margin-right: 23%;
                width: 45%;
            }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Modificar Código de Barras
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Correcciones Label</h3>
    <%
        GridEditMode mode = (GridEditMode)ViewData["mode"];
        GridButtonType type = (GridButtonType)ViewData["type"];    
    %>
    <form id="FormId">
        <fieldset class="scheduler-border">
            <legend class="scheduler-border"></legend>
            <div class="divCampos">
                <input type="checkbox" name="name" value="" id="chkNumLote" />
                <label>
                    Numero del lote:</label>
                <input type="text" name="NumeroLote" value="" id="txtLote" required="required" />
            </div>
            <div class="divCampos">
                <input type="checkbox" name="name" value="" id="chkNegocio" />
                <label>
                    Numero del negocio:</label>
                <input type="text" name="Negocio" value="" id="txtNegocio" required="required" /><br />
            </div>
            <input type="button" name="name" value=":: Buscar ::" id="btnBuscar" class="btn btn-login" />
            <br />
            <br />
            <%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.hay.css")) %>
            <%= Html.Telerik().Grid<GestorDocumental.Models.ModificarCodigosPropiedades>()
         .Name("GrillaCodigos")
          .DataKeys(keys =>
            {
              keys.Add(p => p.negocio);
            })
          .DataBinding(dataBinding =>
          {
              dataBinding.Ajax()
              .Update("_EditRow", "Modificar");
        })
         .Columns(columns => {
             columns.Bound(p => p.negocio).Title("Negocio").ReadOnly(true).HeaderHtmlAttributes(new { style = "height: 40px" });
                    columns.Bound(p => p.CodigoBarras).Title("Código de Barras");
                    columns.Command(Command =>
                    {
                        Command.Edit().ButtonType(type);

                    });
                })
                    .Pageable(c => c.PageSize(1000))
        .Scrollable(scrolling => scrolling.Height(300))
        .Sortable()
        .Filterable()
        .Resizable(resizing => resizing.Columns(true))    
            %>
        </fieldset>
    </form>
</asp:Content>
