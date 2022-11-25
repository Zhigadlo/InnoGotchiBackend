using InnnoGotchi.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InnnoGotchi.DAL.EF
{
    public class InnoGotchiContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Farm> Farms { get; set; }

        
        public InnoGotchiContext(DbContextOptions<InnoGotchiContext> options) : base(options)
        {

        }
    }
}
