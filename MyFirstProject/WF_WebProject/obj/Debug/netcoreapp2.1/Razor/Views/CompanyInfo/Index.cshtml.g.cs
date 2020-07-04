#pragma checksum "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\CompanyInfo\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6d4f5942d13e1dd69be1ff4af7a885f7a2e083bc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CompanyInfo_Index), @"mvc.1.0.view", @"/Views/CompanyInfo/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/CompanyInfo/Index.cshtml", typeof(AspNetCore.Views_CompanyInfo_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6d4f5942d13e1dd69be1ff4af7a885f7a2e083bc", @"/Views/CompanyInfo/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dde7983aead9016f0b7a49cb549d184d43c16a26", @"/Views/_ViewImports.cshtml")]
    public class Views_CompanyInfo_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 6229, true);
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
                        <h3 class=""card-title"">企业基本信息</h3>
                        <div class=""portlet-body"">
                            <div class=""booking-search"">
                                <div class=""row form-group"">
                                    <div class=""fiter col-md-6 col-xs-8"">
                                        <div class="" input-group input-large date-picker input-daterange"" data-date-format=""yyyy-mm-dd"" style=""margin-top:21px;"">
                                            <div class=""fiter"" data-column=""01"" data-name=""CreateDate"">
                                                <input type=""text"" id=""col01_filter"" class=""form-control"" placeholder=""成立开始-日期"" autocomplete=""off");
            WriteLiteral(@""">
                                            </div>
                                            <span class=""input-group-addon"">
                                                到
                                            </span>
                                            <div class=""fiter"" data-column=""02"" data-name=""CreateDate1"">
                                                <input type=""text"" id=""col02_filter"" class=""form-control"" placeholder=""成立结束-日期"" autocomplete=""off"">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class=""row margin-top"">
                                    <div class=""fiter col-md-2 col-xs-5"">
                                        <button class=""btn green btn-block  margin-bottom-20"" id=""Search"">查询<i class=""m-icon-swapright m-icon-white""></i></button>
                                    </div>
  ");
            WriteLiteral(@"                              </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.card-header -->
                    <div class=""card-body"">
                        <table id=""example1"" class=""table table-bordered table-striped"">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>公司名称</th>
                                    <th>行业</th>
                                    <th>法定代表人</th>
                                    <th>成立日期</th>
                                    <th>成立日期1</th>
                                    <th>联系电话</th>
                                    <th>所属省份</th>
                                    <th>所属市区</th>
                                    <th>所属区县</th>
                                    <th>公司类型</th>
                                    <th>办公地址</th>
                          ");
            WriteLiteral(@"          <th>企业公示的联系电话（更多号码）</th>
                                    <th>企业公示的地址</th>
                                    <th>企业公示的网址</th>
                                    <th>企业公示的邮箱</th>
                                    <th>经营范围</th>
                                    <th>注册资本</th>
                                    <th>实缴资本</th>
                                    <th>经营状态</th>
                                    <th>统一社会信用代码</th>
                                    <th>工商注册号</th>
                                    <th>纳税人识别号</th>
                                    <th>组织机构代码</th>
                                    <th>登记机关</th>
                                    <th>营业期限</th>
                                    <th>纳税人资质</th>
                                    <th>人员规模</th>
                                    <th>参保人数</th>
                                    <th>曾用名</th>
                                    <th>英文名称</th>
                                    <th>注册地址</th>
 ");
            WriteLiteral(@"                                   <th>备注</th>
                                </tr>
                            </thead>
                            <tfoot>
                                <tr>
                                    <th>ID</th>
                                    <th>公司名称</th>
                                    <th>行业</th>
                                    <th>法定代表人</th>
                                    <th>成立日期</th>
                                    <th>成立日期1</th>
                                    <th>联系电话</th>
                                    <th>所属省份</th>
                                    <th>所属市区</th>
                                    <th>所属区县</th>
                                    <th>公司类型</th>
                                    <th>办公地址</th>
                                    <th>企业公示的联系电话（更多号码）</th>
                                    <th>企业公示的地址</th>
                                    <th>企业公示的网址</th>
                                    <th>企业公示");
            WriteLiteral(@"的邮箱</th>
                                    <th>经营范围</th>
                                    <th>注册资本</th>
                                    <th>实缴资本</th>
                                    <th>经营状态</th>
                                    <th>统一社会信用代码</th>
                                    <th>工商注册号</th>
                                    <th>纳税人识别号</th>
                                    <th>组织机构代码</th>
                                    <th>登记机关</th>
                                    <th>营业期限</th>
                                    <th>纳税人资质</th>
                                    <th>人员规模</th>
                                    <th>参保人数</th>
                                    <th>曾用名</th>
                                    <th>英文名称</th>
                                    <th>注册地址</th>
                                    <th>备注</th>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
 ");
            WriteLiteral("               </div>\r\n            </div>\r\n        </div>\r\n    </section>\r\n</div>\r\n\r\n");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(6246, 2911, true);
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
                WriteLiteral(@"
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
                $(this).html('<input type=""text"" placeholder=""Sear");
                WriteLiteral(@"ch ' + title + '"" />');
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
                ""scrollX"": true,
                lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
                ajax: {
                    url: """);
                EndContext();
                BeginContext(9158, 37, false);
#line 199 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\CompanyInfo\Index.cshtml"
                     Write(Url.Action("PageData", "CompanyInfo"));

#line default
#line hidden
                EndContext();
                BeginContext(9195, 5638, true);
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
                    { data: ""Name"",  searchable: true, orderable: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""Industry"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""LegalPerson"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""CreateDate"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },

                    { data: ""CreateDate1"", searchable: true, orderable: false, visible: false, width: ""80px"" },
                    { data: ""Tel"",  searchable: true, ");
                WriteLiteral(@"orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Province"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""City"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""District"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Type"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Address"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PublicTel"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PublicAddress"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PublicWebSite"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                   ");
                WriteLiteral(@" { data: ""PublicEmail"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""BusinessScope"",  searchable: true, orderable: true, width: ""500px"", responsivePriority: 1 },
                    { data: ""RegisteredCapital"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PaidCapital"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""BusinessStatus"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""CreditCode"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegistrationNo"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""IdentificationNumber"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""OrganizationCode"",  sear");
                WriteLiteral(@"chable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegistrationAuthority"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""BusinessTerm"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    
                    { data: ""TaxpayerQualification"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PersonnelSize"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""NumberInsured"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""NameUsedBefore"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""EnglishName"",  searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegisterAddress"", searchab");
                WriteLiteral(@"le: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Remarks"", title: ""备注"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1, className: ""editable"" },
                ],
            });

            $('#Search').click(function () {
                var items = $(this).parents('.booking-search').find('.fiter');

                $.each(items, function (index, obj) {
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
   ");
                WriteLiteral(@"                 else {
                        $('#example1').DataTable().column($(selectot)).search(val, false, false);
                    }
                });

                $('#example1').DataTable().columns().search().draw();
            });
        });

        //编辑更新
        function initeditor() {
            editor = new $.fn.dataTable.Editor({
                        idSrc: ""Id"",
                        ajax: {
                            edit: {
                                url: '");
                EndContext();
                BeginContext(14834, 33, false);
#line 275 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\CompanyInfo\Index.cshtml"
                                 Write(Url.Action("Edit", "CompanyInfo"));

#line default
#line hidden
                EndContext();
                BeginContext(14867, 957, true);
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
            BeginContext(15827, 4, true);
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