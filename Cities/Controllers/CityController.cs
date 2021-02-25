using AutoMapper;
using Cities.Dtos;
using Cities.Helpers;
using Cities.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Cities.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace Cities.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepo _repo;
        private readonly ICityService _service;
        private readonly IMapper _mapper;
        //private readonly ILogger<CityController> _logger;

        public CityController(/*ILogger<CityController> logger, */ICityRepo repo, ICityService service, IMapper mapper)
        {
            //_logger = logger;
            _repo = repo;
            _service = service;
            _mapper = mapper;
        }

        // GET	/api/v1/city?name={city name}&code={country code}
        [HttpGet]
        public async Task<IActionResult> GetCityByNameAndCode(string name, string code)
        {
            var cityFromRepo = await _service.GetCityByNameAndCodeAsync(name, code);
            if (cityFromRepo == null)
                return NotFound();

            cityFromRepo.Weather = InfraData.GetCityWeather(name, code);
            return Ok(cityFromRepo);
        }

        // GET	/api/v1/city
        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("listall")]
        public async Task<IActionResult> ListCities()
        {
            var cities = await _service.ListCitiesAsync();

            if (cities != null)
            {
                foreach (var city in cities)
                    city.Weather = InfraData.GetCityWeather(city.Name, city.Alpha2Code);

                return Ok(cities);
            }

            return NotFound();
        }

        // POST	/api/v1/city
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPost]
        public async Task<IActionResult> UploadCity([FromBody] CityUploadDto cityUploadDto)
        {
            if (cityUploadDto == null || !InfraData.CountriesNames.Contains(cityUploadDto.Country))
                return BadRequest("The country name is incorrect!");

            var result = await _service.UploadCityAsync(cityUploadDto);
            if (result.UploadSuccessful) return CreatedAtAction(nameof(GetCityByNameAndCode), result.CityFromMap);

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        // PUT /api/v1/city/3
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCity(int id, CityUpdateDto cityUpdateDto)
        {
            if (cityUpdateDto == null) return BadRequest();

            var cityFromRepo = await _service.GetCityByIdAsync(id);
            if (cityFromRepo == null) return NotFound();

            string country = cityUpdateDto.Country;
            if (!InfraData.CountriesNames.Contains(country)
                || !InfraData.CountriesCodes[country].Alpha2Code.Equals(cityUpdateDto.Alpha2Code)
                || !InfraData.CountriesCodes[country].Alpha3Code.Equals(cityUpdateDto.Alpha3Code)
                || !InfraData.CountriesCodes[country].CurrenciesCode.Equals(cityUpdateDto.CurrenciesCode)
                )
                return BadRequest("Country or its codes or currency is invalid!");

            _mapper.Map<CityUpdateDto, City>(cityUpdateDto, cityFromRepo);

            if (await _service.SaveChangesAsync())
                return NoContent();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // PATCH /api/v1/city/3
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchCity(int id, [FromBody] JsonPatchDocument<City> patchEntity)
        {
            if (patchEntity == null) return BadRequest();

            var cityFromRepo = await _service.GetCityByIdAsync(id);
            if (cityFromRepo == null) return NotFound();

            patchEntity.ApplyTo(cityFromRepo, ModelState);
            await _service.SaveChangesAsync();
            return Ok(cityFromRepo);
        }

        // DELETE /api/v1/city/3
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var cityFromRepo = await _service.GetCityByIdAsync(id);
            if (cityFromRepo == null) return NotFound();

            if (await _service.DeleteCityAsync(cityFromRepo)) return NoContent();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
