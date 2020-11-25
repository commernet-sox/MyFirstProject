$(function () {
    $("#pageSize").dxSelectBox({
        items: [100, 200, 500],
        value: 100,
        onValueChanged: function (data) {
            showPageData(data.value);
        }
    });

    function showPageData(pageSize) {
        var formData = $("#form").dxForm("instance").option("formData");
        formData.BeginRowIndex = 0;
        formData.PageSize = pageSize;
        $("#btnQuery").click();
    }

    $("#btnClear").dxButton({
        type: "normal",
        text: "清空",
        onClick: function (e) {
            $("#form").dxForm("instance").option("formData", null);
        }
    });
    var cellCount = 0;
    $("#btnQuery").dxButton({
        type: "default",
        text: "查询",
        onClick: function (e) {
            var validateResult = $("#form").dxForm("instance").validate();
            if (!validateResult.isValid) return;
            loadPanel.show();
            $("#btnQuery").attr("class", "dx-button dx-button-default dx-button-mode-contained dx-widget dx-button-has-text dx-state-disabled");
            var formData = $("#form").dxForm("instance").option("formData");
            formData.BeginRowIndex = 0;
            var pageSize = $("#pageSize").dxSelectBox("instance").option("value");
            formData.PageSize = pageSize;
            $.ajax({
                type: "post",
                dataType: "json",
                url: "/Stock/QueryWaveList",
                data: formData,
                success: function (jsonData) {
                    if (jsonData.code != null && jsonData.code > 0) {
                        DevExpress.ui.notify(jsonData.message);
                        loadPanel.hide();
                        $("#btnQuery").attr("class", "dx-button dx-button-default dx-button-mode-contained dx-widget dx-button-has-text");
                        return;
                    }
                    if (jsonData.total == null || jsonData.total == undefined) {
                        loadPanel.hide();
                        $("#btnQuery").attr("class", "dx-button dx-button-default dx-button-mode-contained dx-widget dx-button-has-text");
                        return;
                    }
                    $("#gridContainer").dxDataGrid("instance").option("dataSource", jsonData.rows.data);
                    $("#gridContainer").dxDataGrid("instance").refresh();
                    cellCount = jsonData.rows.cellCount;
                    $("#form").dxForm("instance").getEditor("CELL_COUNT").option("value", jsonData.rows.cellCount);
                    $("#btnQuery").attr("class", "dx-button dx-button-default dx-button-mode-contained dx-widget dx-button-has-text");
                    var pageCount = jsonData.total % pageSize == 0 ? jsonData.total / pageSize : parseInt(jsonData.total / pageSize) + 1;
                    toolbar.option("items[0].template", "已经选择0项/共" + jsonData.rows.data.length + "项");
                    toolbar.option("items[2].template", "共" + jsonData.total + "条，每页" + pageSize + "条，第1/" + pageCount + "页");
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
                                url: "/Stock/QueryWaveList",
                                data: formData,
                                success: function (jsonData) {
                                    $("#gridContainer").dxDataGrid("instance").option("dataSource", jsonData.rows.data);
                                    $("#gridContainer").dxDataGrid("instance").refresh();

                                    if (jsonData.total == null || jsonData.total == undefined) {
                                        loadPanel.hide();
                                        return;
                                    }
                                    var pageCount = jsonData.total % pageSize == 0 ? jsonData.total / pageSize : parseInt(jsonData.total / pageSize) + 1;
                                    toolbar.option("items[2].template", "共" + jsonData.total + "条，每页" + pageSize + "条，第" + selectedPage + "/" + pageCount + "页");
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
                    $("#btnQuery").attr("class", "dx-button dx-button-default dx-button-mode-contained dx-widget dx-button-has-text");
                }
            });
        }
    });

    var toolbar = $("#toolbar").dxToolbar({
        height: "60px",
        readOnly: false,
        showColonAfterLabel: true,
        labelLocation: "left",
        items: [
            {
                location: 'before',
                widget: 'dxTabs',
                template: function () {
                    return "已经选择0项/共0项";
                }
            }, {
                location: 'before',
                widget: 'dxButton',
                options: {
                    text: '汇总',
                    onClick: function (e) {
                        var checkedItems = $("#gridContainer").dxDataGrid("instance").getSelectedRowsData();
                        if (checkedItems == null || checkedItems.length < 1) {
                            DevExpress.ui.notify("请选择记录");
                            return;
                        }
                        loadPanel.show();
                        $("#gridInfo").find("div.dx-button").attr("class", "dx-button dx-button-default dx-button-mode-contained dx-widget dx-button-has-text dx-state-disabled");
                        var formData = $("#form").dxForm("instance").option("formData");
                        if (parseInt(formData.CELL_COUNT) > cellCount) {
                            DevExpress.ui.notify("货格数量不能大于" + cellCount);
                            loadPanel.hide();
                            return;
                        }
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "/stock/savewave",
                            data: {
                                waveData: checkedItems, model: {
                                    STORER_ID: formData.STORER_ID,
                                    WAREHOUSE: formData.WAREHOUSE,
                                    LOCATION: formData.LOCATION,
                                    CELL_COUNT: formData.CELL_COUNT,
                                    SOW_UNIT: formData.SOW_UNIT,
                                    IsAll: $("#gridContainer").dxDataGrid("instance").totalCount() == checkedItems.length ? true : false
                                }
                            },
                            success: function (result) {
                                if (result.code != 0) {
                                    DevExpress.ui.notify(result.message);
                                    $("#gridInfo").find("div.dx-button").attr("class", "dx-button dx-button-normal dx-button-mode-text dx-widget dx-button-has-text");
                                    loadPanel.hide();
                                    return;
                                }
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", null);
                                DevExpress.ui.notify("汇总成功！");
                                var pageSize = $("#pageSize").dxSelectBox("instance").option("value");
                                toolbar.option("items[2].template", "共0条，每页" + pageSize + "条，第0/0页");
                                $(".pagination").html("");
                                loadPanel.hide();

                            },
                            error: function (request, status, error) {
                                DevExpress.ui.notify(request + ":" + status + ":" + error);
                                loadPanel.hide();
                            }
                        });
                    }
                }
            }, {
                location: 'after',
                widget: 'dxTabs',
                template: function () {
                    return "共0条，每页100条，第0/0页";
                }
            }
        ]
    }).dxToolbar("instance");

    DevExpress.config({
        editorStylingMode: "outlined"
    });

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
        minColWidth: 100,
        colCount: 1,
        items: [
            {
                itemType: "group",
                caption: "基本信息",
                colCount: 1,
                items: [{
                    dataField: "STORER_ID",
                    label: { text: "货主" },
                    editorType: "dxDropDownBox",
                    validationRules: [{
                        type: "required",
                        message: "货主必填"
                    }],
                    dropDownOptions: {
                        closeOnOutsideClick: true
                    },
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
                                    hoverStateEnabled: true,
                                    filterRow: { visible: true },
                                    scrolling: { mode: "infinite", showScrollbar: "always", useNative: true },
                                    height: 300,
                                    onRowClick: function (e) {
                                        $("#form").dxForm("instance").getEditor("STORER_ID").option("value", e.data.ID);
                                        $.get("/Common/GetWarehouseList", { storerId: e.data.ID }, function (data) {
                                            $("#form").dxForm("instance").getEditor("WAREHOUSE").option("dataSource", data);
                                            $("#form").dxForm("instance").getEditor("STORER_ID").close();

                                        });
                                    }
                                });
                            e.component.on("opened", function (args) {
                                args.component.option("dropDownOptions.width", 400);
                            });
                            return $dataGrid;
                        }
                    }
                },
                {
                    dataField: "WAREHOUSE",
                    label: { text: "仓库" },
                    editorType: "dxSelectBox",
                    showClearButton: true,
                    validationRules: [{
                        type: "required",
                        message: "仓库必填"
                    }],
                    editorOptions: {
                        displayExpr: "WH_DESC",
                        valueExpr: "WAREHOUSE",
                        placeholder: "",
                        onValueChanged: function (e) {
                            $.get("/Common/GetLocationList", { storerId: $("#form").dxForm("instance").getEditor("STORER_ID").option("value"), warehouse: e.value, locationType: "SOWSTORAGE" }, function (data) {
                                $("#form").dxForm("instance").getEditor("LOCATION").option("value", "");
                                $("#form").dxForm("instance").getEditor("LOCATION").option("dataSource", data);
                            });
                        }
                    }
                },
                {
                    dataField: "LOCATION",
                    label: { text: "暂存库位" },
                    showClearButton: true,
                    editorType: "dxTagBox",
                    validationRules: [{
                        type: "required",
                        message: "暂存库位必填"
                    }],
                    editorOptions: {
                        displayExpr: "LOCATION",
                        valueExpr: "LOCATION",
                        multiline: false,
                        placeholder: ""
                    }
                },
                {
                    dataField: "SOW_UNIT",
                    label: { text: "播种方式" },
                    editorType: "dxSelectBox",
                    showClearButton: true,
                    validationRules: [{
                        type: "required",
                        message: "播种方式必填"
                    }],
                    editorOptions: {
                        dataSource: [{ ID: "SKU", NAME: "商品代码-[SKU]" }, { ID: "STYLE", NAME: "款号-[STYLE]" }],
                        displayExpr: "NAME",
                        valueExpr: "ID",
                        placeholder: ""
                    }
                }, {
                    dataField: "CELL_COUNT",
                    label: { text: "货格数量" },
                    editorType: "dxNumberBox",
                    editorOptions: {
                        showSpinButtons: true,
                        showClearButton: true,
                        value: 0,
                        min: 1,
                        max: 10000
                    }
                }]
            }
        ]
    }).dxForm("instance");

    $(".page-footer").width($(window).width() - 30);
    var posBtn = $(window).width() / 3 - 100;
    $("#btnClear").attr("style", "margin-left:" + (posBtn - 80) + "px");
    $("#btnQuery").attr("style", "margin-left:" + posBtn + "px;margin-top:-53.5px");
    $("#gridInfo").attr("style", "margin-top:-" + $("#form-container").height() + "px;width:70%; float:right;");

    $("#form-container").on("submit", function (e) {
        DevExpress.ui.notify({
            message: "You have submitted the form",
            position: {
                my: "center top",
                at: "center top"
            }
        }, "success", 3000);

        e.preventDefault();
    });

    var dataGrid = $("#gridContainer").dxDataGrid({
        keyExpr: "LOT_ID",
        paging: {
            enabled: false
        },
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
        width: "100%",
        height: "auto",
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
            dataField: "LOT_ID",
            caption: "批次",
            width: 120
        },
        {
            dataField: "PACKAGE_CODE",
            caption: "包装数",
            width: 100
        },
        {
            dataField: "PACKAGE_QTY",
            caption: "包装数量",
            width: 120
        },
        {
            dataField: "LOCATION",
            caption: "库位",
            width: 120
        },
        {
            dataField: "SKU",
            caption: "商品代码",
            width: 120
        },
        {
            dataField: "STYLE",
            caption: "款号",
            width: 120
        },
        {
            dataField: "QTY",
            caption: "库存数量",
            width: 120
        }
        ],
        summary: {
            totalItems: [{
                column: "QTY",
                summaryType: "sum"
            }]
        },
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData;
            toolbar.option("items[0].template", "已经选择" + data.length + "项/共" + $("#pageSize").dxSelectBox("instance").option("value") + "项");
            if (data.length > 0) {
                $("#gridInfo").find("div.dx-button").attr("class", "dx-button dx-button-normal dx-button-mode-text dx-widget dx-button-has-text");
            } else {
                $("#gridInfo").find("div.dx-button").attr("class", "dx-button dx-button-default dx-button-mode-contained dx-widget dx-button-has-text dx-state-disabled");
            }
        }
    }).dxDataGrid("instance");
});