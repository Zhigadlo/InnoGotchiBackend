using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;
using InnoGotchi.BLL.Models;
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

        public PaginatedList<PetDTO> GetPage(int page, string sortType, long age, long year, int hungerStage, long feedingPeriod, bool isLastFeedingStage, int thirstyStage, long drinkingPeriod, bool isLastDrinkingStage)
        {
            var pets = _database.Pets.GetAll();

            if (age != 0 && year != 0)
            {
                pets = pets.Where(p =>
                {
                    if (p.DeadTime != DateTime.MinValue)
                    {
                        var life = p.DeadTime.Value.AddTicks(-p.CreateTime.Ticks);
                        return life.Ticks >= age && life.Ticks < age + year;
                    }
                    else
                    {
                        var life = DateTime.UtcNow.AddTicks(-p.CreateTime.Ticks);
                        return life.Ticks >= age && life.Ticks < age + year;
                    }
                });
            }

            if (hungerStage != -1 && feedingPeriod != 0)
            {
                var hungerLavel = hungerStage * feedingPeriod;
                pets = pets.Where(p =>
                {
                    var hungerTime = (DateTime.UtcNow - p.LastFeedingTime).Ticks;
                    if (isLastFeedingStage)
                        return hungerTime > hungerLavel;

                    return hungerTime > hungerLavel && hungerTime < hungerLavel + feedingPeriod;
                    
                });
            }

            if (thirstyStage != -1 && drinkingPeriod != 0)
            {
                var thirstyLavel = thirstyStage * drinkingPeriod;
                pets = pets.Where(p =>
                {
                    var thirstyTime = (DateTime.UtcNow - p.LastDrinkingTime).Ticks;
                    if (isLastDrinkingStage)
                        return thirstyTime > thirstyLavel;
                    
                    return thirstyTime > thirstyLavel && thirstyTime < thirstyLavel + drinkingPeriod;
                    
                });
            }

            int pageSize = 15;
            int totalPages = (int)Math.Ceiling(pets.Count() / (double)pageSize);
            if (page > totalPages)
                page = 1;

            List<Pet> sortedPets = new List<Pet>();

            switch (sortType)
            {
                case "name_asc":
                    sortedPets = pets.OrderBy(p => p.Name).ToList();
                    break;
                case "name_desc":
                    sortedPets = pets.OrderByDescending(p => p.Name).ToList();
                    break;
                case "age_asc":
                    sortedPets = pets.OrderBy(p => DateTime.UtcNow - p.CreateTime).ToList();
                    break;
                case "age_desc":
                    sortedPets = pets.OrderByDescending(p => DateTime.UtcNow - p.CreateTime).ToList();
                    break;
                case "state_asc":
                    sortedPets = pets.OrderBy(p =>
                    {
                        if (p.LastDrinkingTime < p.LastFeedingTime)
                        {
                            return DateTime.UtcNow - p.LastDrinkingTime;
                        }
                        else
                        {
                            return DateTime.UtcNow - p.LastFeedingTime;
                        }
                    }).ToList();
                    break;
                case "state_desc":
                    sortedPets = pets.OrderByDescending(p =>
                    {
                        if (p.LastDrinkingTime < p.LastFeedingTime)
                        {
                            return DateTime.UtcNow - p.LastDrinkingTime;
                        }
                        else
                        {
                            return DateTime.UtcNow - p.LastFeedingTime;
                        }
                    }).ToList();
                    break;
                case "hunger_asc":
                    sortedPets = pets.OrderBy(p => p.LastFeedingTime).ToList();
                    break;
                case "hunger_desc":
                    sortedPets = pets.OrderByDescending(p => p.LastFeedingTime).ToList();
                    break;
                case "thirsty_asc":
                    sortedPets = pets.OrderBy(p => p.LastDrinkingTime).ToList();
                    break;
                case "thirsty_desc":
                    sortedPets = pets.OrderByDescending(p => p.LastDrinkingTime).ToList();
                    break;
                case "happiness_desc":
                    sortedPets = pets.OrderByDescending(p =>
                    {
                        if (p.DeadTime != DateTime.MinValue)
                        {
                            return DateTime.MaxValue.Ticks;
                        }
                        else
                        {
                            return (p.DeadTime - p.FirstHappinessDate).Value.Ticks;
                        }
                    }).ToList();
                    break;
                default:
                    sortedPets = pets.OrderBy(p =>
                    {
                        if (p.DeadTime != DateTime.MinValue)
                        {
                            return DateTime.MaxValue.Ticks;
                        }
                        else
                        {
                            return (p.DeadTime - p.FirstHappinessDate).Value.Ticks;
                        }
                    }).ToList();
                    break;
            }

            var sortedPetsDTO = _mapper.Map<List<PetDTO>>(sortedPets);
            var result = PaginatedList<PetDTO>.Create(sortedPetsDTO, page, pageSize);
            return result;
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
