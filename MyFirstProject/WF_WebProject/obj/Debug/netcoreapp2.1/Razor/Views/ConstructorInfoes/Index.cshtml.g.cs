#pragma checksum "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\ConstructorInfoes\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "25b3891aef5ec1b0e236edf2d26cdd8dc7293a47"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"25b3891aef5ec1b0e236edf2d26cdd8dc7293a47", @"/Views/ConstructorInfoes/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dde7983aead9016f0b7a49cb549d184d43c16a26", @"/Views/_ViewImports.cshtml")]
    public class Views_ConstructorInfoes_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 3990, true);
            WriteLiteral(@"<style>
    table.dataTable {
        border-collapse: collapse !important;
    }
</style>
<div class=""page-content-body"">
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
                                                <input type=""text"" id=""col01_filter"" class=""form-control"" placeholder=""创建开始-日期"" autocomplete=""off"">");
            WriteLiteral(@"
                                            </div>
                                            <span class=""input-group-addon"">
                                                到
                                            </span>
                                            <div class=""fiter"" data-column=""02"" data-name=""DateIssue1"">
                                                <input type=""text"" id=""col02_filter"" class=""form-control"" placeholder=""创建结束-日期"" autocomplete=""off"">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class=""row margin-top"">
                                    <div class=""fiter col-md-2 col-xs-5"">
                                        <button class=""btn green btn-block  margin-bottom-20"" id=""Search"">查询<i class=""m-icon-swapright m-icon-white""></i></button>
                                    </div>
     ");
            WriteLiteral(@"                           </div>
                            </div>
                        </div>
                    </div>
                    <div class=""card-body"">
                        <table id=""example1"" class=""table table-bordered table-striped"">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>省份</th>
                                    <th>企业名称</th>
                                    <th>建造师</th>
                                    <th>注册号</th>
                                    <th>执业印章号</th>
                                    <th>注册证书编号</th>
                                    <th>执业资格证书编号</th>
                                    <th>发证日期</th>
                                    <th>注册专业</th>
                                    <th>注册有效期</th>
                                    <th>备注</th>
                                </tr>
                            </th");
            WriteLiteral(@"ead>
                            <tfoot>
                                <tr>
                                    <th>ID</th>
                                    <th>省份</th>
                                    <th>企业名称</th>
                                    <th>建造师</th>
                                    <th>注册号</th>
                                    <th>执业印章号</th>
                                    <th>注册证书编号</th>
                                    <th>执业资格证书编号</th>
                                    <th>发证日期</th>
                                    <th>注册专业</th>
                                    <th>注册有效期</th>
                                    <th>备注</th>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
</div>

");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(4007, 2951, true);
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
                $('.date-picker').datepicker({
                    rtl: App.isRTL(),
                    orientation: ""left"",
                    autoclose: true,
                    todayHighlight: true,
                    language: 'zh-CN',
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
                $(this).html('<input type=""text"" placeholder=""Search");
                WriteLiteral(@" ' + title + '"" />');
            });
            //查询
            table = $('#example1').DataTable({

                initComplete: function () {
                    // Apply the search
                    this.api().columns().every(function () {
                        var that = this;

                        $('input', this.footer()).on('keyup change clear', function () {
                            if (that.search() !== this.value) {
                                that
                                    .search(this.value)
                                    .draw();
                            }
                        });
                    });
                },
                \
                ""scrollX"": true,
                lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
                \
                ajax: {
                    url: """);
                EndContext();
                BeginContext(6959, 43, false);
#line 159 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\ConstructorInfoes\Index.cshtml"
                     Write(Url.Action("PageData", "ConstructorInfoes"));

#line default
#line hidden
                EndContext();
                BeginContext(7002, 3230, true);
                WriteLiteral(@""",
                    type: ""post""
                },
                order: [[0, 'asc']],//一定要添加
                colReorder: {
                    fixedColumnsLeft: 1
                },
                serverSide:true,
                columns: [
                    { data: ""Id"",  searchable: true, orderable: true, width: ""50px"",visible:false, responsivePriority: 1 },
                    { data: ""Province"",  searchable: true, orderable: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""CompanyName"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Constructor"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegisterNumber"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PracticeSealNo"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Regi");
                WriteLiteral(@"sterCertNo"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""QualificationCertNo"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""DateIssue"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""DateIssue1"", title: """", searchable: true, orderable: false, visible: false, width: ""80px"" },
                    { data: ""RegisterMajor"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""ValidityRegistration"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Remarks"", title: ""备注"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1, className: ""editable"" },
                ],


            });

            //查询事件
            $('#Search').click(function () {
                var items = $(this).parents('.boo");
                WriteLiteral(@"king-search').find('.fiter');
                $.each(items, function (index, obj) {
                    var i = $(obj).attr('data-column');
                    var name = $(obj).attr('data-name') + '';
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
        //编辑更新
        function initeditor() {
            editor = new $.fn.dataTabl");
                WriteLiteral("e.Editor({\r\n                        idSrc: \"Id\",\r\n                        ajax: {\r\n                            edit: {\r\n                                url: \'");
                EndContext();
                BeginContext(10233, 39, false);
#line 214 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\ConstructorInfoes\Index.cshtml"
                                 Write(Url.Action("Edit", "ConstructorInfoes"));

#line default
#line hidden
                EndContext();
                BeginContext(10272, 957, true);
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
            BeginContext(11232, 4, true);
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
