using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.Respositories
{
    /// <summary>
    /// Represents entity that contains all repositories that have access to database entities. Implements patter unit of work.
    /// </summary>
    public class InnoGotchiUnitOfWork
    {
        private InnoGotchiContext _innoGotchiContext;
        private PetRepository _petRepository;
        private UserRepository _userRepository;
        private FarmRepository _farmRepository;
        private RequestRepository _requestRepository;
        private PictureRepository _pictureRepository;

        public InnoGotchiUnitOfWork(DbContextOptions<InnoGotchiContext> innoGotchiOptions)
        {
            _innoGotchiContext = new InnoGotchiContext(innoGotchiOptions);
            _petRepository = new PetRepository(_innoGotchiContext);
            _farmRepository = new FarmRepository(_innoGotchiContext);
            _userRepository = new UserRepository(_innoGotchiContext);
            _requestRepository = new RequestRepository(_innoGotchiContext);
            _pictureRepository = new PictureRepository(_innoGotchiContext);
        }
        /// <summary>
        /// Returns pet repository
        /// </summary>
        public IRepository<Pet> Pets
        {
            get => _petRepository;
        }
        /// <summary>
        /// Returns user repository
        /// </summary>
        public IRepository<User> Users
        {
            get => _userRepository;
        }
        /// <summary>
        /// Returns pictures repository
        /// </summary>
        public IRepository<Picture> Pictures
        {
            get => _pictureRepository;
        }
        /// <summary>
        /// Returns coloboration requests repository
        /// </summary>
        public IRepository<ColoborationRequest> Requests
        {
            get => _requestRepository;
        }
        /// <summary>
        /// Returns farm repository
        /// </summary>
        public IRepository<Farm> Farms
        {
            get => _farmRepository;
        }
        /// <summary>
        /// Saves changes in database
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _innoGotchiContext.SaveChangesAsync();
        }

        public void Detach(object obj)
        {
            _innoGotchiContext.Entry(obj).State = EntityState.Detached;
        }
    }
}
