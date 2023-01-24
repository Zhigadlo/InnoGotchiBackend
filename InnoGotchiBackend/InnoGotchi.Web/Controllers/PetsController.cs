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

        [HttpGet("{page}&{sortType}&{age}&{year}&{hungerLavel}&{feedingPeriod}&{thirstyLavel}&{drinkingPeriod}")]
        public IActionResult GetPage(int page, string sortType, long age = 0, long year = 0,
            long hungerLavel = -1, long feedingPeriod = 0, long thirstyLavel = -1, long drinkingPeriod = 0)
        {
            return Ok(_service.GetPage(page, sortType, age, year, hungerLavel, feedingPeriod, thirstyLavel, drinkingPeriod));
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
        public IActionResult Create([FromForm] PetModel pet)
        {
            var petDTO = _mapper.Map<PetDTO>(pet);
            petDTO.CreateTime = DateTime.UtcNow;
            petDTO.FirstHappinessDate = DateTime.UtcNow;
            petDTO.LastDrinkingTime = DateTime.UtcNow;
            petDTO.LastFeedingTime = DateTime.UtcNow;
            var result = _service.Create(petDTO);
            if (result != -1)
                return Ok(result);
            else
                return BadRequest();
        }

        [HttpPut]
        public IActionResult Update(int id, string newName)
        {
            PetDTO? petDTO = _service.Get(id);

            if (petDTO != null)
            {
                petDTO.Name = newName;
                if (_service.Update(petDTO))
                    return Ok();
                else
                    return BadRequest();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_service.Delete(id))
                return Ok();
            else
                return BadRequest();
        }

        [HttpPut("feed/{id}")]
        public void Feed(int id)
        {
            _service.Feed(id);
        }

        [HttpPut("drink/{id}")]
        public void Drink(int id)
        {
            _service.Drink(id);
        }
    }
}
