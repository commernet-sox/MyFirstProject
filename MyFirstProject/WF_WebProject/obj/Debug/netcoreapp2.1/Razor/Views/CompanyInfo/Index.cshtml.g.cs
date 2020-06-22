#pragma checksum "E:\GitHub\MyFirstProject\MyFirstProject\MyFirstProject\WF_WebProject\Views\CompanyInfo\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "796b0b2ba1b24c1230820570efc9d4d95eccfdd6"
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
#line 1 "E:\GitHub\MyFirstProject\MyFirstProject\MyFirstProject\WF_WebProject\Views\_ViewImports.cshtml"
using WFWebProject;

#line default
#line hidden
#line 2 "E:\GitHub\MyFirstProject\MyFirstProject\MyFirstProject\WF_WebProject\Views\_ViewImports.cshtml"
using WFWebProject.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"796b0b2ba1b24c1230820570efc9d4d95eccfdd6", @"/Views/CompanyInfo/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dde7983aead9016f0b7a49cb549d184d43c16a26", @"/Views/_ViewImports.cshtml")]
    public class Views_CompanyInfo_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 4237, true);
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
                                    <th>联系电话</th>
                                    <th>所属省份</th>
                        ");
            WriteLiteral(@"            <th>所属市区</th>
                                    <th>所属区县</th>
                                    <th>公司类型</th>
                                    <th>办公地址</th>
                                    <th>企业公示的联系电话（更多号码）</th>
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
                                    <th>人员规模</th>");
            WriteLiteral(@"
                                    <th>参保人数</th>
                                    <th>曾用名</th>
                                    <th>英文名称</th>
                                    <th>注册地址</th>
                                </tr>
                            </thead>
                            <tfoot>
                                <tr>
                                    <th>ID</th>
                                    <th>公司名称</th>
                                    <th>行业</th>
                                    <th>法定代表人</th>
                                    <th>成立日期</th>
                                    <th>联系电话</th>
                                    <th>所属省份</th>
                                    <th>所属市区</th>
                                    <th>所属区县</th>
                                    <th>公司类型</th>
                                    <th>办公地址</th>
                                    <th>企业公示的联系电话（更多号码）</th>
                                    <th>企业公示的地址");
            WriteLiteral(@"</th>
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
                                </tr>
                            </tfoot>
       ");
            WriteLiteral("                 </table>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </section>\r\n</div>\r\n\r\n");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(4254, 3160, true);
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
        $(document).ready(function () {
            isdisposed = false;
            App.initResizeHandler();
            Layout.initAjaxContentSuccessCallback();
            App.addResizeHandler(Resize);
            Layout.addAjaxContentSuccessCallback(AjaxDispose);

            //查询
            table = $('#example1').DataTable({
                //""paging"": true,
                //""lengthChange"": true,
                //""searching"": true,
                //""ordering"": true,
                //""info"": true,
                //""autoWidth"": true,
                //""select"": false,
                initComplete: function () {
                    // Apply the search
     ");
                WriteLiteral(@"               this.api().columns().every(function () {
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
                //initComplete: function () {
                //    //console.log(""111"");
                //    table.search("""").draw();
                //},
                //drawCallback: function () {
                //    $('#example1 thead th').each( function () {
                //        var title = $(this).text();
                //        $(this).html( '<input type=""text"" class=""dataTables_si");
                WriteLiteral(@"zing"" style=""width:100%""  placeholder=""Search '+title+'"" />' );
                //    } );
                //     //Apply the search
                //    this.api().columns().every(function () {
                //        //console.log(this.columns());
                //        var that = this;
 
                //        $('input').on('keyup change clear', function () {
                //            //console.log(that);
                //            //console.log(that.search() + "":"" + this.value);
                //            if (this.parentNode.cellIndex == that[0].toString()) {
                //                if (that.search() !== this.value) {
                //                    that[0] = (this.parentNode.cellIndex + 1).toString();
                //                that
                //                    .search( this.value )
                //                    .draw();
                //            }
                //            }
                //        } );
             ");
                WriteLiteral("   //    } );\r\n                //},\r\n                ajax: {\r\n                    url: \"");
                EndContext();
                BeginContext(7415, 37, false);
#line 167 "E:\GitHub\MyFirstProject\MyFirstProject\MyFirstProject\WF_WebProject\Views\CompanyInfo\Index.cshtml"
                     Write(Url.Action("PageData", "CompanyInfo"));

#line default
#line hidden
                EndContext();
                BeginContext(7452, 5087, true);
                WriteLiteral(@""",
                    type: ""post""
                },
                order: [[0, 'asc']],//一定要添加
                colReorder: {
                    fixedColumnsLeft: 1
                },
                serverSide:true,
                columns: [
                    { data: ""Id"", title: ""主键"", searchable: true, orderable: true, width: ""50px"",visible:false, responsivePriority: 1 },
                    { data: ""Name"", title: ""公司名称"", searchable: true, orderable: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""Industry"", title: ""行业"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""LegalPerson"", title: ""法定代表人"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""CreateDate"", title: ""成立日期"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Tel"", title: ""联系电话"", searchable: true, orderable: true, width: ""100px"", ");
                WriteLiteral(@"responsivePriority: 1 },
                    { data: ""Province"", title: ""所属省份"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""City"", title: ""所属市区"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""District"", title: ""所属区县"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Type"", title: ""公司类型"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Address"", title: ""办公地址"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PublicTel"", title: ""企业公示的联系电话（更多号码）"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PublicAddress"", title: ""企业公示的地址"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PublicWebSite"", title: ""企业公示的网址"", ");
                WriteLiteral(@"searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PublicEmail"", title: ""企业公示的邮箱"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""BusinessScope"", title: ""经营范围"", searchable: true, orderable: true, width: ""500px"", responsivePriority: 1 },
                    { data: ""RegisteredCapital"", title: ""注册资本"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PaidCapital"", title: ""实缴资本"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""BusinessStatus"", title: ""经营状态"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""CreditCode"", title: ""统一社会信用代码"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegistrationNo"", title: ""工商注册号"", searchable: true, orderable: true, width: ""100px"", respons");
                WriteLiteral(@"ivePriority: 1 },
                    { data: ""IdentificationNumber"", title: ""纳税人识别号"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""OrganizationCode"", title: ""组织机构代码"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegistrationAuthority"", title: ""登记机关"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""BusinessTerm"", title: ""营业期限"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""TaxpayerQualification"", title: ""纳税人资质"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PersonnelSize"", title: ""人员规模"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""NumberInsured"", title: ""参保人数"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                ");
                WriteLiteral(@"    { data: ""NameUsedBefore"", title: ""曾用名"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""EnglishName"", title: ""英文名称"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegisterAddress"", title: ""注册地址"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                ],
                

            });
        });
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
            BeginContext(12542, 4, true);
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
