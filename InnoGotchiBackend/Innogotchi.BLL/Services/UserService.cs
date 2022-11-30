using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;

namespace InnoGotchi.BLL.Services
{
    public class UserService : IService<UserDTO>
    {
        private IUnitOfWork _database;
        private IMapper _mapper;
        public UserService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
        }
        public void Create(UserDTO item)
        {
            User? user = _database.Users.First(u => u.Email == item.Email);
            
            if(user != null) 
            {
                throw new Exception("This email is already in use");
            }

            User newUser = _mapper.Map<User>(item);

            _database.Users.Create(newUser); 
            _database.SaveChanges();
        }

        public UserDTO? Get(int id)
        {
            User? user = _database.Users.Get(id);
            if (user == null)
                return null;

            return _mapper.Map<UserDTO>(user);
        }

        public IEnumerable<UserDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<UserDTO>>(_database.Users);
        }

        public void Update(int id, UserDTO item)
        {
            User user = _mapper.Map<User>(item);
            _database.Users.Update(id, user);
            _database.SaveChanges();
        }
    }
}
