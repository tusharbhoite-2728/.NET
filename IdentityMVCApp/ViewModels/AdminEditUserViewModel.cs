using System.ComponentModel.DataAnnotations;

namespace IdentityMVCApp.ViewModels
{
    public class AdminEditUserViewModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Range(1, 120)]
        public int age { get; set; }
    }
}
