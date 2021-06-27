using Cities.Data;
using Cities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Cities
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            //Initializer logger 
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
            Log.Information("Application Starting.##############");

            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;

                try
                {
                    var context = service.GetRequiredService<CityContext>();
                    var userManager = service.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = service.GetRequiredService<RoleManager<AppRole>>();
                    context.Database.Migrate();
                    Seed.SeedUsers(userManager, roleManager);
                    Log.Information("User tables are created.");

                    Seed.SeedCities(service);
                    Log.Information("City table is created.");
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occurred during migration.");
                }
            }

            host.Run();
            Log.CloseAndFlush();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
