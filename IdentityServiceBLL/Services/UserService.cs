using AutoMapper;
using IdentityServiceBLL.Exceptions;
using IdentityServiceBLL.Models;
using IdentityServiceDAL.Entities;
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

        public async Task<string> FindUser(string username) 
        {
            var salt = await repository.FindUserAndGetSalt(username);

            if (salt == null)
            {
                throw new UserNotFoundException("There is no user with provided username in the database.");
            }

            return salt;
        }

        public async Task<UserModel> RegisterUser(UserRegistrationModel userToAdd) 
        {

            var user = mapper.Map<User>(userToAdd);

            await repository.RegisterUser(user);

            await repository.SaveChangesAsync();

            var employeeToReturn = mapper.Map<UserModel>(user);

            return employeeToReturn;
        }

    }
}
