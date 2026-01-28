using Microsoft.AspNetCore.Mvc;
using FlightMVC.Data;
using FlightMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightMVC.Controllers
{
    public class FlightController : Controller
    {
        private readonly AppDbContext _context;


        public FlightController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var flights = _context.Flights.ToList();
            return Json(flights);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var flight = _context.Flights.Find(id);
            if (flight == null) return NotFound();
            return Json(flight);
        }

        [HttpPost]
        public IActionResult Create(Flight flight)
        {
            // Check server-side validation
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return BadRequest(errors); // return JSON errors
            }

            {
                bool ISExist = _context.Flights.Any(f => f.FlightNumber == flight.FlightNumber && f.AirlineId == flight.AirlineId && f.DateOfTravel == flight.DateOfTravel );
                if (ISExist)
                {
                    return BadRequest("Flight already exists");
                }
            }

            _context.Flights.Add(flight);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult Edit(Flight flight)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return BadRequest(errors); // return JSON errors
            }


            bool ISExist = _context.Flights.Any(f => f.FlightNumber == flight.FlightNumber && f.AirlineId == flight.AirlineId && f.DateOfTravel == flight.DateOfTravel);
            if (ISExist)
            {
                return BadRequest("Flight already exists");
            }

            _context.Flights.Update(flight);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var flight = _context.Flights.Find(id);
            if (flight == null) return NotFound();

            _context.Flights.Remove(flight);
            _context.SaveChanges();
            return Ok();
        }

        
    }
}
