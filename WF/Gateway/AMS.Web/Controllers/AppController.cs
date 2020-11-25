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
    public class AppController : BaseController
    {
        public IActionResult App()
        {
            return View();
        }

        public PageData<List<AuthAppDTO>> GetList(AppModel model)
        {
            var result = Get("/os/1.0/app/getlist", model).As<PageData<List<AuthAppDTO>>>().Result;
            return result;
        }

        public Outcome Insert(AuthAppDTO dto)
        {
            var result = Post("/os/1.0/app/insert", dto).As<Outcome>().Result;
            return result;
        }

        public Outcome Update(AuthAppDTO dto)
        {
            var result = Post("/os/1.0/app/update", dto).As<Outcome>().Result;
            return result;
        }
    }
}