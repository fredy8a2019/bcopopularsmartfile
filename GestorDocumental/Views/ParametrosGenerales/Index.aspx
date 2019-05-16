<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<GestorDocumental.Models.Parametros>>" %>

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

        legend.scheduler-border {
            font-size: 1.2em !important;
            font-weight: bold !important;
            text-align: left !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Parametros Generales
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        function cerrarVentanas() {
            bootbox.hideAll();
        }

        function nuevoParametro() {
            var formularioNuevo =
                "<form action='/ParametrosGenerales/insertarParametro' method='POST'>" +
                    "<label class='control-label' for='txtCodigo'>Codigo</label>" +
                    "<input type='text' required='true' id='txtCodigo' name='txtCodigo' class='form-control' style='width:250px' />" +

                    "<label class='control-label' for='txtValor'>Valor</label>" +
                    "<input type='text' required='true' id='txtValor' name='txtValor' class='form-control' style='width:250px' />" +

                    "<label class='control-label' for='txtDescripcion'>Descripción</label>" +
                    "<textarea required='true' id='txtDescripcion' name='txtDescripcion' class='form-control' style='width:500px'></textarea>" +

                "<br>" +
                "<input type='submit' class='btn btn-login' id='btnGuardar' value=':: Guardar ::' />" +
                "<input type='button' class='btn btn-danger' id='btnCancelar' value=':: Cancelar ::' onclick='cerrarVentanas()' />" +
                "</form>";

            bootbox.dialog({
                title: "<b>Ingresar nuevo Parametro</b>",
                message: formularioNuevo,
            });
        }

        function EditarParametro(e) {

            var _id = e.dataItem["id"];
            var _codigo = e.dataItem["Codigo"];
            var _valor = e.dataItem["Valor"];
            var _descripcion = e.dataItem["Descripcion"];

            var formularioEditar =
                "<form action='/ParametrosGenerales/editarInformacion' method='POST'>" +
                    "<label class='control-label' for='txtID'>ID</label>" +
                    "<input type='text' id='txtID' name='txtID' readonly='true' class='form-control' value='" + _id + "' style='width:250px' />" +

                    "<label class='control-label' for='txtCodigo'>Codigo</label>" +
                    "<input type='text' required='true' id='txtCodigo' name='txtCodigo' class='form-control' value='" + _codigo + "' style='width:250px' />" +

                    "<label class='control-label' for='txtValor'>Valor</label>" +
                    "<input type='text' required='true' id='txtValor' name='txtValor' class='form-control' value='" + _valor + "' style='width:250px' />" +

                    "<label class='control-label' for='txtDescripcion'>Descripción</label>" +
                    "<textarea required='true' id='txtDescripcion' name='txtDescripcion' class='form-control' style='width:500px'>" + _descripcion + "</textarea>" +

                "<br>" +
                "<input type='submit' class='btn btn-login' id='btnGuardar' value=':: Guardar ::' />" +
                "<input type='button' class='btn btn-danger' id='btnCancelar' value=':: Cancelar ::' onclick='cerrarVentanas()' />" +
                "</form>";

            bootbox.dialog({
                title: "<b>Editar Información</b>",
                message: formularioEditar,
            });
        }
    </script>


    <fieldset class="scheduler-border">
        <legend>Parametros Generales</legend>

        <input type="button" class='btn btn-login' id='btnGuardar' value=':: Nuevo Parametro ::' onclick="nuevoParametro()" />
        <br />
        <br />

        <%= Html.Telerik().StyleSheetRegistrar()
                .DefaultGroup(group => group.Add("telerik.hay.css")) %>

        <%= Html.Telerik().Grid<GestorDocumental.Controllers.grillaParametrosGenerales>()
                .Name("Grilla_ParametrosGenerales")
                .DataBinding(d => d.Ajax().Select("getInfoParametros", "ParametrosGenerales"))
                .Columns(columns => {
                    columns.Bound(o => o.id).Width(100).Title("Id").HeaderHtmlAttributes(new { style = "height: 40px" });
                    columns.Bound(o => o.Codigo).Width(100).Title("Código");
                    columns.Bound(o => o.Valor).Width(100).Title("Valor");
                    columns.Bound(o => o.Descripcion).Width(100).Title("Descripción");
                    columns.Command(o => o.Custom("Editar").Text("Editar")
                        .SendState(false)
                        .Ajax(true)).Width(100);
                })
                .ClientEvents(e => e.OnCommand("EditarParametro"))
                .Pageable(pager => pager.PageSize(10, new int[] { 10,25,50 })
                            .Style(GridPagerStyles.NextPreviousAndNumeric | GridPagerStyles.PageSizeDropDown))
                        .Sortable()
                        .Filterable()
        %>
    </fieldset>

    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>

</asp:Content>


