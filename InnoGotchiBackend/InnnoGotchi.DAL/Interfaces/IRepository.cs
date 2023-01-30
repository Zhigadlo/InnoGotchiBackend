namespace InnnoGotchi.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T? Get(int id);
        IQueryable<T> FindAll(Func<T, bool> predicate);
        T? FirstOrDefault(Func<T, bool> predicate);
        void Create(T item);
        void Update(T item);
        bool Delete(int id);
        bool Contains(Func<T, bool> predicate);
    }
}
