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
    [Route("api/Requests")]
    public class RequestsController : Controller
    {
        private RequestService _service;
        private IMapper _mapper;
        public RequestsController(RequestService service)
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_service.Delete(id))
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult Create(RequestModel request)
        {
            var requestDTO = _mapper.Map<ColoborationRequestDTO>(request);
            requestDTO.IsConfirmed = false;
            requestDTO.Date = DateTime.UtcNow;
            int result = _service.Create(requestDTO);
            if (result != -1)
                return Ok(result);
            else
                return BadRequest();
        }
        [HttpPut("confirm/{id}")]
        public IActionResult ConfirmRequest(int id)
        {
            ColoborationRequestDTO? request = _service.Get(id);
            if (request != null)
            {
                request.IsConfirmed = true;
                if (_service.Update(request))
                    return Ok();
                else
                    return BadRequest();
            }
            else
                return BadRequest();
        }
    }
}
