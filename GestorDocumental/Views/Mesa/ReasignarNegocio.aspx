<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>ReasignarNegocio</title>
    <script type="text/javascript">
        if (<%:ViewData["estado"]%> == 110) {
             location.href = '/Mesa/Index';
        }
        else if (<%:ViewData["estado"]%> == 100) {
            location.href = '/Mesa/Busqueda';
        }
    </script>
</head>
<body>
    <div>
    </div>
</body>
</html>
