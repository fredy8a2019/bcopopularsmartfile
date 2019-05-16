<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Indexación de Imágenes
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <meta http-equiv="expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="-1">

    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />

    <%--<script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/BootsTrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>--%>
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

        /*.t-header, .t-grid-header {
            background-color: #6785C1 !important;
            height: 30px !important;
            text-align: center !important;
            color: white;
            font-weight: bold;
        }*/

        .t-header {
            background-color: #6785C1;
            color: white;
            height: 30px;
        }

            .t-header .t-link {
                color: white;
                height: 100%;
                width: 100%;
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

        .modal {
            background-color: rgba(0,0,0,.8);
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            display: block;
        }

        #botones {
            display: none;
        }

        @media(max-width:700px) {
            #botones {
                display: block;
            }

            #VisorPDF {
                display: none;
            }

            #Div1 {
                display: none;
            }
        }

        @media(max-width:600px) {

            #ColEstado {
                display: none;
            }
        }

        .modal-backdrop {
            z-index: 0;
        }
		#WindowLoad {
            position: fixed;
            top: 0px;
            left: 0px;
            z-index: 3200;
            filter: alpha(opacity=65);
            -moz-opacity: 65;
            opacity: 0.65;
            background: #999;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            jsShowWindowLoad('');
            dimencionesFullScreen();
            setTimeout("inicioPDF()", 1500);
            setTimeout("jsRemoveWindowLoad()", 3000);
        });

        var tiff0;
        var neg;
        //var NumPaginas;
		
        function jsRemoveWindowLoad() {
            // eliminamos el div que bloquea pantalla
            $("#WindowLoad").remove();

        }

        function jsShowWindowLoad(mensaje) {
            //eliminamos si existe un div ya bloqueando
            jsRemoveWindowLoad();

            //si no enviamos mensaje se pondra este por defecto
            if (mensaje === undefined) mensaje = "Procesando la información<br>Espere por favor";

            //centrar imagen gif
            height = 20;//El div del titulo, para que se vea mas arriba (H)
            var ancho = 0;
            var alto = 0;

            //obtenemos el ancho y alto de la ventana de nuestro navegador, compatible con todos los navegadores
            if (window.innerWidth == undefined) ancho = window.screen.width;
            else ancho = window.innerWidth;
            if (window.innerHeight == undefined) alto = window.screen.height;
            else alto = window.innerHeight;

            //operación necesaria para centrar el div que muestra el mensaje
            var heightdivsito = alto / 2 - parseInt(height) / 2;//Se utiliza en el margen superior, para centrar

            //imagen que aparece mientras nuestro div es mostrado y da apariencia de cargando
            imgCentro = "<div style='text-align:center;height:" + alto + "px;'><div  style='color:#000;margin-top:" + heightdivsito + "px; font-size:20px;font-weight:bold'></div><img  src='../../Images/Loading2.gif' width='100' height='100' ></div>";

            //creamos el div que bloquea grande------------------------------------------
            div = document.createElement("div");
            div.id = "WindowLoad"
            div.style.width = ancho + "px";
            div.style.height = alto + "px";
            $("body").append(div);

            //creamos un input text para que el foco se plasme en este y el usuario no pueda escribir en nada de atras
            input = document.createElement("input");
            input.id = "focusInput";
            input.type = "text"

            //asignamos el div que bloquea
            $("#WindowLoad").append(input);

            //asignamos el foco y ocultamos el input text
            $("#focusInput").focus();
            $("#focusInput").hide();

            //centramos el div del texto
            $("#WindowLoad").html(imgCentro);

        }

        neg = '../../../Content/ArchivosCliente/<%=Session["_Negocio"]%>/<%=Session["_Negocio"]%>.pdf';

        function inicioPDF() {
            //'page-fit'
            //'auto'
            //'page-width'
            document.getElementById('Iframe1').contentWindow.scaleInicial('page-width');
            document.getElementById('Iframe1').contentWindow.GotoPages('<%= ViewData["_ValorPagina_"] %>');
        }

        function PaginaIndex() {
            return '<%= ViewData["_ValorPagina_"] %>';
        }

        function CargarPaginaDigitada() {
            //var pag = document.getElementById('txtPagina').value;
            //tiff0.GoToPage(pag);
            document.getElementById('Iframe1').contentWindow.GotoPages('<%= ViewData["_ValorPagina_"] %>');
        }

        function atif_OnPageChange() {
            $('#txtDocumento').focusin();
            //$('#txtPagina').val(tiff0.GetCurrentPage().toString());
            //$('#txtPagina2').val(tiff0.GetCurrentPage().toString());
            $('#txtPagina').val(document.getElementById('Iframe1').contentWindow.actualPage());
            $('#txtPagina2').val(document.getElementById('Iframe1').contentWindow.actualPage());
        }

        function alerta() {
            location.href = "/IndexacionImg/Index";
        }

        function alertaError(Seccion) {
            var txtObservaciones = $("#txtDocumento").val();
            if (txtObservaciones == null) {
                Alternar(Seccion);
            }
        }

        function Alternar(Seccion) {
            var id = "";
            id = $("#" + Seccion);
            if (id.css("display") == "none") {
                id.css("display", "block");
                window.setTimeout(function () {
                    $(".alert-danger").fadeTo(500, 0).slideUp(500, function () {
                        $(this).removeAttr("style");
                        $(this).css("display", "none");
                    });
                }, 3000);
            }
        }

        function removeAtrib() {
            $("#txtDocumento").removeAttr('required');
        }

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

        //INICIO: Funciones Para el visor PDF (creadas por Carlos Torres)
        function RutaNegocio() {
            if ('<%=Session["_Negocio"]%>' != '0') {
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
        function FinalizacionEtapaIndexacion() {
            transact.ajaxPOST("/IndexacionImg/FinIndexacion", null,
                function (data) {

                    //si el data retorna 0 indica que no se han indexado todos los documentos
                    if (data[0] != "") {
                        console.log(data[0]);
                        $('#stro2').empty();
                        $('#stro2').append(data[0]);
                        Alternar(divError);
                        $('#stro2').html();
                    } else {

                        //modalConfirmacion("<h4>¿Continuar con la siguiente Indexación?</h4>", "Confirmación", ".: Continuar :.", "/IndexacionImg/Index", ".: Cancelar :.", "../Home/Index");
					   
                       bootbox.dialog({
                           message: "<h4>¿Continuar con la siguiente Indexación?</h4>",
                           title: "Confirmación",
                           buttons: {
                               success: {
                                   label: ".: Continuar :.",
                                   className: "btn-success",
                                   callback: function () {
                                       //window.location.href = '/IndexacionImg/Index';
                                       
                                       location.href = "/IndexacionImg/Index";
                                   }
                               },
                               danger: {
                                   label: ".: Cancelar :.",
                                   className: "btn-danger",
                                   callback: function () {
                                        
                                       window.location.href = "../Home/Index";
                                   }
                               }
                           }
                       });
                   }
               },
                function (error) { console.log(error); }
                );
           }

           function FinIndexacion() {

               transact.ajaxPOST("/IndexacionImg/validarDocumentosFaltantes", null,
                   function (data) {

                       //si el data retorna 0 indica que no se han indexado todos los documentos
                       if (data[0] == "1") {
                           bootbox.dialog({
                               message: "<h4>¿Seguro que desea finalizar la Indexación? los documentos: " + data[1] + " no han sido indexados</h4>",
                               title: "Confirmación",
                               buttons: {
                                   success: {
                                       label: ".: SI :.",
                                       className: "btn-success",
                                       callback: function () {
                                           //window.location.href = '/IndexacionImg/Index';
                                           FinalizacionEtapaIndexacion();
                                       }
                                   },
                                   danger: {
                                       label: ".: Cancelar :.",
                                       className: "btn-danger",
                                       callback: function () {
                                       
                                       }
                                   }
                               }
                           });
                       } else { 
                           FinalizacionEtapaIndexacion();
                       }
                   },
               function (error) { console.log(error) });

           
           }



           function AgregaIndexacion() {
               var _nroDocumento = $("#txtDocumento").val();
               var _nroPagina = $("#txtPagina").val();

               transact.ajaxPOST("/IndexacionImg/AgregaIndexacion?_nroDocumento=" + _nroDocumento + "&_nroPagina=" + _nroPagina, null,
                   function (data) {
                       if (data[0] != "") {
                           //Si data[0] retorna "" inidca error
                           $('#stro2').empty();
                           $('#stro2').append(data[0]);
                           Alternar(divError);
                           $('#stro2').html();
                           // Modificacion 04/05/2016 William Eduardo Cicua
                           //limpia campo y asigna foco
                           document.getElementById("txtDocumento").value = "";
                           document.getElementById("txtDocumento").focus();
                       } else {
                           // Modificacion 04/05/2016 William Eduardo Cicua
                           // asigan siguiente documento, cambia la pagina del visor y asigna foco a txtDocumento
                           //location.href = "/IndexacionImg/Index";

                           //data[1] contien el nro de la pag siguiente
                           document.getElementById("txtPagina").value = data[1];
                           document.getElementById("txtDocumento").value = "";
                           document.getElementById('Iframe1').contentWindow.GotoPages(data[1]);
                           document.getElementById("txtDocumento").focus();

                           if (data[2] == "visible") {
                               document.getElementById("Finalizar").style.visibility = "visible";
                           }
                           if (data[3] == "disabled='disabled'") {
                               document.getElementById("txtDocumento").disabled = true;
                           }
                           if (data[4] == "hidden") {
                               document.getElementById("Guardar").style.visibility = "hidden";
                           }

                           refrescaGrillaIndexados();
                       }
                   },
                   function (error) { console.log(error) });

           }

           function refrescaGrillaTipologia() {
               $("#GridConsulta.t-grid .t-refresh").trigger('click');
           }

           function refrescaGrillaIndexados() {
               $("#GridDocumentos.t-grid .t-refresh").trigger('click');
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

           function verVisor() {
               if ($("#muestraVisor").hasClass("ative")) {
                   //$("#Iframe1").css("display", "none");
                   $("#VisorPDF").hide("fast");
                   $("#muestraVisor").removeClass("ative");

               }
               else {
                   //$("#Iframe1").css("display", "block");

                   $("#VisorPDF").show("fast");
                   $("#muestraVisor").addClass("ative");
               }
           }

           function verDocumentos() {
               if ($("#muestraDocumentos").hasClass("ative")) {
                   //$("#Iframe1").css("display", "none");
                   $("#Div1").hide("fast");
                   $("#muestraDocumentos").removeClass("ative");

               }
               else {
                   //$("#Iframe1").css("display", "block");

                   $("#Div1").show("fast");
                   $("#muestraDocumentos").addClass("ative");
               }
           }

           function onEdit(e) {

               //var tipocampo = $(e.form).find('#tipocampo').parent().find("input[type='text']").val();

               // if (tipocampo == 'Lista desplegable') {
               //     var Campid = $(e.form).find('#valor').parent().find("input[type='text']").val();
               //     //$(e.form).find('#valor').parent().find("input[type='text']").rem
               // } 


               $(e.form).find('#Documento').parent().find("input[type='text']").attr("disabled", "disabled");
               $(e.form).find('#Modulo').parent().find("input[type='text']").attr("disabled", "disabled");
               $(e.form).find('#Fecha').parent().find("input[type='text']").attr("disabled", "disabled");
               $(e.form).find('#tipocampo').parent().find("input[type='text']").attr("disabled", "disabled");
               $(e.form).find('#docid').parent().find("input[type='text']").attr("disabled", "disabled");
           }

           function indexacionMasiva() {
               bootbox.dialog({
                   message: "<h4>¿Seguro que el resto de páginas son Anexos?</h4>",
                   title: "Confirmación",
                   buttons: {
                       success: {
                           label: ".: SI :.",
                           className: "btn-success",
                           callback: function () {
                               //window.location.href = '/IndexacionImg/Index';

                               transact.ajaxPOST("/IndexacionImg/IndexacionMasiva", null, function (data) {
                                   if (data[0] != "") {
                                       console.log(data[0]);
                                       $('#stro2').empty();
                                       $('#stro2').append(data[0]);
                                       Alternar(divError);
                                       $('#stro2').html();
                                   } else {

                                       //data[1] contien el nro de la pag siguiente
                                       document.getElementById("txtPagina").value = data[1];
                                       document.getElementById("txtDocumento").value = "";
                                       document.getElementById('Iframe1').contentWindow.GotoPages(data[1]);
                                       document.getElementById("txtDocumento").focus();

                                       if (data[1] == "visible") {
                                           document.getElementById("Finalizar").style.visibility = "visible";
                                       }
                                       if (data[2] == "disabled='disabled'") {
                                           document.getElementById("txtDocumento").disabled = true;
                                       }
                                       if (data[3] == "hidden") {
                                           document.getElementById("Guardar").style.visibility = "hidden";
                                       }

                                       refrescaGrillaIndexados();
                                   }
                               },
                               function (error) { console.log(error) });
                               location.href = "/IndexacionImg/Index";
                           }
                       },
                       danger: {
                           label: ".: NO :.",
                           className: "btn-danger",
                           callback: function () {
                           }
                       }
                   }
               });

           }

           function borrarIndexacion() {
               bootbox.dialog({
                   message: "<h4>¿Seguro que desea eliminar todas las paginas Indexadas?</h4>",
                   title: "Confirmación",
                   buttons: {
                       success: {
                           label: ".: SI :.",
                           className: "btn-success",
                           callback: function () {
                               transact.ajaxPOST("/IndexacionImg/BorrarPaginasIndexadas", null, function (data) {
                                   if (data[0] != "") {
                                       console.log(data[0]);
                                       $('#stro2').empty();
                                       $('#stro2').append(data[0]);
                                       Alternar(divError);
                                       $('#stro2').html();
                                   } else {
                                       refrescaGrillaIndexados();
                                   }
                               },
                               function (error) { console.log(error) });
                               location.href = "/IndexacionImg/Index";
                           }
                       },
                       danger: {
                           label: ".: NO :.",
                           className: "btn-danger",
                           callback: function () {
                           }
                       }
                   }
               });
           }

           function onComplete(e) {
               var visible = '<%=ViewData["ultimaPagina"]%>';
                if (visible == '1') {
                    document.getElementById("txtPagina").value = pagina;
                    document.getElementById("txtDocumento").value = "";
                    document.getElementById('Iframe1').contentWindow.GotoPages(pagina);
                    document.getElementById("txtDocumento").focus();

                    document.getElementById("Finalizar").style.visibility = "visible";

                    document.getElementById("txtDocumento").disabled = true;

                    document.getElementById("Guardar").style.visibility = "hidden";

                    
                }
                refrescaGrillaIndexados();
            }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row-fluid" <%= ViewData["tamaño"] %>>
        <div id="zoom" style="overflow: scroll;" hidden="hidden" onmousemove="moverScrolls(event)"></div>
        <br />
        <br />
        <br />
        <fieldset class="scheduler-border">

            <legend class="scheduler-border">
                <%:Session["TITULO"] %>        
            </legend>

            <div class="row">
                <div class="col-lg-7" id="VisorPDF">
                    <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 700px" id="Iframe1" allowfullscreen></iframe>
                </div>
                <div class="col-lg-5">
                    <div class="alert alert-danger" id="divError" style="display: none">
                        <strong id="stro2"></strong>
                    </div>
                    <%--<table style="width: 100%;">--%>
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
                                    <input type="text" id="txtPagina" <%= ViewData["ROtxtPagina"] %> name="txtPagina" value="<%= ViewData["_ValorPagina_"] %>" readonly="True" style="width: 100px;" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-lg-3">
                                    Documento:
                                </div>
                                <div class="col-lg-4">
                                    <input type="text" <%= ViewData["_disableCampDoc"] %> required="required" id="txtDocumento" <%= ViewData["ROtxtDocumento"] %> name="txtDocumento" value="<%= ViewData["ValorDocumento"] %>" style="width: 100px;" onkeypress="return esNumero(event)" autofocus />
                                </div>
                                <div class="col-lg-5">
                                    <input type="button" id="Guardar" name="Guardar" value="Ingresar" style="visibility: <%= ViewData["_btnGuardarVisible"] %>;" class="btn btn-login" onclick="AgregaIndexacion()" />
                                    <input type="button" id="Finalizar" name="Finalizar" value="Finalizar" style="visibility: <%= ViewData["_btnFinalizarVisible"] %>;" class="btn btn-login" onclick="FinIndexacion()" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-12" style="height: 300px; overflow-y: auto">
                            <%--<div id="Div1" style="height: 300px; overflow-y: auto">--%>
                            <%= Html.Telerik().Grid<GestorDocumental.Controllers.listaResultadoDocumentos>()
                                    .Name("GridConsulta")
                                    .DataBinding(databinding => databinding.Ajax().Select("_consultarDocumentos", "IndexacionImg"))
                                    .Columns(colums =>
                                        {
                                            colums.Bound(o => o.docIdMasc).Width(20).Title("Id");
                                            colums.Bound(o => o.docDescripcion).Width(180).Title("Documento");
                                        })
                            %>
                            <%--</div>--%>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <div id="Div3" style="height: 300px; overflow-y: auto; margin-top: 30px;">
                                <%--<table style="width: 100%; text-align: center;">
                                        <tr>
                                            <td valign="top" style="margin-top: 15px">--%>
                                <%= Html.Telerik().Grid<GestorDocumental.Controllers.listaDocumentosIndexados>()
                                                        .Name("GridDocumentos")
                                                        .ClientEvents( c => c.OnCommand("alerta")) 
                                                        .DataBinding(databindig => databindig.Ajax().Select("_consultarPaginasIndexadas", "IndexacionImg"))
                                                        .Columns(colums =>
                                                        {
                                                            colums.Bound(o => o.idMasc).Width(10).Title("Id").HeaderHtmlAttributes(new { style = "height: 30px; width: 10px" });
                                                            colums.Bound(o => o.documento).Width(280).Title("Documento");
                                                            colums.Bound(o => o.pagina).Width(30).Title("Pagina");
                                                            colums.Command(o => o.Custom("Eliminar").Text("Eliminar")
                                                                .SendState(true)
                                                                .DataRouteValues(route =>
                                                                {
                                                                    route.Add(x => x.id).RouteKey("idDocumento");
                                                                    route.Add(x => x.pagina).RouteKey("numPagina");
                                                                })
                                                                .Ajax(true)
                                                                .Action("BorrarDocumento", "IndexacionImg"))
                                                                .Width(70);
                                                        }).Pageable(paginas => paginas.PageSize(5))
                                %>
                                <%--</td>
                                        </tr>
                                    </table>--%>
                            </div>
                        </div>
                    </div>
                    <script type="text/javascript">
                        function prueba() {
                            alert("postback");
                        }
                    </script>
                    <%--</table>--%>
                </div>
            </div>

            <div class="row" >
                <div class="col-lg-12" >
                    <input type="button" id="BorrarIndexacion" name="BorrarIndexacion" value="Borrar Indexacion" class="btn btn-login" onclick="borrarIndexacion()">
                </div>
            </div>

            <div class="row" style="visibility: <%= ViewData["_disableDocFaltantes"] %>;">
                <div class="col-lg-12" id="divBotonclaMasiva">
                    <input type="button" id="Clasificacion Masiva" name="Clasificacion_Masiva" value="Clasificacion Masiva" class="btn btn-login" onclick="indexacionMasiva()">
                </div>
            </div>

            <h3 style="margin-top: 20px; visibility: <%= ViewData["_disableDocFaltantes"] %>;">Documentos que han generado devolución</h3>

            <div class="row">
                <div class="col-lg-12" id="tablaDocFaltantes">
                    <div id="Div3" style="width: 100%; overflow-x: scroll; visibility: <%= ViewData["_disableDocFaltantes"] %>;">
                        <%                     
                            GridEditMode mode = (GridEditMode)ViewData["mode"];
                            GridButtonType type = (GridButtonType)ViewData["type"];
                            GridInsertRowPosition insertRowPosition = (GridInsertRowPosition)ViewData["insertRowPosition"];    
                        %>
                        <%= Html.Telerik().StyleSheetRegistrar() 
                                                        .DefaultGroup(group => group.Add("telerik.hay.css")) %>
                        <%= Html.Telerik().Grid<GestorDocumental.Models.sp_Index_obtenerDocfaltantes_Result>()
                                .Name("TablaDocumentosFaltantes")
                        
                                .DataKeys(keys =>
                                {
                                    keys.Add(o => o.docid);
                                })
                                .Sortable()
                                .Localizable("es-ES")
                                .DataBinding(databindig => databindig.Ajax()
                                    .Select("_ConsultarDocumentosFaltantes", "IndexacionImg")
                                    .Update("_GuardarDocumentosFaltantes", "IndexacionImg", new {pagina =  "<#= pagina #>"}))
                                .Columns(colums =>
                                {
                                    colums.Bound(o => o.docid).Width(10).Title("Id Tabla").HtmlAttributes(new { id = "id" });
                                    colums.Bound(o => o.tipocampo).Width(10).Title("Id documento").Visible(true);
                                    colums.Bound(o => o.documento).Width(20).Title("Documento").Visible(true);/*.Visible(false)*/;
                                    colums.Bound(o => o.modulo).Width(20).Title("Modulo").Visible(true);
                                    colums.Bound(o => o.pagina).Width(10).Title("Pagina").Visible(true);
                                    colums.Bound(o => o.negid).Title("Negid").Visible(false);
                                    colums.Bound(o => o.Fecha).Width(20).Title("Fecha").Visible(true);
                                    
                                    colums.Command(commands =>
                                    { commands.Edit().ButtonType(type); }).Width(10);
                                })
                                .Filterable()
                                .ClientEvents(events => events.OnEdit("onEdit"))
                                .ClientEvents(events => events.OnComplete("onComplete"))
                                .Editable(editing => editing
                           
                                .InsertRowPosition(insertRowPosition)
                                .Mode(mode))
                                .Pageable(paginas => paginas.PageSize(20))
                                .Scrollable(scrolling => scrolling.Height(150))
                                .Resizable(resizing => resizing.Columns(true))
                        %>
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
</asp:Content>
