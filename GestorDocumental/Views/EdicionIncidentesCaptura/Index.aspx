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

    <%-- <link rel="stylesheet" href="http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css">
    <link href="http://www.jqueryscript.net/css/jquerysctipttop.css" rel="stylesheet" type="text/css">
    <script src="https://code.jquery.com/jquery-1.12.2.js"></script>
    <script src="http://netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js"></script>

    <link href="../../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
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

    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../Styles/menu-6.css" type="text/css" />
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>--%>

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
        <div class="alert alert-danger" id="divError" style="display: none">
            <strong id="stro2">Negocio no especificado o modulo no especificado</strong>
        </div>
        <div id="zoom" style="overflow: scroll;" hidden="hidden" onmousemove="moverScrolls(event)"></div>

        <h2>Edicion Incidencias Captura</h2>
        <div class="row">
            <div class="col-lg-1" style="margin: 5px 0px 0px 5px; width: 10%">
                <label>NumeroBizagi:</label>
            </div>
            <div class="col-lg-2" style="margin: 5px 0px 0px 5px">
                <input type='text' id='txtNroBizagi' name='txtNroBizagi' class="form-control" autofocus style="width: 150px; height: 35;" />
            </div>

            <div class="col-lg-4" style="margin: 5px 0px 0px 5px">
                <button class="btn btn-login" style='width: 100px; height: 34px;' id="btn_BuscarNeg" name="btn_BuscarNeg" onclick="buscarNroBizagi()">
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
            <div id="Div3" style="width: 100%; overflow-x: scroll">
                <%= Html.Telerik().StyleSheetRegistrar() 
                                                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
                <%= Html.Telerik().Grid<GestorDocumental.Models.sp_InCap_ObtenerInfoNroBizagi_Result>()
                        .Name("TelEdicionIncidentes")
                        .Sortable()
                        .Localizable("es-ES")  
                        .DataBinding(databindig => databindig.Ajax().Select("_ConsultarNegocioNroBizagi", "EdicionIncidentesCaptura"))
                        .Columns(colums =>
                        {
                            colums.Bound(o => o.negId).Title("NegId").HtmlAttributes(new { id = "IdNeg" }).HeaderHtmlAttributes(new { style = "center: 10px; height: 10px; width: 10px" });
                            //colums.Bound(o => o.Fecha).HtmlAttributes(new { id = "idEtapa", style = "display:none" }).HeaderHtmlAttributes(new { style = "display:none" });
                            colums.Bound(o => o.Fecha).Title("Fecha").HeaderHtmlAttributes(new { style = "center: 40px; height: 10px; width: 30px" });
                            colums.Bound(o => o.documentosAnexos).Title("Documentos").HeaderHtmlAttributes(new { style = "center: 40px; height: 10px; width: 30px" });
                            colums.Bound(o => o.error).Title("Error").HeaderHtmlAttributes(new { style = "center: 40px; height: 10px; width: 30px" });

                            //colums.Command(o => o.Custom("Activar").HtmlAttributes(new { href = "/Suspender/Index"}));
                            colums.Command(o => o.Custom("EditarCaptura").Text("<span class=\"glyphicon glyphicon-pencil\" aria-hidden=\"true\"></span><span id=\"ocultar\">&nbsp;Editar Captura</span>")
                                                            .HtmlAttributes(new { onclick = "EditarCaptura(this)" })
                                                            .SendState(true)
                                                            .DataRouteValues(route =>
                                                            {
                                                                route.Add(x => x.negId).RouteKey("idNeg");
                                                                route.Add(x => x.Fecha).RouteKey("idEtapa");
                                                            })
                                                            );                                                         
                        })
                        .Pageable(paginas => paginas.PageSize(5))
                        .ClientEvents(events => events.OnDataBound("onComplete"))
                        .TableHtmlAttributes(new {id="MyGrid"})
                %>
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

        function EditarCaptura(e) {

            var columna = $(e).parent();
            var fila = $(columna).parent();
            var neg = fila.children("#IdNeg").text();

            transact.ajaxPOST("/EdicionIncidentesCaptura/obtenerNegid?negId=" + neg, null,
                function (data) {
                    var snExt = parseInt(data[0]);
                    if (snExt == 1) {
                        window.location.href = "/EdicionIncidentesCaptura/EditarCaptura";
                    } else {

                        bootbox.dialog({
                            message: "<h4>No fue posible activar el negocio</h4>",
                            title: "Confirmación",
                            buttons: {
                                succes: {
                                    label: ".: Aceptar :.",
                                    className: "btn-success",
                                    callback: function () {
                                        window.location.href = "/EdicionIncidentesCaptura/index";
                                    }
                                },

                            }
                        });
                    }

                }, function (error) { console.log(error) });
        }

        function buscarNroBizagi() {
            var nroBizagi = document.getElementById("txtNroBizagi").value;

            if (nroBizagi == "") {
                Alternar(divError);
                return false;
            }
            transact.ajaxPOST("/EdicionIncidentesCaptura/BuscarNroBizagi?nroBizagi=" + nroBizagi, null,
                function (data) {
                    var snExt = parseInt(data[0]);

                    if (snExt == 1) {
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
</asp:Content>
