using ShopApi.Dtos;

namespace ShopApi.Services
{
    public interface ICategoryService
    {
        IEnumerable<CategoryForReturnDto> GetAllCategories();
        CategoryDto GetCategoryById(int id);
        CategoryDto CreateCategory(CategoryDto categoryDto);
        void UpdateCategory(CategoryDto categoryDto);
        void DeleteCategory(int id);
    }
}
