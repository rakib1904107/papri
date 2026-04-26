using Microsoft.EntityFrameworkCore;

namespace Papri.Services;

public interface IPagedList
{
    int PageIndex { get; }
    int PageSize { get; }
    int TotalCount { get; }
    int TotalPages { get; }
    bool HasPrevious { get; }
    bool HasNext { get; }
}

public class PaginatedList<T> : List<T>, IPagedList
{
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasPrevious => PageIndex > 1;
    public bool HasNext => PageIndex < TotalPages;

    public PaginatedList(IEnumerable<T> items, int totalCount, int pageIndex, int pageSize)
    {
        TotalCount = totalCount;
        PageIndex = pageIndex < 1 ? 1 : pageIndex;
        PageSize = pageSize;
        TotalPages = totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)pageSize);
        AddRange(items);
    }

    public static PaginatedList<T> Empty(int pageSize = 10) =>
        new PaginatedList<T>(Array.Empty<T>(), 0, 1, pageSize);

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageIndex,
        int pageSize,
        CancellationToken ct = default)
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        var count = await source.CountAsync(ct);
        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
