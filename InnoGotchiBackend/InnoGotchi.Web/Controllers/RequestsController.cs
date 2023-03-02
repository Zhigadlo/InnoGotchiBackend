using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Services;
using InnoGotchi.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Requests")]
    public class RequestsController : Controller
    {
        private RequestService _service;
        private IMapper _mapper;
        public RequestsController(RequestService service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }
        /// <summary>
        /// Gets all coloboration requests
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ColoborationRequestDTO>), 200)]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll(false));
        }
        /// <summary>
        /// Gets coloboration request by id
        /// </summary>
        /// <param name="id">Request id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ColoborationRequestDTO), 200)]
        public IActionResult Get(int id)
        {
            return Ok(_service.Get(id, false));
        }
        /// <summary>
        /// Deletes coloboration request by id
        /// </summary>
        /// <param name="id">Request id</param>
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
        /// Creates coloboration request
        /// </summary>
        /// <param name="request">Request model</param>
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAsync(RequestModel request)
        {
            var requestDTO = _mapper.Map<ColoborationRequestDTO>(request);
            int result = await _service.CreateAsync(requestDTO);
            if (result != -1)
                return Ok(result);

            return BadRequest();
        }
        /// <summary>
        /// Confirms request by id
        /// </summary>
        /// <param name="id">Request id</param>
        [HttpPut("confirm/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ConfirmRequestAsync(int id)
        {
            if (await _service.ConfirmAsync(id))
                return Ok();

            return BadRequest();
        }
    }
}
