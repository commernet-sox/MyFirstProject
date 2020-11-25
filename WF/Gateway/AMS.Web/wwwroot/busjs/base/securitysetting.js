$(function () {
    showPopup(null);
});

var popup = null;
var popupOptions = {
    width: "40%",
    height: "60%",
    contentTemplate: function () {
        $("#content").attr("style", "");
        return $("<div />").append(
            $("#content")
        );
    },
    showTitle: true,
    title: "安全设置",
    visible: false,
    dragEnabled: true,
    closeOnOutsideClick: false,
    showCloseButton: true,
    onShown: function () {
        var btnOk = $("#detailForm").find("div[aria-label='确定']");
        $(btnOk).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:80px;");
        var btnCancel = $("#detailForm").find("div[aria-label='取消']");
        $(btnCancel).parent().parent().parent().attr("style", "display: flex; min-width: auto; flex: 1 1 0px;margin-left:-230px;");
        var txtUserId = $("#detailForm").dxForm("instance").getEditor("PwdType");
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
                if (rows[i].data.SysCode == id) {
                    formData = rows[i].data;
                    break;
                }
            }
        }
    } else {
        formData = { Status: true, Level: 0, ShowOrder: 0 };
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
                colCount: 1,
                items: [{
                    dataField: "PwdType",
                    label: { text: "密码类型" },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        dataSource: [{ ID: "0", NAME: "用户密码" }, { ID: "1", NAME: "PDA密码" }],
                        displayExpr: "NAME",
                        valueExpr: "ID",
                        placeholder: ""
                    },
                    validationRules: [{
                        type: "required",
                        message: "密码类型不能为空！"
                    }]
                },
                {
                    dataField: "OldPwd",
                    label: { text: "原密码" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "原密码不能为空！"
                    }]
                },
                {
                    dataField: "NewPwd",
                    label: { text: "新密码" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "新密码不能为空！"
                    }]
                },
                {
                    dataField: "ComfirmPwd",
                    label: { text: "确认新密码" },
                    editorType: "dxTextBox",
                    validationRules: [{
                        type: "required",
                        message: "确认新密码不能为空！"
                    }]
                }
                ]
            }
            
        ],
        
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
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "/securitysetting/update",
                            data: detailFormData,
                            success: function (result) {
                                if (result.Code != 0) {
                                    loadPanel.hide();
                                    DevExpress.ui.notify(result.Message);
                                    return;
                                }
                                loadPanel.hide();
                                $("div.dx-button").click();
                                popup.hide();
                            },
                            error: function (request, status, error) {
                                DevExpress.ui.notify(error);
                            }
                        });  
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
