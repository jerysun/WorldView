using Cities.Helpers;
using Cities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Repositories
{
    public interface IUserRepo
    {
        IQueryable<AppUser> GetUser(int id);
        IQueryable<AppUser> GetUsers();
        Task<bool> SaveAll();
    }
}
