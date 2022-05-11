using AutoMapper;
using IdentityServiceBLL.Models;
using IdentityServiceDAL.Entities;

namespace IdentityServiceBLL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<UserModel, User>().ReverseMap();
        }
    }
}
