using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.IdentityService;
using Data.IdentityService.Model;
using AMS.WebCore;
using CPC;
using CPC.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Web.Controllers
{
    public class OplogController : BaseController
    {
        public IActionResult Oplog()
        {
            return View();
        }

        public PageData<List<SysOplogDTO>> GetList(OplogModel model)
        {
            var result = Get("/os/1.0/oplog/getlist", model).As<PageData<List<SysOplogDTO>>>().Result;
            return result;
        }
    }
}