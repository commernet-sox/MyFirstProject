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
                dataField: "EmployeeId",
                label: { text: "工号" },
                editorType: "dxTextBox"
            },
            {
                dataField: "PersonName",
                label: { text: "姓名" },
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
                            url: "/gateway/getlist",
                            //data: formData,
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
                                        formData.PageIndex = selectedPage - 1;
                                        formData.PageSize = pageSize;

                                        $.ajax({
                                            type: "post",
                                            dataType: "json",
                                            url: "/gateway/getlist",
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
        keyExpr: "ServiceName",
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
            dataField: "ServiceName",
            caption: "服务名称",
            width: 250,
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
            dataField: "DownstreamPathTemplate",
            caption: "下游路径模板",
            width: 250
        },
        {
            dataField: "DownstreamScheme",
            caption: "下游协议类型",
            width: 250
        }, {
            dataField: "UpstreamPathTemplate",
            caption: "上游路径模板",
            width: 250,
        },
        {
            dataField: "ServiceName",
            caption: "操作",
            width: 250,
            fixed: true,
            fixedPosition: "right",
            cellTemplate: function (container, options) {
                $("<div>")
                    .append($("<div style='display:inline-block;margin-right:5px;'><a onclick = \"showPopup('" + options.value + "');\" class='dx-link dx-link-edit'>修改</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a onclick = \"showDelete('" + options.value + "');\" class='dx-link dx-link-edit'>删除</a></div>"))
                    .appendTo(container);
            }
        }
        ],
        onCellPrepared: function (e) {
            
        }
    }).dxDataGrid("instance");

    var sendRequest = function (value) {
        var d = $.Deferred();
        setTimeout(function () {
            var pwdType = $("#detailForm").dxForm("instance").option("formData").PwdpolicyType;
            if (pwdType == "P0") {
                d.resolve(value != "" && value != null && value != undefined);
                return d.promise();
            }
            else if (pwdType == "P1") {
                var patt = "/^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,12}$/";
                d.resolve(patt.test(value));
                return d.promise();
            }
            else if (pwdType == "P2") {
                var patt = "/^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{8,}$/";
                d.resolve(patt.test(value));
                return d.promise();
            }
            else {
                return d.promise();
            }
        }, 1000);
        //var d = $.Deferred();
        //setTimeout(function () {
        //    d.resolve(value === validEmail);
        //}, 1000);
        //return d.promise();
    }
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
    title: "网关配置详情",
    visible: false,
    dragEnabled: true,
    closeOnOutsideClick: false,
    showCloseButton: true,
    onShown: function () {
        var btnOk = $("#detailForm").find("div[aria-label='确定']");
        $(btnOk).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:80px;");
        var btnCancel = $("#detailForm").find("div[aria-label='取消']");
        $(btnCancel).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:-230px;");
        var txtUserId = $("#detailForm").dxForm("instance").getEditor("ServiceName");
        txtUserId.focus();
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
                if (rows[i].data.ServiceName == id) {
                    formData = rows[i].data;
                    break;
                }
            }
            console.log(formData);
        }
    } else {
        formData = { ServiceName: 'xxService', DownstreamPathTemplate: '/{url}', DownstreamScheme:'http', UpstreamPathTemplate: '/xxx/{url}' };
    }



    $("#detailForm").dxForm({
        formData: formData,
        readOnly: false,
        showColonAfterLabel: true,
        labelLocation: "left",
        minColWidth: 300,
        colCount: 1,
        showValidationSummary: false,
        items: [
            {
                itemType: "group",
                colCount: 4,
                items: [
                {
                    dataField: "ServiceName",
                    label: { text: "服务名称" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "服务名称不能为空！"
                    }]
                },
                {
                    dataField: "DownstreamPathTemplate",
                    label: { text: "下游路径模板" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "下游路径模板不能为空！"
                    }]
                },
                {
                    dataField: "DownstreamScheme",
                    label: { text: "下游协议类型" },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        dataSource: [{ ID: "http", NAME: "http" }, { ID: "https", NAME: "https" }],
                        displayExpr: "NAME",
                        valueExpr: "ID",
                        placeholder: "",
                    },
                    validationRules: [{
                        type: "required",
                        message: "下游协议类型不能为空！"
                    }]
                },
                {
                    dataField: "UpstreamPathTemplate",
                    label: { text: "上游路径模板" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "上游路径模板不能为空！"
                    }]
                },]
                
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
                                    url: "/gateway/update",
                                    data: { dto: detailFormData, id: id},
                                    success: function (result) {
                                        if (result.Code != 0) {
                                            loadPanel.hide();
                                            DevExpress.ui.notify(result.Message);
                                            return;
                                        }
                                        var rows = $("#gridContainer").dxDataGrid("instance").getVisibleRows();
                                        if (rows != null && rows.length > 0) {
                                            for (var i = 0; i < rows.length; i++) {
                                                if (rows[i].data.ServiceName == id) {
                                                    rows[i].data = detailFormData;
                                                    $("#gridContainer").dxDataGrid("instance").refresh();
                                                    break;
                                                }
                                            }
                                        }
                                        loadPanel.hide();
                                        $("div.dx-button").click();
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
                                    url: "/gateway/insert",
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
            var txtUserId = $("#detailForm").dxForm("instance").getEditor("ServiceName");
            if (id != undefined && id != null) {
                txtUserId.option("disabled", false);
            }
            txtUserId.focus();
        }
    }).dxForm("instance");
}

function showDelete(id) {
    var b = window.confirm("您确定要删除嘛?");
    if (b == true) {
        $.ajax({
            type: "post",
            dataType: "json",
            url: "/gateway/delete",
            data: { id:id },
            success: function (result) {
                if (result.Code != 0) {
                    //loadPanel.hide();
                    DevExpress.ui.notify(result.Message);
                    return;
                }
                //$("div.dx-button").click();
                //popup.hide();
            },
            error: function (request, status, error) {
                DevExpress.ui.notify(error);
            }
        });
    }
}
