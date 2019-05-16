(function ($) {

    var _ui = window._ui = {

        buildCombobox: function () {
            $.widget("custom.combobox", {
                _create: function () {
                    this.wrapper = $("<span>")
                        .addClass("custom-combobox")
                        .insertAfter(this.element);

                    this.element.hide();
                    this._createAutocomplete();
                    this._createShowAllButton();
                },

                _createAutocomplete: function () {
                    var selected = this.element.children(":selected"),
                        value = selected.val() ? selected.text() : "";

                    this.input = $("<input>")
                        .appendTo(this.wrapper)
                        .val(value)
                        .attr("title", "")
                        .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
                        .autocomplete({
                            delay: 0,
                            minLength: 0,
                            source: $.proxy(this, "_source")
                        })
                        .tooltip({
                            tooltipClass: "ui-state-highlight"
                        });

                    this._on(this.input, {
                        autocompleteselect: function (event, ui) {
                            ui.item.option.selected = true;
                            this._trigger("select", event, {
                                item: ui.item.option
                            });
                            _ui.changeSelect(event);
                        },

                        autocompletechange: "_removeIfInvalid"
                    });
                },

                _createShowAllButton: function () {
                    var input = this.input,
                        wasOpen = false;

                    $("<a>")
                        .attr("tabIndex", -1)
                        .attr("title", "Items")
                        .tooltip()
                        .appendTo(this.wrapper)
                        .button({
                            icons: {
                                primary: "ui-icon-triangle-1-s"
                            },
                            text: false
                        })
                        .removeClass("ui-corner-all")
                        .addClass("custom-combobox-toggle ui-corner-right")
                        .mousedown(function () {
                            wasOpen = input.autocomplete("widget").is(":visible");
                        })
                        .click(function () {
                            input.focus();

                            // Close if already visible
                            if (wasOpen) {
                                return;
                            }

                            // Pass empty string as value to search for, displaying all results
                            input.autocomplete("search", "");
                        });
                },

                _source: function (request, response) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
                    response(this.element.children("option").map(function () {
                        var text = $(this).text();
                        if (this.value && (!request.term || matcher.test(text)))
                            return {
                                label: text,
                                value: text,
                                option: this
                            };
                    }));
                },

                _removeIfInvalid: function (event, ui) {
                    // Selected an item, nothing to do
                    if (ui.item) {
                        return;
                    }

                    // Search for a match (case-insensitive)
                    var value = this.input.val(),
                		valueLowerCase = value.toLowerCase(),
                		valid = false;
                    this.element.children("option").each(function () {
                        if ($(this).text().toLowerCase() === valueLowerCase) {
                            this.selected = valid = true;
                            return false;
                        }
                    });

                    // Found a match, nothing to do
                    if (valid) {
                        return;
                    }

                    // Remove invalid value
                    this.input
                		.val("")
                		.attr("title", value + "No selecciona ningun item")
                		.tooltip("open");
                    this.element.val("");
                    this._delay(function () {
                        this.input.tooltip("close").attr("title", "");
                    }, 2500);
                    this.input.data("ui-autocomplete").term = "";
                },

                _destroy: function () {
                    this.wrapper.remove();
                    this.element.show();
                }
            });
        }, // Fin de la funcion

        builDatepicker: function (_control) {
            $(_control).datepicker({
                maxDate: "+0D",
                defaulDate: "+1w",
                changeMounth: true,
                numberOfMonths: 1,
                dateFormat: "dd/mm/yy"
            });
        },

        dateSystem: function () {
            var date = new Date;
            return date.toLocaleDateString();
        },

        createElement: function (element, attr, text) {
            var element = $('<' + element + '>');
            if (attr) element.attr(attr);
            if (text) element.text(text);
            return element;
        },

        fillCombo: function (combo, items) {
            combo = $(combo);
            $.each(items, function (i, item) {
                switch (combo.data("work")) {
                    case "productos":
                        combo.append(_ui.createElement("option", { "value": item.Value }, item.Text));
                        break;
                    case "oficinas":
                        combo.append(_ui.createElement("option", { "value": item.Value }, item.Text));
                        break;
                    case "subProductos":
                        combo.append(_ui.createElement("option", { "value": item.Value }, item.Text));
                        break;
                    case "estados":
                        combo.append(_ui.createElement("option", { "value": item.Value }, item.Text));
                        break;
                    case "mes":
                        combo.append(_ui.createElement("option", { "value": "" }, ""));
                        break;
                    case "clientes":
                        combo.append(_ui.createElement("option", { "value": item.Value }, item.Text));
                        break;
                    case "ArchivoAlm": //JFPancho, se agrega para el  modulo de archivo
                        if (i == 0) {
                            combo.append(_ui.createElement("option", { "value": "" }, "Seleccione..."))
                        }
                        combo.append(_ui.createElement("option", { "value": item.Value }, item.Text));
                        break;
                    default:
                        if (i == 0) {
                            combo.append(_ui.createElement("option", { "value": "-1" }, ""))
                        }
                        combo.append(_ui.createElement("option", { "value": item.Value }, item.Text));
                }

            });
        },

        opSelSubProducto: null,
        opSelTipoUD: null,

        changeSelect: function (e) {
            var _this = $(e.currentTarget);
            var select = $(_this.parent().prev());
            switch (select.data("work")) {
                case "subProductos":
                    _ui.opSelSubProducto = select.val();
                    if (select.data("name") == "Reporte") {
                        $("#hSubProductos").val($("#dllSubProductos option:selected").val());
                    }
                    break;
                case "productos":
                    if (select.data("name") == "Reporte") {
                        transact.ajaxPOST("/Listas/_GetDropDownListSubProductos?DropDownList_Productos=" + select.val(),
                                      null,
                                       _ui._successSubProductosReporte,
                                       _ui.displayError);

                        $("#hProductos").val($("#ddlProductos option:selected").val());
                        $("#dllSubProductos").next().find("input[autocomplete='off']").val("");

                    } else if (select.data("name") == "facturacion") {

                        $("#hProductos").val($("#ddlProductos option:selected").text());

                    } else {
                        transact.ajaxPOST("/Radicacion/_GetDropDownListSubProductos?DropDownList_Grupos=" + select.val(),
                                      null,
                                       _ui._successSubProductos,
                                       _ui.displayError);
                    }
                    break;
                case "estados":
                    if (select.data("name") != "Reporte") {
                        _fx.clearContent($("#tableContent tbody"));
                        var CodOrigen = _ui.opSelSubProducto + select.val();
                        transact.ajaxGET(
                                       "/Radicacion/_GetCampos?CodOrigen=" + CodOrigen,
                                         null,
                                         _ui.SusscesCargaCampos,
                                         _ui.displayError
                                       );

                        if (select.val() != 120) {
                            $("#radicacionVirtual").css("display", "none");
                        }
                        else {
                            if ($("#sleTipo").val() == "virtual") {
                                $("#radicacionVirtual").css("display", "block");
                            }
                        }
                    } else {
                        $("#hEstados").val($("#ddlEstados option:selected").text());
                    }
                    return select.val();
                    break;
                case "tiposRadicacion":

                    if (select.val() == "virtual") {
                        $("#radicacionVirtual").css("display", "block");
                    } else {
                        $("#radicacionVirtual").css("display", "none");
                    }

                    break;
                case "clientes":
                    console.log(select.val());
                    $("#hClientes").val($("#ddlClientes option:selected").text());

                    transact.ajaxPOST("/Listas/_GetDropDownList_Oficinas?DropDownList_Clientes=" + select.val(),
                                      null,
                                       _ui._successOficinas,
                                       _ui.displayError);

                    transact.ajaxPOST("/Listas/_GetDropDownList_Sociedades?DropDownList_Clientes=" + select.val(),
                                      null,
                                       _ui._successSociedad,
                                       _ui.displayError);

                    if (select.data("name") == "facturacion") {

                        transact.ajaxPOST("/Listas/_GetProductosDelNegocio",
                                      null,
                                       _ui._successProductosFacturacion,
                                       _ui.displayError);
                        $("#fechaInicial").removeAttr("autocomplete");
                        $("#fechaFin").removeAttr("autocomplete");

                    } else {
                        transact.ajaxPOST("/Listas/_GetDropDownListProductos?DropDownList_Clientes=" + select.val(),
                                      null,
                                       _ui._successProductos,
                                       _ui.displayError);
                    }

                    $("#ddlClientes").parent().parent().parent().find("input[autocomplete='off']").val("")
                    $("table input[type='checkbox']").removeAttr("checked");
                    $("table input[type='checkbox']").parent().parent().find("td input[type='checkbox']").next().parent().next().find("a").removeAttr("class");
                    $("table input[type='checkbox']").parent().parent().find("td input[type='checkbox']").next().parent().next().find("a span.ui-button-icon-primary.ui-icon.ui-icon-triangle-1-s").removeAttr("class");
                    $("table input[type='checkbox']").parent().next().find("input[autocomplete='off']").attr("disabled", "disabled");
                    $("table input[type='checkbox']").parent().next().find("input[type='text']").attr("disabled", "disabled");
                    break;
                case "oficinas":
                    $("#hOficinas").val($("#ddlOficinas option:selected").text());
                    break;
                case "Sociedad":
                    $("#hSociedad").val($("#ddlSociedad option:selected").text());
                    break;
                case "estadoRecepcion":
                    $("#hEstado").val($("#ddlEstado option:selected").val());
                    break;
                case 5:
                    var valor = select.val();
                    if (valor == "142") {
                        $("#NoDocumentoSAP").removeAttr("required");
                        $("#Observaciones").attr("required", "required");
                    }
                    else if (valor == "140") {
                        $("#NoDocumentoSAP").attr("required", "required");
                        $("#Observaciones").removeAttr("required");
                    }

                    break;
                case "sociedad":
                    var valor = select.val();
                    if (select.data("name") == "facturacion") {
                        $("#hSociedad").val(valor);
                    } else {
                        $("#HiddenSociedad").val(valor);
                    }
                    break;
                case "causal":
                    var valor = select.val();
                    $("#HiddenCausal").val(valor);
                    break;
                case "ProductoAlmacenamiento":

                    transact.ajaxPOST("/Listas/_GetDropDownListSubProductos?DropDownList_Productos=" + select.val(),
                                      null,
                                       _ui._successSubProductosAlmacenamiento,
                                       _ui.displayError);
                    break;
                case "destinoAlmacenamiento":
                    _fx.clearContent($("#tblContentCampos tbody"));
                    var CodOrigen = _ui.opSelTipoUD + select.val();
                    transact.ajaxGET(
                                       "/Almacenar/_GetCampos?CodOrigen=" + CodOrigen,
                                         null,
                                         _ui.SusscesCargaCamposAlmacenamiento,
                                         _ui.displayError
                                       );
                    break;
                case "tipoContenedor":
                    _ui.opSelTipoUD = select.val();

                    break;
                default:

            }

        },

        SusscesCargaCampos: function (data) {
            $("#tableContent");
            _ui.configCampo($("#tableContent tbody"), data);
        },

        _successOficinas: function (data) {
            var Productos = $("#ddlOficinas option");
            if (Productos.length >= 1) {
                $("#ddlOficinas option").remove();
            }
            _ui.fillCombo($("#ddlOficinas"), data);
        },

        _successProductos: function (data) {
            var Productos = $("#ddlProductos option");
            if (Productos.length >= 1) {
                $("#ddlProductos option").remove();
            }
            _ui.fillCombo($("#ddlProductos"), data);
        },

        _successSubProductos: function (data) {
            var subProductos = $("#sleSubProductos option");
            if (subProductos.length >= 1) {
                $("#sleSubProductos option").remove();
            }
            _ui.fillCombo($("#sleSubProductos"), data);
        },

        _successProductosFacturacion: function (data) {
            var subProductos = $("#ddlProductos option");
            if (subProductos.length >= 1) {
                $("#ddlProductos option").remove();
            }
            _ui.fillCombo($("#ddlProductos"), data);
        },

        _successSubProductosReporte: function (data) {
            var subProductos = $("#dllSubProductos option");
            if (subProductos.length >= 1) {
                $("#dllSubProductos option").remove();
            }
            _ui.fillCombo($("#dllSubProductos"), data);
        },

        _successSociedad: function (data) {
            var subProductos = $("#ddlSociedad option");
            if (subProductos.length >= 1) {
                $("#ddlSociedad option").remove();
            }
            _ui.fillCombo($("#ddlSociedad"), data);
        },

        _successSubProductosAlmacenamiento: function (data) {
            var subproductos = $("#ulSubproductos li");
            if (subproductos.length >= 1) {
                $("#ulSubproductos li").remove();
            }
            console.log(data);
            var _content = $("#ulSubproductos");
            $.each(data, function (i, values) {
                _content.append(_ui.createElement("li", {
                    "value": values.Value,
                    "id": "li_" + i,
                    "data-selected": "falso",
                    "name": "subProc_" + values.Value
                }, values.Text));
                _ui.eventClick($("#li_" + i), _ui.selectSubproducto);
            });

        },

        selectSubproducto: function (e) {
            var _this = $(e.currentTarget);
            _this = $(_this);
            var seleted = _this.attr("data-selected");

            if (seleted == "falso") {
                _this.attr("data-selected", "true");
                _this.find("input").css({
                    "background-color": "#9AAE04",
                    "color": "White"
                });
                _this.css({
                    "background-color": "#9AAE04",
                    "color": "White"
                });
            } else if (seleted == "true") {
                _this.attr("data-selected", "falso");
                _this.find("input").removeAttr("style");
                _this.removeAttr("style");
            }

            console.log(_this);
        },

        SusscesCargaCamposAlmacenamiento: function (data) {
            $("#tblContentCampos");
            _ui.configCampo($("#tblContentCampos tbody"), data);
        },

        _estiloInputCorecto: function (e) {
            $(this).css("border", "1px solid green");
        },

        idCamp: null,
        configCampo: function (content, data) {
            content = $(content);
            console.log("Entra al mentodo configCampo")
            $.each(data, function (i, values) {
                content.append(_ui.createElement("tr", { "id": "tr" + i }, ""));
                $("#tr" + i).append(_ui.createElement("td", { "id": "tdText" + i }, ""));
                $("#tr" + i).append(_ui.createElement("td", { "id": "tdValue" + i }, ""));

                $("#tdText" + i).append(_ui.createElement("label", "", values.CampDescripcion + ":"));
                $("#tdValue" + i).append(_ui.createElement(_ui.validarTipoElemento(values.TcId),
                                                 {
                                                     "type": _ui.validTypeInput(values.TcId),
                                                     "name": "idCampo_" + values.CampId,
                                                     "style": "width:" + values.CampAncho + "%",
                                                     "id": $.trim(((values.CampDescripcion).replace(/\s/g, '')).replace(/\./g, ''))
                                                 }, ""));


                switch (values.TcId) {
                    case 1:
                        _ui.idCamp = $("#" + $.trim(((values.CampDescripcion).replace(/\s/g, '')).replace(/\./g, '')));
                        _ui.idCamp.attr("pattern", "^[0-9]{0," + values.LongMax + "}$");
                        _ui.idCamp.attr("onkeypress", "return numbersonly(event);");
                        break;

                    case 3:
                        _ui.idCamp = $("#" + $.trim(((values.CampDescripcion).replace(/\s/g, '')).replace(/\./g, '')));
                        _ui.idCamp.attr("placeholder", "DD/MM/YYYY")

                        //Validamos la fecha de radicacion para ingresar una nueva opcion
                        var idFecha = $(_ui.idCamp).attr('id');
                        if (idFecha == 'FechaDocumento') {
                            _ui.idCamp.attr("onblur", "validarFecha(this.value," + idFecha + ");");
                        }

                        _ui.builDatepicker(_ui.idCamp);
                        $.mask.definitions['~'] = "[+-]";
                        _ui.idCamp.mask("99/99/9999", {});

                        //-----------Generaba un error de Javascript-----------//
                        //$("input").blur(function () {
                        //    $("#info").html("Unmasked value: " + $(this).mask());
                        //}).dblclick(function () {
                        //    $(this).unmask();
                        //});

                        _ui.eventOutFocus(_ui.idCamp, _ui._estiloInputCorecto);
                        break;

                    case 5:
                        var idCampo = $("#" + $.trim(((values.CampDescripcion).replace(/\s/g, '')).replace(/\./g, '')));
                        if (_ui.validarTipoElemento(values.TcId).match(/select/g) == "select") {
                            idCampo.combobox();
                            $.ajax({
                                "type": "GET",
                                "url": values.CampDescripcion == "Estado" ? "/Contabilizar/_GetDropDownListEstados" : "/Radicacion/_GetValoresList?CampId=" + values.CampId,
                                "success": function (data) {
                                    $(document).ready(function () {
                                        _ui.fillCombo(idCampo, data);
                                        idCampo.combobox();

                                        var padre = idCampo.parent();
                                        if (values.CampObligatorio == true) {
                                            $(padre).find("input").attr({
                                                "required": "",
                                                "title": ""
                                            });
                                        }
                                        $(padre).find("input").val("-1")
                                        $(padre).find("input").val("");
                                    });
                                },
                                "error": _ui.displayError
                            });

                            if ((values.CampDescripcion).match(/Sociedad/g) == "Sociedad") {
                                idCampo.attr("data-work", "sociedad");
                            }

                            if ((values.CampDescripcion).match(/Causal/g) == "Causal") {
                                idCampo.attr("data-work", "causal");
                            }
                        }
                        break;

                    case 8:
                        $("#tdValue" + i).attr("colspan", "4");
                        break;

                    default:
                }

                if (values.CampObligatorio == true) {
                    _ui.idCamp = $("#" + $.trim(((values.CampDescripcion).replace(/\s/g, '')).replace(/\./g, '')));
                    _ui.idCamp.attr({
                        "required": "",
                        "title": "*Campo requerido"
                    });
                }

                if ((values.TcId != 1) && (values.TcId != 5) && (values.TcId != 3)) {
                    _ui.idCamp = $("#" + $.trim(((values.CampDescripcion).replace(/\s/g, '')).replace(/\./g, '')));
                    _ui.idCamp.attr("pattern", "^[0-9a-zA-ZñÑ -]{0," + values.LongMax + "}$");
                }

            });

            return true;
        },

        configCampoData: function (Campos, Data) {
            if (Data.length != 0) {
                $("#btnGuardar").attr("disabled", "disabled");
            }
            $.each(Campos, function (i, values) {
                $.each(Data, function (j, values2) {
                    if (values2.CampId == values.CampId) {
                        var idCampo = $("#" + $.trim(((values.CampDescripcion).replace(/\s/g, '')).replace(/\./g, '')));
                        idCampo.val(values2.NegValor);
                        idCampo.attr("disabled", "disabled");

                        if (values.TcId == 5) {
                            var padre = idCampo.parent();
                            $(padre).find("input").val(values2.NegValor);
                            idCampo.next().children().attr("disabled", "disabled");
                            idCampo.next().find("a").remove();
                        }
                    }
                });

            });
        },

        SusscesCargaListDatos: function (data) {
            _ui.fillCombo(_ui.idCamp, data)
            _ui.idCamp.combobox();
        },

        validarTipoElemento: function (tipo) {
            switch (tipo) {
                case 5 || 6 || 7:
                    return "select data-work='" + tipo + "'";
                    break;
                case 8:
                    return "textarea";
                    break;
                default:
                    return "input";
            }
        },

        validTypeInput: function (tipo) {
            switch (tipo) {
                case 1:
                    return "text";
                    break;
                case 2:
                    return "text";
                    break;
                case 3:
                    return "text";
                case 11:
                    return "checkbox";
                    break;
                case 12:
                    return "email";
                    break;
                default:
            }
        },

        crearMenuMaster: function (data, content) {
            content = $(content);
            var countImg = 1;
            var idPadre = "liPadre", idHijos = "ulHijos_";
            $.each(data, function (i, values) {
                content.append(_ui.createElement("li", { "id": idPadre + values.IdMenu, "data-rol": "menuPadre" }, ""));
                content = $("#" + idPadre + values.IdMenu);
                content.append(_ui.createElement("a", { "id": "link" + values.IdMenu, "class": "staticItem" }, ""));
                $("#link" + values.IdMenu).append(_ui.createElement("img", { "src": "../../.." + values.UrlImagen }, ""));
                $("#link" + values.IdMenu).append(values.DescMenu);
                content.append(_ui.createElement("ul", { "id": idHijos + values.IdMenu, "data-rol": "menuHijos", "class": "level2 dynamic", "style": "display: none; top: 0px; left: 100%;" }, ""));
                _ui.crearMenuHijos($("#" + idHijos + values.IdMenu), values.IdMenu);
                content = content.parent();
                countImg++;
            });

            $("li[data-rol='menuPadre']").hover(
            function () {
                $(this).find("ul[data-rol='menuHijos']").css("display", "block");

            },
            function () {
                $(this).find("ul[data-rol='menuHijos']").css("display", "none");
            });
        },

        crearMenuHijos: function (content, idPadre) {
            $.ajax({
                "type": "GET",
                "url": "/Seguridad/CargarHijosMenu?parentId=" + idPadre,
                "data": null,
                "dataType": "json",
                "success": function (data) {
                    var idHijos = "liHijos";
                    content = $(content);
                    $.each(data, function (i, values) {
                        content.append(_ui.createElement("li", { "id": idHijos + values.IdMenu, "role": "menuitem" }, ""));
                        content = $("#" + idHijos + values.IdMenu);
                        content.append(_ui.createElement("a", { "id": "link" + values.IdMenu, "href": values.Url, "class": "itemSelect" }, values.DescMenu));
                        content = content.parent();
                    });
                },
                "error": function (error) {
                    console.log(error);
                }
            });
        },

        eventClick: function (btn, event) {
            btn = $(btn),
            btn.on("click", event);
        },

        _keypress: function (e) {
            var _this = $(e.currentTarget);
        },

        eventKeypress: function (element) {
            element = $(element);
            element.on("keyup", _ui._keypress);
        },

        eventOnFocus: function (elemento, event) {
            elemento = $(elemento);
            console.log("entra focus in");
            elemento.on("focusin", event);
        },

        eventOutFocus: function (elemento, event) {
            elemento = $(elemento);
            console.log("entra focus OUT");
            elemento.on("focusout", event);
        },

        builModelDialog: function (elemento, open, buttons, width, height) {

            $(elemento).dialog({
                resizable: false,
                height: height,
                width: width,
                modal: true,
                autoOpen: open,
                buttons: buttons
            });
        },

        builResizable: function (elemento, elementoContinuo) {
            $(elemento).resizable({
                grid: 50,
                alsoResize: "#" + $(elementoContinuo).attr("id")
            });
            $(elementoContinuo).resizable();
        },

        btnDetalle: function (row, $grid) {
            console.log("Getalle de la columna " + row);
            var DM = $grid.pqGrid("option", "dataModel");
            console.log(DM.data[0][1]);
        }
    }

})(jQuery);