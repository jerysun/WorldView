using Cities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cities.Data
{
    public class CityContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>,
        AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public CityContext(DbContextOptions<CityContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUserRole>(appUserRole =>
            {
                // Generate a new key for UserRole by combining UserId and RoleId
                // refer to UserRole.cs which has only 2 related properties
                appUserRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                appUserRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                appUserRole.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            // ur means combination(actually ur is an instance of class AppUserRole)
            // with Helpers HasKey() forms the primary key, so table AppUserRole
            // will have a primary key although we don't create this pro-
            // perty explicitly in AppUserRole.cs.
            // builder.Entity<T>() returns an object that can be used to
            // configure a given entity type in the model. If the entity
            // type is not already part of the model, it will be added to
            // the model.
        }
    }
}
