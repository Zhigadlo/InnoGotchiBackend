﻿using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Entities;
using InnnoGotchi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnnoGotchi.DAL.Respositories
{
    /// <summary>
    /// Represents repository that has full access to user entities in database
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        private InnoGotchiContext _context;
        public UserRepository(InnoGotchiContext context)
        {
            _context = context;
        }

        public bool Contains(Func<User, bool> predicate)
        {
            User? user = AllItems().FirstOrDefault(predicate);
            if (user == null)
                return false;

            return true;
        }

        public void Create(User item)
        {
            _context.Users.Add(item);
        }

        public bool Delete(int id)
        {
            User? user = Get(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            return true;
        }

        public IEnumerable<User> FindAll(Func<User, bool> expression, bool isTracking = true)
        {
            return AllItems(isTracking).Where(expression);
        }

        public User? Get(int id, bool isTracking = true)
        {
            return AllItems(isTracking).FirstOrDefault(u => u.Id == id);
        }

        public IQueryable<User> AllItems(bool isTracking = true)
        {
            var items = _context.Users.Include(u => u.CollaboratedFarms).Include(u => u.SentRequests).Include(u => u.ReceivedRequests).Include(u => u.Farm);
            return isTracking ? items : items.AsNoTracking();
        }

        public void Update(User item)
        {
            _context.Users.Update(item);
        }
    }
}
