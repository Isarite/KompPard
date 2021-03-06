﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            if(!User.IsInRole("Manager"))
                RedirectToAction(nameof(Index));
            await _context.InventoryItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateDiscount()
        {
            if (!User.IsInRole("Manager"))
                RedirectToAction(nameof(Index));
            return View();
        }

        public IActionResult CreateCategory()
        {
            if (!User.IsInRole("Manager"))
                RedirectToAction(nameof(Index));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount(Discount item)
        {
            if (!User.IsInRole("Manager"))
                RedirectToAction(nameof(Index));
            await _context.Discounts.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListDiscounts));
        }

        [HttpPost]
        public async Task<IActionResult> AssignDiscounts(int discountId, IEnumerable<int> confirmation)
        {
            if (!User.IsInRole("Manager"))
                RedirectToAction(nameof(Index));
            var items = _context.InventoryItems.Where(c => confirmation.Contains(c.Id));
            foreach(var item in items)
            {
                item.DiscountId = discountId;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListDiscounts));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(InventoryItemCategory item)
        {
            if (!User.IsInRole("Manager"))
                RedirectToAction(nameof(Index));
            await _context.InventoryItemCategories.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListCategories));
        }

        [HttpPost]
        public async Task<IActionResult> WriteReview(int id, Feedback review)
        {
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            review.UserId = identity;
            review.ItemId = id;
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
            //Gets user ID
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //checks if user has bought that item before
            var carts = _context.Carts.Where(c => c.UserId.Equals(identity) && c.IsFinal).Select(c => c.Id);
            var boughtItems = _context.OrderedInventoryItems.Where(c => carts.Contains(c.CartId)).Select(c => c.ItemId);
            ViewBag.BoughtItem = boughtItems.Contains(id);
            var item = await _context.InventoryItems.SingleOrDefaultAsync(d => d.Id == id);
            var discount = await _context.Discounts.SingleOrDefaultAsync(d => d.Id == item.DiscountId);
            if(discount == null)
            {
                ViewBag.DiscountAmount = 0;
            }
            else
            {
                ViewBag.DiscountAmount = discount.Amount;
            }

            return View(item);
        }

        public async Task<IActionResult> DiscountDetails(int id)
        {
            return View(await _context.Discounts.SingleOrDefaultAsync(d => d.Id == id));
        }

        public async Task<IActionResult> ListDiscounts() {
            if (!User.IsInRole("Manager"))
                RedirectToAction(nameof(Index));
            return View(await _context.Discounts.ToListAsync());
        }

        public async Task<IActionResult> AssignDiscounts(int id)
        {
            if (!User.IsInRole("Manager"))
                RedirectToAction(nameof(Index));
            ViewBag.DiscountId = id;
            ViewBag.index = 0;
            var items = await _context.InventoryItems.ToListAsync();
            ViewBag.Discounts = new List<bool>();
            foreach(var item in items)
            {
                ViewBag.Discounts.Add(id == item.DiscountId);
            }
            return View(items);
        }

        public async Task<IActionResult> ListCategories()
        {
            //if (!User.IsInRole("Manager"))
            //    RedirectToAction(nameof(Index));

            return View(await _context.InventoryItemCategories.ToListAsync());
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!User.IsInRole("Manager"))
                RedirectToAction(nameof(Index));
            var item = await _context.InventoryItems.SingleOrDefaultAsync(i => i.Id == id);
            //ViewBag.DiscountId = await _context.Discounts.SingleOrDefaultAsync(d => d.Id == item.DiscountId);
            var list = _context.Discounts.ToList();
            ViewBag.DiscountId = new List<SelectListItem>();
            foreach (var discount in list)
            {
                ViewBag.DiscountId.Add(new SelectListItem { Value = discount.Id.ToString(), Text = discount.Description + discount.Amount });
            }

            var catList = _context.InventoryItemCategories.ToList();
            ViewBag.CategoryId = new List<SelectListItem>();
            foreach (var cat in catList)
            {
                ViewBag.CategoryId.Add(new SelectListItem { Value = cat.Id.ToString(), Text = cat.Name });
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, string Name, decimal Price, string Description, string ImgPath, int Stock, int DiscountId, int CategoryId)
        {
            if (!User.IsInRole("Manager"))
                RedirectToAction(nameof(Index));
            var item = await _context.InventoryItems.SingleOrDefaultAsync(i => i.Id == Id);
            item.Name = Name;
            item.Price = Price;
            item.Description = Description;
            item.ImgPath = ImgPath;
            item.Stock = Stock;
            item.CategoryId = CategoryId;
            //var connection = new MM_InventoryItem_Category();
            //connection.InventoryItemId = Id;
            //connection.CategoryId = CategoryId;
            //var toDelete = await _context.MmInventoryItemCategories.SingleOrDefaultAsync(i => i.InventoryItemId == Id);
            //_context.MmInventoryItemCategories.Remove(toDelete);
            //_context.MmInventoryItemCategories.Add(connection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult WriteReview(int id)
        {
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //checks if user has bought that item before
            var Carts = _context.Carts.Where(c => c.UserId.Equals(identity) && c.IsFinal).Select(c => c.Id);
            var boughtItems = _context.OrderedInventoryItems.Where(c => Carts.Contains(c.CartId)).Select(c => c.ItemId);
            if(!boughtItems.Contains(id))
                return RedirectToAction(nameof(Index));
            ViewBag.Id = id;
            ViewBag.UserId = identity;
            return View();
        }
    }
}