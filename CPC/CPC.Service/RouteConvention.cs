using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace CPC.Service
{
    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;

        public RouteConvention(IRouteTemplateProvider routeTemplateProvider) => _routePrefix = new AttributeRouteModel(routeTemplateProvider);

        public void Apply(ApplicationModel application)
        {
            //遍历所有的 Controller
            foreach (var controller in application.Controllers)
            {
                // 已经标记了 RouteAttribute 的 Controller
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        // 在 当前路由上 再 添加一个 路由前缀
                        selectorModel.AttributeRouteModel.Template = _routePrefix.Template + selectorModel.AttributeRouteModel.Template;
                    }
                }
            }
        }
    }

    public static class MvcOptionsExtensions
    {
        public static void AddRoutePrefix(this MvcOptions opts, string template)
        {
            // 添加我们自定义 实现IApplicationModelConvention的RouteConvention
            var routeAttr = new RouteAttribute(template);
            opts.Conventions.Insert(0, new RouteConvention(routeAttr));
        }
    }
}
