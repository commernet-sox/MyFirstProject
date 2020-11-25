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
    public class RoleMenuController : BaseController
    {
        public IActionResult RoleMenu()
        {
            return View();
        }

        public List<MenuItemModel> GetMenuCommand(string sysCode)
        {
            var result = Get("/os/1.0/rolemenu/getmenucommand", new { sysCode= sysCode }).As<List<MenuItemModel>>().Result;
            return result;
        }

        public List<SysRolemenuDTO> GetRoleMenu(string roleId)
        {
            var result = Get("/os/1.0/rolemenu/getrolemenu", new { roleId = roleId }).As<List<SysRolemenuDTO>>().Result;
            return result;
        }

        public Outcome Insert(SysRolemenuDTO[] menus)
        {
            var result = Post("/os/1.0/rolemenu/insert", menus).As<Outcome>().Result;
            return result;
        }

        public Outcome Update(SysRolemenuDTO[] menus)
        {
            var result = Post("/os/1.0/rolemenu/update", menus).As<Outcome>().Result;
            return result;
        }
    }
}