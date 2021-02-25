using AutoMapper;
using Cities.Dtos;
using Cities.Helpers;
using Cities.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cities.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        //private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, /*ILogger logger, */IMapper mapper)
        {
            _userService = userService;
            //_logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        //public async Task<IActionResult> GetUsers([FromQuery(Name = "pageNumber")] int pageNumber, [FromQuery(Name = "pageSize")] int pageSize) // ASP.NET way
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)//asp.net core way, a handy way for BIG queryStrings
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            userParams.UserId = currentUserId;

            var users = await _userService.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForDisplayDto>>(users);

            // Add pagination to the HttpResponse Headers. Because we're in Controller
            // so we have access to Response, furhtermore we added an extension method
            // to HttpResponse - AddPagination() in Helpers\Extensions.cs
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.Totalpages);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUser(id, false);
            var userToReturn = _mapper.Map<UserForDisplayDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            //check if the passed id = the one from the decodedToken, and if the guy putting
            //the update is the current user(associated with the executing action): Only you
            //can update your own information!
            //ClaimsPrincipal ControllerBase.User {get;}
            //Get the ClaimsPrincipal
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _userService.GetUser(id, true);
            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _userService.SaveAll())
                return NoContent();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
