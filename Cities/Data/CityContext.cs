using Microsoft.EntityFrameworkCore;

namespace Cities.Data
{
    public class CityContext : DbContext
    {
        public CityContext(DbContextOptions<CityContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
    }
}
