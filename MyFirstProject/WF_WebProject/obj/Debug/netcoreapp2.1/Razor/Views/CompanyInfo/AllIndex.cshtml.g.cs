#pragma checksum "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\CompanyInfo\AllIndex.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ed4b29ee3e2623f121f7cacded6b099920032ca9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CompanyInfo_AllIndex), @"mvc.1.0.view", @"/Views/CompanyInfo/AllIndex.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/CompanyInfo/AllIndex.cshtml", typeof(AspNetCore.Views_CompanyInfo_AllIndex))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ed4b29ee3e2623f121f7cacded6b099920032ca9", @"/Views/CompanyInfo/AllIndex.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dde7983aead9016f0b7a49cb549d184d43c16a26", @"/Views/_ViewImports.cshtml")]
    public class Views_CompanyInfo_AllIndex : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 565, true);
            WriteLiteral(@"<style>
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
                        <h3 class=""card-title"">企业全部信息</h3>
                        <div class=""portlet-body"">
                            <div class=""booking-search"">
                                <div class=""row form-group"">
");
            EndContext();
            BeginContext(1742, 11025, true);
            WriteLiteral(@"                                    <div class=""fiter col-md-6 col-xs-8"">
                                        <div class="" input-group input-large date-picker input-daterange"" data-date-format=""yyyy-mm-dd"" style=""margin-top:21px;"">
                                            <div class=""fiter"" data-column=""03"" data-name=""Time"">
                                                <input type=""text"" id=""col03_filter"" class=""form-control"" placeholder=""时间开始-日期"" autocomplete=""off"">
                                            </div>
                                            <span class=""input-group-addon"">
                                                到
                                            </span>
                                            <div class=""fiter"" data-column=""04"" data-name=""Time1"">
                                                <input type=""text"" id=""col04_filter"" class=""form-control"" placeholder=""时间结束-日期"" autocomplete=""off"">
                                            </div>
   ");
            WriteLiteral(@"                                     </div>
                                    </div>
                                </div>
                                <div class=""row form-group"">
                                    <div class=""fiter col-md-6 col-xs-8"">
                                        <div class="" input-group input-large date-picker input-daterange"" data-date-format=""yyyy-mm-dd"" style=""margin-top:21px;"">
                                            <div class=""fiter"" data-column=""05"" data-name=""StartDate"">
                                                <input type=""text"" id=""col05_filter"" class=""form-control"" placeholder=""有效期开始-日期"" autocomplete=""off"">
                                            </div>
                                            <span class=""input-group-addon"">
                                                到
                                            </span>
                                            <div class=""fiter"" data-column=""06"" data-name=""StartDate1"">
  ");
            WriteLiteral(@"                                              <input type=""text"" id=""col06_filter"" class=""form-control"" placeholder=""有效期开始-日期"" autocomplete=""off"">
                                            </div>
                                        </div>
                                    </div>
                                    <div class=""fiter col-md-6 col-xs-8"">
                                        <div class="" input-group input-large date-picker input-daterange"" data-date-format=""yyyy-mm-dd"" style=""margin-top:21px;"">
                                            <div class=""fiter"" data-column=""07"" data-name=""EndDate"">
                                                <input type=""text"" id=""col07_filter"" class=""form-control"" placeholder=""有效期结束-日期"" autocomplete=""off"">
                                            </div>
                                            <span class=""input-group-addon"">
                                                到
                                            </span>
        ");
            WriteLiteral(@"                                    <div class=""fiter"" data-column=""08"" data-name=""EndDate1"">
                                                <input type=""text"" id=""col08_filter"" class=""form-control"" placeholder=""有效期结束-日期"" autocomplete=""off"">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class=""row margin-top"">
                                    <div class=""fiter col-md-2 col-xs-5"">
                                        <button class=""btn green btn-block  margin-bottom-20"" id=""Search"">查询<i class=""m-icon-swapright m-icon-white""></i></button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class=""card-body"">
                        <table id=""example1"" class=""table table-borde");
            WriteLiteral(@"red table-striped"">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>公司名称</th>
                                    <th>备注</th>
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
                                    <th>企业公示的地址</th>
                                    <th>企业公示的网址</th>
                                    <th>企业公示的邮箱</th>
                                    <th>经营范围</th>
                                    <th>注册资本");
            WriteLiteral(@"</th>
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


                                    <th>企业编号</th>
                                    <th>经济类型</th>
                                    <th>注册地（省/市）</th>
                                    <th>注册地（市/区）</th>
                                    <th>时间</th>
                             ");
            WriteLiteral(@"       <th>时间1</th>
                                    <th>邮箱</th>
                                    <th>企业网址</th>
                                    <th>资质类型</th>
                                    <th>资质联系地址</th>
                                    <th>邮编</th>
                                    <th>安全生产许可证编号</th>
                                    <th>有效期（起始）</th>
                                    <th>有效期（起始）1</th>
                                    <th>有效期（截止）</th>
                                    <th>有效期（截止）1</th>
                                    <th>发证机构</th>
                                    <th>安全生产许可证范围</th>
                                    <th>组织机构代码</th>
                                    <th>信用综合评分</th>

                                    <th>修改时间</th>
                                    <th>修改人</th>
                                </tr>
                            </thead>
                            <tfoot>
                                <tr>
        ");
            WriteLiteral(@"                            <th>ID</th>
                                    <th>公司名称</th>
                                    <th>备注</th>
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
                                    <th>企业公示的地址</th>
                                    <th>企业公示的网址</th>
                                    <th>企业公示的邮箱</th>
                                    <th>经营范围</th>
                                    <th>注册资本</th>
                                    <th>实缴资本</th>
                                    <th>经营状态</");
            WriteLiteral(@"th>
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


                                    <th>企业编号</th>
                                    <th>经济类型</th>
                                    <th>注册地（省/市）</th>
                                    <th>注册地（市/区）</th>
                                    <th>时间</th>
                                    <th>时间1</th>
                                    <th>邮箱</th>
                                  ");
            WriteLiteral(@"  <th>企业网址</th>
                                    <th>资质类型</th>
                                    <th>资质联系地址</th>
                                    <th>邮编</th>
                                    <th>安全生产许可证编号</th>
                                    <th>有效期（起始）</th>
                                    <th>有效期（起始）1</th>
                                    <th>有效期（截止）</th>
                                    <th>有效期（截止）1</th>
                                    <th>发证机构</th>
                                    <th>安全生产许可证范围</th>
                                    <th>组织机构代码</th>
                                    <th>信用综合评分</th>

                                    <th>修改时间</th>
                                    <th>修改人</th>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <div class=""modal fade"" id=""issueTableCode""");
            WriteLiteral(@" tabindex=""-1"" role=""dialog"" aria-labelledby=""myModalLabel"" aria-hidden=""true"">
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
                BeginContext(12784, 4613, true);
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
                //$('#col01_filter').datepicker({
                //    language: 'zh-CN',
                //    autoclose: true,
                //    todayHighlight: true,
                //    format: 'yyyy-mm-dd 00:00:00',
                //    clearBtn: true
                //});
                //$('#col02_filter");
                WriteLiteral(@"').datepicker({
                //    language: 'zh-CN',
                //    autoclose: true,
                //    todayHighlight: true,
                //    format: 'yyyy-mm-dd 23:59:59',
                //    clearBtn: true
                //});
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
                $('#col05_filter').datepicker({
                    language: 'zh-CN',
                    autoclose: true,
                    todayHighlight: true,
                    format: 'yyyy-mm-");
                WriteLiteral(@"dd 00:00:00',
                    clearBtn: true
                });
                $('#col06_filter').datepicker({
                    language: 'zh-CN',
                    autoclose: true,
                    todayHighlight: true,
                    format: 'yyyy-mm-dd 23:59:59',
                    clearBtn: true
                });
                $('#col07_filter').datepicker({
                    language: 'zh-CN',
                    autoclose: true,
                    todayHighlight: true,
                    format: 'yyyy-mm-dd 00:00:00',
                    clearBtn: true
                });
                $('#col08_filter').datepicker({
                    language: 'zh-CN',
                    autoclose: true,
                    todayHighlight: true,
                    format: 'yyyy-mm-dd 23:59:59',
                    clearBtn: true
                });
                $('.date-picker').datepicker({
                    rtl: App.isRTL(),
                    orientat");
                WriteLiteral(@"ion: ""left"",
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
                $(this).html('<input type=""text"" placeholder=""Search ' + title + '"" />');
            });
            //查询
            table = $('#example1').DataTable({
                initComplete: function () {
                    // Apply the search
                    this.api().columns().every(function () {
                        var that = this;
                        $('input', this.footer()");
                WriteLiteral(@").on('keyup change clear', function () {
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
                BeginContext(17398, 47, false);
#line 348 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\CompanyInfo\AllIndex.cshtml"
                     Write(Url.Action("CompanyAllPageData", "CompanyInfo"));

#line default
#line hidden
                EndContext();
                BeginContext(17445, 13967, true);
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
                    {
                        data: ""Name"", title: ""公司名称"", searchable: true, orderable: false, width: ""100px"", responsivePriority: 1,
                        render: function (data, type, row, meta) {
                            var title = ""<a href='javascript:void(0);' onclick='Detail("" + meta.row + "")' title='"" + row.Name + ""'>"" + row.Name.substring(0, 8) + ""..."" + ""</a>"";
                            return title;
                        }
                    },
                    { data: ""Remarks"", title: ""备注"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1, className: ");
                WriteLiteral(@"""editable"" },
                    { data: ""Industry"", title: ""行业"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""LegalPerson"", title: ""法定代表人"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    {
                        data: ""CreateDate"", title: ""成立日期"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1,
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
                    
                    { data: ""Tel"", title: ""联系电话"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                ");
                WriteLiteral(@"    { data: ""Province"", title: ""所属省份"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""City"", title: ""所属市区"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""District"", title: ""所属区县"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Type"", title: ""公司类型"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Address"", title: ""办公地址"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""PublicTel"", title: ""企业公示的联系电话（更多号码）"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PublicAddress"", title: ""企业公示的地址"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""PublicWebSite"", title: ""企业公示的网址"", searchable");
                WriteLiteral(@": true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""PublicEmail"", title: ""企业公示的邮箱"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""BusinessScope"", title: ""经营范围"", searchable: true, orderable: true, visible: false, width: ""800px"", responsivePriority: 1 },
                    { data: ""RegisteredCapital"", title: ""注册资本"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""PaidCapital"", title: ""实缴资本"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""BusinessStatus"", title: ""经营状态"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""CreditCode"", title: ""统一社会信用代码"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegistrationNo"", title: ""工商注册号"", sear");
                WriteLiteral(@"chable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""IdentificationNumber"", title: ""纳税人识别号"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""OrganizationCode"", title: ""组织机构代码"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegistrationAuthority"", title: ""登记机关"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""BusinessTerm"", title: ""营业期限"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""TaxpayerQualification"", title: ""纳税人资质"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""PersonnelSize"", title: ""人员规模"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePri");
                WriteLiteral(@"ority: 1 },
                    { data: ""NumberInsured"", title: ""参保人数"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""NameUsedBefore"", title: ""曾用名"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""EnglishName"", title: ""英文名称"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""RegisterAddress"", title: ""注册地址"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },


                    { data: ""Code"", title: ""企业编号"", searchable: true, orderable: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""EconomicType"", title: ""经济类型"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""Province1"", title: ""注册地（省/市）"", searchable: true, orderable: true, width: ""100px"", res");
                WriteLiteral(@"ponsivePriority: 1 },
                    { data: ""City1"", title: ""注册地（市/区）"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    {
                        data: ""Time"", title: ""时间"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1,
                        render: function (val, type, row) {
                            if (val == null) {
                                return """";
                            }
                            else {
                                val = moment(val).format('YYYY-MM-DD ');
                                return val;
                            }
                        }},
                    { data: ""Time1"", title: """", searchable: true, orderable: false, visible: false, width: ""80px"" },
                    { data: ""Email"", title: ""邮箱"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""WebSite"", title: ""企业网址"", searc");
                WriteLiteral(@"hable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""QualificationType"", title: ""资质类型"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""ContactAddress"", title: ""资质联系地址"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""ZipCode"", title: ""邮编"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""SafetyLicenseNo"", title: ""安全生产许可证编号"", searchable: true, orderable: true, visible: false, width: ""100px"", responsivePriority: 1 },
                    { data: ""StartDate"", title: ""有效期（起始）"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 ,
                        render: function (val, type, row) {
                            val =val==""""||val==null?"""": moment(val).format('YYYY-MM-DD ');
                            return val;
     ");
                WriteLiteral(@"                   }
                    },
                    { data: ""StartDate1"", title: """", searchable: true, orderable: false, visible: false, width: ""80px"" },
                    { data: ""EndDate"", title: ""有效期（截止）"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 ,
                        render: function (val, type, row) {
                            val =val==""""||val==null?"""": moment(val).format('YYYY-MM-DD ');
                            return val;
                        }
                    },
                    { data: ""EndDate1"", title: """", searchable: true, orderable: false, visible: false, width: ""80px"" },
                    { data: ""IssuingAuthority"", title: ""发证机构"", searchable: true, orderable: true, visible: false, width: ""500px"", responsivePriority: 1 },
                    { data: ""ScopeLicense"", title: ""安全生产许可证范围"", searchable: true, orderable: true,  width: ""100px"", responsivePriority: 1 },
                    { data: ""OrganizationCode1"", title: ""组");
                WriteLiteral(@"织机构代码"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""ComprehensiveScore"", title: ""信用综合评分"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    
                    { data: ""ModifyTime"", title: ""修改时间"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1 },
                    { data: ""Modifier"", title: ""修改人"", searchable: true, orderable: true, width: ""100px"", responsivePriority: 1},
                ],
            });

            //条件搜索
            $('#Search').click(function () {
                var items = $(this).parents('.booking-search').find('.fiter');

                $.each(items, function (index, obj) {
                    var i = $(obj).attr('data-column');
                    var name = $(obj).attr('data-name') + '';
                    var val = $('#col' + i + '_filter').val();
                    if (val === null)
                        val = '';
         ");
                WriteLiteral(@"           var index = $('#example1').DataTable().columns().dataSrc().indexOf(name);
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
            var text = ""办公地址 : "" + (rows[0].Address == null ? ""无"" : rows[0].Address) + ""</br>"";
            text += ""企业公示的地址 : "" + (rows[0].PublicAddress == null ? ""无"" : rows[0].P");
                WriteLiteral(@"ublicAddress) + ""</br>"";
            text += ""企业公示的网址 : "" + (rows[0].PublicWebSite == null ? ""无"" : rows[0].PublicWebSite) + ""</br>"";
            text += ""企业公示的邮箱 : "" + (rows[0].PublicEmail == null ? ""无"" : rows[0].PublicEmail) + ""</br>"";
            text += ""经营范围 : "" + (rows[0].BusinessScope == null ? ""无"" : rows[0].BusinessScope) + ""</br>"";
            text += ""实缴资本 : "" + (rows[0].PaidCapital == null ? ""无"" : rows[0].PaidCapital) + ""</br>"";
            text += ""工商注册号 : "" + (rows[0].RegistrationNo == null ? ""无"" : rows[0].RegistrationNo) + ""</br>"";
            text += ""纳税人识别号 : "" + (rows[0].IdentificationNumber == null ? ""无"" : rows[0].IdentificationNumber) + ""</br>"";
            text += ""组织机构代码 : "" + (rows[0].OrganizationCode == null ? ""无"" : rows[0].OrganizationCode) + ""</br>"";
            text += ""登记机关 : "" + (rows[0].RegistrationAuthority == null ? ""无"" : rows[0].RegistrationAuthority) + ""</br>"";
            text += ""营业期限 : "" + (rows[0].BusinessTerm == null ? ""无"" : rows[0].BusinessTerm) + ""</br>"";
     ");
                WriteLiteral(@"       text += ""纳税人资质 : "" + (rows[0].TaxpayerQualification == null ? ""无"" : rows[0].TaxpayerQualification) + ""</br>"";
            text += ""人员规模 : "" + (rows[0].PersonnelSize == null ? ""无"" : rows[0].PersonnelSize) + ""</br>"";
            text += ""参保人数 : "" + (rows[0].NumberInsured == null ? ""无"" : rows[0].NumberInsured) + ""</br>"";
            text += ""曾用名 : "" + (rows[0].NameUsedBefore == null ? ""无"" : rows[0].NameUsedBefore) + ""</br>"";
            text += ""英文名称 : "" + (rows[0].EnglishName == null ? ""无"" : rows[0].EnglishName) + ""</br>"";
            text += ""注册地址 : "" + (rows[0].RegisterAddress == null ? ""无"" : rows[0].RegisterAddress) + ""</br>"";

            text += ""经济类型 : "" + (rows[0].EconomicType == null ? ""无"" : rows[0].EconomicType) + ""</br>"";
            text += ""邮箱 : "" + (rows[0].Email == null ? ""无"" : rows[0].Email) + ""</br>"";
            text += ""企业网址 : "" + (rows[0].WebSite == null ? ""无"" : rows[0].WebSite) + ""</br>"";
            text += ""资质联系地址 : "" + (rows[0].ContactAddress == null ? ""无"" : rows[0].Conta");
                WriteLiteral(@"ctAddress) + ""</br>"";
            text += ""邮编 : "" + (rows[0].ZipCode == null ? ""无"" : rows[0].ZipCode) + ""</br>"";
            text += ""安全生产许可证编号 : "" + (rows[0].SafetyLicenseNo == null ? ""无"" : rows[0].SafetyLicenseNo) + ""</br>"";
            text += ""发证机构 : "" + (rows[0].IssuingAuthority == null ? ""无"" : rows[0].IssuingAuthority) + ""</br>"";

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
                BeginContext(31413, 33, false);
#line 521 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\CompanyInfo\AllIndex.cshtml"
                                 Write(Url.Action("Edit", "CompanyInfo"));

#line default
#line hidden
                EndContext();
                BeginContext(31446, 957, true);
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
            BeginContext(32406, 4, true);
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
