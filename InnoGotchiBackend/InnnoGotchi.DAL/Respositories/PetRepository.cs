using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnnoGotchi.DAL.Respositories
{
    /// <summary>
    /// Represents repository that has full access to pet entities in database
    /// </summary>
    public class PetRepository : IRepository<Pet>
    {
        private InnoGotchiContext _context;
        public PetRepository(InnoGotchiContext context)
        {
            _context = context;
        }

        public async Task<bool> ContainsAsync(Expression<Func<Pet, bool>> expression)
        {
            Pet? pet = await AllItems().FirstOrDefaultAsync(expression);
            if (pet == null)
                return false;

            return true;
        }

        public void Create(Pet item)
        {
            _context.Pets.Add(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Pet? pet = await GetAsync(id);
            if (pet == null)
                return false;

            _context.Pets.Remove(pet);
            return true;
        }

        public IEnumerable<Pet> FindAll(Func<Pet, bool> expression, bool isTracking = true)
        {
            return AllItems(isTracking).Where(expression);
        }

        public async Task<Pet?> GetAsync(int id, bool isTracking = true)
        {
            return await AllItems(isTracking).FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Pet> AllItems(bool isTracking = true)
        {
            var items = _context.Pets;
            return isTracking ? items : items.AsNoTracking();
        }

        public void Update(Pet item)
        {
            _context.Pets.Update(item);
        }
    }
}
