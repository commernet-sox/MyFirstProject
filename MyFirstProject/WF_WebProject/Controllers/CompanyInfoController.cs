using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Database.Repository;
using Core.WebServices.Interface;
using Core.WebServices.Model;
using Core.WebServices.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WFWebProject.DTO;
using WFWebProject.Interface;
using WFWebProject.Models;

namespace WFWebProject.Controllers
{
    
    public class CompanyInfoController : Controller
    {
        private readonly DataContext _context;
        private IHttpContextAccessor _accessor;
        private ICompanyInfoService _companyInfoService;
        public CompanyInfoController(DataContext context,IHttpContextAccessor accessor,ICompanyInfoService companyInfoService) 
        {
            _context = context;
            _accessor = accessor;
            _companyInfoService = companyInfoService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> AllIndex()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> PageData()
        {
            var result = this._companyInfoService.DTData(HttpContext);
            return Json(result.DtResponse);
            
        }

        
        [HttpPost]
        public async Task<IActionResult> CompanyAllPageData()
        {
            var result = this._companyInfoService.CompanyAllPageData(new Core.WebServices.Model.CoreRequest(HttpContext));
            return Json(result.DtResponse);

        }


    }
}
