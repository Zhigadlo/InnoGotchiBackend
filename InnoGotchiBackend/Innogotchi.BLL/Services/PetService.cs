using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;
using InnoGotchi.BLL.Validation;

namespace InnoGotchi.BLL.Services
{
    public class PetService : IService<PetDTO>
    {
        private IUnitOfWork _database;
        private IMapper _mapper;
        private PetValidator _validator;

        public PetService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
            _validator = new PetValidator();
        }
        public int Create(PetDTO item)
        {
            if (_database.Pets.Contains(p => p.Name == item.Name))
                throw new Exception("There is pet with such name");
            Farm? farm = _database.Farms.First(f => f.Id == item.FarmId);

            Pet newPet = _mapper.Map<Pet>(item);
            var result = _validator.Validate(newPet);
            if (result.IsValid)
            {
                _database.Pets.Create(newPet);
                _database.SaveChanges();
                return newPet.Id;
            }
            else
                return -1;
        }
        public PetDTO? Get(int id)
        {
            Pet? pet = _database.Pets.Get(id);
            if (pet == null)
                return null;
            else
                return _mapper.Map<PetDTO>(pet);
        }
        public IEnumerable<PetDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<PetDTO>>(_database.Pets.GetAll());
        }
        public bool Update(PetDTO item)
        {
            Pet pet = _mapper.Map<Pet>(item);
            var result = _validator.Validate(pet);
            if (result.IsValid)
            {
                _database.Pets.Update(pet);
                _database.SaveChanges();
                return true;
            }

            return false;
        }
        public bool Delete(int id)
        {
            var pet = _database.Pets.First(p => p.Id == id);
            if (pet != null)
            {
                _database.Pets.Delete(id);
                _database.SaveChanges();
                return true;
            }

            return false;
        }
        public void Feed(int id)
        {
            PetDTO? pet = Get(id);
            if (pet != null)
            {
                pet.Feed();
                Update(pet);
            }
        }
        public void Drink(int id)
        {
            PetDTO? pet = Get(id);
            if (pet != null)
            {
                pet.Drink();
                Update(pet);
            }
        }
    }
}
