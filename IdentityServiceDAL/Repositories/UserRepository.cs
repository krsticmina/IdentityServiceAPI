using IdentityServiceDAL.DbContexts;
using IdentityServiceDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityServiceDAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext context;

        public UserRepository(UserDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User?> ValidateUserCredentials(string username, string password)
        {
            return await context.Users.Where(u => u.UserName.Equals(username) && u.Password.Equals(password)).SingleOrDefaultAsync();
        }

    }
}
