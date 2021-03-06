using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TLC.Taxi.Data.Models;

namespace TLC.Taxi.Data
{
    public interface IRepository<TEntity, TKey> 
        where TEntity : class, IEntity<TKey>, new()
    {
        Task<TEntity> GetAsync(TKey key, CancellationToken ct);

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);

        Task<IEnumerable<T>> QueryAsync<T>(IQuery<TEntity, TKey> query, CancellationToken ct);
    }
}