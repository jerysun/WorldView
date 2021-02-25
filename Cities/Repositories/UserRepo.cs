using Cities.Data;
using Cities.Helpers;
using Cities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly CityContext _context;

        public UserRepo(CityContext context)
        {
            _context = context;
        }

        public IQueryable<AppUser> GetUser(int id)
        {
            return _context.Users.AsQueryable();
        }

        public IQueryable<AppUser> GetUsers()
        {
            return _context.Users.OrderByDescending(u => u.LastActive).AsQueryable();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
