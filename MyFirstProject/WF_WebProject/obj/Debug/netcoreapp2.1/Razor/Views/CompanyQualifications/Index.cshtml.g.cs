#pragma checksum "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\CompanyQualifications\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c05fbb5f5e26f805c882436b6c2c3a0f6a336e32"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CompanyQualifications_Index), @"mvc.1.0.view", @"/Views/CompanyQualifications/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/CompanyQualifications/Index.cshtml", typeof(AspNetCore.Views_CompanyQualifications_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\_ViewImports.cshtml"
using WFWebProject;

#line default
#line hidden
#line 2 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\_ViewImports.cshtml"
using WFWebProject.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c05fbb5f5e26f805c882436b6c2c3a0f6a336e32", @"/Views/CompanyQualifications/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dde7983aead9016f0b7a49cb549d184d43c16a26", @"/Views/_ViewImports.cshtml")]
    public class Views_CompanyQualifications_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 440, true);
            WriteLiteral(@"<style type=""text/css"">
    table.dataTable {
        border-collapse: collapse !important;
    }
    #input {
        position: absolute;
        top: 0;
        left: 0;
        opacity: 0;
        z-index: -10;
    }
    #copybtn {
        float: right;
        cursor: pointer;
        position: absolute;
        right: 0px;
        top: 0px;
        border: 0px;
        background-color: #e4e4e4;
    }
