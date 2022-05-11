using AutoMapper;
using IdentityServiceBLL.Exceptions;
using IdentityServiceBLL.Models;
using IdentityServiceDAL.Repositories;

namespace IdentityServiceBLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserModel> ValidateUserCredentials(UserLoginModel model)
        {
            var user = await repository.ValidateUserCredentials(model.UserName, model.Password);

            if (user == null) 
            {
                throw new UserNotFoundException("There is no user with provided credentials in the database.");
            }

            return mapper.Map<UserModel>(user);
        }

    }
}
