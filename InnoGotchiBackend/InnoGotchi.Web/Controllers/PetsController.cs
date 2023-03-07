using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Models;
using InnoGotchi.BLL.Services;
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

        public PetsController(PetService service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }
        /// <summary>
        /// Gets PaginatedList object by page with sorting, filtration and page pets list and returns this object.
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="sortType">Sort type</param>
        /// <param name="filterModel">Filter model</param>

        [HttpGet("getPage")]
        [ProducesResponseType(typeof(PaginatedList<PetDTO>), 200)]
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
            return Ok(_service.GetAllNames());
        }
        /// <summary>
        /// Gets pet by id 
        /// </summary>
        /// <param name="id">Pet id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PetDTO), 200)]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(await _service.GetAsync(id));
        }
        /// <summary>
        /// Creates pet and returns created pet id
        /// </summary>
        /// <param name="pet">Pet model</param>
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAsync([FromForm] PetModel pet)
        {
            var petDTO = _mapper.Map<PetDTO>(pet);
            var result = await _service.CreateAsync(petDTO);
            if (result == -1)
                return BadRequest();

            return Ok(result);
        }
        /// <summary>
        /// Updates pet name by id
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <param name="newName">New pet name</param>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateNameAsync(int id, string newName)
        {
            if (await _service.UpdateNameAsync(id, newName))
                return Ok();

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
        public async Task<IActionResult> DeathAsync(int id, long deathTime)
        {
            var result = await _service.DeathAsync(id, deathTime);
            if (result)
                return Ok();

            return BadRequest();
        }
        /// <summary>
        /// Delets pet by id
        /// </summary>
        /// <param name="id">Pet id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (await _service.DeleteAsync(id))
                return Ok();

            return BadRequest();
        }
        /// <summary>
        /// Feeds pet by id
        /// </summary>
        /// <param name="id">Pet id</param>
        [HttpPut("feed/{id}")]
        public async Task FeedAsync(int id)
        {
            await _service.FeedAsync(id);
        }
        /// <summary>
        /// Gives a drink to pet by id
        /// </summary>
        /// <param name="id">Pet id</param>
        [HttpPut("drink/{id}")]
        public async Task DrinkAsync(int id)
        {
            await _service.DrinkAsync(id);
        }
    }
}
