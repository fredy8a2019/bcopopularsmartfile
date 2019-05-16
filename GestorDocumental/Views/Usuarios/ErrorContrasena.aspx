<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>ErrorContrasena</title>
    <script type="text/javascript">
        alert("La nueva contraseña ingresada fue usada anteriormente.");
        location.href = "/Usuarios/index";
    </script>
</head>
<body>
    <div>
        
    </div>
</body>
</html>
