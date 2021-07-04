using AutoMapper;
using Cities.Dtos;
using Cities.Models;

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
            CreateMap<UserForUpdateDto, AppUser>();
            CreateMap<RoleForUpdateDto, AppRole>();
        }
    }
}
