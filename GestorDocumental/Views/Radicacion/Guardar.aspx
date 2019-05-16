<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.mouse.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.button.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.dialog.js" type="text/javascript"></script>
    <link href="../../Content/themes/demos.css" rel="stylesheet" type="text/css" />
    <title>Guardar</title>
</head>
<body>
    <div id="dialog-confirm" style="margin-left: 0" title="Confirmacion">
        <label id="subProducto">
            <%=ViewData["subProducto"]%>
        </label>
        <br />
        <br />
        <label id="estadoDescrip">
            <%=ViewData["estadoDescrip"]%>
        </label>
        <br />
        <br />
        <label id="causal">
            <%=ViewData["motivoDevolucion"]%>
        </label>
        <p id="idLabel">
            <%= ViewData["Lable"]%>
        </p>
        <input type="hidden" id="oficina" name="name" value="<%= ViewData["Oficina"]%>" />
        <input type="hidden" id="sociedad" name="sociedad" value="<%= ViewData["descSociedad"] %>" />
        <input type="hidden" id="estadoRadicacion" value="<%=ViewData["estadoRadicacion"] %>" />
        <input type="hidden" id="txtEstado" name="name" value="<%=ViewData["estado"]%>" />
        <input type="hidden" id="txtIdRadicacion" name="name" value="<%= ViewData["idRadicacion"] %>" />
        <span id="mensaje"></span>
    </div>
    <script type="text/javascript" language="javascript">

        
        $(function () {
            $("#dialog-confirm").dialog({
                resizable: false,
                height: 190,
                modal: true,
                buttons: {
                    "Aceptar": function () {
                        if ($("#txtEstado").val() == 120) {

                            if ($("#estadoRadicacion").val() == "fisico") {

                                window.location.href = '/Radicacion/Index';
                                $(this).dialog("close");

                            } else {
                                window.location.href = '/Radicacion/Index';
                                $(this).dialog("close");
                            }

                        }
                        else {
                            window.location.href = '/Radicacion/Index';
                            $(this).dialog("close");
                        }
                    }
                }//bUTONS

            });
        });
    </script>
</body>
</html>
