var makeAsyncDataSource = function (type) {
    return new DevExpress.data.CustomStore({
        loadMode: "raw",
        key: "ID",
        load: function () {
            var data = null;
            switch (type) {
                case "carrier":
                    data = $.getJSON("/Common/GetExpressList");
                    break;
                case "storer":
                    data = $.getJSON("/Common/GetStorerList");
                    break;
                case "company":
                    data = $.getJSON("/Common/GetCompanyList")
                    break;
                case "department":
                    data = $.getJSON("/Common/GetDepartmentList")
                    break;
                case "employee":
                    data = $.getJSON("/Common/GetEmployeeList")
                    break;
                case "system":
                    data = $.getJSON("/Common/GetSystemList")
                    break;
                case "user":
                    data = $.getJSON("/Common/GetUserList")
                    break;
                case "role":
                    data = $.getJSON("/Common/GetRoleList")
                    break;
                case "app":
                    data = $.getJSON("/Common/GetAppList")
                    break;
            }
            return data;
        }
    });
};

var makeAsyncCommonData = function (type) {
    return new DevExpress.data.CustomStore({
        loadMode: "raw",
        key: "ID",
        load: function () {
            var data = null;
            switch (type) {
                case "wavetype":
                    data = $.getJSON("../jsondata/wavetype.json");
                    break;
                case "auditstatus":
                    data = $.getJSON("../jsondata/auditstatus.json");
                    break;
                case "printstatus":
                    data = $.getJSON("../jsondata/printstatus.json");
                    break;
                case "wavestatus":
                    data = $.getJSON("../jsondata/wavestatus.json");
                    break;
                case "editype":
                    data = $.getJSON("/Log/GetEDIType");
                    break;
                case "locationtype":
                    data = $.getJSON("../jsondata/locationtype.json");
                    break;
                case "movestatus":
                    data = $.getJSON("../jsondata/movestatus.json");
                    break;
            }
            return data;
        }
    });
};

var makeCommonData = function (type, key) {
    if (!window.localStorage) {
        alert("浏览器不支持localStorage");
    } else {
        var storage = window.localStorage;
        var storageItem = JSON.parse(storage.getItem(type));
        if (storageItem != null && storageItem.length > 0) {
            for (var i = 0; i < storageItem.length; i++) {
                if (storageItem[i].ID == key) {
                    return storageItem[i].NAME;
                }
            }
        }
    }
    var itemValue = null;
    switch (type) {
        case "wavetype":
            $.ajax({
                type: "get",
                url: "../jsondata/wavetype.json",
                async: false,
                success: function (data) {
                    var storage = window.localStorage;
                    storage.setItem(type, JSON.stringify(data));
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].ID == key) {
                            itemValue = data[i].NAME;
                            break;
                        }
                    }
                }
            });
            break;
        case "auditstatus":
            $.ajax({
                type: "get",
                url: "../jsondata/auditstatus.json",
                async: false,
                success: function (data) {
                    var storage = window.localStorage;
                    storage.setItem(type, JSON.stringify(data));
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].ID == key) {
                            itemValue = data[i].NAME;
                            break;
                        }
                    }
                }
            });
            break;
        case "printstatus":
            $.ajax({
                type: "get",
                url: "../jsondata/printstatus.json",
                async: false,
                success: function (data) {
                    var storage = window.localStorage;
                    storage.setItem(type, JSON.stringify(data));
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].ID == key) {
                            itemValue = data[i].NAME;
                            break;
                        }
                    }
                }
            });
            break;
        case "wavestatus":
            $.ajax({
                type: "get",
                url: "../jsondata/wavestatus.json",
                async: false,
                success: function (data) {
                    var storage = window.localStorage;
                    storage.setItem(type, JSON.stringify(data));
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].ID == key) {
                            itemValue = data[i].NAME;
                            break;
                        }
                    }
                }
            });
            break;
    }
    return itemValue;
};

var loadPanel = $(".loadpanel").dxLoadPanel({
    shadingColor: "rgba(0,0,0,0.4)",
    //position: { of: "#employee" },
    visible: false,
    showIndicator: true,
    showPane: true,
    shading: true,
    closeOnOutsideClick: false
}).dxLoadPanel("instance");