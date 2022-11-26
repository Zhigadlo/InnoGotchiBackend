namespace InnoGotchi.BLL.Interfaces
{
    public interface IService<T> where T : class
    {
        void Create(T item);
        T? Get(int id);
        IEnumerable<T> GetAll();
        void Update(int id, T item);
    }
}
