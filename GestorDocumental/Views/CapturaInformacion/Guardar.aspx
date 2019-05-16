<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Guardar</title>
    <script type="text/javascript">
        window.location.href = '/CapturaInformacion/Index?Captura=<%=Session["_NoCaptura"]%>';
    </script>
</head>
<body>
    <div>
        
    </div>
</body>
</html>
