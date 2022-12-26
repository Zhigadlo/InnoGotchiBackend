using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;

namespace InnnoGotchi.DAL.Respositories
{
    public class PetRepository : IRepository<Pet>
    {
        private InnoGotchiContext _context;
        public PetRepository(InnoGotchiContext context)
        {
            _context = context;
        }

        public bool Contains(Func<Pet, bool> predicate)
        {
            Pet? pet = _context.Pets.FirstOrDefault(predicate);
            if(pet == null)
                return false;
            else
                return true;
        }

        public void Create(Pet item)
        {
            _context.Pets.Add(item);
        }

        public void Delete(int id)
        {
            Pet? pet = _context.Pets.FirstOrDefault(p => p.Id == id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
            }
        }

        public IEnumerable<Pet> Find(Func<Pet, bool> predicate)
        {
            return _context.Pets.Where(predicate);
        }

        public Pet? First(Func<Pet, bool> predicate)
        {
            return _context.Pets.FirstOrDefault(predicate);
        }

        public Pet? Get(int id)
        {
            return _context.Pets.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Pet> GetAll()
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
