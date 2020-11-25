$(function () {
    $("#pageSize").dxSelectBox({
        items: [20, 50, 100],
        value: 20,
        onValueChanged: function (data) {
            showPageData(data.value);
        }
    });

    //DevExpress.config({
    //    editorStylingMode: "outlined"
    //});

    function showPageData(pageSize) {
        loadPanel.show();
        var formData = $("#form").dxForm("instance").option("formData");
        formData.BeginRowIndex = 0;
        formData.PageSize = pageSize;
        $("#form").find("div.dx-button").click();
    }

    var date = new Date();
    var initFormData = {
        "CREATE_DATE_START": date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + " 00:00:00",
        "CREATE_DATE_END": date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + " 23:59:59"
    };

    var form = $("#form").dxForm({
        formData: initFormData,
        readOnly: false,
        showColonAfterLabel: true,
        labelLocation: "left",
        minColWidth: 300,
        colCount: 4,
        items: [
            {
                dataField: "WAVE_ID",
                label: { text: "波次单号" },
                editorType: "dxTextBox"
            },
            {
                dataField: "WAREHOUSE",
                label: { text: "仓库" },
                editorType: "dxTextBox"
            },
            {
                dataField: "STATUS",
                label: { text: "状态" },
                showClearButton: true,
                editorType: "dxTagBox",
                editorOptions: {
                    dataSource: makeAsyncCommonData("wavestatus"),
                    displayExpr: "NAME",
                    valueExpr: "ID",
                    multiline: false,
                    placeholder: ""
                }
            },
            {
                dataField: "STORER_ID",
                label: { text: "货主" },
                editorType: "dxDropDownBox",
                editorOptions: {
                    valueExpr: "ID",
                    placeholder: "",
                    displayExpr: "Name",
                    showClearButton: true,
                    dropDownOptions: { resizeEnabled: true },
                    dataSource: makeAsyncDataSource("storer"),
                    height: "auto",
                    contentTemplate: function (e) {
                        var value = e.component.option("value"),
                            $dataGrid = $("<div>").dxDataGrid({
                                dataSource: e.component.option("dataSource"),
                                columns: [{
                                    dataField: "ID",
                                    caption: "代码"
                                }, {
                                    dataField: "NAME",
                                    caption: "名称"
                                }],
                                loadPanel: {
                                    enabled: true,
                                    showPane: true,
                                    shading: true,
                                    shadingColor: "#F7F7F7"
                                },
                                hoverStateEnabled: true,
                                //paging: { enabled: true, pageSize: 10 },
                                filterRow: { visible: true },
                                scrolling: { mode: "infinite", showScrollbar: "always", useNative: true },
                                height: 300,
                                selection: {
                                    mode: "multiple",
                                    showCheckBoxesMode: "always"
                                },
                                selectedRowKeys: value,
                                onSelectionChanged: function (selectedItems) {
                                    var keys = selectedItems.selectedRowKeys;
                                    e.component.option("value", keys);
                                }
                            });

                        dataGrid = $dataGrid.dxDataGrid("instance");

                        e.component.on("valueChanged", function (args) {
                            var value = args.value;
                            dataGrid.selectRows(value, false);
                        });
                        e.component.on("opened", function (args) {
                            args.component.option("dropDownOptions.width", 400);
                        });
                        return $dataGrid;
                    }
                }
            },
            {
                dataField: "CREATE_DATE_START",
                label: { text: "创建时间" },
                editorType: "dxDateBox",
                editorOptions: {
                    dateSerializationFormat: "yyyy-MM-dd HH:mm:ss",
                    displayFormat: "yyyy-MM-dd HH:mm:ss",
                    useMaskBehavior: true
                }
            },
            {
                dataField: "CREATE_DATE_END",
                label: { text: "至" },
                editorType: "dxDateBox",
                editorOptions: {
                    dateSerializationFormat: "yyyy-MM-dd HH:mm:ss",
                    displayFormat: "yyyy-MM-dd HH:mm:ss",
                    useMaskBehavior: true
                }
            },
            {
                itemType: "button",
                horizontalAlignment: "left",
                buttonOptions: {
                    text: "查询",
                    type: "success",
                    useSubmitBehavior: true,
                    onClick: function (e) {
                        //var validateResult = $("#form").dxForm("instance").validate();
                        //if (!validateResult.isValid) return;
                        loadPanel.show();
                        $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text dx-state-disabled");
                        var formData = $("#form").dxForm("instance").option("formData");
                        formData.BeginRowIndex = 0;
                        var pageSize = $("#pageSize").dxSelectBox("instance").option("value");
                        formData.PageSize = pageSize;
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "/Stock/GetWaveList",
                            data: formData,
                            success: function (jsonData) {
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", jsonData.rows);
                                $("#gridContainer").dxDataGrid("instance").refresh();
                                $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text");
                                var pageCount = jsonData.total % pageSize == 0 ? jsonData.total / pageSize : parseInt(jsonData.total / pageSize) + 1;
                                toolbar.option("items[4].template", "共" + jsonData.total + "条，每页" + pageSize + "条，第1/" + pageCount + "页");
                                loadPanel.hide();

                                $(".pagination").Paging({
                                    isFirst: true,
                                    isPre: true,
                                    showRecordNum: pageSize,
                                    totalNum: jsonData.total,
                                    isShow: false,
                                    showNum: function (selectedPage) {
                                        if (selectedPage < 1) return;
                                        loadPanel.show();
                                        pageSize = $("#pageSize").dxSelectBox("instance").option("value");
                                        formData.BeginRowIndex = (selectedPage - 1) * pageSize;
                                        formData.PageSize = pageSize;

                                        $.ajax({
                                            type: "post",
                                            dataType: "json",
                                            url: "/stock/GetWaveList",
                                            data: formData,
                                            success: function (jsonData) {
                                                $("#gridContainer").dxDataGrid("instance").option("dataSource", jsonData.rows);
                                                $("#gridContainer").dxDataGrid("instance").refresh();

                                                var pageCount = jsonData.total % pageSize == 0 ? jsonData.total / pageSize : parseInt(jsonData.total / pageSize) + 1;
                                                toolbar.option("items[4].template", "共" + jsonData.total + "条，每页" + pageSize + "条，第" + selectedPage + "/" + pageCount + "页");
                                                loadPanel.hide();
                                            },
                                            error: function (request, status, error) {
                                                DevExpress.ui.notify(error);
                                                loadPanel.hide();
                                            }
                                        });
                                    }
                                });
                            },
                            error: function (request, status, error) {
                                DevExpress.ui.notify(error);
                            }
                        });
                    }
                }
            }
        ]
    }).dxForm("instance");

    var toolbar = $("#toolbar").dxToolbar({
        items: [
            {
                location: 'before',
                widget: 'dxTabs',
                template: function () {
                    return "已经选择0项/共20项";
                }
            }, {
                location: 'before',
                widget: 'dxButton',
                options: {
                    text: '创建波次单',
                    onClick: function (e) {
                        window.open("/stock/rtnwavecreate", "_blank");
                    }
                }
            }, {
                location: 'before',
                widget: 'dxButton',
                locateInMenu: 'auto',
                options: {
                    text: "删除",
                    onClick: function () {
                        var checkedItems = $("#gridContainer").dxDataGrid("instance").getSelectedRowsData();
                        if (checkedItems == null || checkedItems.length < 1) {
                            DevExpress.ui.notify("请选择记录");
                            return;
                        }
                        if (!confirm("确认要删除勾选波次？")) {
                            return;
                        }
                        for (var i = 0; i < checkedItems.length; i++) {
                            loadPanel.show();
                            $("#toolbar").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text dx-state-disabled");
                            $.ajax({
                                type: "post",
                                dataType: "json",
                                url: "/stock/deletertnwave",
                                async: false,
                                data: {
                                    waveId: checkedItems[i].WAVE_ID
                                },
                                success: function (result) {
                                    if (result.code != 0) {
                                        DevExpress.ui.notify(result.message);
                                        loadPanel.hide();
                                        $("#toolbar").find("div.dx-button").attr("class", "dx-button dx-button-normal dx-button-mode-text dx-widget dx-button-has-text");
                                        return;
                                    }
                                    $("#toolbar").find("div.dx-button").attr("class", "dx-button dx-button-normal dx-button-mode-text dx-widget dx-button-has-text");
                                    loadPanel.hide();
                                    DevExpress.ui.notify("删除成功！");
                                },
                                error: function (request, status, error) {
                                    loadPanel.hide();
                                    DevExpress.ui.notify(request + ":" + status + ":" + error);
                                }
                            });
                        }
                    }
                }
            }, {
                location: 'after',
                widget: 'dxTabs',
                template: function () {
                    return "共0条，每页20条，第1/4页";
                }
            }
        ]
    }).dxToolbar("instance");

    var dataGrid = $("#gridContainer").dxDataGrid({
        keyExpr: "WAVE_ID",
        paging: {
            enabled: false
        },
        //pager: {
        //    showPageSizeSelector: true,
        //    allowedPageSizes: [20, 50, 100],
        //    //showInfo: true,
        //    //infoText: "{0}/{1}{2} ",
        //    showNavigationButtons: true
        //},
        selection: {
            mode: "multiple",
            showCheckBoxesMode: "always"
        },
        loadPanel: {
            enabled: false,
            showPane: false,
            shading: false,
            shadingColor: "#F7F7F7"
        },
        allowColumnReordering: true,
        allowColumnResizing: true,
        showBorders: true,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        columnsAutoWidth: true,
        filterRow: { visible: true },
        //filterPanel: { visible: true },
        headerFilter: { visible: true },
        scrolling: { mode: "standard", showScrollbar: "always", useNative: true },
        columns: [{
            dataField: "WAVE_ID",
            caption: "波次单号",
            width: 200
        },
        {
            dataField: "STATUS",
            caption: "状态",
            width: 120
        },
        {
            dataField: "WAREHOUSE",
            caption: "仓库",
            width: 120
        },
        {
            dataField: "STORER_ID",
            caption: "货主",
            width: 120
        }, {
            dataField: "MAX_LOCATION",
            caption: "货格数量",
            width: 120
        }, {
            dataField: "GOODSCOUNT",
            caption: "SKU种类",
            width: 120
        }, {
            dataField: "SKUCOUNT",
            caption: "SKU数量",
            width: 120
        }, {
            dataField: "SOW_UNIT",
            caption: "播种方式",
            width: 200
        }, {
            dataField: "MXT_ID",
            caption: "移位单号",
            width: 200
        }, {
            dataField: "VERSION",
            caption: "版本",
            width: 200
        }, {
            dataField: "CREATE_BY",
            caption: "创建人",
            width: 120
        }, {
            dataField: "CREATE_DATE",
            caption: "创建时间",
            width: 200
        }, {
            dataField: "MODIFY_BY",
            caption: "修改人",
            width: 120
        }, {
            dataField: "MODIFY_DATE",
            caption: "修改时间",
            width: 200
        },
        {
            dataField: "REMARKS",
            caption: "备注",
            width: 120
        }
        ],
        summary: {
            totalItems: [{
                column: "WAVE_ID",
                summaryType: "count"
            }]
        },
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData;
            toolbar.option("items[0].template", "已经选择" + data.length + "项/共" + $("#pageSize").dxSelectBox("instance").option("value") + "项");
        },
        onCellPrepared: function (e) {
            if (e.rowType == "data") {
                //状态
                if (e.columnIndex == 3) {
                    e.cellElement[0].innerText = makeCommonData("wavestatus", e.data.STATUS);
                }
            }
        },
        masterDetail: {
            enabled: true,
            template: function (container, options) {
                var data = options.data;
                var storerId = data.STORER_ID;
                var waveId = data.WAVE_ID;
                $.getJSON("/stock/GetWaveDetail/?storerId=" + storerId + "&waveId=" + waveId, null, function (jsonData) {
                    $("<div>").dxDataGrid({
                        showBorders: true,
                        columnsAutoWidth: false,
                        allowColumnReordering: true,
                        allowColumnResizing: true,
                        showColumnLines: true,
                        showRowLines: true,
                        paging: {
                            enabled: true
                        },
                        pager: {
                            showPageSizeSelector: true,
                            allowedPageSizes: [500, 1000, 2000],
                            showNavigationButtons: true
                        },
                        filterRow: { visible: true },
                        //filterPanel: { visible: true },
                        headerFilter: { visible: true },
                        rowAlternationEnabled: true,
                        columns: [
                            {
                                dataField: "RTN_LOCATION",
                                caption: "播种库位",
                                width: 80,
                                allowSorting: false
                            },
                            {
                                dataField: "RTN_AISLE",
                                caption: "播种通道",
                                width: 150,
                                allowSorting: false
                            },
                            {
                                dataField: "SKU",
                                caption: "商品代码",
                                width: 120,
                                allowSorting: false
                            },
                            {
                                dataField: "STYLE",
                                caption: "款号",
                                width: 120,
                                allowSorting: false
                            },
                            {
                                dataField: "EXP_BASE_QTY",
                                caption: "通知数量",
                                width: 120,
                                allowSorting: false
                            },
                            {
                                dataField: "PICKUP_QTY",
                                caption: "播种数量",
                                width: 200,
                                allowSorting: false
                            },
                            {
                                dataField: "PUTAWAY_QTY",
                                caption: "上架数量",
                                width: 200,
                                allowSorting: false
                            }, {
                                dataField: "CREATE_BY",
                                caption: "创建人",
                                width: 120
                            }, {
                                dataField: "CREATE_DATE",
                                caption: "创建时间",
                                width: 200
                            }, {
                                dataField: "MODIFY_BY",
                                caption: "修改人",
                                width: 120
                            }, {
                                dataField: "MODIFY_DATE",
                                caption: "修改时间",
                                width: 200
                            },
                            {
                                dataField: "TRF_LINE_SEQ",
                                caption: "移位序号",
                                width: 120
                            }], summary: {
                                totalItems: [{
                                    column: "EXP_BASE_QTY",
                                    summaryType: "sum"
                                }, {
                                    column: "PICKUP_QTY",
                                    summaryType: "sum"
                                }, {
                                    column: "PUTAWAY_QTY",
                                    summaryType: "sum"
                                }]
                            },
                        dataSource: new DevExpress.data.DataSource({
                            store: new DevExpress.data.ArrayStore({
                                key: "WAVE_ID",
                                data: jsonData.rows
                            }),
                            filter: ["WAVE_ID", "=", options.key]
                        })
                    }).appendTo(container);
                });

            }
        }
    }).dxDataGrid("instance");
});