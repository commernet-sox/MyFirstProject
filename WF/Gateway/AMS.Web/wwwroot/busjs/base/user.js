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
                            url: "/user/getlist",
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
                                            url: "/user/getlist",
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
        keyExpr: "UserId",
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
            dataField: "UserId",
            caption: "用户编码",
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
            dataField: "EmployeeId",
            caption: "工号",
            width: 150
        },
        {
            dataField: "CompanyId",
            caption: "公司编号",
            width: 200
        }, {
            dataField: "DefaultCkId",
            caption: "默认Id",
            width: 120,
        },
        {
            dataField: "Status",
            caption: "状态",
            width: 120,
            editorType: "dxCheckBox"
        },
        {
            dataField: "Remark",
            caption: "备注",
            width: 120
        }, {
            dataField: "CreateDate",
            caption: "创建日期",
            width: 200
        }, {
            dataField: "ModifyDate",
            caption: "修改日期",
            width: 200
        }, {
            dataField: "ExpireDate",
            caption: "到期日期",
            width: 200
        }, {
            dataField: "ForbidLoginDate",
            caption: "禁止登录日期",
            width: 200
        }, {
            dataField: "ENForcePWDPolicy",
            caption: "强制密码策略",
            width: 200
        }, {
            dataField: "ENForceExpirePolicy",
            caption: "强制密码过期",
            width: 200
        }, {
            dataField: "PWDPolicyType",
            caption: "密码类型",
            width: 200
        }, {
            dataField: "Ex1",
            caption: "Ex1",
            width: 120
        }, {
            dataField: "Ex2",
            caption: "Ex2",
            width: 120
        }, {
            dataField: "Ex3",
            caption: "Ex3",
            width: 200
        }, {
            dataField: "Ex4",
            caption: "Ex4",
            width: 120
        }, {
            dataField: "Pdapwd",
            caption: "PDA密码",
            width: 200
        },
        {
            dataField: "Sex",
            caption: "性别",
            width: 200
        },
        {
            dataField: "PersonName",
            caption: "姓名",
            width: 200
        },
        {
            dataField: "ID_Number",
            caption: "身份证号码",
            width: 200
        },
        {
            dataField: "Cell_Number",
            caption: "手机号",
            width: 200
        },
        {
            dataField: "User_Type",
            caption: "用户类型",
            width: 200
        },
        {
            dataField: "CreateBy",
            caption: "创建人",
            width: 200
        },
        {
            dataField: "ModifyBy",
            caption: "修改人",
            width: 200
        },
        {
            dataField: "DDSendId",
            caption: "钉钉编号",
            width: 200
        },
        {
            dataField: "UserId",
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
                //性别
                if (e.column.dataField == "Sex") {
                    if (e.data.Sex == "0") {
                        e.cellElement[0].innerText = "男";
                    } else if (e.data.Status == "1") {
                        e.cellElement[0].innerText = "女";
                    }
                }
                //状态
                if (e.column.dataField == "Status") {
                    if (e.data.Status == "0") {
                    } else if (e.data.Status == "1") {
                        e.cellElement[0].getElementsByTagName("span")[0].style.backgroundColor = '#03A9F4';
                    }
                }
                //密码类型
                if (e.column.dataField == "PWDPolicyType") {
                    if (e.data.PWDPolicyType == "P1") {
                        e.cellElement[0].innerText = "字母数字组合，至少6位";
                    } else if (e.data.Status == "P2") {
                        e.cellElement[0].innerText = "字母数字特殊符号组合，至少8位";
                    } else {
                        e.cellElement[0].innerText = "无";
                    }
                }
            }
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
    title: "用户详情",
    visible: false,
    dragEnabled: true,
    closeOnOutsideClick: false,
    showCloseButton: true,
    onShown: function () {
        var btnOk = $("#detailForm").find("div[aria-label='确定']");
        $(btnOk).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:80px;");
        var btnCancel = $("#detailForm").find("div[aria-label='取消']");
        $(btnCancel).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:-230px;");
        var txtUserId = $("#detailForm").dxForm("instance").getEditor("UserId");
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
                if (rows[i].data.UserId == id) {
                    formData = rows[i].data;
                    break;
                }
            }
            var code = [];
            for (var j = 0; j < formData.SysCode.length; j++) {
                code.push({ "ID": formData.SysCode[j] });
            }
            formData.Code = code;
            console.log(formData);
        }
    } else {
        formData = { Status: true, Level: 0, ShowOrder: 0, PwdpolicyType: 'P0', UserPwd: '8888', ENForcePWDPolicy: false, ENForceExpirePolicy: false, Sex: '0' };
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
                items: [{
                    dataField: "UserId",
                    label: { text: "用户编码" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "用户编码不能为空！"
                    }]
                },
                {
                    dataField: "EmployeeId",
                    label: { text: "工号" },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        dataSource: makeAsyncDataSource("employee"),
                        displayExpr: "NAME",
                        valueExpr: "ID",
                        placeholder: "",
                        searchEnabled: true,
                    },
                    validationRules: [{
                        type: "required",
                        message: "工号不能为空！"
                    }]
                },
                {
                    dataField: "CompanyId",
                    label: { text: "公司编号" },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        dataSource: makeAsyncDataSource("company"),
                        displayExpr: "NAME",
                        valueExpr: "ID",
                        placeholder: ""
                    },
                    validationRules: [{
                        type: "required",
                        message: "公司编号不能为空！"
                    }]
                },
                //{
                //    dataField: "DefaultCkId",
                //    label: { text: "默认id" },
                //    editorType: "dxTextBox",
                //},
                {
                    dataField: "DDSendId",
                    label: { text: "钉钉编号" },
                    editorType: "dxTextBox",
                },
                {
                    dataField: "ExpireDate",
                    colSpan: 2,
                    label: { text: "失效时间" },
                    editorType: "dxDateBox",
                    dateSerializationFormat: "yyyy-MM-dd HH:mm:ss",
                    editorOptions: {
                        width: "100%"
                    },
                },
                {
                    dataField: "ForbidLoginDate",
                    colSpan: 2,
                    label: { text: "禁止登录时间" },
                    editorType: "dxDateBox",
                    dateSerializationFormat: "yyyy-MM-dd HH:mm:ss",
                    editorOptions: {
                        width: "100%"
                    },
                },
                {
                    dataField: "Status",
                    label: { text: "状态" },
                    editorType: "dxCheckBox"
                },
                {
                    dataField: "ENForcePWDPolicy",
                    label: { text: "强制密码策略" },
                    editorType: "dxCheckBox"
                },
                {
                    dataField: "ENForceExpirePolicy",
                    label: { text: "强制密码过期" },
                    editorType: "dxCheckBox"
                },
                {
                    dataField: "PwdpolicyType",
                    label: { text: "密码类型" },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        dataSource: [{ ID: "P0", NAME: "无" }, { ID: "P1", NAME: "字母数字组合，至少6位" }, { ID: "P2", NAME: "字母数字特殊符号组合，至少8位" }],
                        displayExpr: "NAME",
                        valueExpr: "ID",
                        placeholder: "",
                    },
                    //validationRules: [{
                    //    type: "required",
                    //    message: "密码类型不能为空！"
                    //}]
                },
                //{
                //    dataField: "Ex1",
                //    label: { text: "Ex1" },
                //    editorType: "dxTextBox",
                //},
                //{
                //    dataField: "Ex2",
                //    label: { text: "Ex2" },
                //    editorType: "dxTextBox",
                //},
                //{
                //    dataField: "Ex3",
                //    label: { text: "Ex3" },
                //    editorType: "dxTextBox",
                //},
                //{
                //    dataField: "Ex4",
                //    label: { text: "Ex4" },
                //    editorType: "dxTextBox",
                //},
                {
                    dataField: "Pdapwd",
                    label: { text: "PDA密码" },
                    editorType: "dxTextBox",
                    //validationRules: [{
                    //    type: "required",
                    //    message: "PDA密码不能为空！"
                    //}]
                },
                {
                    dataField: "Sex",
                    label: { text: "性别" },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        dataSource: [{ ID: "0", NAME: "男" }, { ID: "1", NAME: "女" }],
                        displayExpr: "NAME",
                        valueExpr: "ID",
                        placeholder: ""
                    }
                },
                {
                    dataField: "PersonName",
                    label: { text: "姓名" },
                    editorType: "dxTextBox",
                },
                {
                    dataField: "ID_Number",
                    label: { text: "身份证号码" },
                    editorType: "dxTextBox",
                },
                {
                    dataField: "Cell_Number",
                    label: { text: "手机号" },
                    editorType: "dxTextBox",
                },
                {
                    dataField: "UserPwd",
                    label: { text: "密码" },
                    editorType: "dxTextBox",
                    //validationRules: [{
                    //    type: "required",
                    //    message: "密码不能为空！"
                    //},
                    //{
                    //    type: "async",
                    //    message: "密码和密码类型不匹配",
                    //    validationCallback: function (params) {
                    //        return sendRequest(params.value);
                    //    }
                    //}]
                },
                {
                    dataField: "User_Type",
                    label: { text: "用户类型" },
                    editorType: "dxTextBox",
                },
                {
                    dataField: "SysCode",
                    label: { text: "系统权限" },
                    editorType: "dxDropDownBox",
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
                                        caption: "系统编码"
                                    }, {
                                        dataField: "NAME",
                                        caption: "系统名称"
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

                                        if (selectedItems == null) {
                                            $("#detailForm").dxForm("instance").getEditor("DefaultSysCode").option("dataSource", null);
                                        } else {
                                            $("#detailForm").dxForm("instance").getEditor("DefaultSysCode").option("dataSource", selectedItems.selectedRowsData);
                                        }
                                    },
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
                    },
                    validationRules: [{
                        type: "required",
                        message: "系统权限不能为空！"
                    }]
                },
                {
                    dataField: "DefaultSysCode",
                    label: { text: "默认系统" },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        dataSource: formData.Code,
                        displayExpr: "ID",
                        valueExpr: "ID",
                        placeholder: ""
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
                                    url: "/user/update",
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
                                    url: "/user/insert",
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
            var txtUserId = $("#detailForm").dxForm("instance").getEditor("UserId");
            if (id != undefined && id != null) {
                txtUserId.option("disabled", true);
            }
            txtUserId.focus();
        }
    }).dxForm("instance");
}
