using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            ColoborationRequest? request = FirstOrDefault(predicate);
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
            if (request != null)
            {
                _context.Requests.Remove(request);
                return true;
            }

            return false;
        }

        public IQueryable<ColoborationRequest> FindAll(Expression<Func<ColoborationRequest, bool>> expression)
        {
            return GetAll().Where(expression);
        }

        public ColoborationRequest? FirstOrDefault(Func<ColoborationRequest, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public ColoborationRequest? Get(int id)
        {
            ColoborationRequest? request = FirstOrDefault(r => r.Id == id);

            if (request != null)
            {
                _context.Entry(request).State = EntityState.Detached;
                return request;
            }
            else
                return null;
        }

        public IQueryable<ColoborationRequest> GetAll()
        {
            return _context.Requests.Include(r => r.RequestOwner)
                                    .Include(r => r.RequestReceipient);
        }

        public void Update(ColoborationRequest item)
        {
            _context.Entry(item).State = EntityState.Detached;
            _context.Requests.Update(item);
        }
    }
}
