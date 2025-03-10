using System.Security.Claims;
using ERP.Models;
using ERP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    public class MandateController1 : Controller
    {
        private readonly ErpservicesContext _context;
        private readonly ViewModel _viewModel;

        public MandateController1(ErpservicesContext context, ViewModel viewModel)
        {
            _context = context;
            _viewModel = viewModel;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            CreateMandateFormViewModel createMandateFormViewModel = new()
            {
                Tasks = _context.Tasks.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList(),
                Countries = _context.Countries.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),
                EntitlementTypes = _context.EntitlementTypes.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                }).ToList()
            };
            ViewData["EmployeeId"] = new SelectList(_context.AspNetUsers, "Id", "Name");
            return View(createMandateFormViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMandateFormViewModel createMandateForm, IFormFile? ReportUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                // Handle the case where the user is not logged in
                return Json(new { success = false, message = "User is not authenticated." });
            }
            if (ReportUrl != null)
            {
                string reportUrl = await _viewModel.UploadAttachment(ReportUrl, "wwwroot/Uploads/Mandates");
                if (!string.IsNullOrEmpty(reportUrl))
                    createMandateForm.ReportUrl = reportUrl;
            }
            Mandate mandate = new()
            {
                TaskDescription = createMandateForm.TaskDescription,
                City = createMandateForm.City,
                MandateDestination = createMandateForm.MandateDestination,
                TaskId = createMandateForm.TaskId,
                CountryId = createMandateForm.CountryId,
                EntitlementTypeId = createMandateForm.EntitlementTypeId,
                StartDate = createMandateForm.StartDate,
                EndDate = createMandateForm.EndDate,
                OffDaysCount = createMandateForm.OffDaysCount,
                ReportUrl = createMandateForm.ReportUrl,
                //Tasks = _context.Tasks.Select(t=> new TaskName {
                //    Id = t.TaskId,
                //    Name = _context.Tasks.Find(createMandateForm.TaskId).Name
                //}).ToList(),
                //Countries = _context.Countries.Find(createMandateForm.CountryId),
                //EntitlementTypes = _context.EntitlementTypes.Find(createMandateForm.EntitlementTypeId),

            };
            _context.Add(mandate);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم حفظ البيانات بنجاح" });
        }
        public IActionResult EndTask()
        {
            return View();
        }
    }
}
