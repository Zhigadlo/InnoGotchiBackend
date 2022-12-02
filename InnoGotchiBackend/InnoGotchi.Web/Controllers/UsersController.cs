using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi.Web.Controllers
{
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private UserService _service;
        public UsersController(UserService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }
        [HttpGet]
        public IActionResult Get(int id)
        {
            return Ok(_service.Get(id));
        }
        [HttpPost]
        public IActionResult Create(UserDTO user)
        {
            int result = _service.Create(user);
            if (result != -1)
                return Ok(result);
            else
                return BadRequest();
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
        [HttpPut]
        public IActionResult Update(UserDTO user)
        {
            _service.Update(user);
            return Ok();
        }
    }
}
