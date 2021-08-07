using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Dapper;
using Npgsql;

namespace IdentityApp.User.Repository
{
    public class IdentityRoleRepository :
        IQueryableRoleStore<IdentityRole<int>>,
        IRoleStore<IdentityRole<int>>
    {
        private readonly string _connectionString;

        public IdentityRoleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IQueryable<IdentityRole<int>> Roles
        {
            get
            {
                try
                {
                    using var conn = new NpgsqlConnection(_connectionString);

                    var roles = conn.Query<IdentityRole<int>>("SELECT * FROM identity.roles;");

                    return roles.AsQueryable();
                }
                catch (Exception)
                {
                    return Enumerable.Empty<IdentityRole<int>>().AsQueryable();
                }
            }
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole<int> role, CancellationToken cancellationToken)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);

                var id = await conn.QueryFirstAsync<int>(
                    "SELECT identity.create_role(@name, @normalizedname, @concurrencystamp);",
                    new { role.Name, role.NormalizedName, role.ConcurrencyStamp });

                role.Id = id;

                return IdentityResult.Success;
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole<int> role, CancellationToken cancellationToken)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);

                await conn.ExecuteAsync(
                    "SELECT identity.delete_role(@id);",
                    new { role.Id });

                return IdentityResult.Success;
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }
        }

        public void Dispose()
        {
        }

        public Task<IdentityRole<int>> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityRole<int>> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var role = await conn.QueryFirstOrDefaultAsync<IdentityRole<int>>(
                "SELECT * FROM identity.roles WHERE normalizedname = @normalizedrolename;",
                new { normalizedRoleName });

            return role;
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole<int> role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(IdentityRole<int> role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(IdentityRole<int> role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole<int> role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(IdentityRole<int> role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole<int> role, CancellationToken cancellationToken)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);

                await conn.ExecuteAsync(
                    "SELECT identity.update_role(@id, @name, @normalizedname, @concurrencystamp);",
                    new { role.Id, role.Name, role.NormalizedName, role.ConcurrencyStamp });

                return IdentityResult.Success;
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }
        }
    }
}
