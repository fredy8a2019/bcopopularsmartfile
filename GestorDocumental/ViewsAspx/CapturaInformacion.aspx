<%@ Page Title="" Language="C#" MasterPageFile="Site1.Master" AutoEventWireup="true"
    CodeBehind="CapturaInformacion.aspx.cs" Inherits="GestorDocumental.ViewsAspx.CapturaInformacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Content/Scripts/splitter.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/FuncionesValidacionCliente.js"></script>
    <script type="text/javascript" src="../Scripts/jqBootstrapValidation.js"></script>
    <link rel="stylesheet" href="../Content/Styles/estilos.css" type="text/css" />
    <!--Esto lo hizo elena--->
    <link href="../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../Content/Scripts/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../Content/Scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../Content/Scripts/ui/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script src="../Content/Scripts/ui/i18n/jquery.ui.datepicker-es.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.button.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.menu.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/ui/jquery.ui.tooltip.js" type="text/javascript"></script>
    <link href="../Content/Styles/comboboxUI.css" rel="stylesheet" type="text/css" />
    <!--Esto lo hize yo--->
    <script src="../Scripts/Listas.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.validate.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../Styles/jquery.ui.datepicker.css" />
    <script src="../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function () {            
            CargarDoc();
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#splitterContainer").splitter({
                minAsize: 100,
                maxAsize: 300,
                splitVertical: true,
                A: $('#leftPane'),
                B: $('#rightPane'),
                slave: $("#rightSplitterContainer"),
                closeableto: 0
            });
            $("#rightSplitterContainer").splitter({
                splitHorizontal: true,
                A: $('#rightTopPane'),
                B: $('#rightBottomPane'),
                closeableto: 100
            });

