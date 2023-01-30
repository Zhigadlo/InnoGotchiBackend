using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Models;
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

        [HttpGet("getPage")]
        public IActionResult GetPage(int page, string sortType, PetFilterModel filterModel)
        {
            return Ok(_service.GetPage(page, sortType, filterModel));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("getAllNames")]
        public IActionResult GetAllNames()
        {
            return Ok(_service.GetAll().Select(p => p.Name));
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

        [HttpPut("death/{id}&{deathTime}")]
        public IActionResult Death(int id, long deathTime)
        {
            PetDTO? petDTO = _service.Get(id);
            if (petDTO != null)
            {
                if (petDTO.DeathTime != DateTime.MinValue)
                    return Ok();
                petDTO.DeathTime = DateTime.MinValue.AddTicks(deathTime);
                petDTO.FirstHappinessDate = DateTime.MinValue;
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
