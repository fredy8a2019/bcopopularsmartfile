<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Reimprimir Código de Barras
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
<h3>Codigos  de Barras</h3>
        <%= Html.Telerik().Grid<GestorDocumental.Controllers.GrillaCodigosBarras>()
        .Name("Grid")
        .DataBinding(dataBinding => dataBinding.Ajax().Select("_GrillaCodigosBarras", "Radicacion"))    
        .Columns(columns =>
        {
            columns.Bound(o => o._CodigoBarras).Width(130).Title("Codigo Barras");
            columns.Bound(o => o._NomCliente).Width(100).Title("Cliente").Visible(false);
            columns.Bound(o => o._NomOficina).Width(100).Title("Oficina");
            columns.Bound(o => o._NomProducto).Width(170).Title("Producto");
            columns.Bound(o => o._NomSubProducto).Width(100).Title("Sub Producto");
            columns.Bound(o => o._NomImpresora).Width(100).Title("Impresora");
            columns.Bound(o => o._NomSociedad).Width(200).Title("Sociedad");
            columns.Bound(o => o._NomUsuario).Width(100).Title("Usuario");
            columns.Bound(o => o.FechaRadicacion).Width(120).Title("Fecha \n Radicacion");
            columns.Bound(o => o.FechaImpreso).Width(110).Title("Fecha \n Impreso");
            columns.Bound(o => o.idCodBarras).Width(100).Visible(false);
            columns.Bound(o => o.idradicaicion).Width(100).Visible(false);
            columns.Command(o => o.Custom("Imprimir").Text("Imprimir")
                                                    .SendState(true)
                                                    .DataRouteValues(route => route.Add(x => x.idCodBarras).RouteKey("idCodBarras"))
                                                    .Ajax(true)
                                                    .Action("Imprimir", "Radicacion"))
                                                    .Width(90);
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
              if (e.name == "Imprimir") {
                  alert("Codigo impreso");
              }
          }

          function OnComplete(e) {
              //              if (e.name == "Imprimir") {
              //                  var grid = $("#Grid").data("tGrid");
              //                  grid.ajaxRequest();
              //              }
          }


    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>