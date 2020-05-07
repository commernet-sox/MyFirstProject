(function () {
    $.fn.select2.defaults.set("theme", "classic");
    $.fn.select2.defaults.set("placeholder", "请选择");
    $.fn.select2.defaults.set("language", "zh-CN");
    $.fn.select2.defaults.set("tags", true);
    $.fn.select2.defaults.set("allowClear", "true");
    $.fn.select2.defaults.set("minimumInputLength", 0);

    $.extend(true, $.fn.dataTable.Editor.defaults, {
        i18n: {
            create: {
                button: "创建",
                title: "创建对象",
                submit: "创建"
            },
            edit: {
                button: "编辑",
                title: "编辑对象",
                submit: "编辑"
            },
            remove: {
                button: "删除",
                title: "删除对象",
                submit: "删除",
                confirm: {
                    "_": "确认删除 %d 条记录?",
                    "1": "确认删除 1 条记录?"
                }
            },
            error: {
                system: "发生了一个系统错误 (更多信息)"
            },
            multi: {
                title: "多个值",
                info: "选定的项目包含输入不同的值。编辑和设置该输入值相同的所有项目，单击或点击这里，否则他们会保留单个值。",
                restore: "撤销",
                noMulti: "该输入可单独编辑，但不是一个组的一部分。"
            },
            datetime: {
                previous: '前一个',
                next: '下一个',
                months: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                weekdays: ['星期天', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
                amPm: ['上午', '下午'],
                unknown: '`'
            }
        }
    });
    $.extend(true, $.fn.dataTable.defaults, {
        "oLanguage": {
            "sProcessing": '<img src="http://hrms.ruyicang.com/RES/Content/assets/global/img/loading-spinner-grey.gif" align=""><span>&nbsp;&nbsp;' + '数据加载中...' + '</span>',
            "sLengthMenu": "显示 _MENU_ 项结果",
            "sZeroRecords": "没有匹配结果",
            "sInfo": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
            "sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
            "sInfoPostFix": "",
            "sSearch": "搜索:",
            "sUrl": "",
            "sEmptyTable": "表中数据为空",
            "sLoadingRecords": "载入中...",
            "sInfoThousands": ",",
            "oPaginate": {
                "sFirst": "首页",
                "sPrevious": "上页",
                "sNext": "下页",
                "sLast": "末页"
            },
            "oAria": {
                "sSortAscending": ": 以升序排列此列",
                "sSortDescending": ": 以降序排列此列"
            },
            "select": {
                "rows": {
                    "_": "选择 %d 行",
                    "0": "",
                    "1": "选择 1 行"
                },
                "columns": {
                    "_": "选择 %d 列",
                    "0": "",
                    "1": "选择 1 列"
                },
                "cells": {
                    "_": "选择 %d 单元格",
                    "0": "",
                    "1": "选择 1 单元格"
                }
            }
        },
        //fixedHeader: {
        //    header: true,
        //    footer: false
        //},
        lengthMenu: [[10, 25, 50, 100, 500, -1], [10, 25, 50, 100, 500, "All"]],
        select: true,
        autoWidth: false,
        dom: "<'row'<'col-sm-10'B><'col-sm-2 text-right'l>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        colReorder: true,
        searching: true,
        responsive: {
            breakpoints: [
                { name: 'bigdesktop', width: Infinity },
                { name: 'meddesktop', width: 1480 },
                { name: 'smalldesktop', width: 1280 },
                { name: 'medium', width: 1188 },
                { name: 'tabletl', width: 1024 },
                { name: 'btwtabllandp', width: 848 },
                { name: 'tabletp', width: 768 },
                { name: 'mobilel', width: 480 },
                { name: 'mobilep', width: 320 }
            ]
        },
        serverSide: true,
        processing: true,
    });

    $.fn.dataTable.ext.errMode = 'none';

})();