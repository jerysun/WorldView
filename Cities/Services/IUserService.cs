﻿using Cities.Helpers;
using Cities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Services
{
    public interface IUserService
    {
        Task<List<AppUser>> GetUsers(UserParams userParams);
        Task<AppUser> GetUser(int id, bool isCurrentUser);
    }
}
