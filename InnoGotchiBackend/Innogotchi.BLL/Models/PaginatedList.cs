namespace InnoGotchi.BLL.Models
{
    public class PaginatedList<T> where T : class
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public IEnumerable<T> Items { get; private set; }

        public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }
    }
}
