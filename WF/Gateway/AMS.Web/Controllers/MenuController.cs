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
    public class MenuController : BaseController
    {
        public IActionResult Menu()
        {
            return View();
        }

        public PageData<List<SysMenuDTO>> GetList(MenuModel model)
        {
            var result = Get("/os/1.0/menu/getlist", model).As<PageData<List<SysMenuDTO>>>().Result;
            return result;
        }

        public List<SysCommandDTO> GetCommands(string mid)
        {
            var result = Get("/os/1.0/menu/getcommands", new { mid=mid}).As<List<SysCommandDTO>>().Result;
            return result;
        }

    public Outcome Insert(MenuCommandModel model)
    {
        var result = Post("/os/1.0/menu/insert", model).As<Outcome>().Result;
        return result;
    }

    public Outcome Update(MenuCommandModel model)
    {
        var result = Post("/os/1.0/menu/update", model).As<Outcome>().Result;
        return result;
    }
}
}