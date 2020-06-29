using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WFWebProject.Interface;
using WFWebProject.Models;
using WFWebProject.Service;

namespace WFWebProject.Controllers
{
    [Authorize]
    public class CodeMastersController : Controller
    {
        private readonly DataContext _context;
        private ICodeMasterService _codeMasterService;
        public CodeMastersController(DataContext context,ICodeMasterService codeMasterService)
        {
            _context = context;
            _codeMasterService = codeMasterService;
        }

        // GET: CodeMasters
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PageData()
        {
            var result = this._codeMasterService.DTData(HttpContext);
            return Json(result.DtResponse);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(string Id)
        {
            Core.WebServices.Model.CoreRequest coreRequest = new Core.WebServices.Model.CoreRequest(HttpContext);
            Core.WebServices.Model.CoreResponse core_response = new Core.WebServices.Model.CoreResponse(coreRequest);
            this._codeMasterService.EditData(Id, coreRequest);
            return Json(core_response.DtResponse);
        }
    }
}
