<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    .:: Gestor Documental ::.
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 
<%  Html.Telerik().Splitter().Name("Splitter")
            .HtmlAttributes(new { style = "height: 100%;" })
            .Orientation(SplitterOrientation.Vertical)
            .Panes(vPanes =>
            {
                vPanes.Add()
                    .Size("50%")
                    .Collapsible(true)
                    .Content(() =>
                    {%>
                       <object 
                       width="100%" 
                       height="100%"
                       id="tiffobj0"
                       classid="CLSID:106E49CF-797A-11D2-81A2-00E02C015623">
                        <%--<param name="src" value="/Content/ArchivosCliente/405/405.tif">--%>
                        <param name="src" value="<%: Session["IMG_VISOR"] %>">                        
                        <param name=enableevents value="32">
                        <embed toolbaritems="<%: Session["TOOL_BAR"] %>" width="100%" 
                        height="100%"
                        id="tiffemb0"
                        enableevents="32"
                        src="<%: Session["IMG_VISOR"] %>" 
                        access="8"
                        type="application/x-alternatiff">
                       </object>
                    <%});
                vPanes.Add()
                    .Size("30%")
                    .Collapsible(true)
                    .Content(() =>
                    {%>
                       
                       <table>
                       <tr>
                        <td>
                            <input id="Button1" class="boton" type="button" value="Eliminar" onclick="Eliminar_onclick()" /></td>
                        <td>
                            <input id="Button2" type="button" class="boton" value="Rotar" onclick="Rotar_onclick()" /></td>
                        <td>
                            <input id="Button3" type="button" class="boton" value="Adicionar" onclick="return Adicionar_onclick()" /></td>
                            <td>
                            <input id="Button4" type="button" class="boton" value=".:: Aplicar cambios ::." onclick="Aplicar_onclick()" /></td><td>
                            <input id="finalizarLoteo" type="button"  class="boton" value="Finalizar loteo" onclick="return finalizarLoteo_onclick()" /></td>
                        </tr>
                       </table>

                       <%= Html.Telerik().Grid((IEnumerable<GestorDocumental.Models.LoteoCambios>)ViewBag.cambios)
                            .Name("GrillaCambios")                            
                            .DataKeys(keys => keys.Add(c => c.id))
                            //.ToolBar(commands => commands.Insert())
                            .DataBinding(dataBinding => dataBinding.Ajax()
                                .Select("SelectCambios","Loteo")
                             )
                            //    .Select("EditingServerSide", "Grid", new { mode = mode, type = type })
                            //    .Insert("Insert", "Grid", new { mode = mode, type = type })
                            //    .Update("Save", "Grid", new { mode = mode, type = type })
                            //    .Delete("Delete", "Grid", new { mode = mode, type = type }))
                            .Columns(columns =>
                            {
                                columns.Bound(p => p.pagina).Width(210).Title("Numero pagina");
                                columns.Bound(p => p.rotar).Title("Rotar ?");
                                columns.Bound(p => p.eliminar).Width(120).Title("Eliminar ?");
                                columns.Bound(p => p.adicionar).Title("Adicionar ?");
                                columns.Bound(p => p.rutaAdicionar).Title("Imagen adicionar");
                                //columns.Command(commands =>
                                //{
                                //    //commands.Edit().ButtonType(GridButtonType.Image);
                                //    //commands.Delete().ButtonType(GridButtonType.Image);
                                //}).Width(200).Title("");
                            })
                            .Editable(editing => editing.Mode(GridEditMode.InLine))
                            .Pageable()
                            .Sortable()
                    %>

                    <%});
            })
            .Render();
    %>
    <% Html.Telerik().Window()
           .Name("WindowUpload")
           .Title("Telerik Window for ASP.NET MVC")
           .Draggable(true)
           .Resizable(resizing => resizing
               .Enabled(true)
               .MinHeight(250)
               .MinWidth(250)
               .MaxHeight(500)
               .MaxWidth(500)
            )
            .ClientEvents(
              events =>
                events.OnClose("onCloseWindow"))
            .Visible(false)
           .Scrollable(true)
           .Modal(true)
           .Buttons(b => b.Maximize().Close())
           .Content(() =>
           {%>
                 <%= Html.Telerik().Upload()
            .Name("attachments")
            .Multiple(false)
            .Async(async => async
                .Save("Save", "Loteo")
                //.Remove("Remove", "Upload")
                .AutoUpload(true)
            )
            %>
           <%})
           .Width(200)
           .Height(100)
           .Render();
    %>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #Button1
        {
            width: 166px;
            height: 30px;
        }
        #Button2
        {
            width: 160px;
            height: 30px;
        }
        #Button3,#Button4
        {
            width: 160px;
            height: 30px;
        }
        #Button5
        {
            height: 30px;
        }
        #finalizarLoteo
        {
            height: 31px;
            width: 125px;
        }
    </style>
     <script type="text/javascript" language="javascript">

         function refreshGridCambios() {
             $('#GrillaCambios').data('t-grid').ajaxRequest();
         }

         function showWindowUpload() {
             $("#WindowUpload").data("tWindow").open();             
         } 

         function Eliminar_onclick() {
             var p1 = tiffemb0.GetCurrentPage();
             
             $.get("/Loteo/InsertarCambio", { pagina: p1, eliminar: 1, rotar: 0, adicionar: 0, rutaAdicionar: "" });
             refreshGridCambios();
             tiffemb0.GoToPageSpecial(4);
         }
         function Rotar_onclick() {
             var p1 = tiffemb0.GetCurrentPage();
             
             $.get("/Loteo/InsertarCambio", { pagina: p1, eliminar: 0, rotar: 1, adicionar: 0, rutaAdicionar: "" });
             refreshGridCambios();
             //Next page codigo 4
             tiffemb0.GoToPageSpecial(4);
         }

         function Aplicar_onclick() {
             location.href = '/Loteo/finalizarImagen';
         }

         function Adicionar_onclick() {
             var p1 = tiffemb0.GetCurrentPage();

             $.get("/Loteo/setCurrentPage", { pagina: p1});
             showWindowUpload();
         }

         function onCloseWindow(e) {             
             refreshGridCambios();
             tiffemb0.GoToPageSpecial(4);
         }

         function finalizarLoteo_onclick() {
             location.href = '/Loteo/finLoteo/';
         }

     </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
