<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>confirmaPagina</title>
    <link rel="stylesheet" href="../../Styles/jquery-ui.css" />
    <script type="text/javascript" src="../../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.min.js"></script>
    <link href="../../Styles/BootsTrap/css/bootstrap.min.css" rel="stylesheet" />

    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            bootbox.dialog({
                message: "<h4>¿Continuar con el siguiente negocio?</h4>",
                title: "Confirmación",
                buttons: {
                    success: {
                        label: ":: Continuar ::",
                        className: "btn-success",
                        callback: function () {
                            window.location.href = '/CapturaInformacion/Index?Captura=<%=ViewData["captura"]%>';
                        }
                    },
                    danger: {
                        label: ":: Cancelar ::",
                        className: "btn-danger",
                        callback: function () {
                            window.location.href = "/Home/Index";
                        }
                    }
                }
            });
        });
    </script>
</head>
<body>
    <div>
        
    </div>
</body>
</html>
