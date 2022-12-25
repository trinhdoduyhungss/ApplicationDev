using System;
using System.Collections.Generic;
using System.Security.Claims;
using ApplicationDev.Data;
using ApplicationDev.Models;
using ApplicationDev.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDev.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductService _productService;
        private readonly ApplicationDbContext _context;

        public ProductController(IProductService productService, IWebHostEnvironment hostEnvironment, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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
        [Authorize(Roles = "Admin, StoreOwner")]
        public IActionResult Create()
        {
            var objId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userRole = _context.UserRoles.Where(x => x.UserId == objId).Select(x => x.RoleId).ToList();
            var role = _context.Roles.Where(x => userRole.Contains(x.Id)).Select(x => x.Name).ToList();
            if (role.Contains("Admin") || role.Contains("SuperAdmin"))
            {
                Product product = new Product()
                {
                    StoreList = _context.Stores.Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                };
                return View(product);
            }
            else
            {
                Product product = new Product()
                {
                    StoreList = _context.Stores.Where(x => x.UserId == objId).ToList().Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                };
                return View(product);
            };
        }

        [HttpPost]
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Create(Product product)
        {
            var obj = _context.Products;
            await _productService.Create(product);
            return RedirectToAction("ProductInStore", "Product", new { id = product.StoreId });
        }

        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _productService.Delete(id);
            return RedirectToAction("ProductInStore", "Product", new { id = obj.StoreId });
        }

        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Edit(int id)
        {
            var obj = await _productService.GetById(id);
            var objId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userRole = _context.UserRoles.Where(x => x.UserId == objId).Select(x => x.RoleId).ToList();
            var role = _context.Roles.Where(x => userRole.Contains(x.Id)).Select(x => x.Name).ToList();
            if (role.Contains("Admin") || role.Contains("SuperAdmin"))
            {
                obj.StoreList = _context.Stores.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });
            }
            else
            {
                obj.StoreList = _context.Stores.Where(x => x.UserId == objId).ToList().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });
            };
            return View(obj);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Edit(Product product)
        {
            var obj = _context.Products;
            await _productService.Update(product, product.Id);
            return RedirectToAction("ProductInStore", "Product", new { id = product.StoreId });
        }

        [HttpGet]
        public async Task<IActionResult> ProductInStore(int id)
        {
            var obj = _context.Products
                .Include(x => x.Store)
                .Where(x => x.StoreId == id);
            return View(obj);
        }

    }
}