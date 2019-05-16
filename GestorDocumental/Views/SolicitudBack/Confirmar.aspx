<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title></title>
    <link rel="stylesheet" href="../../Styles/jquery-ui.css" />
    <script type="text/javascript" src="../../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.min.js"></script>
    <link href="../../Content/themes/controls.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>

    <link href="../../Styles/BootsTrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Styles/Default.css" rel="stylesheet" type="text/css" />
    <link rel="Shortcut Icon" href="../../Images/Icono.ico" />
    <link href="../../Content/Styles/impromptu.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <div>
        <script type="text/javascript">
            $.noConflict();
            var _default = null;
            (function ($) {
                _default = {
                    countRow: 1,
                    _loadPage: function () {
                        $(document).ready(function () {
                            confirmarTransaccion();
                        });
                    }
                }
                _default._loadPage();
            })(jQuery);

            function confirmarTransaccion() {
                bootbox.dialog({
                    message: "<h4>Proceso terminado correctamente</h4>",
                    title: "<b>Confirmación</b>",
                    buttons: {
                        success: {
                            label: ":: Aceptar ::",
                            className: "btn btn-login",
                            callback: function () {
                                window.location.href = "/SolicitudBack/Index";
                            }
                        }
                    }
                });
            }

        </script>
        <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
    </div>
</body>
</html>
