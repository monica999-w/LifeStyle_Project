using LifeStyle.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.IntegrationTests.Helpers
{
    public class DataContextBuilder : IDisposable
    {
        private readonly LifeStyleContext _dataContext;

        public DataContextBuilder(string dbName = "TestDatabase")
        {
            var options = new DbContextOptionsBuilder<LifeStyleContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new LifeStyleContext(options);

            _dataContext = context;
        }

        public LifeStyleContext GetContext()
        {
            _dataContext.Database.EnsureCreated();
            return _dataContext;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dataContext.Dispose();
            }
        }
    }
}
