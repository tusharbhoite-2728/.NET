using System.ComponentModel.DataAnnotations;

namespace IdentityMVCApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username or Email is required.")]
        [Display(Name = "Username or Email")]
        [StringLength(100, ErrorMessage = "Username/Email is too long.")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; } = true;

        public string? ReturnUrl { get; set; }
    }
}
