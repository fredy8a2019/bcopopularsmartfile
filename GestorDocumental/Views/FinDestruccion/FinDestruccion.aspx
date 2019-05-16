<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content5" ContentPlaceHolderID="TitleContent" runat="server">
    Destrucción Documental
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="../../Styles/jquery-ui.css" />
    <script type="text/javascript" src="../../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.min.js"></script>
    <link href="../../Content/themes/controls.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.maskedinput.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_fx.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <script src="../../Content/Scripts/Page/ajax.js" type="text/javascript"></script>
    <script src="../../Styles/BootsTrap/js/bootbox.js"></script>

    <style type="text/css">
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

            .span:hover {
                background-color: rgb(250, 250, 250) !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function alerta(e) {
            var cudSeleccionado = e.dataItem["cud"];
            bootbox.dialog({
                message: "<h4>¿Confirmar el envio de la Unidad Documental Seleccionada?</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ":: Si ::",
                        className: "btn btn-login",
                        callback: function () {
                            formularioEnvio(cudSeleccionado);
                        }
                    },
                    danger: {
                        label: ":: No ::",
                        className: "btn-danger"
                    }
                }
            });
        }

        function botones(e) {
            var nomBtn = e.name;
            if (nomBtn == "Imprimir") {
                //alert("Imprimir");
                printGrid(e);
            } else if (nomBtn == "Destruir") {
                //alert("Destruir");
                finDestruc(e);
            }

        }

        //Funcion para generar la planilla
        function printGrid(e) {
            var pru = e.dataItem["_nroActa"];
            var gridElement = $("#" + pru),
                printableContent,
                win = window.open('', '', 'width=800, height=500'),
                doc = win.document.open();

            var htmlStart =
                    '<!DOCTYPE html>' +
                    '<html>' +
                    '<head>' +
                    '<meta charset="utf-8" />' +
                    '<title>Planilla de datos</title>' +
                    '<style>' +
                    'table, th, td { border: 1px solid black; }' +
                    'html { font: 11pt sans-serif; }' +
                    '.t-grid { border-top-width: 0; }' +
                    '.t-grid, .t-grid-content { height: auto !important; }' +
                    '.t-grid-content { overflow: visible !important; }' +
                    'div.t-grid table { table-layout: auto; width: 100% !important; }' +
                    '.t-grid .t-grid-header th { border-top: 1px solid; }' +
                    '.t-grid-toolbar, .t-grid-pager > .t-link { display: none; }' +
                    '</style>' +
                    '</head>' +
                    '<body>';

            var htmlEnd =
                    '</body>' +
                    '</html>';

            var gridHeader = gridElement.children('.t-grid-header');
            if (gridHeader[0]) {
                var thead = gridHeader.find('thead').clone().addClass('t-grid-header');
                printableContent = gridElement
                    .clone()
                        .children('.t-grid-header').remove()
                    .end()
                        .children('.t-grid-content')
                            .find('table')
                                .first()
                                    .children('tbody').before(thead)
                                .end()
                            .end()
                        .end()
                    .end()[0].outerHTML;
            } else {
                printableContent = gridElement.clone();
                printableContent.find('a').remove();

                printableContent = printableContent.clone()[0].outerHTML;
            }

            doc.write(htmlStart + printableContent + htmlEnd);
            doc.close();
            win.print();
        }

        //Funcion para terminar el proceso de Destruccion Documental
        function finDestruc(e) {
            var nroActa = e.dataItem["_nroActa"];
            bootbox.dialog({
                message: "<h4>Confirma que desea terminar el proceso de Destrucción para el acta #" + nroActa + "</h4>",
                title: "<b>Finalizar Destruccion Documental</b>",
                buttons: {
                    succes: {
                        label: ".: SI :.",
                        className: "btn btn-login",
                        callback: function () {
                            transact.ajaxPOST("/FinDestruccion/destrucFinal?_nroActa= " + nroActa, null,
                                            function () {
                                                location.href = "/FinDestruccion/FinDestruccion";
                                            }, function (error) { console.log(error); });
                        }
                    },
                    danger: {
                        label: ".: NO :.",
                        className: "btn-danger"
                    }
                }
            });
        }

    </script>
    <h2>Confirmar Destrucción</h2>
    <fieldset class="scheduler-border">
        <legend>Actas Destrucción</legend>
        <%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.hay.css")) %>
        <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_InfoActa>()
                .Name("GrillaActasPadre")
                .Columns(columns =>
                {
                    columns.Bound(o => o._nroActa).Width(100).Title("Nro. Acta").HeaderHtmlAttributes(new { style = "height: 40px" });
                    columns.Bound(o => o._cantNeg).Width(100).Title("Cant. Negocios");
                    columns.Bound(o => o._fechCreacion).Width(100).Format("{0:F}").Title("Fecha Creación Acta");
                    columns.Bound(o => o._oficina).Width(100).Title("Oficina");
                    columns.Bound(o => o._cliente).Width(100).Title("Cliente");
                    columns.Command(o => o.Custom("Imprimir").Text("Imprimir")
                        .SendState(false)
                        .Ajax(true)).Width(100);
                    columns.Command(o => o.Custom("Destruir").Text("Destruir")
                        .SendState(false)
                        .Ajax(true)).Width(100);
                })
                    .DetailView(details => details.ClientTemplate(
                        Html.Telerik().Grid<GestorDocumental.Controllers.grilla_DetalleActa>()
                        .Name("<#=_nroActa#>")
                        .Columns(columns =>
                        {
                            columns.Bound(a => a._codNeg).Width(100).Title("Posición").HeaderHtmlAttributes(new { style = "height: 40px" });
                            columns.Bound(a => a._codUsuario).Width(100).Title("Negocio");
                            columns.Bound(a => a._codBarras).Width(100).Title("Codigo de Barras");
                        })
                        .DataBinding(databinding => databinding.Ajax()
                            .Select("getDetalleActa", "FinDestruccion", new { _nroActa = "<#= _nroActa #>" }))
                        .Pageable(paginas => paginas.PageSize(10))
                        .ToHtmlString()))

                .DataBinding(d => d.Ajax().Select("getInformacionActa", "FinDestruccion"))
                .Pageable(pager => pager.PageSize(25, new int[] { 10,25,50 })
                    .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
                .Sortable()
                .Filterable()
                .ClientEvents(e => e.OnCommand("botones"))
        %>
    </fieldset>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
</asp:Content>
