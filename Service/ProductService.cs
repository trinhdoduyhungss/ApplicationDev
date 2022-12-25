using ApplicationDev.Data;
using ApplicationDev.Models;
using ApplicationDev.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationDev.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<List<Product>> GetAll()
        {
            var obj = _context.Products.ToList();
            return obj;
        }

        public async Task<Product> GetById(int id)
        {
            var obj = await _context.Products.FindAsync(id);
            return obj;
        }

        public async Task<Product> Create(Product product)
        {
            if (product.Id == 0)
            {                
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            return product;
        }

        public Task<List<Product>> ProductInStore(int? id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Product> Update(Product product, int id)
        {
            if (product.Id == id)
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            return product;
        }

        public async Task<Product> Delete(int id)
        {
            var obj = await _context.Products.FindAsync(id);
            if (obj != null)
            {
                _context.Products.Remove(obj);
                await _context.SaveChangesAsync();
            }
            Console.WriteLine(obj.StoreId);
            return obj;
        }

        public Task<Product> Detail(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}