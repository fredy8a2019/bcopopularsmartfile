<%@ Page Title="" Language="C#" MasterPageFile="Site1.Master" AutoEventWireup="true" CodeBehind="AsignacionMenuPerfiles.aspx.cs" Inherits="GestorDocumental.ViewsAspx.AsignacionMenuPerfiles" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 109px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<h2> Asignacion de Tareas por Perfil</h2>
    <table style="width:100%;">
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                         <td>
                           <h3>Perfil </h3>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                
                                    <ContentTemplate>
                                        <asp:DropDownList ID="lstPerfil" runat="server" AutoPostBack="True" 
                                            onselectedindexchanged="lstPerfil_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Width="100%"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:RadioButtonList ID="rbPadres" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="rbPadres_SelectedIndexChanged" BackColor="#999999" 
                                        RepeatDirection="Horizontal">
                                    </asp:RadioButtonList>
                                    <asp:RoundedCornersExtender ID="rbPadres_RoundedCornersExtender" runat="server" 
                                        Enabled="True" TargetControlID="rbPadres">
                                    </asp:RoundedCornersExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:CheckBoxList ID="chkHijos" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="chkHijos_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
     </table>
    <br />
</asp:Content>
