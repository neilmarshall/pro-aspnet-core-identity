using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Product.Repository;

namespace IdentityApp.Pages
{
    public class LandingModel : PageModel
    {
        private readonly IProductRepository _productRepository;

        public IEnumerable<Models.Product> Products { get => _productRepository.GetAllProducts(); }

        public LandingModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
    }
}
