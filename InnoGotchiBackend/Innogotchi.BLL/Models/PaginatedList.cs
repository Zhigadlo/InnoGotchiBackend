namespace InnoGotchi.BLL.Models
{
    /// <summary>
    /// Represents entity that contains all page items and other info about pages
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> where T : class
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public T Items { get; private set; }

        public PaginatedList(T items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }
    }
}
