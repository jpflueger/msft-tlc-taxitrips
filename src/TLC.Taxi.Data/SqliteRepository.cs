using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using TLC.Taxi.Data.Models;

namespace TLC.Taxi.Data
{
    public class SqliteRepository<TEntity, TKey> : BaseSqlRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        private readonly string _connStr;

        public SqliteRepository(string dbPath = null)
        {
            // empty data-source creates a temporary database
            var builder = new SqliteConnectionStringBuilder()
            {
                DataSource = dbPath ?? ":memory:",
                ForeignKeys = true,
            };
            _connStr = builder.ToString();
        }

        public override async Task<DbConnection> OpenAsync(CancellationToken ct)
        {
            var conn = new SqliteConnection(_connStr);
            
            await conn.OpenAsync().ConfigureAwait(false);
            
            return conn;
        }
    }
}