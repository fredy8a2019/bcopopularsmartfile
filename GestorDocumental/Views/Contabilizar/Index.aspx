<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Index</title>
    <link href="../../Styles/BootsTrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap-theme.min.css" rel="stylesheet"
        type="text/css" />
    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../../Content/Scripts/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.button.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.mouse.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.menu.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.tooltip.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.resizable.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Scripts/page/validaciones.js" type="text/javascript"></script>
    <style>
        body {
            font-family: Calibri !important;
        }

        #visor, #datos {
            width: 100%;
            height: 50%;
            padding: 0.5em;
        }

        #Campos {
            width: 90%;
        }

        .custom-combobox {
            position: relative;
            display: inline-block;
            width: 100%;
            font-size: 70%;
            font-family: "Trebuchet MS", "Arial", "Helvetica", "Verdana", "sans-serif";
        }


        .custom-combobox-toggle {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
            /* support: IE7 */
            *height: 1.7em;
            *top: 0.1em;
        }

        .custom-combobox-input {
            margin: 0;
            padding: 0.3em;
            width: 85%;
        }

        #ui-id-1 {
            font-size: 70%;
            font-family: "Trebuchet MS", "Arial", "Helvetica", "Verdana", "sans-serif";
        }

        input[type="text"], input[type="email"] {
            width: 95%;
            height: 1.5em;
            border-radius: 5px;
            border: solid 1px #999;
            padding: 2px 3px 2px 3px;
            font-size: 1em;
            font-family: inherit;
        }

        .boton {
            background-color: #9AAE04;
            font-weight: bold;
            color: White;
        }

        #imgEveris {
            margin-left: 70%;
        }

        .modal {
            background-color: rgba(0,0,0,.8);
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            display: block;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var _default = {

                datosCampos: null,

                _init: function () {
                    transact.ajaxGET("/Contabilizar/_GetCampos?CodOrigen=" + 0, null, _default._successCampos, _default._error);
                    transact.ajaxGET("/Contabilizar/_GetValorCampos?negId=" + $("#NegId").val(), null, _default._successDatos, _default._error);
                },

                _successCampos: function (data) {
                    var termino = _ui.configCampo($("#Campos"), data);
                    _default.datosCampos = data;
                },

                _successDatos: function (data) {
                    _ui.configCampoData(_default.datosCampos, data);
                },

                _error: function (error) {
                    console.log(error);
                },

                _loagPage: function () {
                    $(document).ready(function () {
                        var rol = '<%=Session["ROL_USUARIO"].ToString()%>'
                        if (parseInt(rol) == 10) {
                            $('#btnGuardar').css("display", "none");
                        }

                        _default.mostrarAlertas();
                        _ui.buildCombobox();
                        _ui.builModelDialog($("#dialogInicial"));
                        _ui.builResizable($("#visor"), $("#datos"));
                        _ui.eventClick($("#btnCancelar"), _default._Cancelar);

                        dimencionesFullScreen();
                        setTimeout("inicioPDF()", 1000);
                    });
                },

                _Cancelar: function () {
                    window.close();
                },

                mostrarAlertas: function () {
                    var paginas = $("#numPaginas").val() == "" ? 0 : parseInt($("#numPaginas").val());
                    if (paginas == 1) {
                        $("#unaPagias").css("display", "block");
                        $("#variasPaginas").css("display", "none");
                    } else {
                        $("#unaPagias").css("display", "none");
                        $("#variasPaginas").css("display", "block");
                        $("#cntPaginas").text(paginas);
                    }
                }

            }
            _default._init();
            _default._loagPage();
        });

        //$(document).ready(function () {
        //    dimencionesFullScreen();
        //    setTimeout("inicioPDF()", 1000);
        //});

        var pag;
        var neg = '../../../' + '<%=Session["IMG_DOWN_PDF"] %>';

        function inicioPDF() {
            //'page-fit'
            //'auto'
            //'page-width'
            document.getElementById('Iframe1').contentWindow.scaleInicial('page-width');
            document.getElementById('Iframe1').contentWindow.GotoPages(1);
        }

        //INICIO: Funciones Para el visor PDF (creadas por Carlos Torres)
        function RutaNegocio() {
            if ('<%=Session["IMG_DOWN_PDF"] %>' != '') {
                return neg;
            }
            else {
                return '';
            }
        }

        window.addEventListener('keydown', function keydown(evt) {
            if (document.getElementById("zoom").className == 'modal') {
                switch (evt.keyCode) {
                    case 27: // esc key
                        document.getElementById('Iframe1').contentWindow.salirZoomFullScreen();
                        break;
                    case 61: // FF/Mac '='
                    case 107: // FF '+' and '='
                    case 187: // Chrome '+'
                    case 171: // FF with German keyboard
                        document.getElementById('Iframe1').contentWindow.ZoomInFull();
                        break;
                    case 173: // FF/Mac '-'
                    case 109: // FF '-'
                    case 189: // Chrome '-'
                        document.getElementById('Iframe1').contentWindow.ZoomOutFull();
                        break;
                }
            }
        });

        function moverScrolls(e) {
            var x = e.clientX;
            var y = e.clientY;

            var zoom = document.getElementById('Iframe1').contentWindow.retornaZoom();

            if (isNaN(zoom)) {
                zoom = 0;
            }

            var elmnt = document.getElementById("zoom");
            elmnt.scrollLeft = (x * (zoom + 1));
            elmnt.scrollTop = (y * (zoom + 2));
        }

        function dimencionesFullScreen() {
            var browserWidth = $(window).width(); //document.documentElement.clientWidth;
            var browserHeight = $(window).height(); //document.documentElement.clientHeight;
            document.getElementById("zoom").style.width = browserWidth;
            document.getElementById("zoom").style.height = browserHeight;
        }
        //FIN: Funciones Para el visor PDF 

    </script>
