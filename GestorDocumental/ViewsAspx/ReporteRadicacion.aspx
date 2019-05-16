<%@ Page Title="" Language="C#" MasterPageFile="~/ViewsAspx/Site1.Master" AutoEventWireup="true"
    CodeBehind="ReporteRadicacion.aspx.cs" Inherits="GestorDocumental.ViewsAspx.ReporteRadicacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%-- <script src="../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>--%>
    <link href="../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../Content/Scripts/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../Content/Scripts/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../Content/Scripts/ui/jquery.ui.button.js" type="text/javascript"></script>
    <script src="../Content/Scripts/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../Content/Scripts/ui/jquery.ui.menu.js" type="text/javascript"></script>
    <script src="../Content/Scripts/ui/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="../Content/Scripts/ui/jquery.ui.tooltip.js" type="text/javascript"></script>
    <script src="../Content/Scripts/ui/jquery.ui.datepicker.js" type="text/javascript"></script>
    <link href="../Content/themes/controls.css" rel="stylesheet" type="text/css" />
    <link href="../Content/Styles/comboboxUI.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
         #tableDatos,#datosEspecificos
        {
            width:100%;
        }
        
        .distanciaLeft
        {
            width:15%;    
        }
        
        .distanciaright
        {
            width:10%;
        }
        
        
        .custom-combobox {
        position: relative;
        display: inline-block;
        width: 100%;
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
        
        #divContenerdor
        {
            margin-top:2%;    
        }

        .styleMensaje
        {
            color:Red;   
        }
        
        #txtCodigoBarras, #txtNoDocumento,#txtNegId,#txtCausal,#txtProveedor
        {
            width: 80%;
            height: 1.5em;
            border-radius: 5px;
            border: solid 1px #999;
            padding: 2px 3px 2px 3px;
            margin-bottom: 15px;
            font-size: 1em;
            font-family: inherit;
            letter-spacing: 0.2ex;         
        }
        
        fieldset
        {
            margin-top:1%;    
        }
        
  </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divContenerdor">
        <h3>
            Reporte radicacion.
        </h3>
        <asp:Panel ID="Panel1" runat="server">
            <fieldset id="FiltrosPrincipales" class="scheduler-border">
                <legend class="scheduler-border">
                    <input type="checkbox" id="chkFiltrosPrincipales" name="name" value="" checked="checked" />Filtros
                    Principales</legend>
                <table id="tableDatos">
                    <tr>
                        <td class="distanciaLeft">
                            <label>
                                Fecha inicial:</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaIni" runat="server" Width="60%" ClientIDMode="Static" required="required"></asp:TextBox>
                            <asp:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="txtFechaIni" Format="dd-MM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <td class="distanciaright">
                            <label id="labelFecha">
                                Fecha final:
                            </label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaFin" runat="server" Width="60%" ClientIDMode="Static" required="required"></asp:TextBox>
                            <asp:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" Format="dd-MM-yyyy"
                                TargetControlID="txtFechaFin">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Clientes:
                            </label>
                        </td>
                        <td>
                            <input type="hidden" value="" id="hClientes" runat="server" clientidmode="Static" />
                            <select id="ddlClientes" runat="server" clientidmode="Static" data-work="clientes">
                                <option value=""></option>
                            </select>
                        </td>
                        <td>
                            <input type="checkbox" name="" id="chkOficionas" value=" " />
                            <label class="">
                                Oficinas:
                            </label>
                        </td>
                        <td>
                            <input type="hidden" value="" id="hOficinas" runat="server" clientidmode="Static" />
                            <select id="ddlOficinas" runat="server" clientidmode="Static" data-work="oficinas">
                                <option value=""></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="checkbox" name="" id="chkProductos" value=" " />
                            <label>
                                Productos:
                            </label>
                        </td>
                        <td>
                            <input type="hidden" value="" id="hProductos" runat="server" clientidmode="Static" />
                            <select id="ddlProductos" runat="server" clientidmode="Static" data-work="productos"
                                data-name="Reporte">
                                <option value=""></option>
                            </select>
                        </td>
                        <td>
                            <input type="checkbox" name="" id="chkSubProductos" value=" " />
                            <label class="">
                                SubProductos:
                            </label>
                        </td>
                        <td>
                            <input type="hidden" value="" id="hSubProductos" runat="server" clientidmode="Static" />
                            <select id="dllSubProductos" runat="server" clientidmode="Static" data-work="subProductos"
                                data-name="Reporte">
                                <option value=""></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="checkbox" name="" id="chkEstados" value="" />
                            <label class="">
                                Estados:
                            </label>
                        </td>
                        <td>
                            <input type="hidden" value="" id="hEstados" runat="server" clientidmode="Static" />
                            <select id="ddlEstados" runat="server" clientidmode="Static" data-work="estados"
                                data-name="Reporte">
                                <option value=""></option>
                            </select>
                        </td>
                        <td>
                            <input type="checkbox" name="" id="chkProveedor" value=" " />
                            <label>
                                RFC Proveedor:
                            </label>
                        </td>
                        <td>
                            <input type="text" id="txtProveedor" name="name" value="" clientidmode="Static" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="checkbox" name="" id="chkNomSociedad" value=" " />
                            <label class="">
                                Nombre sociedades:
                            </label>
                        </td>
                        <td>
                            <input type="hidden" value="" id="hSociedad" runat="server" clientidmode="Static" />
                            <select id="ddlSociedad" runat="server" clientidmode="Static" data-work="Sociedad">
                                <option value="">text</option>
                            </select>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="FiltrosEspecificos" disabled="disabled" class="scheduler-border">
                <legend class="scheduler-border">
                    <input type="checkbox" id="chkFiltrosEspecificos" name="name" value=" " />
                    Filtros Especificos</legend>
                <table id="datosEspecificos">
                    <tr>
                        <td class="distanciaLeft">
                            <input type="checkbox" name="" id="chkCodBarras" value=" " />
                            <label>
                                Codigo Barras:</label>
                        </td>
                        <td>
                            <input type="text" id="txtCodigoBarras" runat="server" name="name" value="" clientidmode="Static" />
                        </td>
                        <td class="distanciaright">
                            <input type="checkbox" name="" id="chkNegId" value=" " />
                            <label>
                                No. Negocio:</label>
                        </td>
                        <td>
                            <input type="text" id="txtNegId" runat="server" name="name" value="" clientidmode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="checkbox" name="" id="chkNoDocumentos" value=" " />
                            <label>
                                No. Documento:</label>
                        </td>
                        <td>
                            <input type="text" id="txtNoDocumento" runat="server" name="name" value="" clientidmode="Static" />
                        </td>
                        <td>
                            <input type="checkbox" name="" id="chkCausal" value=" " />
                            <label>
                                Causal:</label>
                        </td>
                        <td>
                            <input type="text" id="txtCausal" runat="server" name="name" value="" clientidmode="Static" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="tnGenerar" runat="server" Text="Generar reporte" CssClass="btn btn-login"
                        ValidationGroup="btnGuardar" OnClick="tnGenerar_Click" />
                    <input type="button" name="name" id="btnLimpiar" onclick="limpiarCampos()" class="btn btn-login"
                        value="Limpiar" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            </asp:UpdatePanel>
        </asp:Panel>
        <br />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Font-Names="Verdana"
            Font-Size="8pt" InteractiveDeviceInfos="(Colección)" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt" Height="500px" OnDrillthrough="ReportViewer1_Drillthrough"
            ShowRefreshButton="False">
            <LocalReport ReportPath="Reportes\Radicacion.rdlc" EnableHyperlinks="True" EnableExternalImages="True">
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:RoundedCornersExtender ID="ReportViewer1_RoundedCornersExtender" runat="server"
            Enabled="True" TargetControlID="ReportViewer1">
        </asp:RoundedCornersExtender>
    </div>
    <script src="../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <script src="../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script type="text/javascript">
        (function () {
            var _default = {

                data: function () {
                    transact.ajaxPOST("/Listas/_GetDropDownListClientes", null, _default._sucessesClientes, _default._error);
                    transact.ajaxPOST("/Listas/_GetDropDownListEstados", null, _default._sucessesEstado, _default._error);
                },

                _sucessesClientes: function (data) {
                    console.log(data);
                    _ui.fillCombo($("#ddlClientes"), data);
                },

                _sucessesEstado: function (data) {
                    console.log(data);
                    _ui.fillCombo($("#ddlEstados"), data);
                },

                _error: function (error) {
                    console.log(error);
                },

                _accionCheck: function (e) {
                    //var elemento = ;
                    var chek = e.currentTarget.checked;
                    if (chek == true) {
                        var idCampo = e.currentTarget.id;
                        switch (idCampo) {
                            case "chkProveedor":
                                $("#txtProveedor").removeAttr("disabled");
                                break;
                            case "chkCodBarras":
                                $("#txtCodigoBarras").removeAttr("disabled");
                                $("#txtNoDocumento").attr("disabled", "disabled");
                                $("#txtNoDocumento").val("");
                                $("#chkNoDocumentos").attr("checked", false);

                                $("#txtNegId").attr("disabled", "disabled");
                                $("#txtNegId").val("");
                                $("#chkNegId").attr("checked", false);

                                $("#txtCausal").attr("disabled", "disabled");
                                $("#txtCausal").val("");
                                $("#chkCausal").attr("checked", false);
                                break;
                            case "chkNoDocumentos":
                                $("#txtNoDocumento").removeAttr("disabled");
                                $("#txtCodigoBarras").attr("disabled", "disabled");
                                $("#txtCodigoBarras").val("");
                                $("#chkCodBarras").attr("checked", false);

                                $("#txtNegId").attr("disabled", "disabled");
                                $("#txtNegId").val("");
                                $("#chkNegId").attr("checked", false);

                                $("#txtCausal").attr("disabled", "disabled");
                                $("#txtCausal").val("");
                                $("#chkCausal").attr("checked", false);
                                break;
                            case "chkNegId":
                                $("#txtNegId").removeAttr("disabled", "disabled");
                                $("#txtNoDocumento").attr("disabled", "disabled");
                                $("#txtNoDocumento").val("");
                                $("#chkNoDocumentos").attr("checked", false);

                                $("#txtCodigoBarras").attr("disabled", "disabled");
                                $("#txtCodigoBarras").val("");
                                $("#chkCodBarras").attr("checked", false);

                                $("#txtCausal").attr("disabled", "disabled");
                                $("#txtCausal").val("");
                                $("#chkCausal").attr("checked", false);
                                break;
                            case "chkCausal":
                                $("#txtCausal").removeAttr("disabled", "disabled");
                                $("#txtNoDocumento").attr("disabled", "disabled");
                                $("#txtNoDocumento").val("");
                                $("#chkNoDocumentos").attr("checked", false);

                                $("#txtCodigoBarras").attr("disabled", "disabled");
                                $("#txtCodigoBarras").val("");
                                $("#chkCodBarras").attr("checked", false);

                                $("#txtNegId").attr("disabled", "disabled");
                                $("#txtNegId").val("");
                                $("#chkNegId").attr("checked", false);
                                break;
                            case "chkFiltrosPrincipales":
                                $("#FiltrosPrincipales").removeAttr("disabled");
                                $("#FiltrosEspecificos").attr("disabled", "disabled");
                                $("#chkFiltrosEspecificos").attr("checked", false);
                                $("#ddlClientes").next().find("input[autocomplete='off']").next().attr("class", "ui-button ui-widget ui-state-default ui-button-icon-only custom-combobox-toggle ui-corner-right");
                                $("#ddlClientes").next().find("input[autocomplete='off']").removeAttr("disabled");
                                $(($("#ddlClientes").next().find("input[autocomplete='off']").next().children())[0]).attr("class", "ui-button-icon-primary ui-icon ui-icon-triangle-1-s");
                                $("#chkFiltrosEspecificos").parent().parent().find("input[type='checkbox']").attr("checked", false);
                                $("#chkFiltrosEspecificos").parent().parent().find("input[type='text']").val("");
                                $("#txtFechaIni").attr("required", "required");
                                $("#txtFechaFin").attr("required", "required");
                                break;
                            case "chkFiltrosEspecificos":
                                $("#FiltrosEspecificos").removeAttr("disabled");
                                $("#FiltrosPrincipales").attr("disabled", "disabled");
                                $("#chkFiltrosPrincipales").attr("checked", false);
                                $("#txtFechaIni").removeAttr("required");
                                $("#txtFechaFin").removeAttr("required");
                                $("#chkFiltrosEspecificos").parent().parent().find("input[type='text']").attr("disabled", "disabled");
                                $("#ddlClientes").next().find("input[autocomplete='off']").next().removeAttr("class");
                                $(($("#ddlClientes").next().find("input[autocomplete='off']").next().children())[0]).removeAttr("class");
                                $("#ddlClientes").next().find("input[autocomplete='off']").attr("disabled", "disabled");
                                $("#ddlClientes").next().find("input[autocomplete='off']").val("");
                                $("#ddlClientes").next().find("input[type='hidden']").val("");
                                break;
                            default:
                                $("#" + idCampo).parent().next().find("input[autocomplete='off']").next().attr("class", "ui-button ui-widget ui-state-default ui-button-icon-only custom-combobox-toggle ui-corner-right");
                                $("#" + idCampo).parent().next().find("input[autocomplete='off']").removeAttr("disabled");
                                $(($("#" + idCampo).parent().next().find("input[autocomplete='off']").next().children())[0]).attr("class", "ui-button-icon-primary ui-icon ui-icon-triangle-1-s");
                                break;

                        }
                    } else {
                        var idCampo = e.currentTarget.id;
                        switch (idCampo) {
                            case "chkProveedor":
                                $("#txtProveedor").attr("disabled", "disabled");
                                $("#txtProveedor").val("");
                                break;
                            case "chkCodBarras":
                                $("#txtCodigoBarras").attr("disabled", "disabled");
                                $("#txtCodigoBarras").val("");
                                break;
                            case "chkNoDocumentos":
                                $("#txtNoDocumento").attr("disabled", "disabled");
                                $("#txtNoDocumento").val("");
                                break;
                            case "chkNegId":
                                $("#txtNegId").attr("disabled", "disabled");
                                $("#txtNegId").val("");
                                break;
                            case "chkCausal":
                                $("#txtCausal").attr("disabled", "disabled");
                                $("#txtCausal").val("");
                                break;
                            case "chkFiltrosPrincipales":
                                $("#FiltrosPrincipales").attr("disabled", "disabled");
                                $("#FiltrosEspecificos").removeAttr("disabled");
                                $("#chkFiltrosEspecificos").attr("checked", true);
                                $("#ddlClientes").next().find("input[autocomplete='off']").next().removeAttr("class");
                                $(($("#ddlClientes").next().find("input[autocomplete='off']").next().children())[0]).removeAttr("class");
                                $("#ddlClientes").next().find("input[autocomplete='off']").attr("disabled", "disabled");
                                $("#ddlClientes").next().find("input[autocomplete='off']").val("");
                                $("#ddlClientes").next().find("input[type='hidden']").val("");
                                $("#txtFechaIni").val("");
                                $("#txtFechaFin").val("");
                                $("#txtFechaIni").removeAttr("required");
                                $("#txtFechaFin").removeAttr("required");
                                $("#chkFiltrosEspecificos").parent().parent().find("input[type='text']").attr("disabled", "disabled");
                                break;
                            case "chkFiltrosEspecificos":
                                $("#FiltrosEspecificos").attr("disabled", "disabled");
                                $("#FiltrosPrincipales").removeAttr("disabled");
                                $("#chkFiltrosPrincipales").attr("checked", true);
                                $("#ddlClientes").next().find("input[autocomplete='off']").next().attr("class", "ui-button ui-widget ui-state-default ui-button-icon-only custom-combobox-toggle ui-corner-right");
                                $("#ddlClientes").next().find("input[autocomplete='off']").removeAttr("disabled");
                                $(($("#ddlClientes").next().find("input[autocomplete='off']").next().children())[0]).attr("class", "ui-button-icon-primary ui-icon ui-icon-triangle-1-s");
                                $("#chkFiltrosEspecificos").parent().parent().find("input[type='checkbox']").attr("checked", false);
                                $("#chkFiltrosEspecificos").parent().parent().find("input[type='text']").val("");
                                $("#txtFechaIni").attr("required", "required");
                                $("#txtFechaFin").attr("required", "required");
                                break;
                            default:
                                $("#" + idCampo).parent().next().find("input[autocomplete='off']").next().removeAttr("class");
                                $(($("#" + idCampo).parent().next().find("input[autocomplete='off']").next().children())[0]).removeAttr("class");
                                $("#" + idCampo).parent().next().find("input[autocomplete='off']").attr("disabled", "disabled");
                                $("#" + idCampo).parent().next().find("input[autocomplete='off']").val("");
                                $("#" + idCampo).parent().next().find("input[type='hidden']").val("");
                                break;

                        }
                    }
                },

                _CamposBloquedos: function () {
                    $("#chkOficionas").parent().next().find("input[autocomplete='off']").next().removeAttr("class");
                    $(($("#chkOficionas").parent().next().find("input[autocomplete='off']").next().children())[0]).removeAttr("class");
                    $("#chkOficionas").parent().next().find("input[autocomplete='off']").attr("disabled", "disabled");
                    $("#chkOficionas").parent().next().find("input[autocomplete='off']").val("");
                    $("#chkOficionas").parent().next().find("input[type='hidden']").val("");

                    $("#chkProductos").parent().next().find("input[autocomplete='off']").next().removeAttr("class");
                    $(($("#chkProductos").parent().next().find("input[autocomplete='off']").next().children())[0]).removeAttr("class");
                    $("#chkProductos").parent().next().find("input[autocomplete='off']").attr("disabled", "disabled");
                    $("#chkProductos").parent().next().find("input[autocomplete='off']").val("");
                    $("#chkProductos").parent().next().find("input[type='hidden']").val("");

                    $("#chkSubProductos").parent().next().find("input[autocomplete='off']").next().removeAttr("class");
                    $(($("#chkSubProductos").parent().next().find("input[autocomplete='off']").next().children())[0]).removeAttr("class");
                    $("#chkSubProductos").parent().next().find("input[autocomplete='off']").attr("disabled", "disabled");
                    $("#chkSubProductos").parent().next().find("input[autocomplete='off']").val("");
                    $("#chkSubProductos").parent().next().find("input[type='hidden']").val("");

                    $("#chkNomSociedad").parent().next().find("input[autocomplete='off']").next().removeAttr("class");
                    $(($("#chkNomSociedad").parent().next().find("input[autocomplete='off']").next().children())[0]).removeAttr("class");
                    $("#chkNomSociedad").parent().next().find("input[autocomplete='off']").attr("disabled", "disabled");
                    $("#chkNomSociedad").parent().next().find("input[autocomplete='off']").val("");
                    $("#chkNomSociedad").parent().next().find("input[type='hidden']").val("");

                    $("#chkEstados").parent().next().find("input[autocomplete='off']").next().removeAttr("class");
                    $(($("#chkEstados").parent().next().find("input[autocomplete='off']").next().children())[0]).removeAttr("class");
                    $("#chkEstados").parent().next().find("input[autocomplete='off']").attr("disabled", "disabled");
                    $("#chkEstados").parent().next().find("input[autocomplete='off']").val("");
                    $("#chkEstados").parent().next().find("input[type='hidden']").val("");

                    $("#chkProveedor").parent().next().find("input[type='text']").attr("disabled", "disabled");
                    $("#chkProveedor").parent().next().find("input[type='text']").val("");

                    $("#txtCodigoBarras").attr("disabled", "disabled");
                    $("#txtCodigoBarras").val("");
                    $("#txtNoDocumento").attr("disabled", "disabled");
                    $("#txtNoDocumento").val("");
                },

                _limpiarCampos: function () {
                    $("#txtFechaIni").val("");
                    $("#txtFechaFin").val("");
                    $("select option").attr("value", "");
                    $("#ddlClientes").parent().next().find("input[autocomplete='off']").val("");
                    $("#hClientes").val("");
                    _default._CamposBloquedos();
                },

                loagPage: function () {
                    _ui.buildCombobox();
                    _default.data();
                    $("#ddlClientes").combobox();
                    $("#ddlOficinas").combobox();
                    $("#ddlProductos").combobox();
                    $("#dllSubProductos").combobox();
                    $("#ddlSociedad").combobox();
                    $("#ddlEstados").combobox();
                    _ui.eventClick($("input[type='checkbox']"), _default._accionCheck);
                    _default._CamposBloquedos();
                }
            }

            _default.loagPage();

        })();

        function limpiarCampos() {
            $("#txtFechaIni").val("");
            $("#txtFechaFin").val("");
            $("#txtCodigoBarras").attr("disabled", "disabled");
            $("#txtCodigoBarras").val("");
            $("#txtProveedor").attr("disabled", "disabled");
            $("#txtProveedor").val("");
            $("#txtNoDocumento").attr("disabled", "disabled");
            $("#txtNoDocumento").val("");
            $("#txtCodigoBarras").attr("disabled", "disabled");
            $("#txtCodigoBarras").val("");
            $("#txtNegId").attr("disabled", "disabled");
            $("#txtNegId").val("");
            $("#txtCausal").attr("disabled", "disabled");
            $("#txtCausal").val("");
            $("select").parent().find("input[type='hidden']").val("");
            $("select").val("")
            $("select").parent().find("input[autocomplete='off']").val("");
            $("table input[type='checkbox']").removeAttr("checked");
            $("table input[type='checkbox']").parent().parent().find("td input[type='checkbox']").next().parent().next().find("a").removeAttr("class");
            $("table input[type='checkbox']").parent().parent().find("td input[type='checkbox']").next().parent().next().find("a span.ui-button-icon-primary.ui-icon.ui-icon-triangle-1-s").removeAttr("class");
            $("table input[type='checkbox']").parent().next().find("input[autocomplete='off']").attr("disabled", "disabled");
            $("#ctl00_MainContent_ReportViewer1_fixedTable").remove();
            $("#chkFiltrosEspecificos").attr("checked", false);
            $("#FiltrosPrincipales").removeAttr("disabled");
            $("#chkFiltrosPrincipales").attr("checked", true);
            $("#FiltrosEspecificos").attr("disabled", "disabled");
        }
    </script>
</asp:Content>
