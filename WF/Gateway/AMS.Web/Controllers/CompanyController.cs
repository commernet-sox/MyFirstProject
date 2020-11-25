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
    public class CompanyController : BaseController
    {
        public IActionResult Company()
        {
            return View();
        }

        public PageData<List<SysCompanyDTO>> GetList(CompanyModel model)
        {
            var result = Get("/os/1.0/company/getlist", model).As<PageData<List<SysCompanyDTO>>>().Result;
            return result;
        }

        public Outcome Insert(SysCompanyDTO dto)
        {
            var result = Post("/os/1.0/company/insert", dto).As<Outcome>().Result;
            return result;
        }

        public Outcome Update(SysCompanyDTO dto)
        {
            var result = Post("/os/1.0/company/update", dto).As<Outcome>().Result;
            return result;
        }
    }
}