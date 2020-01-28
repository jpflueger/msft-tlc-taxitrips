using System.Data.Common;
using System.Threading.Tasks;
using Bogus;
using Dapper;
using TLC.Taxi.Data.Models;

namespace TLC.Taxi.Data.Test
{
    public class TestTable : IEntity<int>
    {
        private static readonly Faker _faker = new Faker();

        public static TestTable Generate()
        {
            return new TestTable
            {
                Id = _faker.Random.Int(1, 1000),
                Message = _faker.Database.Random.Words(5),
            };
        }

        public int Id { get; set; }

        public string Message { get; set; }

        public static async Task CreateTableAsync(DbConnection conn)
        {
            using (var trans = conn.BeginTransaction())
            {
                await conn.ExecuteAsync(
                    $@"CREATE TABLE {nameof(TestTable)} (
                            Id INTEGER,
                            Message TEXT,
                            PRIMARY KEY(Id)
                        ) WITHOUT ROWID", trans);

                await trans.CommitAsync();
            }
        }

        public static async Task DropAsync(DbConnection conn)
        {
            using (var trans = conn.BeginTransaction())
            {
                await conn.ExecuteAsync(
                    $@"DROP TABLE {nameof(TestTable)}", trans);

                await trans.CommitAsync();
            }
        }

        public async Task InsertAsync(DbConnection connection)
        {
            using (var trans = await connection.BeginTransactionAsync().ConfigureAwait(false))
            {
                await connection.ExecuteAsync(
                    $@"INSERT INTO {nameof(TestTable)}(Id, Message) VALUES (@id, @message);",
                    new { id = Id, message = Message }).ConfigureAwait(false);

                await trans.CommitAsync().ConfigureAwait(false);
            }
        }
    }
}