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

    //var date = new Date();
    //var initFormData = {
    //    "CREATE_DATE_START": date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + " 00:00:00",
    //    "CREATE_DATE_END": date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + " 23:59:59"
    //};

    var form = $("#form").dxForm({
        //formData: initFormData,
        readOnly: false,
        showColonAfterLabel: true,
        labelLocation: "left",
        minColWidth: 300,
        colCount: 4,
        items: [
            {
                dataField: "MxtId",
                label: { text: "移位单号" },
                editorType: "dxTextBox"
            },
            {
                dataField: "Status",
                label: { text: "状态" },
                showClearButton: true,
                editorType: "dxSelectBox",
                editorOptions: {
                    dataSource: makeAsyncCommonData("movestatus"),
                    displayExpr: "NAME",
                    valueExpr: "ID",
                    multiline: false,
                    placeholder: ""
                }
            },
            {
                dataField: "StorerId",
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
                                },
                                onCellClick: function (e) {
                                    if (e.rowIndex >= 0) {
                                        if (e.values[0] == true) {
                                            e.component.deselectRows(e.data.ID);
                                            e.values[0] = false;
                                        } else {
                                            var keys = e.component.getSelectedRowKeys();
                                            keys.push(e.data.ID);
                                            e.component.selectRows(keys, true);
                                            e.values[0] = true;
                                        }
                                    }
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
                dataField: "Warehouse",
                label: { text: "仓库" },
                editorType: "dxTextBox"
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
                            url: "/stock/getstockmovelist",
                            data: formData,
                            success: function (jsonData) {
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", jsonData.rows);
                                $("#gridContainer").dxDataGrid("instance").refresh();
                                $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text");
                                var pageCount = jsonData.total % pageSize == 0 ? jsonData.total / pageSize : parseInt(jsonData.total / pageSize) + 1;
                                toolbar.option("items[5].template", "共" + jsonData.total + "条，每页" + pageSize + "条，第1/" + pageCount + "页");
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
                                            url: "/stock/getstockmovelist",
                                            data: formData,
                                            success: function (jsonData) {
                                                $("#gridContainer").dxDataGrid("instance").option("dataSource", jsonData.rows);
                                                $("#gridContainer").dxDataGrid("instance").refresh();

                                                var pageCount = jsonData.total % pageSize == 0 ? jsonData.total / pageSize : parseInt(jsonData.total / pageSize) + 1;
                                                toolbar.option("items[5].template", "共" + jsonData.total + "条，每页" + pageSize + "条，第" + selectedPage + "/" + pageCount + "页");
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
                    text: '导出',
                    onClick: function (e) {
                        var c = e.component;
                        DevExpress.ui.notify("导出!");
                    }
                }
            }, {
                location: 'after',
                widget: 'dxButton',
                locateInMenu: 'auto',
                options: {
                    text: "下载模板",
                    onClick: function () {
                        DevExpress.ui.notify("下载模板!");
                    }
                }
            },
            {
                location: 'after',
                widget: 'dxButton',
                locateInMenu: 'auto',
                options: {
                    text: "导入",
                    onClick: function () {
                        DevExpress.ui.notify("导入!");
                    }
                }
            }, {
                location: 'after',
                widget: 'dxButton',
                locateInMenu: 'auto',
                options: {
                    text: "新增",
                    onClick: function () {
                        DevExpress.ui.notify("新增!");
                    }
                }
            }, {
                location: 'after',
                widget: 'dxTabs',
                template: function () {
                    return "共0条，每页20条，第0/0页";
                }
            }
        ]
    }).dxToolbar("instance");

    var dataGrid = $("#gridContainer").dxDataGrid({
        keyExpr: "MxtId",
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
        allowColumnReordering: true,
        allowColumnResizing: true,
        showBorders: true,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        columnsAutoWidth: true,
        filterRow: { visible: true },
        width: "auto",
        height: $(window).height() - 300,
        //filterPanel: { visible: true },
        headerFilter: { visible: true },
        scrolling: { mode: "standard", showScrollbar: "always", useNative: true },
        columnFixing: true,
        columnResizingMode: "widget",
        columns: [{
            dataField: "MxtId",
            caption: "移位单号",
            width: 200,
            fixed: true,
            cellTemplate: function (container, options) {
                var link = "<a href='#id=" + options.value + "' class='dx-link dx-link-edit'";
                link += " onclick = \"showPopup('" + options.value + "');\">" + options.value + "</a>";
                $("<div>")
                    .append($(link))
                    .appendTo(container);
            }
        },
        {
            dataField: "Status",
            caption: "状态",
            width: 120
        },
        {
            dataField: "Warehouse",
            caption: "仓库标识",
            width: 120
        },
        {
            dataField: "StorerId",
            caption: "货主",
            width: 120
        },
        {
            dataField: "OrderType",
            caption: "单据类型",
            width: 120
        }, {
            dataField: "EffDate",
            caption: "生效日期",
            width: 120
        }, {
            dataField: "CfmDate",
            caption: "确认日期",
            width: 200
        }, {
            dataField: "OrderFlag",
            caption: "单据来源",
            width: 150
        }, {
            dataField: "Version",
            caption: "版本",
            width: 200
        }, {
            dataField: "Remarks",
            caption: "备注",
            width: 120
        }, {
            dataField: "CreateBy",
            caption: "创建人",
            width: 120
        }, {
            dataField: "CreateDate",
            caption: "创建时间",
            width: 200
        }, {
            dataField: "ModifyBy",
            caption: "修改人",
            width: 120
        }, {
            dataField: "ModifyDate",
            caption: "修改时间",
            width: 200
        },
        {
            dataField: "MxtId",
            caption: "操作",
            width: 120,
            fixed: true,
            fixedPosition: "right",
            cellTemplate: function (container, options) {
                $("<div>")
                    .append($("<div style='display:inline-block;margin-right:5px;'><a href='#id=" + options.value + "' class='dx-link dx-link-edit'>作业</a></div>"))
                    .append($("<div style='display:inline-block;margin-right:5px;'><a href='#id=" + options.value + "' class='dx-link dx-link-edit'>完成</a></div>"))
                    .append($("<div style='display:inline-block;margin-right:5px;'><a href='#id=" + options.value + "' class='dx-link dx-link-edit'>取消</a></div>"))
                    .appendTo(container);
            }
        }
        ],
        //summary: {
        //    totalItems: [{
        //        column: "MxtId",
        //        summaryType: "count"
        //    }]
        //},
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData;
            toolbar.option("items[0].template", "已经选择" + data.length + "项/共" + $("#pageSize").dxSelectBox("instance").option("value") + "项");
        },
        onCellPrepared: function (e) {
            if (e.rowType == "data") {
                //状态
                if (e.column.dataField == "Status") {
                    if (e.data.Status == "00") {
                        e.cellElement[0].innerText = "删除";
                    } else if (e.data.Status == "10") {
                        e.cellElement[0].innerText = "创建";
                    } else if (e.data.Status == "15") {
                        e.cellElement[0].innerText = "审核";
                    } else if (e.data.Status == "20") {
                        e.cellElement[0].innerText = "部分完成";
                    } else if (e.data.Status == "25") {
                        e.cellElement[0].innerText = "移位完毕";
                    }
                }
                if (e.column.dataField == "OrderType") {
                    if (e.data.OrderType=="MOV") {
                        e.cellElement[0].innerText = "移位";
                    } else if (e.data.OrderType == "MXT") {
                        e.cellElement[0].innerText = "移位和调整";
                    } else if (e.data.OrderType == "TRF") {
                        e.cellElement[0].innerText = "调整";
                    }
                }
            }
        }
    }).dxDataGrid("instance");
});

