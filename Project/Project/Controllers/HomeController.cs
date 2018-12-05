﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Project.Services;
using Project.ViewModels;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            EmailSender.SendEmail("gediminasmilasius@gmail.com", "Test1", "Test2");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
