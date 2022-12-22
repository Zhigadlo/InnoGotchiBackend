using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;
using InnoGotchi.BLL.Services;
using InnoGotchi.Web.Mapper;
using InnoGotchi.Web.Models;
using InnoGotchi.Web.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InnoGotchi.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
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

        [Authorize]
        [HttpGet("authUser")]
        public IActionResult GetAuthorizedUser()
        {
            return Json(_service.Get(int.Parse(User.FindFirstValue("user_id"))));
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult Token(string email, string password)
        {
            var identity = GetIdentity(email, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid email or password." });
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return Json(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
        private ClaimsIdentity GetIdentity(string email, string password)
        {
            UserDTO? person = _service.FindUserByEmailAndPassword(email, password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
                    new Claim("user_id", person.Id.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
