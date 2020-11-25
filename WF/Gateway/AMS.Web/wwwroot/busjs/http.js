$.extend($,
    {
        PostData: function (url, data, successCallback, failureCallback) {
            $.ajax({
                type: "post",
                dataType: "json",
                url: url,
                data: data,
                beforeSend: function (xhr) {
                    //var storage = window.localStorage;
                    //var token = storage.token;
                    //if (token != null && token != undefined) {
                    //    xhr.setRequestHeader('Authorization', 'Bearer ' + token);
                    //}
                },
                success: function (response) {
                    if (response == undefined || response == null) {
                        alert("获取数据超时");
                        window.location.href = "/home/login?type=logout";
                    }
                    if (successCallback != null && successCallback != undefined) {
                        successCallback(response);
                    }
                },
                error: function (xhr) {
                    //访问令牌已过期
                    if (xhr.status == 401) {
                        alert("访问令牌已过期");
                        window.location.href = "/home/login?type=logout";
                        return;
                    }
                    if (failureCallback != null && failureCallback != undefined) {
                        failureCallback(xhr);
                    }
                }
            });
        },
        GetData: function (url, data, successCallback, failureCallback) {
            $.ajax({
                type: "get",
                dataType: "json",
                url: url,
                data: data,
                beforeSend: function (xhr) {
                    //var storage = window.localStorage;
                    //var token = storage.token;
                    //if (token != null && token != undefined) {
                    //    xhr.setRequestHeader('Authorization', 'Bearer ' + token);
                    //}
                },
                success: function (response) {
                    if (response == undefined || response == null) {
                        alert("获取数据超时");
                        window.location.href = "/home/login?type=logout";
                    }
                    if (successCallback != null && successCallback != undefined) {
                        successCallback(response);
                    }
                },
                error: function (xhr) {
                    //访问令牌已过期
                    if (xhr.status == 401) {
                        alert("访问令牌已过期");
                        window.location.href = "/home/login?type=logout";
                        return;
                    }
                    if (failureCallback != null && failureCallback != undefined) {
                        failureCallback(xhr);
                    }
                }
            });
        }
    });

function RefreshToken(action) {
    $.post("/api/token", null, function (result) {
        if (result.code == 0) {
            window.location.href = "/home/login";
        } else {
            var localStorage = window.localStorage;
            localStorage.token = result.data;
            if (action != null && action != undefined) {
                action();
            }
        }
    }, "json");
}
