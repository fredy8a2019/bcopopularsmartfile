<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="GestorDocumental.Controllers" %>
<%@ Import Namespace="GestorDocumental.Models" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Login</title>
    <link href="../../Styles/InicioStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jqBootstrapValidation.js" type="text/javascript"></script>
    <link rel="Shortcut Icon" href="../../Images/Icono.ico" />
</head>
<body>
    <div>
        <form id="form1" runat="server" action="/Seguridad/ValidarUsuario">
        <div id="loginBox" class="centrar form-group">
            <div>
                <img src="../../Images/Everis1.fw.png" />
                <br />
                <label class="control-label">
                    Usuario:
                </label>
                <div class="form-group has-success has-feedback" id="textUsuario" runat="server">
                    <asp:TextBox ID="txtUsuario" PlaceHolder="Ingrese..." CssClass="form-control" Required
                        runat="server"></asp:TextBox>
                </div>
                <label class="control-label">
                    Contraseña: </label>
                <div class="form-group has-success has-feedback" id="textContrasena" runat="server">
                    <asp:TextBox ID="txtContrasena" PlaceHolder="Ingrese..." CssClass="form-control"
                        Required runat="server" TextMode="Password"></asp:TextBox>
                </div>
                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn btn-login" Font-Bold="True"
                    ForeColor="White" /><br />
                <asp:Label ID="lblError" runat="server" ForeColor="Red"><%=ViewData["Respuesta"]%></asp:Label>
            </div>
        </div>
        </form>
    </div>
</body>
</html>
