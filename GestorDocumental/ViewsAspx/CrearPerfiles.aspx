<%@ Page Title="" Language="C#" MasterPageFile="Site1.Master" AutoEventWireup="true" CodeBehind="CrearPerfiles.aspx.cs" Inherits="GestorDocumental.ViewsAspx.CrearPerfiles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 191px;
        }

        .tablaSeparada
        {
            border-collapse: separate;
            border-spacing: 10px 10px;
        }

        legend.scheduler-border
        {
            font-size: x-large !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <fieldset class="scheduler-border">
        <legend class="scheduler-border">Creación de Perfiles</legend>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="tablaSeparada">
                    <tr>
                        <td>
                            <label>Descripción del perfil:</label>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtDescPerfil" runat="server" Width="400px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ControlToValidate="txtDescPerfil" Display="None" ErrorMessage="Campo requerido"
                                        ValidationGroup="G1">
                                    </asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>
                                    <asp:Button ID="btnAgregar" runat="server" OnClick="btnAgregar_Click" CssClass="btn btn-login"
                                        Text="Agregar" ValidationGroup="G1" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblerror" runat="server" ForeColor="Red" Width="100%"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvRoles" runat="server" AutoGenerateColumns="False" CssClass="table table-striped"
                                        OnRowCancelingEdit="gvRoles_RowCancelingEdit" OnRowEditing="gvRoles_RowEditing"
                                        OnRowUpdating="gvRoles_RowUpdating">
                                        <Columns>
                                            <asp:TemplateField HeaderText="ID">
                                                <EditItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("RolId") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("RolId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DESCRIPCION">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("DescRol") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("DescRol") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ButtonType="Button" ShowEditButton="True" ControlStyle-CssClass="btn btn-login" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
