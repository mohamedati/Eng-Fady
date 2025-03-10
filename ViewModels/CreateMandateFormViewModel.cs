using ERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP.ViewModels
{
    public class CreateMandateFormViewModel
    {
        public int TaskId { get; set; }
        public IEnumerable<SelectListItem> Tasks { get; set; }
        public int? CountryId { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        public int? EntitlementTypeId { get; set; }
        public IEnumerable<SelectListItem> EntitlementTypes { get; set; }
        public string TaskDescription { get; set; }
        public string City { get; set; }
        public string MandateDestination { get; set; }
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int OffDaysCount { get; set; }
        public string? ReportUrl { get; set; }

    }
}
