using E_mob_shoppy.DataAccess.Data;
using E_mob_shoppy.DataAccess.Repository;
using E_mob_shoppy.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using E_mob_shoppy.Utility;
using Stripe;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

using E_mob_shoppy.DataAccess.DbInitializer;

namespace E_mob_shoppy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Application Insights
            builder.Services.AddApplicationInsightsTelemetry();

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
                options.ClientId = "454370760301-hupnkkjm6vb4k2s6nqjrp5sjj3kt5f7v.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Increased from 5 minutes to 30 minutes
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
            app.UseRouting();

            app.UseSession(); // Ensure this is before Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            SeedDatabase();

            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            // Warm up the application to prevent cold start issues
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            Task.Run(async () =>
            {
                try
                {
                    logger.LogInformation("Starting application warmup process");
                    await Startup.WarmupApplication(app.Services, logger);
                    logger.LogInformation("Application warmup completed successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error during application warmup");
                }
            });

            app.Run();

            void SeedDatabase()
            {
                try
                {
                    using (var scope = app.Services.CreateScope())
                    {
                        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                        dbInitializer.Initialize();
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error during database seeding: {ex.Message}");
                    // In a production environment, you might want to use a more robust logging solution
                }
            }
        }
    }
}
