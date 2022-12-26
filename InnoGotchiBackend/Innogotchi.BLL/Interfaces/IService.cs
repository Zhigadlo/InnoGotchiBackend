namespace InnoGotchi.BLL.Interfaces
{
    public interface IService<T> where T : class
    {
        int Create(T item);
        T? Get(int id);
        IEnumerable<T> GetAll();
        bool Update(T item);
        bool Delete(int id);
    }
}
