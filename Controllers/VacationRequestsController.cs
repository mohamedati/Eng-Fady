using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Models;
using System.Security.Claims;
using System.Numerics;
using static ERP.Models.clsEnum;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ERP.Controllers
{
    public class VacationRequestsController : Controller
    {
        private readonly ErpservicesContext _context;
        private readonly ViewModel _viewModel;
        private readonly UserManager<ApplicationUser> _userManager;

        public VacationRequestsController(ErpservicesContext context, ViewModel viewModel,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _viewModel = viewModel;
            _userManager = userManager;
        }

        // GET: VacationRequests
        public async Task<IActionResult> Index()
        {
            var erpservicesContext = _context.VacationRequests.Include(v => v.Employee).Include(v => v.VacationType);
            return View(await erpservicesContext.ToListAsync());
        }

        // GET: VacationRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationRequest = await _context.VacationRequests
                
                .Include(v => v.Employee)
                .Include(v => v.VacationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationRequest == null)
            {
                return NotFound();
            }

            return View(vacationRequest);
        }

        // GET: VacationRequests/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.AspNetUsers, "Id", "Name");
            ViewData["VacationTypeId"] = new SelectList(_context.VacationTypes, "Id", "Name");
            return View();
        } 
        // GET: VacationRequests/Create
        public IActionResult Regular()
        {
            ViewData["EmployeeId"] = new SelectList(_context.AspNetUsers, "Id", "Name");
            ViewData["VacationTypeId"] = _context.VacationTypes.Where(p => p.Name ==clsEnum.VacationTypes.أجازة_اعتيادية.ToString().Replace("_"," ")).FirstOrDefault().Id;
            return View();
        }
        //public IActionResult Sick()
        //{
        //    ViewData["EmployeeId"] = new SelectList(_context.AspNetUsers, "Id", "Name");
        //    ViewData["VacationTypeId"] = _context.VacationTypes.Where(p => p.Name ==clsEnum.VacationTypes.أجازة_مرضية.ToString().Replace("_"," ")).FirstOrDefault().Id;
        //    return View();
        //}



        // GET: VacationRequests/Create
        public IActionResult Illness()
        {
            ViewData["EmployeeId"] = new SelectList(_context.AspNetUsers, "Id", "Name");
            ViewData["VacationTypeId"] = _context.VacationTypes.Where(p => p.Name == clsEnum.VacationTypes.أجازة_مرضية.ToString().Replace("_", " ")).FirstOrDefault().Id;
            return View();
        }


        // GET: VacationRequests/Create
        public IActionResult Death()
        {
            ViewData["EmployeeId"] = new SelectList(_context.AspNetUsers, "Id", "Name");
            ViewData["VacationTypeId"] = _context.VacationTypes.Where(p => p.Name == clsEnum.VacationTypes.أجازة_وفاة.ToString().Replace("_", " ")).FirstOrDefault().Id;
            return View();
        }



        // GET: VacationRequests/Create
        public IActionResult Patriarchy()
        {
            ViewData["EmployeeId"] = new SelectList(_context.AspNetUsers, "Id", "Name");
            ViewData["VacationTypeId"] = _context.VacationTypes.Where(p => p.Name == clsEnum.VacationTypes.أجازة_الأبوية.ToString().Replace("_", " ")).FirstOrDefault().Id;
            return View();
        }


        // GET: VacationRequests/Create
        public IActionResult Accompanyingapatient()
        {
            ViewData["EmployeeId"] = new SelectList(_context.AspNetUsers, "Id", "Name");
            ViewData["VacationTypeId"] = _context.VacationTypes.Where(p => p.Name == clsEnum.VacationTypes.أجازة_مرافقة_مريض.ToString().Replace("_", " ")).FirstOrDefault().Id;
            ViewData["HospitalTypes"] = clsEnum.HospitalTypes.حكومى;
            return View();
        }


        // GET: VacationRequests/MyVacations
        public async Task<IActionResult> MyVacations()
        {
            ViewData["EmployeeId"] = new SelectList(_context.AspNetUsers, "Id", "Name");
            
            var currentUser=await _userManager.Users.Where(a=>a.Email== User.Identity.Name).FirstOrDefaultAsync();
            var myVacationsList =await  _context.VacationRequests
                 
                .Include(v => v.VacationType)
                .Where(a => a.EmployeeId == currentUser.Id)
                 .AsNoTracking()
                .ToListAsync();
      
            return View(myVacationsList);
        }


        public async Task<IActionResult> GetRequestHistory(int id)
        {
            var data = await _context.EmployeeVacations
                .Where(a => a.VacationRequestID == id && a.EmployeeID == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Include(a => a.Employee)
                .Include(A => A.VacationRequest)
                  .ThenInclude(a => a.VacationType)
                 .Select(a => new
                 {
                     StartDate = a.VacationRequest.StartDate,
                     EndDate = a.VacationRequest.EndDate,
                     Status = a.Status.ToString(),
                     Date = a.Date.ToString("yyyy-MM-dd"),
                     Name = a.VacationRequest.VacationType.Name,
                     OffDaysCount=a.VacationRequest.OffDaysCount

                 })
                 .ToListAsync();

            data = data.Select(res => new
            {
              
                res.StartDate,
                res.EndDate,
                Status = Enum.GetName(typeof(clsEnum.VacationRequestStatus), int.Parse(res.Status)) ?? "Unknown",
                res.Date,

                res.Name,
                res.OffDaysCount
               
            }).ToList();

            if (data.Count > 0)
                return Json(new { success = true, data=data });

            return Json(new { success = false});
        }


        [HttpPost]

        public async Task<JsonResult>  Cancel(int id,string? Reason)
        {
            var vacation = await _context.VacationRequests.FindAsync(id);
            if(vacation is null)
            {
                return Json(new { success = false });
            }
                
            else
            {
                vacation.CancellactionReason = Reason;
                vacation.Status = (int)clsEnum.VacationRequestStatus.ملغى;
                await _context.SaveChangesAsync();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var EmpVacation = new EmployeeVacations
                {
                    EmployeeID = userId,
                    Status = (int)clsEnum.VacationRequestStatus.ملغى,
                    VacationRequestID = id,
                    CancellationReason=Reason,

                };
                _context.Add(EmpVacation);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "تم الغاء الاجازة بنجاح" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacationRequest vacationRequest, IFormFile? ReportUrl)
        {
            
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(vacationRequest.VacationTypeId== _context.VacationTypes.Where(p => p.Name == clsEnum.VacationTypes.أجازة_مرضية.ToString().Replace("_", " ")).FirstOrDefault().Id && vacationRequest.HospitalType == null)
                {
                    return Json(new { success = false, message = "يجب اختيار نوع المستشفى" });
                }
                if (string.IsNullOrEmpty(userId))
                {
                    // Handle the case where the user is not logged in
                    return Json(new { success = false, message = "User is not authenticated." });
                }
                vacationRequest.EmployeeId = userId;
            vacationRequest.Status = (int)clsEnum.VacationRequestStatus.جديد;

                    if (ReportUrl != null)
                    {
                        string reportUrl = await _viewModel.UploadAttachment(ReportUrl, "wwwroot/Uploads/Vacations");
                        if (!string.IsNullOrEmpty(reportUrl))
                            vacationRequest.ReportUrl = reportUrl;
                    }

                    _context.Add(vacationRequest);
                    await _context.SaveChangesAsync();
            var EmpVacation = new EmployeeVacations
            {
                EmployeeID = userId,
                Status = (int)clsEnum.VacationRequestStatus.جديد,
                VacationRequestID = vacationRequest.Id

            };
            _context.Add(EmpVacation);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم حفظ البيانات بنجاح" });
                }


        // GET: VacationRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationRequest = await _context.VacationRequests.FindAsync(id);
            if (vacationRequest == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.AspNetUsers, "Id", "Name", vacationRequest.EmployeeId);
            ViewData["VacationTypeId"] = new SelectList(_context.VacationTypes, "Id", "Name", vacationRequest.VacationTypeId);

            return View(vacationRequest);
        }

        // POST: VacationRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VacationRequest vacationRequest,string? cReportUrl, IFormFile  ? NewReportUrl)
        {
            if (id != vacationRequest.Id)
            {
                return NotFound();
            }

           
                try
                {
                if (cReportUrl is null && NewReportUrl is null && vacationRequest.VacationTypeId!= _context.VacationTypes.Where(p => p.Name == clsEnum.VacationTypes.أجازة_اعتيادية.ToString().Replace("_", " ")).FirstOrDefault().Id)
                {


                    return Json(new { success = false, message = "يجب ارفاق التقرير" });
                }

                 else if (NewReportUrl != null)
                {
                    string reportUrl = await _viewModel.UploadImageFile(NewReportUrl, "wwwroot/Uploads/Vacations");
                    if (!string.IsNullOrEmpty(reportUrl))
                        vacationRequest.ReportUrl = reportUrl;

                }
                else if(cReportUrl is not null)
                {
                    vacationRequest.ReportUrl = cReportUrl;
                }
                //if (vacationRequest.VacationTypeId == _context.VacationTypes.Where(p => p.Name == clsEnum.VacationTypes.أجازة_مرضية.ToString().Replace("_", " ")).FirstOrDefault().Id && vacationRequest.HospitalType == null)
                //{
                //    return Json(new { success = false, message = "يجب اختيار نوع المستشفى" });
                //}
                //if (vacationRequest.VacationTypeId == _context.VacationTypes.Where(p => p.Name == clsEnum.VacationTypes.أجازة_مرضية.ToString().Replace("_", " ")).FirstOrDefault().Id && vacationRequest.VacationNoOnList == null)
                //{
                //    return Json(new { success = false, message = "يجب ادخال رقم الاجازة بمنصة صحتى" });
                //}
                _context.Update(vacationRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacationRequestExists(vacationRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true, message = "تم حفظ البيانات بنجاح" });


          
        }

        // GET: VacationRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationRequest = await _context.VacationRequests
                
                .Include(v => v.Employee)
                .Include(v => v.VacationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationRequest == null)
            {
                return NotFound();
            }

            return View(vacationRequest);
        }

        // POST: VacationRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacationRequest = await _context.VacationRequests.FindAsync(id);
            if (vacationRequest != null)
            {
                _context.VacationRequests.Remove(vacationRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> RemoveByAjax(int id)
        {

            return await clsCustomDelete<VacationRequest>.RemoveByAjax(id, _context.VacationRequests, _context);

        }
        private bool VacationRequestExists(int id)
        {
            return _context.VacationRequests.Any(e => e.Id == id);
        }
    }
}
