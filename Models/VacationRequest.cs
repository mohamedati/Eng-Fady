using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models;

public partial class VacationRequest
{
    public int Id { get; set; }

    public string EmployeeId { get; set; } = null!;

    public int VacationTypeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int OffDaysCount { get; set; }
    public string? ReportUrl { get; set; }

    public string? VacationNoOnList { get; set; }

    public string? HospitalType { get; set; }

    public DateTime CreationDate { get; set; }

    public byte? Status { get; set; }

    public string? CancellactionReason { get; set; }


    public string? Comment { get; set; }
    public virtual AspNetUser Employee { get; set; } = null!;

    public virtual VacationType VacationType { get; set; } = null!;

    public virtual ICollection<EmployeeVacations> EmployeeVacations { get; set; } = new List<EmployeeVacations>();

}
