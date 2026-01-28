using System.ComponentModel.DataAnnotations;

namespace Domain.VMModels
{
    public class VMMaster
    {
        public int AirlineMasterID { get; set; }

        [Required(ErrorMessage = "Airline code is required.")]
        [MaxLength(10)]
        [RegularExpression(@"^\d[A-Z]$", ErrorMessage = "Airline code must be a digit followed by an uppercase letter.")]
        public string AirlineCode { get; set; }

        [Required(ErrorMessage = "Airline name is required.")]
        [MaxLength(15, ErrorMessage = "Airline name can have maximum 15 characters.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Airline name must contain only letters.")]
        public string AirlineName { get; set; }

        [Required(ErrorMessage = "LPC is required.")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "LPC must be exactly 4 digits.")]
        public string LPC { get; set; }

        [Required(ErrorMessage = "Operational status is required.")]
        [RegularExpression(@"^(Yes|No)$", ErrorMessage = "IsOperational must be either 'Yes' or 'No'.")]
        public string IsOperational { get; set; } // Yes / No

        public DateTime LastChangedAt { get; set; } = DateTime.Now;
    }
}