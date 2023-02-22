using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;

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
            else
                return true;
        }

        public void Create(Pet item)
        {
            _context.Pets.Add(item);
        }

        public bool Delete(int id)
        {
            Pet? pet = Get(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
                return true;
            }
            return false;
        }

        public IEnumerable<Pet> FindAll(Func<Pet, bool> expression)
        {
            return AllItems().Where(expression);
        }

        public Pet? Get(int id)
        {
            return AllItems().FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Pet> AllItems()
        {
            return _context.Pets;
        }

        public void Update(Pet item)
        {
            _context.ChangeTracker.Clear();
            _context.Pets.Update(item);
        }
    }
}
