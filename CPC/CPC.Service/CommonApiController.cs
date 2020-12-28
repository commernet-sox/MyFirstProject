using AspectCore.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text;

namespace CPC.Service
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommonApiController : Controller
    {
        #region Result
        [NonAction]
        public virtual BodyResult Custom(ApiCode code, string message = "") => Custom(new Outcome(code, message));

        [NonAction]
        public virtual BodyResult Custom(Outcome oc) => new BodyResult(HttpStatusCode.OK, oc);
        #endregion

        #region Override
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

                context.Result = Custom(new Outcome { Code = ApiCode.InvalidData, Message = errMsg.ToString().TrimEnd(';') });
            }

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is ApiException apiException)
                {
                    if (apiException.Body is Outcome oc)
                    {
                        context.Result = Custom(oc);
                    }
                    else
                    {
                        oc = new Outcome<object>(apiException.Code, string.Empty, apiException.Body);
                        context.Result = Custom(oc);
                    }

                }
                else
                {
                    LogUtility.Error(context.Exception, nameof(CommonApiController));
                    var env = HttpContext.RequestServices.Resolve<IWebHostEnvironment>();
                    if (env?.IsDevelopment() ?? false)
                    {
                        context.Result = Custom(ApiCode.SystemBusy, context.Exception.ToString());
                    }
                    else
                    {
                        context.Result = Custom(ApiCode.SystemBusy, "system busy");
                    }
                }
                context.ExceptionHandled = true;
            }

            base.OnActionExecuted(context);
        }
        #endregion
    }
}
