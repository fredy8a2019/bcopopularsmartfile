<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Validación Documental
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

     <meta http-equiv="expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="-1">
    
    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />

    <%--<link rel="stylesheet" href="http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css">
    <link href="http://www.jqueryscript.net/css/jquerysctipttop.css" rel="stylesheet" type="text/css">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script src="http://netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js"></script>

    <%--<script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../../Content/Scripts/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/i18n/jquery.ui.datepicker-es.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.button.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.menu.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.tooltip.js" type="text/javascript"></script>

    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>

    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>--%>


    <link rel="stylesheet" href="../../Styles/menu-6.css" type="text/css" />

    <style type="text/css">
        body {
            background: #ffffff;
        }

        .divleft {
            float: left;
            display: inline;
            width: 27%;
        }

            .divleft label {
                display: inline;
            }

        #cotBuscar {
            float: left;
            display: inline;
            width: 80%;
        }

            #cotBuscar input[type=text], #contentSearch input[type=text] {
                width: 31%;
                margin-right: 25px;
                margin-left: 20px;
            }

        textarea {
            width: 86%;
            border-radius: 5px;
            margin-right: 25px;
        }

        .span {
            /*background-color: rgb(238, 238, 238) !important;*/
            color: rgb(102, 102, 102) !important;
            border: none !important;
            cursor: pointer !important;
            padding: 2px 12px 3px 12px !important;
            text-decoration: none !important;
            border: 1px solid rgb(128,128,128) !important;
            border-radius: 4px !important;
        }

        #arriba {
            position: relative;
            z-index: 1;
        }

        #abajo {
            position: relative;
            z-index: 0;
        }

        .span:hover {
            background-color: rgb(250, 250, 250) !important;
        }

        .modal {
            /*background-color: rgba(0,0,0,.8);*/
            background-color: rgb(128,128,128);
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            display: block;
        }

        element.style {
            font-weight: bolder;
        }

    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        var paginaValDocumental = null;
        var _negId = '<%=ViewData["_negId"]%>';
        (function () {
            
            paginaValDocumental = {
                loadValDocumental: function () {
                    $(document).ready(function () {
                        $("GridConsultaCausales.t-grid .t-refresh").trigger('click');
                        
                        var negId = '<%=ViewData["_negId"]%>';
                        if (negId == 0) {
                            $("#alerta2").removeAttr("Style");
                            $("#CausalesImg").prop("disabled", true);
                            $("#CausalesG").prop("disabled", true);
                            $("#LstCausales").prop("disabled", true);
                        }
                    });
                }
            },
            paginaValDocumental.loadValDocumental();
        })();


        var neg;
        var url = '<%=Session["_neg_"]%>';
        neg = '../../../Content/ArchivosCliente/<%=ViewData["_negId"]%>/<%=ViewData["_negId"]%>.pdf';

        function inicioPDF() {
            //'page-fit'
            //'auto'
            //'page-width'
            document.getElementById('Iframe1').contentWindow.scaleInicial('page-fit');
            document.getElementById('Iframe1').contentWindow.GotoPages('1');
        }

        //funcion seleccion de página
        function alerta(pag, docId) {

            document.getElementById('Iframe1').contentWindow.GotoPages(pag);
            transact.ajaxPOST("/Val_Documental/generaCampos?_DocId=" + docId, null,
                function (data) {

                    $('#strongP').empty();
                    $('#strongP').append(data[1]);
                    $('#strongP').html();

                    if (data[0] == 1) {
                        data = "";
                        //Alternar(alertNoHayCausales);
                        $('#strong1').empty();
                        $('#strong1').append("No existen causales parametrizadas para el documento seleccionado!");
                        Alternar1(alertNoHayCausales);
                        $('#strong1').html();
                    }

                    $('#CausalesImagen').empty();
                    $("#CausalesImagen").html(data[0]);
                }, function (error) { console.log(error) });
        }

        //FUNCION VALORES POLITICAS ESPECIFICAS
        function CausalesImagenSN() {
            var valLst;

            var contador = 0;
            var control = true;
            for (var i = 0; i < $("#CausalesImagen :input").length; i++) {
                if ($("#CausalesImagen :input")[i].value == '-1') {
                    control = false;
                }
            }
            if (!control) {
                $('#strong1').empty();
                $('#strong1').append("Debe validar todas las causales!");
                Alternar1(alertNoHayCausales);
                $('#strong1').html();
            } else {
                var _snCausal;
                var nameSelect;
                var contCausal = 0;
                for (var i = 0; i < $("#CausalesImagen :input").length; i++) {
                    $("#CausalesImagen :input")
                    if (contCausal == 0) {

                        nameSelect = $("#CausalesImagen :input")[i].name;
                        valLst = $("#CausalesImagen :input")[i].value;
                        _snCausal = valLst;

                    } else {

                        nameSelect = nameSelect + "_" + $("#CausalesImagen :input")[i].name;
                        valLst = $("#CausalesImagen :input")[i].value;
                        _snCausal = _snCausal + "_" + valLst;

                    }
                    contCausal = contCausal + 1;
                }
                transact.ajaxPOST("/Val_Documental/CausalesEspecificas?_campId=" + nameSelect + "&_snCausal=" + _snCausal + "&_negId=" + _negId + "&cantCausal=" + contCausal, null,
                                    function (data) {
                                        if (data == 1) {
                                            $('#strong1').empty();
                                            $('#strong1').append("Debe validar todas las causales!");
                                            Alternar1(alertNoHayCausales);
                                            $('#strong1').html();
                                        }
                                        //cambio de metodo william
                                        // refrescaGrillaCausales();
                                    },
                                    function (error) { console.log(error); });
                refrescaGrillaCausales();
            }
        }


        //FUNCION VALORES POLITICAS GENERALES
        function CausalesGeneralesSN() {

            var valChck;
            var codPolG;
            var contador = 0;

            $("#CausalesGenerales").find('input[type=radio]:checked').each(function () {
                var valChck = $(this).val();
                var valoresHidden = $("#CausalesGenerales").find(':input[type=hidden]');

                //console.log(_negId, valoresHidden[contador].id, $(this).val());

                InstCausales(valoresHidden[contador].id, valChck, _negId);
                contador++;
            });

            refrescaGrillaCausales();
        }

        function finValidacion(Seccion) {

            var valChck = 0;
            var txtObservaciones = $("#txtObservacion").val();

            $("#SN_Indexacion").find(function () {

                if ($('input[name="indxSN"]').is(':checked')) {
                    valChck = 1
                }
            });


            transact.ajaxGET("/Val_Documental/snCausalesCompletas?_negId=" + _negId + "&_snIndx=" + valChck + "&_observaciones=" + txtObservaciones, null,
                function (data) {
                    if (data == 1) {
                        window.location.href = "/Val_Documental/confirmaValidacion?_negId=" + _negId;
                    } else if ((data == 0)) {
                        Alternar(Seccion);
                    }
                },
                function (error) {
                    console.log(error);
                });

            //transact.ajaxPOST("/Val_Documental/finValidacion?negId=" + _negId, null,
            //function () { window.location.href = "/Val_Documental/confirmaValidacion"; },
            //function (error) { console.log(error) });

        }

        function refrescaGrillaCausales() {
            $("#GridConsultaCausales.t-grid .t-refresh").trigger('click');
        }

        function InstCausales(_codCausal, _snCausal, neg) {
            transact.ajaxPOST("/Val_Documental/InsertaCausales?_codCausal=" + _codCausal + "&_snCausal=" + _snCausal + "&_negId=" + _negId, null,
                function () { },
                function (error) { console.log(error) });
        }


        function Alternar(Seccion) {
            if (Seccion.style.display == "none") {
                Seccion.style.display = ""
                window.setTimeout(function () {
                    $(".alert-danger").fadeTo(300, 0).slideUp(300, function () {
                        $(this).removeAttr("style");
                        $(this).css("display", "none");
                    });

                }, 3000);
            }
        }
        function Alternar1(Seccion) {
            if (Seccion.style.display == "none") {
                Seccion.style.display = ""
                window.setTimeout(function () {
                    $(".alert-danger-1").fadeTo(300, 0).slideUp(300, function () {
                        $(this).removeAttr("style");
                        $(this).css("display", "none");
                    });
                }, 3000);ul
            }
        }

        //INICIO: Funciones Para el visor PDF (creadas por Carlos Torres)
        function RutaNegocio() {
            if ('<%=ViewData["_negId"]%>' != '') {
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
    <br />
    <br />
    <br />
    <table style="width: 100%">
        <tr>
            <td>
                <h3>Validación Documental </h3> <h4>Negocio: <%=ViewData["TITULO"]%></h4>
            </td>
        </tr>
    </table>

    <%--MENU DESPLEGABLE HORIZONTAL --%>
    <%--
    <div class="row-fluid">
        <fieldset class="scheduler-border">
            <legend>Documentos relacionados</legend>
            <div id="menu">
                <ul>
                    <li class="nivel1"><a class="nivel1 primera"><%=ViewData["_negId"]%>.tiff</a>
                        <ul id="primero">
                            <%=ViewData["_cadenaMenu"].ToString() %>
                        </ul>
                    </li>
                </ul>
            </div>
        </fieldset>
    </div>
    --%>
    <div class="row-fluid" style="position: relative" id="arbol">
        <fieldset class="scheduler-border">
            <legend>Datos</legend>
            <%--<h3><%=ViewData["_mensajeInformacion"] %></h3>--%>
            <div class="alert alert-danger" id="alerta2" style="display: none;">
                <strong>Atención! </strong><%=ViewData["_mensajeInformacion"] %>
            </div>
            <div class="col-lg-5" style="right: inherit;">
                <%--MENU DESPLEGABLE VERTICAL --%>
                <fieldset class="scheduler-border">
                    <legend class="scheduler-border">Documentos relacionados</legend>
                    <div class="easy-tree" style="height: 280px; overflow-y: auto">
                        <ul style="font-weight: normal;">
                            <li>Negocio <%=ViewData["_negId"]%>
                                <ul>
                                    <li><%=ViewData["_negIdProduc"]%>
                                        <ul id="arbolDocs">
                                            <%=ViewData["_cadenaMenu"].ToString() %>
                                        </ul>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                    <script>
                        (function ($) {
                            function init() {
                                $('.easy-tree').EasyTree({
                                    selectable: true,
                                    addable: false,
                                    editable: false,
                                    deletable: false
                                });
                            }

                            window.onload = init();
                        })(jQuery)
                    </script>
                </fieldset>
                <br />
                <strong id="strongP" style="font-size: 1.2em"><b></b></strong>
                <fieldset class="scheduler-border" id="CausalesImg">
                    <legend></legend>
                    <div class="alert alert-danger-1" id="alertNoHayCausales" style="display: none;">
                        <strong id="strong1"></strong>
                    </div>
                    <div id="CausalesImagen" style="overflow-y: auto">
                    </div>
                    <br />
                    <input type="submit" id="btnAgregaCausalesI" value=".:Actualizar Causales:." class="btn btn-Comando" onclick="CausalesImagenSN()" />
                </fieldset>

            </div>
            <div class="span6">
                <div id="abajo" style="position: inherit!important; /*top: -42px!important; height: 550px!important*/">
                    <div id="zoom" style="overflow: scroll;" hidden="hidden" onmousemove="moverScrolls(event)"></div>
                    <fieldset class="scheduler-border" style="width: 660px; height: 650px">
                        <legend class="scheduler-border">&nbsp;</legend>
                        <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 90%;" id="Iframe1" allowfullscreen></iframe>
                    </fieldset>
                </div>
                <div id="SN_Indexacion">
                    <%--<fieldset class="scheduler-border" style="width: 510px; height: 80px">
                        <legend class="scheduler-border">Verificar Indexación</legend>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <label>INDEXACIÓN ERRADA </label>
                                </td>
                                <td>
                                    <input type="checkbox" name="indxSN" id="indxSN" value="1" />
                                    <input type="hidden" name="H_indxSN" id="H_indxSN" value="0" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>--%>


                    <%--<fieldset class="scheduler-border" id="CausalesG" style="width: 510px"> SE quita el elemento de las causales generales 
                        william Eduardo Cicua
                        <legend class="scheduler-border">Políticas generales</legend>
                        <div id="CausalesGenerales">
                            <table style="width: 100%">
                                <%=ViewData["_cadenaPoliticas"].ToString() %>
                            </table>
                            <br />
                        </div>
                        <br/>

                        <input type="button" id="btnAgregaCausalesG" value=".:Actualizar Causales:." class="btn btn-Comando" onclick="CausalesGeneralesSN()" />

                    </fieldset>--%>
                </div>
            </div>
        </fieldset>
    </div>
    <fieldset class="scheduler-border" id="LstCausales">
        <legend class="scheduler-border">Listado de causales</legend>
        <div class="alert alert-danger" id="alerta1" style="display: none">
            <strong>Atención!</strong> No se ha realizado la validación de todas las causales para el respectivo Negocio.
        </div>
        <div>
            <%= Html.Telerik().StyleSheetRegistrar()
                    .DefaultGroup(group => group.Add("telerik.hay.css")) %>
            <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_CausalesNeg>()
                    .Name("GridConsultaCausales")
                    .DataBinding(databinding => databinding.Ajax())
                    .Columns(colums =>
                    {
                        colums.Bound(o => o._Documento).Title("Documento Validado").HeaderHtmlAttributes(new { style = "center: 40px; height: 30px; " });
                        colums.Bound(o => o._nomCausal).Title("Política");
                        colums.Bound(o => o._descCausal).Title("Descripción Política");
                        colums.Bound(o => o._fecValidacion).Format("{0:F}").Title("Fecha Validación");
                        
                    })
                    .DataBinding(d => d.Ajax().Select("getConsulCausales", "Val_Documental", new { _negId = ViewData["_negId"].ToString() }))
                    .Pageable(paginas => paginas.PageSize(10))
                    .Sortable()
                    .Filterable()
                    .ToHtmlString()
            %>
        </div>
        <br />
        <label>Observaciones:</label>
        <input type="text" id="txtObservacion" style="width: 100%; height: 100px" maxlength="250" required="required" />
        <br />
        <div>
            <input type='button' id='btnCancelar' value='.: Finalizar Validación :.' class='btn btn-login' onclick="finValidacion(alerta1)" />
        </div>

    </fieldset>
    <%--<script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>--%>
</asp:Content>