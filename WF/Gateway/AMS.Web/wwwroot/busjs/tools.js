String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, '');
}
String.prototype.ltrim = function () {
    return this.replace(/(^\s*)/g, '');
}
String.prototype.rtrim = function () {
    return this.replace(/(\s*$)/g, '');
}

function GroupBy(datas, keys, callBack) {
    const list = datas || [];
    const groups = [];
    list.forEach(v => {
        const key = {};
        const data = {};
        keys.forEach(k => {
            key[k] = v[k];
        });
        let group = groups.find(v => {
            return v._key === JSON.stringify(key);
        });
        if (!group) {
            group = {
                _key: JSON.stringify(key),
                key: key
            };
            groups.push(group);
        }
        if (callBack) {
            group.data = callBack(group.data, v);
            group.total = group.total || 0;
            group.total++;
        } else {
            group.data = group.data || [];
            group.data.push(v);
        }
    });
    return groups;
}

function getBrowserInfo() {
    var ua = navigator.userAgent.toLocaleLowerCase();
    var browserType = "无法识别的浏览器！";
    if (ua.match(/msie/) != null || ua.match(/trident/) != null) {
        //browserType = "IE";
        browserVersion = ua.match(/msie ([\d.]+)/) != null ? ua.match(/msie ([\d.]+)/)[1] : ua.match(/rv:([\d.]+)/)[1];
        console.log(window);
        if (window.navigator.msManipulationViewsEnabled && indow.navigator.msPointerEnabled) {
            browserType = "IE";
        }
        else {
            browserType = "360兼容";
        }
    } else if (ua.match(/firefox/) != null) {
        browserType = "火狐";
    } else if (ua.match(/ubrowser/) != null) {
        browserType = "UC";
    } else if (ua.match(/opera/) != null) {
        browserType = "欧朋";
    } else if (ua.match(/bidubrowser/) != null) {
        browserType = "百度";
    } else if (ua.match(/metasr/) != null) {
        browserType = "搜狗";
    } else if (ua.match(/tencenttraveler/) != null || ua.match(/qqbrowse/) != null) {
        browserType = "QQ";
    } else if (ua.match(/maxthon/) != null) {
        browserType = "遨游";
    } else if (ua.match(/chrome/) != null) {
        browserType = "chrome";
        var is360 = _mime("type", "application/vnd.chromium.remoting-viewer");
        function _mime(option, value) {
            var mimeTypes = navigator.mimeTypes;
            for (var mt in mimeTypes) {
                if (mimeTypes[mt][option] == value) {
                    return true;
                }
            }
            return false;
        }
        if (is360) {
            browserType = '360急速';
        } else {
            $('html').css("zoom", ".80");
        }
    } else if (ua.match(/safari/) != null) {
        browserType = "Safari";
    }
    return browserType;
}

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}