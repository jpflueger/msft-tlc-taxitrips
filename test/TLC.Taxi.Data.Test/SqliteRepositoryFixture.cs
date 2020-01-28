using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Xunit;

namespace TLC.Taxi.Data.Test
{
    public class SqliteRepositoryFixture : IDisposable
    {
        private readonly string _tempDbPath;

        public SqliteRepositoryFixture()
        {
            _tempDbPath = Path.ChangeExtension(Path.GetTempFileName(), ".db3");
        }

        public void Dispose()
        {
            if (File.Exists(_tempDbPath))
            {
                File.Delete(_tempDbPath);
            }
        }

        [Fact]
        public async Task Ctor_Null_Opens_InMemory()
        {
            //Given
            var repo = new SqliteRepository<TestTable, int>(null);
            TestTable expected = TestTable.Generate();

            //When
            TestTable actual = null;
            using (var conn = await repo.OpenAsync(CancellationToken.None))
            {
                await TestTable.CreateTableAsync(conn);
                
                await expected.InsertAsync(conn);

                actual = await conn.QuerySingleOrDefaultAsync<TestTable>(
                    $"SELECT * FROM {nameof(TestTable)} WHERE Id = @id",
                    new { id = expected.Id });

                await TestTable.DropAsync(conn);
            }

            //Then
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Message, actual.Message);
        }
    
        [Fact]
        public async Task Get_By_Id()
        {
            //Given
            var repo = new SqliteRepository<TestTable, int>(_tempDbPath);
            TestTable expected = TestTable.Generate();

            //When
            TestTable actual = null;
            using (var conn = await repo.OpenAsync(CancellationToken.None))
            {
                await TestTable.CreateTableAsync(conn);

                await expected.InsertAsync(conn);

                actual = await repo.GetAsync(expected.Id);

                await TestTable.DropAsync(conn);
            }

            //Then
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Message, actual.Message);
        }

        [Fact]
        public async Task Get_All()
        {
            //Given
            var repo = new SqliteRepository<TestTable, int>(_tempDbPath);
            TestTable expected1 = TestTable.Generate();
            TestTable expected2 = TestTable.Generate();

            //When
            TestTable actual1 = null;
            TestTable actual2 = null;
            using (var conn = await repo.OpenAsync(CancellationToken.None))
            {
                await TestTable.CreateTableAsync(conn);

                await expected1.InsertAsync(conn);
                await expected2.InsertAsync(conn);

                var actual = await repo.GetAllAsync().ToListAsync();
                actual1 = actual.SingleOrDefault(x => x.Id == expected1.Id);
                actual2 = actual.SingleOrDefault(x => x.Id == expected2.Id);

                await TestTable.DropAsync(conn);
            }

            //Then
            Assert.NotNull(actual1);
            Assert.NotNull(actual2);
            Assert.Equal(expected1.Id, actual1.Id);
            Assert.Equal(expected1.Message, actual1.Message);
            Assert.Equal(expected2.Id, actual2.Id);
            Assert.Equal(expected2.Message, actual2.Message);
        }
    }
}