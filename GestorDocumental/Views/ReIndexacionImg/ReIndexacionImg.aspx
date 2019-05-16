<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Reindexación de Imágenes
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>

    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <style type="text/css">
        #splitterContainer {
            /* Main splitter element */
            height: 90%;
            width: 100%;
            margin: 0;
            padding: 0;
        }

        #leftPane {
            float: left;
            width: 25%;
            height: 100%;
            border-top: solid 1px #9AAE04;
            background: #9AAE04;
            overflow: auto;
        }

        #rightPane {
            /*Contains toolbar and horizontal splitter*/
            float: right;
            width: 51% !important;
            height: 100%;
        }

        .splitbarV {
            height: 550px;
        }

        #contentDoc, #contentPagIndexada {
            margin-top: 15px;
            margin-left: 15px;
        }

            #contentDoc th, #contentPagIndexada th {
                text-align: center;
                width: 0.5%;
                background-color: #1994A4;
                border: solid 1px;
                color: White;
            }

            #contentDoc td, #contentDoc input, #contentPagIndexada td {
                text-align: center;
            }

        #contentPaginacion {
            margin-left: 20px;
            margin-bottom: 15px;
        }

            #contentPaginacion td {
                text-align: right;
            }

        #contentPagIndexada {
            box-shadow: 0px 0px 5px 3px rgba(154, 174, 4, 0.47);
            margin-top: 15px;
        }

        #contentDoc input {
            margin-left: 10px;
        }

        legend.scheduler-border {
            font-size: x-large !important;
        }

        .t-header, .t-grid-header {
            background-color: #6785C1 !important;
            height: 30px !important;
            text-align: center !important;
            color: white;
            font-weight: bold;
        }

        #txtPagina {
            border-radius: 5px;
            background-color: #EDEDED !important;
            text-align: center !important;
        }

        #txtDocumento {
            border-radius: 5px;
            text-align: center !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        var tiff0;
        var neg = "";
        var NumPaginas;


        function buscarNegocio() {
            var _NegId = $("#txtNegocio").val();

            if (_NegId == "") {
                $('#stro1').empty();
                $('#stro1').append("DEBE INGRESAR UN NEGOCIO PARA REALIZAR EL PROCESO DE INDEXACIÓN MANUAL.");
                Alternar(negocioVacio);
                $('#stro1').html();
            }
            else {

                transact.ajaxPOST("/ReIndexacionImg/BuscarNegocio?_NegId=" + _NegId, null,
                    function (data) {

                        var snExt = parseInt(data[0]);
                        neg = data[1];
                        NumPaginas = data[2];
                        console.log(data[4]);
                        if (snExt == 1) {
                            if (data[4] == 1) {

                                $('#stro1').empty();
                                $('#stro1').append("NO SE PUEDE REALIZAR LA INDEXACION MANUAL DEL NEGOCIO DEBIDO A QUE LA ETAPA DE CAPTURA SE ENCUENTRA FINALIZADA");
                                Alternar(negocioVacio);
                                $('#stro1').html();

                            } else if (data[4] == 2) {
                                $('#stro1').empty();
                                $('#stro1').append("NO EXISTE LA ETAPA DE CAPTURA PARA EL NEGOCIO QUE INTENTA REINDEXAR");
                                Alternar(negocioVacio);
                                $('#stro1').html();

                            } else if (data[4] == 0) {

                                $("#Iframe1").attr('src', '../../ViewsAspx/PagePDF/web/PDFViewer.aspx');
                                neg = '../../' + data[1];
                                console.log(NumPaginas);

                                //cargar nro de pagina en campo Página
                                $("#txtPagina").attr("value", NumPaginas);

                                //activar boton que ingresa indexacion
                                if (data[3] == 1) {
                                    //carga archivo
                                    RutaNegocio();
                                    setTimeout("inicioPDF(" + NumPaginas + ")", 1000);

                                    //activa campo Documento
                                    //$('#txtDocumento').focus();
                                    $("#txtDocumento").removeAttr('disabled')

                                    //activa boton INGRESAR
                                    $("#Guardar").removeAttr('disabled')
                                    $("#Guardar").css('display', 'block')
                                    $("#Finalizar").hide()
                                    LimpiarCampos()

                                } else if (data[3] == 2) {
                                    //carga archivo
                                    var NumPag = NumPaginas - 1;
                                    RutaNegocio();
                                    setTimeout("inicioPDF(" + NumPag + ")", 1000);

                                    $("#txtDocumento").attr("value", "")
                                    $("#txtDocumento").attr("disabled", "disabled")
                                    //activa boton FINALIZAR
                                    $("#Finalizar").removeAttr('disabled')
                                    $("#Finalizar").css('display', 'block')
                                    $("#Guardar").hide()
                                }

                                //carga grilla con la tipologia Documental
                                refrescaGrillaTipologia();

                                //cargar grilla de los documentos indexados
                                refrescaGrillaIndexados();
                            }
                        }
                        else if (snExt == 0) {
                            $('#stro1').empty();
                            $('#stro1').append("NO EXISTE EL NEGOCIO CONSULTADO.");
                            Alternar(negocioVacio);
                        }
                    }, function (error) { console.log(error) }
                );
            }
        }

        function inicioPDF(numeroPaginas) {

            document.getElementById('Iframe1').contentWindow.scaleInicial('page-width');
            document.getElementById('Iframe1').contentWindow.GotoPages(numeroPaginas);
            
        }

        //function CargarPaginaDigitada(numeroPaginas) {
        //    document.getElementById('Iframe1').contentWindow.GotoPages(numeroPaginas);
        //}

        //function atif_OnPageChange() {
        //    $('#txtDocumento').focusin();
        //    $('#txtPagina').val(document.getElementById('Iframe1').contentWindow.actualPage());
        //    $('#txtPagina2').val(document.getElementById('Iframe1').contentWindow.actualPage());
        //}

        function AgregaIndexacion() {
            var _negId = $("#txtNegocio").val();
            var _nroDocumento = $("#txtDocumento").val();
            var _nroPagina = $("#txtPagina").val();

            transact.ajaxPOST("/ReIndexacionImg/AgregaIndexacion?_nroDocumento=" + _nroDocumento + "&_nroPagina=" + _nroPagina + "&_negId=" + _negId, null,
                function (data) {
                    if (data[0] != "") {
                        $('#stro2').empty();
                        $('#stro2').append(data[0]);
                        Alternar(divError);
                        $('#stro2').html();
                    }
                },
                function (error) { console.log(error) });

            //refrescaGrillaIndexados()
            LimpiarCampos()
            buscarNegocio()
        }

        function FinIndexacion() {

            transact.ajaxPOST("/ReIndexacionImg/FinIndexacion", null,
                function (data) {
                    if (data[0] != "") {
                        buscarNegocio()
                    } else if (data[0] == "") {
                        bootbox.dialog({
                            message: "<h4>Proceso de Indexación Manual terminado</h4>",
                            title: "Confirmación",
                            buttons: {
                                danger: {
                                    label: ".: Aceptar :.",
                                    className: "btn-danger",
                                    callback: function () {
                                        alerta();
                                        $('#txtNegocio').val('');
                                        $('#txtPagina').val('');
                                        LimpiarCampos();
                                    }
                                }
                            }
                        });
                    }
                },
                function (error) { console.log(error); }
                );
        }

        function LimpiarCampos() {
            $('#txtDocumento').val('');
            $('#txtDocumento').focus();
        }

        function refrescaGrillaTipologia() {
            $("#GridConsulta.t-grid .t-refresh").trigger('click');
        }

        function refrescaGrillaIndexados() {
            $("#GridDocumentos.t-grid .t-refresh").trigger('click');
        }

        function alerta() {
            location.href = "/ReIndexacionImg/ReIndexacionImg";
        }

        function alertaError(Seccion) {
            var txtObservaciones = $("#txtDocumento").val();
            if (txtObservaciones == null) {
                Alternar(Seccion);
            }
        }

        function removeAtrib() {
            $("#txtDocumento").removeAttr('required');
        }

        //funcion que valida que en un campo solo se ingresen numeros
        function esNumero(e) {
            tecla = (document.all) ? e.keyCode : e.which;
            //Tecla de retroceso para borrar, siempre la permite 
            if (tecla == 8 || tecla == 0) {
                return true;
            }
            // Patron de entrada, en este caso solo acepta numeros 
            patron = /[0-9]/;
            tecla_final = String.fromCharCode(tecla);
            return patron.test(tecla_final);
        }


        //funcion que pinta las alertas y les asigna tiempo para desaparecer 
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

        //INICIO: FUNCIONES PARA EL VISOR PDF (CREADAS POR CARLOS TORRES)
        function RutaNegocio() {
            return neg;
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
        //FIN: FUNCIONES PARA EL VISOR PDF 

    </script>
    <br />
    <br />
    <h2>Indexación Manual</h2>

    <div class="row-fluid" class="scheduler-border">
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Negocio: 
                <input type='text' required='required' id='txtNegocio' name='txtNegocio' value='' style='width: 100px; height: 30px' onkeypress='return esNumero(event)' autofocus />
                <input type="button" id="btn_BuscarNeg" name="btn_BuscarNeg" value="..." class="btn btn-login" onclick="buscarNegocio()" />
            </legend>

            <%--<div style="font-weight: bold; color: red; text-align: center">
                DEBE INGRESAR UN NEGOCIO PARA REINDEXAR!
            </div>--%>
            <div class="alert alert-danger" id="negocioVacio" style="display: none">
                <strong id="stro1"></strong>
            </div>

            <div class="row">
                <div class="col-lg-7" id="VisorPDF">
                    <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 700px" id="Iframe1" allowfullscreen></iframe>
                </div>
                <div class="col-lg-5">
                    <div class="alert alert-danger-1" id="divError" style="display: none">
                        <strong id="stro2"></strong>
                    </div>
                    <div class="row">
                        <div class="col-lg-2" style="font-weight: bold; color: red; text-align: center">
                            <%= Session["MensajeError_"] %>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="row">
                                <div class="col-lg-3">
                                    Pagina:
                                </div>
                                <div class="col-lg-3">
                                    <input type="text" id="txtPagina" <%= ViewData["ROtxtPagina"] %> name="txtPagina" readonly="True" style="width: 100px;" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-lg-3">
                                    Documento:
                                </div>
                                <div class="col-lg-4">
                                    <input type="text" <%= ViewData["_disableCampDoc"] %> required="required" id="txtDocumento" <%= ViewData["ROtxtDocumento"] %> name="txtDocumento" value="" style="width: 100px;" onkeypress="return esNumero(event)" />
                                </div>
                                <div class="col-lg-5">
                                    <input type="button" id="Guardar" name="Guardar" value="Ingresar" disabled='disabled' class="btn btn-login" onclick="AgregaIndexacion()" />
                                    <input type="button" id="Finalizar" name="Finalizar" value="Finalizar" disabled='disabled' class="btn btn-login" onclick="FinIndexacion()" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-12" style="height: 300px; overflow-y: auto">
                            <%= Html.Telerik().Grid<GestorDocumental.Controllers._grillalstResultDocs>()
                                .Name("GridConsulta")
                                .DataBinding(databinding => databinding.Ajax().Select("_consultarDocumentos", "ReIndexacionImg", new { _negId = "0"}))
                                .Columns(colums =>
                                {
                                    colums.Bound(o => o.docIdMasc).Width(20).Title("Id");
                                    colums.Bound(o => o.docDescripcion).Width(180).Title("Documento");
                                })
                                .DataBinding(d => d.Ajax().Select("_consultarDocumentos", "ReIndexacionImg"))
                                .Sortable()
                                .ToHtmlString()
                            %>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <div id="Div3" style="height: 300px; overflow-y: auto; margin-top: 30px;">
                                <%= Html.Telerik().Grid<GestorDocumental.Controllers._grillalstDocsIndexados>()
                                    .Name("GridDocumentos")
                                    .ClientEvents( c => c.OnCommand("buscarNegocio"))
                                    .DataBinding(databinding => databinding.Ajax().Select("_consultarPaginasIndexadas", "ReIndexacionImg"))
                                    .Columns(colums =>
                                    {
                                        colums.Bound(o => o.idMasc).Width(10).Title("Id").HeaderHtmlAttributes(new { style = "height: 30px; width: 10px" });
                                        colums.Bound(o => o.documento).Width(280).Title("Documento");
                                        colums.Bound(o => o.pagina).Width(30).Title("Pagina");
                                        colums.Bound(o => o.negId).Visible(false).Title("negId");
                                        colums.Command(o => o.Custom("Eliminar").Text("Eliminar")
                                            .SendState(true)
                                            .DataRouteValues(route =>
                                            {
                                                route.Add(x => x.id).RouteKey("idDocumento");
                                                route.Add(x => x.pagina).RouteKey("numPagina");
                                                route.Add(x => x.negId).RouteKey("negId");
                                            })
                                            .Ajax(true)
                                            .Action("BorrarDocumento", "ReIndexacionImg"))
                                            .Width(70);
                                    })
                                    .Pageable(paginas => paginas.PageSize(5))
                                %>
                            </div>
                        </div>
                    </div>
                    &nbsp;                    
                </div>
            </div>

        </fieldset>
    </div>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
</asp:Content>
