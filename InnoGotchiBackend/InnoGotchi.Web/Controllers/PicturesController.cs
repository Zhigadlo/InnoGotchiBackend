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
    [Route("api/Pictures")]
    public class PicturesController : Controller
    {
        private PictureService _service;
        private IMapper _mapper;

        public PicturesController(PictureService service)
        {
            _service = service;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new ViewModelProfile()));
            _mapper = config.CreateMapper();
        }
        /// <summary>
        /// Gets all pictures
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PictureDTO>), 200)]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }
        /// <summary>
        /// Gets picture by id
        /// </summary>
        /// <param name="id">Picture id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PictureDTO), 200)]
        public IActionResult Get(int id)
        {
            return Ok(_service.Get(id));
        }
        /// <summary>
        /// Creates picture
        /// </summary>
        /// <param name="picture">Picture model</param>
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public IActionResult Create(PictureModel picture)
        {
            var pictureDTO = _mapper.Map<PictureDTO>(picture);
            var result = _service.Create(pictureDTO);
            if (result != -1)
                return Ok(result);
            else
                return BadRequest();
        }
        /// <summary>
        /// Deletes picture by id
        /// </summary>
        /// <param name="id">Picture id</param>
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Delete(int id)
        {
            if (_service.Delete(id))
                return Ok();
            else
                return BadRequest();
        }
    }
}
