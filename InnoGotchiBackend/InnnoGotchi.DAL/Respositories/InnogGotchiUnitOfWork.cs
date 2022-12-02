using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.Respositories
{
    public class InnogGotchiUnitOfWork : IUnitOfWork
    {
        private InnoGotchiContext _innoGotchiContext;
        private UserContext _userContext;
        private PetRepository _petRepository;
        private UserRepository _userRepository;
        private FarmRepository _farmRepository;

        public InnogGotchiUnitOfWork(DbContextOptions<InnoGotchiContext> innoGotchiOptions, DbContextOptions<UserContext> userOptions)
        {
            _innoGotchiContext = new InnoGotchiContext(innoGotchiOptions);
            _userContext = new UserContext(userOptions);
            _petRepository = new PetRepository(_innoGotchiContext);
            _farmRepository = new FarmRepository(_innoGotchiContext);
            _userRepository = new UserRepository(_userContext);
        }
        public IRepository<Pet> Pets
        {
            get => _petRepository;
        }

        public IRepository<User> Users
        {
            get => _userRepository;
        }

        public IRepository<Farm> Farms
        {
            get => _farmRepository;
        }

        public void SaveChanges()
        {
            _innoGotchiContext.SaveChanges();
        }
    }
}
