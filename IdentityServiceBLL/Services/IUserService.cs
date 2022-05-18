using IdentityServiceBLL.Models;

namespace IdentityServiceBLL.Services
{
    public interface IUserService
    {
        Task<string> FindUser(string username);
        Task<UserModel> ValidateUserCredentials(UserLoginModel model);
        Task<UserModel> RegisterUser(UserRegistrationModel model);
    }
}
