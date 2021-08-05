using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Repository;

namespace IdentityApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;

        public AdminController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllProductsAsync();

            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit", new Models.Product());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var product = await _productRepository.GetProductAsync(id);

            if (product != null)
            {
                return View("Edit", product);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Save(Models.Product product)
        {
            await _productRepository.SaveProductAsync(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var product = await _productRepository.GetProductAsync(id);

            if (product != null)
            {
                await _productRepository.DeleteProductAsync(product);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
