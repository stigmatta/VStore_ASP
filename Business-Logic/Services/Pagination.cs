using Microsoft.Extensions.Logging;

namespace Business_Logic.Services
{
    public interface IPaginationService<T> where T : class
    {
        (IEnumerable<T?> Items, int TotalCount) Paginate(
            IEnumerable<T?> source,
            int pageIndex,
            int pageSize);
    }

    public class PaginationService<T> : IPaginationService<T> where T : class
    {
        private readonly ILogger<PaginationService<T>> _logger;
        public PaginationService(ILogger<PaginationService<T>> logger)
        {
            _logger = logger;
        }
        public (IEnumerable<T?> Items, int TotalCount) Paginate(
            IEnumerable<T?> source,
            int pageIndex,
            int pageSize)
        {
            var totalCount = source.Count();

            if (totalCount == 0 || pageIndex * pageSize >= totalCount)
                return (Enumerable.Empty<T>(), totalCount);

            var paginatedItems = source
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            return (paginatedItems, totalCount);
        }
    }
}