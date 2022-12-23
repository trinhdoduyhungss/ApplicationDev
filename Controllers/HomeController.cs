﻿using System.Diagnostics;
using ApplicationDev.Data;
using Microsoft.AspNetCore.Mvc;
using ApplicationDev.Models;

namespace ApplicationDev.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

     public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

    public IActionResult Index()
    {
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
