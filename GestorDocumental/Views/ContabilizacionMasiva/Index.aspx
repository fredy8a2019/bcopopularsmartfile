<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Contabilización Masiva
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>

    <script src="../../Content/Styles/bootbox.js"></script>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>

    <style type="text/css">
        legend.scheduler-border {
            font-size: x-large !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function onImageUpload(e) {
            e.data = { tContabilizacion: $('input:radio[name=tipoContabilizacion]:checked').val() };
        }

        function completado() {
            $.ajax({
                type: "GET",
                url: "/ContabilizacionMasiva/obtenerTotalArchivos",
                dataType: "json",
                success: function (result) {
                    $("#txtNoDocumentos").val(result);
                    bootbox.alert("Archivo subido correctamente!");
                },
                error: function (result) {
                    console.log(result);
                }
            });
        }

        function validarUpload() {
            if ($('#fContabilizados').is(':checked') || $('#fRechazados').is(':checked')) {
                $("#rUpload").removeAttr("style");
            }
        }

        function validarDocumento() {
            var valor = $("#txtNoDocumentos").val();
            if (valor == "") {
                bootbox.dialog({
                    message: "El archivo seleccionado es incorrecto",
                    title: "<b>Error</b>",
                    buttons: {
                        danger: {
                            label: ":: Aceptar ::",
                            className: "btn-danger",
                        }
                    }
                });
                return false;
            }
        }

    </script>
    <form action="/ContabilizacionMasiva/GuardarContabilizacion" method="post" onsubmit="return validarDocumento()">
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Contabilización Masiva</legend>
            <div class="container-fluid">
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-4">
                            Fecha y Hora
                            <input type="text" class="form-control" id="txtFecha" name="txtFecha" value="<%= DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString() %>" readonly="true" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4">
                            <input type="radio" name="tipoContabilizacion" onclick="validarUpload()" id="fContabilizados" value="contabilizados" />
                            Formato Contabilizados
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4">
                            <input type="radio" name="tipoContabilizacion" onclick="validarUpload()" id="fRechazados" value="rechazados" />
                            Formato Rechazados
                        </div>
                    </div>
                    <br />
                    <div class="row" id="rUpload" style="display: none">
                        <div class="col-xs-4">
                            <%= Html.Telerik().Upload()
                                .Name("files")
                                .Multiple(false)
                                .Async(async => async
                                    .Save("CrearContabilizacionMasiva", "ContabilizacionMasiva")
                                    .AutoUpload(true))
                                .ClientEvents(events => events
                                    .OnUpload("onImageUpload")
                                    .OnSuccess("completado"))
                            %>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4">
                            No. de Documentos
                            <input type="text" class="form-control" id="txtNoDocumentos" name="txtNoDocumentos" readonly="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="container-fluid">
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-4">
                            <textarea name="txtObservaciones" id="txtObservaciones"
                                placeholder="Observaciones" cols="20" rows="5" class="form-control"></textarea>
                        </div>
                    </div>
                </div>
            </div>
            <input type="submit" value=":: Guardar ::" class="btn btn-login"  />
        </fieldset>
    </form>
</asp:Content>
