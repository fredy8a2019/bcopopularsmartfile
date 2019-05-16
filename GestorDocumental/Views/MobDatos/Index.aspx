<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMobile.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <h2>
        Ingreso de Datos de Devolución Movíl</h2>
    <% using (Html.BeginForm("GuardarDatos", "MobDatos", FormMethod.Post))
       { %>
    <table>
        <tr>
            <td>
                <h2>
                    Número Documento</h2>
            </td>
            <td>
                <input type="text" class="campoText" id="txtDocumento"
                    name="txtDocumento" class="t-input" onfocus="focused('txtDocumento')" onblur="blurred('txtDocumento')" />
            </td>
        </tr>
        <tr>
            <td>
                <h2>
                    Número Proveedor</h2>
            </td>
            <td>
                <input type="text" class="campoText" id="txtNoProveedor"
                    name="txtNoProveedor" class="t-input" onfocus="focused('txtNoProveedor')" onblur="blurred('txtNoProveedor')" />
            </td>
        </tr>
        <tr>
            <td>
                <h2>
                    Sociedad</h2>
            </td>
            <td>
                <%= Html.Telerik().DropDownList()
                                  .Name("cmbSociedad")
                                  .DataBinding(bindig => bindig.Ajax().Select("_obtenerListaClientes", "MobDatos"))
                                  .HtmlAttributes(new { style = string.Format("width:{0}px; font-size: 2em;", 500) })
                                  .Placeholder("Seleccione Oficina.....")%>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <h2>
                    Observaciones</h2>
            </td>
            <td>
                <%= Html.Telerik().Editor()
                                  .Name("txtObservaciones")
                                  .HtmlAttributes(new { style = "width: 500px; rows:3" })
                                  .Tools(tools => tools.Clear())%>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <input type="submit" style="width: 500px; height: 60px;" class="t-button" value="Guardar" />
            </td>
        </tr>
    </table>
    <%
       }
    %>
    <% Html.EndForm(); %>
    <script type="text/javascript">

        function numbersonly(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode != 8 && unicode != 45 && unicode != 9) {
                if (unicode < 43 || unicode > 57) //if not a number
                { return false } //disable key press    
            }
        }

        function focused(campo) {
            var test = document.getElementById(campo);
            setTimeout(function () {
                if (document.activeElement === test) {
                    test.type = 'text';
                }
            }, 1);
        }

        function blurred(campo) {
            var test = document.getElementById(campo);
            var value = test.value;
            test.type = 'tel';
        }

        if (!/^(iPhone|iPad|iPod)$/.test(navigator.platform)) alert('Code specific to Mobile Safari');

    </script>
    <style type="text/css">
        .campoText
        {
            width: 99%;
            height: 1.5em;
            border-radius: 5px;
            border: solid 1px #999;
            padding: 2px 3px 2px 3px;
            margin-bottom: 15px;
            font-size: 2em;
            font-family: inherit;
            letter-spacing: 0ex;
        }
        
        .li
        {
            width: 99%;
            height: 1.5em;
            border-radius: 5px;
            border: solid 1px #999;
            padding: 2px 3px 2px 3px;
            margin-bottom: 15px;
            font-size: 2em;
            font-family: inherit;
            letter-spacing: 0ex;
        }
    </style>
</asp:Content>
