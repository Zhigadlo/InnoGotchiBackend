using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Models;
using InnoGotchi.BLL.Services;
using InnoGotchi.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
                               IConfiguration configuration,
                               IMapper mapper)
        {
            _mapper = mapper;
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
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(await _service.GetAsync(id));
        }
        /// <summary>
        /// Gets all coloborators by user id
        /// </summary>
        /// <param name="id">User id</param>
        [HttpGet("coloborators/{id}")]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public async Task<IActionResult> ColoboratorsAsync(int id)
        {
            return Ok(await _service.ColoboratorsAsync(id));
        }
        /// <summary>
        /// Gets users that sent coloboration requests to user by id
        /// </summary>
        /// <param name="id">User id</param>
        [HttpGet("sentRequestUsers/{id}")]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public IActionResult GetUsersThatSentRequest(int id)
        {
            return Ok(_service.UsersSentRequestAsync(id));
        }
        /// <summary>
        /// Gets users that received coloboration requests from user by id
        /// </summary>
        /// <param name="id">User that sent request id</param>
        [HttpGet("receivedRequestUsers/{id}")]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public IActionResult GetUsersThatReceivedRequest(int id)
        {
            return Ok(_service.UsersReceivedRequestAsync(id));
        }
        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="user">User model</param>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAsync([FromForm] UserModel user)
        {
            var userDTO = _mapper.Map<UserDTO>(user);
            int result = await _service.CreateAsync(userDTO);
            if (result != -1)
                return Ok(result);

            return BadRequest();
        }
        /// <summary>
        /// Deletes user by id
        /// </summary>
        /// <param name="id">User id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (await _service.DeleteAsync(id))
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
        public async Task<IActionResult> UpdateAsync(UserModel user)
        {
            var userDTO = _mapper.Map<UserDTO>(user);
            if (await _service.UpdateAsync(userDTO))
                return Ok();

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
        public async Task<IActionResult> AvatarUpdateAsync(int Id, byte[] Avatar)
        {
            if (await _service.UpdateAvatarAsync(Id, Avatar))
                return Ok();

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
        public async Task<IActionResult> PasswordUpdateAsync(int Id, string OldPassword,
                                            string NewPassword, string ConfirmPassword)
        {
            if (await _service.UpdatePasswordAsync(Id, OldPassword, NewPassword, ConfirmPassword))
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Gets authorized user
        /// </summary>
        [HttpGet("authUser")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAuthorizedUserAsync()
        {
            var userId = User.FindFirstValue(nameof(SecurityTokenModel.UserId));
            if (userId.IsNullOrEmpty())
                return NotFound();
            return Json(await _service.GetAsync(int.Parse(userId)));
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
        public async Task<IActionResult> TokenAsync(string email, string password)
        {
            var token = await _service.TokenAsync(email, password, _configuration);
            if (token == null)
                return BadRequest(new { errorText = "Invalid email or password." });

            return Json(token);
        }

        /// <summary>
        /// Gets user by email and password
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        [AllowAnonymous]
        [HttpGet("{email}&{password}")]
        public async Task<UserDTO?> GetUserAsync(string email, string password)
        {
            return await _service.FindUserByEmailAndPasswordAsync(email, password);
        }
    }
}
