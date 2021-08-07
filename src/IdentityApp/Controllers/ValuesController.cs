using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Product.Repository;

namespace IdentityApp.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api/data")]
    public class ValuesController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<ValuesController> logger;

        public ValuesController(IProductRepository productRepository, ILogger<ValuesController> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<Models.Product> GetProducts() => productRepository.GetAllProducts();

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductBindingTarget target)
        {
            logger.LogDebug($"Creating product: {target.Name}, {target.Category}, {target.Price}");

            if (ModelState.IsValid)
            {
                var product = new Models.Product
                {
                    Category = target.Category,
                    Name = target.Name,
                    Price = target.Price
                };

                await productRepository.SaveProductAsync(product);

                return Ok(product);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task DeleteProduct(long id)
        {
            logger.LogDebug($"Deleting product: {id}");

            await productRepository.DeleteProductAsync(new Models.Product
            {
                Id = id
            });
        }
    }

    public class ProductBindingTarget
    {
        [Required]
        public string Category { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
