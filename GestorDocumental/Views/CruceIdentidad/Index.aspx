<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Cruce de Identidad
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Cruce de identidad</h2>
    <form id="from1" method="post" onsubmit="return validaFormulario(this)" action="/CruceIdentidad/GuardarArchivo">
    <div id="contentLeft">
        <label>
            Fecha recepciòn:</label>
        <%= Html.Telerik().DatePicker()
                         .Name("fechaRecepcion")
                         
                        .Value((DateTime.Now))
        %>
        <label>
            Archivo:</label>
        <%= Html.Telerik().Upload()
                    .Name("attachments")  
        .Async(async => async
              .Save("SaveFile", "CruceIdentidad")
              .AutoUpload(true))
        
        %>
        <label>
            Observaciones:</label>
        <textarea id="observaciones" name="observaciones" rows="5" cols="20"></textarea>
        <input id="Guardar" class="boton" type="submit" value="Guardar" />

    </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
        function validaFormulario(formulario) {

            if (formulario.fechaRecepcion.value == "") {
                alert('Por favor seleccione fecha');
                return false;
            }

            if ($(".t-filename").text() == "") {
                alert('Por favor seleccione el archivo');
                return false;
            }

            if ($("#observaciones").val() == "") {
                alert('Por favor escriba una observacion');
                return false;

            }

            return true;
        }
    </script>
    <link href="../../Content/Styles/page.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
