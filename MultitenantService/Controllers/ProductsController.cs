using Microsoft.AspNetCore.Mvc;
using MultitenantService.Services;

namespace MultitenantService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int id)
        {
            var productDetails = await _service.GetByIdAsync(id);
            return Ok(productDetails);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateProductRequest request)
        {
            return Ok(await _service.CreateAsync(request.Name, request.Description, request.Price));
        }
    }

    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
