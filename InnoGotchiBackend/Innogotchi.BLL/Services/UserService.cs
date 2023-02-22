using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;
using InnoGotchi.BLL.Models;
using InnoGotchi.BLL.Options;
using InnoGotchi.BLL.Validation;
using Microsoft.Extensions.Configuration;
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
    public class UserService : IService<UserDTO>
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
        public int Create(UserDTO item)
        {
            if (_database.Users.Contains(u => u.Email == item.Email))
                throw new Exception("This email is already in use");

            User newUser = _mapper.Map<User>(item);
            var result = _validator.Validate(newUser);
            if (result.IsValid)
            {
                newUser.PasswordHash = PasswordToHash(item.Password);
                _database.Users.Create(newUser);
                _database.SaveChanges();
                return newUser.Id;
            }
            else
                return -1;
        }
        public UserDTO? Get(int id)
        {
            User? user = _database.Users.Get(id);
            if (user == null)
                return null;
            else
                return _mapper.Map<UserDTO>(user);
        }
        public IEnumerable<UserDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<UserDTO>>(_database.Users.GetAll());
        }
        public bool Update(UserDTO item)
        {
            User user = _mapper.Map<User>(item);
            user.PasswordHash = PasswordToHash(item.Password);
            var result = _validator.Validate(user);
            if (result.IsValid)
            {
                _database.Users.Update(user);
                _database.SaveChanges();
                return true;
            }

            return false;
        }
        /// <summary>
        /// Updates user avatar by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newAvatar"></param>
        public bool UpdateAvatar(int id, byte[] newAvatar)
        {
            User? user = _database.Users.GetAll().FirstOrDefault(u => u.Id == id);
            user.Avatar = newAvatar;
            var result = _validator.Validate(user);
            if (result.IsValid)
            {
                _database.Users.Update(user);
                _database.SaveChanges();
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        public bool UpdatePassword(int id, string oldPassword, string newPassword, string confirmPassword)
        {
            User? user = _database.Users.Get(id);
            if (user != null && user.PasswordHash == PasswordToHash(oldPassword) && newPassword == confirmPassword)
            {
                user.PasswordHash = PasswordToHash(newPassword);
                _database.Users.Update(user);
                _database.SaveChanges();
                return true;
            }

            return false;
        }
        /// <summary>
        /// Finds and returns user by email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public UserDTO FindUserByEmailAndPassword(string email, string password)
        {
            User? user = _database.Users.GetAll().FirstOrDefault(u => u.Email == email && u.PasswordHash == PasswordToHash(password));
            if (user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }
            else
                return null;
        }
        /// <summary>
        /// Returns all coloborators for user by his id 
        /// </summary>
        /// <param name="userId"></param>
        public IEnumerable<UserDTO>? Coloborators(int userId)
        {
            var requests = _database.Requests.FindAll(r => r.IsConfirmed && (r.RequestOwnerId == userId || r.RequestReceipientId == userId));

            var coloborators = new List<UserDTO>();
            foreach (var r in requests)
            {
                if (r.RequestOwnerId == userId)
                {
                    var user = _database.Users.Get(r.RequestReceipientId);
                    coloborators.Add(_mapper.Map<UserDTO>(user));
                }


                if (r.RequestReceipientId == userId)
                {
                    var user = _database.Users.Get(r.RequestOwnerId);
                    coloborators.Add(_mapper.Map<UserDTO>(user));
                }
            }
            return coloborators.AsEnumerable();
        }
        /// <summary>
        /// Gets all users that sent request to user by id
        /// </summary>
        /// <param name="id">User that gets requests id</param>
        public IEnumerable<UserDTO>? UsersSentRequest(int id)
        {
            var usersSentRequests = new List<UserDTO>();
            var user = Get(id);
            foreach (var rr in user.ReceivedRequests)
            {
                if (rr.IsConfirmed == false)
                {
                    var u = Get(rr.RequestOwnerId);
                    usersSentRequests.Add(u);
                }
            };

            return usersSentRequests.AsEnumerable();
        }
        /// <summary>
        /// Gets all users that received requests from user by id
        /// </summary>
        /// <param name="id">User that sent request id</param>
        public IEnumerable<UserDTO>? UsersReceivedRequest(int id)
        {
            var usersReceivedRequests = new List<UserDTO>();
            var user = Get(id);
            foreach (var sr in user.SentRequests)
            {
                if (sr.IsConfirmed == false)
                {
                    var u = Get(sr.RequestReceipientId);
                    usersReceivedRequests.Add(u);
                }
            };

            return usersReceivedRequests.AsEnumerable();
        }

        public bool Delete(int id)
        {
            if (_database.Pets.Contains(p => p.Id == id))
            {
                var isDeleted = _database.Users.Delete(id);
                if (!isDeleted)
                    return false;

                _database.SaveChanges();
                return true;
            }
            return false;
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

        public SecurityTokenModel? Token(string email, string password, IConfiguration configuration)
        {
            UserDTO? person = FindUserByEmailAndPassword(email, password);

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

            var authOptions = new AuthOptions(configuration);
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
