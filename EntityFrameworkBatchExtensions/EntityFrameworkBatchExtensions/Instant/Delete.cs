using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFrameworkBatchExtensions.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkBatchExtensions.Instant
{
    // TODO: Write Tests
    // TODO: Write Docs
    public static class Delete
    {
        // TODO: Add appropriate batch size assertion mechanism
        private const int BatchSize = 1000;
        
        public static void BatchDelete<T, TK>(
            this DbSet<T> set, List<TK> ids
        ) where T : class {
            if (ids.Count < 1) return;

            var idBatches = ids.Batches(BatchSize);
            var ctx = set.GetDbContext();
            foreach (var idBatch in idBatches) {
                var joinedIds = string.Join(", ", idBatch);
                var sql = SqlQueryBuilder.BuildDeleteQuery(set.GetTableName(), joinedIds);
                ctx.Database.ExecuteSqlRaw(sql);
            }
        }
        
        public static async Task BatchDeleteAsync<T, TK>(
            this DbSet<T> set, List<TK> ids
        ) where T : class {
            if (ids.Count < 1) return;

            var idBatches = ids.Batches(BatchSize);
            var ctx = set.GetDbContext();
            foreach (var idBatch in idBatches) {
                var joinedIds = string.Join(", ", idBatch);
                var sql = SqlQueryBuilder.BuildDeleteQuery(set.GetTableName(), joinedIds);
                await ctx.Database.ExecuteSqlRawAsync(sql);
            }
        }
    }
}