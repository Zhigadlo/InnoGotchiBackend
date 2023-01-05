using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using System.Linq;

namespace InnnoGotchi.DAL.Respositories
{
    public class PictureRepository : IRepository<Picture>
    {
        private InnoGotchiContext _context;

        public PictureRepository(InnoGotchiContext context)
        {
            _context = context;
        }
        public bool Contains(Func<Picture, bool> predicate)
        {
            Picture? picture = _context.Pictures.FirstOrDefault(predicate);
            if (picture == null)
                return false;

            return true;
        }

        public void Create(Picture item)
        {
            _context.Pictures.Add(item);
        }

        public void Delete(int id)
        {
            Picture? picture = _context.Pictures.FirstOrDefault(p => p.Id == id);
            if (picture != null)
                _context.Pictures.Remove(picture);
        }

        public IEnumerable<Picture> Find(Func<Picture, bool> predicate)
        {
            return _context.Pictures.Where(predicate).AsEnumerable();
        }

        public Picture? First(Func<Picture, bool> predicate)
        {
            return _context.Pictures.FirstOrDefault(predicate);
        }

        public Picture? Get(int id)
        {
            return First(p => p.Id == id);
        }

        public IEnumerable<Picture> GetAll()
        {
            return _context.Pictures.AsEnumerable();
        }

        public void Update(Picture item)
        {
            _context.Update(item);
        }
    }
}
