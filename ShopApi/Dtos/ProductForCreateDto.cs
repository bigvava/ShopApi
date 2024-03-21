using ShopApi.Entities;

namespace ShopApi.Dtos
{
    public class ProductForCreateDto
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
        public int CategoryId { get; set; }
    }
}
