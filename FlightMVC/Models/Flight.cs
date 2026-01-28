
using FlightMVC.Models;
using FlightMVC.Validators;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FlightMVC.Models
{
    public class Flight
    {
        public int FlightId { get; set; }



        
        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "FlightNumber must contain only uppercase letters and numbers")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "FlightNumber must be between 3 and 4 characters")]
        [Required(ErrorMessage = "FlightNumber is required")]
        public string FlightNumber { get; set; }

        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "AirlineId must contain exactly 2 capital letters")]
        [Required(ErrorMessage = "AirlineId is required")]
        [StringLength(2, MinimumLength = 2)]
        public string AirlineId { get; set; }

        [Required(ErrorMessage = "Date of travel is required")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Date of travel cannot be in the past")]
        public DateTime DateOfTravel { get; set; }

        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Source must contain only letters")]
        [StringLength(10, ErrorMessage = "Source cannot be longer than 10 characters")]
        [Required(ErrorMessage = "Source is required")]
        public string Source { get; set; }

        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Destination must contain only letters")]
        [StringLength(10, ErrorMessage = "Destination cannot be longer than 10 characters")]
        [Required(ErrorMessage = "Destination is required")]
        public string Destination { get; set; }

        [Required(ErrorMessage = "IntDom is required")]
        public string IntDom { get; set; }  // International or Domestic

        [Required(ErrorMessage = "Aircraft is required")]
        public string Aircraft { get; set; }
    }
}






