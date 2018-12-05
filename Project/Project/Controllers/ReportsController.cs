using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reports.Include(r => r.Creator);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }
            
            //priklausomai nuo to, koks ataskaitos tipas, reikai suformuoti teksto eilutę su kažkokia "ataskaitos" informacija
            if(report.Type.ToString() == "Clients")
            {
                foreach (var item in _context.Users)
                {
                    if (item.RegisterDate < report.EndTime && item.RegisterDate > report.StartTime)
                    {
                        report.ContentsHtml.Insert(report.ContentsHtml.Length, item.UserName + " " + item.Email + " " + item.RegisterDate + " \n");
                    }
                }
                if(report.ContentsHtml.Length < 10)
                {
                    report.ContentsHtml = "The selected time period does not have enough data to create a report, try to select a wider time period \n";
                }
            }
            if (report.Type.ToString() == "Profits")
            {
                decimal sum = 0;
                foreach (var item in _context.Carts)
                {
                    if (item.IsFinal && item.LastEditDate > report.StartTime && item.LastEditDate < report.EndTime)
                    {
                        sum += item.TotalValue;
                    }
                }
                report.ContentsHtml = "Total profits during selected time period: " + sum + " € \n";
                if (sum == 0)
                {
                    report.ContentsHtml = "No profits were made during the selected time period \n";
                }
            }
            if (report.Type.ToString() == "MostPopularServices")
            {
                int[] serv = new int[100];//Nežinau, iki kiek eina serviceid
                string[] names = new string[100];
                for (int i = 0; i < serv.Length; i++)
                { serv[i] = 0; }
                int maxid = 0;
                int maxnum = 0;
                string maxname = " ";

                foreach (var item in _context.OrderedServiceItems)
                {
                    if (item.StartDate > report.StartTime && item.EndDate < report.EndTime)
                    {
                        serv[item.ServiceId]++;
                        names[item.ServiceId] = item.ServiceItem.Name;
                    }
                }
                for (int i = 0; i < serv.Length; i++)
                {
                    if(serv[i] != 0 && serv[i] > maxnum)
                    {
                        maxnum = serv[i];
                        maxid = i;
                        maxname = names[i];
                    }
                }
                report.ContentsHtml = "Most popular service during the selected time period:   ID= " + maxid + "   Name = " + maxname + "   Was ordered " + maxnum + " times \n";
                if (maxnum == 0)
                {
                    report.ContentsHtml = "The selected time period does not have enough data to create a report, try to select a wider time period \n";
                }

            }

            if (report.Type.ToString() == "MostPopularGoods")
            {
                int[] good = new int[100];//Nežinau, iki kiek eina id
                string[] names = new string[100];
                for (int i = 0; i < good.Length; i++)
                { good[i] = 0; }
                int maxid = 0;
                int maxnum = 0;
                string maxname = "";

                foreach (var item in _context.Carts)
                {
                    if (item.IsFinal && item.LastEditDate > report.StartTime && item.LastEditDate < report.EndTime)
                    {
                        foreach (var item2 in item.OrderedInventoryItems)
                        {
                            good[item2.ItemId]++;
                            names[item2.ItemId] = item2.InventoryItem.Name;
                        }

                    }
                }
                for (int i = 0; i < good.Length; i++)
                {
                    if (good[i] != 0 && good[i] > maxnum)
                    {
                        maxnum = good[i];
                        maxid = i;
                        maxname = names[i];
                    }
                }
                report.ContentsHtml = "Most popular goods during the selected time period:   ID= " + maxid + "   Name = " + maxname + "   Was ordered " + maxnum + " times \n";
                if (maxnum == 0)
                {
                    report.ContentsHtml = "The selected time period does not have enough data to create a report, try to select a wider time period \n";
                }
            }

            if (report.Type.ToString() == "AllOrders")
            {
                foreach (var item in _context.Carts)
                {
                    if (item.IsFinal && item.LastEditDate > report.StartTime && item.LastEditDate < report.EndTime)
                    {
                        report.ContentsHtml.Insert(report.ContentsHtml.Length, "Order ID = " + item.Id + " Order Sum = " + item.TotalValue + " Placement date = " + item.LastEditDate + " Client = " + item.User + "\n");
                    }
                }

                if (report.ContentsHtml.Length < 10)
                {
                    report.ContentsHtml = "The selected time period does not have enough data to create a report, try to select a wider time period \n aaa";
                }
            }

            if (report.Type.ToString() == "ActiveOrders")
            {
                foreach (var item in _context.Carts)
                {
                    if (item.IsFinal && item.LastEditDate.AddDays(7) > DateTime.Today) // Laikoma, kad užsakymai aktyvūs 7 dienas
                    {
                        report.ContentsHtml.Insert(report.ContentsHtml.Length, "Order ID = " + item.Id + " Order Sum = " + item.TotalValue + " Placement date = " + item.LastEditDate + " Client = " + item.User + "\n");
                    }
                }

                if (report.ContentsHtml.Length < 10)
                {
                    report.ContentsHtml = "There are no active orders at this time \n";
                }
            }

            return View(report);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            ViewData["CreatorId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,StartTime,EndTime,Description,ContentsHtml,CreatorId")] Report report)
        {
            if (ModelState.IsValid)
            {
                report.Id = Guid.NewGuid();
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", report.CreatorId);
            return View(report);
        }
    }
}
