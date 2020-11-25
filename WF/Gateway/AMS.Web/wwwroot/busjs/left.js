$(function () {
    $("#fullScreen").click(function () {
        var isFullScreen = top.document.fullscreenElement || top.document.mozFullScreenElement || top.document.webkitFullscreenElement;
        if (isFullScreen) {
            exitFullscreen();
        } else {
            var element = top.document.documentElement || event.target;
            launchFullScreen(element);
        }
    });
    function launchFullScreen(element) {
        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.mozRequestFullScreen) {
            element.mozRequestFullScreen();
        } else if (element.webkitRequestFullscreen) {
            element.webkitRequestFullscreen();
        } else if (element.msRequestFullscreen) {
            element.msRequestFullscreen();
        }
    }

    function exitFullscreen() {
        if (top.document.exitFullscreen) {
            top.document.exitFullscreen();
        } else if (top.document.mozCancelFullScreen) {
            top.document.mozCancelFullScreen();
        } else if (top.document.webkitExitFullscreen) {
            top.document.webkitExitFullscreen();
        }
    }
});

function subSystemClick(sysCode) {
    var storage = window.localStorage;
    if (storage.userId == undefined || storage.userId == null) {
        alert("登录超时，请重新登录！");
        window.parent.location.href = "/home/login?type=logout";
    }
    $.GetData("/home/getauthcode", { userId: storage.userId }, function (response) {
        if (response != null && response.Code != null) {
            var authCode = response.Code;
            var url = "";
            switch (sysCode) {
                case "AMS":
                    url = "/portal/main";
                    break;
                //case "HWMS":
                //    url = "http://wms.ruyicang.com:2019/portal/portal?code=" + authCode;
                //    break;
                case "HRMS":
                    url = "https://hrms.ruyicang.com/hrms/home/authentication?code=" + authCode;
                    break;
                case "WOMS":
                    url = "http://wms.ruyicang.com:6007/WOMS/home/authentication?code=" + authCode;
                    break;
                case "RPTMS":
                    url = "https://opr.ruyicang.com/Home/authentication?code=" + authCode;
                    break;
                case "BIMS":
                    url = "http://wms.ruyicang.com:6007/Web/Home/authentication?code=" + authCode;
                    break;
                case "BSWMS":
                    url = "http://10.27.1.87/wms?code=" + authCode;
                    break;
                case "TMS":
                    url = "http://tms.ruyicang.com?code=" + authCode;
                    break;
            }
            window.parent.frames["main"].location.href = url;
        } else {
            alert("获取授权码失败！");
            window.parent.location.href = "/home/login?type=logout";
        }
    });
}

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return (false);
}

