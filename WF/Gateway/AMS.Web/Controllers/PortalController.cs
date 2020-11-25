using CPC;
using AMS.WebCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Web.Controllers
{
    [Authorize]
    public class PortalController : BaseController
    {
        public PortalController()
        {
        }

        public IActionResult Portal()
        {
            return View();
        }

        public IActionResult Main()
        {
            return View();
        }

        public IActionResult Left()
        {
            return View();
        }

        public IActionResult Top()
        {
            return View();
        }
        public IActionResult Center()
        {
            return View();
        }
        //[HttpGet]
        //public JsonResult GetOrderCountInfo()
        //{
        //    var result = GlobalContext.Resolve<IAPIClient>().GetData("/common/getordercountinfo", null);
        //    return Json(result);
        //}

        //[HttpGet]
        //public JsonResult GetOrderInfo()
        //{
        //    var result = GlobalContext.Resolve<IAPIClient>().GetData("/common/getorderinfo", null);
        //    return Json(result);
        //}
    }
}