var popup = null;
var popupOptions = {
    width: "80%",
    height: function () {
        return window.innerHeight;
    },
    position: "right",
    contentTemplate: function () {
        $("#content").attr("style", "");
        return $("<div />").append(
            $("#content")
        );
    },
    showTitle: true,
    title: "移位单详情",
    visible: false,
    dragEnabled: false,
    closeOnOutsideClick: true,
    showCloseButton: true,
    animation: {
        show: {
            type: 'fadeIn',
            duration: 400
        }
    },
    onShown: function () {
        // bind data
    }
};
if (popup) {
    $(".popup").remove();
}

function showPopup(id) {
    var $popupContainer = $("<div />")
        .addClass("popup")
        .appendTo($("#popup"));
    popup = $popupContainer.dxPopup(popupOptions).dxPopup("instance");
    popup.show();

    var formData = null;
    var rows = $("#gridContainer").dxDataGrid("instance").getVisibleRows();
    if (rows != null && rows.length > 0) {
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].data.MxtId == id) {
                formData = rows[i].data;
                break;
            }
        }
    }

    $("#detailForm").dxForm({
        formData: formData,
        readOnly: false,
        showColonAfterLabel: true,
        labelLocation: "left",
        minColWidth: 300,
        colCount: 4,
        items: [
            {
                dataField: "MxtId",
                label: { text: "移位单号" },
                editorType: "dxTextBox"
            },
            {
                dataField: "Warehouse",
                label: { text: "仓库" },
                editorType: "dxTextBox"
            }, {
                dataField: "StorerId",
                label: { text: "货主" },
                editorType: "dxTextBox"
            }, {
                dataField: "OrderType",
                label: { text: "单据类型" },
                editorType: "dxTextBox"
            }, {
                dataField: "EffDate",
                label: { text: "生效日期" },
                editorType: "dxTextBox"
            }, {
                dataField: "CfmDate",
                label: { text: "确认日期" },
                editorType: "dxTextBox"
            }, {
                dataField: "OrderFlag",
                label: { text: "单据来源" },
                editorType: "dxTextBox"
            },
            {
                dataField: "CreateBy",
                label: { text: "创建人" },
                editorType: "dxTextBox"
            },
            {
                dataField: "CreateDate",
                label: { text: "创建时间" },
                editorType: "dxTextBox"
            },
            {
                dataField: "ModifyBy",
                label: { text: "修改人" },
                editorType: "dxTextBox"
            },
            {
                dataField: "ModifyDate",
                label: { text: "修改时间" },
                editorType: "dxTextBox"
            }
        ]
    }).dxForm("instance");

    var textboxs = $("#detailForm").find("div[class='dx-textbox dx-texteditor dx-editor-outlined dx-widget']");
    if ($(textboxs) != null && $(textboxs).length > 0) {
        for (var i = 0; i < $(textboxs).length; i++) {
            $(textboxs).attr("class", "dx-textbox dx-texteditor dx-editor dx-widget");
        }
    }
    textboxs = $("#detailForm").find("div[class='dx-textbox dx-texteditor dx-editor-outlined dx-widget dx-texteditor-empty']");
    if ($(textboxs) != null && $(textboxs).length > 0) {
        for (var i = 0; i < $(textboxs).length; i++) {
            $(textboxs).attr("class", "dx-textbox dx-texteditor dx-editor dx-widget");
        }
    }

    var detailGrid = $("#detailGrid").dxDataGrid({
        keyExpr: ["WorkCenter", "StorerId", "MxtId", "MxtLineSeq"],
        paging: {
            enabled: false
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
        height: "280px",
        //filterPanel: { visible: true },
        headerFilter: { visible: true },
        scrolling: { mode: "standard", showScrollbar: "always", useNative: true },
        columnFixing: true,
        columns: [
            {
                dataField: "WorkCenter",
                caption: "WorkCenter",
                visible: false
            }, {
                dataField: "StorerId",
                caption: "StorerId",
                visible: false
            }, {
                dataField: "MxtId",
                caption: "MxtId",
                visible: false
            }, {
                dataField: "MxtLineSeq",
                caption: "MxtLineSeq",
                visible: false
            },
            {
                dataField: "Status",
                caption: "状态",
                width: 120
            },
            {
                dataField: "LotIdFrom",
                caption: "(原始)批次",
                width: 120
            },
            {
                dataField: "LotIdTo",
                caption: "(当前)批次",
                width: 120
            },
            {
                dataField: "LocationFrom",
                caption: "(原始)库位",
                width: 120
            }, {
                dataField: "EffDate",
                caption: "生效日期",
                width: 120
            }, {
                dataField: "LocationTo",
                caption: "(当前)库位",
                width: 200
            }, {
                dataField: "SkuFrom",
                caption: "(原始)SKU",
                width: 150
            }, {
                dataField: "Color",
                caption: "颜色",
                width: 200
            }, {
                dataField: "Size",
                caption: "尺码",
                width: 120
            }, {
                dataField: "UpcCode",
                caption: "商品条码",
                width: 120
            }, {
                dataField: "SkuDesc",
                caption: "商品描述",
                width: 200
            }, {
                dataField: "ProductBatchFrom",
                caption: "生产批次号",
                width: 120
            }, {
                dataField: "ManufDateFrom",
                caption: "生产日期",
                width: 200
            }, {
                dataField: "ExpiryDateFrom",
                caption: "到期日期",
                width: 200
            }, {
                dataField: "SkuStatusFrom",
                caption: "(原始)SKU状态",
                width: 200
            }, {
                dataField: "QtyFrom",
                caption: "(原始)可用数",
                width: 200
            }, {
                dataField: "QtyTo",
                caption: "(当前)可用数",
                width: 200
            }, {
                dataField: "BaseQtyFrom",
                caption: "(原始)调整基数",
                width: 200
            }, {
                dataField: "BaseQtyTo",
                caption: "(当前)调整基数",
                width: 200
            }, {
                dataField: "PackageCodeFrom",
                caption: "(原始)包装单位",
                width: 200
            }, {
                dataField: "PackageQtyFrom",
                caption: "(原始)包装数",
                width: 200
            }, {
                dataField: "AvaQty",
                caption: "可用数",
                width: 200
            }, {
                dataField: "DiscQty",
                caption: "差异数",
                width: 200
            }, {
                dataField: "Version",
                caption: "版本",
                width: 200
            }, {
                dataField: "Remarks",
                caption: "备注",
                width: 200
            }, {
                dataField: "CreateBy",
                caption: "创建人",
                width: 200
            }, {
                dataField: "CreateDate",
                caption: "创建时间",
                width: 200
            }, {
                dataField: "ModifyBy",
                caption: "修改人",
                width: 200
            }, {
                dataField: "ModifyDate",
                caption: "修改时间",
                width: 200
            }
        ],
        summary: {
            totalItems: [{
                column: "Status",
                summaryType: "count"
            }, {
                column: "QtyFrom",
                summaryType: "sum"
            }, {
                column: "QtyTo",
                summaryType: "sum"
            }, {
                column: "AvaQty",
                summaryType: "sum"
            }, {
                column: "DiscQty",
                summaryType: "sum"
            }]
        },
        onContentReady: function () {
            $("#detailGrid").dxDataGrid("instance").option("height", $(window).height() - 240);
        },
        onCellPrepared: function (e) {
            if (e.rowType == "data") {
                //状态
                if (e.columnIndex == 0) {
                    if (e.data.Status == "00") {
                        e.cellElement[0].innerText = "删除";
                    } else if (e.data.Status == "10") {
                        e.cellElement[0].innerText = "创建";
                    } else if (e.data.Status == "15") {
                        e.cellElement[0].innerText = "审核";
                    } else if (e.data.Status == "20") {
                        e.cellElement[0].innerText = "部分完成";
                    } else if (e.data.Status == "25") {
                        e.cellElement[0].innerText = "移位完毕";
                    }
                }
            }
        }
    }).dxDataGrid("instance");

    loadPanel.show();
    $.ajax({
        type: "get",
        dataType: "json",
        url: "/stock/getstockmovedetail",
        data: { storerId: formData.StorerId, mxtId: formData.MxtId },
        success: function (jsonData) {
            $("#detailGrid").dxDataGrid("instance").option("dataSource", jsonData.rows);
            $("#detailGrid").dxDataGrid("instance").refresh();
            loadPanel.hide();
        },
        error: function (request, status, error) {
            DevExpress.ui.notify(error);
            loadPanel.hide();
        }
    });
}

