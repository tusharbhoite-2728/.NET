using System.ComponentModel.DataAnnotations;

namespace IdentityMVCApp.ViewModels
{
    public class AdminCreateUserViewModel
    {
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

        
        [Required(ErrorMessage = "Age is required.")]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
        [Display(Name = "Age")]
        public int age { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [StringLength(100, ErrorMessage = "Password is too long.")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirm password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
