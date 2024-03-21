using ShopApi.Entities;

namespace ShopApi.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        Product Insert(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
