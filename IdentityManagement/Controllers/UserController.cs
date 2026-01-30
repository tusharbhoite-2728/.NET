using IdentityManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManagement.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, string email)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.Email = email;
            user.UserName = email;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [Authorize(Policy = "UserEdit")]
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            return Ok("Access granted");
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(
    string userId, string role,
    [FromServices] UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.AddToRoleAsync(user, role);
            return Ok();
        }


    }

}
