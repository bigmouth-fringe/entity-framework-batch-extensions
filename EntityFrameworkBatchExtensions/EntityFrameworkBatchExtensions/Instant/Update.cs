using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkBatchExtensions.Internal;
using EntityFrameworkBatchExtensions.Internal.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkBatchExtensions.Instant
{
    // TODO: Write Tests
    // TODO: Write Docs
    public static class Update
    {
        // TODO: Add appropriate batch size assertion mechanism
        private const int BatchSize = 1000;
        
        public static void BatchUpdate<T, TK>(
            this DbSet<T> set, List<TK> ids, T obj
        ) where T : class {
            if (obj == null || ids.Count < 1) return;
            
            var ctx = set.GetDbContext();
            var entityType = ctx.Model.FindEntityType(typeof(T));
            var props = entityType.GetProperties().Select(p => p.PropertyInfo);
            
            var idBatches = ids.Batches(BatchSize);
            foreach (var idBatch in idBatches) {
                var sql = SqlQueryBuilder.BuildUpdateQuery(
                    set.GetTableName(), idBatch.ToList(), props.ToList(), obj
                );
                ctx.Database.ExecuteSqlRaw(sql);
            }
        }
        
        public static async Task BatchUpdateAsync<T, TK>(
            this DbSet<T> set, List<TK> ids, T obj
        ) where T : class {
            if (obj == null || ids.Count < 1) return;
            
            var ctx = set.GetDbContext();
            var entityType = ctx.Model.FindEntityType(typeof(T));
            var props = entityType.GetProperties().Select(p => p.PropertyInfo);
            
            var idBatches = ids.Batches(BatchSize);
            foreach (var idBatch in idBatches) {
                var sql = SqlQueryBuilder.BuildUpdateQuery(
                    set.GetTableName(), idBatch.ToList(), props.ToList(), obj
                );
                await ctx.Database.ExecuteSqlRawAsync(sql);
            }
        }
    }
}