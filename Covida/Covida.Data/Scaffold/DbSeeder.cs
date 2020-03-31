using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covida.Data.Scaffold
{
    public abstract class DbSeeder<T> where T : DbContext
    {
        public T DbContext { get; }

        protected DbSeeder(T dbContext)
        {
            DbContext = dbContext;
        }

        protected async Task ClearDatabase()
        {
            foreach (var entityType in DbContext.Model.GetEntityTypes())
            {
                var entityName = entityType.Name;
                var method = DbContext.GetType().GetMethod(nameof(DbContext.Set));
                var genericMethod = method.MakeGenericMethod(entityType.ClrType);
                var set = genericMethod?.Invoke(DbContext, null);
                var setType = set.GetType();
                if (set is IEnumerable enumerable)
                {
                    var elements = enumerable.OfType<object>().ToArray();
                    DbContext.RemoveRange(elements);
                }
            }
            await DbContext.SaveChangesAsync();
        }

        public abstract Task Run();
        public abstract Task Commit();
    }
}
