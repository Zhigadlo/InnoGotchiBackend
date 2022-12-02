using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi.Web.Controllers
{
    [Route("api/Farms")]
    public class FarmsController : Controller
    {
        private FarmService _service;
        public FarmsController(FarmService service) 
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
        public IActionResult Create(FarmDTO farm)
        {
            int result = _service.Create(farm);
            if(result != -1)
                return Ok();
            else
                return BadRequest();
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
        public IActionResult Update(FarmDTO farm)
        {
            _service.Update(farm);
            return Ok();
        }
    }
}
