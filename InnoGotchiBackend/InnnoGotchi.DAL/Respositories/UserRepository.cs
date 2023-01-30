using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.Respositories
{
    /// <summary>
    /// Represents repository that has full access to user entities in database
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        private InnoGotchiContext _context;
        public UserRepository(InnoGotchiContext context)
        {
            _context = context;
        }

        public bool Contains(Func<User, bool> predicate)
        {
            User? user = FirstOrDefault(predicate);
            if (user == null)
                return false;
            else
                return true;
        }

        public void Create(User item)
        {
            _context.Users.Add(item);
        }

        public bool Delete(int id)
        {
            User? user = Get(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                return true;
            }
            return false;
        }

        public IQueryable<User> FindAll(Func<User, bool> predicate)
        {
            return GetAll().Where(predicate).AsQueryable();
        }

        public User? FirstOrDefault(Func<User, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public User? Get(int id)
        {
            return FirstOrDefault(u => u.Id == id);
        }

        public IQueryable<User> GetAll()
        {
            return _context.Users.Include(u => u.CollaboratedFarms)
                                 .Include(u => u.SentRequests)
                                 .Include(u => u.ReceivedRequests)
                                 .Include(u => u.Farm);
        }

        public void Update(User item)
        {
            _context.Users.Update(item);
        }
    }
}
