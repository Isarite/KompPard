﻿using System;
using System.Linq;
using System.Security.Claims;
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
            //Viewbag.DiscountId =
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryItem item)
        {
            await _context.InventoryItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateDiscount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount(Discount item)
        {
            await _context.Discounts.AddAsync(item);
            await _context.SaveChangesAsync();
            return View(nameof(ListDiscounts));
        }

        [HttpPost]
        public async Task<IActionResult> WriteReview(Feedback review)
        {
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            review.UserId = identity;
            await _context.Feedback.AddAsync(review);
            await _context.SaveChangesAsync();
            var item = await _context.InventoryItems.SingleOrDefaultAsync(d => d.Id == review.ItemId);
            var stars = _context.Feedback.Where(c => c.ItemId == item.Id).Average(d => d.Rating);
            item.Rating = stars;           
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _context.InventoryItems.SingleOrDefaultAsync(d => d.Id == id));
        }

        public async Task<IActionResult> DiscountDetails(int id)
        {
            return View(await _context.Discounts.SingleOrDefaultAsync(d => d.Id == id));
        }

        public async Task<IActionResult> ListDiscounts() {
       
                return View(await _context.Discounts.ToListAsync());
        }


        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.InventoryItems.SingleOrDefaultAsync(i => i.Id == id);
            return View(item);
        }

        public IActionResult WriteReview(int id)
        {
            //Gets user ID
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //checks if user has bought that item before
            var Carts = _context.Carts.Where(c => c.UserId.Equals(identity) && c.IsFinal).Select(c => c.Id);
            var boughtItems = _context.OrderedInventoryItems.Where(c => Carts.Contains(c.CartId)).Select(c => c.ItemId);
            ViewBag.BoughtItem = boughtItems.Contains(id);
            return View();
        }
    }
}