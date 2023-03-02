using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.Respositories
{
    /// <summary>
    /// Represents repository that has full access to coloboration request entities in database
    /// </summary>
    public class RequestRepository : IRepository<ColoborationRequest>
    {
        private InnoGotchiContext _context;
        public RequestRepository(InnoGotchiContext context)
        {
            _context = context;
        }

        public bool Contains(Func<ColoborationRequest, bool> predicate)
        {
            ColoborationRequest? request = AllItems().FirstOrDefault(predicate);
            if (request == null)
                return false;

            return true;
        }

        public void Create(ColoborationRequest item)
        {
            _context.Requests.Add(item);
        }

        public bool Delete(int id)
        {
            ColoborationRequest? request = Get(id);
            if (request == null)
                return false;

            _context.Requests.Remove(request);
            return true;
        }

        public IEnumerable<ColoborationRequest> FindAll(Func<ColoborationRequest, bool> expression, bool isTracking = true)
        {
            return AllItems(isTracking).Where(expression);
        }

        public ColoborationRequest? Get(int id, bool isTracking = true)
        {
            var items = AllItems(isTracking);
            return items.FirstOrDefault(r => r.Id == id);
        }

        public IQueryable<ColoborationRequest> AllItems(bool isTracking = true)
        {
            var items = _context.Requests.Include(r => r.RequestOwner).Include(r => r.RequestReceipient);
            return isTracking ? items : items.AsNoTracking();
        }

        public void Update(ColoborationRequest item)
        {
            _context.Requests.Update(item);
        }
    }
}
