using AutoMapper;
using ShopApi.Dtos;
using ShopApi.Entities;

namespace ShopApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductForCreateDto, Product>();
            CreateMap<Category, CategoryDto>().ReverseMap();
            //CreateMap<CategoryForReturnDto, Category>().ReverseMap();
            CreateMap<Category, CategoryForReturnDto>();

        }
    }
}
