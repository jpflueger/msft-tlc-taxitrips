using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TLC.Taxi.Data
{
    public interface IRepository<TEntity, TKey> 
        where TEntity : class, IEntity<TKey>, new()
    {
        Task<TEntity> GetAsync(TKey key, CancellationToken ct);

        IAsyncEnumerable<TEntity> GetAllAsync(CancellationToken ct);

        IAsyncEnumerable<TEntity> QueryAsync(IQuery<TEntity, TKey> query, CancellationToken ct);
    }
}