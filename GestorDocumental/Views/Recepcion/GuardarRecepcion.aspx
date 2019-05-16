<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <script type="text/javascript" language="javascript">
        alert('Registro para recepción exitoso, recuerde el número de lote asignado es <%=ViewData["numero_lote"] %>');
        location.href = '/Recepcion/Index';
    </script>
</head>
<body>
    <div>
        
    </div>
</body>
</html>
