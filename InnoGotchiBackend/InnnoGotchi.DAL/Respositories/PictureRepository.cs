using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        public async Task<bool> ContainsAsync(Expression<Func<Picture, bool>> expression)
        {
            Picture? picture = await AllItems().FirstOrDefaultAsync(expression);
            if (picture == null)
                return false;

            return true;
        }

        public void Create(Picture item)
        {
            _context.Pictures.Add(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Picture? picture = await GetAsync(id);
            if (picture == null)
                return false;

            _context.Pictures.Remove(picture);
            return true;
        }

        public IEnumerable<Picture> FindAll(Func<Picture, bool> expression, bool isTracking = true)
        {
            return AllItems(isTracking).Where(expression);
        }

        public async Task<Picture?> GetAsync(int id, bool isTracking = true)
        {
            return await AllItems(isTracking).FirstOrDefaultAsync(p => p.Id == id);
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
