using Cities.Helpers;
using Cities.Models;
using Cities.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<AppUser> GetUser(int id, bool isCurrentUser)
        {
            var query = _userRepo.GetUser(id);
            if (isCurrentUser)
                query = query.IgnoreQueryFilters();

            var appUser = await query.FirstOrDefaultAsync(u => u.Id == id);
            return appUser;
        }

        public async Task<List<AppUser>> GetUsers(UserParams userParams)
        {
            var source = _userRepo.GetUsers();
            //source = (IOrderedQueryable<AppUser>)source.Where(u => u.Id != userParams.UserId);

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch(userParams.OrderBy)
                {
                    case "created":
                        source = source.OrderByDescending(u => u.Created);
                        break;
                    default:
                        break;
                }
            }

            var appUsers = await source.ToListAsync();
            return appUsers;
        }
    }
}
