<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:Session["TITULO"] %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <meta http-equiv="expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="-1">

    <script src="../../Scripts/jquery-3.1.1.js"></script>
    <script src="../../Scripts/jquery-3.1.1.min.js"></script>

    <%--<script src="../../Scripts/jquery-1.7.min.js"></script>--%>
    <%--<script src="../../Content/Scripts/jquery-1.4.1.min.js"></script>--%>
    <%--<script src="../../Content/Scripts/jquery-1.4.3.min.js" type="text/javascript"></script>--%>
    <%--<script src="../../Content/Scripts/jquery-1.6.4.min.js"  type="text/javascript"></script>--%>
    <%--<script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>--%>
    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />

    <%--<script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
    <script src="../../Scripts/splitter.js" type="text/javascript"></script>--%>

    <script type="text/javascript" src="../../Scripts/FuncionesValidacionCliente.js"></script>
    <script type="text/javascript" src="../../Scripts/jqBootstrapValidation.js"></script>
    <link rel="stylesheet" href="../../Content/Styles/estilos.css" type="text/css" />

    <!--Esto lo hizo elena--->
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
    <script src="../../Scripts/Reestricciones.js" type="text/javascript"></script>
    <link href="../../Content/Styles/comboboxUI.css" rel="stylesheet" type="text/css" />

    <!--Esto lo hize yo--->

    <script src="../../Scripts/Listas.js" type="text/javascript"></script>
    <script src="../../Scripts/ListasCiudades.js"></script>
    <script src="../../Scripts/ListasPadre.js"></script>


    <script src="../../Scripts/jquery.maskedinput.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.validate.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../../Styles/jquery.ui.datepicker.css" />
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>

    <script src="../../Styles/BootsTrap/js/bootbox.js" type="text/javascript"></script>

    <%--librerias transact ajax --%>
    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <%-- ====================== --%>

    <!--Menu Contextual --->
    <script src="../../Scripts/MenuContext/jquery.contextmenu.js"></script>
    <link href="../../Scripts/MenuContext/jquery.contextmenu.css" rel="stylesheet" />

    <style type="text/css" media="all">
        #MySplitter {
            height: 550px;
            width: 100%;
        }

        #TopPane {
            /*overflow: auto;*/
            height: 100px;
            min-height: 250px;
            max-height: 100%; /*Valor maximo de expansion de la ventana del Tiff*/
        }

        #BottomPane {
            overflow: auto;
        }

        #MySplitter .hsplitbar {
            height: 9px;
            background: #6785C1 no-repeat center;
        }

            #MySplitter .hsplitbar.active, #MySplitter .hsplitbar:hover {
                background: #9AAE04 no-repeat center;
            }

        .TablaFormulario {
            border-collapse: separate;
            border-spacing: 5px;
        }

        .scrollCampos {
            width: auto;
            height: 150px;
            overflow-y: scroll;
        }


        .modal {
            background-color: rgba(0,0,0,8);
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            display: block;
        }
    </style>
    <style type="text/css" media="all">
        input[readonly="readonly"] {
            background-color: #dbdbdb;
        }

        table[data-work="idGrilla"] {
            width: 100%;
        }

            table[data-work="idGrilla"] input {
                width: 75%;
                height: 1.5em;
                border-radius: 5px;
                border: solid 1px #999;
                padding: 2px 3px 2px 3px;
                margin-bottom: 15px;
                font-size: 1em;
                font-family: inherit;
                letter-spacing: 0.2ex;
            }

        table[data-work="idGrillaControlCaldImp"] {
            width: 100%;
        }

            table[data-work="idGrillaControlCaldImp"] input {
                width: 75%;
                height: 1.5em;
                border-radius: 5px;
                border: solid 1px #999;
                padding: 2px 3px 2px 3px;
                margin-bottom: 15px;
                font-size: 1em;
                font-family: inherit;
                letter-spacing: 0.2ex;
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
        //$().ready(function () {
        //    $("#MySplitter").splitter({
        //        type: "h",
        //        sizeTop: true, /* use height set in stylesheet */
        //        accessKey: "P"
        //    });
        //});
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            jsShowWindowLoad('');
            dimencionesFullScreen();
            setTimeout("inicioPDF()", 2000);
            document.oncontextmenu = new Function("return false");
            setTimeout("jsRemoveWindowLoad()", 3000);
        });
        
        window.onbeforeunload = jsShowWindowLoad('');

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


        $(document).contextmenu = false;
        //$(document).end
        var classFechas = "";
        var Obligatoriedad = 1;

        var menu = [ <%=ViewData["_MenuContextual"]%>];

        function infoAdicional(valorFiltro, idCampo) {
            obtenerJson(valorFiltro, idCampo);
        }

        function obtenerJson(_valorfiltro, _idCampo) {
            var data;
            var sendInfo = {
                Valor: data
            };

            $.ajax({
                type: "POST",
                url: "/CapturaInformacion/consultarValores?parametro=" + _valorfiltro,
                dataType: "json",
                success: function (dato) {
                    $('.' + _idCampo).html(dato.data);
                },
                dato: sendInfo
            });
        }

        function CopiarValor(campoCopiado, campoCopiar) {
            var campo = "#" + campoCopiado;
            var valorCampo = $(campo).val();

            var campo_Copia = "#" + campoCopiar;
            $(campo_Copia).val(valorCampo);
        }

        //=========================================================================================================
        //JFPancho;05Mayo2017; se validacion que solo permite la entrada de minúsculas, mayúsculas y Ññ, "-"
        function validaTexto(idCampo) {
            console.log("validaTexto " + idCampo);

            $('#' + idCampo).on('input', function (e) {
                if (!/^[ a-zñ\-]*$/i.test(this.value)) {
                    this.value = this.value.replace(/[^ a-zñ\-]+/ig, "");
                }
            });
        }
        //=========================================================================================================

        //Funcion Listas de Departamentos y Ciudades.
        function llenarListaHijo(campoCiudad, valor) {
            var departamento = document.getElementById(valor).value;
            if (departamento == "-1") {
                var CampoCiudad = document.getElementById(campoCiudad);
                CampoCiudad.options.length = 0;
            }
            else {
                var listas = obtenerListasCiudades(departamento);
                var listasIndices = obtenerListasIndiceCiudades(departamento);

                var arrListasCiudades = listas.split(",");
                var arrListasIndices = listasIndices.split(",");
                var totalValores = arrListasIndices.length;

                var CampoCiudad = document.getElementById(campoCiudad);
                CampoCiudad.options.length = 0;

                var optSel = new Option("Seleccione...", "-1");
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
        }

        //se valida que la fecha ingresada no sea menor a la actual
        function validarFechaMenorActual(date) {
            var x = new Date();
            var fecha = date.split("/");
            x.setFullYear(fecha[2], fecha[1] - 1, fecha[0]);
            var today = new Date();

            if (x > today)
                return false;
            else
                return true;
        }

        //agrega Camilo Padilla; junio-2016
        //funcion que valida que la fecha ingresada no sea menor a la minima parametrizada
        function validarFechaMenor(date, valor) {

            var arra1 = date.split("/");
            var arra2 = valor.split("/");
            var date2 = new Date(arra1[2], arra1[1], arra1[0]);
            var valor2 = new Date(arra2[2], arra2[1], arra2[0]);

            //console.log(date2);

            if (valor2 < date2)
                return false;
            else
                return true;
        }

        /************************************************************************************************************************************/
        //inicio Validacion de fecha Ivan Rodriguez

        function validarFechaCaptura(idCampo, AñoMinimo, AñoMaximo, simboloFechamin, simboloFechaMax) {

            var elementoFoco = document.activeElement.valueOf();
            var valor = $("#" + idCampo).val();
            var error = "Fecha invalida";
            var ilegible = 0;
            if (valor == "-111" || valor == "-999") {
                ilegible = 1;
            }
            else if (valor == '') {
                //$("#" + idCampo).focus();
                //$("#" + idCampo).val('-111');
                //var elemento = document.getElementById(idCampo);
                //elemento.blur();
                //imprimirError(error, idCampo);
            }
            else {
                var f = new Date();
                var dia = f.getDate().toString();
                var yearMin = Number(f.getFullYear());
                var yearMax = Number(f.getFullYear());

                if (simboloFechamin == "menorQue") {
                    AñoMinimo = yearMin - Number(AñoMinimo);
                } else {
                    AñoMinimo = yearMin + Number(AñoMinimo);
                }
                if (simboloFechaMax == "menorQue") {
                    AñoMaximo = yearMax - Number(AñoMaximo);
                } else {
                    AñoMaximo = yearMax + Number(AñoMaximo);
                }
                var fechaMen = dia.concat("/", f.getMonth() + 1, "/", AñoMinimo);
                var fechaMay = dia.concat("/", f.getMonth() + 1, "/", AñoMaximo);
                error = "La fecha no coincide con el formato(DD/MM/AAAA) o no se encuentra en el intervalo (" + fechaMen + " , " + fechaMay + ")";
                if (!validaFechaDDMMAAAA(valor, AñoMinimo, AñoMaximo)) {
                    imprimirError(error, idCampo, ilegible);
                    $("#" + idCampo).removeAttr("disabled");
                } 
            }
        }


        //funcion que valida el formato de la fecha y si esta entre el intervalo del AñoMinimo y AñoMaximo
        function validaFechaDDMMAAAA(fecha, AñoMinimo, AñoMaximo) {
            var dtCh = "/";
            var minYear = AñoMinimo;
            var maxYear = AñoMaximo;
            function isInteger(s) {
                var i;
                for (i = 0; i < s.length; i++) {
                    var c = s.charAt(i);
                    if (((c < "0") || (c > "9"))) return false;
                }
                return true;
            }
            function stripCharsInBag(s, bag) {
                var i;
                var returnString = "";
                for (i = 0; i < s.length; i++) {
                    var c = s.charAt(i);
                    if (bag.indexOf(c) == -1) returnString += c;
                }
                return returnString;
            }
            function daysInFebruary(year) {
                return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
            }
            function DaysArray(n) {
                for (var i = 1; i <= n; i++) {
                    this[i] = 31
                    if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
                    if (i == 2) { this[i] = 29 }
                }
                return this
            }
            function isDate(dtStr) {
                var daysInMonth = DaysArray(12)
                var pos1 = dtStr.indexOf(dtCh)
                var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
                var strDay = dtStr.substring(0, pos1)
                var strMonth = dtStr.substring(pos1 + 1, pos2)
                var strYear = dtStr.substring(pos2 + 1)
                strYr = strYear
                if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
                if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
                for (var i = 1; i <= 3; i++) {
                    if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
                }
                month = parseInt(strMonth)
                day = parseInt(strDay)
                year = parseInt(strYr)
                if (pos1 == -1 || pos2 == -1) {
                    return false
                }
                if (strMonth.length < 1 || month < 1 || month > 12) {
                    return false
                }
                if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
                    return false
                }
                if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
                    return false
                }
                if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
                    return false
                }
                if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) {          
                    return false;
                }
                if (mes == 2) { // bisiesto
                    var bisiesto = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
                    if (day > 29 || (day == 29 && !bisiesto)) {
                        return false;
                    }
                }
                return true
            }
            if (isDate(fecha)) {
                var f = new Date();
                var dia = f.getDate().toString();
                var fechaMen = dia.concat("/", f.getMonth() + 1, "/", AñoMinimo);
                var fechaMay = dia.concat("/", f.getMonth() + 1, "/", AñoMaximo);
                if (diferenciafechasEndias(fechaMen, fecha) >= 0) {
                    if (diferenciafechasEndias(fecha, fechaMay) >= 0) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
                return true;
            } else {
                return false;
            }
        }

        //fa la fecha nmenos y fb la fecha mayor
        function diferenciafechasEndias(fa, fb) {

            var aFecha1 = fa.split('/');
            var aFecha2 = fb.split('/');


            //Siempre f2 > f1
            var fFecha1 = new Date(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
            var fFecha2 = new Date(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);

            var dif = fFecha2.getTime() - fFecha1.getTime();
            var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
            return dias;

        }

        function imprimirError(Error, idCampo, ilegible) {

            bootbox.dialog({
                message: "" + Error,
                title: "<b>Error</b>",
                buttons: {
                }
            });

            var readonly = document.getElementById(idCampo).getAttribute("readonly");

            if (readonly == "readonly") {

            } else {
                $("#" + idCampo).val("");
            }
        }

        //========================================================================================
        //JFP: se agregan funciones para campos dependientes; mayo/2016
        function activaCampDependientes(idCampo) {
            var array = ["lst_", "txt_", "chk_"];


            //se obtiene el valor seleccionado de la lista padre
            for (var x = 0; x < array.length; x++) {
                var idListaPadre = array[x] + idCampo;
                if ($("#" + idListaPadre).length > 0) {
                    if (array[x] == "lst_") {
                        var opcionSeleccionada = $("#" + idListaPadre + " option:selected").val();
                    }
                    if (array[x] == "chk_") {
                        if ($("#" + idListaPadre + ":checked").val() == "on") {
                            var opcionSeleccionada = 1;
                        } else {
                            var opcionSeleccionada = 0;
                        }
                    }
                }
            }

            transact.ajaxPOST("/CapturaInformacion/validaParamCamposDep?_CampId=" + idCampo + "&_opcionLst=" + opcionSeleccionada, null,
                function (result) {
                    transact.ajaxPOST("/CapturaInformacion/lstCamposDep?_campId=" + idCampo, null,
                        function (data) {
                            for (var i = 0; i < data.length; i++) {
                                for (var x = 0; x < array.length; x++) {
                                    var idCamp = array[x] + data[i];
                                    if ($("#" + idCamp).length > 0) {
                                        if (result == 1) {
                                            $("#" + idCamp).removeAttr("disabled");
                                        } else if (result == 0) {
                                            $("#" + idCamp).attr('disabled', 'disabled');
                                        }


                                    }
                                }
                            }
                        }, function (error) { console.log(error); }
                        );
                },
                function (error) { console.log(error); }

                );
        }
        //FIN Funcion JFPancho ====================================================================


        //Funcion llenar Listas Padre e hijo
        //campoCiudad = nombre de la lista de ciudades
        //campoDpto = nombre de la lista de dptos
        //valor = CampID
        //function llenarListasPadreHijo(campoCiudad, valor) {
        function llenarListasPadreHijo(campoCiudad, campoDpto, valor) {
            //$.blockUI();
            //se obtiene el CodId
            var departamento = document.getElementById(campoDpto).value;

            if (departamento == "-1") {
                var CampoCiudad = document.getElementById(campoCiudad);
                CampoCiudad.options.length = 0;
            }
            else {
                //SE AGREGA FUNCION QUE OBTIENE EL CODIGO PADRE 
                var codPadre;
                transact.ajaxPOST("/CapturaInformacion/obtieneCodPadre?_codId=" + departamento + "&_campId=" + valor, null,
                    function (data) {
                        codPadre = data;
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

        function eliminarUsuario(obj) {
            var oTr = obj;
            while (oTr.nodeName.toLowerCase() != 'tr') {
                oTr = oTr.parentNode;
            }

            var root = oTr.parentNode;
            root.removeChild(oTr);
        }

        //var tiff0;
        var pag;
        var neg = '../../../Content/ArchivosCliente/<%=ViewData["_Negocio"]%>/<%=ViewData["_Negocio"]%>.pdf';


        function inicioPDF() {
            //'page-fit'
            //'auto'
            //'page-width'
            document.getElementById('Iframe1').contentWindow.scaleInicial('page-width');
            document.getElementById('Iframe1').contentWindow.GotoPages('<%=ViewData["NumPagina"]%>');
        }

        //**************************************************************************************

        function formReset() {
            document.getElementById("form1").reset();
        }

        //Esta funcion se creo para poder asignar el evento onfocus al checkbox pues este no se puede asignar uno a uno desde el lenguaje
        function CoordenadasCheckbox(objeto) {
            var a = $(objeto).attr('value');
            var arr = a.split('_');
            var Posx = arr[1];
            var PosY = arr[2];
            scrollVisor(Posx, PosY);
        }
        //***************************************************************************************
        //Funcion movimiento de cordenadas dentro del visor (Carlos Torres) 
        var Frames = window.frames;
        function scrollVisor(Posx, PosY) {
            var pag = Frames[0].document.getElementById("pageContainer<%=ViewData["NumPagina"]%>");
            document.getElementById('Iframe1').contentWindow.cordenadasCaptura(pag, Posx, PosY);
        }
        //***************************************************************************************

        jQuery.validator.setDefaults({
            debug: true,
            succes: "valid"
        });

        $(".valFecha").validate({
            rules: {
                field: {
                    required: true,
                    date: true
                }
            }
        });

        function pageLoad() {
            //$('.cmenu1').contextMenu(menu, { theme: 'gloss,gloss-cyan', shadow: true });
            $(".dp").datepicker({
                maxDate: "+0D",
                defaulDate: "+1w",
                changeMounth: true,
                numberOfMonths: 1,
                dateFormat: "dd/mm/yy"
            });
            //$(".msk").mask("99/99/9999");
            $(".dec").blur(function () {
                if ($.isNumeric(this.value)) {
                    if (this.value.length < 1) {
                        var amt2 = 0;
                        $(this).val(amt2.toFixed(2));
                    }
                    else {
                        var amt = parseFloat(this.value);
                        $(this).val(amt.toFixed(2));
                    }
                } else {
                    $(this).val('');
                }
            });
        }

        //Para que solo ingrese numeros
        function numbersonly(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode != 8 && unicode != 45 && unicode != 9) {
                if (unicode < 43 || unicode > 57) //if not a number
                { return false } //disable key press    
            }
        }

        //Para que solo ingrese letras
        function soloLetras(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
            especiales = [8, 37, 39, 46];

            tecla_especial = false
            for (var i in especiales) {
                if (key == especiales[i]) {
                    tecla_especial = true;
                    break;
                }
            }

            if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                return false;
            }
        }

        //Funcion para bloquear todo el teclado (Para los campos que tienen calendarios)
        function bloqTeclado(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode != 8) {
                if (unicode > 1) //if not a number
                { return false } //disable key press    
            }
        }

        //Funcion que se agrega a la funcion numbersOnly para la creacion de un campo decimal.
        $(function () {
            //$('.cmenu1').contextMenu(menu, { theme: 'gloss,gloss-cyan', shadow: true });
            $(".dec").blur(function () {
                if ($.isNumeric(this.value)) {
                    if (this.value.length < 1) {
                        var amt2 = 0;
                        $(this).val(amt2.toFixed(2));
                    }
                    else {
                        var amt = parseFloat(this.value);
                        $(this).val(amt.toFixed(2));
                    }
                } else {
                    $(this).val('');
                }
            });
        });

        //Funcion para eliminar una fila de la grilla
        function eliminarFila(obj) {
            var oTr = obj;
            while (oTr.nodeName.toLowerCase() != 'tr') {
                oTr = oTr.parentNode;
            }
            var root = oTr.parentNode;
            root.removeChild(oTr);
        }

        var datos = null;
        var datosIndices = null;
        var posicionCampo = 2;
        var posCampo = 1;

        function resetearIndice() {
            posCampo = 1;
        }

        function validarFecha(campo) {
            var fechaValidar = campo.value;
            var esValida = /^(0[1-9]|[12]\d|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$/.test(fechaValidar);
            if (!esValida && fechaValidar != '__/__/____') {
                alert('Fecha invalida');
                campo.value = "";
                campo.focus();
            }
        }

        function asignarValorCheck(id) {
            if (document.getElementById("ck" + id).checked) {
                document.getElementById("CapturaTres" + id).value = 1;
            }
            else {
                document.getElementById("CapturaTres" + id).value = 0;
            }
        }

        //asigna los caracteres / a los campos tipo fecha 

        function validarFormatoFecha(idCampo) {

            var valor = $("#" + idCampo).val();
            if (valor != -111 && valor != -999) {

                var caracteres = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "0"];
                var letras = valor.split("");
                var caracMalo = ""
                valor = "";
                if (letras.length >= 10)
                    letras.splice(10)
                if (letras.length >= 2)
                    letras[2] = "/";
                if (letras.length >= 5)
                    letras[5] = "/";
                for (var i = 0 ; i < letras.length ; i++) {
                    if (caracteres.indexOf(letras[i]) == -1) {
                        if (i == 2 && letras[i].indexOf("/") != -1)
                            valor += letras[i];
                        else if (i == 5 && letras[i].indexOf("/") != -1)
                            valor += letras[i];
                        else
                            caracMalo = 1;

                    } else {
                        valor += letras[i];
                    }
                }
                $("#" + idCampo).val(valor);
            }

        }

        function ValidarMinCampo(idCampo, MinLongitud, campId) {

            infoAdicional($("#" + idCampo).val(), "_lbl" + campId);

            var valor = $("#" + idCampo).val();
            var error = "El minimo de caracteres para estecampo es: " + MinLongitud;
            if (valor.length > 0 && valor.length < MinLongitud) {
                imprimirError(error, idCampo, "0");
            }
        }



        function agregarFila(totalCamposPintar, nombreCampo, tipoCampo, nombreTabla, disponibilidad, listaCampos, maxLongitud, campoObligatorio, operadorMingr, operadorMaxgr, fechaMinimagr, fechaMaximagr, CordenadaX, CordenadaY, MinLongitud) {
            
            nuevaFila = document.getElementById(nombreTabla).insertRow(-1);
            nuevaFila.id = posicionCampo;
            nuevaCelda = nuevaFila.insertCell(-1);

            var existeUno = 0;
            //para maximo minimo y operador de validacion fecha de campo 
            if (fechaMinimagr.indexOf(',') != -1) {
                var arrOperadorMingr = operadorMingr.split(",");
                var arrOperadorMaxgr = operadorMaxgr.split(",");
                var arrFechaMinimagr = fechaMinimagr.split(",");
                var arrFechaMaximagr = fechaMaximagr.split(",");

            } else existeUno = 1;

            //para minimo y operador de validacion de campos numericos 
            var existeUnoMinCampo = 0;
            var arrMinLongitud = 0;
            if (MinLongitud.indexOf(',') != -1) {
                arrMinLongitud = MinLongitud.split(",");


            } else {
                existeUnoMinCampo = 1;
                arrMinLongitud = MinLongitud;
            }

            //Para asignar el tipo de campo a crear
            var arrTipoCampos = tipoCampo.split(",");
            var tipoCampo;

            //Para asignarle el nombre  a los campos
            var arrCampos = nombreCampo.split(",");
            var campo;

            //Para saber que campo esta disponible y que no en la captura
            var arrDisponibilidad = disponibilidad.split(",");
            var disp;

            //Para los id de los campos y las listas despeglables
            var arrDatosListas = listaCampos.split(",");
            var datListas;

            //Para el maximo longitud
            var arrMaximaLongitud = maxLongitud.split(",");
            var datMaxLongitud;

            //Para el campo Obligatorio
            var arrCamposObligatorios = campoObligatorio.split(",");
            var datCampObligatorio;

            //Para las cordenadas
            var arrCordenadasX = CordenadaX.split(",");
            var datCordenadasX;

            var arrCordenadasY = CordenadaY.split(",");
            var datCordenadasY

            nuevaCelda.innerHTML = "<td><input id=\"indice\" name=\"indicePos\" value='" + posCampo + "' type=\"hidden\" style = \"text-decoration: underline; font-weight: bold;width: 0px;\" /></td>";
            nuevaCelda = nuevaFila.insertCell(-1);

            for (var i = 0; i <= totalCamposPintar - 1; i++) {
                tipoCampo = arrTipoCampos[i];
                campo = arrCampos[i];
                disp = arrDisponibilidad[i];
                datListas = arrDatosListas[i];
                datMaxLongitud = arrMaximaLongitud[i];
                datCampObligatorio = arrCamposObligatorios[i];
                CorX = arrCordenadasX[i];
                CorY = arrCordenadasY[i];
                javascriptFocus = "";

                if (CorX != null && CorY != null) {

                    javascriptFocus = " onfocus=\"scrollVisor(" + CorX + "," + CorY + ")\"";

                }

                switch (tipoCampo) {
                    case "1":

                        var campIdMinimo;
                        var MinimoCampo = 0;
                        if (existeUnoMinCampo == 1) {
                            MinimoCampo = arrMinLongitud.split("_")[1];
                        } else {
                            for (var j = 0 ; j < arrMinLongitud.length ; j++) {
                                var campIdMin;
                                campIdMinimo = arrMinLongitud[j].split("_");
                                campIdMin = "A" + campIdMinimo[0]
                                if (campIdMin == campo) {
                                    MinimoCampo = campIdMinimo[1];
                                    cont = j;
                                }
                            }
                        }

                        if (disp == "1") {
                            if (datCampObligatorio == "True") {
                                nuevaCelda.innerHTML = "<td><input  align=\"center\" style=\"width: 200px; margin-right: 10px;margin-left: 10px;\"" + javascriptFocus + " onkeypress=\"return numbersonly(event);\" maxlength=" + datMaxLongitud + " required type=\"text\" name='" + campo + "' id='" + campo + posicionCampo + "' onKeyUp =validarCaracteres('" + campo + posicionCampo + "')  onblur =ValidarMinCampo('" + campo + posicionCampo + "','" + MinimoCampo + "','" + MinimoCampo + "')  onmousedown = ilegibleFaltante('" + campo + posicionCampo + "',event) ></td>";
                                nuevaCelda = nuevaFila.insertCell(-1);
                            }
                            else {
                                nuevaCelda.innerHTML = "<td><input  align=\"center\" style=\"width: 200px; margin-right: 10px;margin-left: 10px;\"" + javascriptFocus + " onkeypress=\"return numbersonly(event);\" maxlength=" + datMaxLongitud + " type=\"text\" name='" + campo + "' id='" + campo + posicionCampo + "' onKeyUp =validararacteres('" + campo + posicionCampo + "') onblur =ValidarMinCampo('" + campo + posicionCampo + "','" + MinimoCampo + "','" + MinimoCampo + "') onmousedown = ilegibleFaltante('" + campo + posicionCampo + "',event) ></td>";
                                nuevaCelda = nuevaFila.insertCell(-1);
                            }
                        }
                        else {
                            nuevaCelda.innerHTML = "<td><input readonly=\"readonly\" value=\"\" onkeypress=\"return numbersonly(event);\" type=\"text\" name='" + campo + "' id='" + campo + "' ></td>";
                            nuevaCelda = nuevaFila.insertCell(-1);
                        }
                        break;

                    case "2":
                        if (disp == "1") {

                            if (datCampObligatorio == "True") {
                                nuevaCelda.innerHTML = "<td><input   align=\"center\" style=\"width: 200px; margin-right: 10px;margin-left: 10px;\" type=\"text\" " + javascriptFocus + "  required maxlength=" + datMaxLongitud + " name='" + campo + "' id='" + campo + posicionCampo + "' onmousedown = ilegibleFaltante('" + campo + posicionCampo + "',event) ></td>";
                                nuevaCelda = nuevaFila.insertCell(-1);
                            }
                            else {
                                nuevaCelda.innerHTML = "<td><input   align=\"center\" style=\"width: 200px; margin-right: 10px;margin-left: 10px;\" type=\"text\" " + javascriptFocus + " maxlength=" + datMaxLongitud + " name='" + campo + "' id='" + campo + posicionCampo + "'  onmousedown = ilegibleFaltante('" + campo + posicionCampo + "',event)></td>";
                                nuevaCelda = nuevaFila.insertCell(-1);
                            }
                        }
                        else {
                            nuevaCelda.innerHTML = "<td><input readonly=\"readonly\" value=\" \" type=\"text\" name='" + campo + "' id='" + campo + "' ></td>";
                            nuevaCelda = nuevaFila.insertCell(-1);
                        }
                        break;

                        // William modificacion realizada para que se validen las fechas
                        //case "3":
                        //    if (disp == "1") {
                        //        if (datCampObligatorio == "True") {
                        //            nuevaCelda.innerHTML = "<td><input class=\"dp msk cmenu1\" onblur=\"validarFecha(this)\" required type=\"text\" name='" + campo + "' ></td>";
                        //            nuevaCelda = nuevaFila.insertCell(-1);
                        //        }
                        //        else {
                        //            nuevaCelda.innerHTML = "<td><input class=\"dp msk cmenu1\" onblur=\"validarFecha(this)\" type=\"text\" name='" + campo + "' ></td>";
                        //            nuevaCelda = nuevaFila.insertCell(-1);
                        //        }
                        //    }
                        //    else {
                        //        nuevaCelda.innerHTML = "<td><input readonly=\"readonly\" value=\" \" type=\"text\" name='" + campo + "' ></td>";
                        //        nuevaCelda = nuevaFila.insertCell(-1);
                        //    }
                        //    break;
                    case "3":
                        if (disp == "1") {
                            //Ivan Rodriguez
                            //codigo para la validacion de fechas 
                            var cont;
                            var campIdfec;
                            var fechaMin;
                            var fechaMax;
                            var opMin;
                            var opMax;
                            if (existeUno == 1) {
                                fechaMax = fechaMaximagr;
                                opMin = operadorMingr;
                                opMax = operadorMaxgr;
                                fechaMin = fechaMinimagr.split("_")[1];
                            } else {
                                for (var j = 0 ; j < arrFechaMinimagr.length ; j++) {
                                    var campIdfecha;
                                    campIdfec = arrFechaMinimagr[j].split("_");
                                    campIdfecha = "A" + campIdfec[0]
                                    if (campIdfecha == campo) {
                                        fechaMin = campIdfec[1];
                                        cont = j;
                                    }
                                }
                                fechaMax = arrFechaMaximagr[cont];
                                opMin = arrOperadorMingr[cont];
                                opMax = arrOperadorMaxgr[cont];
                            }


                            if (datCampObligatorio == "True") {
                                nuevaCelda.innerHTML = "<td><input class=\"dp msk \"  align=\"center\" " + javascriptFocus + " style=\"width: 200px; margin-right: 10px;margin-left: 10px;\" onKeyUp=\"validarFormatoFecha('" + campo + posicionCampo + "')\" onblur=\"validarFechaCaptura('" + campo + posicionCampo + "','" + fechaMin + "','" + fechaMax + "','" + opMin + "','" + opMax + "')\" required type=\"text\" name='" + campo + "' id='" + campo + posicionCampo + "' onmousedown = ilegibleFaltante('" + campo + posicionCampo + "',event) ></td>";
                                nuevaCelda = nuevaFila.insertCell(-1);
                            }
                            else {
                                nuevaCelda.innerHTML = "<td><input class=\"dp msk \"  align=\"center\" " + javascriptFocus + " style=\"width: 200px; margin-right: 10px;margin-left: 10px;\"  onKeyUp=\"validarFormatoFecha('" + campo + posicionCampo + "')\" onblur=\"validarFechaCaptura('" + campo + posicionCampo + "','" + fechaMin + "','" + fechaMax + "','" + opMin + "','" + opMax + "')\" type=\"text\" name='" + campo + "' id='" + campo + posicionCampo + "' onmousedown = ilegibleFaltante('" + campo + posicionCampo + "',event) ></td>";
                                nuevaCelda = nuevaFila.insertCell(-1);
                            }
                        }
                        else {
                            nuevaCelda.innerHTML = "<td><input readonly=\"readonly\" value=\" \" type=\"text\" name='" + campo + "' id='" + campo + "'></td>";
                            nuevaCelda = nuevaFila.insertCell(-1);
                        }
                        break;

                    case "16":
                        if (disp == "1") {
                            if (datCampObligatorio == "True") {
                                nuevaCelda.innerHTML = "<td><input   align=\"center\" " + javascriptFocus + " style=\"width: 200px; margin-right: 10px;margin-left: 10px;\" maxlength=" + datMaxLongitud + " required onkeypress=\"return numbersonly(event);\" type=\"text\" name='" + campo + "' onchange=\"decimal(txt_" + campo + posicionCampo + ")\" onKeyUp=\"validarCaracteres('txt_" + campo + posicionCampo + "')\" onmousedown = ilegibleFaltante('txt_" + campo + posicionCampo + "',event)  id =\"txt_" + campo + posicionCampo + "\" ></td>";
                                nuevaCelda = nuevaFila.insertCell(-1);
                            }
                            else {
                                nuevaCelda.innerHTML = "<td><input   align=\"center\" " + javascriptFocus + " style=\"width: 200px; margin-right: 10px;margin-left: 10px;\" maxlength=" + datMaxLongitud + " onkeypress=\"return numbersonly(event);\" type=\"text\" name='" + campo + "'  onchange=\"decimal(txt_" + campo + posicionCampo + ")\" onKeyUp=\"validarCaracteres('txt_" + campo + posicionCampo + "')\" onmousedown = ilegibleFaltante('txt_" + campo + posicionCampo + "',event)  id =\"txt_" + campo + posicionCampo + "\"></td>";
                                nuevaCelda = nuevaFila.insertCell(-1);
                            }
                        }
                        else {
                            nuevaCelda.innerHTML = "<td><input readonly=\"readonly\" value=\" \" type=\"text\" name='" + campo + "'  ></td>";
                            nuevaCelda = nuevaFila.insertCell(-1);
                        }
                        break;

                    case "11":
                        if (disp == "1") {
                            if (datCampObligatorio == "True") {
                                nuevaCelda.innerHTML = "<td><input name='" + campo + "' style=\"width: 200px; margin-right: 10px;margin-left: 10px;\" type=\"checkbox\"   align=\"center\" " + javascriptFocus + " value=\"1\"  required></td>";
                                nuevaCelda = nuevaFila.insertCell(-1);
                            }
                            else {
                                nuevaCelda.innerHTML = "<td><input name='" + campo + "' style=\"width: 200px; margin-right: 10px;margin-left: 10px;\" type=\"checkbox\"  align=\"center\" " + javascriptFocus + " value=\"1\" ></td>";
                                nuevaCelda = nuevaFila.insertCell(-1);
                            }
                        }
                        else {
                            nuevaCelda.innerHTML = "<td><input readonly=\"readonly\" value=\" \" type=\"text\" name='" + campo + "' ></td>";
                            nuevaCelda = nuevaFila.insertCell(-1);
                        }
                        break;

                    case "5":
                        datos = obtenerListas(datListas);
                        datosIndices = obtenerListasIndice(datListas);

                        if (disp == "1") {

                            var arrDatosObtenidos = datos.split(",");
                            var arrDatosIndicesObtenidos = datosIndices.split(",");
                            var totalValores = arrDatosObtenidos.length;

                            var lista = "<td><select style=\"width: 200px; margin-right: 10px;margin-left: 10px;margin-bottom: 16px;\" name ='" + campo + "' " + javascriptFocus + ">";

                            //var lista = "<td><select style=\"width: 100px;\" name ='" + campo + "' " + javascriptFocus + ">";
                            var opciones = "";

                            for (var a = 0; a < totalValores; a++) {
                                var opcionDato = arrDatosObtenidos[a];
                                var opcionIndice = arrDatosIndicesObtenidos[a];
                                if (opciones == "") {
                                    opciones = "<option value=" + opcionIndice + ">" + opcionDato + "</option>";
                                }
                                else {
                                    opciones = opciones + "<option value=" + opcionIndice + ">" + opcionDato + "</option>";
                                }
                            }

                            lista = lista + opciones + "</select></td>";
                            nuevaCelda.innerHTML = lista;
                            nuevaCelda = nuevaFila.insertCell(-1);
                        }
                        else {
                            nuevaCelda.innerHTML = "<td><input  readonly=\"readonly\" value=\"---No disponible---\" type=\"text\" name='" + campo + "' ></td>";
                            nuevaCelda = nuevaFila.insertCell(-1);
                        }
                        break;
                }
            }

            nuevaCelda.innerHTML = "<td><img style=\"margin-right: 10px;margin-left: 10px;margin-bottom: 16px;\" src='../Images/Delete.png' title=\"Eliminar Registro\" onclick='eliminarFila(this)' onmouseover=\"this.style.cursor='pointer'\"/></td>";
            posicionCampo++;
            posCampo++;

            //$('.cmenu1').contextMenu(menu, { theme: 'gloss,gloss-cyan', shadow: true });
            $(".dec").blur(function () {

                if (this.value.length < 1) {
                    var amt2 = 0;
                    $(this).val(amt2.toFixed(2));
                    if ($.isNumeric(this.value)) {
                    }
                    else {
                        var amt = parseFloat(this.value);
                        $(this).val(amt.toFixed(2));
                    }
                } else {
                    $(this).val('');
                }

            });

            /*$(".dp").datepicker({
                maxDate: '+0D',
                defaulDate: '+1w',
                changeMounth: true,
                numberOfMonths: 1,
                dateFormat: 'dd/mm/yy'
            });*/
            //$(".msk").mask("99/99/9999");

            
        }

        function addTableRow(table) {
            // clonar la ultima fila de la tabla
            var _tabla = document.getElementById(table);
            var $tr = $(_tabla).find("tbody tr:last").clone();

            $tr.find("td").each(function () {

                var idcampoinput = $(this).children().attr("id");
                var onblur = $(this).children().attr("onblur");
                var onkeyup = $(this).children().attr("onkeyup");
                var onchange = $(this).children().attr("onchange");
                var onclick = $(this).children().attr("onclick");
                var onkeypress = $(this).children().attr("onkeypress");
                if (idcampoinput != "indice" || idcampoinput != "indice") {
                    if (!(typeof (idcampoinput) === "undefined")) {

                        var lengt = idcampoinput.length - 1;
                        var idNuevoCam = idcampoinput.substring(0, lengt);
                        idNuevoCam = idNuevoCam + posicionCampo;
                        $(this).children().attr("id", idNuevoCam);
                        if (!(typeof (onblur) === "undefined")) {
                            onblur = onblur.replace(idcampoinput, idNuevoCam);
                            $(this).children().attr("onblur", onblur);
                        }
                        if (!(typeof (onkeyup) === "undefined")) {
                            onkeyup = onkeyup.replace(idcampoinput, idNuevoCam);
                            $(this).children().attr("onkeyup", onkeyup);
                        }
                        if (!(typeof (onchange) === "undefined")) {
                            onchange = onchange.replace(idcampoinput, idNuevoCam);
                            $(this).children().attr("onchange", onchange);
                        }
                        if (!(typeof (onclick) === "undefined")) {
                            onclick = onclick.replace(idcampoinput, idNuevoCam);
                            $(this).children().attr("onclick", onclick);
                        }
                        if (!(typeof (onkeypress) === "undefined")) {
                            onclick = onkeypress.replace(idcampoinput, idNuevoCam);
                            $(this).children().attr("onkeypress", onkeypress);
                        }
                    }
                }
                
            });
            posicionCampo = posicionCampo + 1;

            // añadir la nueva fila a la tabla
            $(_tabla).find("tbody tr:last").after($tr);

            $(".dec").blur(function () {
                if ($.isNumeric(this.value)) {
                    if (this.value.length < 1) {
                        var amt2 = 0;
                        $(this).val(amt2.toFixed(2));
                    }
                    else {
                        var amt = parseFloat(this.value);
                        $(this).val(amt.toFixed(2));
                    }
                } else {
                    $(this).val('');
                }
            });

            $('.cmenu1').contextMenu(menu, { theme: 'gloss,gloss-cyan', shadow: true });
            $(".dp").datepicker({
                maxDate: "+0D",
                defaulDate: "+1w",
                changeMounth: true,
                numberOfMonths: 1,
                dateFormat: "dd/mm/yy"
            });
            //$(".msk").mask("99/99/9999");
            $(".dp").remo
        }


        //INICIO: Funciones Para el visor PDF (creadas por Carlos Torres)
        function RutaNegocio() {
            if ('<%=ViewData["_Negocio"]%>' != '') {
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
        function anular(e) {
            tecla = (document.all) ? e.keyCode : e.which;
            return (tecla != 13);
        }
        //FIN: Funciones Para el visor PDF 


        //Funcion para ilegible y faltante


        function ilegibleFaltante(idCampo, event) {

            $("#" + idCampo).contextmenu = false;
            document.oncontextmenu = function () { return false }
            if (event.button == 2) {
                var valor = $("#" + idCampo).val();
                if (valor == '-111') {
                    $("#" + idCampo).val('');
                    $("#" + idCampo).removeAttr('readonly');
                    //$('.dp').datepicker({ maxDate: '+0D', defaulDate: '+1w', changeMounth: true, numberOfMonths: 1, dateFormat: 'dd/mm/yy' });
                    $('.dpms').datepicker({ maxDate: '+0D', defaulDate: '+1w', changeMounth: true, numberOfMonths: 1, dateFormat: 'dd/mm/yy' });

                } else if (valor == '-999') {
                    $("#" + idCampo).val('');
                    $("#" + idCampo).removeAttr('readonly');
                    //$('.dp').datepicker({ maxDate: '+0D', defaulDate: '+1w', changeMounth: true, numberOfMonths: 1, dateFormat: 'dd/mm/yy' });
                    $('.dpms').datepicker({ maxDate: '+0D', defaulDate: '+1w', changeMounth: true, numberOfMonths: 1, dateFormat: 'dd/mm/yy' });
                } else {
                    bootbox.dialog({
                        message: "<h4>Seleccione la incidencia del campo</h4>",
                        title: "Campo Ilegible o faltante",
                        buttons: {
                            succes: {
                                label: ".: Faltante :.",
                                className: "btn-success",
                                callback: function () {
                                    $("#" + idCampo).val("-999");
                                    $("#" + idCampo).attr('readonly', true);
                                    //$(this).datepicker('destroy');
                                }
                            }, danger: {
                                label: ":: Ilegible ::",
                                className: "btn-danger",
                                callback: function () {
                                    $("#" + idCampo).val("-111");
                                    $("#" + idCampo).attr('readonly', true);
                                    //$(this).datepicker('destroy');
                                }
                            }
                        }
                    });
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%=@Html.Raw(ViewBag.pageLoad)%>
    <br />
    <div class="row-fluid">
        <div class="span4">
            <h3><%:Session["TITULO"] %></h3>
        </div>

        <div class="row">
            <div class="col-lg-9">
                <h3><%=ViewData["Negocio"]%></h3>
            </div>
            <%--<div class="col-lg-2" style="margin: 5px 0px 0px 5px">
                <button class="btn btn-login" style='width: 180px; height: 34px;' id="btn_BuscarNeg" name="btn_BuscarNeg" onclick="quitarObligatoriedad()">
                    <span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span>&nbsp; Quitar Obligatoriedad
                </button>
            </div>--%>
        </div>
    </div>

    <div id="zoom" style="overflow: scroll;" hidden="hidden" onmousemove="moverScrolls(event)"></div>
    <br />
    <br />
    <div id="MySplitter">
        <div id="TopPane">
            <iframe src="../../ViewsAspx/PagePDF/web/PDFViewer.aspx" style="width: 100%; height: 100%" id="Iframe1" allowfullscreen></iframe>
        </div>

        <div id="BottomPane2">
            <%-- Modifica William Cicua; Junio-2016 --%>
            <%--<form action="/CapturaInformacion/Guardar" method="post">--%>

            <form action="/CapturaInformacion/Guardar" method="post" onsubmit="check()" id="formulario" onkeypress="return anular(event)">
                <h5>
                    <%=ViewData["noNegocios"]%></h5>
                <div style="background-color: #6785C1; color: white;">
                    <table>
                        <tr>
                            <td style="width: 150px; font-weight: bold; font-size: 18px;" align="center">Documento:
                            </td>
                            <td style="width: 300px; font-size: 18px;" align="left">
                                <%=ViewData["_NomDocumento"]%>
                            </td>
                            <td style="width: 150px; font-weight: bold; font-size: 18px;">Grupo:
                            </td>
                            <td style="font-size: 18px;" align="left">
                                <%=ViewData["_NomGrupo"]%><br />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="row-fluid scrollCampos">
                    <%=ViewData["CamposGenerados"]%>
                </div>
                <br />
                <input type="hidden" value="<%=ViewData["NumPagina"]%>" name="NumPagina" id="NumPagina" />
                <input type="submit" name="btnGuardar" id="btnGuardar" value="Guardar" class="btn btn-login" style="width: auto" />
                <input type="reset" name="btnRestablecer" id="btnRestablecer" value="Restablecer" class="btn btn-login" style="width: auto" />
            </form>
        </div>
    </div>
    <script type="text/javascript">
        //$(document).ready(function () {
        //    $('[data-toggle="popover"]').popover();
        //});

        function quitarObligatoriedad() {

            bootbox.dialog({
                message: "<h4>¿Está seguro que desea quitar la obligatoriedad a todos los campos?</h4>",
                title: "Confirmación",
                buttons: {
                    succes: {
                        label: ".: Aceptar :.",
                        className: "btn-success",
                        callback: function () {
                            transact.ajaxPOST("/CapturaInformacion/marcarNegocioDevolucion", null,
                                function (data) {

                                    var snExt = parseInt(data[0]);
                                    if (snExt == 1) {

                                        if (Obligatoriedad == 1) {
                                            var elementos = document.getElementById("formulario");
                                            for (var i = 0; i < elementos.length; i++) {
                                                var campo = elementos.elements[i];
                                                var tipocampo = campo.type;
                                                if (tipocampo == "text" || tipocampo == "select-one" || tipocampo == "checkbox") {
                                                    $(elementos[i]).removeAttr("required");
                                                } else if (tipocampo == "input") {
                                                    $(elementos[i]).removeAttr("required");
                                                }
                                            }
                                            $('.TablaFormulario tr').each(function () {
                                                $(this).find("td").eq(0).removeAttr("style");
                                                $(this).find("td").eq(0).attr("style", "font-weight:normal");
                                            });
                                            Obligatoriedad = 0;
                                        } else {
                                            location.reload(true);
                                        }
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

                                },
                                function (error) { console.log(error); });
                        }
                    }, danger: {
                        label: ":: Cancelar ::",
                        className: "btn-danger",
                        callback: function () {

                        }
                    }
                }
            });
        }

        (function () {
            //Fecha Formulario
            $(".dpms").datepicker({
                maxDate: "+0D",
                defaulDate: "+1w",
                changeMounth: true,
                numberOfMonths: 1,
                dateFormat: "dd/mm/yy"
            });

            //$.mask.definitions['~'] = "[+-]";
            //$(".dpms").mask("99/99/9999", {});
            /*$(".dpms").blur(function () {
                $("#info").html("Unmasked value: " + $(this).mask());
            }).dblclick(function () {
                $(this).unmask();
            });*/

            //$(".dpms").blur(function () {
            //    validaFechaDDMMAAAA(this.value, this.id);
            //});

            //$(".dpms").attr('data-toggle', 'popover');
            //$(".dpms").attr('data-placement', 'bottom');
            //$(".dpms").attr('data-content', 'Fecha no valida');
            //$(".dpms").attr('popover-trigger', 'focus');

            //HoraFormulario
            $.mask.definitions['~'] = "[+-]";
            $(".hpms").mask("99:99", {});
            $(".hpms").blur(function () {
                $("#info").html("Unmasked value: " + $(this).mask());
            }).dblclick(function () {
                $(this).unmask();
            });

        })();

        //crea funcion William Cicua; Junio-2016 
        function check() {
            jsShowWindowLoad('');
            formulario = document.getElementById("formulario");
            for (var i = 0; i < formulario.elements.length; i++) {
                var elemento = formulario.elements[i];
                if (elemento.type == "checkbox") {
                    if (!elemento.checked) {
                        document.getElementById("" + elemento.id).value = 0
                        //$("#" + elemento.id).val() = "0"
                    } else {
                        document.getElementById("" + elemento.id).value = 1
                        //$("#" + elemento.id).val() = "1";
                    }
                }
            }
            return true;
        }
        //crea funcion William Cicua; Julio-2016 
        function InsertarCheck(name, value) {
            transact.ajaxPOST("/CapturaInformacion/GuardarCheck?causal=" + name + "&valor=" + value, null,
                function (data) { },
                function (error) { Console.log(error) });
        }
        // William Eduardo Cicua Ruiz
        function UnCheckObligatorio(data) {
            var campo = data;
            transact.ajaxPOST("/CapturaInformacion/Checks?CampId=" + campo, null,
               function (campos) {
                   var a = 0;
                   for (var i = 0; i < campos.length; i++) {
                       //document.getElementById("chk_" + campos[0]).removeAttribute('disabled')
                       if (document.getElementById("chk_" + campos[i]).checked == true) {
                           a = 1;
                       }
                   }
                   if (a == 1) {
                       for (var i = 0; i < campos.length; i++) {
                           document.getElementById("chk_" + campos[i]).removeAttribute("required");
                       }
                   } else {
                       for (var i = 0; i < campos.length; i++) {
                           document.getElementById("chk_" + campos[i]).setAttribute("required", null);
                       }
                   }
               },
               function (error) { console.log(error); });
        }

        function decimal(id) {

            var temp = id.id;
            var separador = "." // separador para los miles
            var sepDecimal = ',' // separador para los decimales
            var num = document.getElementById(temp + "").value
            num += '';
            var splitStr = num.split('.');
            var splitLeft = splitStr[0];
            var splitRight = ""//splitStr.length & gt; 1 ? this.sepDecimal + splitStr[1] : '';
            var regx = /(\d+)(\d{3})/;
            while (regx.test(splitLeft)) {
                splitLeft = splitLeft.replace(regx, '$1' + separador + '$2');
            }
            var dec = splitLeft + splitRight;
            document.getElementById(temp + "").value = dec

        }

        function validarCaracteres(idCampo) {

            var valor = $("#" + idCampo).val();
            var caracteres = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "0", ".", "-"];
            var letras = valor.split("");
            var caracMalo = ""
            valor = "";
            for (var i = 0 ; i < letras.length ; i++) {
                if (caracteres.indexOf(letras[i]) == -1) {
                    caracMalo = 1;
                } else {
                    valor += letras[i];
                }
            }
            if (caracMalo != "") {
                $("#" + idCampo).val(valor);
            }
        }




        function imprimirErrorCampo(idCampo, Error) {
            bootbox.dialog({
                message: "" + Error,
                title: "<b>Error</b>",
                buttons: {
                    danger: {
                        label: ":: Aceptar ::",
                        className: "btn-danger",
                        callback: function () {

                            //$("#" + idCampo).val("");
                        }
                    }
                }
            });
            document.getElementById(idCampo).focus();

        }


    </script>
</asp:Content>
