<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Busqueda
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <label id="lblNegocioView" style="display: none;">
        Negocio:
        <%: ViewData["Id_Neg"] %></label>
    <h2>
        Busqueda de contacto</h2>
    <div>
        <hr class="clear" />
        <fieldset class="scheduler-border">
            <legend class="scheduler-border"><b>Datos Basicos </b></legend>
            <div id="contentLeft">
                <label>
                    Tipo de documento:</label>
                <span class="span">
                    <%:ViewData["DatosIdNeg1"]%>
                </span>
                <label>
                    Primer nombre:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg3"]%>
                </span>
                <label>
                    Segundo nombre:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg4"]%>
                </span>
                <hr class="clear" />
                <label>
                    Primer apellido:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg5"]%>
                </span>
                <label>
                    Segundo apellido:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg6"]%>
                </span>
                <label>
                    Direcciòn:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg29"]%>
                </span>
                <label>
                    Celular:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg9"]%></span>
                <label>
                    Telèfono:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg30"]%></span>
            </div>
            <div id="contentRigth">
                <label>
                    Nùmero de documento:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg2"]%>
                </span>
                <label>
                    Fecha de nacimiento</label>
                <span class="span">
                    <%: ViewData["DatosIdNeg8"]%></span>
                <label>
                    Genero:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg7"]%>
                </span>
                <hr class="clear" />
                <label>
                    Departamento:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg11"]%>
                </span>
                <label>
                    Ciudad:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg12"]%>
                </span>
                <label>
                    Correo:
                </label>
                <span class="span">
                    <%:ViewData["DatosIdNeg10"]%></span>
            </div>
            <div>
                <label>
                    Firma:
                </label>
                <input type="checkbox" checked='<%:ViewData["DatosIdNeg53"]%>' disabled="disabled"
                    name="name" value="" />
                <label>
                    Aceptaciòn de terminos:
                </label>
                <input type="checkbox" checked='<%:ViewData["DatosIdNeg51"]%>' disabled="disabled"
                    name="name" value="" />
            </div>
        </fieldset>
    </div>
    <hr class="clear" />
    <hr />
    <%-- Falta el from--%>
    <div id="infoConfirmacion">
        <form action="/Mesa/GuardarCambiosBusqueda" method="post" onsubmit="return validarCampos()">
        <fieldset class="scheduler-border">
            <legend class="scheduler-border"><b>Datos de busqueda</b></legend>
            <label>
                Telèfono fijo:
            </label>
            <input type="text" id="telFijo" name="telFijo" value='' />
            <label>
                Nùmero celular:
            </label>
            <input type="text" id="txtCelular" name="numCelular" value='' />
            <label>
                Observaciones</label>
            <textarea name="observaciones"> </textarea>
            <input type="text" name="IdNeg" class="displayNone" value='<%: ViewData["Id_Neg"] %>' />
            <br />
        </fieldset>
        <input type="submit"  id="btnGuardar" value="Guardar" class="boton" />
        </form>
        <!--Reasignar el negocios -->
        <form action="/Mesa/ReasignarNegocio" method="post">
        <input type="text"  id="txtEstado" name="estado" class="displayNone" value="100" />
        <input type="text"  id="txtNegID" name="negID" class="displayNone" value='<%: ViewData["Id_Neg"] %>' />
        <input type="submit" id="btnReasignar"   value=":: Reasignar ::" class="boton" />
        </form>
    </div>
    <script type="text/javascript">
        (function () {
            //deshabilitar el enter 
            $(document).ready(function () {
                $("form").keypress(function (e) {
                    if (e.which == 13) {
                        return false;
                    }
                });
            });

            $("#lblNegocio").text($("#lblNegocioView").text());

            $("#infoConfirmacion fieldset input[type='text']").focusin(function () {
                $("#infoConfirmacion").find(".error").remove();
            })
        })();       
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Content/Styles/page.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
