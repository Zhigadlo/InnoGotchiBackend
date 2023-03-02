using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<bool> ContainsAsync(Expression<Func<User, bool>> expression)
        {
            User? user = await AllItems().FirstOrDefaultAsync(expression);
            if (user == null)
                return false;

            return true;
        }

        public void Create(User item)
        {
            _context.Users.Add(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            User? user = await GetAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            return true;
        }

        public IEnumerable<User> FindAll(Func<User, bool> expression, bool isTracking = true)
        {
            return AllItems(isTracking).Where(expression);
        }

        public async Task<User?> GetAsync(int id, bool isTracking = true)
        {
            return await AllItems(isTracking).FirstOrDefaultAsync(u => u.Id == id);
        }

        public IQueryable<User> AllItems(bool isTracking = true)
        {
            var items = _context.Users.Include(u => u.CollaboratedFarms).Include(u => u.SentRequests).Include(u => u.ReceivedRequests).Include(u => u.Farm);
            return isTracking ? items : items.AsNoTracking();
        }

        public void Update(User item)
        {
            _context.Users.Update(item);
        }
    }
}
