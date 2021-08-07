using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityApp.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class ApiAuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser<int>> signInManager;
        private readonly UserManager<IdentityUser<int>> userManager;
        private IConfiguration configuration;

        public ApiAuthController(
            SignInManager<IdentityUser<int>> signInManager,
            UserManager<IdentityUser<int>> userManager,
            IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost("signin")]
        public async Task<object> ApiSignIn([FromBody] SignInCredentials credentials)
        {
            var user = await userManager.FindByEmailAsync(credentials.Email);
            var result = await signInManager.CheckPasswordSignInAsync(user, credentials.Password, true);
            if (result.Succeeded)
            {
                var descriptor = new SecurityTokenDescriptor
                {
                    Subject = (await signInManager.CreateUserPrincipalAsync(user)).Identities.First(),
                    Expires = DateTime.Now.AddMinutes(int.Parse(configuration["BearerTokens:ExpiryMins"])),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["BearerTokens:Key"])),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var handler = new JwtSecurityTokenHandler();
                var secToken = new JwtSecurityTokenHandler().CreateToken(descriptor);

                return new { Success = true, Token = handler.WriteToken(secToken) };
            }

            return new { Success = false };
        }

        //[HttpPost("signout")]
        //public async Task<IActionResult> ApiSignOut()
        //{
        //    await signInManager.SignOutAsync();

        //    return Ok();
        //}

    }

    public class SignInCredentials
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
