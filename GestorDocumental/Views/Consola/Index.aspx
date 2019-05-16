<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Consola de carga de Ficheros XML
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <h3>Consola de carga de Ficheros XML</h3>

    <%--<%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.common.css")
                                       .Add("telerik.Forest.css")) %>--%>
    <%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.hay.css")) %>
    <%= Html.Telerik().Grid<GestorDocumental.Controllers.grillaResultado>()
        .Name("Grid")
        .DataBinding(databinding => databinding.Ajax().Select("_verResultados", "Consola"))
        .Columns(columns =>
        {
            columns.Bound(o => o.no_negocio).Width(100).Title("Id").HeaderHtmlAttributes(new { style = "height: 40px" });
            columns.Bound(o => o.no_negocio_gestor).Width(100).Title("Numero negocio");
            columns.Bound(o => o.no_factura).Width(100).Title("Numero factura");
            columns.Bound(o => o.tipo_documento).Width(100).Title("Tipo documento");
            columns.Bound(o => o.fecha_documento).Format("{0:F}").Width(200).Title("Fecha documento");
            columns.Command(o => o.Custom("Reenviar a SAP").Text("Reenviar a SAP")
                                                        .SendState(true)
                                                        .DataRouteValues(route => route.Add(x => x.no_negocio).RouteKey("idNegocio"))
                                                        .Ajax(true)
                                                        .Action("ReenviarNegocio", "Consola"))
                                                        .Width(100);

            //columns.Command(o => o.Custom("Modificar").Text("Modificar")
            //                                            .SendState(true)
            //                                            .DataRouteValues(route => route.Add(s => s.no_negocio).RouteKey("idNegocio"))
            //                                            .Ajax(true)
            //                                            .Action("nodificarNegocio", "Consola"))
            //                                            .Width(100);
            
        })
                    .ClientEvents(events => events.OnRowDataBound("prueba_detallesResultado"))
                    .DetailView(details => details.ClientTemplate(
                        Html.Telerik().Grid<GestorDocumental.Controllers.grillaResultadoDetalle>()
                        .Name("<#= no_negocio #>")
                        .Columns(columns =>
                        {
                            columns.Bound(a => a.rcode).Width(100).Title("RCODE").HeaderHtmlAttributes(new { style = "height: 40px" });
                            columns.Bound(a => a.ok_code).Width(100).Title("OK CODE");
                            columns.Bound(a => a.descripcion).Width(500).Title("Descripción");
                            columns.Bound(a => a.fecha_respuesta).Format("{0:F}").Title("Fecha de Respuesta");
                        })
                        .DataBinding(databing => databing.Ajax()
                        .Select("_verResultadosDetalle", "Consola", new { idNegocio = "<#= no_negocio_gestor #>" }))
                        .Pageable(pagings => pagings.PageSize(4))                        
                        .Sortable()
                        .ToHtmlString()
                    ))

        .ClientEvents(events2 => events2.OnCommand("ConfirmarEliminar"))
        .Pageable(pager => pager.PageSize(50, new int[] {75,100,150 })
                        .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
        .Sortable()
        .Scrollable(scrolling => scrolling.Height(600))
        .Groupable()
        .Filterable()
    %>

    <script type="text/javascript">

        function expandFirstRow(grid, row) {
            if (grid.$rows().index(row) == 0) {
                grid.expandRow(row);
            }
        }

        function prueba_detallesResultado(e) {
            var grid = $(this).data('tGrid');
            expandFirstRow(grid, e.row);
        }

        function ConfirmarEliminar() {
            var deleteUser = confirm('¿Desea Reenviar la factura Seleccionada?');
            if (deleteUser) {
                return true;
            }
            else {
                return false;
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
