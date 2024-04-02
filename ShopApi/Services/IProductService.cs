using ShopApi.Dtos;

namespace ShopApi.Services
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetAllProducts();
        Task <ProductDto> GetProductById(int id);
        ProductDto CreateProduct(ProductForCreateDto productDto, int userId);
        void UpdateProduct(ProductDto productDto);
        void DeleteProduct(int id);
    }
}
