using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.Respositories
{
    public class InnogGotchiUnitOfWork : IUnitOfWork
    {
        private InnoGotchiContext _context;
        private PetRepository _petRepository;
        private UserRepository _userRepository;
        private FarmRepository _farmRepository;

        public InnogGotchiUnitOfWork(DbContextOptions<InnoGotchiContext> options)
        {
            _context = new InnoGotchiContext(options);
            _petRepository = new PetRepository(_context);
            _farmRepository = new FarmRepository(_context);
            _userRepository = new UserRepository(_context);
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
            _context.SaveChanges();
        }
    }
}