//            _ui.buildCombobox();
//            $("#MainContent_UpdatePanel2 select").combobox();
        });

        function eliminarUsuario(obj) {
            var oTr = obj;
            while (oTr.nodeName.toLowerCase() != 'tr') {
                oTr = oTr.parentNode;
            }

            var root = oTr.parentNode;
            root.removeChild(oTr);
        }
  
    </script>
    <style>
        #MainContent_pControls table[data-work='tablaPrincipal'] input[type='text']
        {
            width: 25% !important;
            height: 1.5em;
            border-radius: 5px;
            border: solid 1px #999 !important;
            padding: 2px 3px 2px 3px;
            margin-bottom: 15px;
            font-size: 1em;
            font-family: inherit;
            letter-spacing: 0.2ex;
        }
        
        #MainContent_pControls table
        {
            width: 100%;
        }
        
        #MainContent_pControlsM
        {
            box-shadow: 0px 0px 5px 3px rgba(154, 174, 4, 0.47);
            margin-top: 15px;
        }
        
        #MainContent_pControls table tr:first-child td:first-child
        {
            width: 20%;
        }
        
        #MainContent_pControls table tr td:first-child
        {
            text-align: inherit;
        }
        
        
        #divControles
        {
            margin-left: 20px;
        }
        
        #MainContent_pControls table[data-work='tablaPrincipalControlCalidad'] input[type="text"]
        {
            width: 90% !important;
            height: 1.5em;
            border-radius: 5px;
            border: solid 1px #999 !important;
            padding: 2px 3px 2px 3px;
            margin-bottom: 15px;
            font-size: 1em;
            font-family: inherit;
            letter-spacing: 0.2ex;
        }
        
        #MainContent_pControls table[data-work="tablaPrincipalControlCalidad"] tr:first-child td:nth-child(2)
        {
            width: 21%;
        }
        
        #MainContent_pControls table[data-work="idGrilla"] tr td:first-child, #MainContent_pControls table[data-work="idGrilla"] tr:first-child td:nth-child(2)
        {
            width: 0%;
        }
        
        #MainContent_pControls table[data-work="idGrilla"] input
        {
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
        
        #MainContent_pControls table[data-work="idGrillaControlCaldImp"] tr td:first-child, #MainContent_pControls table[data-work="idGrillaControlCaldImp"] tr:first-child td:nth-child(2)
        {
            width: 0%;
        }
        
        #MainContent_pControls table[data-work="idGrillaControlCaldImp"] input
        {
            height: 1.5em;
            border-radius: 5px;
            border: solid 1px #999;
            padding: 2px 3px 2px 3px;
            margin-bottom: 15px;
            font-size: 1em;
            font-family: inherit;
            letter-spacing: 0.2ex;
        }
        
        #MainContent_UpdatePanel1
        {
            color: Black;
            text-align:center;
        }
        
        #divControles
        {
            margin-bottom:15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h3>
        <%:Session["TITULO"] %></h3>
    <div id="splitterContainer">
        <div id="rightPane">
            <div id="rightSplitterContainer">
                <div id="rightTopPane">
                    <object id="tiffobj0" width="100%" height="90%" classid="CLSID:106E49CF-797A-11D2-81A2-00E02C015623"
                        style="margin-top: 0px">
                        <param name="src" value="samples/sample.alttif" />
                        <param name="enableevents" value="32">
                        <embed enableevents="32" toolbaritems="<%: Session["TOOL_BAR"] %>" id="tiffemb0"
                            access="1740" width="100%" height="100%" src="" type="application/x-alternatiff">
                    </object>
                </div>
                <script type="text/javascript">

                    var tiff0;
                    var pag;
                    if (document.getElementById) {
                        if (document.getElementById('tiffemb0')) {
                            tiff0 = document.getElementById('tiffemb0');
                        }
                        else if (document.getElementById('tiffobj0')) {
                            tiff0 = document.getElementById('tiffobj0');
                        }
                    }
                    else if (document.all) {
                        tiff0 = document.all.tiffobj0;
                    }

                    function scrollVisor(x, y) {
                        //                    tiff0.SetValue(15, x);
                        //                    tiff0.SetValue(16, y);
                    }

                    function CargarDoc() {
                        pag = document.getElementById('NumPagina').value;
                        tiff0.LoadImage(neg, pag, 0);
                    }

                    //$(document).ready(CargarDoc)
                    //setTimeout(CargarDoc(), 3000);
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


                    function CargarPaginaDigitada() {
                        alert('Cambio de índice de los documentos');
                        tiff0.GoToPage(pag);
                    }

                    function onRequestEnd() {
                        $.unblockUI();
                        pag = document.getElementById('NumPagina').value;
                        tiff0.GoToPage(pag);
                    }

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

                    $(document).ready(function () {
                        $(".dp").datepicker();
                    });

                    function pageLoad() {
                        $(".dp").datepicker();
                        $(".msk").mask("99/99/9999");
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
                            //                        if (this.value.length < 1) {
                            //                            var amt2 = 0;
                            //                            $(this).val(amt2.toFixed(2));
                            //                        }
                            //                        else {
                            //                            var amt = parseFloat(this.value);
                            //                            $(this).val(amt.toFixed(2));
                            //                        }
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
                            //                        if (this.value.length < 1) {
                            //                            var amt2 = 0;
                            //                            $(this).val(amt2.toFixed(2));
                            //                        }
                            //                        else {
                            //                            var amt = parseFloat(this.value);
                            //                            $(this).val(amt.toFixed(2));
                            //                        }
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

                    function agregarFila(totalCamposPintar, nombreCampo, tipoCampo, nombreTabla, disponibilidad, listaCampos, maxLongitud, campoObligatorio) {

                        nuevaFila = document.getElementById(nombreTabla).insertRow(-1);
                        nuevaFila.id = posicionCampo;
                        nuevaCelda = nuevaFila.insertCell(-1);

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

                        nuevaCelda.innerHTML = "<td><input id=\"indice\" name=\"indicePos\" value='" + posCampo + "' type=\"hidden\" /></td>";
                        nuevaCelda = nuevaFila.insertCell(-1);

                        for (var i = 0; i <= totalCamposPintar - 1; i++) {
                            tipoCampo = arrTipoCampos[i];
                            campo = arrCampos[i];
                            disp = arrDisponibilidad[i];
                            datListas = arrDatosListas[i];
                            datMaxLongitud = arrMaximaLongitud[i];
                            datCampObligatorio = arrCamposObligatorios[i];

                            switch (tipoCampo) {
                                case "1":
                                    if (disp == "1") {
                                        if (datCampObligatorio == "True") {
                                            nuevaCelda.innerHTML = "<td><input onkeypress=\"return numbersonly(event);\" maxlength=" + datMaxLongitud + " required type=\"text\" name='" + campo + "' id='" + campo + "' ></td>";
                                            nuevaCelda = nuevaFila.insertCell(-1);
                                        }
                                        else {
                                            nuevaCelda.innerHTML = "<td><input onkeypress=\"return numbersonly(event);\" maxlength=" + datMaxLongitud + " type=\"text\" name='" + campo + "' id='" + campo + "' ></td>";
                                            nuevaCelda = nuevaFila.insertCell(-1);
                                        }
                                    }
                                    else {
                                        nuevaCelda.innerHTML = "<td><input disabled value=\" \" onkeypress=\"return numbersonly(event);\" type=\"text\" name='" + campo + "' id='" + campo + "' ></td>";
                                        nuevaCelda = nuevaFila.insertCell(-1);
                                    }
                                    break;

                                case "2":
                                    if (disp == "1") {
                                        if (datCampObligatorio == "True") {
                                            nuevaCelda.innerHTML = "<td><input type=\"text\" required maxlength=" + datMaxLongitud + " name='" + campo + "' id='" + campo + "' ></td>";
                                            nuevaCelda = nuevaFila.insertCell(-1);
                                        }
                                        else {
                                            nuevaCelda.innerHTML = "<td><input type=\"text\" maxlength=" + datMaxLongitud + " name='" + campo + "' id='" + campo + "' ></td>";
                                            nuevaCelda = nuevaFila.insertCell(-1);
                                        }
                                    }
                                    else {
                                        nuevaCelda.innerHTML = "<td><input disabled value=\" \" type=\"text\" name='" + campo + "' id='" + campo + "' ></td>";
                                        nuevaCelda = nuevaFila.insertCell(-1);
                                    }
                                    break;

                                case "3":
                                    if (disp == "1") {
                                        if (datCampObligatorio == "True") {
                                            nuevaCelda.innerHTML = "<td><input class=\"dp msk \" onblur=\"validarFecha(this)\" required type=\"text\" name='" + campo + "' ></td>";
                                            nuevaCelda = nuevaFila.insertCell(-1);
                                        }
                                        else {
                                            nuevaCelda.innerHTML = "<td><input class=\"dp msk \" onblur=\"validarFecha(this)\" type=\"text\" name='" + campo + "' ></td>";
                                            nuevaCelda = nuevaFila.insertCell(-1);
                                        }
                                    }
                                    else {
                                        nuevaCelda.innerHTML = "<td><input disabled value=\" \" type=\"text\" name='" + campo + "' ></td>";
                                        nuevaCelda = nuevaFila.insertCell(-1);
                                    }
                                    break;

                                case "16":
                                    if (disp == "1") {
                                        if (datCampObligatorio == "True") {
                                            nuevaCelda.innerHTML = "<td><input class=\"dec\" maxlength=" + datMaxLongitud + " required onkeypress=\"return numbersonly(event);\" type=\"text\" name='" + campo + "' ></td>";
                                            nuevaCelda = nuevaFila.insertCell(-1);
                                        }
                                        else {
                                            nuevaCelda.innerHTML = "<td><input class=\"dec\" maxlength=" + datMaxLongitud + " onkeypress=\"return numbersonly(event);\" type=\"text\" name='" + campo + "' ></td>";
                                            nuevaCelda = nuevaFila.insertCell(-1);
                                        }
                                    }
                                    else {
                                        nuevaCelda.innerHTML = "<td><input disabled value=\" \" type=\"text\" name='" + campo + "' ></td>";
                                        nuevaCelda = nuevaFila.insertCell(-1);
                                    }
                                    break;

                                case "11":
                                    if (disp == "1") {
                                        if (datCampObligatorio == "True") {
                                            nuevaCelda.innerHTML = "<td><input name='" + campo + "' type=\"checkbox\" value=\"1\" required></td>";
                                            nuevaCelda = nuevaFila.insertCell(-1);
                                        }
                                        else {
                                            nuevaCelda.innerHTML = "<td><input name='" + campo + "' type=\"checkbox\" value=\"1\" ></td>";
                                            nuevaCelda = nuevaFila.insertCell(-1);
                                        }
                                    }
                                    else {
                                        nuevaCelda.innerHTML = "<td><input disabled value=\" \" type=\"text\" name='" + campo + "' ></td>";
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

                                        var lista = "<td><select name ='" + campo + "'>";
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
                                        nuevaCelda.innerHTML = "<td><input disabled value=\"---No disponible---\" type=\"text\" name='" + campo + "' ></td>";
                                        nuevaCelda = nuevaFila.insertCell(-1);
                                    }
                                    break;
                            }
                        }

                        nuevaCelda.innerHTML = "<td><img src='../Images/Delete.png' title=\"Eliminar Registro\" onclick='eliminarFila(this)' onmouseover=\"this.style.cursor='pointer'\"/></td>";
                        posicionCampo++;
                        posCampo++;

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

                        $(".dp").datepicker({
                            maxDate: "+0D",
                            defaulDate: "+1w",
                            changeMounth: true,
                            numberOfMonths: 1,
                            dateFormat: "dd/mm/yy"
                        });
                        $(".msk").mask("99/99/9999");
                    }

                </script>
                <div id="rightBottomPane">
                    <div id="BottomPane">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel1" runat="server" BackColor="#1994A4" BorderColor="#999999" BorderStyle="Solid">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td class="style3" colspan="4">
                                                                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                                                        <asp:HiddenField ID="NumPagina" runat="server" ClientIDMode="Static" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="style3">
                                                                        Documento:
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="lstDocumentos" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstDocumentos_SelectedIndexChanged"
                                                                            Enabled="False">
                                                                        </asp:DropDownList>
                                                                        <asp:RoundedCornersExtender ID="lstDocumentos_RoundedCornersExtender" runat="server"
                                                                            Corners="All" Enabled="True" TargetControlID="lstDocumentos">
                                                                        </asp:RoundedCornersExtender>
                                                                    </td>
                                                                    <td class="style4">
                                                                        Grupo:
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="lstGrupos" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstGrupos_SelectedIndexChanged"
                                                                            Enabled="False">
                                                                        </asp:DropDownList>
                                                                        <asp:RoundedCornersExtender ID="lstGrupos_RoundedCornersExtender" runat="server"
                                                                            Corners="All" Enabled="True" TargetControlID="lstGrupos">
                                                                        </asp:RoundedCornersExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:RoundedCornersExtender ID="Panel1_RoundedCornersExtender" runat="server" Color="Aquamarine"
                                                Enabled="True" Radius="6" TargetControlID="Panel1">
                                            </asp:RoundedCornersExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pControlsM" runat="server">
                                                <br />
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <asp:Table ID="tblControls" runat="server" EnableViewState="False">
                                                                <asp:TableRow ID="R1" runat="server">
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="divControles">
                                                                <asp:Panel ID="pControls" runat="server">
                                                                </asp:Panel>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnInsertar" OnClientClick="resetearIndice();" runat="server" OnClick="Button1_Click1"
                                                                Text="Guardar" CssClass="btn btn-login" ViewStateMode="Enabled" />
                                                            &nbsp;&nbsp;
                                                            <asp:Button ID="btnReset" runat="server" CssClass="btn btn-login" OnClientClick="formReset();"
                                                                Text="Reset" ViewStateMode="Enabled" />
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:RoundedCornersExtender ID="pControlsM_RoundedCornersExtender" runat="server"
                                                Corners="All" Enabled="True" TargetControlID="pControlsM">
                                            </asp:RoundedCornersExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
