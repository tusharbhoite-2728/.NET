using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityManagement.Controllers
{
    [ApiController]
    [Route("api/role-claims")]
    public class RoleClaimsController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleClaimsController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Add(string roleId, string type, string value)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            await _roleManager.AddClaimAsync(role, new Claim(type, value));
            return Ok();
        }
    }
}
