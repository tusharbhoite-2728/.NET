using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RemoteValidationDemo.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Remote(
            action: "IsEmailAvailable",
            controller: "Users",
            ErrorMessage = "Email already exists"
        )]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
