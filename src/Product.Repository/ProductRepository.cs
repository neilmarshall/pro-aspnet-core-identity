using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace Product.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IdentityApp.Models.Product> GetProductAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var product = await conn.QueryFirstAsync<IdentityApp.Models.Product>(
                "SELECT * FROM products.product WHERE id = @id;",
                new { id });
            return product;
        }

        public IEnumerable<IdentityApp.Models.Product> GetAllProducts()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var products = conn.Query<IdentityApp.Models.Product>(
                "SELECT * FROM products.product;");
            return products.ToArray();
        }

        public async Task<IEnumerable<IdentityApp.Models.Product>> GetAllProductsAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var products = await conn.QueryAsync<IdentityApp.Models.Product>(
                "SELECT * FROM products.product;");
            return products.ToArray();
        }

        public async Task<IEnumerable<IdentityApp.Models.Product>> GetProductsAsync(IEnumerable<int> ids)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var products = await conn.QueryAsync<IdentityApp.Models.Product>(
                "SELECT * FROM products.product WHERE id = ANY(@ids);",
                new { ids });
            return products.ToArray();
        }

        public async Task SaveProductAsync(IdentityApp.Models.Product product)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            if (product.Id != 0)
            {
                await conn.ExecuteAsync(
                    "UPDATE products.product SET name = @name , price = @price, category = @category WHERE id = @id;",
                    new { id = product.Id, nm = product.Name, price = product.Price, category = product.Category });
            }
            else
            {
                await conn.ExecuteAsync(
                    "INSERT INTO products.product (name, price, category) VALUES (@name, @price, @category);",
                    new { name = product.Name, price = product.Price, category = product.Category });
            }
        }

        public async Task DeleteProductAsync(IdentityApp.Models.Product product)
        {
            if (product != null)
            {
                using var conn = new NpgsqlConnection(_connectionString);
                await conn.ExecuteAsync(
                    "DELETE FROM products.product WHERE id = @id;",
                    new { id = product.Id });
            }
        }
    }
}
