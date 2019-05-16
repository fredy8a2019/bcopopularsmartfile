<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Recepción
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
        //Index
        function validaFormulario(formulario) {
            //alert(formulario.DropDownList_Oficinas.value);
            //            if()
            //            if (formulario.DropDownList_Clientes.value == 0) {
            //                alert('Por favor seleccione un cliente');
            //                return false;
            //            }

            if (formulario.DropDownList_Oficinas.value == 0 || formulario.DropDownList_Oficinas.value == '') {
                alert('Por favor seleccione la oficina');
                return false;
            }

            if (formulario.DropDownList_Grupos.value == 0 || formulario.DropDownList_Grupos.value == '') {
                alert('Por favor seleccione el producto');
                return false;
            }

            if (formulario.DropDownList_SubGrupos.value == 0 || formulario.DropDownList_SubGrupos.value == '') {
                alert('Por favor seleccione el subproducto.');
                return false;

            }

            if (formulario.Principales.value == 0 || formulario.Principales.value == '') {
                alert('Por favor digite la cantidad de principales.');
                return false;

            }

            if (formulario.Anexos.value == '') {
                alert('Por favor digite la cantidad de anexos.');
                return false;

            }

            return true;
        }
    </script>
    <style type="text/css">
        #Submit1
        {
            text-align: left;
            width: 121px;
        }
        .style6
        {
            width: 167px;
            height: 6px;
        }
        .style8
        {
            height: 6px;
        }
        .style9
        {
            text-align: left;
        }
        .style11
        {
            width: 167px;
        }
        #Button1
        {
            width: 229px;
            text-align: center;
        }
        #Reset1
        {
            width: 193px;
        }
        #observaciones
        {
            width: 286px;
        }
        #Guardar
        {
            width: 141px;
        }
        #Reset
        {
            width: 136px;
        }
        .style12
        {
            text-align: left;
            height: 1px;
        }
        .style13
        {
            width: 167px;
            height: 1px;
        }
        .stylef
        {
            width: 600px;
            height: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form2" onsubmit="return validaFormulario(this)" action="/Recepcion/GuardarRecepcion"
    method="post" class="stylef">
    <h2>Recepción</h2>
    <fieldset class="scheduler-border">
        <legend class="scheduler-border"><b>Datos Básicos</b></legend>
        <div id="contentLeft">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="style9" colspan="2">
                </tr>
                <tr>
                    <td class="style9" colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        Fecha:
                    </td>
                    <td class="style11">
                        <%= DateTime.Now %>
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        &nbsp;
                    </td>
                    <td class="style11">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        Cliente:
                    </td>
                    <td class="style11">
                        <%=((GestorDocumental.Models.Clientes)Session["CLIENTE"]).CliNombre%>
                        <%--=Html.Telerik().DropDownList()
                            .Name("DropDownList_Clientes")
                            .SelectedIndex(2)
                            .Enable (false)
                            .BindTo(new SelectList(new GestorDocumental.Controllers.ClienteController().obtenerClientes().ToList(), "CliNit", "CliNombre"))
                            .HtmlAttributes(new { style = string.Format("width:{0}px", 150) })
                            .CascadeTo("DropDownList_Grupos")
                                                                                           
                                                                      
                                                 
                        --%>
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        &nbsp;
                    </td>
                    <td class="style11">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        Oficina:
                    </td>
                    <td class="style11">
                        <%= Html.Telerik().DropDownList()
                    .Name("DropDownList_Oficinas")
                    .DataBinding(binding => binding.Ajax().Select("_GetDropDownList_Oficinas", "Recepcion"))
                    .HtmlAttributes(new { style = string.Format("width:{0}px", 150) })
                    .Placeholder("Seleccione Oficina...")
                        %>
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        &nbsp;
                    </td>
                    <td class="style11">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        Producto:
                    </td>
                    <td class="style11">
                        <%= Html.Telerik().DropDownList()
                    .Name("DropDownList_Grupos")
                    .DataBinding(binding => binding.Ajax().Select("_GetDropDownListProductos", "Recepcion"))
                    .CascadeTo("DropDownList_SubGrupos")
                    .HtmlAttributes(new { style = string.Format("width:{0}px", 300) })
                    .Placeholder("Seleccione Producto...")
                        %>
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        &nbsp;
                    </td>
                    <td class="style11">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        Subproducto:
                    </td>
                    <td class="style11">
                        <%= Html.Telerik().DropDownList()
                    .Name("DropDownList_SubGrupos")
                    .DataBinding(binding => binding.Ajax().Select("_GetDropDownListSubProductos", "Recepcion"))
                    .HtmlAttributes(new { style = string.Format("width:{0}px", 300) })
                    .Placeholder("Seleccione Producto...")
                        %>
                    </td>
                </tr>
                <tr>
                    <td class="style8">
                        &nbsp;
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style8">
                        Cantidad Principales:&nbsp;
                    </td>
                    <td class="style6">
                        <%= Html.Telerik().IntegerTextBox()
                     .Name("Principales")
                     .MinValue(1)
                        %>
                    </td>
                </tr>
                <tr>
                    <td class="style8">
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style8">
                        Anexos:&nbsp;
                    </td>
                    <td class="style6">
                        <%= Html.Telerik().IntegerTextBox()
                     .Name("Anexos")
                     .MinValue(0)
                        %>
                    </td>
                </tr>
                <tr>
                    <td class="style8">
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                </tr>
                <tr style="display: none">
                    <td class="style8" colspan="2">
                        <strong>Datos Complementarios</strong>
                    </td>
                </tr>
                <tr style="display: none">
                    <td class="style8">
                        Archivo de Conciliación<strong>:&nbsp;
                    </td>
                    <td class="style6">
                        <%= Html.Telerik().Upload()
            .Name("attachments")
            .Multiple(false)    
                                
            .Async(async => async
                .Save("SaveFile", "Recepcion")
                //.Remove("Remove", "Upload")
                .AutoUpload(true)
            )
                        %>
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        &nbsp;
                    </td>
                    <td class="style11">
                        &nbsp;
                    </td>
                </tr>
                <tr style="display: none">
                    <td class="style9">
                        Fecha de Embalaje:
                    </td>
                    <td class="style11">
                        <%= Html.Telerik().DateTimePicker()
                    .Name("Fecha_embalaje")
                    .HtmlAttributes(new { id = "DateTimePicker_wrapper1" })
                    .Interval(15)
                    .Value(DateTime.Now)
                        %>
                    </td>
                </tr>
                <tr style="display: none">
                    <td class="style9">
                        Tienda:
                    </td>
                    <td class="style11">
                        <%= Html.Telerik().DropDownList()
                    .Name("DropDownList_tienda")
                    .HtmlAttributes(new { style = "width: 300px" })
                    .DataBinding(binding => binding.Ajax().Select("_GetDropDownList_tienda", "Recepcion"))
                    .Placeholder("Seleccione Tienda...")
                        %>
                    </td>
                </tr>
                <tr style="display: none">
                    <td class="style9">
                        Fecha y Hora de Retiro:
                    </td>
                    <td class="style11">
                        <%= Html.Telerik().DateTimePicker()
                .Name("fechahora_Retiro")
                .HtmlAttributes(new { id = "DateTimePicker_wrapper" })
                .Interval(15)
                .Value(DateTime.Now)
                        %>
                </tr>
                <tr style="display: none">
                    <td>
                        Número de Guia:&nbsp;
                    </td>
                    <td class="style6">
                        <%= Html.Telerik().IntegerTextBox()
                     .Name("guia")
                     .MinValue(1)
                        %>
                    </td>
                </tr>
                <tr style="display: none">
                    <td class="style8">
                        Paquete:&nbsp;
                    </td>
                    <td class="style6">
                        <%= Html.Telerik().IntegerTextBox()
                     .Name("paquete")
                     .MinValue(1)
                        %>
                    </td>
                </tr>
                <tr>
                    <td class="style8">
                        Observaciones:
                    </td>
                    <td class="style6">
                        <textarea id="observaciones" name="observaciones" rows="5" cols="20"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="style8">
                        &nbsp;
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                </tr>
    </fieldset>
    <tr>
        <td class="style8">
            <input id="Guardar" class="btn btn-login" type="submit" value="Guardar" />&nbsp;
        </td>
        <td class="style6">
            <input id="Reset" class="btn btn-login" type="reset" value="Cancelar" />
        </td>
    </tr>
    </table> </div>
    </form>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
