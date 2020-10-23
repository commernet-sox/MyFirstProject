using AspectCore.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using SDT.BaseTool;
using System.Text;

namespace SDT.Service
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommonApiController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!ModelState.IsValid)
            {
                var errMsg = new StringBuilder();
                foreach (var val in ModelState.Values)
                {
                    foreach (var err in val.Errors)
                    {
                        errMsg.Append(err.ErrorMessage + ";");
                    }
                }

                context.Result = new JsonResult(new Outcome(ApiCode.InvalidData, errMsg.ToString().TrimEnd(';')));
            }

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                context.ExceptionHandled = true;
                if (context.Exception is ApiException apiException)
                {
                    context.Result = Json(apiException.Body);
                }
                else
                {
                    LogUtility.Error(context.Exception, nameof(CommonApiController));
                    var env = HttpContext.RequestServices.Resolve<IWebHostEnvironment>();
                    if (env?.IsDevelopment() ?? false)
                    {
                        context.Result = Json(new Outcome(ApiCode.SystemBusy, context.Exception.ToString()));
                    }
                    else
                    {
                        context.Result = Json(new Outcome(ApiCode.SystemBusy, "system busy"));
                    }
                }
            }

            base.OnActionExecuted(context);
        }
    }
}
