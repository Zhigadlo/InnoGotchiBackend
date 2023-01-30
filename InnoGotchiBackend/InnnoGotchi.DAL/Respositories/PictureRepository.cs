using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;

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
            Picture? picture = FirstOrDefault(predicate);
            if (picture == null)
                return false;

            return true;
        }

        public void Create(Picture item)
        {
            _context.Pictures.Add(item);
        }

        public bool Delete(int id)
        {
            Picture? picture = Get(id);
            if (picture != null)
            {
                _context.Pictures.Remove(picture);
                return true;
            }
            else
                return false;
        }

        public IQueryable<Picture> FindAll(Func<Picture, bool> predicate)
        {
            return GetAll().Where(predicate).AsQueryable();
        }

        public Picture? FirstOrDefault(Func<Picture, bool> predicate)
        {
            return _context.Pictures.FirstOrDefault(predicate);
        }

        public Picture? Get(int id)
        {
            return FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Picture> GetAll()
        {
            return _context.Pictures;
        }

        public void Update(Picture item)
        {
            _context.Update(item);
        }
    }
}
