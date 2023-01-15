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
        public IActionResult Create(PictureModel picture)
        {
            var pictureDTO = _mapper.Map<PictureDTO>(picture);
            var result = _service.Create(pictureDTO);
            if (result != -1)
                return Ok(result);
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (_service.Delete(id))
                return Ok();
            else
                return BadRequest();
        }
    }
}
