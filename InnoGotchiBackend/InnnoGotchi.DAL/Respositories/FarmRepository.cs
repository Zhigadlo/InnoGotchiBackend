using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.Respositories
{
    public class FarmRepository : IRepository<Farm>
    {
        private InnoGotchiContext _context;
        public FarmRepository(InnoGotchiContext context)
        {
            _context = context;
        }

        public void Create(Farm item)
        {
            _context.Farms.Add(item);
        }

        public void Delete(int id)
        {
            Farm? farm = _context.Farms.FirstOrDefault();
            if (farm != null)
            {
                _context.Farms.Remove(farm);
            }
        }

        public IEnumerable<Farm> Find(Func<Farm, bool> predicate)
        {
            return _context.Farms.Include(f => f.Owner).Include(f => f.Pets).Where(predicate);
        }

        public Farm? First(Func<Farm, bool> predicate)
        {
            return _context.Farms.Include(f => f.Owner).Include(f => f.Pets).FirstOrDefault(predicate);
        }

        public Farm? Get(int id)
        {
            return _context.Farms.Include(f => f.Owner).Include(f => f.Pets).FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<Farm> GetAll()
        {
            return _context.Farms.Include(f => f.Owner).Include(f => f.Pets);
        }

        public void Update(Farm item)
        {
            _context.Farms.Update(item);
        }
    }
}
