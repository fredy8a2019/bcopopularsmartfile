<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <label id="lblNegocioView" style="display: none;">
        Negocio:
        <%: ViewData["Id_Neg"] %></label>
    <h2>
        Referenciacion
    </h2>
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
                <input type="checkbox" id="ckFirma1" disabled="disabled" name="name" value='<%: ViewData["ValueChekFirma"] %>' />
                <label>
                    Aceptaciòn de terminos:
                </label>
                <input type="checkbox" id="ckAceptacion1" disabled="disabled" name="name" value='<%:ViewData["ValueChekTerminos"] %>' />
            </div>
        </fieldset>
    </div>
    <hr class="clear" />
    <div>
        <form action="/Mesa/GuardarCambios" method="post" onsubmit="return validarCampos()">
        <!-- Datos de busqueda de contacto-->
        <div id="infoBusqueda">
            <fieldset class="scheduler-border">
                <legend class="scheduler-border"><b>Datos de busqueda del contacto </b></legend>
                <label>
                    Telèfono:
                </label>
                <span class="span">
                    <%:ViewData["busqTel"]%></span>
                <label>
                    Celular</label>
                <span class="span">
                    <%:ViewData["busqCel"]%></span>
                <label>
                    Observaciones:
                </label>
                <textarea disabled="disabled"><%:ViewData["busqObser"]%></textarea>
            </fieldset>
        </div>
        <!--Datos para confirmar-->
        <div id="infoConfirmacion">
            <fieldset class="scheduler-border">
                <legend class="scheduler-border"><b>Datos para corrección</b> </legend>
                <input type="text" id="txtIdNeg" name="IdNeg" class="displayNone" value='<%: ViewData["Id_Neg"] %>' />
                <label>
                    Correo:
                </label>
                <input type="email" id="txtCorreo" name="correo" value='<%:ViewData["DatosIdNeg10"]%>' pattern="/^[0-9a-z_\-\.]+@([a-z0-9\-]+\.?)*[a-z0-9]+\.([a-z]{2,4}|travel)$/i" placeholder="ejemplo@dominio.com" />
                <label>
                    Fecha de nacimiento:
                </label>
                <%= Html.Telerik().DatePicker()
                                            .Name("fechaNacimiento")
                                            .Value(ViewData["DatosIdNeg8"].ToString())
                                            .Enable((bool)ViewData["EnabledFecha"])
                %>
                <%--<input type="text" id="txtFechaNacimiento" name="fechaNacimiento" value='<%:ViewData["DatosIdNeg8"] %>' />--%>
                <label>
                    Celular:
                </label>
                <input type="text" id="txtCelular" name="celular" value='<%:ViewData["DatosIdNeg9"]%>' placeholder="Numero de celular"/>
                <label>
                    Firma:
                </label>
                <input type="checkbox" id="ckFirma" name="firma" value='<%:ViewData["ValueChekFirma"] %>' />
                <label>
                    Aceptaciòn de terminos:
                </label>
                <input type="checkbox" id="ckAceptacion" name="aceptacion" value='<%:ViewData["ValueChekTerminos"] %>' />
            </fieldset>
        </div>
        <hr class="clear" />
        <!--Datos de la llamada-->
        <div id="infoLlamada">
            <fieldset class="scheduler-border">
                <legend class="scheduler-border"><b>Datos llamada </b></legend>
                <label>
                    Observaciones:</label>
                <textarea name="observaciones"> </textarea>
            </fieldset>
        </div>
        <input type="submit" id="btnGuardar"  value="Guardar" class="boton" />
        </form>
    </div>
    <form action="/Mesa/ReasignarNegocio" method="post">
        <input type="text" id="txtEstado" name="estado" class="displayNone" value="110" />
        <input type="text" id="txtNegID" name="negID" class="displayNone" value='<%: ViewData["Id_Neg"] %>' />
        <input type="submit" d="btnReasignar" value=":: Reasignar ::" class="boton" />
     </form>
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

            activar($("#txtCorreo"));
            //activar($("#txtFechaNacimiento"));
            activar($("#txtCelular"));
            activarCheck($("#ckFirma"));
            activarCheck($("#ckAceptacion"));
            //Datos base
            activarCheck($("#ckFirma1"));
            activarCheck($("#ckAceptacion1"));


            $("#infoConfirmacion fieldset input[type='text']").focusin(function () {
                $("#infoConfirmacion").find(".error").remove();
            })

            $("#infoConfirmacion fieldset input[type='checkbox']").on("click", function () {
                console.log("hace click")
                if ($(this).attr("checked") == "checked") {
                    $(this).val(1);
                    //alert('Tomo 1');
                }
                else {
                    $(this).val(0);
                    //alert('Tomo ');
                }
            });

        })();

        function activar(elemento) {
            if (elemento.val() != "") {
                elemento.attr("disabled", "disabled");
            }
            else {
                if (elemento.attr("id") == "txtFechaNacimiento") {
                    // elemento.attr("type", "date");
                }

            }
        }

        function activarCheck(elemento) {
            if (elemento.val() == "1") {
                elemento.attr("disabled", "disabled");
                elemento.attr("checked", true);
            }
        }

        function validarCampos() {
            //Se valida el campo numero de telefono
            var exp = /^[0-9]{9,10}$/
            var valoCel = $("#txtCelular").val();
            if (valoCel != "") {
                if (!(exp.test(valoCel))) {
                    $("#txtCelular").after("<p class='error'>Ingrese un valor correcto</p>");
                    return false;
                }
            }
            
            return true;
        }
    </script>
    <script src="../../Scripts/page/validaciones.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Content/Styles/page.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
