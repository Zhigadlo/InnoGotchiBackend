using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.Respositories
{
    public class InnoGotchiUnitOfWork : IUnitOfWork
    {
        private InnoGotchiContext _innoGotchiContext;
        private PetRepository _petRepository;
        private UserRepository _userRepository;
        private FarmRepository _farmRepository;
        private RequestRepository _requestRepository;

        public InnoGotchiUnitOfWork(DbContextOptions<InnoGotchiContext> innoGotchiOptions)
        {
            _innoGotchiContext = new InnoGotchiContext(innoGotchiOptions);
            _petRepository = new PetRepository(_innoGotchiContext);
            _farmRepository = new FarmRepository(_innoGotchiContext);
            _userRepository = new UserRepository(_innoGotchiContext);
            _requestRepository = new RequestRepository(_innoGotchiContext);
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

        public IRepository<ColoborationRequest> Requests
        {
            get => _requestRepository;
        }

        public void SaveChanges()
        {
            _innoGotchiContext.SaveChanges();
        }
    }
}
