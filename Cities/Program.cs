using Cities.Data;
using Cities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                var logger = service.GetRequiredService<ILogger<Program>>();

                try
                {
                    var context = service.GetRequiredService<CityContext>();
                    var userManager = service.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = service.GetRequiredService<RoleManager<AppRole>>();
                    context.Database.Migrate();
                    Seed.SeedUsers(userManager, roleManager);
                    logger.LogInformation("User tables are created.");

                    Seed.SeedCities(service);
                    logger.LogInformation("City table is created.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured during migration.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
