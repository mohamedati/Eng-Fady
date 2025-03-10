using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Models;

namespace ERP.Controllers
{
    public class MandatesController : Controller
    {
        private readonly ErpservicesContext _context;

        public MandatesController(ErpservicesContext context)
        {
            _context = context;
        }

        // GET: Mandates
        public async Task<IActionResult> Index()
        {
            var erpservicesContext = _context.Mandates.Include(m => m.Countries).Include(m => m.EntitlementTypes);
            return View(await erpservicesContext.ToListAsync());
        }

        // GET: Mandates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mandate = await _context.Mandates
                .Include(m => m.Countries)
                .Include(m => m.EntitlementTypes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mandate == null)
            {
                return NotFound();
            }

            return View(mandate);
        }

        // GET: Mandates/Create
        public IActionResult Create()
        {

            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["TaskId"] = new SelectList(_context.Tasks, "Id", "Name");
            ViewData["EntitlementTypeId"] = new SelectList(_context.EntitlementTypes, "Id", "Name");
            return View();
        }

        // POST: Mandates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mandate mandate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mandate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", mandate.CountryId);
            ViewData["TaskId"] = new SelectList(_context.Tasks, "Id", "Name");
            ViewData["EntitlementTypeId"] = new SelectList(_context.EntitlementTypes, "Id", "Name", mandate.EntitlementTypeId);
            return View(mandate);
        }

        // GET: Mandates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mandate = await _context.Mandates.FindAsync(id);
            if (mandate == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", mandate.CountryId);
            ViewData["EntitlementTypeId"] = new SelectList(_context.EntitlementTypes, "Id", "Name", mandate.EntitlementTypeId);
            return View(mandate);
        }

        // POST: Mandates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Mandate mandate)
        {
            if (id != mandate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mandate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MandateExists(mandate.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", mandate.CountryId);
            ViewData["EntitlementTypeId"] = new SelectList(_context.EntitlementTypes, "Id", "Name", mandate.EntitlementTypeId);
            return View(mandate);
        }

        // GET: Mandates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mandate = await _context.Mandates
                .Include(m => m.Countries)
                .Include(m => m.EntitlementTypes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mandate == null)
            {
                return NotFound();
            }

            return View(mandate);
        }

        // POST: Mandates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mandate = await _context.Mandates.FindAsync(id);
            if (mandate != null)
            {
                _context.Mandates.Remove(mandate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MandateExists(int id)
        {
            return _context.Mandates.Any(e => e.Id == id);
        }
    }
}
