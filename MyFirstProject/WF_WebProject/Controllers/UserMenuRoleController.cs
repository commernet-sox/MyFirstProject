using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WFWebProject.Interface;

namespace WFWebProject.Controllers
{
    public class UserMenuRoleController : Controller
    {
        private IUserMenuRoleService _userMenuRoleService;
        public UserMenuRoleController(IUserMenuRoleService userMenuRoleService)
        {
            _userMenuRoleService = userMenuRoleService;
        }
        [HttpPost]
        public IActionResult Index()
        {
            var result = this._userMenuRoleService.DTData(HttpContext);
            return Json(result.DtResponse);
        }
        [HttpPost]
        public IActionResult Create()
        {
            var result = this._userMenuRoleService.DTData(HttpContext);
            return Json(result.DtResponse);
        }
        [HttpPost]
        public IActionResult Update()
        {
            var result = this._userMenuRoleService.DTData(HttpContext);
            return Json(result.DtResponse);
        }
    }
}