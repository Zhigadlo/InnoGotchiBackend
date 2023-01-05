using InnnoGotchi.DAL.Entities;

namespace InnnoGotchi.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Pet> Pets { get; }
        IRepository<User> Users { get; }
        IRepository<Picture> Pictures { get; }
        IRepository<Farm> Farms { get; }
        IRepository<ColoborationRequest> Requests { get; }
        void SaveChanges();
    }
}
