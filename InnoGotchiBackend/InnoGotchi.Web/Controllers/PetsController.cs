using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi.Web.Controllers
{
    [Route("api/Pets")]
    public class PetsController : Controller
    {
        private PetService _service;

        public PetsController(PetService service)
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
        [HttpPost]
        public IActionResult Create(PetDTO pet)
        {
            return Ok(_service.Create(pet));
        }

        [HttpPut("{id}")]
        public IActionResult Update(PetDTO item)
        {
            _service.Update(item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }

        [HttpPut("feed")]
        public void Feed(int id)
        {
            _service.Feed(id);
        }
        [HttpPut("drink")]
        public void Drink(int id)
        {
            _service.Drink(id);
        }
    }
}
