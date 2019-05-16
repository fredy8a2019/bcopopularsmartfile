var _grilla = null;
(function ($) {

    _grilla = window._grilla = {

        contador: 1,
        $grid: null,
        pqSearch: {
            txt: "",
            rowIndices: [],
            curIndx: null,
            colIndx: 0,
            sortIndx: null,
            sortDir: null,
            results: null,

            prevResult: function () {
                var colIndx = this.colIndx,
                    rowIndices = this.rowIndices;
                if (rowIndices.length == 0) {
                    this.curIndx = null;
                }
                else if (this.curIndx == null || this.curIndx == 0) {
                    this.curIndx = rowIndices.length - 1;
                }
                else {
                    this.curIndx--;
                }
                this.updateSelection(colIndx);
            },

            nextResult: function () {
                //debugger;
                var rowIndices = this.rowIndices;
                if (rowIndices.length == 0) {
                    this.curIndx = null;
                }
                else if (this.curIndx == null) {
                    this.curIndx = 0;
                }
                else if (this.curIndx < rowIndices.length - 1) {
                    this.curIndx++;
                }
                else {
                    this.curIndx = 0;
                }
                this.updateSelection();
            },

            updateSelection: function () {
                var colIndx = this.colIndx,
                    curIndx = this.curIndx,
                    rowIndices = this.rowIndices;
                if (rowIndices.length > 0) {
                    //results.html("Selected " + (curIndx + 1) + " , "+ rowIndx[curIndx] +" of " + rowIndx.length + " matche(s).");
                    this.results.html("Selected " + (curIndx + 1) + " of " + rowIndices.length + " match(es).");
                }
                else {
                    this.results.html("Nothing found.");
                }
                $grid.pqGrid("setSelection", null);
                //$grid.pqGrid("option", "customData", { foundRowIndices: rowIndices, txt: this.txt, searchColIndx: colIndx });
                //$grid.pqGrid("refreshColumn", { colIndx: colIndx });
                $grid.pqGrid("setSelection", { rowIndx: rowIndices[curIndx], colIndx: colIndx });
            },

            search: function (input, select, $grid) {

                var txt = $(input).val().toUpperCase(),//$("#txtSerch").val().toUpperCase(),
                    colIndx = $(select).val(),//$("#slcSerch").val(),
                    DM = $grid.pqGrid("option", "dataModel"),
                    sortIndx = DM.sortIndx,
                    sortDir = DM.sortDir;

                if (txt == this.txt && colIndx == this.colIndx && sortIndx == this.sortIndx && sortDir == this.sortDir) {
                    return;
                }
                this.rowIndices = [], this.curIndx = null;
                this.sortIndx = sortIndx;
                this.sortDir = sortDir;
                if (colIndx != this.colIndx) {
                    //clean the prev column.
                    //$grid.pqGrid("option", "customData", { foundRowIndices: [], txt: "", searchColIndx: colIndx });
                    $grid.pqGrid("option", "customData", null);
                    $grid.pqGrid("refreshColumn", { colIndx: this.colIndx });
                    this.colIndx = colIndx;
                }
                //debugger;

                if (txt != null && txt.length > 0) {
                    txt = txt.toUpperCase();
                    //this.colIndx = $("select#pq-crud-select-column").val();

                    var data = DM.data;
                    //debugger;
                    for (var i = 0; i < data.length; i++) {
                        var row = data[i];
                        var cell = row[this.colIndx].toUpperCase();
                        if (cell.indexOf(txt) != -1) {
                            this.rowIndices.push(i);
                        }
                    }
                }
                $grid.pqGrid("option", "customData", { foundRowIndices: this.rowIndices, txt: txt, searchColIndx: colIndx });
                $grid.pqGrid("refreshColumn", { colIndx: colIndx });
                this.txt = txt;
            },

            render: function (ui) {
                var rowIndxPage = ui.rowIndxPage,
                rowIndx = ui.rowIndx,
                //data = ui.dataModel.data,
                rowData = ui.rowData,
                dataIndx = ui.dataIndx,
                colIndx = ui.colIndx,
                val = rowData[dataIndx];
                //debugger;
                if (ui.customData) {

                    var rowIndices = ui.customData.foundRowIndices,
                    searchColIndx = ui.customData.searchColIndx,
                    txt = ui.customData.txt,
                    txtUpper = txt.toUpperCase(),
                    valUpper = val.toUpperCase();
                    if ($.inArray(rowIndx, rowIndices) != -1 && colIndx == searchColIndx) {
                        var indx = valUpper.indexOf(txtUpper);
                        if (indx >= 0) {
                            var txt1 = val.substring(0, indx);
                            var txt2 = val.substring(indx, indx + txt.length);
                            var txt3 = val.substring(indx + txt.length);
                            return txt1 + "<span style='background:yellow;color:#333;'>" + txt2 + "</span>" + txt3;
                        }
                        else {
                            return val;
                        }
                    }
                }
                return val;
            }

        },

        buildGrillaHTML: function (that) {

            var tbl = $("#tableHTML");
            var obj = $.paramquery.tableToArray(tbl);
            var newObj = {
                //width: 900, height: 460, sortIndx: 0,
                title: "<b>Unidad Documental</b>",
                selectionModel: { type: 'row' },
                editModel: { saveKey: 13 },
                freezeCols: 1,
                resizable: false,
                editable: false,
                flexHeight: true,
                flexWidth: true,
                scrollModel: { pace: 'fast', horizontal: false }
            };
            newObj.dataModel = { data: obj.data, paging: "local", rPP: 15, rPPOptions: [10, 15, 20, 50, 100] };
            newObj.colModel = obj.colModel;

            for (var i = 0; i < newObj.colModel.length ; i++) {
                var width = 300;
                if (i == 0) width = 100;
                $.extend(newObj.colModel[i], {
                    width: width,
                    render: function (ui) {
                        return _grilla.pqSearch.render(ui);
                    }
                });
            }

            $("#content_tableHTML").on("pqgridrender", function (evt, obj) {
                if ($("#contentSearch").length < 1) {
                    var $toolbar = $("<div class='pq-grid-toolbar pq-grid-toolbar-search' id='contentSearch'></div>").appendTo($(".pq-grid-top", this));

                    $("<span class='span' value='Show Popup'>Ver en Detalle</span>&nbsp;&nbsp;&nbsp;").appendTo($toolbar).button({
                        icons: {
                            primary: "ui-icon-search"
                        }
                    }).click(function (evt) {
                        _grilla.detalleRow();
                    });

                    $("&nbsp;&nbsp;&nbsp;&nbsp;<span class='span'>Eliminar</span>").appendTo($toolbar).button({
                        icons: {
                            primary: "ui-icon-circle-minus"
                        }
                    }).click(function () {
                        _grilla.deleteRow();
                    });                 

                    $toolbar.disableSelection();
                }

            });

            ///refresh the search after grid sort.
            $("#content_tableHTML").on("pqgridsort", function (evt, obj) {
                _grilla.pqSearch.search($("#txtSerch"), $("#slcSerch"));
                _grilla.pqSearch.nextResult();
            });
            //change the message after change in selection.
            $("#content_tableHTML").on("pqgridrowselect pqgridcellselect", function (evt, obj) {
                if (evt.originalEvent && evt.originalEvent.type == "click") {
                    if (_grilla.pqSearch.rowIndices.length > 0) {
                        _grilla.pqSearch.results.html(_grilla.pqSearch.rowIndices.length + " match(es).");
                    }
                }
            });

            $grid = $("#content_tableHTML").pqGrid(newObj);
            tbl.css("display", "none");

        },

        deleteRow: function () {
            var rowIndx = _grilla.getRowIndx();
            if (rowIndx != null) {

                var tbl = $("#tableHTML");
                var obj = $.paramquery.tableToArray(tbl);
                var DM = $grid.pqGrid("option", "dataModel");
                DM.data.splice(rowIndx, 1);
                $grid.pqGrid("refreshDataAndView");
                $grid.pqGrid("setSelection", { rowIndx: rowIndx });
                tbl.children().children()[rowIndx + 1].remove()
            }
        },

        getRowIndx: function () {
            //var $grid = $("#content_tableHTML");

            // var obj = $grid.pqGrid("getSelection");
            //debugger;
            var arr = $grid.pqGrid("selection", { type: 'row', method: 'getSelection' });
            if (arr && arr.length > 0) {
                var rowIndx = arr[0].rowIndx;

                //if (rowIndx != null && colIndx == null) {
                return rowIndx;
            } else {
                alert("Select a row.");
                return null;
            }
        },

        

        detalleRow: function () {
            var rowIndx = _grilla.getRowIndx();
            if (rowIndx != null) {
                var DM = $grid.pqGrid("option", "dataModel");
                transact.ajaxGET("/Almacenar/_getDetalle?loteONumeroUnico=" + DM.data[rowIndx][1], null, _grilla.buildGrillaDetalle, function () { });
            }
        },

        buildGrillaDetalle: function (data) {
            $("#txtSubProductos_Detalle").val(data[0].GruDescripcion);

            var Obj = {
                //width: 900, height: 460, sortIndx: 0,
                title: "<b>Detalle</b>",
                selectionModel: { type: 'row' },
                editModel: { saveKey: 13 },
                freezeCols: 1,
                resizable: true,
                editable: false,
                flexHeight: true,
                flexWidth: true,
                scrollModel: { pace: 'fast', horizontal: false }
            };
            Obj.dataModel = {
                data: data, paging: "local", rPP: 10, rPPOptions: [10, 15, 20, 50, 100]
            };
            Obj.colModel = [{ title: "NegId", width: 100, dataType: "integer", dataIndx: "NegId" },
                       { title: "Codigo Barras", width: 100, dataType: "string", dataIndx: "CodBarras" },
                       { title: "Paginas", width: 100, dataType: "integer", align: "center", dataIndx: "Paginas" },
                       //{ title: "Campo2", width: 150, dataType: "string", align: "center", dataIndx: "Campo2" },
            ];

            $("#dialogDetalle").dialog({

                height: 500,
                width: 600,
                create: function (evt, ui) {
                    //   $(this).detach().appendTo($("<input type='button'>"));

                },
                open: function (evt, ui) {

                    var $grid = $("#contetDetalle_TableHTML");
                    var ht = $grid.parent().height() - 2;
                    var wd = $grid.parent().width() - 2;
                    //alert("ht=" + ht + ", wd=" + wd);                        
                    if ($grid.hasClass('pq-grid')) {
                        $grid.pqGrid("option", { height: ht, width: wd });
                    }
                    else {
                        Obj.width = wd;
                        Obj.height = ht;
                        $grid.pqGrid(Obj);
                    }
                    //refresh the selections made before closing grid if any.
                    //$grid.pqGrid("selection", { type: 'cell', method: 'refresh' });
                    return true;
                },
                close: function () {
                    var $grid = $("#contetDetalle_TableHTML");
                    $grid.pqGrid('destroy');
                },
                resizeStop: function (evt, ui) {
                    var $grid = $("#contetDetalle_TableHTML");
                    var ht = $grid.parent().height();
                    var wd = $grid.parent().width();
                    $grid.pqGrid("option", { height: ht - 2, width: wd - 2 });
                },
                show: {
                    effect: "blind",
                    duration: 500
                },
                hide: {
                    effect: "explode",
                    duration: 500
                }
            });

            $("#dialogDetalle").dialog("open");
        }

    }

})(jQuery);