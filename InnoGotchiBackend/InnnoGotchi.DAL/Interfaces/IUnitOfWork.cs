using InnnoGotchi.DAL.Entities;

namespace InnnoGotchi.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Pet> Pets { get; }
        IRepository<User> Users { get; }
        IRepository<Farm> Farms { get; }
        void SaveChanges();
    }
}
