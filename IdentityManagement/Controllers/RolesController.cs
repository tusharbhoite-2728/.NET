using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManagement.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            await _roleManager.CreateAsync(new IdentityRole(name));
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            role.Name = name;
            await _roleManager.UpdateAsync(role);
            return Ok();
        }
    }
}
