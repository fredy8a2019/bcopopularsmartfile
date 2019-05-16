<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMobile.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<label >
        <b>Devoluciones Movil</b>
    </label>
    <label class="distanciaLabel">
        <%: ViewData["NomUsuario"].ToString() %></label>
    <label class="distanciaLabel">
        Numero:
        <%: Session["ID_MOVIL"]%></label>
    
    <form action="/Mobile/Guardar/" method="post">
    <%= Html.Telerik().Grid((IEnumerable<GestorDocumental.Models.DocumentosMobil>)ViewData["Documentos"])
            .Name("Documentos")
        .Columns(columns =>
        {
            columns.Bound(c => c.Id).Width(130).Hidden(true).Title("Id documento");
            columns.Bound(c => c.descripcion).Width(250).Title("Documento");
        })
        .DataBinding(dataBinding => dataBinding.Ajax().Select("_SelectionClientSide_Documentos",
        "Mobile"))
        .Pageable()
        .Sortable()
        .Filterable()
        .Selectable()
        .ClientEvents(events => events.OnRowSelect("onRowSelected"))
        .RowAction(row => row.Selected = row.DataItem.Id.Equals(ViewData["id"]))        
    %>
    <%= Html.Telerik().Grid((IEnumerable<GestorDocumental.Models.ListaChequeo>)ViewData["listaChequeo"])
        .Name("listaChequeo")
        .Columns(columns=>
        {
            columns.Bound(c => c.id).Width(100).Hidden(true);
            columns.Bound(c => c.seleccion).ClientTemplate("<input type='checkbox' name='checkedRecords'value='<#= seleccion #>' onclick='selected(this);' />")
                  .Title("")
                   .Width(50).HtmlAttributes(new { style = "text-align:center" });
            columns.Bound(c => c.descripcion).Width(900).Title("Causal devolucion");
        })
            .DataBinding(dataBinding => dataBinding.Ajax().Select("_SelectionClientSide_Lista",
        "Mobile", new { documentoID = 1 }))
                    .ClientEvents(clientEvents => clientEvents
                                                             .OnDataBinding("onDataBinding"))
        .Pageable()
        .Sortable()        
    %>
    <input type="submit" id="btnSiguiente" value="Siguiente" class="boton" />
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        var documentoID;
        function onRowSelected(e) {
            var ordersGrid = $('#listaChequeo').data('tGrid');
            documentoID = e.row.cells[0].innerHTML;
            // update ui text
            $('#id').text(documentoID);
            // rebind the related grid
            ordersGrid.rebind();

        }

        function selected(e) {
            var valor = $(e).attr("checked");
            var tr = $(e).parent().parent().find("td");
            console.log(tr[0]);
            if (valor == "checked") {
                $(e).attr("name", "checked");
                $(e).val($(tr[0]).text());
            } else {
                //checkedRecords
                $(e).attr("name", "checkedRecords");
                $(e).val("");
            }
        }

        function onDataBinding(e) {
            e.data = $.extend(e.data, { documentoID: documentoID });
        }

    </script>
    <style>
        #Documentos
        {
            width: 100%;
        }
        #listaChequeo
        {
            width: 100%;
        }
        
        input[type="checkbox"]
        {
            transform: scale(1.8,1.8);
            -ms-transform: scale(1.8,1.8); /* IE 9 */
            -moz-transform: scale(1.8,1.8); /* Firefox */
            -webkit-transform: scale(1.8,1.8); /* Safari and Chrome */
            -o-transform: scale(1.8,1.8); /* Opera */
        }
        
        .distanciaLabel
        {
            margin-left: 24px;
        }
        
        .boton
        {
            background-color: #9AAE04;
            font-weight: bold;
            color: White;
            height: 30px;
            margin-left:5%;
            margin-top:1%;
            
            
            -ms-transform: scale(3,2); /* IE 9 */
            -moz-transform: scale(3,2); /* Firefox */
            -webkit-transform: scale(3,2); /* Safari and Chrome */
            -o-transform: scale(3,2); /* Opera */
            
        }
        
        b
        {
            font-size:x-large;    
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
