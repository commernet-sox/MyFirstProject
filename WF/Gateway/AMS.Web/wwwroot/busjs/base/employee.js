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
        minColWidth: 200,
        colCount: 6,
        items: [
            {
                dataField: "EmployeeId",
                label: { text: "员工编码" },
                editorType: "dxTextBox"
            },
            {
                dataField: "EmployeeName",
                label: { text: "员工姓名" },
                editorType: "dxTextBox"
            }, {
                dataField: "CompanyId",
                label: { text: "所属公司" },
                editorType: "dxSelectBox",
                editorOptions: {
                    dataSource: makeAsyncDataSource("company"),
                    displayExpr: "NAME",
                    valueExpr: "ID",
                    placeholder: "",
                    onValueChanged: function (e) {
                        $.get("/Common/GetDepartmentList", { CompanyId: e.value }, function (data) {
                            $("#form").dxForm("instance").getEditor("DepartmentId").option("dataSource", data);
                        });
                    }
                }
            }, {
                dataField: "DepartmentId",
                label: { text: "所属部门" },
                editorType: "dxSelectBox",
                editorOptions: {
                    dataSource: makeAsyncDataSource("department"),
                    displayExpr: "NAME",
                    valueExpr: "ID",
                    placeholder: ""
                }
            },
            {
                itemType: "button",
                horizontalAlignment: "right",
                buttonOptions: {
                    text: "清空",
                    type: "success",
                    useSubmitBehavior: true,
                    onClick: function (e) {
                        loadPanel.show();
                        $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text dx-state-disabled");
                        $("#form").dxForm("instance").option("formData",null);
                        $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text");
                        loadPanel.hide();
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
                        loadPanel.show();
                        $("#form").find("div.dx-button").attr("class", "dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-text dx-state-disabled");
                        var formData = $("#form").dxForm("instance").option("formData");
                        formData.PageIndex = 0;
                        var pageSize = $("#pageSize").dxSelectBox("instance").option("value");
                        formData.PageSize = pageSize;
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "/employee/getlist",
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
                                        formData.PageIndex = selectedPage - 1;
                                        formData.PageSize = pageSize;

                                        $.ajax({
                                            type: "post",
                                            dataType: "json",
                                            url: "/employee/getlist",
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
        keyExpr: "EmployeeId",
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
            dataField: "EmployeeId",
            caption: "员工编码",
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
            dataField: "EmployeeName",
            caption: "员工姓名",
            width: 150
            }, {
            dataField: "IsFired",
                caption: "是否离职",
                width: 120,
                editorType: "dxCheckBox"
            },
            {
                dataField: "Idcard",
                caption: "ID卡号",
                width: 150
            },
            {
                dataField: "Sex",
                caption: "性别",
                width: 150
            }, {
                dataField: "EnterDate",
                caption: "入职时间",
                width: 200
            },
        {
            dataField: "CompanyId",
            caption: "所属公司",
            width: 120
            },
            {
                dataField: "BirthDate",
                caption: "员工生日",
                width: 120
            },
        {
            dataField: "Job",
            caption: "职位",
            width: 120
            }, {
                dataField: "Address",
                caption: "地址",
                width: 150
            }, {
                dataField: "Phone",
                caption: "电话",
                width: 200
            }, {
            dataField: "Mobile", 
                caption: "手机",
                width: 200
            }, {
                dataField: "ZipCode",
                caption: "邮编",
                width: 200
            }, {
                dataField: "EMail",
                caption: "邮箱",
                width: 200
            }, {
                dataField: "Memo", 
                caption: "备注",
                width: 120
            }, {
                dataField: "DepartmentId", 
                caption: "所属部门",
                width: 120
            }, {
            dataField: "LeaveDate",
                caption: "离职日期",
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
            dataField: "EmployeeId",
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
        ],
        onCellPrepared: function (e) {
            if (e.rowType == "data") {
                //状态
                if (e.column.dataField == "Sex") {
                    if (e.data.Sex == 0 ) {
                        e.cellElement[0].innerText = "男";
                    } else if (e.data.Sex == 1 ) {
                        e.cellElement[0].innerText = "女";
                    } 
                }
                else if (e.column.dataField == "IsFired") {
                    if (e.data.IsFired == 0) {
                        e.cellElement[0].innerText = "否";
                    } else if (e.data.IsFired == 1) {
                        e.cellElement[0].innerText = "是";
                    }
                }
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
    title: "员工详情",
    visible: false,
    dragEnabled: true,
    closeOnOutsideClick: false,
    showCloseButton: true,
    onShown: function () {
        var btnOk = $("#detailForm").find("div[aria-label='确定']");
        $(btnOk).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:80px;");
        var btnCancel = $("#detailForm").find("div[aria-label='取消']");
        $(btnCancel).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:-230px;");
        var txtEmployeeId = $("#detailForm").dxForm("instance").getEditor("EmployeeId");
        txtEmployeeId.focus();
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
                if (rows[i].data.EmployeeId == id) {
                    formData = rows[i].data;
                    break;
                }
            }
        }
    } else {
        formData = { IsFired: false, Level: 0, ShowOrder: 0 };
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
                    dataField: "EmployeeId",
                    label: { text: "员工编码" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "员工编码不能为空！"
                    }]
                },
                {
                    dataField: "EmployeeName",
                    label: { text: "员工姓名" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "员工姓名不能为空！"
                    }]
                    }, {
                        dataField: "CompanyId",
                        label: { text: "所属公司" },
                        editorType: "dxSelectBox",
                        editorOptions: {
                            dataSource: makeAsyncDataSource("company"),
                            displayExpr: "NAME",
                            valueExpr: "ID",
                            placeholder: "",
                    onValueChanged: function (e) {
                        $.get("/Common/GetDepartmentList", { CompanyId: e.value }, function (data) {
                            $("#detailForm").dxForm("instance").getEditor("DepartmentId").option("dataSource", data);
                        });
                    }
                    },
                    validationRules: [{
                        type: "required",
                        message: "所属公司不能为空！"
                    }]
                    }, {
                        dataField: "DepartmentId",
                        label: { text: "所属部门" },
                        editorType: "dxSelectBox",
                        editorOptions: {
                            dataSource: makeAsyncDataSource("department"),
                            displayExpr: "NAME",
                            valueExpr: "ID",
                            placeholder: ""
                    },
                    validationRules: [{
                        type: "required",
                        message: "所属部门不能为空！"
                    }]
                    }, {
                    dataField: "Idcard",
                        label: { text: "ID 卡号" },
                    editorType: "dxTextBox"
                    }, {
                    dataField: "Sex",
                        label: { text: "性  别" },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        dataSource: [{ "ID": 0, "NAME": "男" }, { "ID": 1, "NAME": "女" }],
                        displayExpr: "NAME",
                        valueExpr: "ID",
                        placeholder: ""
                    }
                    }, {
                        dataField: "Job",
                        label: { text: "职  位" },
                        editorType: "dxTextBox"
                    }, {
                    dataField: "EnterDate",
                        label: { text: "入职时间" },
                    editorType: "dxDateBox",
                    editorOptions: {
                        dateSerializationFormat: "yyyy-MM-dd HH:mm:ss",
                        displayFormat: "yyyy-MM-dd HH:mm:ss",
                        useMaskBehavior: true
                    }
                    }, {
                    dataField: "Phone",
                    label: { text: "电  话" },
                    editorType: "dxTextBox"
                    }, {
                        dataField: "Mobile",
                        label: { text: "手  机" },
                        editorType: "dxTextBox"
                    }, {
                        dataField: "IsFired",
                        label: { text: "是否离职" },
                        editorType: "dxCheckBox"
                    }, {
                    dataField: "BirthDate",
                        label: { text: "员工生日" },
                    editorType: "dxDateBox",
                    editorOptions: {
                        dateSerializationFormat: "yyyy-MM-dd HH:mm:ss",
                        displayFormat: "yyyy-MM-dd HH:mm:ss",
                        useMaskBehavior: true
                    }
                    }, {
                        dataField: "ZipCode",
                        label: { text: "邮  编" },
                        editorType: "dxTextBox"
                    }, {
                        dataField: "EMail",
                        label: { text: "邮  箱" },
                        editorType: "dxTextBox"
                    }, {
                        dataField: "LeaveDate",
                        label: { text: "离职日期" },
                    editorType: "dxDateBox",
                    editorOptions: {
                        dateSerializationFormat: "yyyy-MM-dd HH:mm:ss",
                        displayFormat: "yyyy-MM-dd HH:mm:ss",
                        useMaskBehavior: true
                    }
                    }]
            },
            {
                itemType: "group",
                colSpan: 4,
                items: [{
                    dataField: "Address",
                    label: { text: "地址" },
                    editorType: "dxTextBox",
                }, {
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
                                    url: "/employee/update",
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
                                                if (rows[i].data.EmployeeId == id) {
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
                                    url: "/employee/insert",
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
            var txtEmployeeId = $("#detailForm").dxForm("instance").getEditor("EmployeeId");
            if (id != undefined && id != null) {
                txtEmployeeId.option("disabled", true);
            }
            txtEmployeeId.focus();
        }
    }).dxForm("instance");
}
