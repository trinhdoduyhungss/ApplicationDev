using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationDev.Data;
using ApplicationDev.Models;
using ApplicationDev.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        public IActionResult Create()
        {
            var objId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Product product = new Product()
            {
                StoreList = _context.Stores.Where(x => x.UserId == objId).ToList().Select(x=> new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }),
            };
            return View(product);
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