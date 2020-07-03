using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WFWebProject.Interface;
using WFWebProject.Models;

namespace WFWebProject.Controllers
{
    [Authorize]
    public class CompanyQualificationsController : Controller
    {
        private readonly DataContext _context;
        private ICompanyQualificationService _companyQualificationService;

        public CompanyQualificationsController(DataContext context, ICompanyQualificationService companyQualificationService)
        {
            _context = context;
            _companyQualificationService = companyQualificationService;
        }

        // GET: CompanyQualifications
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PageData()
        {

            var result = this._companyQualificationService.DTData(HttpContext);
            return Json(result.DtResponse);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(string Id)
        {
            Core.WebServices.Model.CoreRequest coreRequest = new Core.WebServices.Model.CoreRequest(HttpContext);
            Core.WebServices.Model.CoreResponse core_response = new Core.WebServices.Model.CoreResponse(coreRequest);
            this._companyQualificationService.EditData(Id, coreRequest);
            return Json(core_response.DtResponse);
        }
    }
}
