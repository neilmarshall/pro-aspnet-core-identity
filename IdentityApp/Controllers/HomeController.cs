using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Product.Repository;

namespace IdentityApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();

            return View(products);
        }
    }
}
