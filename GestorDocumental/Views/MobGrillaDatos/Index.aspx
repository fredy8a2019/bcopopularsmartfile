<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Reporte Devolución Móvil
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Grilla de Resultados - Devoluciones Móvil</h2>
    <%= Html.Telerik().Grid<GestorDocumental.Controllers._mobDatos>()
        .Name("Grid")
        .DataBinding(databinding => databinding.Ajax().Select("_verMobDatos", "MobGrillaDatos"))
        .Columns(colums =>
        {
            colums.Bound(o => o._numRad).Width(60).Title("Número Radicado");
            colums.Bound(o => o._idMobDoc).Width(70).Title("Número de Documento");
            colums.Bound(o => o._numProveedor).Width(70).Title("Número de Proveedor");
            colums.Bound(o => o._fecha).Width(100).Format("{0:D}").Title("Fecha de Devolución");
            colums.Bound(o => o._idSociedad).Width(100).Title("Sociedad");
            colums.Bound(o => o._observaciones).Width(100).Title("Observaciones");
        })

                .ClientEvents(events => events.OnRowDataBound("prueba_detallesResultado"))
                .DetailView(details => details.ClientTemplate(
                    Html.Telerik().Grid<GestorDocumental.Controllers._detalleGrilla>()
                    .Name("<#= _numRad #>")
                    .Columns(columns =>
                    {
                        columns.Bound(a => a._descripcionLChequeo).Width(550).Title("Lista de Chequeo");
                        columns.Bound(a => a._descripcionDocumento).Width(100).Title("Documento");
                    })
                    .DataBinding(databing => databing.Ajax()
                    .Select("_verDetalleGrilla", "MobGrillaDatos", new { idRad = "<#= _numRad #>" }))
                    .Sortable()
                    .Pageable(pag => pag.PageSize(5))
                    .ToHtmlString()
                    
                  ))
        
        .Pageable(paging => paging.PageSize(20))
        .Sortable()
        .Scrollable(scrolling => scrolling.Height(400))
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
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
