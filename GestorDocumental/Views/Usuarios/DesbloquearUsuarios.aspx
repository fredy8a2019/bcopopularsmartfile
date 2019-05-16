<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Desbloquear Usuarios
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Scripts/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="../../Scripts/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />
    <%--<link href="../../Styles/BootsTrap/css/bootstrap.min.css" rel="stylesheet" />--%>
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
    <%--<link href="../../Styles/dataTables.bootstrap.min.css" rel="stylesheet" />--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <h3>Desbloquear Usuarios</h3>
    <fieldset class="scheduler-border">
        <legend class="scheduler-border"></legend>
        <label>
        </label>
        <%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.hay.css")) %>
        <%= Html.Telerik().Grid<GestorDocumental.Controllers.listaDesbloquearUsuarios>()
                .Name("GridConsulta").DataBinding(databinding => databinding.Ajax().Select("_UsuariosBloqueados", "Usuarios"))
                .Columns(colums =>
                {
                    colums.Bound(o => o.ID_Usuario).Width(100).Title("Id Usuario").HeaderHtmlAttributes(new { style = "height: 30px;" });
                    colums.Bound(o => o.UsuDominio).Width(100).Title("Dominio Usuario");
                    colums.Bound(o => o.NomUsuario).Width(150).Title("Nombre");
                    colums.Bound(o => o.Rol).Width(100).Title("Rol");
                    colums.Bound(o => o.fecha).Width(150).Title("Fecha");
                    colums.Command(o => o.Custom("Desbloquear").Text("Activar").HtmlAttributes(new { onclick = "refrescaGrillaTipologia(this)" })
                                                                .SendState(true)
                                                                .DataRouteValues(route =>
                                                                {
                                                                 route.Add(x => x.ID_Usuario).RouteKey("Id_Usuario");                                                                 
                                                                })
                                                                .Ajax(true)
                                                                .Action("ActivarUsuario", "Usuarios")).HtmlAttributes(new { onclick = "refrescaGrillaTipologia()" })
                                                                .Width(70).Title("Desbloquear");
                    colums.Command(o => o.Custom("Reset Pass").Text("Reset Pass").HtmlAttributes(new { onclick = "refrescaGrillaTipologia(this)" })
                                                                .SendState(true)
                                                                .DataRouteValues(route =>
                                                                {
                                                                    route.Add(x => x.ID_Usuario).RouteKey("Id_Usuario");
                                                                })
                                                                .Ajax(true)
                                                                .Action("ResetPass", "Usuarios")).HtmlAttributes(new { onclick = "refrescaGrillaTipologia()" })
                                                                .Width(70).Title("Reset Pass");
                 }) .Pageable(pager => pager.PageSize(10, new int[] { 15,20,30 })
                .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
        .Scrollable(scrolling => scrolling.Height(300))
        .Sortable()
        .Filterable()
        .Resizable(resizing => resizing.Columns(true))
        %>
    </fieldset>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
    <script>

        function refrescaGrillaTipologia() {
            $(".t-grid .t-refresh").trigger('click');
        }

    </script>
</asp:Content>