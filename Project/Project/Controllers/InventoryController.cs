using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.InventoryItems.ToListAsync());
        }

        public async Task<IActionResult> Search(string text)
        {
            var items =  _context.InventoryItems.Where(p => p.Name.Contains(text) || p.Description.Contains(text)
            || p.Category.Name.Contains(text));
            return View(await items.ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryItem item)
        {
            await _context.InventoryItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _context.InventoryItems.SingleOrDefaultAsync(d => d.Id == id));
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(await _context.InventoryItems.SingleOrDefaultAsync(i => i.Id == id));
        }

        public IActionResult WriteReview(int id)
        {
            return View();
        }
    }
}