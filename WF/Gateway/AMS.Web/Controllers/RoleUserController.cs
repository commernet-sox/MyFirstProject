using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.IdentityService;
using Data.IdentityService.Model;
using AMS.WebCore;
using CPC;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Web.Controllers
{
    public class RoleUserController : BaseController
    {
        public IActionResult RoleUser()
        {
            return View();
        }
        public PageData<List<SysRoleuserDTO>> GetList(RoleuserModel model)
        {
            var result = Get("/os/1.0/roleuser/getlist", model).As<PageData<List<SysRoleuserDTO>>>().Result;
            return result;
        }

        public Outcome Insert(SysRoleuserDTO dto)
        {
            var result = Post("/os/1.0/roleuser/insert", dto).As<Outcome>().Result;
            return result;
        }

        public Outcome Update(SysRoleuserDTO[] dto)
        {
            var result = Post("/os/1.0/roleuser/update", dto).As<Outcome>().Result;
            return result;
        }

        public List<SysRoleuserDTO> GetRoleUserMenu(string UserId)
        {
            var result = Get("/os/1.0/roleuser/GetRoleUserMenu", new { UserId = UserId }).As<List<SysRoleuserDTO>>().Result;
            return result;
        }

        public List<MenuItemModel> GetRoleUserCommand()
        {
            var result = Get("/os/1.0/roleuser/GetRoleUserCommand").As<List<MenuItemModel>>().Result;
            return result;
        }
    }
}
