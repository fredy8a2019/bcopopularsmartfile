<%@ Page Title="" Language="C#" MasterPageFile="Site1.Master" AutoEventWireup="true"
    CodeBehind="IndexacionImg.aspx.cs" Inherits="GestorDocumental.ViewsAspx.IndexacionImg" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        window.onload = function () {
            CargarDoc();
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#splitterContainer").splitter({
                minAsize: 490,
                maxAsize: 900,
                splitVertical: true,
                A: $('#leftPane'),
                B: $('#rightPane'),
                slave: $("#rightSplitterContainer"),
                closeableto: 0
            });
        });

        function mycallbackfunc(e, v, m, f) {
            tiffobj0.style.display = 'inline';
        }

        function HideTv() {
            tiffobj0.style.display = 'none';
        }
        var neg;
        var NumPaginas;

    </script>
    <style type="text/css">
        #splitterContainer
        {
            /* Main splitter element */
            height: 90%;
            width: 100%;
            margin: 0;
            padding: 0;
        }
        
        #leftPane
        {
            float: left;
            width: 25%;
            height: 100%;
            border-top: solid 1px #9AAE04;
            background: #9AAE04;
            overflow: auto;
        }
        
        #rightPane
        {
            /*Contains toolbar and horizontal splitter*/
            float: right;
            width: 51% !important;
            height: 100%;
        }
        
        .splitbarV
        {
            height: 550px;
        }
        #contentDoc, #contentPagIndexada
        {
            margin-top: 15px;
            margin-left: 15px;
        }
        
        #contentDoc th, #contentPagIndexada th
        {
            text-align: center;
            width: 0.5%;
            background-color: #1994A4;
            border: solid 1px;
            color: White;
        }
        
        #contentDoc td, #contentDoc input, #contentPagIndexada td
        {
            text-align: center;
        }
        
        #contentPaginacion
        {
            margin-left: 20px;
            margin-bottom: 15px;
        }
        
        
        #contentPaginacion td
        {
            text-align: right;
        }
        
        #contentPagIndexada
        {
            box-shadow: 0px 0px 5px 3px rgba(154, 174, 4, 0.47);
            margin-top: 15px;
        }
        
        #MainContent_UpdatePanel1
        {
            box-shadow: 0px 0px 5px 3px rgba(154, 174, 4, 0.47);
            margin-top: 15px;
        }
        
        #contentDoc input
        {
            margin-left:10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h3>
        <%:Session["TITULO"] %></h3>
    <div id="splitterContainer">
        <div id="leftPane">
            <div style="position: inherit!important; top: -42px!important; height: 550px!important">
                <object id="tiffobj0" width="100%" height="90%" classid="CLSID:106E49CF-797A-11D2-81A2-00E02C015623"
                    style="margin-top: 0px; visibility: visible;">
                    <param name="src" value="samples/sample.alttif" />
                    <param name="wmode" value="transparent">
                    <embed toolbaritems="<%: Session["TOOL_BAR"] %>" id="tiffemb0" width="100%" height="100%"
                        src="" type="application/x-alternatiff" wmode="transparent">
                </object>
            </div>
        </div>
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
                tiff0.LoadImage(neg, 1, 0)
            }

            function CargarPaginaDigitada() {
                var pag = document.getElementById('txtPagina').value;
                tiff0.GoToPage(pag);

            }

            function onRequestEnd() {
                $.unblockUI();
                var pag = document.getElementById('txtPagina').value;
                tiff0.GoToPage(pag);
            }
        </script>
        <div id="rightPane">
            <div style="top: -42px !important">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <div id="contentDoc">
                                                        <table style="width: 85%;">
                                                            <tr>
                                                                <td valign="top">
                                                                    <%-- Nueva grilla MVC --%>
                                                                    <%%>

                                                                    <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#DEDFDE"
                                                                        BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                                                                        AutoGenerateColumns="False">
                                                                        <AlternatingRowStyle BackColor="White" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="DOCID" HeaderText="ID" />
                                                                            <asp:BoundField DataField="DOCDESCRIPCION" HeaderText="DOCUMENTO" />
                                                                        </Columns>
                                                                        <FooterStyle />
                                                                        <HeaderStyle Font-Bold="True" ForeColor="Black" />
                                                                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" />
                                                                        <RowStyle />
                                                                        <SelectedRowStyle Font-Bold="True" ForeColor="Black" />
                                                                        <SortedAscendingCellStyle />
                                                                        <SortedAscendingHeaderStyle />
                                                                        <SortedDescendingCellStyle />
                                                                        <SortedDescendingHeaderStyle />
                                                                    </asp:GridView>
                                                                </td>
                                                                <td valign="top">
                                                                    <div id="contentPaginacion">
                                                                        <table style="width: 100%;">
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Pagina:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtPagina" runat="server" ClientIDMode="Static" Width="80px" OnTextChanged="txtPagina_TextChanged"
                                                                                        Enabled="false" ReadOnly="True"></asp:TextBox>
                                                                                    <asp:RoundedCornersExtender ID="txtPagina_RoundedCornersExtender" runat="server"
                                                                                        Enabled="True" TargetControlID="txtPagina">
                                                                                    </asp:RoundedCornersExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Documento:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDocumento" runat="server" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="txtDocumento_FilteredTextBoxExtender" runat="server"
                                                                                        Enabled="True" FilterType="Numbers" TargetControlID="txtDocumento" ViewStateMode="Enabled">
                                                                                    </asp:FilteredTextBoxExtender>
                                                                                    <asp:RoundedCornersExtender ID="txtDocumento_RoundedCornersExtender" runat="server"
                                                                                        Enabled="True" TargetControlID="txtDocumento">
                                                                                    </asp:RoundedCornersExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Ingresar"
                                                                                        CssClass="btn btn-login" />
                                                                                    &nbsp;&nbsp;&nbsp;
                                                                                    <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar" OnClick="btnFinalizar_Click"
                                                                                        CssClass="btn btn-login" Visible="False" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
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
                                    <asp:Panel ID="pControls" runat="server">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="contentPagIndexada">
                                                        <asp:GridView ID="gvDocsIndexados" runat="server" BackColor="White" BorderStyle="None"
                                                            BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" OnRowCommand="gvDocsIndexados_RowCommand">
                                                            <AlternatingRowStyle BackColor="White" />
                                                            <Columns>
                                                                <asp:ButtonField ButtonType="Button" CommandName="DEL" Text="Eliminar" ControlStyle-CssClass="btn btn-login" />
                                                            </Columns>
                                                            <FooterStyle />
                                                            <HeaderStyle Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" />
                                                            <RowStyle />
                                                            <SelectedRowStyle Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle />
                                                            <SortedAscendingHeaderStyle />
                                                            <SortedDescendingCellStyle />
                                                            <SortedDescendingHeaderStyle />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:RoundedCornersExtender ID="pControls_RoundedCornersExtender" runat="server"
                                        Corners="All" Enabled="True" TargetControlID="pControls">
                                    </asp:RoundedCornersExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
