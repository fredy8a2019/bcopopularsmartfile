<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>confirmaValidacion</title>
    <link rel="stylesheet" href="../../Styles/jquery-ui.css" />
    <script type="text/javascript" src="../../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.min.js"></script>
    <link href="../../Styles/BootsTrap/css/bootstrap.min.css" rel="stylesheet" />

    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>

    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>

    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            bootbox.dialog({
                message: "<h4>Desea finalizar la Validación Documental para el Negocio:" + <%=ViewData["_negId"]%> + "</h4>",
                title: "Confirmación",
                buttons: {
                    succes: {
                        label: ".: SI :.",
                        className: "btn-success",
                        callback: function () {
                            transact.ajaxPOST("/Val_Documental/finValidacion", null,
                                function () {
                                    sigNeg();
                                    var _neg_vis_b = '<%=Session["_neg_"] = ""%>';
                                },
                                function (error) { console.log(error); });
                               
                        }
                    },
                    danger: {
                        label: ".: NO :.",
                        className: "btn-danger",
                        callback: function () {
                            window.location.href = '/Val_Documental/Val_Documental';
                        }
                    }
                }
            });
        });

        function sigNeg() {
            bootbox.dialog({
                message: "<h4>¿Continuar con el siguiente negocio?</h4>",
                title: "Confirmación",
                buttons: {
                    success: {
                        label: ":: Continuar ::",
                        className: "btn-success",
                        callback: function () {
                            window.location.href = '/Val_Documental/Val_Documental';
                        }
                    },
                    danger: {
                        label: ":: Cancelar ::",
                        className: "btn-danger",
                        callback: function () {
                            window.location.href = '/Home/Index';
                        }
                    }
                }
            });
        }

        function formularioEnvio(cud, NegId) {
            bootbox.dialog({
                title: "<b>Fin Validación</b>",
                message: "<h4>Validación del Negocio: <b>" + cud + "</b></h4>" +
                            "<form action='/Solicitud/generarSolicitud' method='post'>" +
                            "<input type='hidden' id='NegId' name='NegId' value='" + NegId + "' />" +
                            "<input type='hidden' id='idCUD' name='idCUD' value='" + cud + "' />" +
                            "<%= ViewData["_camposEnvio"] %>" +
                            "<hr>" +
                            "<input type='submit' class='btn btn-login' id='envio' value=':: Enviar ::' />" +
                            "</form>"
            });
        }

    </script>
</head>
<body>
    <div>
    </div>
</body>
</html>
