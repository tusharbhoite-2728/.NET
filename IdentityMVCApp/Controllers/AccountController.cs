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

        // ---------------- REGISTER ----------------

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastError"] = "Please fix the validation errors.";
                return View(model);
            }

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

                TempData["ToastSuccess"] = "Registration successful!";
                return RedirectToAction("Index", "Home"); // unchanged
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            TempData["ToastError"] = "Registration failed.";
            return View(model);
        }

        // ---------------- LOGIN ----------------

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastError"] = "Please enter valid credentials.";
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserNameOrEmail)
                       ?? await _userManager.FindByEmailAsync(model.UserNameOrEmail);

            if (user == null)
            {
                TempData["ToastError"] = "Invalid username/email or password.";
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
                TempData["ToastSuccess"] = "Login successful!";

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Dashboard"); // unchanged
            }

            if (result.IsLockedOut)
            {
                TempData["ToastError"] = "Account locked due to multiple failed attempts.";
                return View(model);
            }

            TempData["ToastError"] = "Invalid username/email or password.";
            return View(model);
        }

        // ---------------- LOGOUT ----------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["ToastInfo"] = "You have been logged out.";
            return RedirectToAction("Login", "Account");
        }

        // ---------------- GOOGLE LOGIN ----------------

        [HttpGet]
        public IActionResult GoogleLogin(string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Account", new { returnUrl });
            var props = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(props, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["ToastError"] = "Google login failed.";
                return RedirectToAction("Login");
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: true,
                bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                TempData["ToastSuccess"] = "Logged in with Google.";
                return LocalRedirect(returnUrl);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["ToastError"] = "Google account email not found.";
                return RedirectToAction("Login");
            }

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                var linkResult = await _userManager.AddLoginAsync(existingUser, info);
                if (linkResult.Succeeded)
                {
                    await _signInManager.SignInAsync(existingUser, isPersistent: true);
                    TempData["ToastSuccess"] = "Google account linked successfully.";
                    return LocalRedirect(returnUrl);
                }

                TempData["ToastError"] = "Failed to link Google account.";
                return View("Login", new LoginViewModel { ReturnUrl = returnUrl });
            }

            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "";
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "";

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName,
                age = 18
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                TempData["ToastError"] = "Failed to create account from Google login.";
                return View("Login", new LoginViewModel { ReturnUrl = returnUrl });
            }

            await _userManager.AddLoginAsync(user, info);
            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, isPersistent: true);

            TempData["ToastSuccess"] = "Account created using Google.";
            return LocalRedirect(returnUrl);
        }
    }
}
