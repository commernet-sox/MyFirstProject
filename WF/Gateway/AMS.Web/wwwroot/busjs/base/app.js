﻿$(function () {
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
                dataField: "AppKey",
                label: { text: "AppKey" },
                editorType: "dxTextBox"
            },
            {
                dataField: "AppName",
                label: { text: "AppName" },
                editorType: "dxTextBox"
            },
            {
                dataField: "AppType",
                label: { text: "AppType" },
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
                        $.PostData("/app/getlist",formData,
                             function (jsonData) {
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
                                        formData.PageIndex = selectedPage - 1;
                                        formData.PageSize = pageSize;

                                        $.PostData("/app/getlist", formData,
                                             function (jsonData) {
                                                $("#gridContainer").dxDataGrid("instance").option("dataSource", jsonData.Data);
                                                $("#gridContainer").dxDataGrid("instance").refresh();

                                                var pageCount = jsonData.Total % pageSize == 0 ? jsonData.Total / pageSize : parseInt(jsonData.Total / pageSize) + 1;
                                                toolbar.option("items[2].template", "共" + jsonData.Total + "条，每页" + pageSize + "条，第" + selectedPage + "/" + pageCount + "页");
                                                loadPanel.hide();
                                            },
                                            function (request, status, error) {
                                                DevExpress.ui.notify(error);
                                                loadPanel.hide();
                                            }
                                        );
                                    }
                                });
                            },
                            function (request, status, error) {
                                DevExpress.ui.notify(error);
                            }
                        );
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
        keyExpr: "AppKey",
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
            dataField: "AppKey",
            caption: "AppKey",
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
            dataField: "AppSecret",
            caption: "AppSecret",
            width: 150
        },
        {
            dataField: "SessionKey",
            caption: "SessionKey",
            width: 200
        }, {
            dataField: "RedirectUrl",
            caption: "RedirectUrl",
            width: 120
        },
        {
            dataField: "AppName",
            caption: "AppName",
            width: 120
        },
        {
            dataField: "AppType",
            caption: "AppType",
            width: 120
        }, {
            dataField: "AppClass",
            caption: "AppClass",
            width: 120
        }, {
            dataField: "Status",
            caption: "Status",
            width: 150
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
            dataField: "MofifyDate",
            caption: "修改时间",
            width: 200
        }, {
            dataField: "Memo",
            caption: "备注",
            width: 120
        },
        {
            dataField: "AppKey",
            caption: "操作",
            width: 160,
            fixed: true,
            fixedPosition: "right",
            cellTemplate: function (container, options) {
                $("<div>")
                    .append($("<div style='display:inline-block;margin-right:5px;'><a onclick = \"showPopup('" + options.value + "');\" class='dx-link dx-link-edit'>修改</a></div>"))
                    .append($("<div style='display:inline-block;margin-right:5px;'><a onclick = \"expressPararams('" + options.value + "');\" class='dx-link dx-link-edit'>配置快递参数</a></div>"))
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
    height: "65%",
    contentTemplate: function () {
        $("#content").attr("style", "");
        return $("<div />").append(
            $("#content")
        );
    },
    showTitle: true,
    title: "修改APP明细",
    visible: false,
    dragEnabled: true,
    closeOnOutsideClick: false,
    showCloseButton: true,
    onShown: function () {
        var btnOk = $("#detailForm").find("div[aria-label='确定']");
        $(btnOk).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:80px;");
        var btnCancel = $("#detailForm").find("div[aria-label='取消']");
        $(btnCancel).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:-230px;");
        var txtCompanyId = $("#detailForm").dxForm("instance").getEditor("CompanyId");
        txtCompanyId.focus();
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
                if (rows[i].data.AppKey == id) {
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
        readOnly: false,
        showColonAfterLabel: true,
        labelLocation: "left",
        minColWidth: 300,
        colCount: 1,
        items: [
            {
                itemType: "group",
                colCount: 4,
                items: [{
                    dataField: "AppKey",
                    label: { text: "AppKey" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "AppKey不能为空！"
                    }]
                },
                {
                    dataField: "AppSecret",
                    label: { text: "AppSecret" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "AppSecret不能为空！"
                    }]
                }, {
                    dataField: "SessionKey",
                    label: { text: "SessionKey" },
                    editorType: "dxTextBox"
                }, {
                    dataField: "RedirectUrl",
                    label: { text: "RedirectUrl" },
                    editorType: "dxTextBox"
                }, {
                    dataField: "AppName",
                    label: { text: "AppName" },
                    editorType: "dxTextBox"
                }, {
                    dataField: "AppIcon",
                    label: { text: "AppIcon" },
                    editorType: "dxTextBox"
                }, {
                    dataField: "AppType",
                    label: { text: "AppType" },
                    editorType: "dxTextBox"
                }, {
                    dataField: "AppClass",
                    label: { text: "AppClass" },
                    editorType: "dxTextBox"
                }, {
                    dataField: "Status",
                    label: { text: "Status" },
                    editorType: "dxCheckBox"
                }]
            },
            {
                itemType: "group",
                colSpan: 4,
                items: [{
                    dataField: "Memo",
                    label: { text: "备注" },
                    editorType: "dxTextBox",
                }]
            },
            {
                itemType: "group",
                colCount: 4,
                items: [{
                    itemType: "button",
                    horizontalAlignment: "left",
                    buttonOptions: {
                        text: "确定",
                        type: "success",
                        useSubmitBehavior: true,
                        onClick: function (e) {
                            var validateResult = $("#detailForm").dxForm("instance").validate();
                            if (!validateResult.isValid) return;
                            loadPanel.show();
                            var detailFormData = $("#detailForm").dxForm("instance").option("formData");
                            if (id != undefined && id != null) {
                                $.ajax({
                                    type: "post",
                                    dataType: "json",
                                    url: "/app/update",
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
                                                if (rows[i].data.CompanyId == id) {
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
                                    url: "/app/insert",
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
                }, {
                    itemType: "button",
                        horizontalAlignment: "left",
                    buttonOptions: {
                        text: "取消",
                        type: "normal",
                        onClick: function (e) {
                            popup.hide();
                        }
                    }
                }]
            }
        ],
        onContentReady: function (e) {
            var txtCompanyId = $("#detailForm").dxForm("instance").getEditor("AppKey");
            if (id != undefined && id != null) {
                txtCompanyId.option("disabled", true);
            }
            txtCompanyId.focus();
        }
    }).dxForm("instance");
}

function expressPararams(id) {
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
                if (rows[i].data.AppKey == id) {
                    formData = rows[i].data;
                    break;
                }
            }
        }
    } else {
        formData = { Status: true, Level: 0, ShowOrder: 0 };
    }
}
