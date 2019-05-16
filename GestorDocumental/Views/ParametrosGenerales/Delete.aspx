<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<GestorDocumental.Models.Parametros>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Delete</h2>

<h3>¿Are you sure you want to delete this?</h3>
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
<% using (Html.BeginForm()) { %>
    <p>
        <input type="submit" value="Delete" /> |
        <%: Html.ActionLink("Back to List", "Index") %>
    </p>
<% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
