using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.WebServices.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WFWebProject.Models;

namespace WFWebProject.Controllers
{
    public class CompanyInfoController : Controller
    {
        private readonly DataContext _context;
        private IHttpContextAccessor _accessor;
        public CompanyInfoController(DataContext context,IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        // GET: CodeMasters
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PageData()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("data", await _context.CompanyInfo.ToListAsync());
            dic.Add("options", "");
            dic.Add("files", "");

            //var core_request = new Core.WebServices.Model.CoreRequest(_accessor.HttpContext);
            //CoreResponse core_response = new CoreResponse(core_request);
            //core_response.DtResponse.data = await _context.CompanyInfo.ToListAsync();
            return Json(dic);
            
        }
        // GET: CodeMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeMaster = await _context.CompanyInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codeMaster == null)
            {
                return NotFound();
            }

            return View(codeMaster);
        }

        // GET: CodeMasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CodeMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModifyTime,Modifier,CreateTime,Creator,CodeGroup,CodeId,CodeName,IsActive,Remarks,HUDF_01")] CompanyInfo codeMaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(codeMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(codeMaster);
        }

        // GET: CodeMasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeMaster = await _context.CompanyInfo.FindAsync(id);
            if (codeMaster == null)
            {
                return NotFound();
            }
            return View(codeMaster);
        }

        // POST: CodeMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModifyTime,Modifier,CreateTime,Creator,CodeGroup,CodeId,CodeName,IsActive,Remarks,HUDF_01")] CompanyInfo codeMaster)
        {
            if (id != codeMaster.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(codeMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodeMasterExists(codeMaster.Id))
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
            return View(codeMaster);
        }

        // GET: CodeMasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeMaster = await _context.CompanyInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codeMaster == null)
            {
                return NotFound();
            }

            return View(codeMaster);
        }

        // POST: CodeMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var codeMaster = await _context.CompanyInfo.FindAsync(id);
            _context.CompanyInfo.Remove(codeMaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodeMasterExists(int id)
        {
            return _context.CompanyInfo.Any(e => e.Id == id);
        }
    }
}
