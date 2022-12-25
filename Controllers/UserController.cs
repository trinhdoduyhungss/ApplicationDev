using ApplicationDev.Service.IService;
using ApplicationDev.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ApplicationDev.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        public UserController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }
        // GET
        public async Task<IActionResult> Index()
        {
            var obj = await _userService.GetAll();
            return View(obj);
        }
        // CREATE
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            await _userService.AddRole(roleName);
            return RedirectToAction(nameof(Index));
        }
        //GET 
        public async Task<IActionResult> UserRolesDetail()
        {
            var obj = await _userService.UserRolesDetail();
            return View(obj);
        }

        public async Task<IActionResult> ManagerUserRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View();
            }
            ViewBag.UserName = user.UserName;
            var obj = await _userService.ViewManagerUserRole(userId);
            return View(obj);
        }
        
        //POST
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> ManagerUserRole(List<RolesManagerVM> model, string userId)
        {
            await _userService.ManagerUserRole(model, userId);
            return RedirectToAction(nameof(UserRolesDetail));
        }

    }
}