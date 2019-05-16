<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>GuardarCambiosBusqueda</title>
     <script type="text/javascript" language="javascript">
         function confirmaPagina() {

             var r = confirm("¿Continuar con el siguiente negocio?");

             if (r == true) {
                 location.href = '/Mesa/Busqueda';
             } else {
                 window.location.href = "/ViewsAspx/Inicio.aspx";
             }
         }

         confirmaPagina();
       
    </script>
</head>
<body>
    <div>
        
    </div>
</body>
</html>
