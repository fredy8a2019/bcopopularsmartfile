<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<GestorDocumental.Models.Parametros>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Details</h2>

<fieldset class="scheduler-border">
    <legend class="scheduler-border">Parametros</legend>

    <div class="display-label">codigo</div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.codigo) %>
    </div>

    <div class="display-label">valor</div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.valor) %>
    </div>

    <div class="display-label">descripcion</div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.descripcion) %>
    </div>
</fieldset>
<p>

    <%: Html.ActionLink("Edit", "Edit", new { id=Model.id }) %> |
    <%: Html.ActionLink("Back to List", "Index") %>
</p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
