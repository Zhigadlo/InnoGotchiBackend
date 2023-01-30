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
        /// <summary>
        /// Gets PaginatedList object by page with sorting, filtration and page pets list and returns this object.
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="sortType">Sort type</param>
        /// <param name="filterModel">Filter model</param>

        [HttpGet("getPage")]
        [ProducesResponseType(typeof(PaginatedList<PetDTO>) ,200)]
        public IActionResult GetPage(int page, string sortType, PetFilterModel filterModel)
        {
            return Ok(_service.GetPage(page, sortType, filterModel));
        }
        /// <summary>
        /// Returns all pets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PetDTO>), 200)]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }
        /// <summary>
        /// Returns all pet names
        /// </summary>
        [HttpGet("getAllNames")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public IActionResult GetAllNames()
        {
            return Ok(_service.GetAll().Select(p => p.Name));
        }
        /// <summary>
        /// Gets pet by id 
        /// </summary>
        /// <param name="id">Pet id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PetDTO), 200)]
        public IActionResult Get(int id)
        {
            return Ok(_service.Get(id));
        }
        /// <summary>
        /// Creates pet and returns created pet id
        /// </summary>
        /// <param name="pet">Pet model</param>
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
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
        /// <summary>
        /// Updates pet name by id
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <param name="newName">New pet name</param>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
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
        /// <summary>
        /// Sets death time to pet by id
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <param name="deathTime">Death time</param>
        [HttpPut("death/{id}&{deathTime}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
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
        /// <summary>
        /// Delets pet by id
        /// </summary>
        /// <param name="id">Pet id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Delete(int id)
        {
            if (_service.Delete(id))
                return Ok();
            else
                return BadRequest();
        }
        /// <summary>
        /// Feeds pet by id
        /// </summary>
        /// <param name="id">Pet id</param>
        [HttpPut("feed/{id}")]
        public void Feed(int id)
        {
            _service.Feed(id);
        }
        /// <summary>
        /// Gives a drink to pet by id
        /// </summary>
        /// <param name="id">Pet id</param>
        [HttpPut("drink/{id}")]
        public void Drink(int id)
        {
            _service.Drink(id);
        }
    }
}
