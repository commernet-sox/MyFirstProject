#pragma checksum "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4ecd7671ecff5b529ed945cf1bf772ff408ab4ab"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_Menu_Default), @"mvc.1.0.view", @"/Views/Shared/Components/Menu/Default.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/Components/Menu/Default.cshtml", typeof(AspNetCore.Views_Shared_Components_Menu_Default))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4ecd7671ecff5b529ed945cf1bf772ff408ab4ab", @"/Views/Shared/Components/Menu/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dde7983aead9016f0b7a49cb549d184d43c16a26", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_Menu_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<MenuModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(18, 134, true);
            WriteLiteral("<nav class=\"mt-2\">\r\n    <ul class=\"nav nav-pills nav-sidebar flex-column\" data-widget=\"treeview\" role=\"menu\" data-accordion=\"false\">\r\n");
            EndContext();
#line 4 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
         if (Model.MenuName.Where(t => t.ToString() == "系统数据").FirstOrDefault() != null)
        {

#line default
#line hidden
            BeginContext(253, 388, true);
            WriteLiteral(@"            <li class=""nav-item has-treeview menu-open"">
                <a href=""#"" class=""nav-link active"">
                    <i class=""nav-icon fa fa-dashboard""></i>
                    <p>
                        系统数据
                        <i class=""right fa fa-angle-left""></i>
                    </p>
                </a>
                <ul class=""nav nav-treeview"">
");
            EndContext();
#line 15 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                     if (Model.MenuName.Where(t => t.ToString() == "菜单管理").FirstOrDefault() != null)
                    {

#line default
#line hidden
            BeginContext(766, 77, true);
            WriteLiteral("                        <li class=\"nav-item\">\r\n                            <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 843, "\"", 877, 1);
#line 18 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
WriteAttributeValue("", 850, Url.Action("Menu", "Home"), 850, 27, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(878, 204, true);
            WriteLiteral(" class=\"nav-link \">\r\n                                <i class=\"fa fa-circle-o nav-icon\"></i>\r\n                                <p>菜单管理</p>\r\n                            </a>\r\n                        </li>\r\n");
            EndContext();
#line 23 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                    }

#line default
#line hidden
            BeginContext(1105, 20, true);
            WriteLiteral("                    ");
            EndContext();
#line 24 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                     if (Model.MenuName.Where(t => t.ToString() == "测试数据").FirstOrDefault() != null)
                    {

#line default
#line hidden
            BeginContext(1230, 77, true);
            WriteLiteral("                        <li class=\"nav-item\">\r\n                            <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 1307, "\"", 1349, 1);
#line 27 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
WriteAttributeValue("", 1314, Url.Action("Index", "CodeMasters"), 1314, 35, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1350, 203, true);
            WriteLiteral(" class=\"nav-link\">\r\n                                <i class=\"fa fa-circle-o nav-icon\"></i>\r\n                                <p>测试数据</p>\r\n                            </a>\r\n                        </li>\r\n");
            EndContext();
#line 32 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                    }

#line default
#line hidden
            BeginContext(1576, 42, true);
            WriteLiteral("                </ul>\r\n            </li>\r\n");
            EndContext();
#line 35 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
        }

#line default
#line hidden
            BeginContext(1629, 8, true);
            WriteLiteral("        ");
            EndContext();
#line 36 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
         if (Model.MenuName.Where(t => t.ToString() == "基础数据").FirstOrDefault() != null)
        {

#line default
#line hidden
            BeginContext(1730, 390, true);
            WriteLiteral(@"            <li class=""nav-item has-treeview menu-open"">
                <a href=""#"" class=""nav-link active"">
                    <i class=""nav-icon fa fa-dashboard""></i>
                    <p>
                        基础数据
                        <i class=""right fa fa-angle-left""></i>
                    </p>
                </a>
                <ul class=""nav nav-treeview"">

");
            EndContext();
#line 48 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                     if (Model.MenuName.Where(t => t.ToString() == "企业基本信息").FirstOrDefault() != null)
                    {

#line default
#line hidden
            BeginContext(2247, 77, true);
            WriteLiteral("                        <li class=\"nav-item\">\r\n                            <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 2324, "\"", 2366, 1);
#line 51 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
WriteAttributeValue("", 2331, Url.Action("Index", "CompanyInfo"), 2331, 35, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2367, 206, true);
            WriteLiteral(" class=\"nav-link \">\r\n                                <i class=\"fa fa-circle-o nav-icon\"></i>\r\n                                <p>企业基本信息</p>\r\n                            </a>\r\n                        </li>\r\n");
            EndContext();
#line 56 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                    }

#line default
#line hidden
            BeginContext(2596, 20, true);
            WriteLiteral("                    ");
            EndContext();
#line 57 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                     if (Model.MenuName.Where(t => t.ToString() == "企业资质信息").FirstOrDefault() != null)
                    {

#line default
#line hidden
            BeginContext(2723, 77, true);
            WriteLiteral("                        <li class=\"nav-item\">\r\n                            <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 2800, "\"", 2852, 1);
#line 60 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
WriteAttributeValue("", 2807, Url.Action("Index", "CompanyQualifications"), 2807, 45, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2853, 205, true);
            WriteLiteral(" class=\"nav-link\">\r\n                                <i class=\"fa fa-circle-o nav-icon\"></i>\r\n                                <p>企业资质信息</p>\r\n                            </a>\r\n                        </li>\r\n");
            EndContext();
#line 65 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                    }

#line default
#line hidden
            BeginContext(3081, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 67 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                     if (Model.MenuName.Where(t => t.ToString() == "建造师信息").FirstOrDefault() != null)
                    {

#line default
#line hidden
            BeginContext(3209, 77, true);
            WriteLiteral("                        <li class=\"nav-item\">\r\n                            <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 3286, "\"", 3334, 1);
#line 70 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
WriteAttributeValue("", 3293, Url.Action("Index", "ConstructorInfoes"), 3293, 41, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3335, 204, true);
            WriteLiteral(" class=\"nav-link\">\r\n                                <i class=\"fa fa-circle-o nav-icon\"></i>\r\n                                <p>建造师信息</p>\r\n                            </a>\r\n                        </li>\r\n");
            EndContext();
#line 75 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                    }

#line default
#line hidden
            BeginContext(3562, 20, true);
            WriteLiteral("                    ");
            EndContext();
#line 76 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                     if (Model.MenuName.Where(t => t.ToString() == "企业全部信息").FirstOrDefault() != null)
                    {

#line default
#line hidden
            BeginContext(3689, 77, true);
            WriteLiteral("                        <li class=\"nav-item\">\r\n                            <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 3766, "\"", 3811, 1);
#line 79 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
WriteAttributeValue("", 3773, Url.Action("AllIndex", "CompanyInfo"), 3773, 38, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3812, 205, true);
            WriteLiteral(" class=\"nav-link\">\r\n                                <i class=\"fa fa-circle-o nav-icon\"></i>\r\n                                <p>企业全部信息</p>\r\n                            </a>\r\n                        </li>\r\n");
            EndContext();
#line 84 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
                    }

#line default
#line hidden
            BeginContext(4040, 44, true);
            WriteLiteral("\r\n                </ul>\r\n            </li>\r\n");
            EndContext();
#line 88 "C:\Users\FS\source\repos\MyFirstProject\WF_WebProject\Views\Shared\Components\Menu\Default.cshtml"
        }

#line default
#line hidden
            BeginContext(4095, 23, true);
            WriteLiteral("\r\n        </ul>\r\n</nav>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<MenuModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
