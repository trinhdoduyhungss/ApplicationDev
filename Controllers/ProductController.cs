
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationDev.Models;
using System.Linq;using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApplicationDev.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductService _productService;
        private readonly ApplicationDbContext _context;
    public ProductController(IProductService productService, IWebHostEnvironment hostEnvironment, ApplicationDbContext context,IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _hostEnvironment = hostEnvironment;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        // GET
        public async Task<IActionResult> Index()
        {
            var obj = await _productService.GetAll();
            return View(obj);
        }
    }
}