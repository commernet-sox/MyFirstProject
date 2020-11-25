

$(function () {
    $("#body").attr("class", "gray-bg sidebar-content");

    $.get("/portal/GetOrderCountInfo", null, function (data) {
        data = JSON.parse(data);
        if (data != null && data != undefined) {
            $("#totalCount").html(data.totalCount);
            $("#issueCount").html(data.issueCount);
            if (data.totalCount > 0) {
                $("#order_percent").html(((data.issueCount / data.totalCount) * 100).toFixed(2) + "%");
            } else {
                $("#order_percent").html("0%");
            }
            $("#preHourOrderCount").html(data.preHourOrderCount);
            var date = new Date();
            var mins = date.getHours() * 60 + date.getMinutes();
            $("#perMinIssueCount").html((data.totalCount / mins).toFixed(2));
        }
    }, "json");

    $.get("/portal/GetOrderInfo", null, function (data) {
        data = JSON.parse(data);
        if (data != null && data != undefined) {
            $("#orderInfo").dxChart({
                dataSource: data,
                commonSeriesSettings: {
                    label: {
                        visible: true,
                        position: "inside",
                        backgroundColor: "transparent",
                        font: {
                            color:"#1470CC"
                        }
                    }
                },
                series: {
                    argumentField: "name",
                    valueField: "value",
                    name: "订单",
                    type: "bar",
                    color: '#99ccff'
                },
                size: {
                    height: 167
                }
            });

        }
    }, "json");
});