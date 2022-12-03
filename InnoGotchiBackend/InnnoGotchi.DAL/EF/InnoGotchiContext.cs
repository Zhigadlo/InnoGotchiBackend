using InnnoGotchi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.EF
{
    public class InnoGotchiContext : DbContext
    {
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ColoborationRequest> Requests { get; set; }

        public InnoGotchiContext(DbContextOptions<InnoGotchiContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
