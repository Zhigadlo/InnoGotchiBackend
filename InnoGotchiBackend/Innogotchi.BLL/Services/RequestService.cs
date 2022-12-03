using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;
using InnoGotchi.BLL.Validation;

namespace InnoGotchi.BLL.Services
{
    public class RequestService : IService<ColoborationRequestDTO>
    {
        private IUnitOfWork _database;
        private IMapper _mapper;
        private RequestValidator _validator;
        public RequestService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
            _validator = new RequestValidator();
        }

        public int Create(ColoborationRequestDTO item)
        {
            var newRequest = _mapper.Map<ColoborationRequest>(item);
            var result = _validator.Validate(newRequest);
            if (result.IsValid)
            {
                _database.Requests.Create(newRequest);
                _database.SaveChanges();
                return newRequest.Id;
            }
            else
                return -1;
        }

        public void Delete(int id)
        {
            _database.Requests.Delete(id);
            _database.SaveChanges();
        }

        public ColoborationRequestDTO? Get(int id)
        {
            ColoborationRequest? request = _database.Requests.Get(id);
            if(request != null)
            {
                return _mapper.Map<ColoborationRequestDTO>(request);
            }
            else
                return null;
        }

        public IEnumerable<ColoborationRequestDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<ColoborationRequestDTO>>(_database.Requests.GetAll());
        }

        public void Update(ColoborationRequestDTO item)
        {
            ColoborationRequest request = _mapper.Map<ColoborationRequest>(item);
            var result = _validator.Validate(request);
            if (result.IsValid)
            {
                _database.Requests.Update(request);
                _database.SaveChanges();
            }

        }
    }
}
