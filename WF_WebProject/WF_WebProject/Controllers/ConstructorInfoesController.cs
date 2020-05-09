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
    public class ConstructorInfoesController : Controller
    {
        private readonly DataContext _context;

        public ConstructorInfoesController(DataContext context)
        {
            _context = context;
        }

        // GET: ConstructorInfoes
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PageData()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("data", await _context.ConstructorInfo.ToListAsync());
            dic.Add("options", "");
            dic.Add("files", "");

            //var core_request = new Core.WebServices.Model.CoreRequest(_accessor.HttpContext);
            //CoreResponse core_response = new CoreResponse(core_request);
            //core_response.DtResponse.data = await _context.CompanyInfo.ToListAsync();
            return Json(dic);

        }
        // GET: ConstructorInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var constructorInfo = await _context.ConstructorInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (constructorInfo == null)
            {
                return NotFound();
            }

            return View(constructorInfo);
        }

        // GET: ConstructorInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConstructorInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Province,CompanyName,Constructor,RegisterNumber,PracticeSealNo,RegisterCertNo,QualificationCertNo,DateIssue,RegisterMajor,ValidityRegistration")] ConstructorInfo constructorInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(constructorInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(constructorInfo);
        }

        // GET: ConstructorInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var constructorInfo = await _context.ConstructorInfo.FindAsync(id);
            if (constructorInfo == null)
            {
                return NotFound();
            }
            return View(constructorInfo);
        }

        // POST: ConstructorInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Province,CompanyName,Constructor,RegisterNumber,PracticeSealNo,RegisterCertNo,QualificationCertNo,DateIssue,RegisterMajor,ValidityRegistration")] ConstructorInfo constructorInfo)
        {
            if (id != constructorInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(constructorInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConstructorInfoExists(constructorInfo.Id))
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
            return View(constructorInfo);
        }

        // GET: ConstructorInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var constructorInfo = await _context.ConstructorInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (constructorInfo == null)
            {
                return NotFound();
            }

            return View(constructorInfo);
        }

        // POST: ConstructorInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var constructorInfo = await _context.ConstructorInfo.FindAsync(id);
            _context.ConstructorInfo.Remove(constructorInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConstructorInfoExists(int id)
        {
            return _context.ConstructorInfo.Any(e => e.Id == id);
        }
    }
}