</head>
<body>
    <div id="div">
        <input type="hidden" name="" value="<%=ViewData["paginas"]%>" id="numPaginas" />
        <header>
            <%--    <img src="../../Content/Images/logo_gas_natural.png" />
    <img src="../../Content/Images/logon_everis.png" id="imgEveris" />--%>
            <span>
                <%= ViewData["Cliente"] %></span>
        </header>
        <%--dialogInicial--%>
        <div id="zoom" style="overflow: scroll;" hidden="hidden" onmousemove="moverScrolls(event)"></div>
        <div id="visor" class="ui-widget-content">
            <h3>Negocio
                <%=ViewData["NEG"]%>
            </h3>
            <div>
                <div id="">
                    <div class="alert alert-danger" id="variasPaginas" style="display: none; height: inherit;">
                        <h4 id="MensajeLabel">Este documento contiene <span id="cntPaginas"></span>páginas, por favor recuerde
                            visualizar todos sus anexos.
                        </h4>
                    </div>
                </div>
                <div id="">
                    <div class="alert alert-warning" id="unaPagias" style="display: none; height: inherit;">
                        <h4 id="H1">Este documento contiene una sola pagina.
                        </h4>
                    </div>
                </div>
            </div>
            <a href="<%=Session["IMG_DOWN_PDF"] %>">Guardar en pdf</a>
            <input type="hidden" name="name" id="NegId" value="<%=ViewData["NEG"]%>" />
            <%--<object width="100%" height="100%" id="tiffobj0" classid="CLSID:106E49CF-797A-11D2-81A2-00E02C015623">
                <param name="src" value="<%: Session["IMG_VISOR_REP"] %>">
                <param name="enableevents" value="32">
                <embed toolbaritems="<%: Session["TOOL_BAR_REP"] %>" width="100%" height="70%" id="tiffemb0"
                    enableevents="32" src="<%: Session["IMG_VISOR_REP"] %>" access="8" type="application/x-alternatiff">
            </object>--%>
            <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframe1" allowfullscreen></iframe>
        </div>
        <hr />
        <div id="datos">
            <form action="/Contabilizar/Guardar" onsubmit="remplazarComas(this)" method="post">
                <table id="Campos">
                    <tr>
                        <td>
                            <label>
                                Label:
                            </label>
                        </td>
                        <td>
                            <input type="text" name="name" disabled="disabled" value=" <%= ViewData["Lable"] %>" />
                        </td>
                        <td>
                            <label>
                                Producto:
                            </label>
                        </td>
                        <td>
                            <input type="text" name="name" disabled="disabled" value="<%= ViewData["Producto"]%> " />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                SupProducto:
                            </label>
                        </td>
                        <td>
                            <input type="text" name="name" disabled="disabled" value="<%= ViewData["SubProducto"]%> " />
                        </td>
                        <td>
                            <label>
                                No. Documento:
                            </label>
                        </td>
                        <td>
                            <input type="text" name="name" disabled="disabled" value=" <%= ViewData["NoDocumento"]%>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Id. Proveedor:
                            </label>
                        </td>
                        <td>
                            <input type="text" name="name" disabled="disabled" value="<%= ViewData["Proveedor"]%>" />
                        </td>
                        <td>
                            <label>
                                Sociedad:
                            </label>
                        </td>
                        <td>
                            <input type="text" name="name" disabled="disabled" value="<%= ViewData["Sociedad"]%> " />
                        </td>
                    </tr>
                </table>
                <input type="submit" id="btnGuardar" name="name" value="Guardar" class="btn btn-login" />
                <input type="button" id="btnCancelar" name="name" value="Cancelar" class="btn btn-login" />
            </form>
        </div>
    </div>
</body>
</html>
