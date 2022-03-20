namespace GoPlayServer.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int currentPage { get; private set; }
        public int totalPages { get; private set; }
        public int pageSize { get; private set; }
        public int totalCount { get; private set; }

        public bool hasPrevious => currentPage > 1;
        public bool hasNext => currentPage < totalPages;

        public PagedList(List<T> items, int count, int pageNumber, int _pageSize)
        {
            totalCount = count;
            pageSize = _pageSize;
            currentPage = pageNumber;
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
