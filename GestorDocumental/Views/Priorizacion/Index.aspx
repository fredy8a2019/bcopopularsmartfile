<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/responsive Menu.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Priorizacion
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Content/Scripts/Page/_ui.js" type="text/javascript"></script>
    <style type="text/css">
        /*form div {
            margin-top: 2%;
            height: 15%;
            margin-bottom: 25px;
        }*/

        /*fieldset {
            width: 35%;
            height: 100%;
        }*/

        /*textarea {
            height: 75%;
            width: 85%;
            margin-bottom: 25px;
        }*/

        /*input[type='submit'] {
            margin-left: 15%;
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <h2>Priorización</h2>

    <form action="/Priorizacion/Priorizar" method="post">
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Opciones</legend>
            <div class="span3">
                <fieldset class="scheduler-border" id="contNegocios">
                    <legend class="scheduler-border">
                        <input type="checkbox" id="chkNegocios" name="name" value="" checked="checked" />
                        <label>Negocios</label>
                    </legend>
                    <textarea name="txtNegocios"></textarea>
                </fieldset>
            </div>
            <div class="span3">
                <fieldset id="contCodBarras" disabled="disabled" class="scheduler-border">
                    <legend class="scheduler-border">
                        <input type="checkbox" id="chkCodBarras" name="name" value="" />
                        <label>Codigos de barras</label>
                    </legend>
                    <textarea name="txtCodBarras"></textarea>
                </fieldset>
            </div>
            <div class="span3">
                <fieldset id="contLote" disabled="disabled" class="scheduler-border">
                    <legend class="scheduler-border">
                        <input type="checkbox" id="chkLotes" name="name" value="" />
                        <label>Lotes</label>
                    </legend>
                    <textarea name="txtLote"></textarea>
                </fieldset>
            </div>
            <br />
        </fieldset>
        <div>
            <input type="submit" name="name" id="btnPriorizar" value=":: Priorizar ::" class="btn btn-login" />
        </div>
    </form>

    <%--<form action="/Priorizacion/Priorizar" method="post">
        <div>
            <fieldset class="scheduler-border" id="contNegocios">
                <legend class="scheduler-border">
                    <input type="checkbox" id="chkNegocios" name="name" value="" checked="checked" />
                    <label>
                        Negocios</label>
                </legend>
                <textarea name="txtNegocios"></textarea>
            </fieldset>
        </div>
        <div>
            <fieldset id="contCodBarras" disabled="disabled" class="scheduler-border">
                <legend class="scheduler-border">
                    <input type="checkbox" id="chkCodBarras" name="name" value="" />
                    <label>
                        Codigos de barras</label>
                </legend>
                <textarea name="txtCodBarras"></textarea>
            </fieldset>
        </div>
        <div>
            <fieldset id="contLote" disabled="disabled" class="scheduler-border">
                <legend class="scheduler-border">
                    <input type="checkbox" id="chkLotes" name="name" value="" />
                    <label>
                        Lotes</label>
                </legend>
                <textarea name="txtLote"></textarea>
            </fieldset>
        </div>
        <input type="submit" name="name" id="btnPriorizar" value=":: Priorizar ::" class="btn-login" />
    </form>--%>

    <script type="text/javascript">
        (function () {
            var _default = {

                _accionCheck: function (e) {
                    var chek = e.currentTarget.checked;
                    if (chek == true) {
                        var idCampo = e.currentTarget.id;
                        switch (idCampo) {
                            case "chkNegocios":
                                $("#contNegocios").removeAttr("disabled");
                                $("#contCodBarras").attr("disabled", "disabled");
                                $("#contLote").attr("disabled", "disabled");

                                $("#chkCodBarras").removeAttr("checked", "checked");
                                $("#chkLotes").removeAttr("checked", "checked");

                                $("textarea[name='txtCodBarras']").val("");
                                $("textarea[name='txtLote']").val("");

                                break;
                            case "chkCodBarras":
                                $("#contCodBarras").removeAttr("disabled");
                                $("#contNegocios").attr("disabled", "disabled");
                                $("#contLote").attr("disabled", "disabled");

                                $("#chkNegocios").removeAttr("checked", "checked");
                                $("#chkLotes").removeAttr("checked", "checked");

                                $("textarea[name='txtNegocios']").val("");
                                $("textarea[name='txtLote']").val("");
                                break;
                            case "chkLotes":
                                $("#contLote").removeAttr("disabled");
                                $("#contNegocios").attr("disabled", "disabled");
                                $("#contCodBarras").attr("disabled", "disabled");

                                $("#chkNegocios").removeAttr("checked", "checked");
                                $("#chkCodBarras").removeAttr("checked", "checked");

                                $("textarea[name='txtNegocios']").val("");
                                $("textarea[name='txtCodBarras']").val("");
                                break;
                        }
                    } else {
                        var idCampo = e.currentTarget.id;
                        switch (idCampo) {
                            case "chkNegocios":
                                $("#contNegocios").attr("disabled", "disabled");

                                $("textarea[name='txtNegocios']").val("");
                                $("textarea[name='txtCodBarras']").val("");
                                $("textarea[name='txtLote']").val("");
                                break;
                            case "chkCodBarras":
                                $("#contCodBarras").attr("disabled", "disabled");
                                $("textarea[name='txtNegocios']").val("");
                                $("textarea[name='txtCodBarras']").val("");
                                $("textarea[name='txtLote']").val("");
                                break;
                            case "chkLotes":
                                $("#contLote").attr("disabled", "disabled");
                                $("textarea[name='txtNegocios']").val("");
                                $("textarea[name='txtCodBarras']").val("");
                                $("textarea[name='txtLote']").val("");
                                break;
                        }
                    }
                },

                _loagPage: function () {
                    _ui.eventClick($("#chkNegocios"), _default._accionCheck);
                    _ui.eventClick($("#chkCodBarras"), _default._accionCheck);
                    _ui.eventClick($("#chkLotes"), _default._accionCheck);
                }
            }

            _default._loagPage();
        })();
    </script>
</asp:Content>