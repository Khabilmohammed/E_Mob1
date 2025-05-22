using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using E_mob_shoppy.DataAccess.Data;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace E_mob_shoppy
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
            // Services are configured in Program.cs
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Middleware is configured in Program.cs
        }

        // This method is used to warm up the application during startup
        public static async Task WarmupApplication(IServiceProvider serviceProvider, ILogger logger)
        {
            try
            {
                // Warm up the database connection
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    
                    // Execute a simple query to establish connection
                    var connectionIsReady = await dbContext.Database.CanConnectAsync();
                    
                    if (connectionIsReady)
                    {
                        logger.LogInformation("Database connection successfully established during warmup");
                    }
                    else
                    {
                        logger.LogWarning("Failed to establish database connection during warmup");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during application warmup");
            }
        }
    }
}
