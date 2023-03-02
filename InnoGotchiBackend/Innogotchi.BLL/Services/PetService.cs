using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Models;
using InnoGotchi.BLL.Validation;

namespace InnoGotchi.BLL.Services
{
    /// <summary>
    /// Represents service that get access to pet entities
    /// </summary>
    public class PetService
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
        public async Task<int> CreateAsync(PetDTO item)
        {
            if (_database.Pets.Contains(p => p.Name == item.Name))
                return -1;

            Pet newPet = _mapper.Map<Pet>(item);
            var result = _validator.Validate(newPet);
            if (!result.IsValid)
                return -1;

            _database.Pets.Create(newPet);
            await _database.SaveChangesAsync();
            _database.Detach(newPet);
            return newPet.Id;
        }
        public PetDTO? Get(int id, bool isTracking = true)
        {
            return _mapper.Map<PetDTO>(_database.Pets.Get(id, isTracking));
        }
        public IEnumerable<PetDTO> GetAll(bool isTracking = true)
        {
            return _mapper.Map<IEnumerable<PetDTO>>(_database.Pets.AllItems(isTracking));
        }
        /// <summary>
        /// Gets PaginatedList object that have information about page with sorting and filtration
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sortType"></param>
        /// <param name="filterModel"></param>
        public PaginatedList<IEnumerable<PetDTO>> GetPage(int page, string sortType, PetFilterModel filterModel)
        {
            IEnumerable<Pet>? pets = null;

            if (filterModel.Age != 0 && filterModel.GameYear != 0)
            {
                Func<Pet, bool> predicate = p =>
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
                };

                pets = _database.Pets.FindAll(predicate);
            }

            if (filterModel.HungerLavel != -1 && filterModel.FeedingPeriod != 0)
            {
                var minHungerTime = filterModel.HungerLavel * filterModel.FeedingPeriod;
                Func<Pet, bool> predicate = p =>
                {
                    var hungerTime = (DateTime.UtcNow - p.LastFeedingTime).Ticks;
                    if (filterModel.IsLastHungerStage)
                        return hungerTime > minHungerTime;

                    return hungerTime > minHungerTime && hungerTime < minHungerTime + filterModel.FeedingPeriod;

                };
                pets = _database.Pets.FindAll(predicate);
            }

            if (filterModel.ThirstyLavel != -1 && filterModel.DrinkingPeriod != 0)
            {
                var minThirstyTime = filterModel.ThirstyLavel * filterModel.DrinkingPeriod;
                Func<Pet, bool> predicate = p =>
                {
                    var thirstyTime = (DateTime.UtcNow - p.LastDrinkingTime).Ticks;
                    if (filterModel.IsLastThirstyStage)
                        return thirstyTime > minThirstyTime;

                    return thirstyTime > minThirstyTime && thirstyTime < minThirstyTime + filterModel.DrinkingPeriod;

                };

                pets = _database.Pets.FindAll(predicate);
            }

            if (pets == null)
                pets = _database.Pets.AllItems();

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

            var petsDTO = _mapper.Map<IEnumerable<PetDTO>>(pets);
            var result = new PaginatedList<IEnumerable<PetDTO>>(petsDTO, count, page, pageSize);

            return result;
        }

        public async Task<bool> UpdateNameAsync(int id, string newName)
        {
            var pet = _database.Pets.Get(id);
            if (pet == null)
                return false;

            pet.Name = newName;
            _database.Pets.Update(pet);
            await _database.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(PetDTO item)
        {
            Pet pet = _mapper.Map<Pet>(item);
            var result = _validator.Validate(pet);
            if (!result.IsValid)
                return false;

            _database.Pets.Update(pet);
            await _database.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pet = _database.Pets.Get(id);
            if (pet == null)
                return false;
            
            var isDeleted = _database.Pets.Delete(id);
            if (!isDeleted)
                return false;

            await _database.SaveChangesAsync();
            return true;
            
        }
        /// <summary>
        /// Feeds pet by id
        /// </summary>
        /// <param name="id"></param>
        public async Task FeedAsync(int id)
        {
            PetDTO? pet = Get(id);
            if (pet != null)
            {
                pet.Feed();
                await UpdateAsync(pet);
            }
        }

        /// <summary>
        /// Gives a drink to pet by id
        /// </summary>
        /// <param name="id"></param>
        public async Task DrinkAsync(int id)
        {
            PetDTO? pet = Get(id);
            if (pet != null)
            {
                pet.Drink();
                await UpdateAsync(pet);
            }
        }

        public async Task<bool> DeathAsync(int id, long deathTime)
        {
            PetDTO? petDTO = Get(id);
            if (petDTO == null)
                return false;

            if (petDTO.DeathTime != DateTime.MinValue)
                return true;

            petDTO.DeathTime = DateTime.MinValue.AddTicks(deathTime);
            petDTO.FirstHappinessDate = DateTime.MinValue;
            if (await UpdateAsync(petDTO))
                return true;

            return false;
        }
    }
}
