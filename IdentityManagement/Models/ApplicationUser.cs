using Microsoft.AspNetCore.Identity;

namespace IdentityManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
