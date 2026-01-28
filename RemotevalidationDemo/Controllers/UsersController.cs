using Microsoft.AspNetCore.Mvc;
using RemoteValidationDemo.Models;
using System.Collections.Generic;
using System.Linq;

namespace RemoteValidationDemo.Controllers
{
    public class UsersController : Controller
    {
    
        private static List<string> ExistingEmails = new()
        {
            "tushar@gmail.com",
            "admin@example.com"
        };

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Success");
            }

            return View(user);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsEmailAvailable(string email)
        {
            bool emailExists = ExistingEmails
                .Any(e => e.ToLower() == email.ToLower());

            if (emailExists)
            {
                return Json(false);
            }

            return Json(true);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
