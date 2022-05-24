using IdentityServiceDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityServiceDAL.DbContexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
    }
}
