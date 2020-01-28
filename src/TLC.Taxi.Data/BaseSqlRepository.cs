using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using TLC.Taxi.Data.Models;

namespace TLC.Taxi.Data
{
    public abstract class BaseSqlRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        private readonly string _tableName;

        public BaseSqlRepository()
        {
            // simple naming convention, could use an attribute here
            _tableName = typeof(TEntity).Name;
        }

        public async Task<TEntity> GetAsync(TKey key, CancellationToken ct = default)
        {
            using (DbConnection conn = await OpenAsync(ct).ConfigureAwait(false))
            {
                string sql = $"SELECT * FROM {_tableName} WHERE Id = @id";
                return await conn.QuerySingleOrDefaultAsync<TEntity>(sql, new { id = key }).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
        {
            using (DbConnection conn = await OpenAsync(ct))
            {
                string sql = $"SELECT * FROM {_tableName}";
                return await conn.QueryAsync<TEntity>(sql, ct).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(IQuery<TEntity, TKey> query, CancellationToken ct = default)
        {
            using (DbConnection conn = await OpenAsync(ct).ConfigureAwait(false))
            {
                (string sql, object pars) = query.ToSql();
                return await conn.QueryAsync<T>(sql, pars).ConfigureAwait(false);
            }
        }

        public abstract Task<DbConnection> OpenAsync(CancellationToken ct);
    }
}