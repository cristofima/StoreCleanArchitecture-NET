using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Contexts;
using Store.SharedDatabaseSetup;
using System;
using System.Data.Common;

namespace Store.IntegrationTests
{
    public class SharedDatabaseFixture : IDisposable
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        private string dbName = "IntegrationTestsDatabase.db";

        public SharedDatabaseFixture()
        {
            Connection = new SqliteConnection($"Filename={dbName}");

            Seed();

            Connection.Open();
        }

        public DbConnection Connection { get; }

        public StoreContext CreateContext(DbTransaction? transaction = null)
        {
            var context = new StoreContext(new DbContextOptionsBuilder<StoreContext>().UseSqlite(Connection).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        DatabaseSetup.SeedData(context);
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose() => Connection.Dispose();
    }
}