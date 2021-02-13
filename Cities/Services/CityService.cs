using AutoMapper;
using Cities.Dtos;
using Cities.Helps;
using Cities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepo _cityRepo;
        private readonly IMapper _mapper;

        public CityService(ICityRepo cityRepo, IMapper mapper)
        {
            _cityRepo = cityRepo;
            _mapper = mapper;
        }

        public async Task<City> GetCityByNameAndCodeAsync(string name, string countryCode)
        {
            return await _cityRepo.GetCityByNamesAsync(name, countryCode);
        }

        public async Task<IEnumerable<City>> ListCitiesAsync()
        {
            return await _cityRepo.GetAllCitiesAsync();
        }

        public async Task<ReturnedUpload> UploadCityAsync(CityUploadDto cityUploadDto)
        {
            var cityFromMap = _mapper.Map<City>(cityUploadDto);
            cityFromMap.Alpha2Code = InfraData.CountriesCodes[cityUploadDto.Country].Alpha2Code;
            cityFromMap.Alpha3Code = InfraData.CountriesCodes[cityUploadDto.Country].Alpha3Code;
            cityFromMap.CurrenciesCode = InfraData.CountriesCodes[cityUploadDto.Country].CurrenciesCode;

            _cityRepo.Add(cityFromMap);
            bool successful = await _cityRepo.SaveChangesAsync();

            ReturnedUpload result = new ReturnedUpload
            {
                CityFromMap = cityFromMap,
                UploadSuccessful = successful
            };

            return result;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _cityRepo.SaveChangesAsync();
        }

        public async Task<City> GetCityByIdAsync(int id)
        {
            return await _cityRepo.GetCityByIdAsync(id);
        }

        public async Task<bool> DeleteCityAsync(City city)
        {
            _cityRepo.Delete(city);
            return await SaveChangesAsync();
        }
    }
}
