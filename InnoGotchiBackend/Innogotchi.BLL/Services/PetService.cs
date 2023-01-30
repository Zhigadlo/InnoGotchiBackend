using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;
using InnoGotchi.BLL.Models;
using InnoGotchi.BLL.Validation;

namespace InnoGotchi.BLL.Services
{
    /// <summary>
    /// Represents service that get access to pet entities
    /// </summary>
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
            Farm? farm = _database.Farms.Get(item.FarmId);

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
            return _mapper.Map<PetDTO>(_database.Pets.Get(id));
        }
        public IEnumerable<PetDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<PetDTO>>(_database.Pets.GetAll());
        }
        /// <summary>
        /// Gets PaginatedList object that have information about page with sorting and filtration
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sortType"></param>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        public PaginatedList<PetDTO> GetPage(int page, string sortType, PetFilterModel filterModel)
        {
            IQueryable<Pet>? pets = null;

            if (filterModel.Age != 0 && filterModel.GameYear != 0)
            {
                pets = _database.Pets.FindAll(p =>
                {
                    if (p.DeathTime != DateTime.MinValue)
                    {
                        var life = p.DeathTime.AddTicks(-p.CreateTime.Ticks);
                        return life.Ticks >= filterModel.Age && life.Ticks < filterModel.Age + filterModel.GameYear;
                    }
                    else
                    {
                        var life = DateTime.UtcNow.AddTicks(-p.CreateTime.Ticks);
                        return life.Ticks >= filterModel.Age && life.Ticks < filterModel.Age + filterModel.GameYear;
                    }
                });
            }

            if (filterModel.HungerLavel != -1 && filterModel.FeedingPeriod != 0)
            {
                var minHungerTime = filterModel.HungerLavel * filterModel.FeedingPeriod;
                pets = _database.Pets.FindAll(p =>
                {
                    var hungerTime = (DateTime.UtcNow - p.LastFeedingTime).Ticks;
                    if (filterModel.IsLastHungerStage)
                        return hungerTime > minHungerTime;

                    return hungerTime > minHungerTime && hungerTime < minHungerTime + filterModel.FeedingPeriod;

                });
            }

            if (filterModel.ThirstyLavel != -1 && filterModel.DrinkingPeriod != 0)
            {
                var minThirstyTime = filterModel.ThirstyLavel * filterModel.DrinkingPeriod;
                pets = _database.Pets.FindAll(p =>
                {
                    var thirstyTime = (DateTime.UtcNow - p.LastDrinkingTime).Ticks;
                    if (filterModel.IsLastThirstyStage)
                        return thirstyTime > minThirstyTime;

                    return thirstyTime > minThirstyTime && thirstyTime < minThirstyTime + filterModel.DrinkingPeriod;

                });
            }

            if (pets == null)
                pets = _database.Pets.GetAll();

            int pageSize = 15;
            int totalPages = (int)Math.Ceiling(pets.Count() / (double)pageSize);
            if (page > totalPages)
                page = 1;

            switch (sortType)
            {
                case "name_asc":
                    pets = pets.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    pets = pets.OrderByDescending(p => p.Name);
                    break;
                case "age_asc":
                    pets = pets.OrderByDescending(p => p.CreateTime);
                    break;
                case "age_desc":
                    pets = pets.OrderBy(p => p.CreateTime);
                    break;
                case "state_asc":
                    pets = pets.OrderBy(p => p.LastDrinkingTime);
                    break;
                case "state_desc":
                    pets = pets.OrderByDescending(p => p.LastDrinkingTime);
                    break;
                case "hunger_asc":
                    pets = pets.OrderBy(p => p.LastFeedingTime);
                    break;
                case "hunger_desc":
                    pets = pets.OrderByDescending(p => p.LastFeedingTime);
                    break;
                case "thirsty_asc":
                    pets = pets.OrderBy(p => p.LastDrinkingTime);
                    break;
                case "thirsty_desc":
                    pets = pets.OrderByDescending(p => p.LastDrinkingTime);
                    break;
                case "happiness_desc":
                    pets = pets.OrderByDescending(p => p.FirstHappinessDate);
                    break;
                default:
                    pets = pets.OrderBy(p => p.FirstHappinessDate);
                    break;
            }

            var count = pets.Count();
            pets = pets.Skip((page - 1) * pageSize).Take(pageSize);

            var petsDTO = _mapper.Map<IEnumerable<PetDTO>>(pets?.AsEnumerable());
            var result = new PaginatedList<PetDTO>(petsDTO, count, page, pageSize);

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
            var pet = _database.Pets.Get(id);
            if (pet != null)
            {
                var isDeleted = _database.Pets.Delete(id);
                if (!isDeleted)
                    return false;

                _database.SaveChanges();
                return true;
            }

            return false;
        }
        /// <summary>
        /// Feeds pet by id
        /// </summary>
        /// <param name="id"></param>
        public void Feed(int id)
        {
            PetDTO? pet = Get(id);
            if (pet != null)
            {
                pet.Feed();
                Update(pet);
            }
        }

        /// <summary>
        /// Gives a drink to pet by id
        /// </summary>
        /// <param name="id"></param>
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
