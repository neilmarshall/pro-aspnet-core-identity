using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Product.Repository.Tests
{
    [TestClass]
    public class ProductRepositoryFixture
    {
        private static IProductRepository _productRepository;

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = configuration.GetConnectionString("Default");
            _productRepository = new ProductRepository(connectionString);
        }

        [TestMethod]
        public async Task GetProductFixture()
        {
            var product = await _productRepository.GetProductAsync(3);

            Assert.AreEqual("Soccer", product.Category);
            Assert.AreEqual("Soccer Ball", product.Name);
            Assert.AreEqual(19.50m, product.Price);
        }

        [TestMethod]
        public async Task GetAllProductsFixture()
        {
            var products = (await _productRepository.GetAllProductsAsync()).ToArray();

            Assert.AreEqual(9, products.Count());
        }

        [TestMethod]
        public async Task GetProductsFixture()
        {
            var products = (await _productRepository.GetProductsAsync(new[] { 3, 4, 6 })).ToArray();

            Assert.AreEqual(3, products.Count());

            Assert.AreEqual("Soccer", products[0].Category);
            Assert.AreEqual("Soccer Ball", products[0].Name);
            Assert.AreEqual(19.50m, products[0].Price);

            Assert.AreEqual("Soccer", products[1].Category);
            Assert.AreEqual("Corner Flags", products[1].Name);
            Assert.AreEqual(34.95m, products[1].Price);

            Assert.AreEqual("Chess", products[2].Category);
            Assert.AreEqual("Thinking Cap", products[2].Name);
            Assert.AreEqual(16.00m, products[2].Price);
        }
    }
}
