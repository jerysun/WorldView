using Cities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cities.Repositories
{
    public class CityRepo : ICityRepo
    {
        private readonly CityContext _context;

        public CityRepo(CityContext context)
        {
            _context = context;
        }

        public void Add<T>(T enitty) where T : class
        {
            _context.Add(enitty);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<City> GetCityByNamesAsync(string name, string countryCode)
        {
            if (string.IsNullOrEmpty(countryCode))
            {
                return await _context.Cities.FirstOrDefaultAsync(ct => ct.Name.Equals(name));
            }
            else
            {
                /*return await _context.Cities.FirstOrDefaultAsync(ct => string.Compare(ct.Name, name, true) == 0
                    && string.Compare(ct.Alpha2Code, countryCode, true) == 0);*/
                return await _context.Cities.FirstOrDefaultAsync(ct => ct.Name.Equals(name) && ct.Alpha2Code.Equals(countryCode));
            }
        }

        public async Task<City> GetCityByIdAsync(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            return city;
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            return await _context.Cities.ToListAsync();
        }
    }
}
