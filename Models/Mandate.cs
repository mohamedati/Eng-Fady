using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP.Models
{
    public class Mandate
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public TaskName Tasks { get; set; }
        public int? CountryId { get; set; }
        public Country Countries { get; set; }
        public int? EntitlementTypeId { get; set; }
        public EntitlementType EntitlementTypes { get; set; }
        public string TaskDescription { get; set; }
        public string City { get; set; }
        public string MandateDestination { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int OffDaysCount { get; set; }
        public string? ReportUrl { get; set; }

    }
}
