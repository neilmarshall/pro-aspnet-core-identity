using IdentityApp.User.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddIdentity<IdentityUser<int>, IdentityRole>();
            services.AddTransient<IUserStore<IdentityUser<int>>>(_ =>
                new IdentityUserRepository(Configuration.GetConnectionString("Default")));
            services.AddTransient<IRoleStore<IdentityRole>>(_ =>
                new IdentityRoleRepository(Configuration.GetConnectionString("Default")));

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/SignIn";
                options.LogoutPath = "/Identity/SignOut";
                options.AccessDeniedPath = "/Identity/Forbidden";
            });

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
