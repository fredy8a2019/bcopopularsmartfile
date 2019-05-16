<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Creación de Archivos
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
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

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var consec = 1;
        //Para que solo ingrese numeros
        function numbersonly(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode != 8 && unicode != 45 && unicode != 9) {
                if (unicode < 43 || unicode > 57) //if not a number
                { return false } //disable key press    
            }
        }
        var paginaCrearArchivo = null;
        (function () {
            paginaCrearArchivo = {
                loadPageCargarArchivo: function () {
                    $(document).ready(function () {
                        //$.ajax({
                        //    "url": "/CrearArchivo/GenArchivo",
                        //    "type": "GET",
                        //    "data": "",
                        //    "dataType": "json",
                        //    "success": function (data) {
                        //        $("#txtCodArchivo").val(data);
                        //    },
                        //    "error": function (error) {
                        //        console.log(error);
                        //    }
                        //});
                        transact.ajaxGET("/CrearArchivo/GenArchivo", null, function (data) {
                            $("#txtCodArchivo").val(data);
                            //se llama funcion que genera el cod del modulo inicial
                            paginaCrearArchivo.GenCodModulo(data);
                        }, function (error) {
                            console.log(error);
                        });

                        _ui.eventClick($("#btnAgregarAGrid"), paginaCrearArchivo.btnAgregarGrid);
                        _ui.eventClick($("#btnAceptar"), paginaCrearArchivo.btnAceptarClick);
                    });
                },

                //obtener valores para insertar los datos del modulo
                btnAgregarGrid: function () {

                    var nomArchivo = $("#txtNomArchivo").val();

                    if ($("#txtNomModulo").val() == null || $("#txtNomModulo").val() == "") {
                        $("#txtNomModulo").focus();
                        return false;
                    }
                    if ($("#txtCantNiveles").val() == null || $("#txtCantNiveles").val() == "") {
                        $("#txtCantNiveles").focus();
                        return false;
                    }
                    if ($("#txtSubNiveles").val() == null || $("#txtSubNiveles").val() == "") {
                        $("#txtSubNiveles").focus();
                        return false;
                    }
                    var codArchivo = $("#txtCodArchivo").val();
                    var codModulo = $("#txtCodModulo").val();

                    var nomModulo = $("#txtNomModulo").val();
                    var cantNiveles = $("#txtCantNiveles").val();
                    var cantSubN = $("#txtSubNiveles").val();

                    transact.ajaxPOST("/CrearArchivo/InstModulos?codArchivo=" + codArchivo + "&codModulo=" + codModulo + "&nomModulo=" + nomModulo + "&cantNiveles=" + cantNiveles + "&cantSubN=" + cantSubN + "&nomArchivo=" + nomArchivo, null, function () { }, function (error) { console.log(error) });
                    location.href = "/CrearArchivo/CrearArchivo";
                    refrescaGrillaMod();
                    actualizarConsecutivoM();
                    limpiarCampos();

                },

                btnAceptarClick: function () {
                    if ($("#txtNomArchivo").val() == null || $("#txtNomArchivo").val() == "") {
                        $("#txtNomArchivo").focus();
                        return false;
                    }
                    var codArchivo = $("#txtCodArchivo").val();
                    var nomArchivo = $("#txtNomArchivo").val();
                    transact.ajaxPOST("/CrearArchivo/InstDatArchivo?_codArchivo=" + codArchivo + "&_nomArchivo=" + nomArchivo, null,
                        function () {
                            bootbox.dialog({
                                message: "El archivo No." + codArchivo + " ha sido creado correctamente",
                                title: "<b>Confirmación</b>",
                                buttons: {
                                    success: {
                                        label: ":: Aceptar ::",
                                        className: "btn btn-login",
                                        callback: function () {
                                            location.href = "/CrearArchivo/CrearArchivo";
                                        }
                                    }
                                }
                            });
                        },
                        function (error) {
                            console.log(error);
                        });
                },

                //funcion que genera el codigo del modulo partiendo del cod archivo
                GenCodModulo: function (codArchivo) {
                    var txtCodArchivo = $("#txtCodArchivo").val();
                    transact.ajaxGET("/CrearArchivo/GenCodModulo?_codArchivo=" + txtCodArchivo, null, function (data) {
                        $("#txtCodModulo").val(data);
                        refrescaGrillaMod();
                    }, function (error) {
                        console.log(error);
                    });
                }
            }
            paginaCrearArchivo.loadPageCargarArchivo();

            function refrescaGrillaMod() {
                var _codARCHIVO = $("#txtCodArchivo").val();
                var _codMODULO = $("#txtCodModulo").val();

                var link = "/CrearArchivo/getDetalleM?_codArchivo=" + _codARCHIVO + "&_codModulo=" + _codMODULO;
                $.ajax({
                    type: "POST",
                    url: link,
                    data: { dato: _codARCHIVO / _codMODULO },
                    dataType: "json",
                    success: function (result) {
                        $("#GrillaArchivoModulo").data("tGrid").dataBind(result.data);
                    },
                    error: function (result) {
                        alert(result.message);
                    }
                })
                $("#GrillaArchivoModulo.t-grid .t-refresh").trigger('click');
            }

            function actualizarConsecutivoM() {
                var codArchivo = $("#txtCodArchivo").val();
                consec = consec + 1;
                $("#txtCodModulo").val(codArchivo.toString() + consec);
            }
            function limpiarCampos() {
                $("#txtNomArchivo").val("");
                $("#txtNomModulo").val("");
                $("#txtCantNiveles").val("");
                $("#txtSubNiveles").val("");
            }

        })();
    </script>
    <br />
    <h2>Crear Archivo</h2>
    <br />
    <fieldset class="scheduler-border">
        
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Archivo</legend>
            <div>
                <label>Código del Archivo:</label>&nbsp;&nbsp;
                <input type="text" style="width: 150px; background-color: #D2C9C9" id="txtCodArchivo" name="txtCodArchivo" readonly="readonly" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <label>Nombre del Archivo:</label>&nbsp;&nbsp;
                <input type="text" style="width: 300px;" id="txtNomArchivo" name="txtNomArchivo" required="required" value="<%= ViewData["nomArchivo"].ToString() %>" />
            </div>
        </fieldset>
        <br />
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Módulo</legend>
            <div>
                <table style="width: 900px;">
                    <tr>
                        <td>
                            <label>Código del Módulo:</label>&nbsp;&nbsp;
                            <input type="text" style="width: 150px; background-color: #D2C9C9" id="txtCodModulo" readonly="readonly" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <label>Nombre del Módulo:</label>&nbsp;&nbsp;
                            <input type="text" style="width: 300px;" id="txtNomModulo" required="required" /><br />

                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <label>Niveles:</label>&nbsp;&nbsp;
                            <input type="text" style="width: 50px;" id="txtCantNiveles" onkeypress="return numbersonly(event);" maxlength="3" required="required" />
                        </td>
                        <td>
                            <label>Sub Niveles:</label>&nbsp;&nbsp;
                            <input type="text" style="width: 50px;" id="txtSubNiveles" maxlength="3" onkeypress="return numbersonly(event);" required="required" />
                        </td>
                        <td style="align-content">
                            <input type="button" id="btnAgregarAGrid" value=".: Agregar :." class="btn btn-login" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <%= Html.Telerik().StyleSheetRegistrar()
                .DefaultGroup(group => group.Add("telerik.hay.css")) %>
            <%= Html.Telerik().Grid<GestorDocumental.Controllers.LlenarGrilla>()
                .Name("GrillaArchivoModulo")
                .Columns(columns =>
                {
                    columns.Bound(o => o._codModulo).Width(100).Title("Cód. Módulo").HeaderHtmlAttributes(new { style = "height: 40px" });
                    columns.Bound(o => o._nomModulo).Width(100).Title("Nombre Módulo");
                    columns.Bound(o => o._cantN).Width(100).Title("Cant. Niveles");
                    columns.Bound(o => o._cantSN).Width(100).Title("Cant. subNiveles");
                })
                .Pageable(paginas => paginas.PageSize(10))
                .Sortable()
                .Filterable()
            %>
        </fieldset>
        <br />
        <br />
        <input type="submit" id="btnAceptar" value=".: Crear Archivo :." class="btn btn-login" />
    </fieldset>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
</asp:Content>
