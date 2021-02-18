using Cities.Dtos;
using Cities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Services
{
    public interface ICityService
    {
        Task<City> GetCityByNameAndCodeAsync(string name, string countryCode);
        Task<ReturnedUpload> UploadCityAsync(CityUploadDto cityUploadDto);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<City>> ListCitiesAsync();
        Task<City> GetCityByIdAsync(int id);
        Task<bool> DeleteCityAsync(City city);
    }
}
