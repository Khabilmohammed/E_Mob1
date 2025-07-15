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
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)
                ));

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

               /* builder.Services.AddAuthentication().AddGoogle(options =>
                {
                    options.ClientId = "xxx"; // Replace with real values from config
                    options.ClientSecret = "xxx";
                });*/

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
                string logPath = "D:\\home\\LogFiles\\startup-log.txt";
                System.IO.File.AppendAllText(logPath, $"App started at {DateTime.Now}\n");
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
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            int retryCount = 0;
            const int maxRetries = 5;

            while (true)
            {
                try
                {
                    dbInitializer.Initialize();
                    break; // success
                }
                catch (Exception ex)
                {
                    retryCount++;
                    System.IO.File.AppendAllText("D:\\home\\site\\wwwroot\\seed-error.txt", $"Retry {retryCount}: {ex.Message}\n");

                    if (retryCount >= maxRetries)
                        throw;

                    Thread.Sleep(3000); // wait 3 seconds and retry
                }
            }
        }

    }

}