using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Validation;

namespace InnoGotchi.BLL.Services
{
    /// <summary>
    /// Represents service that get access to coloboration requests entities
    /// </summary>
    public class RequestService
    {
        private InnoGotchiUnitOfWork _database;
        private IMapper _mapper;
        private RequestValidator _validator;
        public RequestService(InnoGotchiUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
            _validator = new RequestValidator();
        }

        public async Task<int> CreateAsync(ColoborationRequestDTO item)
        {
            item.IsConfirmed = false;
            item.Date = DateTime.UtcNow;
            var newRequest = _mapper.Map<ColoborationRequest>(item);
            var result = _validator.Validate(newRequest);

            if (!result.IsValid)
                return -1;

            _database.Requests.Create(newRequest);
            await _database.SaveChangesAsync();
            _database.Detach(newRequest);
            return newRequest.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!_database.Requests.Contains(r => r.Id == id))
                return false;
            
            var isDeleted = _database.Requests.Delete(id);
            if (!isDeleted)
                return false;

            await _database.SaveChangesAsync();
            return true;  
        }

        public ColoborationRequestDTO? Get(int id, bool isTracking = true)
        {
            ColoborationRequest? request = _database.Requests.Get(id, isTracking);
            return _mapper.Map<ColoborationRequestDTO>(request);
        }

        public IEnumerable<ColoborationRequestDTO> GetAll(bool isTracking = true)
        {
            return _mapper.Map<IEnumerable<ColoborationRequestDTO>>(_database.Requests.AllItems(isTracking));
        }

        public async Task<bool> ConfirmAsync(int id)
        {
            var request = _database.Requests.Get(id, false);
            if (request == null)
                return false;

            request.IsConfirmed = true;
            _database.Requests.Update(request);
            await _database.SaveChangesAsync();
            return true;
        }
    }
}
