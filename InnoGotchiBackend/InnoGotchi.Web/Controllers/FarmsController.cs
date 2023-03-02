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
    [Route("api/Farms")]
    public class FarmsController : Controller
    {
        private FarmService _service;
        private IMapper _mapper;
        public FarmsController(FarmService service)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new ViewModelProfile()));
            _mapper = config.CreateMapper();
            _service = service;
        }
        /// <summary>
        /// Gets all farms
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FarmDTO>), 200)]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }
        /// <summary>
        /// Gets all farm names
        /// </summary>
        [HttpGet("getAllNames")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public IActionResult GetAllNames()
        {
            return Ok(_service.GetAll().Select(f => f.Name));
        }
        /// <summary>
        /// Gets farm by id
        /// </summary>
        /// <param name="id">Farm id</param>

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FarmDTO), 200)]
        public IActionResult Get(int id)
        {
            return Ok(_service.Get(id));
        }
        /// <summary>
        /// Creates farm and returns created farm id
        /// </summary>
        /// <param name="farm">Farm model</param>
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create(FarmModel farm)
        {
            int result = await _service.CreateAsync(_mapper.Map<FarmDTO>(farm));
            if (result == -1)
                return BadRequest();

            return Ok(result);
        }
        /// <summary>
        /// Deletes farm by id
        /// </summary>
        /// <param name="id">Farm id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _service.DeleteAsync(id))
                return Ok();

            return BadRequest();
        }
        /// <summary>
        /// Updates farm
        /// </summary>
        /// <param name="farm">Farm model</param>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateAsync(FarmModel farm)
        {
            var farmDTO = _mapper.Map<FarmDTO>(farm);
            if (await _service.UpdateAsync(farmDTO))
                return Ok();

            return BadRequest();
        }
    }
}
