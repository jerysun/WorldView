using AutoMapper;
using Cities.Data;
using Cities.Dtos;
using Cities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly CityContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public AdminController(CityContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await _context.Users
                .OrderBy(u => u.UserName)
                .Select(user => new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = (from userRole in user.UserRoles
                             join role in _context.Roles
                             on userRole.RoleId equals role.Id
                             select role.Name).ToList()
                }).ToListAsync();

            if (userList == null) return NotFound();
            return Ok(userList);
        }

        //api/v1/admin/editRoles/userName
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            //in case the end user deletes all of his roles, so it could be null
            var selectedRoles = roleEditDto.RoleNames;
            selectedRoles = selectedRoles ?? new string[] { };

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!result.Succeeded)
                return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded)
                return BadRequest("Failed to remove the roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("role/{id}", Name = "GetRole")]
        public async Task<IActionResult> GetRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null) return Ok(role);
            return NotFound();
        }

        //api/v1/admin/role
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("role")]
        public async Task<IActionResult> Create(AppRole appRole)
        {
            var result = await _roleManager.CreateAsync(appRole);
            if (result.Succeeded)
            {
                return CreatedAtRoute("GetRole", new { id = appRole.Id }, appRole);
            }
            return BadRequest(result.Errors);
        }

        //api/v1/admin/role/2
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("role/{id}")]
        public async Task<IActionResult> UpdateRole(string id, RoleForUpdateDto roleForUpdateDto)
        {
            var roleFromRepo = await _roleManager.FindByIdAsync(id);
            if (roleFromRepo == null) return NotFound();

            _mapper.Map<RoleForUpdateDto, AppRole>(roleForUpdateDto, roleFromRepo);

            var result = await _roleManager.UpdateAsync(roleFromRepo);
            if (!result.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError);

            return NoContent();
        }

        //api/v1/admin/role/3
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("role/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var roleFromRepo = await _roleManager.FindByIdAsync(id);
            if (roleFromRepo == null) return NotFound();

            var result = await _roleManager.DeleteAsync(roleFromRepo);
            if (!result.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError);

            return NoContent();
        }
    }
}
