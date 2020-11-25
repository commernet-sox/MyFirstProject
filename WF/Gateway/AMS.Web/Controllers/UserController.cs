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
    public class UserController : BaseController
    {
        public IActionResult User()
        {
            return View();
        }
        public PageData<List<SysUserDTO>> GetList(UserModel model)
        {
            var result = Get("/os/1.0/user/getlist", model).As<PageData<List<SysUserDTO>>>().Result;
            return result;
        }

        public Outcome Insert(SysUserDTO dto)
        {
            var result = Post("/os/1.0/user/insert", dto).As<Outcome>().Result;
            return result;
        }

        public Outcome Update(SysUserDTO dto)
        {
            var result = Post("/os/1.0/user/update", dto).As<Outcome>().Result;
            return result;
        }
    }
}
