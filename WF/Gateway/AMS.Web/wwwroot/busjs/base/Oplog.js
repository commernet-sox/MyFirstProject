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
        colCount: 5,
        items: [
            {
                dataField: "LogId",
                label: { text: "日志编码" },
                editorType: "dxTextBox"
            },
            {
                dataField: "LogType",
                label: { text: "日志类型" },
                editorType: "dxTextBox"
            },
            {
                dataField: "Mid",
                label: { text: "菜单编码" },
                editorType: "dxTextBox"
            },
            {
                dataField: "Mname",
                label: { text: "菜单名称" },
                editorType: "dxTextBox"
            },
            {
                itemType: "button",
                horizontalAlignment: "left",
                buttonOptions: {
                    text: "查询",
                    type: "success",
                    useSubmitBehavior: true,
                    onClick: function (e) {
                        loadPanel.show();
                        $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text dx-state-disabled");
                        var formData = $("#form").dxForm("instance").option("formData");
                        formData.PageIndex = 0;
                        var pageSize = $("#pageSize").dxSelectBox("instance").option("value");
                        formData.PageSize = pageSize;
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "/oplog/getlist",
                            data: formData,
                            success: function (jsonData) {
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", jsonData.Data);
                                $("#gridContainer").dxDataGrid("instance").refresh();
                                $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text");
                                var pageCount = jsonData.Total % pageSize == 0 ? jsonData.Total / pageSize : parseInt(jsonData.Total / pageSize) + 1;
                                toolbar.option("items[1].template", "共" + jsonData.Total + "条，每页" + pageSize + "条，第1/" + pageCount + "页");
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
                                        formData.PageIndex = selectedPage - 1;
                                        formData.PageSize = pageSize;

                                        $.ajax({
                                            type: "post",
                                            dataType: "json",
                                            url: "/oplog/getlist",
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
                widget: 'dxTabs',
                template: function () {
                    return "共0条，每页20条，第0/0页";
                }
            }
        ]
    }).dxToolbar("instance");

    var dataGrid = $("#gridContainer").dxDataGrid({
        keyExpr: "LogId",
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
            dataField: "LogId",
            caption: "日志编码",
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
            dataField: "LogType",
            caption: "日志类型",
            width: 150
            },
            {
                dataField: "SysCode",
                caption: "系统编码",
                width: 150
            },
            {
                dataField: "Mid",
                caption: "菜单编码",
                width: 150
            },
            {
                dataField: "Mname",
                caption: "菜单名称",
                width: 150
            },
            {
                dataField: "CommandId",
                caption: "命令编码",
                width: 150
            },
            {
                dataField: "CommandName",
                caption: "命令名称",
                width: 150
            },
            {
                dataField: "KeyId",
                caption: "KeyId",
                width: 150
            },
            {
                dataField: "TableName",
                caption: "TableName",
                width: 150
            },
            {
                dataField: "OperationEmp",
                caption: "操作人",
                width: 150
            },
            {
                dataField: "OperationDate",
                caption: "操作时间",
                width: 150
            },
            {
                dataField: "ClientName",
                caption: "终端名称",
                width: 150
            },
            {
                dataField: "Ip",
                caption: "Ip地址",
                width: 150
            }, {
            dataField: "Memo",
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
        },
        {
            dataField: "LogId",
            caption: "操作",
            width: 120,
            fixed: true,
            fixedPosition: "right",
            cellTemplate: function (container, options) {
                $("<div>")
                    .append($("<div style='display:inline-block;margin-right:5px;'><a onclick = \"showPopup('" + options.value + "');\" class='dx-link dx-link-edit'>查看详情</a></div>"))
                    .appendTo(container);
            }
        }
        ],
        onCellPrepared: function (e) {
            if (e.rowType == "data") {
                //状态
                //if (e.column.dataField == "Status") {
                //    if (e.data.Status == "0") {
                //        e.cellElement[0].innerText = "无效";
                //    } else if (e.data.Status == "1") {
                //        e.cellElement[0].innerText = "有效";
                //    }
                //}
            }
        }
    }).dxDataGrid("instance");
});

var popup = null;
var popupOptions = {
    width: "80%",
    height: "80%",
    contentTemplate: function () {
        $("#content").attr("style", "");
        return $("<div />").append(
            $("#content")
        );
    },
    showTitle: true,
    title: "日志详情",
    visible: false,
    dragEnabled: true,
    closeOnOutsideClick: false,
    showCloseButton: true,
    onShown: function () {
        var btnCancel = $("#detailForm").find("div[aria-label='关闭']");
        $(btnCancel).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:-230px;");
        var txtLogId = $("#detailForm").dxForm("instance").getEditor("LogId");
        txtLogId.focus();
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
                if (rows[i].data.LogId == id) {
                    formData = rows[i].data;
                    break;
                }
            }
        }
    } else {
        formData = {Status:true,Level:0,ShowOrder:0};
    }

    $("#detailForm").dxForm({
        formData: formData,
        readOnly: true,
        showColonAfterLabel: true,
        labelLocation: "left",
        minColWidth: 300,
        colCount: 1,
        items: [
            {
                itemType: "group",
                colCount: 3,
                items: [{
                    dataField: "LogId",
                    label: { text: "日志编码" },
                    editorType: "dxTextBox",
                },
                {
                    dataField: "LogType",
                    label: { text: "日志类型" },
                    editorType: "dxTextBox"
                }, {
                    dataField: "SysCode",
                    label: { text: "系统编码" },
                    editorType: "dxTextBox"
                }, {
                    dataField: "Mid",
                    label: { text: "菜单编码" },
                    editorType: "dxTextBox"
                    }, {
                    dataField: "CommandId",
                        label: { text: "命令编码" },
                        editorType: "dxTextBox"
                    }, {
                    dataField: "CommandName",
                        label: { text: "命令名称" },
                        editorType: "dxTextBox"
                    }, {
                    dataField: "KeyId",
                    label: { text: "KeyId" },
                        editorType: "dxTextBox"
                    }, {
                    dataField: "TableName",
                    label: { text: "TableName" },
                        editorType: "dxTextBox"
                    }, {
                    dataField: "OperationEmp",
                        label: { text: "操作人" },
                        editorType: "dxTextBox"
                    }, {
                    dataField: "OperationDate",
                        label: { text: "操作时间" },
                        editorType: "dxTextBox"
                    }, {
                    dataField: "ClientName",
                        label: { text: "终端名称" },
                        editorType: "dxTextBox"
                    }, {
                    dataField: "Ip",
                        label: { text: "IP 地址" },
                        editorType: "dxTextBox"
                    }, {
                    dataField: "CreateBy",
                        label: { text: "创建人" },
                        editorType: "dxTextBox"
                    }, {
                    dataField: "CreateDate",
                        label: { text: "创建时间" },
                        editorType: "dxTextBox"
                    }]
            },
            {
                itemType: "group",
                colSpan: 3,
                items: [ {
                    dataField: "Memo",
                    label: { text: "备注" },
                    editorType: "dxTextBox",
                }]
            },
            {
                itemType: "group",
                colCount: 3,
                items: [{
                    itemType: "button",
                        horizontalAlignment: "left",
                    buttonOptions: {
                        text: "关闭",
                        type: "normal",
                        onClick: function (e) {
                            popup.hide();
                        }
                    }
                }]
            }
        ],
        onContentReady: function (e) {
            var txtLogId = $("#detailForm").dxForm("instance").getEditor("LogId");
            if (id != undefined && id != null) {
                txtLogId.option("disabled", true);
            }
            txtLogId.focus();
        }
    }).dxForm("instance");
}
