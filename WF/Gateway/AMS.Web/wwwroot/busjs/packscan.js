window.onbeforeunload = function (e) {
    //var result = confirm("确定离开此页面吗？");
    //if (result) {
    //    var command = {
    //        WorkCenter: "",
    //        HZID: "",
    //        WMSBillId: "",
    //        MenuId: "M41",
    //        CartonNumber: "001",
    //        KdBillId: "",
    //        CTN_LINE_SEQ: 0,
    //        CommandType: 2
    //    };
    //    SendCommand("stopimmediately", JSON.stringify(command));
    //}
    //return result;
}

window.onbeforeunload = function (e) {
    e = e || window.event;
    // 兼容IE8和Firefox 4之前的版本
    if (e) {
        e.returnValue = '关闭提示';
    }
    // Chrome, Safari, Firefox 4+, Opera 12+ , IE 9+
    return '关闭提示......';
};

$(function () {
    var popup = null;
    var popupOptions = {
        width: 600,
        height: 250,
        contentTemplate: function () {
            return $("<div />").append(
                $("<div id='popdiv' style='font-size:24px;line-height:30px;width:540px;margin-left:30px;'></div>"),
                $("<div id='beginDo' style='float:right;margin-right:5px;margin-top:50px;'></div>")
            );
        },
        showTitle: true,
        title: "请扫描工作台编号",
        visible: false,
        dragEnabled: false,
        closeOnOutsideClick: false,
        showCloseButton: false,
        onShown: function () {
            $(txtpf).focus();
        }
    };
    if (popup) {
        $(".popup").remove();
    }
    var $popupContainer = $("<div />")
        .addClass("popup")
        .appendTo($("#popup"));
    popup = $popupContainer.dxPopup(popupOptions).dxPopup("instance");
    popup.show();

    var txtpf = $("#popdiv").dxTextBox({
        stylingMode: "outlined",
        height: "60px",
        elementAttr: {
            id: "txtpf_id"
        },
        onKeyDown: function (e) {
            var keynum = (e.event.keyCode ? e.event.keyCode : e.event.which);
            if (keynum == '13' || keynum == '9') {
                $("#beginDo").click();
            }
        },
        onContentReady: function (e) {
            $("#txtpf_id>div").attr("style", "height:60px;");
            $("#txtpf_id>div>input").attr("style", "font-size:36px;");
        }
    }).dxTextBox("instance");

    $("#beginDo").dxButton({
        text: "开始作业[Enter]",
        width: 150,
        onClick: function () {
            scanworkplatform();
        }
    });

    //$("#popdiv").keydown(function (event) {
    //    var keynum = (event.keyCode ? event.keyCode : event.which);
    //    if (keynum == '13') {
    //        scanworkplatform();
    //    }
    //});

    var wbId = null;
    function scanworkplatform() {
        wbId = txtpf.option("text");
        if (wbId.length < 1) {
            DevExpress.ui.notify({ message: "请扫描工作台编号", width: 300, shading: true }, "error", 500);
            $(txtpf).focus();
            playNotExist();
            return;
        }
        $.getJSON("/Common/GetWorkBench", { wbId: wbId }, function (data) {
            if (data != null && data.length > 0) {
                $("#checkplatform").html("工作台:" + data);
                popup.hide();
                $("#txtScan").focus();
                playOk();

                var command = {
                    WorkCenter: "",
                    HZID: "",
                    WMSBillId: "",
                    MenuId: "M41",
                    CartonNumber: "001",
                    KdBillId: "",
                    CTN_LINE_SEQ: 0,
                    CommandType: 00
                };
                SendCommand("open", JSON.stringify(command));
            } else {
                DevExpress.ui.notify({ message: "工作台不存在或未启用，请重新输入", width: 300, shading: true }, "error", 500);
                $(txtpf).focus();
                playNotExist();
            }
        });
    }

    var wwdith = $(window).width();
    var wheight = $(window).height();
    $(".demo-container").width(wwdith);
    $(".circle").attr("style", "display:inline-block; margin-left:" + (wwdith - 100) + "px;");
    var rowDivs = $("div[class='row']");
    if (rowDivs != null && rowDivs.length > 0) {
        var divHeight = (wheight - $("#txtScan").height() - $("#sumInfo").height() - 100) / 4;
        for (var i = 0; i < rowDivs.length; i++) {
            $(rowDivs[i]).attr("style", "height:" + (divHeight - 36) + "px");
        }
    }


    $("#batch_check").dxSwitch({
        value: false
    });

    $("#scan_container").dxSwitch({
        value: false
    });

    var dataGrid = $("#gridContainer").dxDataGrid({
        keyExpr: "SKU",
        paging: {
            enabled: false
        },
        height: "auto",
        allowColumnReordering: true,
        allowColumnResizing: true,
        showBorders: true,
        showColumnLines: true,
        showRowLines: true,
        //rowAlternationEnabled: true,
        columnsAutoWidth: true,
        scrolling: { mode: "standard", showScrollbar: "always", useNative: true },
        columns: [{
            dataField: "ID",
            caption: "序号",
            width: 120,
            height: 20
        }, {
            dataField: "SKU",
            caption: "商品条码",
            width: 250,
            height: 20
        },
        {
            dataField: "SKU_DESC",
            caption: "商品名称",
            width: 350,
            height: 20
        },
        {
            dataField: "ALLOCATED_QTY",
            caption: "商品数",
            width: 120,
            height: 20
        },
        {
            dataField: "PACKED_QTY",
            caption: "已检数",
            width: 120,
            height: 20
        }
        ],
        summary: {
            totalItems: [{
                column: "ALLOCATED_QTY",
                summaryType: "sum",
                displayFormat: "{0}"
            }, {
                column: "PACKED_QTY",
                summaryType: "sum",
                displayFormat: "{0}"
            }]
        },
        onRowPrepared: function (e) {
            if (e.rowType == "data" && scanType == "sku") {
                if (curSku != null && curSku.length > 0) {
                    if ($($(e.rowElement).children("td")[1]).html().toLowerCase() == curSku.toLowerCase()) {
                        $(e.rowElement).attr("style", "color:red;");
                    } else {
                        $(e.rowElement).attr("style", "");
                    }
                } else {
                    $(e.rowElement).attr("style", "");
                }
            }
        }
    }).dxDataGrid("instance");

    refreshLayout();

    function refreshScanSkuColor() {
        var trlist = $("tbody tr[class='dx-row dx-data-row dx-row-lines dx-column-lines']");
        if (trlist != null && trlist.length > 0) {
            for (var i = 0; i < trlist.length; i++) {
                $(trlist[i]).attr("style", "");
            }
        }
    }

    function refreshLayout() {
        var gridHeight = parseInt($("#switchbutton").position().top) -
            parseInt($("#txtScan").css("height")) -
            parseInt($("#scanInfo").css("height")) +
            parseInt($("#gridContainer").css("height"));
        $("#gridContainer").css("height", gridHeight);
        //$(".dx-datagrid-nodata").html("");

        $("#showInfo").attr("style", "margin-top:-" + (gridHeight + $("#sumInfo").height()) + "px;margin-left:" + ($("#gridContainer").width() + 20) + "px; width:25%;display:inline-grid;vertical-align:top;");
    }

    $("#viewdiff").dxButton({
        text: "查看异常信息",
        width: 150,
        onClick: function () {
            DevExpress.ui.notify("查看异常信息");
        }
    });
    $("#orderfree").dxButton({
        text: "订单挂起[F9]",
        width: 150,
        onClick: function () {
            DevExpress.ui.notify("订单挂起[F9]");
        }
    });
    $("#packagecomplete").dxButton({
        text: "完成组包[F6]",
        width: 150,
        onClick: function () {
            DevExpress.ui.notify("完成组包[F6]");
        }
    });
    $("#clearbox").html("清除当前箱[ESC]");
    $("#checkplatform").html("工作台:");

    var scanType = "trackingNo";
    $("#txtScan").keydown(function (event) {
        var keynum = (event.keyCode ? event.keyCode : event.which);
        if (keynum == '13') {
            var scanCode = $("#txtScan").val().trim();
            switch (scanType) {
                case "trackingNo":
                    scanTrackingNo(scanCode);
                    break;
                case "sku":
                    scanSku(scanCode);
                    break;
            }
        }
    });

    var skuData = null;
    var pickData = null;
    var CARRIER = null;
    function scanTrackingNo(scanCode) {
        if (scanCode.length < 1) {
            showErrorMessage("请扫描运单号");
            return;
        }
        $.post("/outscan/scan", { type: scanType, code: scanCode }, function (result) {
            if (result.code != 0) {
                showErrorMessage(result.message);
                return;
            }
            skuData = result.data.SKU;
            pickData = result.data.PICKData;
            CARRIER = result.data.CARRIER;

            $("#cellNo").html(result.data.OrderSource.length < 1 ? result.data.CellNo : result.data.CellNo + "/" + result.data.OrderSource);
            $("#other").html(result.data.Other);
            $("#material").html("");
            $("#storerId").html(result.data.StorerId);
            refreshGridSource();
            refreshLayout();
            showRightMessage("请扫描条码");
            scanType = "sku";
            curSku = "";
            playOk();

            //开启天眼
            if (pickData != null && pickData.length > 0) {
                var command = {
                    WorkCenter: pickData[0].WORK_CENTER,
                    HZID: pickData[0].STORER_ID,
                    WMSBillId: pickData[0].REL_ID,
                    MenuId: "M41",
                    CartonNumber: "001",
                    KdBillId: result.data.KdBillId,
                    CTN_LINE_SEQ: 0,
                    CommandType: 0
                };
                SendCommand("start", JSON.stringify(command));
            }
        });
    }

    var curSku = "";
    function scanSku(scanCode) {
        if (scanCode.length < 1) {
            showErrorMessage("请扫描商品条码");
            return;
        }
        var sku = null;
        var multiple = 1;
        if (skuData != null && skuData.length > 0) {
            for (var i = 0; i < skuData.length; i++) {
                if (skuData[i].UPC_CODE.toLowerCase() == scanCode.toLowerCase()) {
                    sku = skuData[i].SKU;
                    if (skuData[i].PACKAGE_CODE.toLowerCase() == "ctn") {
                        multiple = skuData[i].PACKAGE_QTY;
                        if (isNaN(multiple)) {
                            multiple = 0;
                        }
                    }
                    break;
                }
            }
        }
        if (sku == null || sku.length < 1) {
            showErrorMessage("商品条码" + scanCode + "不存在！");
            return;
        }
        if (multiple <= 0) {
            showErrorMessage("包装数量不能小于0");
            return;
        }
        var exitsSku = false;
        for (var i = 0; i < pickData.length; i++) {
            if (pickData[i].SKU.toLowerCase() == sku.toLowerCase()) {
                exitsSku = true;
                break;
            }
        }
        if (!exitsSku) {
            showErrorMessage("商品条码" + scanCode + ",SKU：" + sku + "不在拣货明细中!");
            return;
        }
        var batchcheck = $("#batch_check").dxSwitch("instance").option("value");
        var skuCount = 0;
        if (batchcheck == "true") {

        } else {
            skuCount = 1 * multiple;
        }
        var allocatedQty = 0, packedQty = 0;
        for (var i = 0; i < pickData.length; i++) {
            if (pickData[i].SKU.toLowerCase() == sku.toLowerCase()) {
                allocatedQty += parseInt(pickData[i].ALLOCATED_QTY);
                packedQty += parseInt(pickData[i].PACKED_QTY);
            }
        }
        if (allocatedQty - packedQty < skuCount) {
            showErrorMessage("SKU数量已溢出");
            return;
        }
        curSku = sku;
        var scanCount = skuCount;
        for (var i = 0; i < pickData.length; i++) {
            if (pickData[i].SKU.toLowerCase() == sku.toLowerCase() &&
                parseInt(pickData[i].ALLOCATED_QTY) > parseInt(pickData[i].PACKED_QTY)) {
                if (parseInt(pickData[i].PACKED_QTY) + scanCount > parseInt(pickData[i].ALLOCATED_QTY)) {
                    pickData[i].PACKED_QTY = parseInt(pickData[i].ALLOCATED_QTY);
                    scanCount -= parseInt(pickData[i].ALLOCATED_QTY) - parseInt(pickData[i].PACKED_QTY);
                } else {
                    pickData[i].PACKED_QTY += scanCount;
                    scanCount = 0;
                    break;
                }
            }
        }
        var isFinish = true;
        for (var i = 0; i < pickData.length; i++) {
            if (parseInt(pickData[i].ALLOCATED_QTY) > parseInt(pickData[i].PACKED_QTY)) {
                isFinish = false;
                break;
            }
        }
        if (isFinish) {
            $.post("/outscan/autopack", { pickData: pickData, wbId: wbId, CARRIER: CARRIER }, function (result) {
                if (result.code != 0) {
                    showErrorMessage(result.message);
                    return;
                }
                $("#cellNo").html("");
                $("#other").html("");
                $("#material").html("");
                $("#storerId").html("");
                $("#sumSKU").html("");
                $("#haveReview").html("");
                $("#haveNotReview").html("");
                $("#gridContainer").dxDataGrid("instance").option("dataSource", null);
                skuData = null;
                pickData = null;
                CARRIER = null;

                scanType = "trackingNo";
                showRightMessage("请扫描运单号");
                refreshLayout();
                refreshScanSkuColor("");

                if (pickData != null && pickData.length > 0) {
                    var command = {
                        WorkCenter: pickData[0].WORK_CENTER,
                        HZID: pickData[0].STORER_ID,
                        WMSBillId: pickData[0].REL_ID,
                        MenuId: "M41",
                        CartonNumber: "001",
                        KdBillId: result.data.KdBillId,
                        CTN_LINE_SEQ: 0,
                        CommandType: 1
                    };
                    SendCommand("stop", JSON.stringify(command));
                }
            });
        } else {
            refreshGridSource();
            showRightMessage("提交成功");


            if (pickData != null && pickData.length > 0) {
                var command = {
                    WorkCenter: pickData[0].WORK_CENTER,
                    HZID: pickData[0].STORER_ID,
                    WMSBillId: pickData[0].REL_ID,
                    MenuId: "M41",
                    CartonNumber: "001",
                    KdBillId: result.data.KdBillId,
                    CTN_LINE_SEQ: 0,
                    CommandType: 3,
                    Detail: {
                        Sku: scanCode,
                        Qty: skuCount,
                        TrackingNo: result.data.KdBillId,
                        OrderId: pickData[0].REL_ID,
                        CartonNumber: "001"
                    }
                };
                SendCommand("transdetail", JSON.stringify(command));
            }
        }
    }

    function refreshGridSource() {
        var sumSku = 0, haveReview = 0;
        var groups = GroupBy(pickData, ['SKU']);
        var list = [];
        if (groups != null && groups.length > 0) {
            for (var i = 0; i < groups.length; i++) {
                list[i] = new Object();
                list[i].ID = i + 1;
                list[i].SKU = groups[i].data[0].SKU;
                list[i].SKU_DESC = groups[i].data[0].SKU_DESC;

                var allocatedQty = 0, packedQty = 0;
                for (var j = 0; j < groups[i].data.length; j++) {
                    allocatedQty += parseInt(groups[i].data[j].ALLOCATED_QTY);
                    packedQty += parseInt(groups[i].data[j].PACKED_QTY);
                }
                list[i].ALLOCATED_QTY = allocatedQty;
                list[i].PACKED_QTY = packedQty;
                sumSku += allocatedQty;
                haveReview += packedQty;
            }
        }
        $("#sumSKU").html(sumSku);
        $("#haveReview").html(haveReview);
        $("#haveNotReview").html(sumSku - haveReview);
        $("#gridContainer").dxDataGrid("instance").option("dataSource", list);
    }

    function showErrorMessage(message) {
        $("#txtScan").val("");
        $("#txtScan").attr("placeholder", message);
        $("#txtScan").css("background-color", "red");
        setTimeout(function () {
            $("#txtScan").css("background-color", "#54697a");
            $("#txtScan").focus();
        }, 500);
        playError();
    }

    function showRightMessage(message) {
        $("#txtScan").val("");
        $("#txtScan").attr("placeholder", message);
        $("#txtScan").css("background-color", "darkgreen");
        setTimeout(function () {
            $("#txtScan").css("background-color", "#54697a");
            $("#txtScan").focus();
        }, 500);
        playSuccess();
    }

    $(".dx-form-group-content").keydown(function (event) {
        var keynum = (event.keyCode ? event.keyCode : event.which);
        //alert(keynum);
        if (keynum == '32') {
            $("#txtScan").focus();
        }
    });
});

function SendCommand(type, command) {
    var ws = new WebSocket("ws://localhost:13145/CommnadService?type=" + type);
    ws.onopen = function () {
        ws.send(command);
    };
    ws.onmessage = function (e) {
        //alert(e.data);
    };
    ws.onclose = function () {
        //alert("close");
    };
}