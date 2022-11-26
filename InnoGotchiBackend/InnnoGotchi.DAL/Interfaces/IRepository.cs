﻿namespace InnnoGotchi.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? Get(int id);
        IEnumerable<T> Find(Func<T, bool> predicate);
        T? First(Func<T, bool> predicate);
        void Create(T item);
        void Update(int id, T item);
        void Delete(int id);
    }
}
