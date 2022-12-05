using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi.Web.Controllers
{
    [Route("api/Requests")]
    public class RequestsController : Controller
    {
        private RequestService _service;

        public RequestsController(RequestService service)
        {
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }

        [HttpPost]
        public IActionResult Create(ColoborationRequestDTO request)
        {
            int result = _service.Create(request);
            if (result != -1)
                return Ok();
            else
                return BadRequest();
        }
        [HttpPut]
        public IActionResult Update(ColoborationRequestDTO request)
        {
            _service.Update(request);
            return Ok();
        }
    }
}
