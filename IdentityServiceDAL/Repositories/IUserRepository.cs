using IdentityServiceDAL.Entities;

namespace IdentityServiceDAL.Repositories
{
    public interface IUserRepository
    {
        Task<string?> FindUserAndGetSalt(string username);
        Task<User?> ValidateUserCredentials(string username, string password);
        Task RegisterUser(User user);
        Task<bool> SaveChangesAsync();
        Task<User?> FindUserByUsername(string username, string email);
    }
}
