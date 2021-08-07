using System.Text;
using IdentityApp.User.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Product.Repository;

namespace IdentityApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddIdentity<IdentityUser<int>, IdentityRole<int>>(options =>
            {
                options.Lockout = new LockoutOptions
                {
                    MaxFailedAccessAttempts = 2
                };
            });

            services.AddAuthentication().AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,
                opts =>
                {
                    opts.TokenValidationParameters.ValidateAudience = false;
                    opts.TokenValidationParameters.ValidateIssuer = false;
                    opts.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["BearerTokens:Key"]));
                });

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = System.TimeSpan.FromMinutes(1);
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/SignIn";
                options.LogoutPath = "/Identity/SignOut";
                options.AccessDeniedPath = "/Identity/Forbidden";
                options.Events.DisableRedirectionForApiClients();
            });

            services.AddCors(opts =>
            {
                opts.AddDefaultPolicy(builder => builder
                    .WithOrigins("http://localhost:5100")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });

            services.AddTransient<IUserStore<IdentityUser<int>>>(_ =>
                new IdentityUserRepository(Configuration.GetConnectionString("Default")));
            services.AddTransient<IRoleStore<IdentityRole<int>>>(_ =>
                new IdentityRoleRepository(Configuration.GetConnectionString("Default")));
            services.AddTransient<IProductRepository>(_ =>
                new ProductRepository(Configuration.GetConnectionString("Default")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

            app.SeedUserStoreForDashboard();
        }
    }
}
