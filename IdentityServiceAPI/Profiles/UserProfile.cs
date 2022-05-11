using AutoMapper;
using IdentityServiceAPI.Models;
using IdentityServiceBLL.Models;

namespace IdentityServiceAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<UserLoginDto, UserLoginModel>().ReverseMap();

            CreateMap<UserDto, UserModel>().ReverseMap();
        }
    }
}
