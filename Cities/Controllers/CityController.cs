using AutoMapper;
using Cities.Dtos;
using Cities.Helps;
using Cities.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Cities.Services;

namespace Cities.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepo _repo;
        private readonly ICityService _service;
        private readonly ILogger<CityController> _logger;

        public CityController(ILogger<CityController> logger, ICityRepo repo, ICityService service)
        {
            _logger = logger;
            _repo = repo;
            _service = service;
        }

        // GET	/api/v1/city?name={city name}&code={country code}
        [HttpGet("city")]
        public async Task<IActionResult> GetCityByNameAndCode(string name, string code)
        {
            var cityFromRepo = await _service.GetCityByNameAndCodeAsync(name, code);
            if (cityFromRepo == null)
                return NotFound();

            cityFromRepo.Weather = InfraData.GetCityWeather(name, code);
            return Ok(cityFromRepo);
        }

        [HttpPost("city")]
        // POST	/api/v1/city
        public async Task<IActionResult> UploadCity([FromBody] CityUploadDto cityUploadDto)
        {
            if (!InfraData.CountriesNames.Contains(cityUploadDto?.Country))
                return BadRequest("The country name is incorrect!");

            var result = await _service.UploadCityAsync(cityUploadDto);
            if (result.UploadSuccessful) return CreatedAtAction(nameof(GetCityByNameAndCode), result.CityFromMap);

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
