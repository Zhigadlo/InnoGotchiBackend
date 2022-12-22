using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Services;
using InnoGotchi.Web.Mapper;
using InnoGotchi.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Pets")]
    public class PetsController : Controller
    {
        private PetService _service;
        private IMapper _mapper;

        public PetsController(PetService service)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new ViewModelProfile()));
            _mapper = config.CreateMapper();
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_service.Get(id));
        }

        [HttpPost]
        public IActionResult Create(PetModel pet)
        {
            var petDTO = _mapper.Map<PetDTO>(pet);
            petDTO.CreateTime = DateTime.Now;
            petDTO.FirstHappinessDate = DateTime.Now;
            petDTO.LastDrinkingTime = DateTime.Now;
            petDTO.LastFeedingTime = DateTime.Now;
            return Ok(_service.Create(petDTO));
        }

        [HttpPut]
        public IActionResult Update(int id, string newName)
        {
            PetDTO? petDTO = _service.Get(id);

            if (petDTO != null)
            {
                petDTO.Name = newName;
                _service.Update(petDTO);
                return Ok();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }

        [HttpPut("feed")]
        public void Feed(int id)
        {
            _service.Feed(id);
        }

        [HttpPut("drink")]
        public void Drink(int id)
        {
            _service.Drink(id);
        }
    }
}
