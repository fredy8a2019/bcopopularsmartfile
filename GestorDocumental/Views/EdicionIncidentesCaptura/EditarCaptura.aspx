<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Editar Captura
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <meta http-equiv="expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="-1">
    
    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />

    <%--<link rel="stylesheet" href="http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css">
    <link href="http://www.jqueryscript.net/css/jquerysctipttop.css" rel="stylesheet" type="text/css">
    <script src="https://code.jquery.com/jquery-1.12.2.js"></script>
    <script src="http://netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js"></script>

    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../../Content/Scripts/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.button.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.menu.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.tooltip.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.datepicker.js" type="text/javascript"></script>
    <link href="../../Content/themes/controls.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput.js" type="text/javascript"></script>

    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>

    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../Styles/menu-6.css" type="text/css" />
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>--%>

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
            background-color: rgb(238, 238, 238) !important;
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

        h2 {
            font-size: 30px;
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

        .t-header {
            background-color: #205390;
            color: white;
            height: 30px;
        }

            .t-header .t-link {
                color: white;
                height: 100%;
                width: 100%;
            }

        #TopPane {
            /*overflow: auto;*/
            height: 100px;
            min-height: 250px;
            max-height: 100%; /*Valor maximo de expansion de la ventana del Tiff*/
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid" style="height: 800px">
        <br />
        <div class="alert alert-danger" id="divError" style="display: none">
            <strong id="stro2">Negocio no especificado o modulo no especificado</strong>
        </div>
        <div id="zoom" style="overflow: scroll;" hidden="hidden" onmousemove="moverScrolls(event)"></div>

        <h3>Edicion Incidencias Captura</h3>

        <h4>Negocio: <%=ViewData["TITULO"]%></h4>
        
        <h4><%=Session["ErrorCaptura"]%></h4>

        <div class="alert alert-danger" id="negocioVacio" style="display: none">
            <strong id="stro1"></strong>
        </div>
        <br />
        <fieldset class="scheduler-border" style="width: 1050px;">
            <%--<div class="span5">
                    <h4>Documento Original:</h4>
                    <div style="position: inherit!important; top: -42px!important; margin-top: 5%; height: 550px!important">
                        <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx"  style="width: 100%;  height: 600px" id="Iframe1" ></iframe>
                 </div>--%>

            <div id="MySplitter">
                <div id="TopPane" style="margin-top: 1%;">
                    <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframe1"></iframe>
                </div>
            </div>

            <br />
            <div id="Div3" style="width: 100%; overflow-x: scroll">
                <%                     
                    GridEditMode mode = (GridEditMode)ViewData["mode"];
                    GridButtonType type = (GridButtonType)ViewData["type"];
                    GridInsertRowPosition insertRowPosition = (GridInsertRowPosition)ViewData["insertRowPosition"];    
                %>
                <%= Html.Telerik().StyleSheetRegistrar() 
                                                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
                <%= Html.Telerik().Grid<GestorDocumental.Models.sp_InCap_ObtenerCampos_Result>()
                        .Name("TelEdicionIncidentes")
                        
                        .DataKeys(keys =>
                        {
                            keys.Add(o => o.numeroFila);
                        })
                        .Sortable()
                        .Localizable("es-ES")
                        .DataBinding(databindig => databindig.Ajax()
                            .Select("_ConsultarCamposNegocio", "EdicionIncidentesCaptura")
                            .Update("_GuardarEdicionCaptura", "EdicionIncidentesCaptura", new {valor =  "<#= valor #>"}))
                        .Columns(colums =>
                        {
                            colums.Bound(o => o.numeroFila).Width(5).Title("numeroFila").Visible(false);
                            colums.Bound(o => o.campid).Width(10).Title("Id Campo").HtmlAttributes(new { id = "campid" })/*.Visible(false)*/;
                            colums.Bound(o => o.indice).Width(5).Title("indice").Visible(true);
                            colums.Bound(o => o.numCaptura).Title("NumCaptura").Visible(false);
                            colums.Bound(o => o.docid).Title("Docid").Visible(false);
                            colums.Bound(o => o.documento).Width(20).Title("Documento");
                            colums.Bound(o => o.tipocampo).Width(20).Title("Tipo Campo");
                            colums.Bound(o => o.NombreCampo).Width(20).Title("Nombre Campo");
                            colums.Bound(o => o.valor).Width(20).Title("Valor Campo");

                            colums.Command(commands =>
                            { commands.Edit().ButtonType(type); }).Width(10);
                        })
                        .Filterable()
                        .ClientEvents(events => events.OnEdit("onEdit"))
                        .Editable(editing => editing
                           
                        .InsertRowPosition(insertRowPosition)
                        .Mode(mode))
                        .Pageable(paginas => paginas.PageSize(20))
                        .Scrollable(scrolling => scrolling.Height(400))
                        .Resizable(resizing => resizing.Columns(true))
                %>
            </div>
            <div class="col-lg-4" style="margin: 15px 10px 10px 10px">
                <button class="btn btn-login" style='width: 140px; height: 34px;' id="btn_GuardarEnviar" name="btn_GuardarEnviar" onclick="GuardarEnviar()">
                    <span class="glyphicon glyphicon-floppy-disk" aria-hidden="true"></span>&nbsp;Guardar y Enviar
                </button>
            </div>
            <br />
        </fieldset>
    </div>

    <script>

        function onEdit(e) {

            var tipocampo = $(e.form).find('#tipocampo').parent().find("input[type='text']").val();

            if (tipocampo == 'Lista desplegable') {
                var Campid = $(e.form).find('#valor').parent().find("input[type='text']").val();
                //$(e.form).find('#valor').parent().find("input[type='text']").rem
            }

            $(e.form).find('#campid').parent().find("input[type='text']").attr("disabled", "disabled");
            $(e.form).find('#indice').parent().find("input[type='text']").attr("disabled", "disabled");
            $(e.form).find('#documento').parent().find("input[type='text']").attr("disabled", "disabled");
            $(e.form).find('#tipocampo').parent().find("input[type='text']").attr("disabled", "disabled");
            $(e.form).find('#NombreCampo').parent().find("input[type='text']").attr("disabled", "disabled");

        }

        function llenarListasPadreHijo(campoCiudad, valor) {

            //se obtiene el CodId
            var departamento = document.getElementById(campoDpto).value;

            if (departamento == "-1") {
                var CampoCiudad = document.getElementById(campoCiudad);
                CampoCiudad.options.length = 0;
            }
            else {
                //SE AGREGA FUNCION QUE OBTIENE EL CODIGO PADRE 
                var codPadre;
                transact.ajaxPOST("/CapturaInformacion/obtieneCodPadre?_campId=" + valor, null,
                    function (data) {
                        codPadre = data[0];
                        var campoDpto = data[1];
                        if (data != null) {
                            console.log(codPadre);

                            //var listas = obtenerListasPadre(departamento);
                            var listas = obtenerListasPadre(codPadre);
                            //var listasIndices = obtenerListasIndicePadre(departamento);
                            var listasIndices = obtenerListasIndicePadre(codPadre);

                            var arrListasCiudades = listas.split(",");
                            var arrListasIndices = listasIndices.split(",");
                            var totalValores = arrListasIndices.length;

                            var CampoCiudad = document.getElementById(campoCiudad);
                            CampoCiudad.options.length = 0;

                            // Modificacion realizada para que las listas obligatorias no queden en seleccione William Eduardo Cicua
                            //var optSel = new Option("Seleccione...", "-1");
                            var optSel = new Option("Seleccione...", "");
                            CampoCiudad.add(optSel);
                            for (var i = 0; i < totalValores; i++) {
                                var opt = new Option(arrListasCiudades[i], arrListasIndices[i]);
                                try {
                                    CampoCiudad.add(opt);
                                } catch (e) {
                                    CampoCiudad.add(opt);
                                }
                            }
                        }
                    },
                    function (error) { console.log(error); }
                    );
            }
        }

        function getClientDataModel(e) {
            var columna = $(e).parent();
            var fila = $(columna).parent();
            var campid = fila.children("#campid").text();
            return campid;
        }

        var negociovisor = "";

        var neg = '../../../Content/ArchivosCliente/<%=Session["_Negocio"]%>/<%=Session["_Negocio"]%>.pdf';

        function onComplete(e) {
            $(".t-button").removeAttr('href');
            var h = $("#MyGrid").outerWidth();

        }

        function inicioPDF() {

            document.getElementById('Iframe1').contentWindow.scaleInicial('page-width');
            document.getElementById('Iframe1').contentWindow.GotoPages('<%=ViewData["NumPagina"]%>');
        }

        function RutaNegocio() {
            if (negociovisor != '0') {

                return neg;
            }
            else {
                return '';
            }
        }

        $(document).ready(function () {
            dimencionesFullScreen();
            setTimeout("inicioPDF()", 2000);
        });

        function dimencionesFullScreen() {
            var browserWidth = $(window).width(); //document.documentElement.clientWidth;
            var browserHeight = $(window).height(); //document.documentElement.clientHeight;
            document.getElementById("zoom").style.width = browserWidth;
            document.getElementById("zoom").style.height = browserHeight;
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



        function GuardarEnviar() {

            transact.ajaxPOST("/EdicionIncidentesCaptura/GuardarEnviar?", null,
                function (data) {

                    if (data == 1) {

                        bootbox.dialog({
                            message: "<h4>El envió de captura se reinicio</h4>",
                            title: "Confirmación",
                            buttons: {
                                succes: {
                                    label: ".: Aceptar :.",
                                    className: "btn-success",
                                    callback: function () {
                                        window.location.href = "/EdicionIncidentesCaptura/index";
                                    }
                                },

                            }
                        });
                    } else {

                        bootbox.dialog({
                            message: "<h4>No fue posible reiniciar el envió de captura</h4>",
                            title: "Confirmación",
                            buttons: {
                                succes: {
                                    label: ".: Aceptar :.",
                                    className: "btn-success",
                                    callback: function () {
                                        window.location.href = "/EdicionIncidentesCaptura/EditarCaptura";
                                    }
                                },

                            }
                        });
                    }

                }, function (error) { console.log(error) });
        }

        function buscarNroBizagi() {
            var nroBizagi = document.getElementById("txtNroBizagi").value;

            if (nroBizagi == "") {
                Alternar(divError);
                return false;
            }
            transact.ajaxPOST("/EdicionIncidentesCaptura/BuscarNroBizagi?nroBizagi=" + nroBizagi, null,
                function (data) {
                    var snExt = parseInt(data[0]);

                    if (snExt == 1) {
                        refrescaGrilla();
                    }
                    else if (data[0] == 0) {
                        $('#stro1').empty();
                        $('#stro1').append(data[1]);
                        Alternar(negocioVacio);
                    }
                    else {
                        $('#stro1').empty();
                        $('#stro1').append("Error desconocido");
                        Alternar(negocioVacio);
                    }
                }, function (error) { console.log(error) });

        }

        function refrescaGrilla() {
            $("#TelEdicionIncidentes.t-grid .t-refresh").trigger('click');
        }

        function Alternar(Seccion) {
            if (Seccion.style.display == "none") {
                Seccion.style.display = ""
                window.setTimeout(function () {
                    $(".alert-danger").fadeTo(300, 0).slideUp(300, function () {
                        $(this).removeAttr("style");
                        $(this).css("display", "none");
                    });
                }, 3500);
            }
        }

    </script>
</asp:Content>
