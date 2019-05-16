<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html>

<html>
<head runat="server">

    <%--<link href="https://cdn.datatables.net/1.10.11/css/jquery.dataTables.min.css" rel="stylesheet" />--%>
    <%--<link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/css/jquery.dataTables.css">--%>
    <link href="../../Styles/JQueryUiBase/jquery.dataTables.css" rel="stylesheet" />
    <script src="../../Content/Scripts/ui/jquery-1.10.1.min.js" type="text/javascript"></script>
    <script src="../../Styles/BootsTrap/js/bootstrap.min.js"></script>
    <%--<link rel="stylesheet" href="http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css">--%>
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
    <script src="../../Scripts/Reestricciones.js" type="text/javascript"></script>
    <script src="../../Styles/BootsTrap/js/bootbox.js" type="text/javascript"></script>

    <%-- <link href="../../Styles/BootsTrap/css/dataTables.bootstrap.css" rel="stylesheet" />--%>

    <style type="text/css">
        body {
            background-color: white;
        }

        tb {
            margin-right: 5px;
        }

        h2 {
            font-size: 30px;
            background-color: white;
        }

        input[type="submit"] {
            -webkit-appearance: button;
            cursor: pointer;
        }

        button, html input[type="button"], input[type="reset"], input[type="submit"] {
            -webkit-appearance: button;
            cursor: pointer;
        }

        button, html input[type="button"], input[type="reset"], input[type="submit"] {
            -webkit-appearance: button;
            cursor: pointer;
        }

        .btn {
            display: inline-block;
            margin-bottom: 0;
            font-weight: 400;
            text-align: center;
            vertical-align: middle;
            cursor: pointer;
            background-image: none;
            border: 1px solid transparent;
            white-space: nowrap;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.42857143;
            border-radius: 4px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

        .btn-login {
            color: #ffffff;
            background-color: #9AAE04;
            border-color: #94A132;
        }

        .btn {
            display: inline-block;
            margin-bottom: 0;
            font-weight: 400;
            text-align: center;
            vertical-align: middle;
            cursor: pointer;
            background-image: none;
            border: 1px solid transparent;
            white-space: nowrap;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.428571429;
            border-radius: 4px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            -o-user-select: none;
            user-select: none;
        }

        .btn {
            display: inline-block;
            padding: 6px 12px;
            margin-bottom: 0;
            font-size: 14px;
            font-weight: normal;
            line-height: 1.428571429;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            cursor: pointer;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            -o-user-select: none;
            user-select: none;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 4px;
        }

        input, button, select, textarea {
            font-family: inherit;
            font-size: inherit;
            line-height: inherit;
        }

        input {
            line-height: normal;
        }

        button, input, optgroup, select, textarea {
            color: inherit;
            font: inherit;
            margin: 0;
        }

        input, textarea, .uneditable-input {
            margin-left: 0;
        }

        input, button, select, textarea {
            font-family: inherit;
            font-size: inherit;
            line-height: inherit;
        }

        input {
            line-height: normal;
        }

        button, input, optgroup, select, textarea {
            color: inherit;
            font: inherit;
            margin: 0;
        }

        input, button, select, textarea {
            font-family: inherit;
            font-size: inherit;
            line-height: inherit;
        }

        input {
            line-height: normal;
        }

        button, input, optgroup, select, textarea {
            margin: 0;
            font: inherit;
            color: inherit;
        }

        body {
            font-family: Calibri !important;
        }

        body {
            font-family: Verdana, Arial, Helvetica, sans-serif;
        }

        body {
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
            font-size: 14px;
            line-height: 1.42857143;
            color: #333;
        }

        body {
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
            font-size: 14px;
            line-height: 1.428571429;
            color: #333;
        }

        body {
            font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
            font-size: 14px;
            line-height: 1.428571429;
            color: #333;
        }

        html {
            font-size: 62.5%;
        }

        html {
            font-family: sans-serif;
            -webkit-text-size-adjust: 100%;
        }

        html {
            font-size: 62.5%;
        }

        html {
            font-family: sans-serif;
            -webkit-text-size-adjust: 100%;
        }

        html {
            font-size: 62.5%;
        }

        html {
            font-family: sans-serif;
            -webkit-text-size-adjust: 100%;
        }

        .custom-combobox {
            width: 200%;
            font-size: 70%;
        }

        .alert-danger {
            color: #a94442;
            background-color: #f2dede;
            border-color: #ebccd1;
        }
    </style>


</head>

<body>
    <%-- los fieldset son los 4 cmapos desde FUV hasta cedula internamente tiene una estructura de tablas para mantener alineado los
            elementos que estemos pintando los cuales se toman desde base de datos y se imprimen en pantalla y tomara los datos requeridos para
            realizar las devoluciones cuando no se aprube la face de dactiloscopia --%>
    <form>
        <h2>Politicas de dactiloscopia</h2>

        <%--<input type="button" value="Cargar Causales" onclick="alerta()" />--%>



        <form action="" method>
            <fieldset class="scheduler-border" id="CausalesImg">
                <legend class="scheduler-border">FUV</legend>
                <div id="CausalesFuv">
                </div>
                <br />
                <input type="button" value=".:Actualizar Causales:." onclick="CausalesImagenSN()" />
            </fieldset>
        </form>
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Cedula</legend>
            <div id="CausalesCC">
            </div>
            <br />
            <input type="button" value=":Actualizar Causales:." onclick="CausalesImagenSN3()" />
        </fieldset>
        <fieldset class="scheduler-border" style ="<%=Session["Ocultar"]%>">
            <legend class="scheduler-border">Pagare</legend>
            <div id="CausalesPagare">
            </div>
            <br />
            <input type="button" value=":Actualizar Causales:." onclick="CausalesImagenSN2()" />
        </fieldset>
        <%--<fieldset class="scheduler-border" style ="<%=Session["Ocultar"]%>">
            <legend class="scheduler-border">Carta de instrucciones</legend>
            <div id="CausalesCart">
            </div>
            <br />
            <input type="button" value=":Actualizar Causales:." onclick="CausalesImagenSN1()" />
        </fieldset>--%>

        <fieldset class="scheduler-border" style ="<%=Session["Mostrar"]%>" >
                <legend class="scheduler-border">Pagare & Carta de Instrucciones</legend>
                <div id="CausalesPagareyCarta">
                </div>
                <br/>
                <input type="button" value=".:Actualizar Causales:." onclick="CausalesImagenSN4()"/>
    	</fieldset>

        <%--<fieldset class="scheduler-border" id="CausalesG">
            <legend class="scheduler-border">Generales</legend>

            <div id="CausalesGenerales">
                <table style="width: 330px">
                    <%=ViewData["_cadenaPoliticas"].ToString() %>
                </table>
                <br />
            </div>
            <input type="button" value=".:Actualizar Causales:." onclick="CausalesGeneralesSN()" />
        </fieldset>--%>

        <fieldset>
            <legend class="scheduler-border">Listado de causales</legend>
            <div style="margin-top: 10px">
                <table id="example">
                    <thead>
                        <tr>
                            <th class="site_name">Documento</th>
                            <th>Causal</th>
                            <th>Descripcion</th>

                        </tr>
                    </thead>
                    <tbody id="causales">
                    </tbody>
                </table>
            </div>
        </fieldset>
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Observaciones</legend>
            <input type="text" style="width: 95%" id="txtObservacion" />
            
        </fieldset>
        <fieldset class="scheduler-border">
            
            <legend class="scheduler-border">Historico de Observaciones</legend>
            <%--<input type="text" style="width: 100%; height: 100px" maxlength="250" disabled id="txtHistoricoObservacion" value="<%=ViewData["HistoricoComentarios"]%>" />--%>
          
            <textarea name="comentarios" disabled rows="10" cols="85"><%=ViewData["HistoricoComentarios"]%></textarea>

        </fieldset>

        <div class="alert alert-danger" id="alerta2" style="display: none;">
            <strong>Atención! </strong>Faltan validar causales
        </div>

        <input type="button" value="Guardar" onclick="finValidacion(alerta2)" />

    </form>

    <%--<script type="text/javascript" charset="utf8" src="http://cdn.datatables.net/1.10.11/js/jquery.dataTables.min.js"></script>--%>
    <script type="text/javascript" charset="utf8" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js"></script>
    <script src="../../Scripts/jquery.dataTables.min.js"></script>
    <script>

        var _negId = '<%=ViewData["_negId"]%>';

        var table = $('#example').DataTable({

            destroy: false,
            searching: false,
            entries: false,
            paging: false,
            ordering: false,

            "language": {
                "sProcessing": "<img src='../../Images/Splitter/ajax-loader-green-large.gif' />",
                "sLengthMenu": "Mostrar _MENU_ registros",
                "sZeroRecords": "No se encontraron resultados",
                "sEmptyTable": "Ningún dato disponible en esta tabla",
                "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",

                "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sInfoPostFix": "",
                "sSearch": "Buscar:",
                "sUrl": "",
                "sInfoThousands": ",",
                "sLoadingRecords": "<img src='../../Images/Splitter/ajax-loader-green-large.gif' />",
                "oPaginate": {

                    "sFirst": "Primero",
                    "sLast": "Último",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"

                },

                "oAria": {

                    "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                    "sSortDescending": ": Activar para ordenar la columna de manera descendente"

                }
            }
        });

        $(Document).ready(alerta());


        function alerta() {

            transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 1"]%>', null,
                function (data) {
                    $("#CausalesFuv").html(data);
                }, function (error) { console.log(error) });

            transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 2"]%>', null,
                function (data) {
                    $("#CausalesCC").html(data);
                }, function (error) { console.log(error) });

            if ('<%=Session["Tajeta"]%>' == '2') {
                transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 3"]%>', null,
                function (data) {
                    $("#CausalesPagare").html(data);
                }, function (error) { console.log(error) });

                transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 4"]%>', null,
                function (data) {
                    $("#CausalesCart").html(data);
                }, function (error) { console.log(error) });
            } if ('<%=Session["Tajeta"]%>' == '1') {
                transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 3"]%>', null,
                function (data) {
                    $("#CausalesPagareyCarta").html(data);
                }, function (error) { console.log(error) });
            } if ('<%=Session["Tajeta"]%>' == '3') {
                transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 4"]%>', null,
                function (data) {
                    $("#CausalesPagareyCarta").html(data);
                }, function (error) { console.log(error) });

                transact.ajaxPOST("/Dactiloscopia/generaCampos?_DocId=" + '<%=ViewData["Doc 3"]%>', null,
                function (data) {
                    $("#CausalesPagare").html(data);
                }, function (error) { console.log(error) });
            }
            ActualizarDataTable();
        }
        

        function CausalesGeneralesSN() {
            var valChck;
            var codPolG;
            var contador = 0;
            var validacion = 0;

            $("#CausalesGenerales").find('input[type=radio]:checked').each(function () {
                validacion = validacion + 1;
            });

            if (validacion == 2) {
                $("#CausalesGenerales").find('input[type=radio]:checked').each(function () {
                    var valChck = $(this).val();
                    var valoresHidden = $("#CausalesGenerales").find(':input[type=hidden]');

                    //console.log(_negId, valoresHidden[contador].id, $(this).val());

                    InstCausales(valoresHidden[contador].id, valChck, _negId);
                    contador++;
                });

                ActualizarDataTable();
            } else {
                alert("Validar todas las causales");
            }
        }


        function CausalesImagenSN() {
            var valLst;
            var idSelect;
            var nameSelect;
            var contador = 0;
            var validacion = 0;
            $("#CausalesFuv :input").each(function () {
                idSelect = $(this).attr("id");
                valLst = $("#" + idSelect + " option:selected").val();

                if (valLst == -1) {

                    validacion = 1;

                    alert("Validar todas las causales");
                    return false;
                }

            });
            if (validacion == 0) {
                $("#CausalesFuv :input").each(function () {
                    //se obtiene el name del select 
                    idSelect = $(this).attr("id");
                    nameSelect = document.getElementById(idSelect).name;

                    valLst = $("#" + idSelect + " option:selected").val();
                    //valLst = $(this).val();
                    var _snCausal = valLst;

                    transact.ajaxPOST("/Dactiloscopia/CausalesEspecificas?_campId=" + nameSelect + "&_snCausal=" + _snCausal + "&_negId=" + _negId + "&doc=" + '<%=ViewData["Doc 1"]%>', null,
                    function () {
                       //ActualizarDataTable();
                    },
                    function (error) { console.log(error) });
                });

                ActualizarDataTable();
                
            }
            
        }
        function CausalesImagenSN1() {
            var valLst;
            var idSelect;
            var nameSelect;
            var contador = 0;
            var validacion = 0;
            $("#CausalesCart :input").each(function () {
                idSelect = $(this).attr("id");
                valLst = $("#" + idSelect + " option:selected").val();

                if (valLst == -1) {

                    validacion = 1;
                    alert("Validar todas las causales");
                    return false;
                }

            });
            if (validacion == 0) {
                $("#CausalesCart :input").each(function () {
                    //se obtiene el name del select 
                    idSelect = $(this).attr("id");
                    nameSelect = document.getElementById(idSelect).name;

                    valLst = $("#" + idSelect + " option:selected").val();
                    
                    var _snCausal = valLst;

                    transact.ajaxPOST("/Dactiloscopia/CausalesEspecificas?_campId=" + nameSelect + "&_snCausal=" + _snCausal + "&_negId=" + _negId + "&doc=" + '<%=ViewData["Doc 4"]%>', null,
                    function () {
                        //ActualizarDataTable();
                    },
                    function (error) { console.log(error) });
                });
  
                ActualizarDataTable();
            }
            
        }
        function CausalesImagenSN2() {
            var valLst;
            var idSelect;
            var nameSelect;
            var contador = 0;
            var validacion = 0;
            $("#CausalesPagare :input").each(function () {
                idSelect = $(this).attr("id");
                valLst = $("#" + idSelect + " option:selected").val();

                if (valLst == -1) {

                    validacion = 1;
                    alert("Validar todas las causales");
                    return false;
                }

            });
            if (validacion == 0) {
                $("#CausalesPagare :input").each(function () {
                    //se obtiene el name del select 
                    idSelect = $(this).attr("id");
                    nameSelect = document.getElementById(idSelect).name;

                    valLst = $("#" + idSelect + " option:selected").val();
                    
                    var _snCausal = valLst;

                    transact.ajaxPOST("/Dactiloscopia/CausalesEspecificas?_campId=" + nameSelect + "&_snCausal=" + _snCausal + "&_negId=" + _negId + "&doc=" + '<%=ViewData["Doc 3"]%>', null,
                    function () {
                       //ActualizarDataTable();
                    },
                    function (error) { console.log(error) });
                });
                
                ActualizarDataTable();
            }
            
        }
        function CausalesImagenSN3() {
            var valLst;
            var idSelect;
            var nameSelect;
            var contador = 0;
            var validacion = 0;
            $("#CausalesCC :input").each(function () {
                idSelect = $(this).attr("id");
                valLst = $("#" + idSelect + " option:selected").val();

                if (valLst == -1) {

                    validacion = 1;
                    alert("Validar todas las causales");
                    return false;
                }

            });
            if (validacion == 0) {
                $("#CausalesCC :input").each(function () {
                    //se obtiene el name del select 
                    idSelect = $(this).attr("id");
                    nameSelect = document.getElementById(idSelect).name;

                    valLst = $("#" + idSelect + " option:selected").val();
                    
                    var _snCausal = valLst;

                    transact.ajaxPOST("/Dactiloscopia/CausalesEspecificas?_campId=" + nameSelect + "&_snCausal=" + _snCausal + "&_negId=" + _negId + "&doc=" + '<%=ViewData["Doc 2"]%>', null,
                    function () {
                        //ActualizarDataTable();
                    },
                    function (error) { console.log(error) });
                });

               ActualizarDataTable();

            }
            
        }

        function CausalesImagenSN4() {
            var valLst;
            var idSelect;
            var nameSelect;
            var contador = 0;
            var validacion = 0;
            this.
            $("#CausalesPagareyCarta :input").each(function () {
                idSelect = $(this).attr("id");
                valLst = $("#" + idSelect + " option:selected").val();

                if (valLst == -1) {

                    validacion = 1;
                    alert("Validar todas las causales");
                    return false;
                }

            });
            if (validacion == 0) {
                $("#CausalesPagareyCarta :input").each(function () {
                    //se obtiene el name del select 
                    idSelect = $(this).attr("id");
                    nameSelect = document.getElementById(idSelect).name;

                    valLst = $("#" + idSelect + " option:selected").val();
                    
                    var _snCausal = valLst;
                    if ('<%=Session["Tajeta"]%>' == '3') {
                        transact.ajaxPOST("/Dactiloscopia/CausalesEspecificas?_campId=" + nameSelect + "&_snCausal=" + _snCausal + "&_negId=" + _negId + "&doc=" + '<%=ViewData["Doc 4"]%>', null,
                        function () {
                        //ActualizarDataTable();
                    },
                        function (error) { console.log(error) });
                    }else{
                        transact.ajaxPOST("/Dactiloscopia/CausalesEspecificas?_campId=" + nameSelect + "&_snCausal=" + _snCausal + "&_negId=" + _negId + "&doc=" + '<%=ViewData["Doc 3"]%>', null,
                        function () {
                            //ActualizarDataTable();
                        },
                        function (error) { console.log(error) });
                     }
                });
             
                ActualizarDataTable();
            }
            
        }

        function InstCausales(_codCausal, _snCausal, neg) {
            transact.ajaxPOST("/Dactiloscopia/InsertaCausales?_codCausal=" + _codCausal + "&_snCausal=" + _snCausal + "&_negId=" + _negId, null,
                function () { },
                function (error) { console.log(error) });
        }

        function ActualizarDataTable() {
            var a;
            transact.ajaxPOST("/Dactiloscopia/getDatatable?_negId=" + _negId, null,
               function (data) {
                   $("#causales").html(data);

               },
               function (error) { console.log(error) });
        }



        function finValidacion(Seccion) {

            var valChck = 0;
            var txtObservaciones = $("#txtObservacion").val();

       
            transact.ajaxGET("/Dactiloscopia/snExisteCausal?_negId=" + _negId + "&_snIndx=" + valChck + "&_observaciones=" + txtObservaciones, null,
                function (data) {
                    console.log(data, "data");
                    if (data == 1) {
                        window.close();
                    } else if ((data == 0)) {
                        Alternar(Seccion);
                    }
                },
                function (error) {
                    console.log(error);
                });

        }
        
        function Alternar(Seccion) {
            if (Seccion.style.display == "none") {
                Seccion.style.display = ""
                window.setTimeout(function () {
                    $(".alert-danger").fadeTo(500, 0).slideUp(500, function () {
                        $(this).removeAttr("style");
                        $(this).css("display", "none");
                    });
                }, 3000);
            }
        }

    </script>


</body>
</html>
