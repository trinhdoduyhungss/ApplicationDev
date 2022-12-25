using System.Security.Claims;
using ApplicationDev.Data;
using ApplicationDev.Models;
using ApplicationDev.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApplicationDev.Controllers
{
	public class StoreController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly IStoreService _storeService;
        private readonly IUserService _userService;

        public StoreController(IUserService userService, IStoreService storeService, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _storeService = storeService;
            _userService = userService;
            _context = context;
        } 

        [Authorize(Roles = "Admin, StoreOwner")]
        public async Task<IActionResult> Index()
        {
            var obj = await _storeService.GetAll();
            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateStore()
        {
            var objId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (objId != null)
            {
                var allListManagers = _userService.GetManagers().Result.Select(x => x.Id).ToList();
                var assignedListManagers = _storeService.GetAll().Result.Select(x => x.UserId).ToList();
                var unAssignedListManagers = _context.Users.Where(x => allListManagers.Contains(x.Id) && !assignedListManagers.Contains(x.Id)).Select(x => x.Id).ToList();
                Store store = new Store
                {
                    UserList = _context.Users.Where(x => unAssignedListManagers.Contains(x.Id)).Select(x => new SelectListItem
                    {
                        Text = x.UserName,
                        Value = x.Id
                    })
                };
                return View(store);
            }else
            {
                return RedirectToAction("Index", "Home");
            }    
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> CreateStore(Store store)
        {
            await _storeService.Create(store);
            return RedirectToAction(nameof(Index));
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditStore(int id)
        {
            Store obj = await _storeService.GetById(id);
            var allListManagers = _userService.GetManagers().Result.Select(x => x.Id).ToList();
            var assignedListManagers = _storeService.GetAll().Result.Select(x => x.UserId).ToList();
            var unAssignedListManagers = _context.Users.Where(x => allListManagers.Contains(x.Id) && !assignedListManagers.Contains(x.Id)).Select(x => x.Id).ToList();
            obj.UserList = _context.Users.Where(x => unAssignedListManagers.Contains(x.Id)).Select(x => new SelectListItem
            {
                Text = x.UserName,
                Value = x.Id
            });
            return View(obj);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditStore(Store store)
        {
            await _storeService.Update(store);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStore(int id){
            await _storeService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}