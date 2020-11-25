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
        formData.PageIndex = 0;
        formData.PageSize = pageSize;
        $("div.dx-button").click();
    }

    var form = $("#form").dxForm({
        readOnly: false,
        showColonAfterLabel: true,
        labelLocation: "left",
        minColWidth: 300,
        colCount: 4,
        items: [
            {
                dataField: "RoleId",
                label: { text: "角色编码" },
                editorType: "dxTextBox"
            },
            {
                dataField: "RoleName",
                label: { text: "角色名称" },
                editorType: "dxTextBox"
            }, {
                dataField: "SysCode",
                label: { text: "所属系统" },
                editorType: "dxDropDownBox",
                validationRules: [{
                    type: "required",
                    message: "所属系统不能为空！"
                }],
                editorOptions: {
                    valueExpr: "ID",
                    placeholder: "",
                    displayExpr: "Name",
                    showClearButton: false,
                    dropDownOptions: { resizeEnabled: true },
                    dataSource: makeAsyncDataSource("system"),
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
                                    mode: "single",
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
                itemType: "button",
                horizontalAlignment: "left",
                buttonOptions: {
                    text: "查询",
                    type: "success",
                    useSubmitBehavior: true,
                    onClick: function (e) {
                        var validateResult = $("#form").dxForm("instance").validate();
                        if (!validateResult.isValid) return;
                        loadPanel.show();
                        $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text dx-state-disabled");
                        var formData = $("#form").dxForm("instance").option("formData");
                        formData.PageIndex = 0;
                        var pageSize = $("#pageSize").dxSelectBox("instance").option("value");
                        formData.PageSize = pageSize;
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "/role/getlist",
                            data: formData,
                            success: function (jsonData) {
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", jsonData.Data);
                                $("#gridContainer").dxDataGrid("instance").refresh();
                                $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text");
                                var pageCount = jsonData.Total % pageSize == 0 ? jsonData.Total / pageSize : parseInt(jsonData.Total / pageSize) + 1;
                                toolbar.option("items[2].template", "共" + jsonData.Total + "条，每页" + pageSize + "条，第1/" + pageCount + "页");
                                loadPanel.hide();

                                $(".pagination").Paging({
                                    isFirst: true,
                                    isPre: true,
                                    showRecordNum: pageSize,
                                    totalNum: jsonData.Total,
                                    isShow: false,
                                    showNum: function (selectedPage) {
                                        if (selectedPage < 1) return;
                                        loadPanel.show();
                                        pageSize = $("#pageSize").dxSelectBox("instance").option("value");
                                        formData.PageIndex = (selectedPage - 1) * pageSize;
                                        formData.PageSize = pageSize;

                                        $.ajax({
                                            type: "post",
                                            dataType: "json",
                                            url: "/role/getlist",
                                            data: formData,
                                            success: function (jsonData) {
                                                $("#gridContainer").dxDataGrid("instance").option("dataSource", jsonData.Data);
                                                $("#gridContainer").dxDataGrid("instance").refresh();

                                                var pageCount = jsonData.Total % pageSize == 0 ? jsonData.Total / pageSize : parseInt(jsonData.Total / pageSize) + 1;
                                                toolbar.option("items[2].template", "共" + jsonData.Total + "条，每页" + pageSize + "条，第" + selectedPage + "/" + pageCount + "页");
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
                    text: "新增",
                    onClick: function () {
                        showPopup();
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
        keyExpr: "RoleId",
        paging: {
            enabled: false
        },
        //selection: {
        //    mode: "multiple",
        //    showCheckBoxesMode: "always"
        //},
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
        height: $(window).height() - 250,
        //filterPanel: { visible: true },
        headerFilter: { visible: true },
        scrolling: { mode: "standard", showScrollbar: "always", useNative: true },
        columnFixing: true,
        columnResizingMode: "widget",
        columns: [{
            dataField: "RoleId",
            caption: "角色编码",
            width: 120,
            fixed: true,
            cellTemplate: function (container, options) {
                var link = "<a class='dx-link dx-link-edit'";
                link += " onclick = \"showPopup('" + options.value + "');\">" + options.value + "</a>";
                $("<div>")
                    .append($(link))
                    .appendTo(container);
            }
        },
        {
            dataField: "RoleName",
            caption: "角色名称",
            width: 150
        },
        {
            dataField: "SysCode",
            caption: "所属系统",
            width: 200
        }, {
            dataField: "Status",
            caption: "状态",
            width: 120,
            editorType: "dxCheckBox"
        },
        {
            dataField: "IsSysRole",
            caption: "系统内置",
            width: 120
        },
        {
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
            dataField: "RoleId",
            caption: "操作",
            width: 120,
            fixed: true,
            fixedPosition: "right",
            cellTemplate: function (container, options) {
                $("<div>")
                    .append($("<div style='display:inline-block;margin-right:5px;'><a onclick = \"showPopup('" + options.value + "');\" class='dx-link dx-link-edit'>修改</a></div>"))
                    .appendTo(container);
            }
        }
        ]
    }).dxDataGrid("instance");
});

var popup = null;
var popupOptions = {
    width: "50%",
    height: "50%",
    contentTemplate: function () {
        $("#content").attr("style", "");
        return $("<div />").append(
            $("#content")
        );
    },
    showTitle: true,
    title: "角色详情",
    visible: false,
    dragEnabled: true,
    closeOnOutsideClick: false,
    showCloseButton: true,
    onShown: function () {
        var btnOk = $("#detailForm").find("div[aria-label='确定']");
        $(btnOk).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:80px;");
        var btnCancel = $("#detailForm").find("div[aria-label='取消']");
        $(btnCancel).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:-230px;");
        var txtRoleId = $("#detailForm").dxForm("instance").getEditor("RoleId");
        txtRoleId.focus();
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
    if (id != undefined && id != null) {
        var rows = $("#gridContainer").dxDataGrid("instance").getVisibleRows();
        if (rows != null && rows.length > 0) {
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].data.RoleId == id) {
                    formData = rows[i].data;
                    break;
                }
            }
        }
    } else {
        formData = { Status: true, IsSysRole: false };
    }

    $("#detailForm").dxForm({
        formData: formData,
        readOnly: false,
        showColonAfterLabel: true,
        labelLocation: "left",
        minColWidth: 300,
        colCount: 1,
        items: [
            {
                itemType: "group",
                colCount: 2,
                items: [{
                    dataField: "RoleId",
                    label: { text: "角色编码" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "角色编码不能为空！"
                    }]
                },
                {
                    dataField: "RoleName",
                    label: { text: "角色名称" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "角色名称不能为空！"
                    }]
                }, {
                    dataField: "SysCode",
                    label: { text: "所属系统" },
                    editorType: "dxDropDownBox",
                    validationRules: [{
                        type: "required",
                        message: "所属系统不能为空！"
                    }],
                    editorOptions: {
                        valueExpr: "ID",
                        placeholder: "",
                        displayExpr: "Name",
                        showClearButton: false,
                        dropDownOptions: { resizeEnabled: true },
                        dataSource: makeAsyncDataSource("system"),
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
                                        mode: "single",
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
                }, {
                    dataField: "Status",
                    label: { text: "状态" },
                    editorType: "dxCheckBox"
                }, {
                    dataField: "IsSysRole",
                    label: { text: "系统内置" },
                    editorType: "dxCheckBox",
                    editorOptions: {
                        showSpinButtons: true,
                    }
                }]
            }
        ],
        onContentReady: function (e) {
            var txtRoleId = $("#detailForm").dxForm("instance").getEditor("RoleId");
            if (id != undefined && id != null) {
                txtRoleId.option("disabled", true);
            }
            txtRoleId.focus();
        }
    }).dxForm("instance");

    var detailToolbar = $("#detailToolbar").dxToolbar({
        items: [
            {
                location: 'after',
                widget: 'dxButton',
                type: 'success',
                options: {
                    text: '确定',
                    onClick: function (e) {
                        var validateResult = $("#detailForm").dxForm("instance").validate();
                        if (!validateResult.isValid) return;
                        loadPanel.show();
                        var detailFormData = $("#detailForm").dxForm("instance").option("formData");
                        if (id != undefined && id != null) {
                            $.ajax({
                                type: "post",
                                dataType: "json",
                                url: "/role/update",
                                data: detailFormData,
                                success: function (result) {
                                    if (result.Code != 0) {
                                        loadPanel.hide();
                                        DevExpress.ui.notify(result.Message);
                                        return;
                                    }
                                    var rows = $("#gridContainer").dxDataGrid("instance").getVisibleRows();
                                    if (rows != null && rows.length > 0) {
                                        for (var i = 0; i < rows.length; i++) {
                                            if (rows[i].data.RoleId == id) {
                                                rows[i].data = detailFormData;
                                                $("#gridContainer").dxDataGrid("instance").refresh();
                                                break;
                                            }
                                        }
                                    }
                                    loadPanel.hide();
                                    popup.hide();
                                },
                                error: function (request, status, error) {
                                    DevExpress.ui.notify(error);
                                }
                            });
                        } else {
                            $.ajax({
                                type: "post",
                                dataType: "json",
                                url: "/role/insert",
                                data: detailFormData,
                                success: function (result) {
                                    if (result.Code != 0) {
                                        loadPanel.hide();
                                        DevExpress.ui.notify(result.Message);
                                        return;
                                    }
                                    $("div.dx-button").click();
                                    popup.hide();
                                },
                                error: function (request, status, error) {
                                    DevExpress.ui.notify(error);
                                }
                            });
                        }
                    }
                }
            },
            {
                location: 'after',
                widget: 'dxButton',
                locateInMenu: 'auto',
                options: {
                    text: "取消",
                    onClick: function () {
                        popup.hide();
                    }
                }
            }

        ]
    }).dxToolbar("instance");
}
