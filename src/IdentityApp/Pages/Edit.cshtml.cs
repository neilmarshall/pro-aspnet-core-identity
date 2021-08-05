using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Product.Repository;

namespace IdentityApp.Pages
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IProductRepository _productRepository;

        public Models.Product Product { get; set; }

        public EditModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task OnGetAsync(long? id)
        {
            Product = id.HasValue
                ? await _productRepository.GetProductAsync(id.Value) ?? new Models.Product()
                : null;
        }

        public async Task<IActionResult> OnPostAsync(Models.Product product)
        {
            await _productRepository.SaveProductAsync(product);

            return RedirectToPage("Admin");
        }
    }
}
