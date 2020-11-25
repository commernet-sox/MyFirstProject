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
                        var formData = { PageIndex: 0, PageSize: 5000 };
                        $.ajax({
                            type: "get",
                            dataType: "json",
                            url: "/roleuser/GetRoleUserCommand",
                            //data: formData,
                            success: function (jsonData) {
                                $("#treelist1").dxTreeList("instance").option("dataSource", jsonData);
                                $("#treelist1").dxTreeList("instance").refresh();
                                //$("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text");
                                loadPanel.hide();
                            },
                            error: function (request, status, error) {
                                DevExpress.ui.notify(error);
                            }
                        });

                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "/role/getlist",
                            data: { PageIndex: 0, PageSize: 5000, sysCode: sysCode},
                            success: function (data) {
                                $("#treelist").dxTreeList("instance").option("dataSource", data.Data);
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
                        var users = $("#treelist1").dxTreeList("instance").getSelectedRowsData("leavesOnly");
                        if (users == null || users.length < 1) {
                            DevExpress.ui.notify("请选择用户！");
                            return;
                        }
                        //var UserId = new Array();
                        //for (var i = 0; i < users.length; i++) {
                        //    UserId.push(users[i].UserId);
                        //}
                        //var UserId = users[0].UserId;
                        var rows = $("#treelist").dxTreeList("instance").getVisibleRows();
                        if (rows == null || rows.length < 1) {
                            DevExpress.ui.notify("当前没有角色，请先添加系统角色！");
                            return;
                        }
                        var rows = $("#treelist").dxTreeList("instance").getSelectedRowsData("leavesOnly");
                        var data = new Array();
                        if (rows != null && rows.length > 0) {
                            for (var j = 0; j < users.length; j++) {
                                for (var i = 0; i < rows.length; i++) {
                                    data.push({
                                        UserId: users[j].Mid,
                                        RoleId: rows[i].RoleId,
                                    });
                                }
                            }
                        }
                        if (data.length < 1) {
                            DevExpress.ui.notify("请勾选角色！");
                            return;
                        }
                        loadPanel.show();
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "/roleuser/update",
                            data: { dto: data },
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

    var treelist1 = $("#treelist1").dxTreeList({
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
        width: "30%",
        height: $(window).height() - 130,
        scrolling: { mode: "standard", showScrollbar: "always", useNative: true },
        columns: [{
            dataField: "Mid",
            caption: "用户编码",
            width: 200,
        }, {
            dataField: "Mname",
            width: 200,
            caption: "用户名称"
        }
        ],
        onCellClick: function (e) {
            if (e.rowType == "data") {
                loadMenu(e.data.Mid);
            }
        }
    }).dxTreeList("instance");

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
            dataField: "RoleId",
            caption: "角色编码",
            width: 200,
        }, {
            dataField: "RoleName",
            width: 200,
            caption: "角色名称"
        }
        ]
    }).dxTreeList("instance");

});


function loadMenu(UserId) {
    loadPanel.show();
    $.ajax({
        type: "post",
        dataType: "json",
        url: "/roleuser/GetRoleUserMenu",
        data: { UserId: UserId },
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
                    if (data[j].RoleId == node.children[i].data.RoleId ) {
                        keys.push(node.children[i].key);
                        break;
                    }
                }
            }
        }
    }
}


