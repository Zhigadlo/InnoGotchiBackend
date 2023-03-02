using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Validation;

namespace InnoGotchi.BLL.Services
{
    /// <summary>
    /// Represents service that get access to picture entities
    /// </summary>
    public class PictureService
    {
        private InnoGotchiUnitOfWork _database;
        private IMapper _mapper;
        private PictureValidator _validator;

        public PictureService(InnoGotchiUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
            _validator = new PictureValidator();
        }
        public async Task<int> CreateAsync(PictureDTO item)
        {
            Picture? picture = _mapper.Map<Picture>(item);
            var result = _validator.Validate(picture);
            if (!result.IsValid)
                return -1;

            _database.Pictures.Create(picture);
            await _database.SaveChangesAsync();
            return picture.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Picture? picture = await _database.Pictures.GetAsync(id);
            if (picture == null)
                return false;

            var isDeleted = await _database.Pictures.DeleteAsync(id);
            if (!isDeleted)
                return false;

            await _database.SaveChangesAsync();
            return true;
        }

        public async Task<PictureDTO?> GetAsync(int id, bool isTracking = true)
        {
            Picture? picture = await _database.Pictures.GetAsync(id, isTracking);
            return _mapper.Map<PictureDTO>(picture);
        }

        public IEnumerable<PictureDTO> GetAll(bool isTracking = true)
        {
            return _mapper.Map<IEnumerable<PictureDTO>>(_database.Pictures.AllItems(isTracking));
        }

        public async Task<bool> UpdateAsync(PictureDTO item)
        {
            Picture picture = _mapper.Map<Picture>(item);
            var result = _validator.Validate(picture);
            if (!result.IsValid)
                return false;

            _database.Pictures.Update(picture);
            await _database.SaveChangesAsync();
            return true;
        }
    }
}
