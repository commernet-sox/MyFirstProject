var pageLoadFlag = false;

; (function ($, window, document, undefined) {
    // 定义类
    function paging(element, option) {
        this.element = element;
        this.option = {
            currentPage: 1,
            classStyle: option.classStyle,
            backClass: option.backClass,
            isFirst: ((option.isFirst == undefined) ? true : option.isFirst), // 是否显示首页和尾页
            isPre: ((option.isPre == undefined) ? true : option.isPre), // 是否显示下一页和上一页按钮
            isShow: ((option.isShow == undefined) ? true : option.isShow),//是否显示总页数和总记录数
            showRecordNum: option.showRecordNum,
            totalNum: option.totalNum,
            totalPage: ((option.totalNum % option.showRecordNum == 0) ? option.totalNum / option.showRecordNum : option.totalNum / option.showRecordNum + 1),
        }
        if (!pageLoadFlag) {
            this.option.showNum = option.showNum;
            pageLoadFlag = true;
        } 
        this.init();
    }

    paging.prototype = {
        init: function () {
            this.createPage();
            this.changePage();
        },
        createPage: function () {
            var ele = this;
            var arr = new Array();
            var str = "";
            var isFirst = ele.option.isFirst;
            var isPre = ele.option.isPre;
            var isShow = ele.option.isShow;

            var classStyle = ele.option.classStyle;
            var string1 = "";
            for (var ss in classStyle) {
                string1 += ss + ':' + classStyle[ss] + ';';
            }

            var backClass = ele.option.backClass;
            var string2 = "";
            for (var ss in backClass) {
                string2 += ss + ':' + backClass[ss] + ';';
            }

            var currentPage = parseInt(ele.option.currentPage);
            var totalPage = parseInt(ele.option.totalPage);
            var totalNum = parseInt(ele.option.totalNum);
            if (isNaN(currentPage) || isNaN(totalPage) || isNaN(totalNum)) {
                alert("分页插件不能正常工作，请输入正确的数字");
            } else {
                if (isFirst) {
                    if (currentPage == 1) {
                        str = '<li class="disabled"><span style="' + string1 + '">首页</span></li>';
                        console.debug(str);
                        arr.push(str);
                    } else {
                        str = '<li><a href="javascript:void(0)" style="' + string1 + '">首页</a></li>';
                        console.debug(str);
                        arr.push(str);
                    }
                }
                if (isPre) {
                    if (currentPage == 1) {
                        str = '<li class="disabled"><span style="' + string1 + '">上一页</span></li>';
                        arr.push(str);
                    } else {
                        str = '<li><a href="javascript:void(0)" style="' + string1 + '">上一页</a></li>';
                        arr.push(str);
                    }

                }
                // 只显示5个页号
                if (totalPage <= 5) {
                    for (var i = 0; i < totalPage; i++) {
                        if ((i + 1) == currentPage) {
                            str = '<li class="active"><a  href="javascript:void(0)" style="' + string1 + string2 + '">' + (i + 1) + '</a></li>';
                            arr.push(str);
                        } else {
                            str = '<li><a href="javascript:void(0)" style="' + string1 + '">' + (i + 1) + '</a></li>';
                            arr.push(str);
                        }
                    }
                } else {
                    if (currentPage < 4) {
                        for (var i = 1; i <= 4; i++) {
                            if (i == currentPage) {
                                str = '<li class="active"><a  href="javascript:void(0)" style="' + string1 + string2 + '">' + (i) + '</a></li>';
                            } else {
                                str = '<li><a  href="javascript:void(0)" style="' + string1 + '">' + (i) + '</a></li>';
                            }
                            arr.push(str);
                        }
                        str = '<li><span style="' + string1 + '">...</span></li>';
                        arr.push(str);
                    } else {
                        str = '<li><a  href="javascript:void(0)" style="' + string1 + '">' + (1) + '</a></li>';
                        arr.push(str);
                        str = '<li><span style="' + string1 + '">...</span></li>';
                        arr.push(str);
                        if (totalPage - currentPage < 4) {
                            for (var i = totalPage - 3; i < totalPage; i++) {
                                if (i == currentPage) {
                                    str = '<li class="active"><a  href="javascript:void(0)" style="' + string1 + string2 + '">' + (i) + '</a></li>';
                                } else {
                                    str = '<li><a  href="javascript:void(0)" style="' + string1 + '">' + (i) + '</a></li>';
                                }
                                arr.push(str);
                            }
                        } else {
                            for (var i = currentPage; i < currentPage + 3; i++) {
                                if (i == currentPage) {
                                    str = '<li class="active"><a  href="javascript:void(0)" style="' + string1 + string2 + '">' + (i) + '</a></li>';
                                } else {
                                    str = '<li><a  href="javascript:void(0)" style="' + string1 + '">' + (i) + '</a></li>';
                                }
                                arr.push(str);
                            }

                            str = '<li><span style="' + string1 + '">...</span></li>';
                            arr.push(str);
                        }
                    }
                    if (currentPage == totalPage) {
                        str = '<li class="active"><a  href="javascript:void(0)" style="' + string1 + '">' + (totalPage) + '</a></li>';
                    } else {
                        str = '<li><a  href="javascript:void(0)" style="' + string1 + '">' + (totalPage) + '</a></li>';
                    }
                    arr.push(str);
                }
                if (isPre) {
                    if (currentPage == totalPage) {
                        str = '<li class="disabled"><span style="' + string1 + '">下一页</span></li>';
                        arr.push(str);
                    } else {
                        str = '<li><a href="javascript:void(0)" style="' + string1 + '">下一页</a></li>';
                        arr.push(str);
                    }
                }
                if (isFirst) {
                    if (currentPage == totalPage) {
                        str = '<li class="disabled"><span style="' + string1 + '">尾页</span></li>';
                        arr.push(str);
                    } else {
                        str = '<li><a href="javascript:void(0)" style="' + string1 + '">尾页</a></li>';
                        arr.push(str);
                    }
                }
                if (isShow) {
                    str = '<li><span class="' + classStyle + '" style="' + string1 + '">共' + totalPage + '页</span></li>';
                    arr.push(str);
                    str = '<li><span class="' + classStyle + '" style="' + string1 + '">共' + totalNum + '条记录</span></li>';
                    arr.push(str);
                }
                str = arr.join("");
                ele.element.html(str);
            }
        },
        changePage: function () {
            var ele = this;
            //console.debug(ele);
            ele.element.on('click', 'a', function () {
                var prePage = parseInt(ele.option.currentPage);
                var currentPage = prePage;
                var totalpage = parseInt(ele.option.totalPage);
                var ss = $(this).html();
                if (ss == "上一页" && currentPage != 1) {
                    ele.option.currentPage = ele.option.currentPage - 1;
                } else if (ss == "上一页" && currentPage == 1) {
                    ele.option.currentPage = ele.option.currentPage;
                }
                if (ss == '首页') {
                    ele.option.currentPage = 1;
                }
                if (ss == "尾页") {
                    ele.option.currentPage = totalpage;
                }
                if (ss == "下一页" && currentPage != totalpage) {
                    ele.option.currentPage = ele.option.currentPage + 1;
                } else if (ss == "下一页" && currentPage == totalpage) {
                    ele.option.currentPage = totalpage;
                }
                if (ss != "首页" && ss != "上一页" && ss != "下一页" && ss != "尾页") {
                    ele.option.currentPage = parseInt(ss);
                }
                ele.createPage();
                if (pageLoadFlag) {
                    if (ele.option.showNum) {
                        if (prePage != ele.option.currentPage) {
                            ele.option.showNum(ele.option.currentPage);
                        }
                    }
                }
            });
        }
    };

    $.fn.Paging = function (option) {
        return new paging(this, option);
    }
})(jQuery, window, document);