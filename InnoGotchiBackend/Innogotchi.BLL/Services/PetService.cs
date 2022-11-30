using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;

namespace InnoGotchi.BLL.Services
{
    public class PetService : IService<PetDTO>
    {
        private IUnitOfWork _database;
        private IMapper _mapper;
        
        public PetService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
            
        }
        public void Create(PetDTO item)
        {
            Pet? pet = _database.Pets.First(p => p.Name == item.Name);
            if (pet != null)
                throw new Exception("There is pet with such name");
            Farm? farm = _database.Farms.First(f => f.Id == item.FarmId);
            if (farm == null)
                throw new Exception("There is no such farm");

            Pet newPet = _mapper.Map<Pet>(item);
            _database.Pets.Create(newPet);
            _database.SaveChanges();
        }

        public PetDTO? Get(int id)
        {
            Pet? pet = _database.Pets.Get(id);
            if (pet == null)
                return null;
            return _mapper.Map<PetDTO>(pet);
        }

        public IEnumerable<PetDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<PetDTO>>(_database.Pets);
        }

        public void Update(int id, PetDTO item)
        {
            Pet pet = _mapper.Map<Pet>(item);
            _database.Pets.Update(id, pet);
            _database.SaveChanges();
        }
    }
}
