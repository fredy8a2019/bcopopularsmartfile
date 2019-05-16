<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Servicio de cargue
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Servicio de cargue</h2>
    <%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.hay.css")) %>
    <%= Html.Telerik().Grid<GestorDocumental.Controllers.GrillaServicio>()
        .Name("Grid")
        .DataBinding(dataBinding => dataBinding.Ajax().Select("_AjaxBinding", "ServicioCargue"))    
        .Columns(columns =>
        {
            columns.Bound(o => o.Lote).Width(100).HeaderHtmlAttributes(new { style = "height: 40px" });
            columns.Bound(o => o.FechaCreacion).Width(100);
            columns.Bound(o => o.Principlales).Width(100);
            columns.Bound(o => o.Estado).Width(100);
            columns.Command(o => o.Custom("Importar").Text("Importar")
                                                    .SendState(true)
                                                    .DataRouteValues(route => route.Add(x => x.Lote).RouteKey("Lote"))
                                                    .Ajax(true)
                                                    .Action("Importar", "ServicioCargue"))
                                                    .Width(100);
        })
         .ClientEvents(events => events.OnCommand("onCommand")
                                       .OnComplete("OnComplete"))      
        .Pageable()
        .Sortable()
                .Scrollable(scrolling => scrolling.Height(400))
        .Groupable()
        .Filterable()
        .Resizable(resizing => resizing.Columns(true))
    %>
    <script type="text/javascript">
        function onCommand(e) {
            if (e.name == "Importar") {
                alert("Lote importado");
            }
        }

        function OnComplete(e) {
            if (e.name == "Importar") {
                var grid = $("#Grid").data("tGrid");
                grid.ajaxRequest();
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
