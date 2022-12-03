using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;

namespace InnnoGotchi.DAL.Respositories
{
    public class RequestRepository : IRepository<ColoborationRequest>
    {
        private InnoGotchiContext _context;
        public RequestRepository(InnoGotchiContext context)
        {
            _context = context;
        }
        public void Create(ColoborationRequest item)
        {
            _context.Requests.Add(item);
        }

        public void Delete(int id)
        {
            ColoborationRequest? request = _context.Requests.FirstOrDefault(r => r.Id == id);
            if(request != null)
            {
                _context.Requests.Remove(request);
            }
        }

        public IEnumerable<ColoborationRequest> Find(Func<ColoborationRequest, bool> predicate)
        {
            return _context.Requests.Where(predicate).AsEnumerable();
        }

        public ColoborationRequest? First(Func<ColoborationRequest, bool> predicate)
        {
            return _context.Requests.FirstOrDefault(predicate);
        }

        public ColoborationRequest? Get(int id)
        {
            ColoborationRequest? request = _context.Requests.FirstOrDefault(r => r.Id == id);
            if(request != null )
                return request;
            else
                return null;
        }

        public IEnumerable<ColoborationRequest> GetAll()
        {
            return _context.Requests.AsEnumerable();
        }

        public void Update(ColoborationRequest item)
        {
            _context.Requests.Update(item);
        }
    }
}
