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
    public class DepartmentController : BaseController
    {
        public IActionResult Department()
        {
            return View();
        }

        public PageData<List<SysDepartmentDTO>> GetList(DepartmentModel model)
        {
            var result = Get("/os/1.0/department/getlist", model).As<PageData<List<SysDepartmentDTO>>>().Result;
            return result;
        }

        public Outcome Insert(SysDepartmentDTO dto)
        {
            var result = Post("/os/1.0/department/insert", dto).As<Outcome>().Result;
            return result;
        }

        public Outcome Update(SysDepartmentDTO dto)
        {
            var result = Post("/os/1.0/department/update", dto).As<Outcome>().Result;
            return result;
        }
    }
}