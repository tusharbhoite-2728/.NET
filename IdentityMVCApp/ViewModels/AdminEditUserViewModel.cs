using System.ComponentModel.DataAnnotations;

namespace IdentityMVCApp.ViewModels
{
    public class AdminEditUserViewModel
    {
        [Required(ErrorMessage = "User Id is required.")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Only letters, digits, . _ - are allowed.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(100, ErrorMessage = "Email is too long.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters and spaces allowed.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters and spaces allowed.")]
        public string LastName { get; set; } = string.Empty;

        // keep 'age' to match your ApplicationUser.age
        [Required(ErrorMessage = "Age is required.")]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
        [Display(Name = "Age")]
        public int age { get; set; }
    }
}
