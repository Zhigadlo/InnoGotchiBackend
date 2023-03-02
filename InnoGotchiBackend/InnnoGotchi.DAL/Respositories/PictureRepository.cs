using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.Respositories
{
    /// <summary>
    /// Represents repository that has full access to picture entities in database
    /// </summary>
    public class PictureRepository : IRepository<Picture>
    {
        private InnoGotchiContext _context;

        public PictureRepository(InnoGotchiContext context)
        {
            _context = context;
        }
        public bool Contains(Func<Picture, bool> predicate)
        {
            Picture? picture = AllItems().FirstOrDefault(predicate);
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
            if (picture == null)
                return false;

            _context.Pictures.Remove(picture);
            return true;
        }

        public IEnumerable<Picture> FindAll(Func<Picture, bool> expression, bool isTracking = true)
        {
            return AllItems(isTracking).Where(expression);
        }

        public Picture? Get(int id, bool isTracking = true)
        {
            return AllItems(isTracking).FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Picture> AllItems(bool isTracking = true)
        {
            var items = _context.Pictures;
            return isTracking ? items : items.AsNoTracking();
        }

        public void Update(Picture item)
        {
            _context.Update(item);
        }
    }
}
