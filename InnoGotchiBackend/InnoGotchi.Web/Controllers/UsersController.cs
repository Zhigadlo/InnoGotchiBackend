using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Models;
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
        private IConfiguration _configuration;
        public UsersController(UserService service,
                               IConfiguration configuration)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new ViewModelProfile()));
            _mapper = config.CreateMapper();
            _service = service;
            _configuration = configuration;
        }
        /// <summary>
        /// Gets all users
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }
        /// <summary>
        /// Gets all emails that already in use
        /// </summary>
        [AllowAnonymous]
        [HttpGet("getAllEmails")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public IActionResult GetAllEmails()
        {
            return Ok(_service.GetAll().Select(u => u.Email));
        }
        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <param name="id">User id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        public IActionResult Get(int id)
        {
            return Ok(_service.Get(id));
        }
        /// <summary>
        /// Gets all coloborators by user id
        /// </summary>
        /// <param name="id">User id</param>
        [HttpGet("coloborators/{id}")]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public IActionResult Coloborators(int id)
        {
            return Ok(_service.Coloborators(id));
        }
        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="user">User model</param>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public IActionResult Create([FromForm] UserModel user)
        {
            var userDTO = _mapper.Map<UserDTO>(user);
            int result = _service.Create(userDTO);
            if (result != -1)
                return Ok(result);
            else
                return BadRequest();
        }
        /// <summary>
        /// Deletes user by id
        /// </summary>
        /// <param name="id">User id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Delete(int id)
        {
            if (_service.Delete(id))
                return Ok();

            return BadRequest();
        }
        /// <summary>
        /// Updates user
        /// </summary>
        /// <param name="user">User model</param>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Update(UserModel user)
        {
            var userDTO = _mapper.Map<UserDTO>(user);
            if (_service.Update(userDTO))
                return Ok();
            else
                return BadRequest();
        }
        /// <summary>
        /// Updates user avatar by user id
        /// </summary>
        /// <param name="Id">User id</param>
        /// <param name="Avatar">New avatar</param>
        [HttpPut("avatarChange")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AvatarUpdate(int Id, byte[] Avatar)
        {
            if (_service.UpdateAvatar(Id, Avatar))
                return Ok();
            else
                return BadRequest();
        }
        /// <summary>
        /// Updates user password by id
        /// </summary>
        /// <param name="Id">User id</param>
        /// <param name="OldPassword">Old password</param>
        /// <param name="NewPassword">New password</param>
        /// <param name="ConfirmPassword">Confirmation of new password</param>
        [HttpPut("passwordChange")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PasswordUpdate(int Id, string OldPassword,
                                            string NewPassword, string ConfirmPassword)
        {
            if (_service.UpdatePassword(Id, OldPassword, NewPassword, ConfirmPassword))
                return Ok();
            else
                return BadRequest();
        }

        /// <summary>
        /// Gets authorized user
        /// </summary>
        [HttpGet("authUser")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAuthorizedUser()
        {
            var userId = User.FindFirstValue(nameof(SecurityTokenModel.UserId));
            if (userId.IsNullOrEmpty())
                return NotFound();
            return Json(_service.Get(int.Parse(userId)));
        }
        /// <summary>
        /// Gets jwt token for user by email and password
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        [AllowAnonymous]
        [HttpPost("token")]
        [ProducesResponseType(typeof(SecurityTokenModel), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Token(string email, string password)
        {
            var identityTokenModel = GetIdentityTokenModel(email, password);
            if (identityTokenModel == null)
            {
                return BadRequest(new { errorText = "Invalid email or password." });
            }
            var authOptions = new AuthOptions(_configuration);
            var now = DateTime.UtcNow;
            var expiredAt = now.AddHours(authOptions.Lifetime);
            var jwt = new JwtSecurityToken(
                    issuer: authOptions.Issuer,
                    audience: authOptions.Audience,
                    notBefore: now,
                    claims: identityTokenModel.Identity.Claims,
                    expires: expiredAt,
                    signingCredentials: new SigningCredentials(authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var token = new SecurityTokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                UserId = identityTokenModel.Token.UserId,
                FarmId = identityTokenModel.Token.FarmId,
                UserName = identityTokenModel.Token.UserName,
                ExpireAt = expiredAt,
                Email = identityTokenModel.Token.Email
            };

            return Json(token);
        }
        /// <summary>
        /// Gets user by email and password
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        [AllowAnonymous]
        [HttpGet("{email}&{password}")]
        public UserDTO? GetUser(string email, string password)
        {
            return _service.FindUserByEmailAndPassword(email, password);
        }
        /// <summary>
        /// Gets IdentityTokenModel
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        private IdentityTokenModel GetIdentityTokenModel(string email, string password)
        {
            UserDTO? person = _service.FindUserByEmailAndPassword(email, password);
            if (person != null)
            {
                var tokenModel = new SecurityTokenModel()
                {
                    Email = person.Email,
                    UserId = person.Id,
                    FarmId = person.Farm == null ? -1 : person.Farm.Id,
                    UserName = person.FirstName + " " + person.LastName
                };

                var claims = new List<Claim>
                {
                    new Claim(nameof(SecurityTokenModel.Email), tokenModel.Email),
                    new Claim(nameof(SecurityTokenModel.UserId), tokenModel.UserId.ToString()),
                    new Claim(nameof(SecurityTokenModel.FarmId), tokenModel.FarmId.ToString()),
                    new Claim(nameof(SecurityTokenModel.UserName), tokenModel.UserName.ToString()),
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return new IdentityTokenModel() { Token = tokenModel, Identity = claimsIdentity };
            }

            return null;
        }
    }
}