</style>
");
            EndContext();
            BeginContext(480, 407, true);
            WriteLiteral(@"<div class=""page-content-body"" onselectstart=""return false"">
    <section class=""content"">
        <div class=""row"">
            <div class=""col-12"">
                <div class=""card"">
                    <div class=""card-header"">
                        <h3 class=""card-title"">企业资质信息</h3>
                        <div class=""portlet-body"">
                            <div class=""booking-search"">
");
            EndContext();
            BeginContext(1772, 6663, true);
            WriteLiteral(@"                                <div class=""row form-group"">
                                    <div class=""fiter col-md-6 col-xs-8"">
                                        <div class="" input-group input-large date-picker input-daterange"" data-date-format=""yyyy-mm-dd"" style=""margin-top:21px;"">
                                            <div class=""fiter"" data-column=""05"" data-name=""StartDate"">
                                                <input type=""text"" id=""col05_filter"" class=""form-control"" placeholder=""资质生效日期"" autocomplete=""off"">
                                            </div>
                                            <span class=""input-group-addon"">
                                                到
                                            </span>
                                            <div class=""fiter"" data-column=""06"" data-name=""StartDate1"">
                                                <input type=""text"" id=""col06_filter"" class=""form-control"" placeholder=""资质生效日期"" autoco");
            WriteLiteral(@"mplete=""off"">
                                            </div>
                                        </div>
                                    </div>
                                    <div class=""fiter col-md-6 col-xs-8"">
                                        <div class="" input-group input-large date-picker input-daterange"" data-date-format=""yyyy-mm-dd"" style=""margin-top:21px;"">
                                            <div class=""fiter"" data-column=""07"" data-name=""EndDate"">
                                                <input type=""text"" id=""col07_filter"" class=""form-control"" placeholder=""资质到期日期"" autocomplete=""off"">
                                            </div>
                                            <span class=""input-group-addon"">
                                                到
                                            </span>
                                            <div class=""fiter"" data-column=""08"" data-name=""EndDate1"">
                                        ");
            WriteLiteral(@"        <input type=""text"" id=""col08_filter"" class=""form-control"" placeholder=""资质到期日期"" autocomplete=""off"">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class=""row form-group"">
                                    <div class=""fiter col-md-6 col-xs-8"">
                                        <div class="" input-group input-large date-picker input-daterange"" data-date-format=""yyyy-mm-dd"" style=""margin-top:21px;"">
                                            <div class=""fiter"" data-column=""09"" data-name=""SafetyLicenseStartTime"">
                                                <input type=""text"" id=""col09_filter"" class=""form-control"" placeholder=""安许证生效日期"" autocomplete=""off"">
                                            </div>
                                            <span class=""input-group-addon"">
                                    ");
            WriteLiteral(@"            到
                                            </span>
                                            <div class=""fiter"" data-column=""10"" data-name=""SafetyLicenseStartTime1"">
                                                <input type=""text"" id=""col10_filter"" class=""form-control"" placeholder=""安许证生效日期"" autocomplete=""off"">
                                            </div>
                                        </div>
                                    </div>
                                    <div class=""fiter col-md-6 col-xs-8"">
                                        <div class="" input-group input-large date-picker input-daterange"" data-date-format=""yyyy-mm-dd"" style=""margin-top:21px;"">
                                            <div class=""fiter"" data-column=""11"" data-name=""SafetyLicenseEndTime"">
                                                <input type=""text"" id=""col11_filter"" class=""form-control"" placeholder=""安许证到期日期"" autocomplete=""off"">
                                           ");
            WriteLiteral(@" </div>
                                            <span class=""input-group-addon"">
                                                到
                                            </span>
                                            <div class=""fiter"" data-column=""12"" data-name=""SafetyLicenseEndTime1"">
                                                <input type=""text"" id=""col12_filter"" class=""form-control"" placeholder=""安许证到期日期"" autocomplete=""off"">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class=""row margin-top"">
                                    <div class=""fiter col-md-2 col-xs-5"">
                                        <button class=""btn green btn-block  margin-bottom-20"" id=""Search"">查询<i class=""m-icon-swapright m-icon-white""></i></button>
                                    </div>
                                </div>");
            WriteLiteral(@"
                            </div>
                        </div>
                    </div>

                    <div class=""card-body"">
                        <table id=""example1"" class=""table table-bordered table-striped nowrap"">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>电话</th>
                                    <th>企业名称</th>
                                    <th>备注</th>
                                    <th>省份</th>
                                    <th>市区</th>
                                    <th>资质类型</th>
                                    <th>二级资质细分</th>
                                    <th>三级资质细分</th>
                                    <th>资质等级</th>
                                    <th>资质编号</th>
                                    <th>资质生效日期</th>
                                    <th>资质生效日期1</th>
                                    <th>资质到期日期");
            WriteLiteral(@"</th>
                                    <th>资质到期日期1</th>
                                    <th>安全生产许可证编号</th>
                                    <th>安许证生效日期</th>
                                    <th>安许证生效日期1</th>
                                    <th>安许证到期日期</th>
                                    <th>安许证到期日期1</th>
                                    <th>安全生产许可证范围</th>
                                    <th>发证机构</th>
                                </tr>
                            </thead>
");
            EndContext();
            BeginContext(9747, 972, true);
            WriteLiteral(@"                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <div class=""modal fade"" id=""issueTableCode"" tabindex=""-1"" role=""dialog"" aria-labelledby=""myModalLabel"" aria-hidden=""true"">
        <div class=""modal-dialog"">
            <div class=""modal-content"">
                <div class=""modal-header"">
                    <h4 class=""modal-title"" id=""myModalLabel"">详情页</h4>
                    <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                        <span aria-hidden=""true"">&times;</span>
                    </button>

                </div>
                <div class=""modal-body"">
                    <div id=""issueTableCodeContent"" style=""width:100%; height: auto;word-wrap:break-word;word-break:break-all;overflow:hidden;overflow-y:auto""></div>
                </div>
            </div>
        </div>
    </div>
</div>

");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(10736, 4235, true);
                WriteLiteral(@"

    <script>
        function filterColumn(i) {
            $('#example1').DataTable().column(i).search(
                $('#col' + i + '_filter').val(),
                false,
                false
            ).draw();
        }
        var editor, table, isdisposed;
        var curWarehouseId, curType, curStorerId;
        var rowId = '0';
        $(document).ready(function () {
            isdisposed = false;
            App.initResizeHandler();
            Layout.initAjaxContentSuccessCallback();
            App.addResizeHandler(Resize);
            Layout.addAjaxContentSuccessCallback(AjaxDispose);

            //时间插件配置
            if (jQuery().datepicker) {

                //$('#col03_filter').datepicker({
                //    language: 'zh-CN',
                //    autoclose: true,
                //    todayHighlight: true,
                //    format: 'yyyy-mm-dd 00:00:00',
                //    clearBtn: true
                //});
                //$('#col04_filt");
                WriteLiteral(@"er').datepicker({
                //    language: 'zh-CN',
                //    autoclose: true,
                //    todayHighlight: true,
                //    format: 'yyyy-mm-dd 23:59:59',
                //    clearBtn: true
                //});
                //$('#col05_filter').datepicker({
                //    language: 'zh-CN',
                //    autoclose: true,
                //    todayHighlight: true,
                //    format: 'yyyy-mm-dd 00:00:00',
                //    clearBtn: true
                //});
                //$('#col06_filter').datepicker({
                //    language: 'zh-CN',
                //    autoclose: true,
                //    todayHighlight: true,
                //    format: 'yyyy-mm-dd 23:59:59',
                //    clearBtn: true
                //});
                //$('#col07_filter').datepicker({
                //    language: 'zh-CN',
                //    autoclose: true,
                //    todayHighlight: true,");
                WriteLiteral(@"
                //    format: 'yyyy-mm-dd 00:00:00',
                //    clearBtn: true
                //});
                //$('#col08_filter').datepicker({
                //    language: 'zh-CN',
                //    autoclose: true,
                //    todayHighlight: true,
                //    format: 'yyyy-mm-dd 23:59:59',
                //    clearBtn: true
                //});
                $('.date-picker').datepicker({
                    rtl: App.isRTL(),
                    orientation: ""left"",
                    autoclose: true,
                    todayHighlight: true,
                    language: 'zh-CN',
                    format:'yyyy-mm-dd',
                    clearBtn: true,
                });
            }

            //行内编辑
            $('#example1').on('click', 'tbody td.editable', function (e) {
                rowId = table.row($(this).closest('tr')).data().Id;
                console.log(""rowId="" + rowId);
                initeditor();
    ");
                WriteLiteral(@"            //editor.inline(this);
                editor.bubble(this);
            });

            //$('#example1 tfoot th').each(function () {
            //    var title = $(this).text();
            //    $(this).html('<input type=""text"" placeholder=""Search ' + title + '"" />');
            //});
            //查询
            table = $('#example1').DataTable({

                //initComplete: function () {
                //    // Apply the search
                //    this.api().columns().every(function () {
                //        var that = this;

                //        $('input', this.footer()).on('keyup change clear', function () {
                //            if (that.search() !== this.value) {
                //                that
                //                    .search(this.value)
                //                    .draw();
                //            }
                //        });
                //    });
                //},
                ""scrollX"": ");
                WriteLiteral("true,\r\n                lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],\r\n\r\n                ajax: {\r\n                    url: \"");
                EndContext();
                BeginContext(14972, 47, false);
#line 302 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\CompanyQualifications\Index.cshtml"
                     Write(Url.Action("PageData", "CompanyQualifications"));

#line default
#line hidden
                EndContext();
                BeginContext(15019, 8451, true);
                WriteLiteral(@""",
                    type: ""post""
                },
                order: [[0, 'asc']],//一定要添加
                colReorder: {
                    fixedColumnsLeft: 1
                },
                serverSide: true,
                autoWidth: false,
                columns: [
                    { data: ""Id"", title: ""主键"", searchable: true, orderable: true, width: ""50px"",visible:false, responsivePriority: 1 },
                    //{ data: ""Code"", title: ""企业编号"", searchable: true, orderable: false, visible: true, width: ""100px"", responsivePriority: 1 },
                    {
                        data: ""Name"", title: ""企业名称"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1,
                        render: function (data, type, row, meta) {
                            var title = ""<a href='javascript:void(0);' onclick='Detail("" + meta.row + "")' title='"" + row.Name + ""'>"" + row.Name.substring(0, 5) + ""..."" + ""</a><textarea id='input'>复制面板</textarea><button id='copybt");
                WriteLiteral(@"n' onclick='CopyName("" + meta.row + "")'>C</button>"";
                            return title;
                        }
                    },
                    { data: ""Tel"", title: ""电话"", searchable: true, orderable: false, visible: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Remarks"", title: ""备注"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1, className: ""editable"" },
                    { data: ""Province"", title: ""省份"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""City"", title: ""市区"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""QualificationType"", title: ""资质名称"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    {
                        data: ""EndDate"", title: ""资质到期日期"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1, className: ""editable"",
       ");
                WriteLiteral(@"                 render: function (val, type, row) {
                            if (val == null) {
                                return """";
                            }
                            else {
                                val = moment(val).format('YYYY-MM-DD ');
                                return val;
                            }
                        }
                    },
                    {
                        data: ""SafetyLicenseEndTime"", title: ""安许证到期日期"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1, className: ""editable"",
                        render: function (val, type, row) {
                            if (val == null) {
                                return """";
                            }
                            else {
                                val = moment(val).format('YYYY-MM-DD ');
                                return val;
                            }
                        }
                    ");
                WriteLiteral(@"},
                    { data: ""SecondQualificationDetail"", title: ""二级资质细分"", searchable: true, orderable: true, visible: true, width: ""100px"", responsivePriority: 2 },
                    { data: ""ThirdQualificationDetail"", title: ""三级资质细分"", searchable: true, orderable: true, visible: true, width: ""100px"", responsivePriority: 2 },
                    { data: ""QualificationLevel"", title: ""资质等级"", searchable: true, orderable: true, visible: true, width: ""100px"", responsivePriority: 2 },
                    { data: ""QualificationNumber"", title: ""资质编号"", searchable: true, orderable: true, visible: true, width: ""100px"", responsivePriority: 2 },
                    {
                        data: ""StartDate"", title: ""资质生效日期"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 2,
                        render: function (val, type, row) {
                            if (val == null) {
                                return """";
                            }
                            else");
                WriteLiteral(@" {
                                val = moment(val).format('YYYY-MM-DD ');
                                return val;
                            }

                        }
                    },
                    { data: ""StartDate1"", title: """", searchable: true, orderable: false, visible: false, width: ""80px"" },
                    
                    { data: ""EndDate1"", title: """", searchable: true, orderable: false, visible: false, width: ""80px"" },
                    { data: ""SafetyLicenseNo"", title: ""安全生产许可证编号"", searchable: true, orderable: true, visible: true, width: ""100px"", responsivePriority: 2 },
                    {
                        data: ""SafetyLicenseStartTime"", title: ""安许证生效日期"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 2,
                        render: function (val, type, row) {
                            if (val == null) {
                                return """";
                            }
                            else {
");
                WriteLiteral(@"                                val = moment(val).format('YYYY-MM-DD ');
                                return val;
                            }
                        }
                    },
                    { data: ""SafetyLicenseStartTime1"", title: """", searchable: true, orderable: false, visible: false, width: ""80px"" },
                    
                    { data: ""SafetyLicenseEndTime1"", title: """", searchable: true, orderable: false, visible: false, width: ""80px"" },
                    { data: ""ScopeLicense"", title: ""安全生产许可证范围"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 2 },
                    { data: ""IssuingAuthority"", title: ""发证机构"", searchable: true, orderable: true, visible: true, width: ""500px"", responsivePriority: 2 },

                ],
            });
            //条件搜索
            $('#Search').click(function () {
                var items = $(this).parents('.booking-search').find('.fiter');

                $.each(items, function (index, o");
                WriteLiteral(@"bj) {
                    var i = $(obj).attr('data-column');
                    var name = $(obj).attr('data-name') + '';
                    var val = $('#col' + i + '_filter').val();
                    if (val === null)
                        val = '';
                    var index = $('#example1').DataTable().columns().dataSrc().indexOf(name);
                    var selectot = '.search-' + index;
                    if ($(selectot).length === 0) {
                        $('#example1').DataTable().column(index).search(val, false, false);
                    }
                    else {
                        $('#example1').DataTable().column($(selectot)).search(val, false, false);
                    }
                });

                $('#example1').DataTable().columns().search().draw();
            });
        });

        //详情页
        function Detail(Id) {
            console.log(""您点击了"" + Id);
            //显示模态框展示代码
            $(""#issueTableCode"").modal(""show"");
    ");
                WriteLiteral(@"        var rows = table.rows(Id).data();
            $(""#issueTableCodeContent"").html("""");
            var text = ""企业名称 : "" + (rows[0].Name == null ? ""无"" : rows[0].Name) + ""</br>"";
            text += ""企业电话 : "" + (rows[0].Tel == null ? ""无"" : rows[0].Code) + ""</br>"";
            text += ""安全生产许可证编号 : "" + (rows[0].SafetyLicenseNo == null ? ""无"" : rows[0].SafetyLicenseNo) + ""</br>"";
            text += ""发证机构 : "" + (rows[0].IssuingAuthority == null ? ""无"" : rows[0].IssuingAuthority) + ""</br>"";

            $(""#issueTableCodeContent"").html(text);

        }
        //复制公司名称
        function CopyName(Id) {
            var rows = table.rows(Id).data();
            console.log(""您点击了复制..."");
            var input = document.getElementById(""input"");
            input.value = rows[0].Name; // 修改文本框的内容
            input.select(); // 选中文本
            document.execCommand(""copy""); // 执行浏览器复制命令
            //window.clipboardData.setData(""Text"",rows[0].Name);
            console.log(""复制成功..."" + rows[0].Name)");
                WriteLiteral(@";
        }
        //编辑更新
        function initeditor() {
            editor = new $.fn.dataTable.Editor({
                        idSrc: ""Id"",
                        ajax: {
                            edit: {
                                url: '");
                EndContext();
                BeginContext(23471, 43, false);
#line 447 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\CompanyQualifications\Index.cshtml"
                                 Write(Url.Action("Edit", "CompanyQualifications"));

#line default
#line hidden
                EndContext();
                BeginContext(23514, 1280, true);
                WriteLiteral(@"',
                                type: 'POST',
                                dataType: 'json',
                                data: { ""Id"": rowId+'' }
                            }
                        },
                        table: ""#example1"",
                        fields: [
                            { label: ""Remarks:"", name: ""Remarks"" },
                            {
                                label: ""EndDate"", name: ""EndDate"", type: ""datetime"",
                            },
                            {
                                label: ""SafetyLicenseEndTime"", name: ""SafetyLicenseEndTime"", type: ""datetime"",
                            }
                        ]
                    });
        }
        //调整窗口大小
        function Resize() {
            $('#example1').DataTable()
                .columns.adjust()
                .responsive.recalc();
        }
        //页面数据注销
        function AjaxDispose() {
            if (!isdisposed) {
              ");
                WriteLiteral(@"  isdisposed = true;
                try {
                            table.destroy();

                }
                catch
                {
                    console.log(""释放失败"");
                }
            }
        }
    </script>
");
                EndContext();
            }
            );
            BeginContext(24797, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
