using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static ERP.Models.clsEnum;

namespace ERP.Controllers
{
    [Authorize]
    public class VacationTypesController : Controller
    {
        private readonly ErpservicesContext _context;
        private readonly ViewModel _viewModel;

        public VacationTypesController(ErpservicesContext context, ViewModel viewModel)
        {
            _context = context;
            _viewModel = viewModel;
        }

        // GET: VacationTypes
        public async Task<IActionResult> Index()
        {
            var erpservicesContext = _context.VacationTypes.Include(v => v.Creator);
            return View(await erpservicesContext.ToListAsync());
        }

        // GET: VacationTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationType = await _context.VacationTypes
                .Include(v => v.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationType == null)
            {
                return NotFound();
            }

            return View(vacationType);
        }

        // GET: VacationTypes/Create
        public IActionResult Create()
        {
            ViewData["CreatorId"] = new SelectList(_context.AspNetUsers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacationType vacationType)
        {
            try
            {
                // Get the current logged-in user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // This retrieves the user's ID

                if (string.IsNullOrEmpty(userId))
                {
                    // Handle the case where the user is not logged in
                    return Json(new { success = false, message = "User is not authenticated." });
                }

                // Assign the current user's ID to the CreatorId property
                vacationType.CreatorId = userId;

                // Add the vacationType to the context and save changes
                _context.Add(vacationType);
                await _context.SaveChangesAsync();

                // Return a success response
                return Json(new { success = true, message = "تم حفظ البيانات بنجاح" });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                // _logger.LogError(ex, "An error occurred while creating a vacation type.");

                // Return the view with the model to display validation errors
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: VacationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationType = await _context.VacationTypes.FindAsync(id);
            if (vacationType == null)
            {
                return NotFound();
            }
            ViewData["CreatorId"] = new SelectList(_context.AspNetUsers, "Id", "Name", vacationType.CreatorId);
            return View(vacationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VacationType vacationType,string cReportUrl)
        {
            if (id != vacationType.Id)
            {
                return NotFound();
            }

           
                try
                {
              
                    _context.Update(vacationType);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "تم حفظ البيانات بنجاح!" });

                }
                catch (DbUpdateConcurrencyException)
                {
                if (!VacationTypeExists(vacationType.Id))
                {
                    return NotFound();
                }
               
                }
            return View(vacationType);
        }
          
         
    

        // GET: VacationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationType = await _context.VacationTypes
                .Include(v => v.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationType == null)
            {
                return NotFound();
            }

            return View(vacationType);
        }

        // POST: VacationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacationType = await _context.VacationTypes.FindAsync(id);
            if (vacationType != null)
            {
                _context.VacationTypes.Remove(vacationType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> RemoveByAjax(int id)
        {

            return await clsCustomDelete<VacationType>.RemoveByAjax(id, _context.VacationTypes, _context);

        }

        private bool VacationTypeExists(int id)
        {
            return _context.VacationTypes.Any(e => e.Id == id);
        }
    }
}
