using IdentityServiceBLL.Models;

namespace IdentityServiceBLL.Services
{
    public interface IUserService
    {
        Task<UserModel> ValidateUserCredentials(UserLoginModel model);
    }
}
