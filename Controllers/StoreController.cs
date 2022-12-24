using System.Security.Claims;
using System.Web.Http;
using ApplicationDev.Data;
using ApplicationDev.Models;
using ApplicationDev.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApplicationDev.Controllers
{
	[Authorize(Roles = "Admin")]
	public class StoreController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _storeService = storeService;
            _context = context;
        } 
        //GET
        public async Task<IActionResult> Index()
        {
            var obj = await _storeService.GetAll();
            return View(obj);
        }
    }
}