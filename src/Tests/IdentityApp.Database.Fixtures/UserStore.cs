using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityApp.User.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdentityApp.Database.Fixtures
{
    [TestClass]
    public class UserStore
    {
        private static IdentityUserRepository _userStore;

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = configuration.GetConnectionString("Default");
            _userStore = new IdentityUserRepository(connectionString);
        }

        [TestMethod]
        public async Task ShouldAddAndRemoveClaimsCorrectly()
        {
            var result = await _userStore.CreateAsync(new IdentityUser<int>
            {
                Email = "integration.test.user1",
                NormalizedEmail = "INTEGRATION.TEST.USER1",
                UserName = "integration.test.user1",
                NormalizedUserName = "INTEGRATION.TEST.USER1"
            },
            new CancellationToken());

            Assert.IsTrue(result.Succeeded);

            var user = await _userStore.FindByNameAsync("INTEGRATION.TEST.USER1", new CancellationToken());

            await _userStore.AddClaimsAsync(
                user,
                new[]
                {
                    new Claim("integration.test.type1", "integration.test.value1"),
                    new Claim("integration.test.type2", "integration.test.value2"),
                    new Claim("integration.test.type3", "integration.test.value3")
                },
                new CancellationToken());

            var claims = await _userStore.GetClaimsAsync(user, new CancellationToken());

            Assert.AreEqual(3, claims.Count);
            CollectionAssert.AreEqual(
                new[] { "integration.test.type1", "integration.test.type2", "integration.test.type3" },
                claims.Select(c => c.Type).ToArray());
            CollectionAssert.AreEqual(
                new[] { "integration.test.value1", "integration.test.value2", "integration.test.value3" },
                claims.Select(c => c.Value).ToArray());

            await _userStore.RemoveClaimsAsync(
                user,
                new[]
                {
                    new Claim("integration.test.type1", "integration.test.value1"),
                    new Claim("integration.test.type2", "integration.test.value2"),
                    new Claim("integration.test.type3", "integration.test.value3")
                },
                new CancellationToken());

            claims = await _userStore.GetClaimsAsync(user, new CancellationToken());

            Assert.AreEqual(0, claims.Count);

            result = await _userStore.DeleteAsync(user, new CancellationToken());

            Assert.IsTrue(result.Succeeded);
        }
    }
}
