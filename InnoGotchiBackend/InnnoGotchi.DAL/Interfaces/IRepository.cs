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
        IQueryable<T> AllItems(bool isTracking = true);
        /// <summary>
        /// Gets entity by id from database
        /// </summary>
        /// <param name="id"></param>
        Task<T?> GetAsync(int id, bool isTracking = true);
        /// <summary>
        /// Finds all entities from database that matches the condition wherein the type of the data is known
        /// </summary>
        /// <param name="predicate"></param>
        IEnumerable<T> FindAll(Func<T, bool> predicate, bool isTracking = true);
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
        Task<bool> DeleteAsync(int id);
        /// <summary>
        /// Returns true if there is item in database that matches the condition or returns false
        /// </summary>
        /// <param name="expression"></param>
        Task<bool> ContainsAsync(Expression<Func<T, bool>> expression);
    }
}
