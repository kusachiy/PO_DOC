using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepository.Factories
{
    public class SwitchableDbContextFactory : IDbContextFactory
    {
        private readonly string _connectionString;

        public SwitchableDbContextFactory(string connString)
        {
            _connectionString = connString;
        }

        public TDbContext CreateDbContext<TDbContext>() where TDbContext : DbContext
        {
            if (typeof(TDbContext) == typeof(DbContext))
                return new Context(_connectionString) as TDbContext;
            return Activator.CreateInstance<TDbContext>();
        }
    }
}
