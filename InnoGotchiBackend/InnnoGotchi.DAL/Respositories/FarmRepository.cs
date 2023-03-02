using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public bool Contains(Func<Farm, bool> predicate)
        {
            Farm? farm = AllItems().FirstOrDefault(predicate);
            if (farm == null)
                return false;

            return true;
        }

        public void Create(Farm item)
        {
            _context.Farms.Add(item);
        }

        public bool Delete(int id)
        {
            Farm? farm = Get(id);
            if (farm == null)
                return false;

            _context.Farms.Remove(farm);
            return true;
        }

        public IEnumerable<Farm> FindAll(Func<Farm, bool> expression, bool isTracking = false)
        {
            return AllItems(isTracking).Where(expression);
        }

        public Farm? Get(int id, bool isTracking = false)
        {
            return AllItems(isTracking).FirstOrDefault(f => f.Id == id);
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
