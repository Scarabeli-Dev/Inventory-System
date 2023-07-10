using Microsoft.EntityFrameworkCore;

namespace Inventory.Helpers
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PageList() { }

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync(
            IQueryable<T> source, int pageNumber, int pageSize
        )
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
            return new PageList<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<PageList<T>> ListCreateAsync(
    List<T> source, int pageNumber, int pageSize
    )
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}