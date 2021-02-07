using Cities.Dtos;
using Cities.Helps;
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
        Task<IEnumerable<City>> ListCitiesAsync();
        Task<City> UpdateCityAsync(int id);
        Task<City> PatchCityAsync(int id);
        Task DeleteCityAsync(int id);
    }
}
