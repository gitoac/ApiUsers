using Microsoft.EntityFrameworkCore;

namespace ExampleWebApiUser.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> UserItems { get; set; }
        public DbSet<Address> AddressItems { get; set; }
    }
}
