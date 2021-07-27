using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Repository
{
    public interface IProductRepository
    {
        Task<IdentityApp.Models.Product> GetProductAsync(long id);
        IEnumerable<IdentityApp.Models.Product> GetAllProducts();
        Task<IEnumerable<IdentityApp.Models.Product>> GetAllProductsAsync();
        Task<IEnumerable<IdentityApp.Models.Product>> GetProductsAsync(IEnumerable<int> ids);
        Task SaveProductAsync(IdentityApp.Models.Product product);
        Task DeleteProductAsync(IdentityApp.Models.Product product);
    }
}
