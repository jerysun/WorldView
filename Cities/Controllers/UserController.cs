using AutoMapper;
using Cities.Dtos;
using Cities.Helpers;
using Cities.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, ILogger logger, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        //public async Task<IActionResult> GetUsers([FromQuery(Name = "pageNumber")] int pageNumber, [FromQuery(Name = "pageSize")] int pageSize) // ASP.NET way
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)//asp.net core way, a handy way for BIG queryStrings
        {
            var users = await _userService.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForDisplayDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUser(id, false);
            var userToReturn = _mapper.Map<UserForDisplayDto>(user);
            return Ok(userToReturn);
        }

    }
}
