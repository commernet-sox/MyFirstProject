#pragma checksum "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\ConstructorInfoes\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2bef234449090e259c15908274e1cac9d0a9977a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ConstructorInfoes_Index), @"mvc.1.0.view", @"/Views/ConstructorInfoes/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/ConstructorInfoes/Index.cshtml", typeof(AspNetCore.Views_ConstructorInfoes_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2bef234449090e259c15908274e1cac9d0a9977a", @"/Views/ConstructorInfoes/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dde7983aead9016f0b7a49cb549d184d43c16a26", @"/Views/_ViewImports.cshtml")]
    public class Views_ConstructorInfoes_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 7028, true);
            WriteLiteral(@"<style type=""text/css"">
    table.dataTable {
        border-collapse: collapse !important;
    }
</style>
<div class=""page-content-body"" onselectstart=""return false"">
    <section class=""content"">
        <div class=""row"">
            <div class=""col-12"">
                <div class=""card"">
                    <div class=""card-header"">
                        <h3 class=""card-title"">建造师信息</h3>
                        <div class=""portlet-body"">
                            <div class=""booking-search"">
                                <div class=""row form-group"">
                                    <div class=""fiter col-md-6 col-xs-8"">
                                        <div class="" input-group input-large date-picker input-daterange"" data-date-format=""yyyy-mm-dd"" style=""margin-top:21px;"">
                                            <div class=""fiter"" data-column=""01"" data-name=""DateIssue"">
                                                <input type=""text"" id=""col01_filter"" class=""form-contr");
            WriteLiteral(@"ol"" placeholder=""发证日期开始-日期"" autocomplete=""off"">
                                            </div>
                                            <span class=""input-group-addon"">
                                                到
                                            </span>
                                            <div class=""fiter"" data-column=""02"" data-name=""DateIssue1"">
                                                <input type=""text"" id=""col02_filter"" class=""form-control"" placeholder=""发证日期结束-日期"" autocomplete=""off"">
                                            </div>
                                        </div>
                                    </div>
                                    <div class=""fiter col-md-6 col-xs-8"">
                                        <div class="" input-group input-large date-picker input-daterange"" data-date-format=""yyyy-mm-dd"" style=""margin-top:21px;"">
                                            <div class=""fiter"" data-column=""03"" data-name=""ValidityRegi");
            WriteLiteral(@"stration"">
                                                <input type=""text"" id=""col03_filter"" class=""form-control"" placeholder=""注册有效期开始-日期"" autocomplete=""off"">
                                            </div>
                                            <span class=""input-group-addon"">
                                                到
                                            </span>
                                            <div class=""fiter"" data-column=""04"" data-name=""ValidityRegistration1"">
                                                <input type=""text"" id=""col04_filter"" class=""form-control"" placeholder=""注册有效期结束-日期"" autocomplete=""off"">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class=""row margin-top"">
                                    <div class=""fiter col-md-2 col-xs-5"">
                                     ");
            WriteLiteral(@"   <button class=""btn green btn-block  margin-bottom-20"" id=""Search"">查询<i class=""m-icon-swapright m-icon-white""></i></button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class=""card-body"">
                        <table id=""example1"" class=""table table-bordered table-striped"">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>建造师姓名</th>
                                    <th>跟进记录</th>
                                    <th>考证省份</th>
                                    <th>职称</th>
                                    <th>执业证书</th>
                                    <th>执业资格证书编号</th>
                                    <th>执业印章号</th>
                                    <th>发证日期</th>
                                    <th>发证日期1<");
            WriteLiteral(@"/th>
                                    <th>注册状况</th>
                                    <th>注册专业</th>
                                    <th>注册证书编号</th>
                                    <th>注册号</th>
                                    <th>注册有效期</th>
                                    <th>注册有效期1</th>
                                    <th>性别</th>
                                    <th>所在地区</th>
                                    <th>手机</th>
                                    <th>微信</th>
                                    <th>邮箱</th>
                                    <th>QQ</th>
                                </tr>
                            </thead>
                            <tfoot>
                                <tr>
                                    <th>ID</th>
                                    <th>建造师姓名</th>
                                    <th>跟进记录</th>
                                    <th>考证省份</th>
                                    <th>职称</th>
        ");
            WriteLiteral(@"                            <th>执业证书</th>
                                    <th>执业资格证书编号</th>
                                    <th>执业印章号</th>
                                    <th>发证日期</th>
                                    <th>发证日期1</th>
                                    <th>注册状况</th>
                                    <th>注册专业</th>
                                    <th>注册证书编号</th>
                                    <th>注册号</th>
                                    <th>注册有效期</th>
                                    <th>注册有效期1</th>
                                    <th>性别</th>
                                    <th>所在地区</th>
                                    <th>手机</th>
                                    <th>微信</th>
                                    <th>邮箱</th>
                                    <th>QQ</th>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
  ");
            WriteLiteral(@"          </div>
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
                BeginContext(7045, 3471, true);
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
                $('#col01_filter').datepicker({
                    language: 'zh-CN',
                    autoclose: true,
                    todayHighlight: true,
                    format: 'yyyy-mm-dd 00:00:00',
                    clearBtn: true
                });
                $('#col02_filter').datepicker({
 ");
                WriteLiteral(@"                   language: 'zh-CN',
                    autoclose: true,
                    todayHighlight: true,
                    format: 'yyyy-mm-dd 23:59:59',
                    clearBtn: true
                });
                $('#col03_filter').datepicker({
                    language: 'zh-CN',
                    autoclose: true,
                    todayHighlight: true,
                    format: 'yyyy-mm-dd 00:00:00',
                    clearBtn: true
                });
                $('#col04_filter').datepicker({
                    language: 'zh-CN',
                    autoclose: true,
                    todayHighlight: true,
                    format: 'yyyy-mm-dd 23:59:59',
                    clearBtn: true
                });
                $('.date-picker').datepicker({
                    rtl: App.isRTL(),
                    orientation: ""left"",
                    autoclose: true,
                    todayHighlight: true,
                    languag");
                WriteLiteral(@"e: 'zh-CN',
                    clearBtn: true,
                });
            }

            //行内编辑
            $('#example1').on('click', 'tbody td.editable', function (e) {
                rowId = table.row($(this).closest('tr')).data().Id;
                console.log(""rowId="" + rowId);
                initeditor();
                editor.inline(this);
            });

            $('#example1 tfoot th').each(function () {
                var title = $(this).text();
                $(this).html('<input type=""text"" placeholder=""Search ' + title + '"" />');
            });
            //查询
            table = $('#example1').DataTable({

                initComplete: function () {
                    // Apply the search
                    this.api().columns().every(function () {
                        var that = this;

                        $('input', this.footer()).on('keyup change clear', function () {
                            if (that.search() !== this.value) {
           ");
                WriteLiteral(@"                     that
                                    .search(this.value)
                                    .draw();
                            }
                        });
                    });
                },
                ""scrollX"": true,
                lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
                ajax: {
                    url: """);
                EndContext();
                BeginContext(10517, 43, false);
#line 221 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\ConstructorInfoes\Index.cshtml"
                     Write(Url.Action("PageData", "ConstructorInfoes"));

#line default
#line hidden
                EndContext();
                BeginContext(10560, 6543, true);
                WriteLiteral(@""",
                    type: ""post""
                },
                order: [[0, 'asc']],//一定要添加
                colReorder: {
                    fixedColumnsLeft: 1
                },
                serverSide:true,
                columns: [
                    { data: ""Id"", title: ""ID"", searchable: true, orderable: true, width: ""50px"", visible: false, responsivePriority: 1 },
                    {
                        data: ""Constructor"", title: ""建造师姓名"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1,
                        render: function (data, type, row, meta) {
                            if (row.Constructor != null && row.Constructor != """") {
                                var title = ""<a href='javascript:void(0);' onclick='Detail("" + meta.row + "")' title='"" + row.Constructor + ""'>"" + row.Constructor + ""</a>"";
                            }
                            else {
                                var title = ""暂无"";
                         ");
                WriteLiteral(@"   }
                            return title;
                        }
                    },
                    { data: ""Remarks"", title: ""跟进记录"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1, className: ""editable"" },
                    { data: ""Province"", title: ""考证省份"", searchable: true, orderable: false, visible: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Thetitle"", title: ""职称"", searchable: true, orderable: false, visible: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Practicecertificate"", title: ""执业证书"", searchable: true, orderable: false, visible: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""QualificationCertNo"", title: ""执业资格证书编号"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PracticeSealNo"", title: ""执业印章号"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    {
        ");
                WriteLiteral(@"                data: ""DateIssue"", title: ""发证日期"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1,
                        render: function (val, type, row) {
                            if (val == null) {
                                return """";
                            }
                            else {
                                val = moment(val).format('YYYY-MM-DD ');
                                return val;
                            }
                        }
                    },
                    { data: ""DateIssue1"", title: """", searchable: true, orderable: false, visible: false, width: ""80px"" },
                    { data: ""Registrationstatus"", title: ""注册状况"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegisterMajor"", title: ""注册专业"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegisterCertNo"", title: ""注册证书编号"", searchable: t");
                WriteLiteral(@"rue, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegisterNumber"", title: ""注册号"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    {
                        data: ""ValidityRegistration"", title: ""注册有效期"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1,
                        render: function (val, type, row) {
                            if (val == null) {
                                return """";
                            }
                            else {
                                val = moment(val).format('YYYY-MM-DD ');
                                return val;
                            }
                        }
                    },
                    { data: ""ValidityRegistration1"", title: """", searchable: true, orderable: false, visible: false, width: ""80px"" },
                    { data: ""Sex"", title: ""性别"", searchable: true, orderable: true, width: ""100px"", r");
                WriteLiteral(@"esponsivePriority: 1 },
                    { data: ""Location"", title: ""所在地区"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Mobile"", title: ""手机"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Wechat"", title: ""微信"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Email"", title: ""邮箱"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""QQ"", title: ""QQ"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },

                ],
            });

            //查询事件
            $('#Search').click(function () {
                var items = $(this).parents('.booking-search').find('.fiter');
                $.each(items, function (index, obj) {
                    var i = $(obj).attr('data-column');
                    var name = $(obj).attr('data-name') + '';");
                WriteLiteral(@"
                    var val = $('#col' + i + '_filter').val();
                    if (val === null)
                        val = '';
                    var index = $('#example1').DataTable().columns().dataSrc().indexOf(name)
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
            var rows = table.rows(Id).data();
            $(""#issueTableCodeContent"").html("""");
            var text = ""考证省份 : """);
                WriteLiteral(@" + (rows[0].Province == null ? ""暂无"" : rows[0].Province) + ""</br>"";
  
            

            $(""#issueTableCodeContent"").html(text);

        }
        //编辑更新
        function initeditor() {
            editor = new $.fn.dataTable.Editor({
                        idSrc: ""Id"",
                        ajax: {
                            edit: {
                                url: '");
                EndContext();
                BeginContext(17104, 39, false);
#line 332 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\ConstructorInfoes\Index.cshtml"
                                 Write(Url.Action("Edit", "ConstructorInfoes"));

#line default
#line hidden
                EndContext();
                BeginContext(17143, 957, true);
                WriteLiteral(@"',
                                type: 'POST',
                                dataType: 'json',
                                data: { ""Id"": rowId+'' }
                            }
                        },
                        table: ""#example1"",
                        fields: [
                            { label: ""Remarks:"", name: ""Remarks"" },
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
                isdisposed = true;
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
            BeginContext(18103, 4, true);
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
