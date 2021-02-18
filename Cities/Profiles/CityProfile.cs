using AutoMapper;
using Cities.Dtos;
using Cities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            //source -> target
            CreateMap<CityUploadDto, City>();
            CreateMap<CityUpdateDto, City>();

            CreateMap<UserForRegisterDto, AppUser>();
            CreateMap<AppUser, UserForDisplayDto>();
        }
    }
}
