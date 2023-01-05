using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;
using InnoGotchi.BLL.Validation;

namespace InnoGotchi.BLL.Services
{
    public class PictureService : IService<PictureDTO>
    {
        private IUnitOfWork _database;
        private IMapper _mapper;
        private PictureValidator _validator;

        public PictureService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
            _validator = new PictureValidator();
        }
        public int Create(PictureDTO item)
        {
            Picture? picture = _mapper.Map<Picture>(item);
            var result = _validator.Validate(picture);
            if (result.IsValid)
            {
                _database.Pictures.Create(picture);
                _database.SaveChanges();
                return picture.Id;
            }
            else
                return -1;
        }

        public bool Delete(int id)
        {
            Picture? picture = _database.Pictures.First(p => p.Id == id);
            if (picture != null)
            {
                _database.Pictures.Delete(id);
                _database.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public PictureDTO? Get(int id)
        {
            Picture? picture = _database.Pictures.Get(id);
            return _mapper.Map<PictureDTO>(picture);
        }

        public IEnumerable<PictureDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<PictureDTO>>(_database.Pictures.GetAll());
        }

        public bool Update(PictureDTO item)
        {
            Picture picture = _mapper.Map<Picture>(item);
            var result = _validator.Validate(picture);
            if(result.IsValid)
            {
                _database.Pictures.Update(picture);
                _database.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
