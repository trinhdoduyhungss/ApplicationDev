using System.Security.Claims;
using ApplicationDev.Data;
using ApplicationDev.Models;
using ApplicationDev.Service.IService;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApplicationDev.Service
{
    public class StoreService : IStoreService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public StoreService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<Store> Create(Store store)
        {
            if (store.Id == 0)
            {
                await _context.Stores.AddAsync(store);
                await _context.SaveChangesAsync();
            }
            return store;
        }

        public async Task<List<Store>> GetAll()
        {
            var objId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userRole = _context.UserRoles.Where(x => x.UserId == objId).Select(x => x.RoleId).ToList();
            var role = _context.Roles.Where(x => userRole.Contains(x.Id)).Select(x => x.Name).ToList();
            if (role.Contains("Admin") || role.Contains("SuperAdmin"))
            {
                var stores = _context.Stores.ToList();
                return stores;
            }
            else
            {
                var stores = _context.Stores.Where(x => x.UserId == objId).ToList();
                return stores;
            }
        }
    
        public async Task<Store> GetById(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            return store;
        }

        public async Task<Store> Update(Store store)
        {
            if (store.Id != 0)
            {
                _context.Stores.Update(store);
                await _context.SaveChangesAsync();
            }
            return store;
        }

        public async Task<Store> Delete(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store != null)
            {
                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
            }
            return store;
        }
    }
}