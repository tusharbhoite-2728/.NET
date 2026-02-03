using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using IdentityMVCApp.Data;
using IdentityMVCApp.Models;
using IdentityMVCApp.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;


namespace IdentityMVCApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // DbContext (MySQL)
            var cs = builder.Configuration.GetConnectionString("MySqlConnection")!;
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseMySql(cs, ServerVersion.AutoDetect(cs));
            });

            // Identity (Cookie auth for MVC pages)
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthorization();

            // services
          

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("UserAdd", p => p.RequireClaim("permission", "user.add"));
                options.AddPolicy("UserEdit", p => p.RequireClaim("permission", "user.edit"));
            });


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";

                options.ExpireTimeSpan = TimeSpan.FromMinutes(24);

                //  Do NOT extend lifetime on activity
                options.SlidingExpiration = false;

                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            builder.Services.AddAuthentication()
            .AddGoogle(options =>
              {
                // Load credentials from appsettings.json
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
               });



            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Seed roles/admin
            using (var scope = app.Services.CreateScope())
            {
                await IdentitySeeder.SeedAsync(scope.ServiceProvider);
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
