using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnoGotchi.BLL.DTO;

namespace InnoGotchi.BLL.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Pet, PetDTO>().ReverseMap();
            CreateMap<Farm, FarmDTO>().ReverseMap();
            CreateMap<Picture, PictureDTO>().ReverseMap();
            CreateMap<ColoborationRequest, ColoborationRequestDTO>().ReverseMap();
        }
    }
}
