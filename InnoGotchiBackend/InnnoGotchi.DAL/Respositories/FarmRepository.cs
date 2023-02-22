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

        public bool Contains(Func<Farm, bool> predicate)
        {
            Farm? farm = FirstOrDefault(predicate);
            if (farm == null)
                return false;
            else
                return true;
        }

        public void Create(Farm item)
        {
            _context.Farms.Add(item);
        }

        public bool Delete(int id)
        {
            Farm? farm = Get(id);
            if (farm != null)
            {
                _context.Farms.Remove(farm);
                return true;
            }
            else
                return false;
        }

        public IEnumerable<Farm> FindAll(Func<Farm, bool> expression)
        {
            return GetAll().Where(expression);
        }

        public Farm? FirstOrDefault(Func<Farm, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public Farm? Get(int id)
        {
            return FirstOrDefault(f => f.Id == id);
        }

        public IQueryable<Farm> GetAll()
        {
            return _context.Farms.Include(f => f.Owner)
                                 .Include(f => f.Pets);
        }

        public void Update(Farm item)
        {
            _context.Farms.Update(item);
        }
    }
}
