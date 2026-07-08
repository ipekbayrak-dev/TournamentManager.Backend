using Microsoft.EntityFrameworkCore;
using TournamentManager.Application.Common;

namespace TournamentManager.Infrastructure.Common
{
    public static class PaginateExtensions
    {
        public static async Task<Paginate<T>> ToPaginateAsync<T>(
            this IQueryable<T> source,
            int index, int size,
            CancellationToken cancellationToken = default)
             where T : class
        {
            int count = await source.CountAsync(cancellationToken);
            
            ICollection<T> items = await source
                .Skip(index * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            Paginate<T> paginate = new()
            {
                Index = index,
                Size = size,
                Count = count,
                Items = items,
                Pages = (int)Math.Ceiling(count / (double)size)
            };

            return paginate;
        }
    }
}