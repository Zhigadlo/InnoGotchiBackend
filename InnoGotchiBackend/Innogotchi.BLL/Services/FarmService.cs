using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Validation;

namespace InnoGotchi.BLL.Services
{
    /// <summary>
    /// Represents service that get access to farm entities
    /// </summary>
    public class FarmService
    {
        private InnoGotchiUnitOfWork _database;
        private IMapper _mapper;
        private FarmValidator _validator;

        public FarmService(InnoGotchiUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
            _validator = new FarmValidator();
        }
        public async Task<int> CreateAsync(FarmDTO item)
        {
            item.CreateTime = DateTime.UtcNow;
            Farm farm = _mapper.Map<Farm>(item);
            var result = _validator.Validate(farm);
            if (!result.IsValid)
                return -1;

            _database.Farms.Create(farm);
            await _database.SaveChangesAsync();
            return farm.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!(await _database.Farms.ContainsAsync(f => f.Id == id)))
                return false;

            var isDeleted = await _database.Farms.DeleteAsync(id);
            if (!isDeleted)
                return false;

            await _database.SaveChangesAsync();
            return true;
        }

        public async Task<FarmDTO?> GetAsync(int id, bool isTracking = true)
        {
            return _mapper.Map<FarmDTO>(await _database.Farms.GetAsync(id, isTracking));
        }

        public IEnumerable<FarmDTO> GetAll(bool isTracking = true)
        {
            return _mapper.Map<IEnumerable<FarmDTO>>(_database.Farms.AllItems(isTracking));
        }

        public IEnumerable<string> GetAllNames()
        {
            return _database.Farms.AllItems(false).Select(f => f.Name);
        }

        public async Task<bool> UpdateAsync(FarmDTO item)
        {
            Farm farm = _mapper.Map<Farm>(item);
            var result = _validator.Validate(farm);
            if (!result.IsValid)
                return false;

            _database.Farms.Update(farm);
            await _database.SaveChangesAsync();
            return true;
        }
    }
}
