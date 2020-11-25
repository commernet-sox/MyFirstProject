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
    public class RoleController : BaseController
    {
        public IActionResult Role()
        {
            return View();
        }

        public PageData<List<SysRoleDTO>> GetList(RoleModel model)
        {
            var result = Post("/os/1.0/role/getlist", model).As<PageData<List<SysRoleDTO>>>().Result;
            return result;
        }

        public Outcome Insert(SysRoleDTO model)
        {
            var result = Post("/os/1.0/role/insert", model).As<Outcome>().Result;
            return result;
        }

        public Outcome Update(SysRoleDTO model)
        {
            var result = Post("/os/1.0/role/update", model).As<Outcome>().Result;
            return result;
        }
    }
}