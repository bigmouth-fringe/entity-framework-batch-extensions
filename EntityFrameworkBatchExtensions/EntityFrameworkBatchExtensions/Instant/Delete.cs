using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFrameworkBatchExtensions.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkBatchExtensions.Instant
{
    // TODO: Write Tests
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
                // TODO: Move to SQLQueryBuilder (or at least consider)
                // TODO: Figure out how to return DeletedIds after SQL Execution
                var joinedIds = string.Join(", ", idBatch);
                var sql = $@"
                    DELETE FROM {set.GetTableName()}
                    OUTPUT Deleted.Id
                    WHERE Id IN({joinedIds});
                ";
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
                // TODO: Move to SQLQueryBuilder (or at least consider)
                // TODO: Figure out how to return DeletedIds after SQL Execution
                var joinedIds = string.Join(", ", idBatch);
                var sql = $@"
                    DELETE FROM {set.GetTableName()}
                    OUTPUT Deleted.Id
                    WHERE Id IN({joinedIds});
                ";
                await ctx.Database.ExecuteSqlRawAsync(sql);
            }
        }
    }
}