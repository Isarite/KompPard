using System;
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

        public async Task<IActionResult> Index(string text)
        {
            if (string.IsNullOrEmpty(text))
                return View(await _context.InventoryItems.ToListAsync());
            var items = _context.InventoryItems.Where(p => p.Name.Contains(text) || p.Description.Contains(text) || p.Category.Name.Contains(text));
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

        [HttpPost]
        public async Task<IActionResult> CreateDiscount(Discount item)
        {
            await _context.Discounts.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListDiscounts));
        }

        [HttpPost]
        public async Task<IActionResult> WriteReview(Feedback review)
        {
            await _context.Feedback.AddAsync(review);
            await _context.SaveChangesAsync();
            var item = await _context.InventoryItems.SingleOrDefaultAsync(d => d.Id == review.ItemId);
            var stars = _context.Feedback.Where(c => c.ItemId == item.Id).Average(d => d.Rating);
            item.Rating = stars;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListDiscounts));
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _context.InventoryItems.SingleOrDefaultAsync(d => d.Id == id));
        }

        public async Task<IActionResult> DiscountDetails(int id)
        {
            return View(await _context.Discounts.SingleOrDefaultAsync(d => d.Id == id));
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