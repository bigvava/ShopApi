using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos;
using ShopApi.Filters;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [RoleFilter("StandardUser")]
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> Get()
        {
            var products = _productService.GetAllProducts();
            if (products.Count() == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [Authorize]
        //[RoleFilter("Reader,BookAdder")]
        [RoleFilter("StandardUser,Admin")]

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

       
        [HttpPost]
        public IActionResult Post([FromBody] ProductForCreateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var p = _productService.CreateProduct(productDto);
            return CreatedAtAction(nameof(Get), new { id = p.Id }, productDto);
        }
    }
}
