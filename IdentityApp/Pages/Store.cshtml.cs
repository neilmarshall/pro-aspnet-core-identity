using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Product.Repository;

namespace IdentityApp.Pages
{
    [Authorize]
    public class StoreModel : PageModel
    {
        private readonly IProductRepository _productRepository;

        public IEnumerable<Models.Product> Products { get => _productRepository.GetAllProducts(); }

        public StoreModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

    }
}
