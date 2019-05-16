<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script>

        (function () {
            var _default = {

                loadPages: function () {
                    $(document).ready(function () {
                        _ui.eventClick($("input[type='checkbox']"), _default._acccionClick);
                        $("#chkLabel").attr("checked", true);
                        $("#txtNegocio").attr("disabled", "disabled");
                        $("#txtRechazo").attr("disabled", "disabled");
                        _ui.eventOnFocus($("#txtLabel"), _default.cambioMensaje);
                        _ui.eventOnFocus($("#txtNegocio"), _default.cambioMensaje);
                        _ui.eventOnFocus($("#txtRechazo"), _default.cambioMensaje);

                    });
                },

                _acccionClick: function (e) {
                    var chek = e.currentTarget.checked;
                    var idCampo = e.currentTarget.id;
                    if (chek == true) {
                        switch (idCampo) {
                            case "chkLabel":
                                $("#txtLabel").removeAttr("disabled");
                                $("#txtLabel").attr("required", "required");

                                $("#txtNegocio").attr("disabled", "disabled");
                                $("#txtNegocio").val("");
                                $("#txtNegocio").removeAttr("required");
                                $("#chkNegocio").attr("checked", false);

                                $("#txtRechazo").attr("disabled", "disabled");
                                $("#txtRechazo").val("");
                                $("#txtRechazo").removeAttr("required");
                                $("#chkRechazo").attr("checked", false);

                                break;
                            case "chkNegocio":
                                $("#txtNegocio").removeAttr("disabled");
                                $("#txtNegocio").attr("required", "required");

                                $("#txtLabel").attr("disabled", "disabled");
                                $("#txtLabel").val("");
                                $("#txtLabel").removeAttr("required");
                                $("#chkLabel").attr("checked", false);

                                $("#txtRechazo").attr("disabled", "disabled");
                                $("#txtRechazo").val("");
                                $("#chkRechazo").attr("checked", false);
                                break;
                            case "chkRechazo":
                                $("#txtRechazo").removeAttr("disabled");
                                $("#txtRechazo").attr("required", "required");

                                $("#txtNegocio").attr("disabled", "disabled");
                                $("#txtNegocio").val("");
                                $("#txtNegocio").removeAttr("required");
                                $("#chkNegocio").attr("checked", false);

                                $("#txtLabel").attr("disabled", "disabled");
                                $("#txtLabel").val("");
                                $("#txtLabel").removeAttr("required");
                                $("#chkLabel").attr("checked", false);

                                break;
                            default:

                        }
                    } else {
                        switch (idCampo) {
                            case "chkLabel":
                                $("#chkLabel").attr("disabled", "disabled");
                                $("#txtNegocio").removeAttr("disabled");
                                $("#chkLabel").val("");
                                break;
                            case "chkNegocio":
                                $("#txtNegocio").attr("disabled", "disabled");
                                $("#chkLabel").removeAttr("disabled");
                                $("#txtNegocio").val("");
                                break;
                            case "chkRechazo":
                                $("#txtRechazo").attr("disabled", "disabled");
                                $("#chkRechazo").removeAttr("disabled");
                                $("#txtRechazo").val("");
                                break;
                            default:
                        }
                    }
                },

                cambioMensaje: function (e) {
                    var _this = $(e.currentTarget);
                    switch (_this.attr("id")) {
                        case "txtLabel":
                            $("#MensajeLabel").children().remove();
                            $("#MensajeLabel").text("");
                            $("#MensajeLabel").append("<strong><b> Recuerde:<b> </strong> Esta opcion anulara el negocio y la radicacion del label ");
                            break;
                        case "txtNegocio":
                            $("#MensajeLabel").children().remove();
                            $("#MensajeLabel").text("");
                            $("#MensajeLabel").append("<strong><b> Recuerde:<b> </strong> Esta opción anulara el negocio <b>PERO NO</b> la radicación del label");
                            break;
                        case "txtRechazo":
                            $("#MensajeLabel").children().remove();
                            $("#MensajeLabel").text("");
                            $("#MensajeLabel").append("<strong><b> Recuerde:<b> </strong> Esta opcion anulara el negocio y el label de radicacion por solicitid del cliente ");
                            break;

                        default:

                    }
                }
            }

            _default.loadPages();
        })(); 
    </script>
    <link href="../../Content/themes/controls.css" rel="stylesheet" type="text/css" />
    <style>
        .divCampos
        {
            width: 50%;
            float: left;
        }
        
        .divCampos input[type='text']
        {
            float: right;
            margin-right: 23%;
            width: 45%;
        }
        
        .tooltip-inner
        {
            background-color: #94A132 !important;
        }
        
        #alertaSimulado
        {
            margin-top: 10%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Anulaciones
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Anular</h2>
    <form action="/Modificar/ejecutarAnulacion" method="post">
    <fieldset class="scheduler-border">
        <legend></legend>
        <p>
            Seleccione la opcion para anular registros de GNF</p>
        <div class="divCampos">
            <input type="checkbox" name="" value="" id="chkLabel" />
            <label>
                Codido de barras:</label>
            <input type="text" name="txtLabel" value="" id="txtLabel" maxlength="13" class="img"
                required="required" data-toggle="tooltip" data-placement="top" title="Requerido" />
        </div>
        <div class="divCampos">
            <input type="checkbox" name="" value="" id="chkNegocio" />
            <label>
                Numero de negocio:
            </label>
            <input type="text" name="txtNegocio" value="" id="txtNegocio" data-toggle="tooltip"
                data-placement="top" title="Requerido" />
        </div>
        <div class="divCampos">
            <input type="checkbox" name="" value="" id="chkRechazo" />
            <label>
                Rechazo (Label):
            </label>
            <input type="text" name="txtRechazo" value="" id="txtRechazo" maxlength="13" data-toggle="tooltip"
                data-placement="top" title="Requerido" />
        </div>
        <div id="alertaSimulado">
            <div class="alert alert-danger" id="panelAlerta" style="display: block; height: inherit;">
                <h4 id="MensajeLabel">
                </h4>
            </div>
        </div>
        <input type="submit" name="name" value=" : :  Anular  : : " class="btn btn-login" />
    </fieldset>
    </form>
</asp:Content>
