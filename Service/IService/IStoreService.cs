using ApplicationDev.Models;

namespace ApplicationDev.Service.IService
{
    public interface IStoreService
    {
        public Task<Store> Create(Store store);
        public Task<Store> Update(Store store);
        public Task<Store> Delete(int id);
        public Task<List<Store>> GetAll();
        public Task<Store> GetById(int id);
    }
}