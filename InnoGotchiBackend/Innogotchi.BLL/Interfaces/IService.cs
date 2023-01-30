namespace InnoGotchi.BLL.Interfaces
{
    /// <summary>
    /// Provides functionality to get access to data access layer wherein the type of the data is known
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IService<T> where T : class
    {
        /// <summary>
        /// Inserts item in database
        /// </summary>
        /// <param name="item"></param>
        int Create(T item);
        /// <summary>
        /// Gets item by id from database
        /// </summary>
        /// <param name="id"></param>
        T? Get(int id);
        /// <summary>
        /// Gets all items from database
        /// </summary>
        IEnumerable<T> GetAll();
        /// <summary>
        /// Updates item in database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Returns true if updating was successfull else returns false</returns>
        bool Update(T item);
        /// <summary>
        /// Deletes item by id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns true if deleting was successfull else returns false</returns>
        bool Delete(int id);
    }
}
