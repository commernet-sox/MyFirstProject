$(function () {
    $("#pageSize").dxSelectBox({
        items: [20, 50, 100],
        value: 20,
        onValueChanged: function (data) {
            showPageData(data.value);
        }
    });

    DevExpress.config({
        editorStylingMode: "outlined"
    });

    function showPageData(pageSize) {
        loadPanel.show();
        var formData = $("#form").dxForm("instance").option("formData");
        formData.BeginRowIndex = 0;
        formData.PageSize = pageSize;
        $("div.dx-button").click();
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
                dataField: "WAVE_TYPE",
                label: { text: "波次类型" },
                editorType: "dxSelectBox",
                showClearButton: true,
                editorOptions: {
                    dataSource: makeAsyncCommonData("wavetype"),
                    displayExpr: "NAME",
                    valueExpr: "ID",
                    placeholder: ""
                }
            },
            {
                dataField: "STATUS",
                label: { text: "状态" },
                showClearButton: true,
                editorType: "dxSelectBox",
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
                dataField: "AUDIT_STATUS",
                label: { text: "确认状态" },
                showClearButton: true,
                editorType: "dxSelectBox",
                editorOptions: {
                    dataSource: makeAsyncCommonData("auditstatus"),
                    displayExpr: "NAME",
                    valueExpr: "ID",
                    placeholder: ""
                }
            },
            {
                dataField: "PRINT_STATUS",
                label: { text: "打印状态" },
                showClearButton: true,
                editorType: "dxSelectBox",
                editorOptions: {
                    dataSource: makeAsyncCommonData("printstatus"),
                    displayExpr: "NAME",
                    valueExpr: "ID",
                    placeholder: ""
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
                dataField: "CARRIER",
                label: { text: "快递" },
                editorType: "dxDropDownBox",
                editorOptions: {
                    valueExpr: "ID",
                    placeholder: "",
                    displayExpr: "Name",
                    showClearButton: true,
                    dropDownOptions: { resizeEnabled: true },
                    dataSource: makeAsyncDataSource("carrier"),
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
                dataField: "ALLOCATED_DATE_START",
                label: { text: "分配时间" },
                editorType: "dxDateBox",
                editorOptions: {
                    dateSerializationFormat: "yyyy-MM-dd HH:mm:ss",
                    displayFormat: "yyyy-MM-dd HH:mm:ss",
                    useMaskBehavior: true
                }
            },
            {
                dataField: "ALLOCATED_DATE_END",
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
                            url: "/Wave/GetWaveList",
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
                                            url: "/Wave/GetWaveList",
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
                    text: '取消波次单',
                    onClick: function (e) {
                        var c = e.component;
                        DevExpress.ui.notify("取消波次单!");
                    }
                }
            }, {
                location: 'before',
                widget: 'dxButton',
                locateInMenu: 'auto',
                options: {
                    text: "确认波次单",
                    onClick: function () {
                        DevExpress.ui.notify("确认波次单!");
                    }
                }
            },
            {
                location: 'before',
                widget: 'dxButton',
                locateInMenu: 'auto',
                options: {
                    text: "打印",
                    onClick: function () {
                        DevExpress.ui.notify("打印!");
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
            dataField: "AUDIT_STATUS",
            caption: "确认状态",
            width: 120
        },
        {
            dataField: "WAVE_TYPE",
            caption: "波次类型",
            width: 120
        }, {
            dataField: "PRINT_STATUS",
            caption: "打印状态",
            width: 120
        }, {
            dataField: "print_date",
            caption: "打印日期",
            width: 200
        }, {
            dataField: "dtl_count",
            caption: "订单数量",
            width: 150
        }, {
            dataField: "ALLOCATED_DATE",
            caption: "分配日期",
            width: 200
        }, {
            dataField: "CARRIER",
            caption: "快递",
            width: 120
        }, {
            dataField: "STORER_ID",
            caption: "货主",
            width: 120
        }, {
            dataField: "RULE_SOP_NAME",
            caption: "规则",
            width: 120
        }, {
            dataField: "RULE_SOP_UNAME",
            caption: "明细规则",
            width: 120
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
        }
        ],
        summary: {
            totalItems: [{
                column: "dtl_count",
                summaryType: "sum"
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

                //确认状态
                if (e.columnIndex == 4) {
                    e.cellElement[0].innerText = makeCommonData("auditstatus", e.data.AUDIT_STATUS);
                }

                //波次类型
                if (e.columnIndex == 5) {
                    e.cellElement[0].innerText = makeCommonData("wavetype", e.data.WAVE_TYPE);
                }

                //打印状态
                if (e.columnIndex == 6) {
                    if (e.data.PRINT_STATUS == "0") {
                        e.cellElement[0].innerText = "否";
                    } else if (e.data.PRINT_STATUS == "1") {
                        e.cellElement[0].innerText = "是";
                    }
                }
            }
        },
        masterDetail: {
            enabled: true,
            template: function (container, options) {
                var data = options.data;
                var storerId = data.STORER_ID;
                var waveId = data.WAVE_ID;
                $.getJSON("/Wave/GetWaveDetail/?storerId=" + storerId + "&waveId=" + waveId, null, function (jsonData) {
                    $("<div>").dxDataGrid({
                        showBorders: true,
                        columnsAutoWidth: false,
                        allowColumnReordering: true,
                        allowColumnResizing: true,
                        showColumnLines: true,
                        showRowLines: true,
                        rowAlternationEnabled: true,
                        columns: [
                            {
                                dataField: "CELL_NO",
                                caption: "篮号",
                                width: 80,
                                allowSorting: false
                            },
                            {
                                dataField: "Rel_Status",
                                caption: "订单状态",
                                width: 150,
                                allowSorting: false
                            },
                            {
                                dataField: "REL_ID",
                                caption: "订单号",
                                width: 200,
                                allowSorting: false
                            },
                            {
                                dataField: "CARRIER",
                                caption: "快递",
                                width: 120,
                                allowSorting: false
                            },
                            {
                                dataField: "SKU_COUNT",
                                caption: "商品种类",
                                width: 120,
                                allowSorting: false
                            },
                            {
                                dataField: "GOODS_COUNT",
                                caption: "商品数量",
                                width: 120,
                                allowSorting: false
                            },
                            {
                                dataField: "TRACKING_NO",
                                caption: "运单号",
                                width: 200,
                                allowSorting: false
                            },
                            {
                                dataField: "EDI_UP_ID",
                                caption: "同步单号",
                                width: 200,
                                allowSorting: false
                            }],
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