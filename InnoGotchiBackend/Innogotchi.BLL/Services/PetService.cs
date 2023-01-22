using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;
using InnoGotchi.BLL.Validation;

namespace InnoGotchi.BLL.Services
{
    public class PetService : IService<PetDTO>
    {
        private InnoGotchiUnitOfWork _database;
        private IMapper _mapper;
        private PetValidator _validator;

        public PetService(InnoGotchiUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
            _validator = new PetValidator();
        }
        public int Create(PetDTO item)
        {
            if (_database.Pets.Contains(p => p.Name == item.Name))
                return -1;
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

        public IEnumerable<PetDTO> GetPage(int page, string sortType)
        {
            int pageSize = 15;
            switch (sortType)
            {
                case "name_asc":
                    var result = _database.Pets.GetAll().OrderBy(p => p.Name).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
                case "name_desc":
                    result = _database.Pets.GetAll().OrderByDescending(p => p.Name).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
                case "age_asc":
                    result = _database.Pets.GetAll().OrderBy(p => DateTime.UtcNow - p.CreateTime).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
                case "age_desc":
                    result = _database.Pets.GetAll().OrderByDescending(p => DateTime.UtcNow - p.CreateTime).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
                case "state_asc":
                    result = _database.Pets.GetAll().OrderBy(p =>
                    {
                        if (p.LastDrinkingTime < p.LastFeedingTime)
                        {
                            return DateTime.UtcNow - p.LastDrinkingTime;
                        }
                        else
                        {
                            return DateTime.UtcNow - p.LastFeedingTime;
                        }
                    }).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
                case "state_desc":
                    result = _database.Pets.GetAll().OrderByDescending(p =>
                    {
                        if (p.LastDrinkingTime < p.LastFeedingTime)
                        {
                            return DateTime.UtcNow - p.LastDrinkingTime;
                        }
                        else
                        {
                            return DateTime.UtcNow - p.LastFeedingTime;
                        }
                    }).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
                case "hunger_asc":
                    result = _database.Pets.GetAll().OrderBy(p => p.LastFeedingTime).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
                case "hunger_desc":
                    result = _database.Pets.GetAll().OrderByDescending(p => p.LastFeedingTime).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
                case "thirsty_asc":
                    result = _database.Pets.GetAll().OrderBy(p => p.LastDrinkingTime).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
                case "thirsty_desc":
                    result = _database.Pets.GetAll().OrderByDescending(p => p.LastDrinkingTime).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
                case "happiness_desc":
                    result = _database.Pets.GetAll().OrderByDescending(p => DateTime.UtcNow - p.FirstHappinessDate).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
                default:
                    result = _database.Pets.GetAll().OrderBy(p => DateTime.UtcNow - p.FirstHappinessDate).Skip((page - 1) * pageSize).Take(pageSize);
                    return _mapper.Map<IEnumerable<PetDTO>>(result);
            }
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
