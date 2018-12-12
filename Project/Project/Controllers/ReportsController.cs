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

            //priklausomai nuo to, koks ataskaitos tipas, reikia suformuoti teksto eilutę su kažkokia "ataskaitos" informacija
            if (report.Type.ToString() == "Clients")
            {
                report.ContentsHtml = "";
                foreach (var item in _context.Users)
                {
                    if (item.RegisterDate.CompareTo(report.EndTime) == -1 && item.RegisterDate.CompareTo(report.StartTime) == 1)
                    {
                        report.ContentsHtml += item.FirstName + " " + item.LastName + System.Environment.NewLine
                                            + item.Email + System.Environment.NewLine
                                            + item.RegisterDate + System.Environment.NewLine + System.Environment.NewLine;
                    }
                }
                if (report.ContentsHtml.Length < 10)
                {
                    report.ContentsHtml = "The selected time period does not have enough data to create a report, try to select a wider time period" + System.Environment.NewLine;
                }
            }
            if (report.Type.ToString() == "Profits")
            {
                decimal sum = 0;
                foreach (var item in _context.Carts)
                {
                    if (item.IsFinal && item.LastEditDate.CompareTo(report.StartTime) == 1 && item.LastEditDate.CompareTo(report.EndTime) == -1)
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
                for (int i = 0; i < serv.Length; i++)
                { serv[i] = 0; }
                int maxid = 0;
                int maxnum = 0;

                foreach (var item in _context.OrderedServiceItems)
                {
                    if (item.StartDate.CompareTo(report.StartTime) == 1 && item.EndDate.CompareTo(report.EndTime) == -1)
                    {
                        serv[item.ServiceId]++;
                    }
                }
                for (int i = 0; i < serv.Length; i++)
                {
                    if (serv[i] != 0 && serv[i] > maxnum)
                    {
                        maxnum = serv[i];
                        maxid = i;
                    }
                }
                report.ContentsHtml = "Most popular service during the selected time period:   " +
                                    "ID= " + maxid + System.Environment.NewLine +
                                    "Times ordered = " + maxnum + System.Environment.NewLine + System.Environment.NewLine;
                if (maxnum == 0)
                {
                    report.ContentsHtml = "The selected time period does not have enough data to create a report, try to select a wider time period \n";
                }

            }

            if (report.Type.ToString() == "MostPopularGoods")
            {
                int[] good = new int[100];//Nežinau, iki kiek eina id
                for (int i = 0; i < good.Length; i++)
                { good[i] = 0; }
                int maxid = 0;
                int maxnum = 0;

                foreach (var item in _context.Carts)
                {
                    if (item.IsFinal && item.LastEditDate.CompareTo(report.StartTime) == 1 && item.LastEditDate.CompareTo(report.EndTime) == -1)
                    {
                        foreach (var item2 in _context.OrderedInventoryItems)
                        {
                            if (item2.CartId == item.Id)
                            {
                                good[item2.ItemId]++;
                            }
                        }

                    }
                }
                for (int i = 0; i < good.Length; i++)
                {
                    if (good[i] != 0 && good[i] > maxnum)
                    {
                        maxnum = good[i];
                        maxid = i;
                    }
                }
                report.ContentsHtml = "Most popular good during the selected time period:   " +
                                   "ID= " + maxid + System.Environment.NewLine +
                                   "Times ordered = " + maxnum + System.Environment.NewLine + System.Environment.NewLine;
                if (maxnum == 0)
                {
                    report.ContentsHtml = "The selected time period does not have enough data to create a report, try to select a wider time period \n";
                }
            }

            if (report.Type.ToString() == "AllOrders")
            {
                foreach (var item in _context.Carts)
                {
                    //report.ContentsHtml = item.IsFinal + " " +item.LastEditDate + " " + report.StartTime + " " + report.EndTime + " " + item.LastEditDate.CompareTo(report.StartTime) + " " + item.LastEditDate.CompareTo(report.EndTime); 
                    if (item.IsFinal && item.LastEditDate.CompareTo(report.StartTime) == 1 && item.LastEditDate.CompareTo(report.EndTime) == -1)
                    {
                        report.ContentsHtml += "Order ID = " + item.Id + System.Environment.NewLine
                                                + " Order Sum = " + item.TotalValue + System.Environment.NewLine
                                                + " Placement date = " + item.LastEditDate + System.Environment.NewLine
                                                + " Client = " + item.UserId + System.Environment.NewLine + System.Environment.NewLine;
                    }
                }

                /*
                if (report.ContentsHtml.Length < 10)
                {
                    report.ContentsHtml = "The selected time period does not have enough data to create a report, try to select a wider time period \n";
                }
                */
            }

            if (report.Type.ToString() == "ActiveOrders")
            {
                report.ContentsHtml = "";
                foreach (var item in _context.Carts)
                {
                    if (item.IsFinal && item.LastEditDate.AddDays(7).CompareTo(DateTime.Today) == 1) // Laikoma, kad užsakymai aktyvūs 7 dienas
                    {
                        report.ContentsHtml += "Order ID = " + item.Id + System.Environment.NewLine
                                             + " Order Sum = " + item.TotalValue + System.Environment.NewLine
                                             + " Placement date = " + item.LastEditDate + System.Environment.NewLine
                                             + " Client = " + item.User + System.Environment.NewLine + System.Environment.NewLine;
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
