﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.ViewModels;

namespace Project.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CheckoutController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var cart = await _context.Carts
                .Include(c => c.OrderedInventoryItems)
                .ThenInclude(c => c.InventoryItem)
                .Include(c => c.OrderedServiceItems)
                .SingleOrDefaultAsync(c => c.UserId == user.Id && !c.IsFinal);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddInventory(int id, string comment, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);

            var cart = await _context.Carts.SingleOrDefaultAsync(c => c.UserId == user.Id && !c.IsFinal);
            if (cart == null)
            {
                cart = new Cart { Id = Guid.NewGuid(), IsFinal = false, LastEditDate = DateTime.Now, UserId = user.Id };
                await _context.Carts.AddAsync(cart);
            }

            var item = await _context.OrderedInventoryItems.SingleOrDefaultAsync(s => s.ItemId == id);
            if (item == null)
                await _context.OrderedInventoryItems.AddAsync(new OrderedInventoryItem
                {
                    CartId = cart.Id,
                    Comment = comment,
                    ItemId = id,
                    Quantity = quantity
                });
            else
                item.Quantity += quantity;

            cart.TotalValue = await _context.OrderedInventoryItems.Include(o => o.InventoryItem)
                .Where(t => t.CartId == cart.Id).SumAsync(s => s.InventoryItem.Price * s.Quantity);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Inventory");
        }

        public async Task<IActionResult> Continue()
        {
            var user = await _userManager.GetUserAsync(User);

            var cart = await _context.Carts
                .Include(c => c.OrderedInventoryItems)
                .ThenInclude(i => i.InventoryItem)
                .SingleOrDefaultAsync(c => c.UserId == user.Id && !c.IsFinal);

            return View(new Invoice { Cart = cart });
        }

        public async Task<IActionResult> History()
        {
            var uid = _userManager.GetUserId(User);
            return View(await _context.Invoices
                .Include(i => i.Cart)
                .ThenInclude(c => c.OrderedInventoryItems)
                .ThenInclude(oii => oii.InventoryItem)
                .Include(i => i.Cart)
                .ThenInclude(c => c.OrderedServiceItems)
                .ThenInclude(osi => osi.ServiceItem)
                .Where(c => c.Cart.UserId == uid).ToListAsync());
        }
    }
}