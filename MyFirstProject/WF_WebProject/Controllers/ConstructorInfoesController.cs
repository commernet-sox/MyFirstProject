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
    public class ConstructorInfoesController : Controller
    {
        private readonly DataContext _context;
        private IConstructorInfoService _ConstructorInfoService;

        public ConstructorInfoesController(DataContext context, IConstructorInfoService constructorInfoService)
        {
            _context = context;
            _ConstructorInfoService = constructorInfoService;
        }

        // GET: ConstructorInfoes
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PageData()
        {
            var result = this._ConstructorInfoService.DTData(HttpContext);
            return Json(result.DtResponse);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string Id)
        {
            Core.WebServices.Model.CoreRequest coreRequest = new Core.WebServices.Model.CoreRequest(HttpContext);
            Core.WebServices.Model.CoreResponse core_response = new Core.WebServices.Model.CoreResponse(coreRequest);
            this._ConstructorInfoService.EditData(Id, coreRequest);
            return Json(core_response.DtResponse);
        }
    }
}
