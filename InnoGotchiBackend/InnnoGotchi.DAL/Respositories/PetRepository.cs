using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public bool Contains(Func<Pet, bool> predicate)
        {
            Pet? pet = AllItems().FirstOrDefault(predicate);
            if (pet == null)
                return false;

            return true;
        }

        public void Create(Pet item)
        {
            _context.Pets.Add(item);
        }

        public bool Delete(int id)
        {
            Pet? pet = Get(id);
            if (pet == null)
                return false;

            _context.Pets.Remove(pet);
            return true;
        }

        public IEnumerable<Pet> FindAll(Func<Pet, bool> expression, bool isTracking = true)
        {
            return AllItems(isTracking).Where(expression);
        }

        public Pet? Get(int id, bool isTracking = true)
        {
            return AllItems(isTracking).FirstOrDefault(p => p.Id == id);
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
