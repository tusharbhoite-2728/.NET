using IdentityMVCApp.Models;
using IdentityMVCApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMVCApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /AdminUsers
        [HttpGet]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // GET: /AdminUsers/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new AdminCreateUserViewModel());
        }

        // POST: /AdminUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastError"] = "Please fix validation errors.";
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                age = model.age
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);

                TempData["ToastError"] = "Failed to create user.";
                return View(model);
            }

            // Keep your existing behavior
            await _userManager.AddToRoleAsync(user, "User");

            TempData["ToastSuccess"] = "User created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminUsers/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var vm = new AdminEditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                FirstName = user.FirstName,
                LastName = user.LastName,
                age = user.age
            };

            return View(vm);
        }

        // POST: /AdminUsers/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminEditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastError"] = "Please fix validation errors.";
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.age = model.age;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);

                TempData["ToastError"] = "Failed to update user.";
                return View(model);
            }

            TempData["ToastSuccess"] = "User updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ---------------- ROLE MANAGEMENT (NEW) ----------------

        // GET: /AdminUsers/ManageRoles/{id}
        [HttpGet]
        public async Task<IActionResult> ManageRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // ❌ prevent admin changing his own roles
            var currentUserId = _userManager.GetUserId(User);
            if (user.Id == currentUserId)
            {
                TempData["ToastWarning"] = "You cannot change your own role/privileges.";
                return RedirectToAction(nameof(Index));
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name!).ToList();

            var vm = new AdminManageUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName ?? user.Email ?? user.Id,
                Roles = allRoles.Select(role => new RoleSelection
                {
                    RoleName = role,
                    IsSelected = userRoles.Contains(role)
                }).ToList()
            };

            return View(vm);
        }

        // POST: /AdminUsers/ManageRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(AdminManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            // Prevent admin editing himself
            var currentUserId = _userManager.GetUserId(User);
            if (user.Id == currentUserId)
            {
                TempData["ToastError"] = "You cannot change your own role.";
                return RedirectToAction(nameof(Index));
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove all current roles
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add selected role
            if (!string.IsNullOrEmpty(model.SelectedRole) &&
                await _roleManager.RoleExistsAsync(model.SelectedRole))
            {
                await _userManager.AddToRoleAsync(user, model.SelectedRole);
            }

            TempData["ToastSuccess"] = "User role updated successfully.";
            return RedirectToAction(nameof(Index));
        }

    }
}
