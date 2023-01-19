using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.Respositories
{
    public class RequestRepository : IRepository<ColoborationRequest>
    {
        private InnoGotchiContext _context;
        public RequestRepository(InnoGotchiContext context)
        {
            _context = context;
        }

        public bool Contains(Func<ColoborationRequest, bool> predicate)
        {
            ColoborationRequest? request = _context.Requests.FirstOrDefault(predicate);
            if (request == null)
                return false;

            return true;
        }

        public void Create(ColoborationRequest item)
        {
            _context.Requests.Add(item);
        }

        public void Delete(int id)
        {
            ColoborationRequest? request = _context.Requests.FirstOrDefault(r => r.Id == id);
            if (request != null)
            {
                _context.Requests.Remove(request);
            }
        }

        public IEnumerable<ColoborationRequest> Find(Func<ColoborationRequest, bool> predicate)
        {
            return _context.Requests.Include(r => r.RequestOwner)
                                    .Include(r => r.RequestReceipient)
                                    .Where(predicate).AsEnumerable();
        }

        public ColoborationRequest? First(Func<ColoborationRequest, bool> predicate)
        {
            return _context.Requests.Include(r => r.RequestOwner)
                                    .Include(r => r.RequestReceipient)
                                    .FirstOrDefault(predicate);
        }

        public ColoborationRequest? Get(int id)
        {
            ColoborationRequest? request = _context.Requests.Include(r => r.RequestOwner)
                                                            .Include(r => r.RequestReceipient)
                                                            .FirstOrDefault(r => r.Id == id);

            if (request != null)
            {
                _context.Entry(request).State = EntityState.Detached;
                return request;
            }
            else
                return null;
        }

        public IEnumerable<ColoborationRequest> GetAll()
        {
            return _context.Requests.Include(r => r.RequestOwner)
                                    .Include(r => r.RequestReceipient)
                                    .AsEnumerable();
        }

        public void Update(ColoborationRequest item)
        {
            _context.Entry(item).State = EntityState.Detached;
            _context.Requests.Update(item);
        }
    }
}
