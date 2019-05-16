<%@ Page Title="" Language="C#" MasterPageFile="Site1.Master" AutoEventWireup="true"
    CodeBehind="Conciliar.aspx.cs" Inherits="GestorDocumental.ViewsAspx.Conciliar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="../Content/Scripts/splitter1.js"></script>
    <link href="../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $().ready(function () {
            $("#MySplitter").splitter({
                type: "h",
                sizeTop: true, /* use height set in stylesheet */
                accessKey: "P"
            });
        });

        var pag;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="MySplitter">
        <div id="TopPane">
            <object id="tiffobj0" width="100%" height="90%" classid="CLSID:106E49CF-797A-11D2-81A2-00E02C015623"
                style="margin-top: 0px">
                <param name="src" value="samples/sample.alttif" />
                <embed toolbaritems="<%: Session["TOOL_BAR"] %>" id="tiffemb0" width="100%" height="100%"
                    src="" type="application/x-alternatiff">
            </object>
            <script type="text/javascript">

                var tiff0;
                if (document.getElementById) {
                    if (document.getElementById('tiffemb0')) {
                        tiff0 = document.getElementById('tiffemb0');
                    }
                    else if (document.getElementById('tiffobj0')) {
                        tiff0 = document.getElementById('tiffobj0');
                    }
                }
                else if (document.all) {
                    tiff0 = document.all.tiffobj0;
                }





                $(document).ready(CargarDoc)

                function CargarDoc() {
                    pag = document.getElementById('NumPagina').value;
                    tiff0.LoadImage(neg, pag, 0)
                    //alert('pageLoad' + pag);
                    // document.getElementById('txtNegocio').value = 100;

                }


                function CargarPaginaDigitada() {
                    alert('Cambio de índice de los documentos');
                    tiff0.GoToPage(pag);

                }



                function onRequestEnd() {
                    $.unblockUI();
                    pag = document.getElementById('NumPagina').value;
                    tiff0.GoToPage(pag);
                    // alert('requestEnd' + pag);

                } 

               

            </script>
        </div>
        <div id="BottomPane" style="top: 77%;">
            <table style="width: 100%;">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" BackColor="Silver" BorderColor="#999999" BorderStyle="Solid">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td class="style3" colspan="4">
                                                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                                            <asp:HiddenField ID="NumPagina" runat="server" ClientIDMode="Static" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style3">
                                                            Documento:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="lstDocumentos" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstDocumentos_SelectedIndexChanged"
                                                                Enabled="False">
                                                            </asp:DropDownList>
                                                            <asp:RoundedCornersExtender ID="lstDocumentos_RoundedCornersExtender" runat="server"
                                                                Corners="All" Enabled="True" TargetControlID="lstDocumentos">
                                                            </asp:RoundedCornersExtender>
                                                        </td>
                                                        <td class="style4">
                                                            Grupo:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="lstGrupos" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstGrupos_SelectedIndexChanged"
                                                                Enabled="False">
                                                            </asp:DropDownList>
                                                            <asp:RoundedCornersExtender ID="lstGrupos_RoundedCornersExtender" runat="server"
                                                                Corners="All" Enabled="True" TargetControlID="lstGrupos">
                                                            </asp:RoundedCornersExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:RoundedCornersExtender ID="Panel1_RoundedCornersExtender" runat="server" Color="Aquamarine"
                                    Enabled="True" Radius="6" TargetControlID="Panel1">
                                </asp:RoundedCornersExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pControlsM" runat="server" BackColor="#D1D1D1">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Table ID="tblControls" runat="server" EnableViewState="False">
                                                    <asp:TableRow ID="R1" runat="server">
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pControls" runat="server">
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button BackColor="#9AAE04" Font-Bold="True" ForeColor="White" ID="btnInsertar"
                                                    runat="server" OnClick="Button1_Click1" Text="Guardar" ViewStateMode="Enabled" />
                                                &nbsp;
                                                <asp:Button BackColor="#9AAE04" Font-Bold="True" ForeColor="White" ID="btn_omiteConciliacion"
                                                    runat="server" OnClick="Button1_Click" Text="Omite conciliacion" Visible="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:RoundedCornersExtender ID="pControlsM_RoundedCornersExtender" runat="server"
                                    Corners="All" Enabled="True" TargetControlID="pControlsM">
                                </asp:RoundedCornersExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
