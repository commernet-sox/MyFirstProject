using AspectCore.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text;

namespace CPC.Service
{
    [Route("{v:apiVersion}/[controller]")]
    [ApiController]
    public class RestApiController : Controller
    {
        #region Result
        [NonAction]
        public virtual BodyResult Custom(ApiCode code, string message = "") => new BodyResult(code, message);

        [NonAction]
        public virtual BodyResult Custom(Outcome oc) => new BodyResult(oc);

        [NonAction]
        public virtual BodyResult Custom(ApiCode code, object value) => new BodyResult(code, value);

        [NonAction]
        public virtual BodyResult Custom(HttpStatusCode code, object value) => new BodyResult(code, value);
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

                context.Result = Custom(ApiCode.InvalidData, errMsg.ToString().TrimEnd(';'));
            }

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is ApiException apiException)
                {
                    context.Result = Custom(apiException.Code, apiException.Body);
                }
                else
                {
                    LogUtility.Error(context.Exception, nameof(RestApiController));
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
