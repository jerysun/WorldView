using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Cities.Models;

namespace Cities.Data
{
    public class Seed
    {
        public static void SeedCities(IServiceProvider serviceProvider)
        {
            using (var context = new CityContext(serviceProvider.GetRequiredService<DbContextOptions<CityContext>>()))
            {
                if (context.Cities.Any()) return;

                var seedData = File.ReadAllText("Data\\CitySeedData.json");
                var cities = JsonConvert.DeserializeObject<List<City>>(seedData);
                context.Cities.AddRange(cities);
                context.SaveChanges();
            }
        }

        public static void SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            const string PASSWORD = "Pa55Word!";
            const string ADMIN = "Admin";

            if (!userManager.Users.Any())
            {
                var userData = File.ReadAllText("Data\\UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);

                //Create some roles
                var roles = new List<AppRole>
                {
                    new AppRole{Name = RolesName.Member},
                    new AppRole{Name = RolesName.Admin},
                    new AppRole{Name = RolesName.Moderator},
                    new AppRole{Name = RolesName.VIP}
                };

                foreach(var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }

                //Fill users
                foreach(var user in users)
                {
                    userManager.CreateAsync(user, PASSWORD).Wait();
                    userManager.AddToRoleAsync(user, RolesName.Member).Wait();
                }

                //Create Admin user
                var adminUser = new AppUser
                {
                    UserName = ADMIN
                };

                var result = userManager.CreateAsync(adminUser, PASSWORD).Result;
                
                if(result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync(ADMIN).Result;
                    userManager.AddToRolesAsync(admin, new[] { RolesName.Admin, RolesName.Moderator });
                }
            }
        }
    }
}
