using IdentityServiceDAL.Entities;

namespace IdentityServiceDAL.Repositories
{
    public interface IUserRepository
    {
        Task<User?> ValidateUserCredentials(string username, string password);
    }
}
