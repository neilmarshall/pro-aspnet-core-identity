using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Dapper;
using Npgsql;
using System.Security.Claims;

namespace IdentityApp.User.Repository
{
    public partial class IdentityUserRepository :
        IQueryableUserStore<IdentityUser<int>>,
        IUserEmailStore<IdentityUser<int>>,
        IUserPasswordStore<IdentityUser<int>>,
        IUserPhoneNumberStore<IdentityUser<int>>,
        IUserSecurityStampStore<IdentityUser<int>>,
        IUserStore<IdentityUser<int>>
    {
        private readonly string _connectionString;

        public IdentityUserRepository(string connectionString)
        {
            _connectionString = connectionString;

            MapNpgsqlTypes();
        }

        private void MapNpgsqlTypes()
        {
            NpgsqlConnection.GlobalTypeMapper.MapComposite<Claim>("identity.claim");
        }

        public IQueryable<IdentityUser<int>> Users
        {
            get
            {
                try
                {
                    using var conn = new NpgsqlConnection(_connectionString);

                    var users = conn.Query<IdentityUser<int>>(@"SELECT * FROM identity.users;");

                    return users.AsQueryable();
                }
                catch (Exception)
                {
                    return Enumerable.Empty<IdentityUser<int>>().AsQueryable();
                }
            }
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);

                var id = await conn.QueryFirstAsync<int>(
                    @"SELECT identity.create_user(
                        @username,
                        @normalizedusername,
                        @email,
                        @normalizedemail,
                        @emailconfirmed,
                        @passwordhash,
                        @securitystamp,
                        @concurrencystamp,
                        @accessfailedcount);",
                    new
                    {
                        user.UserName,
                        user.NormalizedUserName,
                        user.Email,
                        user.NormalizedEmail,
                        user.EmailConfirmed,
                        user.PasswordHash,
                        user.SecurityStamp,
                        user.ConcurrencyStamp,
                        user.AccessFailedCount
                    });

                user.Id = id;

                return IdentityResult.Success;
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> DeleteAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);

                await conn.ExecuteAsync(
                    "SELECT identity.delete_user(@id);",
                    new { user.Id });

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

        public async Task<IdentityUser<int>> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var user = await conn.QueryFirstOrDefaultAsync<IdentityUser<int>>(
                "SELECT * FROM identity.users WHERE normalizedEmail = @normalizedEmail;",
                new { normalizedEmail });

            return user;
        }

        public async Task<IdentityUser<int>> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var id = int.Parse(userId);

            using var conn = new NpgsqlConnection(_connectionString);

            var user = await conn.QueryFirstOrDefaultAsync<IdentityUser<int>>(
                "SELECT * FROM identity.users WHERE id = @id;",
                new { id });

            return user;
        }

        public async Task<IdentityUser<int>> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var user = await conn.QueryFirstOrDefaultAsync<IdentityUser<int>>(
                "SELECT * FROM identity.users WHERE normalizedusername = @normalizedusername;",
                new { normalizedUserName });

            return user;
        }

        public Task<int> GetAccessFailedCountAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<string> GetEmailAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetPhoneNumberAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult((string)null);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSecurityStampAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task<string> GetUserIdAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount += 1;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task SetEmailAsync(IdentityUser<int> user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(IdentityUser<int> user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(IdentityUser<int> user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(IdentityUser<int> user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(IdentityUser<int> user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberAsync(IdentityUser<int> user, string phoneNumber, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(IdentityUser<int> user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityStampAsync(IdentityUser<int> user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(IdentityUser<int> user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);

                await conn.ExecuteAsync(
                    @"SELECT identity.update_user(
                        @id,
                        @username,
                        @normalizedusername,
                        @email,
                        @normalizedemail,
                        @emailconfirmed,
                        @passwordhash,
                        @securitystamp,
                        @concurrencystamp,
                        @accessfailedcount,
                        @lockoutenabled,
                        @lockoutend);",
                    new
                    {
                        user.Id,
                        user.UserName,
                        user.NormalizedUserName,
                        user.Email,
                        user.NormalizedEmail,
                        user.EmailConfirmed,
                        user.PasswordHash,
                        user.SecurityStamp,
                        user.ConcurrencyStamp,
                        user.AccessFailedCount,
                        user.LockoutEnabled,
                        user.LockoutEnd
                    });

                return IdentityResult.Success;
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }
        }
    }

    public partial class IdentityUserRepository : IUserLockoutStore<IdentityUser<int>>
    {
        public Task<bool> GetLockoutEnabledAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnd);
        }

        public Task SetLockoutEnabledAsync(IdentityUser<int> user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(IdentityUser<int> user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }
    }

    public partial class IdentityUserRepository : IUserRoleStore<IdentityUser<int>>
    {
        public async Task AddToRoleAsync(IdentityUser<int> user, string roleName, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            await conn.ExecuteAsync(
                "SELECT identity.add_user_role(@id, @rolename);",
                new { user.Id, roleName });
        }

        public async Task<IList<string>> GetRolesAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var roles = await conn.QueryAsync<string>(
                "SELECT identity.get_user_roles(@id)",
                new { user.Id });

            return roles.ToList();
        }

        public async Task<IList<IdentityUser<int>>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var users = await conn.QueryAsync<IdentityUser<int>>(
                "SELECT * FROM identity.get_users_in_role(@rolename);",
                new { roleName });

            return users.ToList();
        }

        public async Task<bool> IsInRoleAsync(IdentityUser<int> user, string roleName, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            return await conn.QueryFirstAsync<bool>(
                "SELECT identity.user_is_in_role(@id, @rolename);",
                new { user.Id, roleName });
        }

        public async Task RemoveFromRoleAsync(IdentityUser<int> user, string roleName, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            await conn.ExecuteAsync(
                "SELECT identity.remove_user_role(@userid, @rolename);",
                new { UserId = user.Id, roleName });
        }
    }

    public partial class IdentityUserRepository : IUserClaimStore<IdentityUser<int>>
    {
        public async Task AddClaimsAsync(IdentityUser<int> user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            await conn.OpenAsync(cancellationToken);

            using var cmd = new NpgsqlCommand("SELECT identity.add_claim(@id, @claims);", conn);

            cmd.Parameters.Add(new NpgsqlParameter { ParameterName = "id", Value = user.Id });
            cmd.Parameters.Add(new NpgsqlParameter { ParameterName = "claims", Value = claims.ToArray() });

            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<IList<Claim>> GetClaimsAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var claims = await conn.QueryAsync<(string type, string value)>(
                "SELECT * FROM identity.get_claims(@id);",
                new { user.Id });

            return claims.Select(c => new Claim(c.type, c.value)).ToList();
        }

        public Task<IList<IdentityUser<int>>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveClaimsAsync(IdentityUser<int> user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            foreach (var claim in claims)
            {
                await conn.ExecuteAsync(
                    "SELECT identity.delete_claim(@id, @type);",
                    new { user.Id, claim.Type });
            }
        }

        public async Task ReplaceClaimAsync(IdentityUser<int> user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            await conn.ExecuteAsync(
                "SELECT identity.update_claim(@id, @type, @value);",
                new { user.Id, claim.Type, newClaim.Value });
        }
    }
}