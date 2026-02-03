using IdentityMVCApp.Models;
using IdentityMVCApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityMVCApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                age = model.Age
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }


        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.UserNameOrEmail)
                       ?? await _userManager.FindByEmailAsync(model.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                // 🔴 THIS IS THE KEY PART
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                // Default redirect after login
                return RedirectToAction("Index", "Dashboard");

            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account is locked.");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }


        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        // GET: /Account/GoogleLogin
        [HttpGet]
        public IActionResult GoogleLogin(string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Account", new { returnUrl });
            var props = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(props, "Google");
        }

        // GET: /Account/GoogleResponse
        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login");

            // If already linked, sign in directly
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: true,
                bypassTwoFactor: true);

            if (signInResult.Succeeded)
                return LocalRedirect(returnUrl);

            // Not linked yet -> create user / link user
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Google did not provide an email.";
                return RedirectToAction("Login");
            }

            // 1) If user exists by email, link login
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                var linkResult = await _userManager.AddLoginAsync(existingUser, info);
                if (linkResult.Succeeded)
                {
                    await _signInManager.SignInAsync(existingUser, isPersistent: true);
                    return LocalRedirect(returnUrl);
                }

                foreach (var err in linkResult.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);

                return View("Login", new LoginViewModel { ReturnUrl = returnUrl });
            }

            // 2) Create new user from Google data
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "";
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "";

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName,
                age = 18 // default value (since Google won’t send Age)
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                foreach (var err in createResult.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);

                return View("Login", new LoginViewModel { ReturnUrl = returnUrl });
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                foreach (var err in addLoginResult.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);

                return View("Login", new LoginViewModel { ReturnUrl = returnUrl });
            }

            // Optional: default role
            await _userManager.AddToRoleAsync(user, "User");

            await _signInManager.SignInAsync(user, isPersistent: true);
            return LocalRedirect(returnUrl);
        }

    }
}
