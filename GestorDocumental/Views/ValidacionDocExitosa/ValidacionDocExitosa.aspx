<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edicion Captura
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <meta http-equiv="expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="-1">

    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />


    <style type="text/css">
        body {
            background: #ffffff;
        }

        .t-header {
            background-color: #205390;
            color: white;
            height: 30px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid" style="height: 800px">
        <br />
        <br />

        <div id="zoom" style="overflow: scroll;" hidden="hidden" onmousemove="moverScrolls(event)"></div>

        <h2>Aprobar Validación Documental</h2>
        <div class="row">
            <div class="col-lg-1" style="margin: 5px 0px 0px 5px; width: 10%">
                <label>Negocio:</label>
            </div>
            <div class="col-lg-2" style="margin: 5px 0px 0px 5px">
                <input type='text' id='txtNegocio' name='txtNegocio' class="form-control" autofocus style="width: 150px; height: 35px; margin" onkeypress="return valida(event)" />
            </div>

            <div class="col-lg-4" style="margin: 5px 0px 0px 5px">
                <button class="btn btn-login" style='width: 100px; height: 34px;' id="btn_BuscarNeg" name="btn_BuscarNeg" onclick="BuscarNegocio()">
                    <span class="glyphicon glyphicon-search" aria-hidden="true"></span>&nbsp;Buscar
                </button>
            </div>
            <br />
            <div class="alert alert-danger" id="negocioVacio" style="display: none">
                <strong id="stro1"></strong>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="alert alert-danger" id="divError" style="display: none">
                <strong id="stro2">Negocio no especificado o modulo no especificado</strong>
            </div>
        </div>
        <div class="row">
            <div id="Div3" style="width: 100%; overflow-x: scroll">
                <%= Html.Telerik().StyleSheetRegistrar() 
                                                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
                <%= Html.Telerik().Grid<GestorDocumental.Models.sp_valDocEx_ObtenerInfoCausalesNegadas_Result>()
                        .Name("TelEdicionIncidentes")
                        .Sortable()
                        .Localizable("es-ES")  
                        .DataBinding(databindig => databindig.Ajax().Select("_ConsultarNegocio", "ValidacionDocExitosa"))
                        .Columns(colums =>
                        {
                            colums.Bound(o => o.cod_causal).Title("Codigo causal").HeaderHtmlAttributes(new { style = "center: 10px; height: 30px; width: 10px; margin: 0px 0px 0px 0px;" });
                            //colums.Bound(o => o.Fecha).HtmlAttributes(new { id = "idEtapa", style = "display:none" }).HeaderHtmlAttributes(new { style = "display:none" });
                            colums.Bound(o => o.nom_causal).Title("Nombre Causal").HeaderHtmlAttributes(new { style = "center: 10px; height: 30px; width: 10px; margin: 0px 0px 0px 0px;" });
                            colums.Bound(o => o.DocDescripcion).Title("Documentos").HeaderHtmlAttributes(new { style = "center: 10px; height: 30px; width: 10px; margin: 0px 0px 0px 0px;" });
                            //colums.Bound(o => o.error).Title("Error").HeaderHtmlAttributes(new { style = "center: 40px; height: 10px; width: 30px" });

                            //colums.Command(o => o.Custom("Activar").HtmlAttributes(new { href = "/Suspender/Index"}));
                                                                                    
                        })
                        .Pageable(paginas => paginas.PageSize(5))
                        .ClientEvents(events => events.OnDataBound("onComplete"))
                        .TableHtmlAttributes(new {id="MyGrid"})
                %>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-2" style="margin: 5px 0px 0px 5px">
                <button class="btn btn-login" style='width: 180px; height: 34px;' id="btn_NegExitoso" disabled="disabled" name="btn_NegExitoso" onclick="PasarNegocioExitoso()">
                    <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>&nbsp;Pasar Negocio Exitoso
                </button>
            </div>
            <div class="col-lg-1" style="margin: 5px 0px 0px 5px">
                <button class="btn btn-login" style='width: 100px; height: 34px;' id="btn_Cancelar" disabled="disabled" name="btn_Cancelar" onclick="Cancelar()">
                    <span class="glyphicon glyphicon-remove-circle" aria-hidden="true"></span>&nbsp;Cancelar
                </button>
            </div>
        </div>

    </div>

    <script>

        function onComplete(e) {
            $(".t-button").removeAttr('href');
            var h = $("#MyGrid").outerWidth();

            //$("#GridConsulta").width(h + 2);
        }

        function dimencionesFullScreen() {
            var browserWidth = $(window).width(); //document.documentElement.clientWidth;
            var browserHeight = $(window).height(); //document.documentElement.clientHeight;
            document.getElementById("zoom").style.width = browserWidth;
            document.getElementById("zoom").style.height = browserHeight;
        }

        function BuscarNegocio() {
            var negocio = document.getElementById("txtNegocio").value;

            if (negocio == "") {
                Alternar(divError);
                return false;
            }
            transact.ajaxPOST("/ValidacionDocExitosa/BuscarNegocio?negocio=" + negocio, null,
                function (data) {
                    var snExt = parseInt(data[0]);

                    if (snExt == 1) {
                        document.getElementById("txtNegocio").setAttribute("disabled", "disabled");
                        document.getElementById("btn_BuscarNeg").setAttribute("disabled", "disabled");
                        document.getElementById("btn_NegExitoso").removeAttribute("disabled");
                        document.getElementById("btn_Cancelar").removeAttribute("disabled");
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

        function Cancelar() {
            window.location.href = '/ValidacionDocExitosa/ValidacionDocExitosa';
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

        function valida(e) {
            tecla = (document.all) ? e.keyCode : e.which;

            //Tecla de retroceso para borrar, siempre la permite
            if (tecla == 8) {
                return true;
            }

            // Patron de entrada, en este caso solo acepta numeros
            patron = /[0-9]/;
            tecla_final = String.fromCharCode(tecla);
            return patron.test(tecla_final);
        }

        function PasarNegocioExitoso(Seccion) {

            bootbox.dialog({
                message: "<h4>¿Seguro que el caso aprueba validación Documental?</h4>",
                title: "Confirmación",
                buttons: {
                    success: {
                        label: ":: Aceptar ::",
                        className: "btn-success",
                        callback: function () {
                            var negId = document.getElementById("txtNegocio").value;
                            transact.ajaxPOST("/ValidacionDocExitosa/PasarNegocioExitoso?negId=" + negId, null, function () {
                                window.location.href = '/ValidacionDocExitosa/ValidacionDocExitosa';
                            }, function (error) { console.log(error) });
                        }
                    },
                    danger: {
                        label: ":: Cancelar ::",
                        className: "btn-danger",
                        callback: function () {
                            window.location.href = '/ValidacionDocExitosa/ValidacionDocExitosa';
                        }
                    }
                }
            });
        }

    </script>
</asp:Content>
