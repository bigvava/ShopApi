using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using ShopApi.Dtos;
using ShopApi.Entities;
using ShopApi.Repositories;
using System.Xml.Linq;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ShopApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public ProductService(IProductRepository productRepository, IMapper mapper, IDistributedCache cache)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            //ეს გამორჩენილი გვქონდა. ლექციის შემდეგ დავამატე
            _cache = cache;
        }

        public IEnumerable<ProductDto> GetAllProducts()
        {
            var products = _productRepository.GetAll();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var cacheKey = $"ProductById-{id}";
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonConvert.DeserializeObject<ProductDto>(cachedData);
            }

            var product = _productRepository.GetById(id);
            var productDto = _mapper.Map<ProductDto>(product);


            var cacheOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(productDto), cacheOptions);


            return productDto;
        }

        public ProductDto CreateProduct(ProductForCreateDto productDto, int userId)
        {

            var product = _mapper.Map<Product>(productDto);
            product.AdderId = userId;
            product = _productRepository.Insert(product);
            return _mapper.Map<ProductDto>(product);
        }

        public void UpdateProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _productRepository.Update(product);
        }

        public void DeleteProduct(int id)
        {
            var product = _productRepository.GetById(id);
            if (product != null)
            {
                _productRepository.Delete(product);
            }
        }
    }
}
