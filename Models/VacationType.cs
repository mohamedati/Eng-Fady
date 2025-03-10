using System;
using System.Collections.Generic;

namespace ERP.Models;

public partial class VacationType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Balance { get; set; }

    public DateTime CreationDate { get; set; }

    public string CreatorId { get; set; } = null!;

    public virtual AspNetUser Creator { get; set; } = null!;

    public virtual ICollection<VacationRequest> VacationRequests { get; set; } = new List<VacationRequest>();
}
