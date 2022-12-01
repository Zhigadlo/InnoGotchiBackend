using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;

namespace InnnoGotchi.DAL.Respositories
{
    public class UserRepository : IRepository<User>
    {
        private UserContext _context;
        public UserRepository(UserContext context) 
        {
            _context = context;
        }
        
        public void Create(User item)
        {
            _context.Users.Add(item);
        }

        public void Delete(int id)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return _context.Users.Where(predicate);
        }

        public User? First(Func<User, bool> predicate)
        {
            return _context.Users.FirstOrDefault(predicate);
        }

        public User? Get(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public void Update(int id, User item)
        {
            item.Id = id;
            _context.Users.Update(item);
        }
    }
}
