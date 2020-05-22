#pragma checksum "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\WF_WebProject\Views\CompanyInfo\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "71a920036dd7642d5264710ad013bc1713afd820"
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
#line 1 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\WF_WebProject\Views\_ViewImports.cshtml"
using WFWebProject;

#line default
#line hidden
#line 2 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\WF_WebProject\Views\_ViewImports.cshtml"
using WFWebProject.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"71a920036dd7642d5264710ad013bc1713afd820", @"/Views/CompanyInfo/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dde7983aead9016f0b7a49cb549d184d43c16a26", @"/Views/_ViewImports.cshtml")]
    public class Views_CompanyInfo_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 846, true);
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
                        </table>
                    </div>
                    <!-- /.card-body -->
                </div>
                <!-- /.card -->
            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->
    </section>
</div>

");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(863, 2588, true);
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
        var curWarehouseId, curType, curStorerId; // use a global for the submit and return data rendering in the examples
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
                ""s");
                WriteLiteral(@"crollX"": true,
                lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
                initComplete: function () {
                    //console.log(""111"");
                    table.search("""").draw();
                },
                drawCallback: function () {
                    $('#example1 thead th').each( function () {
                        var title = $(this).text();
                        $(this).html( '<input type=""text"" class=""dataTables_sizing"" style=""width:100%""  placeholder=""Search '+title+'"" />' );
                    } );
                     //Apply the search
                    this.api().columns().every(function () {
                        //console.log(this.columns());
                        var that = this;
 
                        $('input').on('keyup change clear', function () {
                            //console.log(that);
                            //console.log(that.search() + "":"" + this.value);
                            if (this");
                WriteLiteral(@".parentNode.cellIndex == that[0].toString()) {
                                if (that.search() !== this.value) {
                                    that[0] = (this.parentNode.cellIndex + 1).toString();
                                that
                                    .search( this.value )
                                    .draw();
                            }
                            }
                        } );
                    } );
                },
                ajax: {
                    url: """);
                EndContext();
                BeginContext(3452, 37, false);
#line 88 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\WF_WebProject\Views\CompanyInfo\Index.cshtml"
                     Write(Url.Action("PageData", "CompanyInfo"));

#line default
#line hidden
                EndContext();
                BeginContext(3489, 5087, true);
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
            BeginContext(8579, 4, true);
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
