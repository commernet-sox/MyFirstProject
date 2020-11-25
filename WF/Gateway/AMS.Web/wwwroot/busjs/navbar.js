$("#body").attr("style", "padding-top: 50px;");
$("#btnLogout").click(function () {
    var storage = window.localStorage;
    storage.clear();
    window.location.href = "/home/login?type=logout";
});

var pressDivUser = false;
$("#divUser").click(function () {
    if ($("#userInfo").attr("style") == "display:none;") {
        $("#userInfo").attr("style", "");
    } else {
        $("#userInfo").attr("style", "display:none;");
    }
    pressDivUser = true;
});

$.get("../jsondata/menu.json", null, function (data) {
    if (data != null && data.length > 0) {
        var info = "<ul class='nav navbar-nav'><li class='active'><a href='/portal/main' target='main'>首页</a></li>";
        for (var i = 0; i < data.length; i++) {
            info += "<li>";
            info += "<a href='#'>" + data[i].MName + "</a>";
            if (data[i].SecondLevel != null && data[i].SecondLevel.length > 0) {
                info += "<div class='next-overlay-inner cn-topnav-overlay' style='position: absolute; left: 0px; top: 48px; right: auto;'>";
                info += "<div class='cn-topnav-scroll-cont'>";
                var calCount = 0;
                var hasEmpty = false;
                for (var j = 0; j < data[i].SecondLevel.length; j++) {
                    if (data[i].SecondLevel[j].ThirdLevel != null && data[i].SecondLevel[j].ThirdLevel.length > 0) {
                        calCount++;
                    } else {
                        hasEmpty = true;
                    }
                }
                if (hasEmpty) {
                    calCount++;
                }
                info += "<div class='cn-topnav-flow-nav' style='width: " + (calCount * 12) + "vw; min-width: " + (calCount * 12) + "vw;'>";
                info += "<div class='cn-topnav-flow-channels-scroll-cont' style='flex-grow: 2;'>";
                info += "<div class='cn-topnav-flow-channels-cont'>";
                info += "<div class='cn-topnav-flow-channels'>";
                var noraml = "", other = "";
                for (var j = 0; j < data[i].SecondLevel.length; j++) {
                    var secodLevel = data[i].SecondLevel[j];
                    if (secodLevel.ThirdLevel != null && secodLevel.ThirdLevel.length > 0) {
                        noraml += "<div class='cn-topnav-flow-channel'>";
                        noraml += "<div class='cn-topnav-flow-submenu'>";
                        noraml += "<div class='cn-topnav-flow-submenu-header'>";
                        noraml += "<div class='cn-topnav-flow-submenu-title'>" + secodLevel.MName + "</div>";
                        noraml += "</div>";
                        if (secodLevel.ThirdLevel != null && secodLevel.ThirdLevel.length > 0) {
                            noraml += "<ul class='cn-topnav-flow-submenu-menu'>";
                            for (var z = 0; z < secodLevel.ThirdLevel.length; z++) {
                                var thirdLevel = secodLevel.ThirdLevel[z];
                                noraml += "<li class='cn-topnav-flow-submenu-menu-item'>";
                                noraml += "<div class='cn-topnav-flow-submenu-menu-title'>";
                                noraml += "<a href='/" + thirdLevel.ControlName + "/" + thirdLevel.ActionName + "'>" + thirdLevel.MName + "</a>";
                                noraml += "</div>";
                                noraml += "<div class='cn-topnav-flow-submenu-menu-extra'>";
                                noraml += "<a href='/" + thirdLevel.ControlName + "/" + thirdLevel.ActionName + "' target='_blank' class='next-btn next-medium next-btn-normal next-btn-text extra-blank'><i class=''></i></a>";
                                noraml += "</div>";
                                noraml += "</li>";
                            }
                            noraml += "</ul>";
                        }
                        noraml += "</div>";
                        noraml += "</div>";
                        if (j < data[i].SecondLevel.length - 1) {
                            noraml += "<div class='cn-topnav-flow-channel-border'></div>";
                        }
                    } else {
                        other += "<li class='cn-topnav-flow-submenu-menu-item'>";
                        other += "<div class='cn-topnav-flow-submenu-menu-title'>";
                        other += "<a href='#'>" + secodLevel.MName + "</a>";
                        other += "</div>";
                        other += "<div class='cn-topnav-flow-submenu-menu-extra'>";
                        other += "<a href='#' target='_blank' class='next-btn next-medium next-btn-normal next-btn-text extra-blank'><i class=''></i></a>";
                        other += "</div>";
                        other += "</li>";
                    }
                }
                if (hasEmpty) {
                    info += "<div class='cn-topnav-flow-channel'>";
                    info += "<div class='cn-topnav-flow-submenu'>";
                    info += "<div class='cn-topnav-flow-submenu-header'>";
                    info += "<div class='cn-topnav-flow-submenu-title'>" + data[i].MName + "</div>";
                    info += "</div>";
                    info += "<ul class='cn-topnav-flow-submenu-menu'>";
                    info += other;
                    info += "</ul>";
                    info += "</div>";
                    info += "</div>";

                    if (noraml.length > 0) {
                        info += "<div class='cn-topnav-flow-channel-border'></div>";
                        info += noraml;
                    }
                } else {
                    info += noraml;
                }
                info += "</div>";
                info += "</div>";
                info += "</div>";
                info += "</div>";
                info += "</div>";
                info += "</div>";
            }
            info += "</li>";
        }
        info += "</ul>";
        $("#navbar").html(info);

        var liitems = $("ul li");
        var curX = 0, curY = 0;
        var msflag = false;
        hideDivs();
        var activeItem;
        liitems.mouseover(function (e) {
            //其它弹出层取消事件
            if (hasPopupDiv) {
                return;
            }
            for (var i = 0; i < $(liitems).length; i++) {
                var liitem = $(liitems)[i];
                if ($(liitem).html() == $(this).html()) {
                    activeItem = liitem;
                    //console.log("page X,Y:" + e.pageX + "," + e.pageY);
                } else {
                    if ($(liitem).hasClass("active")) {
                        $(liitem).removeClass("active");

                        var divs = $(liitem).find("div");
                        if ($(divs).length < 1) {
                            continue;
                        }
                        var div = divs[0];
                        $(div).hide();
                    }
                }
            }
            if (activeItem != null) {
                if (!$(activeItem).hasClass("active")) {
                    $(activeItem).addClass("active");
                }
                if ($(activeItem).get(0).tagName.toLowerCase() == "li") {
                    var divs = $(activeItem).find("div");
                    if ($(divs).length < 1) {
                        return;
                    }
                    for (var i = 0; i < $(divs).length; i++) {
                        var div = divs[i];
                        $(div).show();
                    }
                    curX = 0;//$(activeItem).position().left;
                    curY = $(activeItem).position().top + $(activeItem).height();
                    //console.log("div X,Y:" + curX + "," + curY);
                    if ($(activeItem).position().left + $(divs[0]).width() > $(document).width()) {
                        $(divs[0]).css({ "left": -($(divs[0]).width() - ($(document).width() - $(activeItem).position().left)) + "px", "top": curY + "px" });
                    } else {
                        $(divs[0]).css({ "left": curX + "px", "top": curY + "px" });
                    }
                    msflag = true;
                }
            }
        });

        function hideDivs() {
            //其它弹出层取消事件
            if (hasPopupDiv) {
                return;
            }

            for (var i = 0; i < $(liitems).length; i++) {
                var liitem = $(liitems)[i];
                var divs = $(liitem).find("div");
                if ($(divs).length < 1) {
                    continue;
                }
                var div = divs[0];
                $(div).hide();

                if ($(liitem).hasClass("active")) {
                    $(liitem).removeClass("active");
                }
            }
        }

        $(body).click(function () {
            hideDivs();
            if (!pressDivUser) {
                if ($("#userInfo").attr("style") == "") {
                    $("#userInfo").attr("style", "display:none;");
                }
            }
            pressDivUser = false;
        });

        $(body).mouseover(function (e) {
            if (activeItem != null) {
                if ($(activeItem).get(0).tagName.toLowerCase() == "li") {
                    var divs = $(activeItem).find("div");
                    if ($(divs).length < 1) {
                        return;
                    }
                    //console.log("pageX:" + e.pageX, "pageY:" + e.pageY);
                    //console.log($(divs[0]).offset().left, $(divs[0]).width());
                    if ($(divs[0]).width() > 0) {
                        if (e.pageX < $(divs[0]).offset().left || e.pageX > $(divs[0]).offset().left + $(divs[0]).width()) {
                            for (var i = 0; i < $(divs).length; i++) {
                                var div = divs[i];
                                $(div).hide();
                            }
                        }
                    }
                }
            }
        });

        var submenus = $("li[class^='cn-topnav-flow-submenu-menu-item']");
        $(submenus).mouseover(function () {
            for (var i = 0; i < $(submenus).length; i++) {
                var submenu = $(submenus)[i];
                if ($(submenu).html() == $(this).html()) {
                    $(submenu).attr("class", "cn-topnav-flow-submenu-menu-item hovered");
                    var liicons = $(submenu).find("i[class='']");
                    if (liicons.length > 0) {
                        $(liicons[0]).attr("class", "dx-icon-menu");
                    }
                } else {
                    $(submenu).attr("class", "cn-topnav-flow-submenu-menu-item");
                    var liicons = $(submenu).find("i");
                    if (liicons.length > 0) {
                        $(liicons[0]).attr("class", "");
                    }
                }
            }
        });

    }
}, "json");

var hasPopupDiv = false;