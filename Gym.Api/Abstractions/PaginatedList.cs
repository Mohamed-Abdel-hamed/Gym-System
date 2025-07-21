using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Gym.Api.Abstractions;

public class PaginatedList<T>(List<T> items, int pageNumber,int count,int pageSize)
{
    public List<T> Items { get; private set; } = items;
    public int PageNumber { get; private set; } = pageNumber;
    public int TotalPages { get; private set; }=(int)Math.Ceiling(count / (double)pageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateASync(IQueryable<T> source,int pageNumber,int pageSize,CancellationToken cancellation=default)
    {
        if(pageNumber<1||pageSize<1)
        {
            pageNumber = 1;
            pageSize = 10;
        }
        if (pageSize>100 )
        {
            pageSize = 100;
        }

        var count = await source.CountAsync(cancellation);
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellation);


        return new PaginatedList<T>(items,pageNumber,count,pageSize);
    }
}
