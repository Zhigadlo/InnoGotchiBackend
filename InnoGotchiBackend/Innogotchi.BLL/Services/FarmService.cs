using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;
using InnoGotchi.BLL.Validation;

namespace InnoGotchi.BLL.Services
{
    /// <summary>
    /// Represents service that get access to farm entities
    /// </summary>
    public class FarmService : IService<FarmDTO>
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
        public int Create(FarmDTO item)
        {
            Farm farm = _mapper.Map<Farm>(item);
            var result = _validator.Validate(farm);
            if (result.IsValid)
            {
                _database.Farms.Create(farm);
                _database.SaveChanges();
                return farm.Id;
            }
            else
                return -1;
        }

        public bool Delete(int id)
        {
            if (_database.Farms.Contains(f => f.Id == id))
            {
                var isDeleted = _database.Farms.Delete(id);
                if (!isDeleted)
                    return false;

                _database.SaveChanges();
                return true;
            }
            return false;
        }

        public FarmDTO? Get(int id)
        {
            return _mapper.Map<FarmDTO>(_database.Farms.Get(id));
        }

        public IEnumerable<FarmDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<FarmDTO>>(_database.Farms.AllItems());
        }

        public bool Update(FarmDTO item)
        {
            Farm farm = _mapper.Map<Farm>(item);
            var result = _validator.Validate(farm);
            if (result.IsValid)
            {
                _database.Farms.Update(farm);
                _database.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
