using ApplicationDev.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ApplicationDev.Service.IService
{
    public interface IUserService
    {
        public Task<List<IdentityRole>> GetAll();
        public Task<string> AddRole(string name);
        public Task<List<UserManagerVM>> UserRolesDetail();
        public Task<List<RolesManagerVM>> ViewManagerUserRole(string userId);
        public Task<List<RolesManagerVM>> ManagerUserRole(List<RolesManagerVM> model, string userId);
    }
}