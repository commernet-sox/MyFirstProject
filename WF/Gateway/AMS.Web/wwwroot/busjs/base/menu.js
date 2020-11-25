$(function () {
    DevExpress.config({
        editorStylingMode: "outlined"
    });

    var form = $("#form").dxForm({
        readOnly: false,
        showColonAfterLabel: true,
        labelLocation: "left",
        minColWidth: 300,
        colCount: 4,
        items: [
            {
                dataField: "MId",
                label: { text: "菜单编码" },
                editorType: "dxTextBox"
            },
            {
                dataField: "Mname",
                label: { text: "菜单名称" },
                editorType: "dxTextBox"
            },
            {
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
                    showClearButton: true,
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
                        formData.PageSize = 5000;
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "/menu/getlist",
                            data: formData,
                            success: function (jsonData) {
                                $("#treelist").dxTreeList("instance").option("dataSource", jsonData.Data);
                                $("#treelist").dxTreeList("instance").refresh();
                                $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text");
                                toolbar.option("items[2].template", "共" + jsonData.Total + "条");
                                loadPanel.hide();
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
                    return "共0条";
                }
            }
        ]
    }).dxToolbar("instance");

    var treeList = $("#treelist").dxTreeList({
        keyExpr: "Mid",
        parentIdExpr: "Pid",
        hasItemsExpr: "HasChildren",
        showRowLines: true,
        showColumnLines: true,
        showBorders: true,
        focusedRowEnabled: true,
        wordWrapEnabled: true,
        columnAutoWidth: true,
        rootValue: "/",
        width: "auto",
        height: $(window).height() - 200,
        scrolling: { mode: "standard", showScrollbar: "always", useNative: true },
        columns: [{
            dataField: "Mid",
            caption: "菜单编码",
            fixed: true,
            width: 200,
            cellTemplate: function (container, options) {
                var link = "<a class='dx-link dx-link-edit'";
                link += " onclick = \"showPopup('" + options.value + "');\">" + options.value + "</a>";
                $("<div>")
                    .append($(link))
                    .appendTo(container);
            }
        }, {
            dataField: "Mname",
            width: 200,
            caption: "菜单名称"
        },
        {
            dataField: "Pid",
            caption: "上级菜单",
            width: 200,
        },
        {
            dataField: "SysCode",
            caption: "所属系统",
            width: 200,
        }, {
            dataField: "IsSystem",
            caption: "系统内置",
            width: 120,
        }, {
            dataField: "MOrder",
            caption: "顺序",
            width: 120,
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
            dataField: "Mid",
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
    }).dxTreeList("instance");
});

var popup = null;
var popupOptions = {
    width: "100%",
    height: "100%",
    contentTemplate: function () {
        $("#content").attr("style", "");
        return $("<div />").append(
            $("#content")
        );
    },
    showTitle: true,
    title: "系统菜单详情",
    visible: false,
    dragEnabled: true,
    closeOnOutsideClick: false,
    showCloseButton: true,
    onShown: function () {
        var btnOk = $("#detailForm").find("div[aria-label='确定']");
        $(btnOk).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:80px;");
        var btnCancel = $("#detailForm").find("div[aria-label='取消']");
        $(btnCancel).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:-230px;");
        var txtMid = $("#detailForm").dxForm("instance").getEditor("Mid");
        txtMid.focus();

        var formData = $("#detailForm").dxForm("instance").option("formData");
        if (formData.Mid != undefined && formData.Mid != null) {
            loadPanel.show();
            $.ajax({
                type: "post",
                dataType: "json",
                url: "/menu/getcommands?mid=" + formData.Mid,
                success: function (data) {
                    $("#detailGrid").dxDataGrid("instance").option("dataSource", data);
                    $("#detailGrid").dxDataGrid("instance").refresh();
                    loadPanel.hide();
                },
                error: function (request, status, error) {
                    loadPanel.hide();
                    DevExpress.ui.notify(error);
                }
            });
        }

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

    var formData = null, curRowData = null;
    var rows = $("#treelist").dxTreeList("instance").getVisibleRows();
    if (rows != null && rows.length > 0) {
        var curId = $("#treelist").dxTreeList("instance").option("focusedRowKey");
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].data.Mid == curId) {
                curRowData = rows[i].data;
                break;
            }
        }
    }
    if (id == undefined || id == null) {//insert
        if (curRowData != null) {
            formData = {
                Pid: curRowData.Mid, Status: true, IsLastLevel: false, IsSystem: false,
                Layer: curRowData.Layer + 1,
                SysCode: curRowData.SysCode,
                MOrder: 0
            };
        } else {
            formData = { Pid: "/", Status: true, IsLastLevel: false, IsSystem: false, Layer: 0, MOrder: 0 };
        }
    } else {//modify
        formData = curRowData;
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
                colCount: 5,
                items: [{
                    dataField: "Mid",
                    label: { text: "菜单编码" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "菜单编码不能为空！"
                    }]
                },
                {
                    dataField: "Mname",
                    label: { text: "菜单名称" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "菜单名称不能为空！"
                    }]
                }, {
                    dataField: "Pid",
                    label: { text: "上级菜单" },
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
                        showClearButton: true,
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
                    dataField: "IsSystem",
                    label: { text: "系统内置" },
                    editorType: "dxCheckBox",
                    editorOptions: {
                        showSpinButtons: true,
                    }
                },
                {
                    dataField: "Mcode",
                    label: { text: "配置编码" },
                    editorType: "dxTextBox"
                },
                {
                    dataField: "OpenName",
                    label: { text: "反射名称" },
                    editorType: "dxTextBox"
                },
                {
                    dataField: "FileName",
                    label: { text: "反射文件" },
                    editorType: "dxTextBox"
                },
                {
                    dataField: "FileType",
                    label: { text: "反射方式" },
                    editorType: "dxSelectBox",
                    showClearButton: true,
                    editorOptions: {
                        dataSource: [{ ID: "0", NAME: "平台配置" },
                        { ID: "1", NAME: "文件配置" },
                        { ID: "2", NAME: "WEB配置" }],
                        displayExpr: "NAME",
                        valueExpr: "ID",
                        placeholder: ""
                    }
                },
                {
                    dataField: "ParamList",
                    label: { text: "参数列表" },
                    editorType: "dxTextBox"
                },
                {
                    dataField: "Layer",
                    label: { text: "显示顺序" },
                    editorType: "dxNumberBox",
                    editorOptions: {
                        showSpinButtons: true,
                    }
                },
                {
                    dataField: "ShortCut",
                    label: { text: "快捷键" },
                    editorType: "dxTextBox"
                }, {
                    dataField: "IsLastLevel",
                    label: { text: "是否末级节点" },
                    editorType: "dxCheckBox",
                    editorOptions: {
                        showSpinButtons: true,
                    }
                },
                ]
            },
            {
                itemType: "group",
                colSpan: 4,
                items: [{
                    dataField: "Memo",
                    label: { text: "备注" },
                    editorType: "dxTextBox",
                }]
            }
        ],
        onContentReady: function (e) {
            var txtMid = $("#detailForm").dxForm("instance").getEditor("Mid");
            if (id != undefined && id != null) {
                txtMid.option("disabled", true);
            }
            var txtPid = $("#detailForm").dxForm("instance").getEditor("Pid");
            txtPid.option("disabled", true);
            txtMid.focus();


        }
    }).dxForm("instance");

    var dataGrid = $("#detailGrid").dxDataGrid({
        paging: {
            enabled: false
        },
        showBorders: true,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        width: $(window).width(),
        height: 250,
        scrolling: { mode: "standard", showScrollbar: "always", useNative: true },
        columnFixing: true,
        columnResizingMode: "widget",
        editing: {
            mode: "cell",
            allowUpdating: true,
            allowDeleting: true
        },
        selection: {
            mode: "multiple",
            showCheckBoxesMode: "always"
        },
        columns: [{
            dataField: "CommandCode",
            caption: "命令编码",
            width: 120,
            validationRules: [{ type: "required" }]
        },
        {
            dataField: "CommandName",
            caption: "命令名称",
            width: 150,
            validationRules: [{ type: "required" }]
        },
        {
            dataField: "Location",
            caption: "位置",
            width: 150,
            lookup: {
                dataSource: [{ ID: "left", NAME: "左边" }, { ID: "center", NAME: "中间" }, { ID: "right", NAME: "右边" }],
                displayExpr: "NAME",
                valueExpr: "ID"
            }
        }, {
            dataField: "Icon",
            caption: "图标",
            width: 150
        }
            //    , {
            //    dataField: "CommandCode",
            //    caption: "操作",
            //    width: 200,
            //    fixed: true,
            //    fixedPosition: "right",
            //    cellTemplate: function (container, options) {
            //        $("<div>")
            //            .append($("<div style='display:inline-block;margin-right:5px;'><a class='dx-link dx-link-edit'>新增</a></div>"))
            //            .append($("<div style='display:inline-block;margin-right:5px;'><a onclick = \"showPopup('" + options.value + "');\" class='dx-link dx-link-edit'>删除</a></div>"))
            //            .appendTo(container);
            //    }
            //}
        ]
    }).dxDataGrid("instance");

    var contextMenu = $("#context-menu").dxContextMenu({
        items: [{ text: "新增" }, { text: "删除" }],
        onItemClick: function (ev) {
            var grid = $("#detailGrid").dxDataGrid('instance');
            var dataSource = grid.option("dataSource");
            if (ev.itemData.text == "新增") {
                if (dataSource == undefined || dataSource == null) {
                    var gridData = [{ Mid: "1", CommandCode: "", CommmandName: "", Location: "left" }];
                    $("#detailGrid").dxDataGrid("instance").option("dataSource", gridData);
                } else {
                    var gridData = { Mid: "1", CommandCode: "", CommmandName: "", Location: "left" };
                    dataSource.push(gridData);
                }
                grid.refresh();
            } else if (ev.itemData.text == "删除") {
                var selectedData = grid.getSelectedRowsData();
                if (selectedData == null || selectedData.length < 1) {
                    DevExpress.ui.notify("请选择要删除的行！");
                    return;
                }
                $(selectedData).each(function (index, element) {
                    var delIndex = dataSource.indexOf(element);
                    dataSource = dataSource.splice(delIndex, 1);
                });
                grid.refresh();
            }
        }
    }).dxContextMenu("instance");

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
                        $("#detailGrid").dxDataGrid("instance").saveEditData();
                        loadPanel.show();
                        var detailFormData = $("#detailForm").dxForm("instance").option("formData");
                        var commands = $("#detailGrid").dxDataGrid("instance").option("dataSource");
                        if (commands != null && commands.length > 0) {
                            for (var i = 0; i < commands.length; i++) {
                                if (commands[i].CommandCode == null || commands[i].CommandCode == "") {
                                    loadPanel.hide();
                                    DevExpress.ui.notify("存在为空的数据，允许提交！");
                                    return;
                                }
                                commands[i].Mid = detailFormData.Mid;
                            }
                        } else {
                            loadPanel.hide();
                            DevExpress.ui.notify("功能菜单必须包含一个命令！");
                            return;
                        }
                        if (id != undefined && id != null) {
                            $.ajax({
                                type: "post",
                                dataType: "json",
                                url: "/menu/update",
                                data: { Menu: detailFormData, Commands: commands },
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
                                    loadPanel.hide();
                                    DevExpress.ui.notify(error);
                                }
                            });
                        } else {
                            $.ajax({
                                type: "post",
                                dataType: "json",
                                url: "/menu/insert",
                                data: { Menu: detailFormData, Commands: commands },
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
                                    loadPanel.hide();
                                    DevExpress.ui.notify(error);
                                }
                            });
                        }
                    }
                }
            }, {
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
