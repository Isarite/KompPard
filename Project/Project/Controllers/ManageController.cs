using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ManageController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userManager.GetUserAsync(User));
        }

        public async Task<IActionResult> Manage(string email)
        {
            if (email == null)
                return View(await _userManager.GetUserAsync(User));
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (await IsAuthorized(await _userManager.GetUserAsync(User), user))
                return View(user);

            // No access
            throw new NotImplementedException();
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> All(string email)
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        private async Task<bool> IsAuthorized(ApplicationUser accessor, ApplicationUser accessee)
        {
            //       User accessing himself          Manager accessing anyone
            return accessor.Id == accessee.Id || await _userManager.IsInRoleAsync(accessor, "Manager");
        }
    }
}