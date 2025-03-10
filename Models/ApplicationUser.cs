using Microsoft.AspNetCore.Identity;

namespace ERP.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string Name { get; set; }
        public required string Password { get; set; }


    }
   
}
