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
    public class SystemController : BaseController
    {
        public IActionResult System()
        {
            return View();
        }

        public PageData<List<SysSystemDTO>> GetList(SystemModel model)
        {
            var result = Get("/os/1.0/system/getlist", model).As<PageData<List<SysSystemDTO>>>().Result;
            return result;
        }

        public Outcome Insert(SysSystemDTO dto)
        {
            var result = Post("/os/1.0/system/insert", dto).As<Outcome>().Result;
            return result;
        }

        public Outcome Update(SysSystemDTO dto)
        {
            var result = Post("/os/1.0/system/update", dto).As<Outcome>().Result;
            return result;
        }
    }
}