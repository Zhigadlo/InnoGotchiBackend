using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Services;
using InnoGotchi.Web.Mapper;
using InnoGotchi.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi.Web.Controllers
{
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private UserService _service;
        private IMapper _mapper;
        public UsersController(UserService service)
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
        public IActionResult Create(UserModel user)
        {
            var userDTO = _mapper.Map<UserDTO>(user);
            int result = _service.Create(userDTO);
            if (result != -1)
                return Ok(result);
            else
                return BadRequest();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
        [HttpPut]
        public IActionResult Update(UserModel user)
        {
            var userDTO = _mapper.Map<UserDTO>(user);
            _service.Update(userDTO);
            return Ok();
        }
    }
}
