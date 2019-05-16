<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
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
    <script type="text/javascript">
        var _default = null;
        $(function () {
            $("#dialog-confirm").dialog({
                resizable: false,
                height: 190,
                width: 400,
                modal: true,
                buttons: {
                    //"SI": function () {
                    //    if ($("#txtParametroPRINT").val() == "1") {
                    //        _default._imprimirLaser();
                    //    } else if ($("#txtParametroPRINT").val() == "2") {
                    //        _default._imprimirZebra();
                    //    }
                    //},
                    ":: Aceptar ::": function () {
                        window.location.href = '/Almacenar/CrearUnidad';
                    }
                }//bUTONS

            });

            _default = {

                _imprimirLaser: function () {
                    $("#dialog-confirm").remove();

                    var stringHTML = " <div>" +
                    "<div id=\"CUD\">" +
                        "<label>" +
                            $("#txtCUD").val() + "</label>" +
                    "</div>" +
                    "<div id=\"detalle\">" +
                        "<span>Empresa:</span><label>" +
                            "Everis BPO</label>" +
                        "<br />" +
                    "<span>Fechas:</span><label>" +
                        new Date().toLocaleDateString() + "</label>"
                    " </div></div>";


                    $("#impresoraLaser").append(stringHTML);
                    window.print();
                    window.location.href = '/Almacenar/CrearUnidad';
                },

                _imprimirZebra: function () {


                }
            }

        });
    </script>
    <style>
        label
        {
            font-size: 4.1em;
        }

        #dialog-confirm
        {
            display: none;
        }

        #CUD
        {
            float: left;
            width: 6%;
            height: 6%;
            text-align: center;
            vertical-align: middle;
        }

            #CUD label
            {
                font-size: 5.5em;
            }

        #detalle
        {
            margin-left: 12%;
        }

            #detalle label
            {
                font-size: 2.0em;
            }

            #detalle span
            {
                font-size: 1.5em;
            }
    </style>
    <title>Guardar</title>
</head>
<body>
    <input type="hidden" id="txtCUD" name="name" value="<%= ViewData["CUD"] %>" />
    <input type="hidden" id="txtParametroPRINT" name="name" value="<%= ViewData["parametroImpresion"] %>" />
    <div id="dialog-confirm" style="margin-left: 0" title="Confirmacion">
        <br />
        <br />
        <label>
            CUD :
        </label>
        <label id="subProducto">
            <%=ViewData["CUD"]%>
        </label>
        <span id="mensaje"></span>
    </div>
    <div id="impresoraLaser">
    </div>
</body>
</html>
