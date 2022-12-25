using ApplicationDev.Service.IService;
using ApplicationDev.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace ApplicationDev.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        
        public async Task<List<IdentityRole>> GetAll()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }

        public async Task<IList<ApplicationUser>> GetManagers()
        {
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            var users1 = await _userManager.GetUsersInRoleAsync("SuperAdmin");
            var users2 = await _userManager.GetUsersInRoleAsync("StoreOwner");
            users.AddRange(users1);
            users.AddRange(users2);
            users = users.DistinctBy(x => x.Id).ToList();
            return users;
        }

        public async Task<string> AddRole(string roleName)
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            return roleName;
        }

        public async Task<List<UserManagerVM>> UserRolesDetail()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserManagerVM>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserManagerVM();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await _userManager.GetRolesAsync(user);
                userRolesViewModel.Add(thisViewModel);
            }

            return userRolesViewModel;
        }

        public async Task<List<RolesManagerVM>> ViewManagerUserRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var models = new List<RolesManagerVM>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new RolesManagerVM()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                models.Add(userRolesViewModel);
            }

            return models;
        }

        public async Task<List<RolesManagerVM>> ManagerUserRole(List<RolesManagerVM> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                new ModelError("Cannot remove user existing roles");
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                new ModelError("Cannot add selected roles to user");
            }
            
            return model;
        }
    }
}