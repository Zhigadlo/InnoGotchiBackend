using AutoMapper;
using InnoGotchi.BLL.DTO;
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create([FromForm] UserModel user)
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
            if (_service.Delete(id))
                return Ok();

            return BadRequest();
        }
        [HttpPut]
        public IActionResult Update(UserModel user)
        {
            var userDTO = _mapper.Map<UserDTO>(user);
            if (_service.Update(userDTO))
                return Ok();
            else
                return BadRequest();
        }

        [HttpPut("avatarChange")]
        public IActionResult AvatarUpdate(int Id, byte[] Avatar)
        {
            if (_service.UpdateAvatar(Id, Avatar))
                return Ok();
            else
                return BadRequest();
        }

        [HttpPut("passwordChange")]
        public IActionResult PasswordUpdate(int Id, string OldPassword,
                                            string NewPassword, string ConfirmPassword)
        {
            if (_service.UpdatePassword(Id, OldPassword, NewPassword, ConfirmPassword))
                return Ok();
            else
                return BadRequest();
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
                    expires: now.AddHours(AuthOptions.LIFETIME),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return Json(new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        [AllowAnonymous]
        [HttpGet("{email}&{password}")]
        public UserDTO? GetUser(string email, string password)
        {
            return _service.FindUserByEmailAndPassword(email, password);
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
