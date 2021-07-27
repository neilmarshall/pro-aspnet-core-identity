using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Product.Repository;

namespace IdentityApp.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminModel : PageModel
    {
        private readonly IProductRepository _productRepository;

        public IEnumerable<Models.Product> Products { get => _productRepository.GetAllProducts(); }

        public AdminModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            var product = await _productRepository.GetProductAsync(id);

            if (product != null)
            {
                await _productRepository.DeleteProductAsync(product);
            }

            return Page();
        }
    }
}
