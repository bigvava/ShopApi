using ShopApi.Entities;

namespace ShopApi.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
        public Category Category { get; set; }
    }
}
