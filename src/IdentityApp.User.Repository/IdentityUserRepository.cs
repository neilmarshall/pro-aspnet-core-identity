using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace IdentityApp.User.Repository
{
    public class IdentityUserRepository : IUserStore<IdentityUser<int>>, IUserEmailStore<IdentityUser<int>>, IUserPasswordStore<IdentityUser<int>>, IUserPhoneNumberStore<IdentityUser<int>>, IQueryableUserStore<IdentityUser<int>>, IUserLockoutStore<IdentityUser<int>>, IUserSecurityStampStore<IdentityUser<int>>
    {
        private readonly string _connectionString;

        public IdentityUserRepository(string connectionString)
        {
            _connectionString = connectionString;
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
                    @"
                        INSERT INTO identity.users (
                            username, normalizedusername, email, normalizedemail, emailconfirmed, passwordhash, securitystamp, concurrencystamp, accessfailedcount
                        ) VALUES (
                            @username, @normalizedusername, @email, @normalizedemail, @emailconfirmed, @passwordhash, @securitystamp, @concurrencystamp, @accessfailedcount
                        )
                        RETURNING id;
                    ",
                    new
                    {
                        UserName = user.UserName,
                        NormalizedUserName = user.NormalizedUserName,
                        Email = user.Email,
                        NormalizedEmail = user.NormalizedEmail,
                        EmailConfirmed = user.EmailConfirmed,
                        PasswordHash = user.PasswordHash,
                        SecurityStamp = user.SecurityStamp,
                        ConcurrencyStamp = user.ConcurrencyStamp,
                        AccessFailedCount = user.AccessFailedCount
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
                    "DELETE FROM identity.users WHERE id = @id;",
                    new { id = user.Id });

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

        public Task<IdentityUser<int>> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
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
                "SELECT * FROM identity.users WHERE normalizedUserName = @normalizedUserName;",
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

        public Task<bool> GetLockoutEnabledAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnd);
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

        public async Task<int> IncrementAccessFailedCountAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount += 1;

            await UpdateAsync(user, cancellationToken);

            return user.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(IdentityUser<int> user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;

            await UpdateAsync(user, cancellationToken);
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

        public async Task SetLockoutEnabledAsync(IdentityUser<int> user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;

            await UpdateAsync(user, cancellationToken);
        }

        public async Task SetLockoutEndDateAsync(IdentityUser<int> user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;

            await UpdateAsync(user, cancellationToken);
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

        public async Task SetSecurityStampAsync(IdentityUser<int> user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;

            await UpdateAsync(user, cancellationToken);
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
                    @"
                        UPDATE identity.users
                           SET username = @username,
                               normalizedusername = @normalizedusername,
                               email = @email,
                               normalizedemail = @normalizedemail,
                               emailconfirmed = @emailconfirmed,
                               passwordhash = @passwordhash,
                               securitystamp = @securitystamp,
                               concurrencystamp = @concurrencystamp,
                               accessfailedcount = @accessfailedcount,
                               lockoutenabled = @lockoutenabled,
                               lockoutend = @lockoutend
                         WHERE id = @id;
                    ",
                    new
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        NormalizedUserName = user.NormalizedUserName,
                        Email = user.Email,
                        NormalizedEmail = user.NormalizedEmail,
                        EmailConfirmed = user.EmailConfirmed,
                        PasswordHash = user.PasswordHash,
                        SecurityStamp = user.SecurityStamp,
                        ConcurrencyStamp = user.ConcurrencyStamp,
                        AccessFailedCount = user.AccessFailedCount,
                        Lockoutenabled = user.LockoutEnabled,
                        Lockoutend = user.LockoutEnd
                    });

                return IdentityResult.Success;
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }
        }
    }
}
