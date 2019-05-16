<%@ Page Title="" Language="C#" MasterPageFile="Site1.Master" AutoEventWireup="true" CodeBehind="CrearUsuarios.aspx.cs" Inherits="GestorDocumental.ViewsAspx.CrearUsuarios" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 185px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
   
            <table>
                <tr>
                    <td class="style1">
                        Num identificacion:</td>
                    <td>
                        <asp:TextBox ID="txtIdentificacion" runat="server" 
                Width="100px" AutoPostBack="True" ontextchanged="txtIdentificacion_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtIdentificacion" Display="None" 
                ErrorMessage="Campo requerido "></asp:RequiredFieldValidator>
                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" 
                runat="server" Enabled="True" 
                TargetControlID="RequiredFieldValidator1">
                        </asp:ValidatorCalloutExtender>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style1">
                        Nombres:</td>
                    <td>
                        <asp:TextBox ID="txtNombres" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="txtNombres" Display="None" 
                ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender" 
                runat="server" Enabled="True" 
                TargetControlID="RequiredFieldValidator2">
                        </asp:ValidatorCalloutExtender>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style1">
                        Password:</td>
                    <td>
                        <asp:TextBox ID="txtPswd" runat="server" 
                TextMode="Password" Width="100px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="txtPswd" Display="None" 
                ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender" 
                runat="server" Enabled="True" 
                TargetControlID="RequiredFieldValidator3">
                        </asp:ValidatorCalloutExtender>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style1">
                        Cliente:</td>
                    <td>
                        <asp:DropDownList ID="lstCliente" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ControlToValidate="lstCliente" Display="None" 
                ErrorMessage="Seleccione una opcion" InitialValue="0"></asp:RequiredFieldValidator>
                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator4_ValidatorCalloutExtender" 
                runat="server" Enabled="True" 
                TargetControlID="RequiredFieldValidator4">
                        </asp:ValidatorCalloutExtender>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style1">
                        Rol:</td>
                    <td>
                        <asp:DropDownList ID="lstRol" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                            ControlToValidate="lstRol" Display="None" 
                ErrorMessage="Seleccione una opcion" InitialValue="0"></asp:RequiredFieldValidator>
                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator5_ValidatorCalloutExtender" 
                runat="server" Enabled="True" 
                TargetControlID="RequiredFieldValidator5">
                        </asp:ValidatorCalloutExtender>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style1">
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red" 
                Width="100%"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style1">
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="btnGuardar" runat="server" 
                onclick="btnGuardar_Click" Text="Guardar" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
      
</asp:Content>
