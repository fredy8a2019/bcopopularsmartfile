<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="GestorDocumental.Models" %>
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

    <style>
        
         .style1
        {
            width: 100%;
        }
        input[type="text"]
        {
            margin-top: 8px;
        }
         .distanciaLeft
        {
            width:10%;    
        }
        
        .distanciaright
        {
            width:15%;
        }
        
           .custom-combobox {
        position: relative;
        display: inline-block;
        width: 100%;
        }

     
        .custom-combobox-toggle {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
            /* support: IE7 */
            *height: 1.7em;
            *top: 0.1em;    
        }

        .custom-combobox-input {
           margin: 0;
           padding: 0.3em;
           width: 85%;   
        }
        
          #fechaInicial, #fechaFin
        {
            width: 100%;
            height: 1.5em;
            border-radius: 5px;
            border: solid 1px #999;
            padding: 2px 3px 2px 3px;
            margin-bottom: 15px;
            }
    </style>
    <% Html.DevExpress().RenderStyleSheets(Page,
        new StyleSheet { ExtensionSuite = ExtensionSuite.PivotGrid, Theme = "Glass" }
    );
    %>
    <% Html.DevExpress().RenderScripts(Page,
        new Script { ExtensionSuite = ExtensionSuite.PivotGrid }
    ); %>
    <script type="text/javascript">
        function exportar() {
            pivotGrid.PerformCallback();
            //$("#btnExportar").attr("disabled", "disabled");
            $("#btnDescargar").removeAttr("disabled");
        }

        function descargar() {
            window.location.href = "/Report/Archivo";
        } 
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    FacturacionIndex
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>
        Cantidades</h3>
    <div>
        <form action="/Report/Filtros" method="post">
        <table>
            <tr>
                <td class="distanciaLeft">
                    <label>
                        Fecha Inicio:</label>
                </td>
                <td>
                    <%=
                     Html.Telerik().DatePicker()
                      .Name("fechaInicial")
                      .Format("dd-MM-yyyy")
                      .Value(Session["FechaInial"].ToString())                     
                     
                    %>
                </td>
                <td class="distanciaright">
                    <label>
                        Fecha Fin:</label>
                </td>
                <td>
                    <%= 
                      Html.Telerik().DatePicker()
                      .Name("fechaFin")
                      .Format("dd-MM-yyyy")
                      .Value(Session["FechaFin"].ToString())
                    %>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        Cliente:
                    </label>
                </td>
                <td>
                    <input type="hidden" name="Cliente" value="" id="hClientes" />
                    <select id="ddlClientes" name="" data-work="clientes" data-name="facturacion">
                    </select>
                </td>
                <td>
                    <label>
                        Oficinas:
                    </label>
                </td>
                <td>
                    <input type="hidden" name="Oficinas" value="" id="hOficinas" />
                    <select id="ddlOficinas" name="" data-work="oficinas">
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        Producto:
                    </label>
                </td>
                <td>
                    <input type="hidden" name="Productos" value="" id="hProductos" />
                    <select id="ddlProductos" name="" data-work="productos" data-name="facturacion">
                    </select>
                </td>
                <td>
                    <label>
                        Sociedades:
                    </label>
                </td>
                <td>
                    <input type="hidden" name="Sociedad" value="" id="hSociedad" />
                    <select id="ddlSociedad" name="" data-work="Sociedad" data-name="facturacion">
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="submit" name="name" value=":: Filtar ::" class="boton" />
                </td>
                <td>
                </td>
            </tr>
        </table>
        <br />
        </form>
    </div>
    <%                   
        Html.RenderPartial("PivotGridPartialFacturacion");
    %>
    <input type="submit" name="name" value=":: Generar Factura ::" id="btnExportar" class="boton" onclick="exportar()" <%--onclick="tableToExcel('pivotGrid_MT', 'W3C Example Table')"--%>/>
    <input type="button" name="name" value=":: Descargar Factura ::" id="btnDescargar" class="boton" onclick="descargar()" />
    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <script type="text/javascript">
        (function () {
            var _default = {
                loadData: function () {
                    transact.ajaxPOST("/Listas/_GetDropDownListClientes/",
                                      null,
                                       _default._successClientes,
                                       _default._error);
                },

                _successClientes: function (data) {
                    _ui.fillCombo($("#ddlClientes"), data);
                },

                loagPage: function () {
                    $("#btnDescargar").attr("disabled", "disabled");
                    _ui.buildCombobox();
                    $("#ddlClientes").combobox();
                    $("#ddlOficinas").combobox();
                    $("#ddlProductos").combobox();
                    $("#ddlSociedad").combobox();

                    $("#ddlClientes").parent().find("input[autocomplete='off']").val('<%=Session["Cliente"]%>');
                    $("#ddlOficinas").parent().find("input[autocomplete='off']").val('<%=Session["Oficina"]%>');
                    $("#ddlProductos").parent().find("input[autocomplete='off']").val('<%= Session["Producto"]%>');
                    $("#ddlSociedad").parent().find("input[autocomplete='off']").val('<%= Session["Sociedad"]%>');
                }

            }

            _default.loadData();
            _default.loagPage();
        })();
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
