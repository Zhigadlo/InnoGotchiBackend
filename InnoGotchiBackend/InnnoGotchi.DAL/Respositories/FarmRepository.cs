using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnnoGotchi.DAL.Respositories
{
    /// <summary>
    /// Represents repository that has full access to farm entities in database
    /// </summary>
    public class FarmRepository : IRepository<Farm>
    {
        private InnoGotchiContext _context;
        public FarmRepository(InnoGotchiContext context)
        {
            _context = context;
        }

        public async Task<bool> ContainsAsync(Expression<Func<Farm, bool>> expression)
        {
            Farm? farm = await AllItems().FirstOrDefaultAsync(expression);
            if (farm == null)
                return false;

            return true;
        }

        public void Create(Farm item)
        {
            _context.Farms.Add(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Farm? farm = await GetAsync(id);
            if (farm == null)
                return false;

            _context.Farms.Remove(farm);
            return true;
        }

        public IEnumerable<Farm> FindAll(Func<Farm, bool> expression, bool isTracking = false)
        {
            return AllItems(isTracking).Where(expression);
        }

        public async Task<Farm?> GetAsync(int id, bool isTracking = false)
        {
            return await AllItems(isTracking).FirstOrDefaultAsync(f => f.Id == id);
        }

        public IQueryable<Farm> AllItems(bool isTracking = true)
        {
            var items = _context.Farms.Include(f => f.Owner).Include(f => f.Pets);
            return isTracking ? items : items.AsNoTracking();
        }

        public void Update(Farm item)
        {
            _context.Farms.Update(item);
        }
    }
}
