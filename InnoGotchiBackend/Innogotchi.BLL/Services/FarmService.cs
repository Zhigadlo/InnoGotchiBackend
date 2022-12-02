using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;
using InnoGotchi.BLL.Validation;

namespace InnoGotchi.BLL.Services
{
    public class FarmService : IService<FarmDTO>
    {
        private IUnitOfWork _database;
        private IMapper _mapper;
        private FarmValidator _validator;

        public FarmService(IUnitOfWork uow, IMapper mapper)
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

        public FarmDTO? Get(int id)
        {
            Farm? farm = _database.Farms.Get(id);
            if (farm == null)
                return null;
            else
                return _mapper.Map<FarmDTO>(farm);
        }

        public IEnumerable<FarmDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<FarmDTO>>(_database.Farms);
        }

        public void Update(int id, FarmDTO item)
        {
            Farm farm = _mapper.Map<Farm>(item);
            var result = _validator.Validate(farm);
            if (result.IsValid)
            {
                _database.Farms.Update(id, farm);
                _database.SaveChanges();
            }
        }
    }
}
