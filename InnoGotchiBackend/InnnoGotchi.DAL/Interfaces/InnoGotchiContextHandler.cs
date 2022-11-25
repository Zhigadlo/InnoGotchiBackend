using InnnoGotchi.DAL.EF;

namespace InnnoGotchi.DAL.Interfaces
{
    public class InnoGotchiContextHandler
    {
        protected InnoGotchiContext _context;
        
        public InnoGotchiContextHandler(InnoGotchiContext context)
        {
            _context = context;
        }
    }
}