var popup_setting = null;
var popupOptions_setting = {
    width: "90%",
    height: "90%",
    contentTemplate: function () {
        $("#setting").attr("style", "");
        return $("<div />").append(
            $("#setting")
        );
    },
    showTitle: true,
    title: "列表页设置",
    visible: false,
    dragEnabled: false,
    closeOnOutsideClick: false,
    showCloseButton: true,
    animation: {
        show: {
            type: 'fadeIn',
            duration: 400
        }
    },
    onHidden: function () {
        hasPopupDiv = false;
    },
    onShown: function () {
        hasPopupDiv = true;
        $(".next-menu-item-inner").attr("style", "");
        $(".next-menu-item-inner").click(function () {
            var items = $(".next-menu-item-inner");
            for (var i = 0; i < $(items).length; i++) {
                if ($(this).parent().attr("tabindex") == undefined) continue;
                if ($(this).parent().attr("tabindex") == $($(items)[i]).parent().attr("tabindex")) {
                    $($(items)[i]).parent().attr("class", "next-menu-item next-nav-item next-selected");
                    if ($(this).parent().attr("tabindex") == "0") {
                        $("#query_fieldlist").attr("style", "display:inline-block;width:80%;");
                        $("#gird_fieldlist").attr("style", "display:none;");
                    } else {
                        $("#query_fieldlist").attr("style", "display:none;");
                        $("#gird_fieldlist").attr("style", "display:inline-block;width:80%;");
                    }
                } else {
                    $($(items)[i]).parent().attr("class", "next-menu-item next-nav-item");
                }
            }
        });

        var fieldlist = new Array();
        var formItems = $("#form").dxForm("instance").option("items");
        if (formItems != null && formItems.length > 0) {
            for (var i = 0; i < formItems.length; i++) {
                if (formItems[i].editorType != undefined) {
                    fieldlist.push({
                        dataField: formItems[i].dataField,
                        text: formItems[i].label.text,
                        isVisible: formItems[i].visible == undefined || formItems[i].visible == true
                    });
                }
            }
        }
        $("#listbox_query").dxList({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.ArrayStore({
                    key: "dataField",
                    data: fieldlist
                })
            }),
            showSelectionControls: true,
            selectionMode: "multiple",
            useNativeScrolling: true,
            onContentReady: function (e) {
                if (fieldlist != null && fieldlist.length > 0) {
                    var selectedItems = new Array();
                    for (var i = 0; i < fieldlist.length; i++) {
                        if (fieldlist[i].isVisible == true) {
                            selectedItems.push(fieldlist[i]);
                        }
                    }
                    e.component.option("selectedItems", selectedItems);
                }
                $('#query_fieldlist').dad({
                    target: "div[class='item']"
                });
            },
            onSelectionChanged: function (e) {
                var addedItems = e.addedItems;
                var removedItems = e.removedItems;
                if (addedItems != null && addedItems.length > 0) {
                    AddQueryAreaItems(addedItems);
                }
                if (removedItems != null && removedItems.length > 0) {
                    RemoveQueryAreaItems(removedItems);
                }
                $('#query_fieldlist').dad({
                    target: "div[class='item']"
                });
            }
        }).dxList("instance");


        $("#cboPageSize").dxSelectBox({
            items: [20, 50, 100],
            value: 20,
            onValueChanged: function (data) {
                //showPageData(data.value);
            }
        });

        var columnsList = new Array();
        var gridColumns = $("#gridContainer").dxDataGrid("instance").option("columns");
        if (gridColumns != null && gridColumns.length > 0) {
            for (var i = 0; i < gridColumns.length; i++) {
                columnsList.push({
                    dataField: gridColumns[i].dataField,
                    text: gridColumns[i].caption,
                    lock: "none",
                    bold: false,
                    visibleIndex: i,
                    isVisible: gridColumns[i].visible == undefined || gridColumns[i].visible==true
                })
            }
        }

        $("#listbox_grid").dxList({
            dataSource: new DevExpress.data.DataSource({
                store: new DevExpress.data.ArrayStore({
                    key: "dataField",
                    data: columnsList
                })
            }),
            searchEnabled: true,
            searchExpr: "text",
            showSelectionControls: true,
            selectionMode: "multiple",
            useNativeScrolling: true,
            onContentReady: function (e) {
                if(columnsList != null && columnsList.length > 0) {
                    var selectedItems = new Array();
                    for (var i = 0; i < columnsList.length; i++) {
                        if (columnsList[i].isVisible == true) {
                            selectedItems.push(columnsList[i]);
                        }
                    }
                    e.component.option("selectedItems", selectedItems);
                }
                $(".dx-placeholder").attr("data-dx_placeholder", "");
            },
            onSelectionChanged: function (e) {

            }
        }).dxList("instance");

        var dataGrid = $("#columnGrid").dxDataGrid({
            dataSource: columnsList,
            keyExpr: "dataField",
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
            allowColumnReordering: true,
            allowColumnResizing: true,
            showBorders: true,
            showColumnLines: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            columnsAutoWidth: true,
            filterRow: { visible: true },
            height: "300px",
            editing: {
                mode: "cell",
                allowUpdating: true
            },
            //filterPanel: { visible: true },
            headerFilter: { visible: true },
            scrolling: { mode: "standard", showScrollbar: "always", useNative: true },
            columnFixing: true,
            columnResizingMode: "widget",
            columns: [
                {
                    dataField: "text",
                    caption: "表头名称",
                    width: 120
                },
                {
                    dataField: "lock",
                    caption: "锁列",
                    width: 120,
                    showClearButton: true,
                    editorType: "dxSelectBox",
                    editorOptions: {
                        dataSource: [{ Name: "不锁列", Value: "none" }, { Name: "左边", Value: "left" }, { Name: "右边", Value: "right" }],
                        displayExpr: "Name",
                        valueExpr: "Value",
                        multiline: false,
                        placeholder: ""
                    }
                },
                {
                    dataField: "bold",
                    caption: "加粗",
                    width: 120,
                    editorType: "dxCheckBox"
                },
                {
                    dataField: "visibleIndex",
                    caption: "表头排序",
                    width: 120
                }
            ],
        }).dxDataGrid("instance");

        $("#btnMoveRight").dxButton({
            type: "default",
            text: ">>",
            onClick: function (e) {

            }
        });

        $("#btnOk").dxButton({
            type: "default",
            text: "确定",
            onClick: function (e) {
                //query
                var items = $("#listbox_query").dxList("instance").option("selectedItems");
                var queryFields = new Array();
                if (items != null && items.length > 0) {
                    for (var i = 0; i < items.length; i++) {
                        queryFields.push(items[i].dataField);
                    }
                }
                var formItems = $("#form").dxForm("instance").option("items");
                if (formItems != null && formItems.length > 0) {
                    for (var i = 0; i < formItems.length; i++) {
                        if (formItems[i].editorType != undefined) {
                            if ($.inArray(formItems[i].dataField, queryFields) > -1) {
                                $("#form").dxForm("instance").itemOption(formItems[i].dataField, "visible", true);
                            } else {
                                $("#form").dxForm("instance").itemOption(formItems[i].dataField, "visible", false);
                            }
                        }
                    }
                }

                //grid
                var gridItems = $("#listbox_grid").dxList("instance").option("selectedItems");
                var gridFields = new Array();
                if (gridItems != null && gridItems.length > 0) {
                    for (var i = 0; i < gridItems.length; i++) {
                        gridFields.push(gridItems[i].dataField);
                    }
                }
                var gridColumns = $("#gridContainer").dxDataGrid("instance").option("columns");
                if (gridColumns != null && gridColumns.length > 0) {
                    for (var i = 0; i < gridColumns.length; i++) {
                        if ($.inArray(gridColumns[i].dataField, gridFields) > -1) {
                            $("#gridContainer").dxDataGrid("instance").columnOption(gridColumns[i].dataField, "visible", true);
                            gridColumns[i].visible = true;
                        } else {
                            $("#gridContainer").dxDataGrid("instance").columnOption(gridColumns[i].dataField, "visible", false);
                            gridColumns[i].visible = false;
                        }
                    }
                }
                popup_setting.hide();
                $("#setting").attr("style", "display: none;");
            }
        });

        $("#btnCancel").dxButton({
            type: "normal",
            text: "取消",
            onClick: function (e) {
                popup_setting.hide();
                $("#setting").attr("style", "display: none;");
            }
        });



    }
};

if (popup_setting) {
    $(".popup").remove();
}

function showPopup_setting() {
    var $popupContainer = $("<div />")
        .addClass("popup")
        .appendTo($("#popup_setting"));
    popup_setting = $popupContainer.dxPopup(popupOptions_setting).dxPopup("instance");
    popup_setting.show();
}

$(".dx-icon-columnchooser").click(function () {
    showPopup_setting();
});

function AddQueryAreaItems(data) {
    for (var i = 0; i < data.length; i++) {
        $("#defaultArea").append("<div class='item' data-field='" + data[i].dataField + "'><span>" + data[i].text + "</span></div>");
    }
}

function RemoveQueryAreaItems(data) {
    var divs = $("#defaultArea").find("div");
    if (divs != null && divs.length > 0) {
        for (var i = 0; i < divs.length; i++) {
            for (var j = 0; j < data.length; j++) {
                if ($(divs[i]).attr("data-field") == data[j].dataField) {
                    $(divs[i]).remove();
                    break;
                }
            }
        }
    }


}

