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
        // GET: CodeMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeMaster = await _context.CodeMaster
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
        public async Task<IActionResult> Create([Bind("Id,ModifyTime,Modifier,CreateTime,Creator,CodeGroup,CodeId,CodeName,IsActive,Remarks,HUDF_01")] CodeMaster codeMaster)
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

            var codeMaster = await _context.CodeMaster.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModifyTime,Modifier,CreateTime,Creator,CodeGroup,CodeId,CodeName,IsActive,Remarks,HUDF_01")] CodeMaster codeMaster)
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

            var codeMaster = await _context.CodeMaster
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
            var codeMaster = await _context.CodeMaster.FindAsync(id);
            _context.CodeMaster.Remove(codeMaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodeMasterExists(int id)
        {
            return _context.CodeMaster.Any(e => e.Id == id);
        }
    }
}
