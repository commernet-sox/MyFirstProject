$(function () {
    DevExpress.config({
        editorStylingMode: "outlined"
    });

    $.get("../jsondata/menu.json", null, function (data) {
        if (data != null && data.length > 0) {
            var tvData = new Array();
            for (var i = 0; i < data.length; i++) {
                var firstItems = new Array();
                var first = { id: data[i].MID, text: data[i].MName, items: firstItems };
                if (data[i].SecondLevel != null && data[i].SecondLevel.length > 0) {
                    for (var j = 0; j < data[i].SecondLevel.length; j++) {
                        var secondItems = new Array();
                        var second = { id: data[i].SecondLevel[j].MID, text: data[i].SecondLevel[j].MName, items: secondItems };
                        if (data[i].SecondLevel[j].ThirdLevel != null && data[i].SecondLevel[j].ThirdLevel.length > 0) {
                            for (var z = 0; z < data[i].SecondLevel[j].ThirdLevel.length; z++) {
                                var third = { id: data[i].SecondLevel[j].ThirdLevel[z].MID, text: data[i].SecondLevel[j].ThirdLevel[z].MName };
                                secondItems.push(third);
                            }
                        }
                        firstItems.push(second);
                    }
                }
                tvData.push(first);
            }
            $("#tvMenu").dxTreeView({
                items: tvData,
                width: 300,
                searchEnabled: true,
                onItemClick: function (e) {
                    var item = e.itemData;
                    if (item.items == null || item.items.length < 1) {

                    }
                }
            }).dxTreeView("instance");
        }
    }, "json");

    var tempitems = $("li[class='temp-list-item']");
    $(tempitems).mouseover(function (e) {
        var tempItem = $(this).find("div[class='temp-item-operate-pannel']");
        if ($(tempItem).attr("style") == "display:none;") {
            $(tempItem).attr("style", "");
        } 
    });

    $(tempitems).mouseleave(function (e) {
        var tempItem = $(this).find("div[class='temp-item-operate-pannel']");
        if ($(tempItem).attr("style") == "") {
            $(tempItem).attr("style", "display:none;");
        } 
    });
});