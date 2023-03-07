using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Models;
using InnoGotchi.BLL.Options;
using InnoGotchi.BLL.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InnoGotchi.BLL.Services
{
    /// <summary>
    /// Represents service that get access to user entities
    /// </summary>
    public class UserService
    {
        private InnoGotchiUnitOfWork _database;
        private IMapper _mapper;
        private UserValidator _validator;
        public UserService(InnoGotchiUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
            _validator = new UserValidator();
        }
        public async Task<int> CreateAsync(UserDTO item)
        {
            if (await _database.Users.ContainsAsync(u => u.Email == item.Email))
                throw new Exception("This email is already in use");

            User newUser = _mapper.Map<User>(item);
            var result = _validator.Validate(newUser);

            if (!result.IsValid)
                return -1;

            newUser.PasswordHash = PasswordToHash(item.Password);
            _database.Users.Create(newUser);
            await _database.SaveChangesAsync();
            return newUser.Id;

        }
        public async Task<UserDTO?> GetAsync(int id, bool isTracking = true)
        {
            User? user = await _database.Users.GetAsync(id, isTracking);
            return _mapper.Map<UserDTO>(user);
        }
        public IEnumerable<UserDTO> GetAll(bool isTracking = true)
        {
            return _mapper.Map<IEnumerable<UserDTO>>(_database.Users.AllItems(isTracking));
        }
        public async Task<bool> UpdateAsync(UserDTO item)
        {
            User user = _mapper.Map<User>(item);
            user.PasswordHash = PasswordToHash(item.Password);
            var result = _validator.Validate(user);
            if (!result.IsValid)
                return false;

            _database.Users.Update(user);
            await _database.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Updates user avatar by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newAvatar"></param>
        public async Task<bool> UpdateAvatarAsync(int id, byte[] newAvatar)
        {
            User? user = await _database.Users.GetAsync(id);
            if (user == null)
                return false;

            user.Avatar = newAvatar;
            var result = _validator.Validate(user);
            if (!result.IsValid)
                return false;

            _database.Users.Update(user);
            await _database.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        public async Task<bool> UpdatePasswordAsync(int id, string oldPassword, string newPassword, string confirmPassword)
        {
            User? user = await _database.Users.GetAsync(id);
            if (user == null || user.PasswordHash != PasswordToHash(oldPassword) || newPassword != confirmPassword)
                return false;

            user.PasswordHash = PasswordToHash(newPassword);
            _database.Users.Update(user);
            await _database.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Finds and returns user by email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public async Task<UserDTO> FindUserByEmailAndPasswordAsync(string email, string password)
        {
            User? user = await _database.Users.AllItems().FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == PasswordToHash(password));
            return _mapper.Map<UserDTO>(user);
        }
        /// <summary>
        /// Returns all coloborators for user by his id 
        /// </summary>
        /// <param name="userId"></param>
        public async Task<IEnumerable<UserDTO>?> ColoboratorsAsync(int userId)
        {
            var requests = _database.Requests.FindAll(r => r.IsConfirmed && (r.RequestOwnerId == userId || r.RequestReceipientId == userId));

            var coloborators = new List<UserDTO>();
            foreach (var r in requests)
            {
                if (r.RequestOwnerId == userId)
                {
                    var user = await _database.Users.GetAsync(r.RequestReceipientId);
                    coloborators.Add(_mapper.Map<UserDTO>(user));
                }

                if (r.RequestReceipientId == userId)
                {
                    var user = await _database.Users.GetAsync(r.RequestOwnerId);
                    coloborators.Add(_mapper.Map<UserDTO>(user));
                }
            }
            return coloborators.AsEnumerable();
        }
        /// <summary>
        /// Gets all users that sent request to user by id
        /// </summary>
        /// <param name="id">User that gets requests id</param>
        public async Task<IEnumerable<UserDTO>?> UsersSentRequestAsync(int id)
        {
            var usersSentRequests = new List<UserDTO>();
            var user = await GetAsync(id);
            foreach (var rr in user.ReceivedRequests)
            {
                if (rr.IsConfirmed == false)
                {
                    var u = await GetAsync(rr.RequestOwnerId);
                    usersSentRequests.Add(u);
                }
            };

            return usersSentRequests.AsEnumerable();
        }
        /// <summary>
        /// Gets all users that received requests from user by id
        /// </summary>
        /// <param name="id">User that sent request id</param>
        public async Task<IEnumerable<UserDTO>?> UsersReceivedRequestAsync(int id)
        {
            var usersReceivedRequests = new List<UserDTO>();
            var user = await GetAsync(id);
            foreach (var sr in user.SentRequests)
            {
                if (sr.IsConfirmed == false)
                {
                    var u = await GetAsync(sr.RequestReceipientId);
                    usersReceivedRequests.Add(u);
                }
            };

            return usersReceivedRequests.AsEnumerable();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _database.Pets.ContainsAsync(p => p.Id == id))
                return false;

            var isDeleted = await _database.Users.DeleteAsync(id);
            if (!isDeleted)
                return false;

            await _database.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Encryptes the password
        /// </summary>
        /// <param name="password"></param>
        private string PasswordToHash(string password)
        {
            using (var hashAlg = MD5.Create())
            {
                byte[] hash = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("X2"));
                }
                return builder.ToString();
            }
        }

        public async Task<SecurityTokenModel?> TokenAsync(string email, string password, IOptions<TokenSettings> tokenSettings)
        {
            UserDTO? person = await FindUserByEmailAndPasswordAsync(email, password);

            if (person == null)
                return null;

            var farmId = person.Farm == null ? -1 : person.Farm.Id;
            var userName = person.FirstName + " " + person.LastName;

            var claims = new List<Claim>
                {
                    new Claim(nameof(SecurityTokenModel.Email), person.Email),
                    new Claim(nameof(SecurityTokenModel.UserId), person.Id.ToString()),
                    new Claim(nameof(SecurityTokenModel.FarmId), farmId.ToString()),
                    new Claim(nameof(SecurityTokenModel.UserName), userName.ToString()),
                };

            var authOptions = new AuthOptions(tokenSettings);
            var now = DateTime.UtcNow;
            var expiredAt = now.AddHours(authOptions.Lifetime);
            var jwt = new JwtSecurityToken(
                    issuer: authOptions.Issuer,
                    audience: authOptions.Audience,
                    notBefore: now,
                    claims: claims,
                    expires: expiredAt,
                    signingCredentials: new SigningCredentials(authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var token = new SecurityTokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                UserId = person.Id,
                FarmId = farmId,
                UserName = userName,
                ExpireAt = expiredAt,
                Email = person.Email
            };

            return token;
        }
    }
}
