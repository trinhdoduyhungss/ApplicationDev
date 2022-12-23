
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
    [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            //Save Image To wwwRoot
            
            var wwwRootPath = _hostEnvironment.WebRootPath;
            var filename = Path.GetFileNameWithoutExtension(product.ImageFile.FileName); // Name of Image
            var extension = Path.GetExtension(product.ImageFile.FileName); // Tails - png, mov. jpg
            product.ImgUrl = filename = filename + DateTime.Now.ToString("yymmssff") + extension; // Save ImageUrl
            var path = Path.Combine(wwwRootPath + "/Image/", filename); // Save in wwwRoot
            await using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(fileStream);
            }
            var obj = _context.Products;
            await _productService.Create(product);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ProductInStore(int? id)
        {
            var productId = id.Value;
            var product = await _context.Stores.FindAsync(productId);
            ViewBag.ProductId = product.Id;
            ViewBag.ProductName = product.Name;
            return View();
        }
}