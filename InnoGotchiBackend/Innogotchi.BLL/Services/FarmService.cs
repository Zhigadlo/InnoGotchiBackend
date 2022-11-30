using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;

namespace InnoGotchi.BLL.Services
{
    public class FarmService : IService<FarmDTO>
    {
        private IUnitOfWork _database;
        private IMapper _mapper;

        public FarmService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
        }
        public void Create(FarmDTO item)
        {
            Farm farm = _mapper.Map<Farm>(item);
            _database.Farms.Create(farm);
            _database.SaveChanges();
        }

        public FarmDTO? Get(int id)
        {
            Farm? farm = _database.Farms.Get(id);
            if(farm == null)
                return null;
            return _mapper.Map<FarmDTO>(farm);

        }

        public IEnumerable<FarmDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<FarmDTO>>(_database.Farms);
        }

        public void Update(int id, FarmDTO item)
        {
            Farm farm = _mapper.Map<Farm>(item);
            _database.Farms.Update(id, farm);
            _database.SaveChanges();
        }
    }
}
