#pragma checksum "C:\Users\FS\source\repos\WF_WebProject\WF_WebProject\Views\Shared\_LeftMenu.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f6096ed493b5fcf458d2cb152e746eb76f92e638"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__LeftMenu), @"mvc.1.0.view", @"/Views/Shared/_LeftMenu.cshtml")]
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
#nullable restore
#line 1 "C:\Users\FS\source\repos\WF_WebProject\WF_WebProject\Views\_ViewImports.cshtml"
using WFWebProject;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\FS\source\repos\WF_WebProject\WF_WebProject\Views\_ViewImports.cshtml"
using WFWebProject.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f6096ed493b5fcf458d2cb152e746eb76f92e638", @"/Views/Shared/_LeftMenu.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8b095058779a1d989e07011361c5cf65da51af88", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__LeftMenu : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("    <ul class=\"page-sidebar-menu  page-header-fixed \" data-keep-expanded=\"false\" data-auto-scroll=\"true\" data-slide-speed=\"200\" style=\"padding-top: 10px\">\r\n\r\n\r\n        <li class=\"nav-item  start active open\">\r\n            <a class=\"ajaxify\"");
            BeginWriteAttribute("href", " href=\"", 370, "\"", 406, 1);
#nullable restore
#line 10 "C:\Users\FS\source\repos\WF_WebProject\WF_WebProject\Views\Shared\_LeftMenu.cshtml"
WriteAttributeValue("", 377, Url.Action("Index", "Home" ), 377, 29, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
                <i class="" font-blue-soft font-lg glyphicon glyphicon-dashboard""></i>
                <span class=""title"">首页</span>
                <span class=""selected""></span>
            </a>
        </li>

        <li class=""nav-item open"">

            <a href=""javascript:;"" class=""nav-link nav-toggle"">
                <i class=""font-blue-soft font-lg fa fa-tasks""></i>
                <span class=""title"">基础数据</span>
                <span class=""arrow open""></span>
            </a>
            <ul class=""sub-menu"" style=""display:block"">

                <li class=""nav-item"">
                    <a class=""ajaxify""");
            BeginWriteAttribute("href", " href=\"", 1051, "\"", 1094, 1);
#nullable restore
#line 27 "C:\Users\FS\source\repos\WF_WebProject\WF_WebProject\Views\Shared\_LeftMenu.cshtml"
WriteAttributeValue("", 1058, Url.Action("Index", "CompanyInfo" ), 1058, 36, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                        <i class=\"font-blue-soft font-lg glyphicon glyphicon-th-list\"></i>\r\n                        CompanyInfo\r\n                    </a>\r\n                </li>\r\n\r\n\r\n            </ul>\r\n\r\n        </li>\r\n\r\n    </ul>\r\n");
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