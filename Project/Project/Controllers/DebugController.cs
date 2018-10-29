using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class DebugController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DebugController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GiveRole(string role)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            if (!await _userManager.IsInRoleAsync(user, role))
                await _userManager.AddToRoleAsync(user, role);
            
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> TakeRole(string role)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            if (await _userManager.IsInRoleAsync(user, role))
                await _userManager.RemoveFromRoleAsync(user, role);

            return RedirectToAction("Index", "Home");
        }
    }
}