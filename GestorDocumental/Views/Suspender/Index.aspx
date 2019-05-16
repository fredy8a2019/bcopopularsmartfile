<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Suspender
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <meta http-equiv="expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="-1">

    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/splitter.js" type="text/javascript"></script>
    <link href="../../Content/Styles/estilos.css" rel="stylesheet" type="text/css" />
    <%--<link rel="stylesheet" href="http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css">
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

        .divleft {
            float: left;
            display: inline;
            width: 27%;
        }

            .divleft label {
                display: inline;
            }

        #cotBuscar {
            float: left;
            display: inline;
            width: 80%;
        }

            #cotBuscar input[type=text], #contentSearch input[type=text] {
                width: 31%;
                margin-right: 25px;
                margin-left: 20px;
            }

        textarea {
            width: 86%;
            border-radius: 5px;
            margin-right: 25px;
        }

        .span {
            background-color: rgb(238, 238, 238) !important;
            color: rgb(102, 102, 102) !important;
            border: none !important;
            cursor: pointer !important;
            padding: 2px 12px 3px 12px !important;
            text-decoration: none !important;
            border: 1px solid rgb(128,128,128) !important;
            border-radius: 4px !important;
        }

        #arriba {
            position: relative;
            z-index: 1;
        }

        #abajo {
            position: relative;
            z-index: 0;
        }

        .span:hover {
            background-color: rgb(250, 250, 250) !important;
        }

        h2 {
            font-size: 30px;
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

        .t-header {
            background-color: #205390;
            color: white;
            height: 30px;
        }

            .t-header .t-link {
                color: white;
                height: 100%;
                width: 100%;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />    
    <div class="row-fluid" style="height: 800px">

        <div class="alert alert-danger" id="divError" style="display: none">
            <strong id="stro2">Negocio no especificado o modulo no especificado</strong>
        </div>
        <div id="zoom" style="overflow: scroll;" hidden="hidden" onmousemove="moverScrolls(event)"></div>
        <h2>Suspender</h2>
        <div class="row">
            <div class="col-lg-1" style="margin: 5px 0px 0px 5px; width: 10%">
                <label>Negocio:</label>
            </div>
            <div class="col-lg-2" style="margin: 5px 0px 0px 5px">
                <input type='text' id='txtNegocio' name='txtNegocio' class="form-control" onkeypress='return esNumero(event)' autofocus style="width: 150px; height: 35;" />
            </div>

            <div class="col-lg-3" style="margin: 5px 0px 0px 5px; width: 6%">
                <label>Modulo:</label>
            </div>

            <div class="col-lg-4" style="margin: 5px 0px 0px 5px; width: 16%">
                <select id='txtEtapa' name='txtEtapa' class="form-control" style="width: 150px; height: 35;">
                    <option value="-1">Seleccione</option>
                    <option value="20">Indexacion</option>
                    <option value="30">Captura 1</option>
                    <option value="40">Captura 2</option>
                    <option value="50">Control Calidad</option>
                    <option value="260">Dactiloscopía</option>
                </select>
            </div>
            <div class="col-lg-4" style="margin: 5px 0px 0px 5px">
                <input type="button" style='width: 100px; height: 34px;' id="btn_BuscarNeg" name="btn_BuscarNeg" value="Suspender" class="btn btn-login" onclick="buscarNegocio()" />
            </div>
        </div>
        <br />
        <div class="row">
            <div id="Div3" style="width: 100%; overflow-x: scroll">
                <%= Html.Telerik().StyleSheetRegistrar() 
                                                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
                <%= Html.Telerik().Grid<GestorDocumental.Models.sp_Suspender_Suspendidos_Result>()
                        .Name("NegSuspendidos")
                        .Sortable()
                        .Localizable("es-ES")  
                        .DataBinding(databindig => databindig.Ajax().Select("_consultarNegociosSuspendidos", "Suspender"))
                        .Columns(colums =>
                        {
                            colums.Bound(o => o.NegId).HtmlAttributes(new { id = "IdNeg", style = "display:none" }).HeaderHtmlAttributes(new { style = "display:none" });
                            colums.Bound(o => o.IdEtapa).HtmlAttributes(new { id = "idEtapa", style = "display:none" }).HeaderHtmlAttributes(new { style = "display:none" });
                            
                            colums.Bound(o => o.NegId).Width(30).Title("NegId");
                            colums.Bound(o => o.DescEtapa).Width(280).Title("Etapa");

                            //colums.Command(o => o.Custom("Activar").HtmlAttributes(new { href = "/Suspender/Index"}));
                            colums.Command(o => o.Custom("Activar").Text("<span class=\"glyphicon glyphicon-ok\" aria-hidden=\"true\"></span><span id=\"ocultar\">&nbsp;Activar</span>")
                                                            .HtmlAttributes(new { onclick = "Activar(this)" })
                                                            .SendState(true)
                                                            .DataRouteValues(route =>
                                                            {
                                                                route.Add(x => x.NegId).RouteKey("idNeg");
                                                                route.Add(x => x.IdEtapa).RouteKey("idEtapa");
                                                            })
                                                            )
                                                            .Title("")
                                                            .Width(25);                                                           
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

        function Activar(e) {



            var columna = $(e).parent();
            var fila = $(columna).parent();

            var neg = fila.children("#IdNeg").text();
            var etapa = fila.children("#idEtapa").text();

            transact.ajaxPOST("/Suspender/Activar?Neg=" + neg + "&Etapa=" + etapa, null,
                function (data) {

                    if (data == 1) {

                        bootbox.dialog({
                            message: "<h4>El negocio se activo corectamente</h4>",
                            title: "Confirmación",
                            buttons: {
                                succes: {
                                    label: ".: Aceptar :.",
                                    className: "btn-success",
                                    callback: function () {
                                        window.location.href = "/Suspender/index";
                                    }
                                },

                            }
                        });
                    } else {

                        bootbox.dialog({
                            message: "<h4>No fue posible activar el negocio</h4>",
                            title: "Confirmación",
                            buttons: {
                                succes: {
                                    label: ".: Aceptar :.",
                                    className: "btn-success",
                                    callback: function () {
                                        window.location.href = "/Suspender/index";
                                    }
                                },

                            }
                        });
                    }

                }, function (error) { console.log(error) });
        }

        function buscarNegocio() {
            var neg = document.getElementById("txtNegocio").value;
            var etapa = document.getElementById("txtEtapa").value;

            if (neg == "") {
                Alternar(divError);
                return false;
            }
            if (etapa == "-1") {
                Alternar(divError);
                return false;
            }
            transact.ajaxPOST("/Suspender/Suspender?Neg=" + neg + "&Etapa=" + etapa, null,
                function (data) {

                    if (data == 1) {
                        //alert("El negocio se suspendio corectamente");
                        //window.location.href = "/Suspender/index";
                        bootbox.dialog({
                            message: "<h4>El negocio se suspendio corectamente</h4>",
                            title: "Confirmación",
                            buttons: {
                                succes: {
                                    label: ".: Aceptar :.",
                                    className: "btn-success",
                                    callback: function () {
                                        window.location.href = "/Suspender/index";
                                    }
                                },

                            }
                        });
                    } else if (data[0] == 0) {
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