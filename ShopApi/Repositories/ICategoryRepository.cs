using ShopApi.Entities;

namespace ShopApi.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        Category Insert(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}
