<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <%--<script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>--%>

    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css"/>
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
    <script src="../../Scripts/Reestricciones.js" type="text/javascript"></script>
    <script src="../../Styles/BootsTrap/js/bootbox.js" type="text/javascript"></script>
    

    <style type="text/css">
        input[type="text"] {
            /*margin-top: 4px;*/
            /*----------------------------------*/
            display: block;
            width: 100%;
            height: 34px;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }

        .custom-combobox {
            /*width: 100%;*/
            font-size: 70%;
            margin-top: 4px;
            /*----------------------------------*/
            display: block;
            width: 100%;
            height: 34px;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Radicación
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <h2>Radicación</h2>
    <form action="/Radicacion/Guardar" method="post" onsubmit="return validFormulario()">
        <fieldset id="DatosPrinciples" class="scheduler-border">
            <legend class="scheduler-border">Datos Principales</legend>
            
            <div class="row">
                <div class="col-lg-2">
                    <label>Fecha Radicación</label>
                </div>
                <div class="col-lg-3">
                        <input type="text" id="Text1" name="FechaRadicacion" value="<%= DateTime.Now %>" disabled="disabled" class="form-control" />
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2">
                        <label>Cliente:</label>
                </div>
                <div class="col-lg-3">
                        <input type="text" id="Text2" name="Cliente" value="<%= ViewData["Cliente"] %>" disabled="disabled" class="form-control" />
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2">
                        <label>Oficinas:</label>
                </div>
                <div class="col-lg-3">
                        <select id="sleOficinas" name="Oficinas" data-work="oficinas" class="form-control">
                        </select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2">
                        <label>Productos:</label>
                </div>
                <div class="col-lg-3">
                        <select id="sleProductos" name="Productos" data-work="productos" class="form-control">
                        </select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2">
                    <label>SubProductos:</label>
                </div>
                <div class="col-lg-3">
                        <select id="sleSubProductos" name="SubProductos" data-work="subProductos" class="form-control">
                        </select>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2">
                    <label>Tipo radicación:</label>
                </div>
                <div class="col-lg-3">
                        <select id="sleTipo" name="Tipo" data-work="tiposRadicacion" class="form-control">
                            <option></option>
                            <%--<option value="fisico">Físico </option>--%>
                            <option value="virtual">Virtual </option>
                        </select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2">
                        <label>Estados:</label>
                </div>
                <div class="col-lg-3">
                    <select id="sleEstados" name="Estados" data-work="estados" class="form-control" >
                    </select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2">
                    <div class="checkbox">
                        <input type="checkbox" id="chkAnulado" name="" value="" disabled="disabled" data-rol="<%= Session["ROL_USUARIO"].ToString() %>" style="visibility:hidden" />
                        <label style="visibility:hidden">Negocio a Anular:</label>
                    </div>
                </div>
                <div class="col-lg-3">
                    <input type="text" name="" id="NegAnulado" disabled="disabled" pattern="^[0-9]{0,8}$" style="visibility: hidden"/>
                </div>
            </div>
            
            <div class="row">
                <div class="col-lg-2">
                    <label id="lblConfirmar" style="display: none;">Confirmar</label>
                </div>
                <div class="col-lg-3">
                    <input type="text" name="NegAnuladoConfirmado" id="txtNegAnuladoConfirmado" value="" style="display: none;" pattern="^[0-9]{0,8}$" />
                </div>
            </div>
        </fieldset>
        <input type="hidden" id="HiddenSociedad" name="Sociedad" value="" />
        <input type="hidden" id="HiddenCausal" name="Causal" value="" />
        <input type="hidden" id="HiddenFechaActual" name="FechaLugarActual" value="" />
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Datos Complementarios</legend>
            <div id="tableContent">
            </div>
            <div id="radicacionVirtual" style="display: none;">
                <label>

                    Archivo:</label>
                <%= Html.Telerik().Upload()
                     .ClientEvents(ev => ev
                         .OnLoad("applyAccept")
                         .OnError("error")
                         .OnComplete("validarArchivoCargue"))
                    .Name("attachments")  
                    .Multiple(false)
                    .Async(async => async                                
                              .Save("SaveFile", "Radicacion")
                              .Remove("Remove", "Radicacion")
                                                      
                    .AutoUpload(true))
                 %>
            </div>
        </fieldset>
        <div class="row">
            <div class="col-lg-4">
                <input type="submit" name="btnGuardar" id="btnGuardar" value=":: Guardar ::" class="btn btn-login" style="width: auto; visibility: hidden;  />
                <input type="button" name="name" id="btnCancelar" value=":: Cancelar ::" style="width: auto;" class="btn btn-login" />
            </div>
        </div>
    </form>

    
    <script type="text/javascript">
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

        function validarFecha(valor, idCampo) {
            var resultado = validarFechaMenorActual(valor);
            if (resultado == false) {

                bootbox.dialog({
                    message: "La fecha no puede ser mayor a la fecha actual",
                    title: "<b>Error</b>",
                    buttons: {
                        danger: {
                            label: ":: Aceptar ::",
                            className: "btn-danger",
                            callback: function () {
                                $(idCampo).val("");
                            }
                        }
                    }
                });

            }
        }

        function validarArchivoCargue() {
            $("#btnGuardar").css("visibility", "visible");
        }

        (function ($) {

            var _default = {

                loadData: function () {

                    transact.ajaxPOST("/Radicacion/_GetDropDownListProductos/",
                                      null,
                                       _default._successProductos,
                                       _default._error);
                    transact.ajaxPOST("/Radicacion/_GetDropDownList_Oficinas/",
                                      null,
                                       _default._successOficinas,
                                       _default._error);
                    transact.ajaxPOST("/Radicacion/_GetDropDownListEstados/",
                                      null,
                                       _default._successEstados,
                                       _default._error);

                    if ($("#chkAnulado").data("rol") == "1") {
                        $("#chkAnulado").removeAttr("disabled");
                    } else {
                        $("#chkAnulado").attr("disabled", "disabled");
                    }
                    var fechaActual = new Date().toLocaleString();
                    $("#HiddenFechaActual").val(fechaActual);
                },

                _successProductos: function (data) {
                    _ui.fillCombo($("#sleProductos"), data);
                },

                _successOficinas: function (data) {
                    _ui.fillCombo($("#sleOficinas"), data);
                },

                _successEstados: function (data) {
                    _ui.fillCombo($("#sleEstados"), data);
                },

                _error: function (error) {
                    console.log(error);
                },

                _cancelarFomulario: function () {
                    window.location.href = '/Radicacion/Index';
                },

                _accionCheck: function (e) {
                    var chek = e.currentTarget.checked;
                    if (chek == true) {
                        var idCampo = e.currentTarget.id;
                        switch (idCampo) {
                            case "chkAnulado":
                                $("#NegAnulado").removeAttr("disabled");
                                $("#txtNegAnuladoConfirmado").css('display', 'block');
                                $("#NegAnulado").attr("required", "required");
                                $("#txtNegAnuladoConfirmado").attr("required", "required");
                                $("#lblConfirmar").css('display', 'block');
                                $("#NegAnulado").val("").css("border", "1px solid red");
                                $("#txtNegAnuladoConfirmado").css("border", "1px solid red");
                                break;
                            default:

                        }
                    } else {
                        var idCampo = e.currentTarget.id;
                        switch (idCampo) {
                            case "chkAnulado":
                                $("#NegAnulado").attr("disabled", "disabled");
                                $("#txtNegAnuladoConfirmado").css('display', 'none');
                                $("#txtNegAnuladoConfirmado").removeAttr("required");
                                $("#NegAnulado").removeAttr("required");
                                $("#lblConfirmar").css('display', 'none');
                                $("#NegAnulado").val("").removeAttr("style");
                                $("#txtNegAnuladoConfirmado").val("")
                                $("#txtNegAnuladoConfirmado").css("border", "1px solid red");
                                break;
                            default:

                        }
                    }
                },

                CompararNeg: function () {
                    var negAnular = $("#NegAnulado").val();
                    var negAnularConfirm = $("#txtNegAnuladoConfirmado").val();
                    if (negAnularConfirm != negAnular) {
                        $("#NegAnulado").val("").css("border", "1px solid red");
                        $("#txtNegAnuladoConfirmado").val("").css("border", "1px solid red");
                    }
                },

                DesmarcarInput: function () {
                    $("#NegAnulado").val("").css("border", "1px solid green");
                    $("#txtNegAnuladoConfirmado").val("").css("border", "1px solid green");
                },

                loagPage: function () {
                    _ui.buildCombobox();
                    $("#sleProductos").combobox();
                    $("#sleSubProductos").combobox();
                    $("#sleOficinas").combobox();
                    $("#sleEstados").combobox();
                    $("#sleTipo").combobox({
                        select: function () {
                            var valorSeleccionado = $("#sleTipo ").val();
                            if (valorSeleccionado == "fisico")
                                $("#btnGuardar").css("visibility", "visible");
                            else
                                $("#btnGuardar").css("visibility", "hidden");
                        }
                    });
                    $("#DatosPrinciples input[autocomplete='off']").attr("required", "required");
                    $($("#DatosPrinciples input[autocomplete='off']")[0]).attr("placeholder", "Seleccioné una oficina");
                    $($("#DatosPrinciples input[autocomplete='off']")[1]).attr("placeholder", "Seleccioné un producto");
                    $($("#DatosPrinciples input[autocomplete='off']")[2]).attr("placeholder", "Seleccioné un sub producto");
                    $($("#DatosPrinciples input[autocomplete='off']")[3]).attr("placeholder", "Seleccioné un tipo");
                    $($("#DatosPrinciples input[autocomplete='off']")[4]).attr("placeholder", "Seleccioné un estado");
                    _ui.eventClick($("#btnCancelar"), _default._cancelarFomulario);
                    _ui.eventClick($("#chkAnulado"), _default._accionCheck);
                }
            }

            _default.loadData();
            _default.loagPage();

        })(jQuery);

        function applyAccept(e) {
            $(this).find("input").attr("accept", "application/pdf");
            $(this).find("input").attr("accept", "application/tif");
        }

        function error(e) {
            alert("Error!! el archivo pesa mas de 5MB o no tiene la extension correcta")
        }

        function validFormulario() {
            var respuesta = true;

            var valor = $("#HiddenSociedad").val();
            if (valor == "" || valor == null) {
                $("#HiddenSociedad").val("-1");
            }


            var valorFecha = $("#FechaRadicacionBizagi").val();
            if (valorFecha != "") {
                var respuesta = _fx.validaFechaDDMMAAAA(valorFecha);

                if (respuesta == false) {
                    $("#FechaRadicacionBizagi").css("border", "1px solid red");
                    return false;
                }
            }
            // se le agrga validacion a la fecha faltante
            var valorFecha = $("#FechaRadicado").val();
            if (valorFecha != "") {
                var respuesta = _fx.validaFechaDDMMAAAA(valorFecha);
                if (respuesta == false) {
                    $("#FechaRadicado").css("border", "1px solid red");
                    return false;
                }
            }

            var negAnular = $("#NegAnulado").val();
            var negAnularConfirm = $("#txtNegAnuladoConfirmado").val();
            if (negAnularConfirm != negAnular) {
                respuesta = false;
            }

            if (respuesta == true) {
                document.getElementById('btnGuardar').disabled = true;
                document.getElementById('btnCancelar').disabled = true;
                document.getElementById('btnGuardar').style.display = 'none';
                document.getElementById('btnCancelar').style.display = 'none';
            }

            return respuesta;
        }
    </script>
    
</asp:Content>

