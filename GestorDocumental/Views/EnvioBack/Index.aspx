<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
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

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Envio de Unidad Documental
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

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

        function formularioEnvio(cud) {
            bootbox.dialog({
                title: "<b>Formato de envio</b>",
                message: "<h4>Envio unidad documental No. " + cud + "</h4>" +
                            "<form action='/EnvioBack/guardarFormato' method='post'>" +
                            "<input type='hidden' id='idCUD' name='idCUD' value='" + cud + "' />" +
                            "<%= ViewData["_camposEnvio"] %>" +
                            "<hr>" +
                            "<input type='submit' class='btn btn-login' id='envio' value=':: Enviar ::' />" +
                            "</form>"
            });
        }

    </script>

    <h1>Envio de Unidades Documentales</h1>
    <fieldset class="scheduler-border">
        <legend class="scheduler-border">Unidades Documentales listas para envio</legend>
        <%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.hay.css")) %>
        <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_EnvioPadre>()
                .Name("GrillaEnvioPadre")
                .Columns(columns =>
                {
                    columns.Bound(o => o.cud).Width(100).Title("CUD").HeaderHtmlAttributes(new { style = "height: 40px" });
                    columns.Bound(o => o.tipoContenedor).Width(100).Title("Tipo de Contenedor");
                    columns.Bound(o => o.nomOficina).Width(100).Title("Nombre Oficina");
                    columns.Bound(o => o.subProductos).Width(100).Title("SubProductos");
                    columns.Bound(o => o.fechaCreacion).Width(100).Format("{0:F}").Title("Fecha de Creación");
                    columns.Command(o => o.Custom("GenerarEnvio").Text("Generar Envio")
                        .SendState(false)
                        .Ajax(true)).Width(100);
                })
                    .DetailView(details => details.ClientTemplate(
                        Html.Telerik().Grid<GestorDocumental.Controllers.grilla_EnvioHijo>()
                        .Name("<#= cud #>")
                        .Columns(columns =>
                        {
                            columns.Bound(a => a.posicion).Width(100).Title("Posición").HeaderHtmlAttributes(new { style = "height: 40px" });
                            columns.Bound(a => a.negId).Width(100).Title("Negocio");
                            columns.Bound(a => a.codBarras).Width(100).Title("Codigo de Barras");
                            columns.Bound(a => a.paginas).Width(100).Title("Páginas");
                            columns.Bound(a => a.fecha).Width(100).Title("Fecha de Almacenamiento");
                            columns.Bound(a => a.campo1).Width(100).Title(ViewData["CAMPO_1"].ToString());
                            columns.Bound(a => a.campo2).Width(100).Title(ViewData["CAMPO_2"].ToString());
                            columns.Bound(a => a.campo3).Width(100).Title(ViewData["CAMPO_3"].ToString());
                        })
                        .DataBinding(databinding => databinding.Ajax()
                            .Select("getDetalle", "EnvioBack", new { cud = "<#= cud #>" }))
                        .Pageable(paginas => paginas.PageSize(10))
                        .Sortable()
                        .Filterable()
                        .ToHtmlString()))

                .DataBinding(d => d.Ajax().Select("getInformacionEnvio", "EnvioBack", new { nitCliente = Session["nit_cliente"] }))
                .Pageable(paginas => paginas.PageSize(10))
                .Sortable()
                .Filterable()
                .ClientEvents(e => e.OnCommand("alerta"))
        %>
    </fieldset>

    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
</asp:Content>
