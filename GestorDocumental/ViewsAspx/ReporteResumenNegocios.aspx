<%@ Page Title="" Language="C#" MasterPageFile="Site1.Master" AutoEventWireup="true" CodeBehind="ReporteResumenNegocios.aspx.cs" Inherits="GestorDocumental.ViewsAspx.ReporteResumenNegocios" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">

        .style1
        {
            width: 137px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" BackColor="Silver">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                Fecha inicial:</td>
                            <td>
                                <asp:TextBox ID="txtFechaIni" runat="server" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtFechaIni" Format="MM/dd/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                Fecha final:</td>
                            <td>
                                <asp:TextBox ID="txtFechaFin" runat="server" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" 
                                    Enabled="True" Format="MM/dd/yyyy" TargetControlID="txtFechaFin">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="tnGenerar" runat="server" onclick="tnGenerar_Click" 
                                            Text="Generar reporte" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:RoundedCornersExtender ID="Panel1_RoundedCornersExtender" runat="server" 
                    Enabled="True" TargetControlID="Panel1">
                </asp:RoundedCornersExtender>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
                    Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%">
                    <LocalReport ReportPath="Reportes\ReporteResumenNegocios.rdlc">
                    </LocalReport>
                </rsweb:ReportViewer>
            </td>
        </tr>
    </table>
</asp:Content>
