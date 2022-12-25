using ApplicationDev.Models;

namespace ApplicationDev.Service.IService
{
    public interface IProductService
    {
        public Task<List<Product>> GetAll();
        public Task<Product> GetById(int id);
        public Task<Product> Create(Product product);
        public Task<List<Product>> ProductInStore(int? id);
        public Task<Product> Update(Product product, int id);
        public Task<Product> Delete(int id);
        public Task<Product> Detail(int id);
    }
}