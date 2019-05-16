<%@ Page Title="" Language="C#" MasterPageFile="Site1.Master" AutoEventWireup="true"
    CodeBehind="ReporteEstadoNegocios.aspx.cs" Inherits="GestorDocumental.ViewsAspx.ReporteEstadoNegocios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 137px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" BackColor="Silver">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                Fecha inicial:
                            </td>
                            <td>
                                <asp:TextBox ID="txtFechaIni" runat="server" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtFechaIni" Format="MM/dd/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                Fecha final:
                            </td>
                            <td>
                                <asp:TextBox ID="txtFechaFin" runat="server" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" Enabled="True"
                                    Format="MM/dd/yyyy" TargetControlID="txtFechaFin">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Button ID="tnGenerar" runat="server" OnClick="tnGenerar_Click" Text="Generar reporte" />
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:RoundedCornersExtender ID="Panel1_RoundedCornersExtender" runat="server" Enabled="True"
                    TargetControlID="Panel1">
                </asp:RoundedCornersExtender>
            </td>
        </tr>
        <tr>
            <td>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Colección)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Height="500px">
                    <LocalReport ReportPath="Reportes\Report1.rdlc">
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:RoundedCornersExtender ID="ReportViewer1_RoundedCornersExtender" runat="server"
                    Enabled="True" TargetControlID="ReportViewer1">
                </asp:RoundedCornersExtender>
            </td>
        </tr>
    </table>
</asp:Content>
