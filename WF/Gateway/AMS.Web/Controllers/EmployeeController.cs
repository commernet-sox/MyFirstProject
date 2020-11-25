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
    public class EmployeeController : BaseController
    {
        public IActionResult Employee()
        {
            return View();
        }

        public PageData<List<SysEmployeeDTO>> GetList(EmployeeModel model)
        {
            var result = Get("/os/1.0/employee/getlist", model).As<PageData<List<SysEmployeeDTO>>>().Result;
            return result;
        }

        public Outcome Insert(SysEmployeeDTO dto)
        {
            var result = Post("/os/1.0/employee/insert", dto).As<Outcome>().Result;
            return result;
        }

        public Outcome Update(SysEmployeeDTO dto)
        {
            var result = Post("/os/1.0/employee/update", dto).As<Outcome>().Result;
            return result;
        }
    }
}