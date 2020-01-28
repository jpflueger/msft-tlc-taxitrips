using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Dapper;
using Xunit;

namespace TLC.Taxi.Data.Test
{
    public class SqliteRepositoryFixture 
    {
        public class TestTable : IEntity<int>
        {
            public int Id { get; set; }

            public string Message { get; set; }
        }

        private readonly Faker _faker = new Faker();

        [Fact]
        public async Task Ctor_Null_Opens_InMemory()
        {
            //Given
            var repo = new SqliteRepository<TestTable, int>(null);
            TestTable expected = Generate();

            //When
            TestTable actual = null;
            using (var conn = await repo.OpenAsync(CancellationToken.None))
            {
                await conn.ExecuteAsync(
                    $@"CREATE TABLE {nameof(TestTable)} (
                            Id INTEGER,
                            Message TEXT,
                            PRIMARY KEY(Id)
                        ) WITHOUT ROWID;
                        INSERT INTO {nameof(TestTable)}(Id, Message) VALUES (@id, @message);",
                        new { id = expected.Id, message = expected.Message }
                );
                
                actual = await conn.QuerySingleOrDefaultAsync<TestTable>(
                    $"SELECT * FROM {nameof(TestTable)} WHERE Id = @id",
                    new { id = expected.Id });
            }

            //Then
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Message, actual.Message);
        }

        private TestTable Generate()
        {
            return new TestTable
            {
                Id = _faker.UniqueIndex,
                Message = _faker.Database.Random.Words(5),
            };
        }
    }
}