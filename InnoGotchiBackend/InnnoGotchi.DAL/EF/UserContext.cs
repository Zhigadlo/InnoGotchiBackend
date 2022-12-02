using InnnoGotchi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.EF
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
