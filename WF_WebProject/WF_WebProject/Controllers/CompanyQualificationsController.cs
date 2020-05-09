using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WFWebProject.Models;

namespace WFWebProject.Controllers
{
    public class CompanyQualificationsController : Controller
    {
        private readonly DataContext _context;

        public CompanyQualificationsController(DataContext context)
        {
            _context = context;
        }

        // GET: CompanyQualifications
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PageData()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("data", await _context.CompanyQualification.ToListAsync());
            dic.Add("options", "");
            dic.Add("files", "");

            //var core_request = new Core.WebServices.Model.CoreRequest(_accessor.HttpContext);
            //CoreResponse core_response = new CoreResponse(core_request);
            //core_response.DtResponse.data = await _context.CompanyInfo.ToListAsync();
            return Json(dic);

        }

        // GET: CompanyQualifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyQualification = await _context.CompanyQualification
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyQualification == null)
            {
                return NotFound();
            }

            return View(companyQualification);
        }

        // GET: CompanyQualifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyQualifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,EconomicType,Province,City,Time,Email,WebSite,QualificationType,ContactAddress,ZipCode,SafetyLicenseNo,StartDate,EndDate,IssuingAuthority,ScopeLicense,OrganizationCode,ComprehensiveScore")] CompanyQualification companyQualification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(companyQualification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companyQualification);
        }

        // GET: CompanyQualifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyQualification = await _context.CompanyQualification.FindAsync(id);
            if (companyQualification == null)
            {
                return NotFound();
            }
            return View(companyQualification);
        }

        // POST: CompanyQualifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,EconomicType,Province,City,Time,Email,WebSite,QualificationType,ContactAddress,ZipCode,SafetyLicenseNo,StartDate,EndDate,IssuingAuthority,ScopeLicense,OrganizationCode,ComprehensiveScore")] CompanyQualification companyQualification)
        {
            if (id != companyQualification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyQualification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyQualificationExists(companyQualification.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(companyQualification);
        }

        // GET: CompanyQualifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyQualification = await _context.CompanyQualification
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyQualification == null)
            {
                return NotFound();
            }

            return View(companyQualification);
        }

        // POST: CompanyQualifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyQualification = await _context.CompanyQualification.FindAsync(id);
            _context.CompanyQualification.Remove(companyQualification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyQualificationExists(int id)
        {
            return _context.CompanyQualification.Any(e => e.Id == id);
        }
    }
}
