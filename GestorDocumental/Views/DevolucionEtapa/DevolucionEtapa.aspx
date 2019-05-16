<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Devolución Etapa
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

        <h2>Devolución Etapa</h2>
        <div class="row">
            <div class="col-lg-1" style="margin: 5px 0px 0px 5px; width: 10%">
                <label>Negocio:</label>
            </div>
            <div class="col-lg-2" style="margin: 5px 0px 0px 5px">
                <input type='text' id='txtNegocio' name='txtNegocio' class="form-control" autofocus style="width: 150px; height: 35px; " onkeypress="return valida(event)" />
            </div>
            <div class="col-lg-3" style="margin: 5px 0px 0px 5px; width: 6%">
                <label>Modulo:</label>
            </div>

            <div class="col-lg-4" style="margin: 5px 0px 0px 5px; width: 16%">
                <select id='txtEtapa' name='txtEtapa' class="form-control" style="width: 150px; height: 35px;">
                    <option value="-1">Seleccione</option>
                    <option value="20">Indexación</option>
                    <option value="30">Captura Uno</option>
                    <option value="40">Captura Dos</option>
                </select>
            </div>

            <div class="col-lg-4" style="margin: 5px 0px 0px 5px">
                <button class="btn btn-login" style='width: 150px; height: 34px;' id="btn_BuscarNeg" name="btn_BuscarNeg" onclick="deovlverEtapa()">
                    <span class="glyphicon glyphicon-fast-backward" aria-hidden="true"></span>&nbsp;Devolver etapa 
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
    </div>

    <script>

        function deovlverEtapa() {
            var negocio = document.getElementById("txtNegocio").value;
            var etapa = document.getElementById("txtEtapa").value;
            if (negocio == "" || etapa==-1) {
                Alternar(divError);
                return false;
            } 
            transact.ajaxPOST("/DevolucionEtapa/DevolverCaso?negocio=" + negocio + "&etapa=" + etapa, null,
                function (data) {
                    var snExt = parseInt(data[0]);

                    if (snExt == 1) {
                        var usuario = data[2]
                        var nometapa = document.getElementById("txtEtapa").textContent;
                        bootbox.dialog({
                            message: "<h4>El caso ya fue devuelto a la etapa" + nometapa + " y esta asignado al usuario:" + usuario + " </h4>",
                            title: "Confirmación",
                            buttons: {
                                success: {
                                    label: ":: Aceptar ::",
                                    className: "btn-success",
                                    callback: function () {
                                        window.location.href = '/DevolucionEtapa/DevolucionEtapa';
                                    }
                                }
                            }
                        });
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

    </script>
</asp:Content>
