<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Archivar UD
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
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

    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/themes/base/jquery-ui.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>

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

        .dialogEnvios .modal-dialog .modal-content {
            width: 550px;
            height: 350px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        //para que solo ingrese letras
        function soloLetras(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
            especiales = [8, 37, 39, 46];

            tecla_especial = false
            for (var i in especiales) {
                if (key == especiales[i]) {
                    tecla_especial = true;
                    break;
                }
            }
            if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                return false;
            }
        }
        //Para que solo ingrese numeros
        function numbersonly(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode != 8 && unicode != 45 && unicode != 9) {
                if (unicode < 43 || unicode > 57) //if not a number
                { return false } //disable key press    
            }
        }

        function validarUD() {
            var codUD = $("#txtCUD").val();
            transact.ajaxPOST("/Archivo/validaUD?_codUD=" + codUD, null,
                function (data) {
                    if (data == 1) {
                        bootbox.dialog({
                            message: "<h4>La Unidad Documental Nro. " + codUD + " ya ha sido archivada</h4>",
                            title: "<b>Confirmación</b>",
                            buttons: {
                                success: {
                                    label: ":: Aceptar ::",
                                    className: "btn btn-login",
                                    callback: function () {
                                        location.href = "/Archivo/Archivo";
                                    }
                                }
                            }
                        });
                    }
                    else if (data == 0) {
                        refrescaGrillaUDArch();
                    }
                    else {

                    }
                }, function (error) { console.log(error); });
        }

        function refrescaGrillaUDArch() {
            $("#GrillaDatosUD.t-grid .t-refresh").trigger('click');
            var _CUD = $("#txtCUD").val();
            if (_CUD == "")
                _CUD = 0
            else
                _CUD = $("#txtCUD").val();
            var link = "/Archivo/getConsulCUDArchivo?_cud=" + _CUD;
            $.ajax({
                type: "POST",
                url: link,
                data: { dato: _CUD },
                datatype: "json",
                success: function (result) {
                    if (result.total == 0) {
                        var codUD = $("#txtCUD").val();
                        $("#lbl_Label").text("");
                        bootbox.dialog({
                            message: "<h4>La Unidad Documental Nro. " + codUD + " no existe o no se encuentra disponible</h4>",
                            title: "<b>Confirmación</b>",
                            buttons: {
                                success: {
                                    label: ":: Aceptar ::",
                                    className: "btn-danger",
                                    callback: function () {
                                        $("#txtCUD").val("");
                                        //limpia la grilla dejandola en blanco
                                        $("#GrillaDatosUD").data('tGrid').dataBind();
                                    }
                                }
                            }
                        });
                    }
                    else {
                        obtenerDestinoCampo();
                        $("#GrillaDatosUD").data("tGrid").dataBind(result.data);
                    }
                },
                error: function (result) {
                    alert(result.message);
                }

            });
        }

        function obtenerDestinoCampo()
        {
            var CUD = $("#txtCUD").val();
            transact.ajaxGET("/Almacenar/_getDataCUD?numeroCud=" + CUD, null,
                function (data) {
                    $("#lbl_Label").text(" - Destino: " + data[0].Destino);
                },
                function (error) {
                    console.log(error);
                });
        }


        function validaCampo() {
            if ($("#txtCUD").val() == null || $("#txtCUD").val() == "") {
                $("#txtCUD").focus();
                //utilizar mensaje con bootstrap
                return false;
            } else {
                validarUD();
            }
        }
        function limpiaCombos() {
            $("#sleModulo").empty();
            $("#sleNivel").empty();
            $("#slePosicion").empty();
        }
        function alerta(e) {
            var CUDseleccionado = e.dataItem["cud"];
            bootbox.dialog({
                message: "<h4>¿Confirmar el archivo de la Unidad Documental Seleccionada?</h4>",
                title: "<b>Confirmación</b>",
                buttons: {
                    success: {
                        label: ".: Si :.",
                        className: "btn btn-login",
                        callback: function () {
                            //siguiente funcion a donde se dirije en caso de dar clic en SI
                            procArchivo();
                            frmEnvioArchivo();
                        }
                    },
                    danger: {
                        label: ".: No :.",
                        className: "btn-danger"
                    }
                }
            });
        }
        function frmEnvioArchivo() {
            //funcion que llama la ventana modal con el formulario
            //que contiene los datos de la UD a archivar
            bootbox.dialog({
                className: "dialogEnvios",
                title: "<b>Formato de Archivo</b>",
                message: $("#plantilla").html()
            });

        }
        function procArchivo() {
            var _cud = $("#txtCUD").val();
            transact.ajaxGET("/Archivo/InsertEstadoArchivo?_cud=" + _cud, null,
                function (data) {
                    var dateString = data[0].Fecha_CreacionUD.substr(6);
                    var currentTime = new Date(parseInt(dateString));
                    var month = currentTime.getMonth() + 1;
                    var day = currentTime.getDate();
                    var year = currentTime.getFullYear();
                    var date = day + "/" + month + "/" + year;
                    //carga los campos de la ventana modal
                    $("#txtNroCUD").val(data[0].CUD);
                    $("#txtCantDocs").val(data[0].Cant_Docs);
                    $("#txtCliente").val(data[0].Cliente);
                    $("#txtFecUD").val(date);
                    $("#txtOficina").val(data[0].Oficina);
                    //carga las listas desplegables
                    transact.ajaxPOST("/Archivo/listaArchivos", null, function (data) {
                        _ui.fillCombo($("#sleArchivo"), data);
                        cargaModulos();
                    }, function (error) {
                        console.log(error);
                    });
                },
                function (error) { console.log(error) });
        }
        function cargaModulos() {
            $("#sleArchivo").live("change", function () {
                var vlrCombo = $("#sleArchivo option:selected").text();
                transact.ajaxPOST("/Archivo/listaModulos?_codArchivo=" + vlrCombo, null, function (data) {
                    limpiaCombos();
                    _ui.fillCombo($("#sleModulo"), data);
                    cargaNiveles();
                }, function (error) {
                    console.log(error);
                });
            });
        }
        function cargaNiveles() {
            $("#sleModulo").live("change", function () {
                var vlrComboN = $("#sleModulo option:selected").text();
                transact.ajaxPOST("/Archivo/listaNiveles?_codModulo=" + vlrComboN, null, function (data) {
                    $("#sleNivel").empty();
                    _ui.fillCombo($("#sleNivel"), data);
                    cargaSubNiveles();
                }, function (error) {
                    console.log(error);
                });
            });
        }
        function cargaSubNiveles() {
            $("#sleNivel").live("change", function () {
                var vlrComboM = $("#sleModulo option:selected").text();
                var vlrComboN = $("#sleNivel option:selected").text();
                transact.ajaxPOST("/Archivo/listaPosic?_codModulo=" + vlrComboM + "&_nivel=" + vlrComboN, null, function (data) {
                    $("#slePosicion").empty();
                    _ui.fillCombo($("#slePosicion"), data);
                }, function (error) {
                    console.log(error);
                });
            });
        }
        function borraProcArchivo() {
            var codUD = $("#txtNroCUD").val();
            transact.ajaxPOST("/Archivo/borraProcArchivo?_codUD=" + codUD, null,
                function () {
                    location.href = "/Archivo/Archivo";
                }, function (error) { console.log(error); });
        }
        function finProcArchivo() {
            var codUD = $("#txtCUD").val();
            var vlrComboM = $("#sleModulo option:selected").text();
            var vlrComboN = $("#sleNivel option:selected").text();
            var vlrComboSN = $("#slePosicion option:selected").text();
            transact.ajaxPOST("/Archivo/finProcArchivo?_codUD=" + codUD + "&_codModulo=" + vlrComboM + "&_codNivel=" + vlrComboN + "&_codSNivel=" + vlrComboSN, null,
                function () {
                    location.href = "/Archivo/Confirmar?codUD=" + codUD + "&vlrComboM=" + vlrComboM + "&vlrComboN=" + vlrComboN + "&vlrComboSN=" + vlrComboSN;
                },
                function (error) { console.log(error); });
        }

        $(document).ready(function () {
            $("#sleArchivo").attr('required', 'required');
        })

    </script>
    <h2>Archivar Unidad Documental</h2>
    <fieldset class="scheduler-border">
        <legend class="scheduler-border">
            Datos <label id="lbl_Label" style="color:#6785C1; font-size:x-large;"></label>
        </legend>
        <div>
            <label>Número CUD:</label>&nbsp;&nbsp;&nbsp;
            <input type="text" style="width: 300px;" id="txtCUD" required="required" onkeypress="return numbersonly(event)" />&nbsp;&nbsp;&nbsp;
            <input type="button" id="btnBuscarCUD" value=".: Buscar :." class="btn btn-login" onclick="validaCampo()" />
        </div>
        <br />
        <%= Html.Telerik().StyleSheetRegistrar()
           .DefaultGroup(group => group.Add("telerik.hay.css")) %>
        <%= Html.Telerik().Grid<GestorDocumental.Controllers.grilla_ConsultaCUD>()
                .Name("GrillaDatosUD")
                .Columns(columns =>
                {
                    columns.Bound(o => o._cud).Width(100).Title("CUD").HeaderHtmlAttributes(new { style = "center: 40px; height: 40px" });
                    columns.Bound(o => o._estado).Width(100).Title("Estado");
                    columns.Bound(o => o._cliente).Width(100).Title("Cliente");
                    columns.Bound(o => o._oficina).Width(100).Title("Oficina");
                    columns.Bound(o => o._fecTermina).Width(100).Format("{0:F}").Title("Fecha de Cierre");
                    columns.Bound(o => o._nomUsuario).Width(100).Title("Usuario Final");
                    columns.Command(o => o.Custom("Archivar").Text("Archivar")
                        .SendState(false)
                        .Ajax(true)).Width(100);
                })
                .Sortable()
                .Filterable()
                .ClientEvents(e => e.OnCommand("alerta"))
        %>
    </fieldset>

    <script type="text/template" id="plantilla">
        <h4>Archivar Unidad Documental:</h4>
        <form action='Archivo/archivaUD' method='post'>

            <table style="width: 520px;">
                <tr>
                    <td>
                        <label>CUD:</label>
                    </td>
                    <td>
                        <input type='text' style='width: 50px; background-color: #D2C9C9' id='txtNroCUD' readonly='readonly' />
                    </td>
                    <td>
                        <label>Cant. Docs:</label>
                    </td>
                    <td>
                        <input type='text' style='width: 50px; background-color: #D2C9C9' id='txtCantDocs' readonly='readonly' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Cliente:</label>
                    </td>
                    <td>
                        <input type='text' style='width: 150px; background-color: #D2C9C9' id='txtCliente' readonly='readonly' />
                    </td>
                    <td>
                        <label>Oficina:</label>
                    </td>
                    <td>
                        <input type='text' style='width: 150px; background-color: #D2C9C9' id='txtOficina' readonly='readonly' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Fecha creación UD:</label>
                    </td>
                    <td>
                        <input type='text' style='width: 150px; background-color: #D2C9C9' id='txtFecUD' readonly='readonly' />
                    </td>
                </tr>
            </table>
            <table style="width: 400px;">
                <tr>
                    <td>
                        <label>Archivo:</label>
                    </td>
                    <td>
                        <select id="sleArchivo" name="selectArchivo" data-work="ArchivoAlm" required="required"></select>
                    </td>
                    <td>
                        <label>Módulo:</label>
                    </td>
                    <td>
                        <select id="sleModulo" name="selectModulo" data-work="ArchivoAlm" required="required"></select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Nivel:</label>
                    </td>
                    <td>
                        <select id="sleNivel" name="selectNivel" data-work="ArchivoAlm" required="required"></select>
                    </td>
                    <td>
                        <label>Posición:</label>
                    </td>
                    <td>
                        <select id="slePosicion" name="selectPosicion" data-work="ArchivoAlm" required="required"></select>
                    </td>
                </tr>
            </table>
            <br />
            <input type="submit" id='btnArchivarUD' value='.: Archivar :.' class='btn btn-login' onclick="finProcArchivo()" />&nbsp;&nbsp;&nbsp;&nbsp;
            <input type='button' id='btnCancelar' value='.: Cancelar :.' class='btn btn-login' onclick="borraProcArchivo()" />
        </form>
    </script>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
</asp:Content>