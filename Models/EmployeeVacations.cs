using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class EmployeeVacations
    {
        public int ID {  get; set; }

        public string EmployeeID { get; set; }

        [ForeignKey(nameof(EmployeeID))]
        public AspNetUser Employee { get; set; }

        public int VacationRequestID { get; set; }

        [ForeignKey(nameof(VacationRequestID))]
        public VacationRequest VacationRequest { get; set; }

        public DateTime Date { get; set; }

        public byte? Status {  get; set; }

        public string? CancellationReason {  get; set; }


    }
}
