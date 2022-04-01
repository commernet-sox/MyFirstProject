using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace CoreSixTest.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!base.ModelState.IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (ModelStateEntry value in base.ModelState.Values)
                {
                    foreach (ModelError error in value.Errors)
                    {
                        stringBuilder.Append(error.ErrorMessage + ";");
                    }
                }

                context.Result =Json( stringBuilder.ToString().TrimEnd(';'));
            }
            base.OnActionExecuting(context);
        }
    }
}
