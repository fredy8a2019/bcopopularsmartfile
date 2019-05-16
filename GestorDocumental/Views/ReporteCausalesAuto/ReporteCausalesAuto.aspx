<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Reporte Causales Automaticas
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

        .cajaTexto {
            /*display: block;*/
            /*padding: 6px 12px;*/
            width: 100px;
            height: 30px;
            /*font-size: 14px;*/
            /*line-height: 1.42857143;*/
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 4px;
            /*-webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);*/
            /*box-shadow: inset 0 1px 1px rgba(0,0,0,.075);*/
            /*-webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;*/
            /*-o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;*/
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function buscarNegocio() {
            var _NegId = $("#txtNegocio").val();

            if (_NegId == "") {
                $('#stro1').empty();
                $('#stro1').append("DEBE INGRESAR UN NEGOCIO PARA COLNSULTAR SUS CAUSALES AUTOMATICAS.");
                Alternar(negocioVacio);
                $('#stro1').html();
            }
            else {

                transact.ajaxPOST("/ReporteCausalesAuto/BuscarNegocio?_NegId=" + _NegId, null,
                    function (data) {
                        var snExt = parseInt(data[0]);

                        if (snExt == 1) {
                            refrescaGrillaReporte();
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
                    }, function (error) { console.log(error) }
                );
            }
        }

        function LimpiarCampos() {
            $('#txtDocumento').val('');
            $('#txtDocumento').focus();
        }

        function refrescaGrillaTipologia() {
            $("#GridConsulta.t-grid .t-refresh").trigger('click');
        }

        function refrescaGrillaReporte() {
            $("#GridReporteCA.t-grid .t-refresh").trigger('click');
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

    </script>
    <div id="divContenerdor">
        <br />
        <h2>Reporte Causales Automáticas</h2>
        <br />
        <div class="row-fluid scheduler-border">

            <fieldset class="scheduler-border">
                <div class="row">
                    <div class="col-lg-1" style="margin: 5px 0px 0px 0px;">
                        <span class="lblMedio" id="lbNCodConsultor"><b>Negocio:</b></span>
                    </div>
                    <div class="col-lg-8">
                        <input type='text' class="cajaTexto" required='required' id='txtNegocio' name='txtNegocio' value='' onkeypress='return esNumero(event)' autofocus />
                        <input type="button" id="btn_BuscarNeg" name="btn_BuscarNeg" value="..." class="btn btn-login" onclick="buscarNegocio()" />
                    </div>
                </div>
                <br />
                <div class="alert alert-danger" id="negocioVacio" style="display: none">
                    <strong id="stro1"></strong>
                </div>

                <table style="width: 100%; text-align: center;">
                    <tr>
                        <td valign="top" style="margin-top: 15px">
                            <%= Html.Telerik().StyleSheetRegistrar() 
                                                .DefaultGroup(group => group.Add("telerik.hay.css")) %>

                            <%= Html.Telerik().Grid<GestorDocumental.Controllers._grillaReporteCausalesAuto>()
                            .Name("GridReporteCA")
                            .DataBinding(databinding => databinding.Ajax().Select("_consultarDatosReporte", "ReporteCausalesAuto"))
                            .Columns(colums =>
                            {
                                    colums.Bound(o => o.NombreCampo).Title("Nombre Campo").HeaderHtmlAttributes(new { style = "center: 40px; height: 10px; width: 150px" });
                                    colums.Bound(o => o.ValorCampo).Title("Valor Campo");
                                    colums.Bound(o => o.NombreCausal).Title("Nombre Causal");
                                    colums.Bound(o => o.ParametroEvaluacion).Title("Parámetro Evaluación");
                                    colums.Bound(o => o.Resultado).Title("Resultado");
                                    colums.Bound(o => o.Documento).Title("Documento");
                             })
                             .Pageable(paginas => paginas.PageSize(5))
                            %>
                        </td>
                    </tr>
                </table>

            </fieldset>

        </div>
        <%--<script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>--%>
    </div>
</asp:Content>
