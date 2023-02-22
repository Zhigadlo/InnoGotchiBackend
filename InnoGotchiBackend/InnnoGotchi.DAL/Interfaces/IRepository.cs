using System.Linq.Expressions;

namespace InnnoGotchi.DAL.Interfaces
{
    /// <summary>
    /// Provides functionality to get access to database wherein the type of the data is known
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets all entities from database
        /// </summary>
        IQueryable<T> GetAll();
        /// <summary>
        /// Gets entity by id from database
        /// </summary>
        /// <param name="id"></param>
        T? Get(int id);
        /// <summary>
        /// Finds all entities from database that matches the condition wherein the type of the data is known
        /// </summary>
        /// <param name="predicate"></param>
        IEnumerable<T> FindAll(Func<T, bool> predicate);
        /// <summary>
        /// Finds first entity that matches the condition or returns null
        /// </summary>
        /// <param name="predicate"></param>
        T? FirstOrDefault(Func<T, bool> predicate);
        /// <summary>
        /// Inserts item in database
        /// </summary>
        /// <param name="item"></param>
        void Create(T item);
        /// <summary>
        /// Updates item in database
        /// </summary>
        /// <param name="item"></param>
        void Update(T item);
        /// <summary>
        /// Removes item from database by id
        /// </summary>
        /// <param name="id"></param>
        bool Delete(int id);
        /// <summary>
        /// Returns true if there is item in database that matches the condition or returns false
        /// </summary>
        /// <param name="predicate"></param>
        bool Contains(Func<T, bool> predicate);
    }
}
