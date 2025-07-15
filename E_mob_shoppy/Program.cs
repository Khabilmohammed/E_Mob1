using E_mob_shoppy.DataAccess.Data;
using E_mob_shoppy.DataAccess.Repository;
using E_mob_shoppy.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using E_mob_shoppy.Utility;
using Stripe;

using E_mob_shoppy.DataAccess.DbInitializer;

namespace E_mob_shoppy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Services.AddControllersWithViews();
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

                builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                builder.Services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = $"/Identity/Account/Login";
                    options.LogoutPath = $"/Identity/Account/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                });

                builder.Services.AddAuthentication().AddGoogle(options =>
                {
                    options.ClientId = "xxx"; // Replace with real values from config
                    options.ClientSecret = "xxx";
                });

                builder.Services.AddDistributedMemoryCache();
                builder.Services.AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromSeconds(300);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                });

                builder.Services.AddScoped<IDbInitializer, DbInitializer>();
                builder.Services.AddRazorPages();
                builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
                builder.Services.AddScoped<IEmailSender, EmailSender>();

                var app = builder.Build();

                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();
                StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
                app.UseRouting();

                app.UseSession();
                app.UseAuthentication();
                app.UseAuthorization();

                SeedDatabase(app);

                app.MapRazorPages();
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
                System.IO.File.AppendAllText("D:\\home\\site\\wwwroot\\log.txt", "App started at " + DateTime.Now + "\n");
                app.Run();
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("D:\\home\\site\\wwwroot\\startup-error.txt", ex.ToString());
                throw;
            }
        }

        private static void SeedDatabase(WebApplication app)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.Initialize();
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("D:\\home\\site\\wwwroot\\seed-error.txt", ex.ToString());
                throw;
            }
        }
    }

}