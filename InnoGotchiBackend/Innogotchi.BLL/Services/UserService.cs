﻿using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Interfaces;
using InnoGotchi.BLL.Validation;
using System.Security.Cryptography;
using System.Text;

namespace InnoGotchi.BLL.Services
{
    public class UserService : IService<UserDTO>
    {
        private IUnitOfWork _database;
        private IMapper _mapper;
        private UserValidator _validator;
        public UserService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
            _validator = new UserValidator();
        }
        public int Create(UserDTO item)
        {
            User? user = _database.Users.First(u => u.Email == item.Email);

            if (user != null)
            {
                throw new Exception("This email is already in use");
            }

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
        public void Update(UserDTO item)
        {
            User user = _mapper.Map<User>(item);
            user.PasswordHash = PasswordToHash(item.Password);
            var result = _validator.Validate(user);
            if (result.IsValid)
            {
                _database.Users.Update(user);
                _database.SaveChanges();
            }
        }
        
        public UserDTO FindUserByEmailAndPassword(string email, string password)
        {
            User? user = _database.Users.First(u => u.Email == email && u.PasswordHash == PasswordToHash(password));
            if (user != null)
                return _mapper.Map<UserDTO>(user);
            else
                return null;
        }

        public void Delete(int id)
        {
            _database.Users.Delete(id);
            _database.SaveChanges();
        }
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
    }
}
