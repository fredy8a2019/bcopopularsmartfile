<%@ Page Title="" Language="C#" MasterPageFile="Site1.Master" AutoEventWireup="true" CodeBehind="ConsultaDocsNegocios.aspx.cs" Inherits="GestorDocumental.ConsultaDocsNegocios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  <script type="text/javascript" src="Scripts/splitter1.js"></script>
     <link href="Styles/estilos.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $().ready(function () {
            $("#MySplitter").splitter({
                type: "h",
                sizeTop: false, /* use height set in stylesheet */
                accessKey: "P"

            });
        });



        var neg;
        var path = '';
    </script>


    <style>
    

        #TopPane
        {
           /* background: #ffe;  Initial/min/max height for this pane */
            min-height: 120PX;
            max-height: 200PX; /* No margin or border allowed */
        }
        /*
 * Bottom element of the splitter; the plugin changes the top
 * position and height of this element dynamically.
 */
        #BottomPane
        {
           /* background: #ffd;*/
            overflow: auto; /* No margin or border allowed */
        }
        /* 
    
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
 <div id="MySplitter">

    <div id="TopPane">
      <table >
        <tr>
            <td>
                <table >
                    <tr>
                        <td >
                            Cedula:</td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtCedula" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                                        onclick="btnBuscar_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Negocios:</td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="lstNegocios" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="lstNegocios_SelectedIndexChanged" 
                                        ClientIDMode="Static" Enabled="False">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Documentos:</td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="lstDocs" runat="server" ClientIDMode="Static" 
                                        Enabled="False" onselectedindexchanged="lstDocs_SelectedIndexChanged">
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
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Width="100%"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </div> 
      <div id="BottomPane">
      
            <object id="tiffobj0" width="100%" height="90%" classid="CLSID:106E49CF-797A-11D2-81A2-00E02C015623"
                style="margin-top: 0px">
                <param name="src" value="../Content/ArchivosCliente/553/553.tif" />
                <embed id="tiffemb0" width="100%" height="100%" type="application/x-alternatiff">
            </object>

             <script type="text/javascript" >

                 var tiff0;
                 if (document.getElementById) {
                     if (document.getElementById('tiffemb0')) {
                         tiff0 = document.getElementById('tiffemb0');
                     }
                     else if (document.getElementById('tiffobj0')) {
                         tiff0 = document.getElementById('tiffobj0');
                     }
                 }else if (document.all) {
                     tiff0 = document.all.tiffobj0;
                 }



                 function CargarDocumento() {
                     var e = document.getElementById("lstNegocios");
                     var strSel = e.options[e.selectedIndex].value;  //+ " and text is: " + e.options[e.selectedIndex].text;
                     path = path + '/' + strSel + '/' + strSel + '.TIF';
                     //alert(path);
                     //path = '../Content/ArchivosCliente/553/553.tif';
                     tiff0.LoadImage(path, 1, 0);

                 }

                 function CargarPagina() {
                     var e = document.getElementById("lstDocs");
                     var strSel = e.options[e.selectedIndex].value;
                     tiff0.GoToPage(strSel);
                 }

                 $(document).ready(CargarDocumento);

</script>
      </div> 
 </div> 
  
</asp:Content>

