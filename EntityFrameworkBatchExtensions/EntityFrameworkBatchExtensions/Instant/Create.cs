using System;
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
    public static class Create
    {
        // TODO: Add appropriate batch size assertion mechanism
        private const int BatchSize = 1000;
        
        public static void BatchCreate<T>(
            this DbSet<T> set, List<T> objs
        ) where T : class {
            // TODO: Test
            if (objs.Count < 1) return;
            var objBatches = objs.Batches(BatchSize);
            var ctx = set.GetDbContext();
            foreach (var objBatch in objBatches) {
                var sql = SqlQueryBuilder.BuildCreateQuery(set, objBatch.ToList());
                // TODO: Test
                ctx.Database.ExecuteSqlRaw(sql);
            }
        }
        
        public static async Task BatchCreateAsync<T>(
            this DbSet<T> set, List<T> objs
        ) where T : class {
            // TODO: Test
            if (objs.Count < 1) return;
            var objBatches = objs.Batches(BatchSize);
            var ctx = set.GetDbContext();
            foreach (var objBatch in objBatches) {
                var sql = SqlQueryBuilder.BuildCreateQuery(set, objBatch.ToList());
                // TODO: Test
                await ctx.Database.ExecuteSqlRawAsync(sql);
            }
        }
    }
}