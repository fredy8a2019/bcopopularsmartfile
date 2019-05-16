<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="GestorDocumental.Models" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Reporte estados por Negocio
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Reporte estados por negocio</h3>
    <div>
        <form action="/Report/VariablesFiltrar" method="post">
            <table>
                <tr>
                    <td>Fecha Inicio:</td>
                    <td>
                        <%= Html.Telerik().DatePicker()
                            .Name("fechaInicial")
                            .Format("dd/MM/yyyy")
                            .Value(Session["FechaInial"].ToString())                     
                        %></td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Fecha Fin:</label></td>
                    <td>
                        <%= Html.Telerik().DatePicker()
                            .Name("fechaFin")
                            .Format("dd/MM/yyyy")
                            .Value(Session["FechaFin"].ToString())
                        %></td>
                </tr>
                <tr>
                    <td>
                        <input type="submit" name="name" value=":: Filtrar ::" class="btn btn-login" /></td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            &nbsp;
        <br />
            &nbsp;
        </form>
    </div>
    <%                   
        Html.RenderPartial("PivotGridPartial");
    %>
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <% Html.DevExpress().RenderStyleSheets(Page,
        new StyleSheet { ExtensionSuite = ExtensionSuite.PivotGrid, Theme = "Glass" }
    );
    %>
    <% Html.DevExpress().RenderScripts(Page,
        new Script { ExtensionSuite = ExtensionSuite.PivotGrid }
    ); %>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
