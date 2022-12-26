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
        public IActionResult Create(FarmModel farm)
        {
            var farmDTO = _mapper.Map<FarmDTO>(farm);
            farmDTO.CreateTime = DateTime.Now;
            int result = _service.Create(farmDTO);
            if (result != -1)
                return Ok(result);
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
        [HttpPut]
        public IActionResult Update(FarmModel farm)
        {
            var farmDTO = _mapper.Map<FarmDTO>(farm);
            if(_service.Update(farmDTO))
                return Ok();
            else
                return BadRequest();
        }
    }
}
