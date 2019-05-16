<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<GestorDocumental.Models.Parametros>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Create</h2>

<script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
<script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

<% using (Html.BeginForm()) { %>
    <%: Html.ValidationSummary(true) %>
    <fieldset class="scheduler-border">
        <legend class="scheduler-border">Parametros</legend>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.codigo) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.codigo) %>
            <%: Html.ValidationMessageFor(model => model.codigo) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.valor) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.valor) %>
            <%: Html.ValidationMessageFor(model => model.valor) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.descripcion) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.descripcion) %>
            <%: Html.ValidationMessageFor(model => model.descripcion) %>
        </div>

        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
<% } %>

<div>
    <%: Html.ActionLink("Back to List", "Index") %>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
