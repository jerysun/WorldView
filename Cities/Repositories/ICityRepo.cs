using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cities.Repositories
{
    public interface ICityRepo
    {
        void Add<T>(T enitty) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<City> GetCityByNamesAsync(string name, string countryCode);
        Task<City> GetCityByIdAsync(int id);
        Task<IEnumerable<City>> GetAllCitiesAsync();
        
    }
}
