namespace ERP.Models
{
    public class EntitlementType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string CreatorId { get; set; } = null!;

        public virtual AspNetUser Creator { get; set; } = null!;
        public DateTime CreationTime { get; set; }
    }
}
