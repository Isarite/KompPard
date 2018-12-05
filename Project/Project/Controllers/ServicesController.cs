using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManger;

        public ServicesController(ApplicationDbContext context, UserManager<ApplicationUser> userManger)
        {
            _context = context;
            _userManger = userManger;
        }

        // GET: ServiceItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceItems.Where(n => n.Name != "Warranty").ToListAsync());
        }

        // GET: ServiceItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceItem = await _context.ServiceItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceItem == null)
            {
                return NotFound();
            }

            return View(serviceItem);
        }

        // GET: ServiceItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,HourDuration,HourPrice")] ServiceItem serviceItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceItem);
        }

        // GET: ServiceItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceItem = await _context.ServiceItems.FindAsync(id);
            if (serviceItem == null)
            {
                return NotFound();
            }
            return View(serviceItem);
        }

        // POST: ServiceItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,HourDuration,HourPrice")] ServiceItem serviceItem)
        {
            if (id != serviceItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceItemExists(serviceItem.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(serviceItem);
        }

        // GET: ServiceItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceItem = await _context.ServiceItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceItem == null)
            {
                return NotFound();
            }

            return View(serviceItem);
        }

        // POST: ServiceItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceItem = await _context.ServiceItems.FindAsync(id);
            _context.ServiceItems.Remove(serviceItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceItemExists(int id)
        {
            return _context.ServiceItems.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ActiveServices()
        {
            var user = await _userManger.GetUserAsync(User);
            var services = _context.OrderedServiceItems.Include(c => c.Cart).Include(c => c.ServiceItem);
            var s1 = services
                .Where(si => si.Cart.UserId == user.Id);
            var s2 = s1.Where(si => si.EndDate > DateTime.Now && si.StartDate < DateTime.Now);
            return View(s2);
        }
    }
}
