$(function () {
    DevExpress.config({
        editorStylingMode: "outlined"
    });

    var toolbar = $("#toolbar").dxToolbar({
        items: [
            {
                location: 'before',
                widget: 'dxTabs',
                template: function () {
                    return "所属系统:";
                }
            },
            {
                location: 'before',
                widget: 'dxSelectBox',
                options: {
                    dataSource: makeAsyncDataSource("system"),
                    displayExpr: "NAME",
                    valueExpr: "ID",
                    placeholder: "",
                    width: 200,
                    onValueChanged: function (data) {
                        var sysCode = [data.value];
                        loadPanel.show();
                        $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text dx-state-disabled");
                        var formData = { PageIndex: 0, PageSize: 5000, sysCode: sysCode };
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "/role/getlist",
                            data: formData,
                            success: function (jsonData) {
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", jsonData.Data);
                                $("#gridContainer").dxDataGrid("instance").refresh();
                                $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text");
                                loadPanel.hide();
                            },
                            error: function (request, status, error) {
                                DevExpress.ui.notify(error);
                            }
                        });

                        $.ajax({
                            type: "get",
                            dataType: "json",
                            url: "/rolemenu/getmenucommand",
                            data: { sysCode: data.value },
                            success: function (data) {
                                $("#treelist").dxTreeList("instance").option("dataSource", data);
                                $("#treelist").dxTreeList("instance").refresh();
                                loadPanel.hide();
                            },
                            error: function (request, status, error) {
                                DevExpress.ui.notify(error);
                            }
                        });
                    },
                }
            },
            {
                location: 'before',
                widget: 'dxButton',
                options: {
                    text: '保存',
                    onClick: function (e) {
                        var roles = $("#gridContainer").dxDataGrid("instance").getSelectedRowsData();
                        if (roles == null || roles.length < 1) {
                            DevExpress.ui.notify("请选择角色！");
                            return;
                        }
                        var roleId = roles[0].RoleId;
                        var rows = $("#treelist").dxTreeList("instance").getVisibleRows();
                        if (rows == null || rows.length < 1) {
                            DevExpress.ui.notify("当前没有菜单，请先添加系统菜单！");
                            return;
                        }
                        var rows = $("#treelist").dxTreeList("instance").getSelectedRowsData("leavesOnly");
                        var data = new Array();
                        if (rows != null && rows.length > 0) {
                            for (var i = 0; i < rows.length; i++) {
                                data.push({
                                    RoleId: roleId,
                                    Mid: rows[i].Pid,
                                    CommandId: rows[i].Mid
                                });
                            }
                        }
                        if (data.length < 1) {
                            DevExpress.ui.notify("请勾选菜单命令！");
                            return;
                        }
                        loadPanel.show();
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "/rolemenu/update",
                            data: { menus: data },
                            success: function (result) {
                                if (result.Code != 0) {
                                    loadPanel.hide();
                                    DevExpress.ui.notify(result.Message);
                                    return;
                                }
                                DevExpress.ui.notify("保存成功！");
                                loadPanel.hide();
                            },
                            error: function (request, status, error) {
                                DevExpress.ui.notify(error);
                                loadPanel.hide();
                            }
                        });
                    }
                }
            }
        ]
    }).dxToolbar("instance");

    var dataGrid = $("#gridContainer").dxDataGrid({
        keyExpr: "RoleId",
        paging: {
            enabled: false
        },
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        allowColumnReordering: true,
        allowColumnResizing: true,
        showBorders: true,
        showColumnLines: true,
        showRowLines: true,
        columnsAutoWidth: true,
        filterRow: { visible: true },
        width: "30%",
        height: $(window).height() - 130,
        headerFilter: { visible: true },
        scrolling: { mode: "standard", showScrollbar: "always", useNative: true },
        columnFixing: true,
        columnResizingMode: "widget",
        columns: [{
            dataField: "RoleId",
            caption: "角色编码",
            width: 120
        },
        {
            dataField: "RoleName",
            caption: "角色名称",
            width: 150
        }
        ],
        onCellClick: function (e) {
            if (e.rowType == "data") {
                loadMenu(e.data.RoleId);
            }
        }
    }).dxDataGrid("instance");

    var treeList = $("#treelist").dxTreeList({
        showRowLines: true,
        showColumnLines: true,
        showBorders: true,
        wordWrapEnabled: true,
        columnAutoWidth: true,
        dataStructure: "tree",
        filterRow: { visible: true },
        itemsExpr: "items",
        selection: {
            mode: "multiple",
            recursive: true
        },
        width: "60%",
        height: $(window).height() - 130,
        scrolling: { mode: "standard", showScrollbar: "always", useNative: true },
        columns: [{
            dataField: "Mid",
            caption: "菜单编码",
            width: 200,
        }, {
            dataField: "Mname",
            width: 200,
            caption: "菜单名称"
        }
        ]
    }).dxTreeList("instance");

});


function loadMenu(roleId) {
    loadPanel.show();
    $.ajax({
        type: "post",
        dataType: "json",
        url: "/rolemenu/getrolemenu",
        data: { roleId: roleId },
        success: function (data) {
            var treeList = $("#treelist").dxTreeList("instance");
            if (data != null && data.length > 0) {
                var rootNode = treeList.getRootNode();
                var keys = new Array();
                recursiveTreelistNode(rootNode, keys, data);
                if (keys.length > 0) {
                    treeList.selectRows(keys, true);
                }
                treeList.refresh();
            } else {
                treeList.deselectAll();
            }
            loadPanel.hide();
        },
        error: function (request, status, error) {
            DevExpress.ui.notify(error);
        }
    });
}

function recursiveTreelistNode(node, keys, data) {
    if (node.hasChildren) {
        for (var i = 0; i < node.children.length; i++) {
            if (node.children[i].hasChildren && node.children[i].children.length > 0) {
                recursiveTreelistNode(node.children[i], keys, data);
            } else {
                for (var j = 0; j < data.length; j++) {
                    if (data[j].Mid == node.children[i].data.Pid && data[j].CommandId == node.children[i].data.Mid) {
                        keys.push(node.children[i].key);
                        break;
                    }
                }
            }
        }
    }
}
