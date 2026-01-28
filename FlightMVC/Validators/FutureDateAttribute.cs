using System;
using System.ComponentModel.DataAnnotations;

namespace FlightMVC.Validators
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(ErrorMessage);

            DateTime date = (DateTime)value;

            if (date.Date < DateTime.Today)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
