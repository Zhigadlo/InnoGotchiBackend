using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.Web.Models;

namespace InnoGotchi.Web.Mapper
{
    public class ViewModelProfile : Profile
    {
        public ViewModelProfile()
        {
            CreateMap<UserDTO, UserModel>().ReverseMap();
            CreateMap<FarmDTO, FarmModel>().ReverseMap();
            CreateMap<PetDTO, PetModel>().ReverseMap();
            CreateMap<ColoborationRequestDTO, RequestModel>().ReverseMap();
        }
    }
}
