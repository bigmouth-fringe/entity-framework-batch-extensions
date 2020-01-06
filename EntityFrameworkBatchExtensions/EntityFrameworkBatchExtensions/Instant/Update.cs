using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkBatchExtensions.Helpers;
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
            
            var props = typeof(T).GetProperties();
            var propSetters = props.Select(p => $"{p.Name} = {p.GetValue(obj)}");
            var joinedPropSetters = string.Join(", ", propSetters);
            
            var idBatches = ids.Batches(BatchSize);
            var ctx = set.GetDbContext();
            foreach (var idBatch in idBatches) {
                var joinedIds = string.Join(", ", idBatch);
                var sql = SqlQueryBuilder.BuildUpdateQuery(set.GetTableName(), joinedPropSetters, joinedIds);
                ctx.Database.ExecuteSqlRaw(sql);
            }
        }
        
        public static async Task BatchUpdateAsync<T, TK>(
            this DbSet<T> set, List<TK> ids, T obj
        ) where T : class {
            if (obj == null || ids.Count < 1) return;
            
            var props = typeof(T).GetProperties();
            var propSetters = props.Select(p => $"{p.Name} = {p.GetValue(obj)}");
            var joinedPropSetters = string.Join(", ", propSetters);
            
            var idBatches = ids.Batches(BatchSize);
            var ctx = set.GetDbContext();
            foreach (var idBatch in idBatches) {
                var joinedIds = string.Join(", ", idBatch);
                var sql = SqlQueryBuilder.BuildUpdateQuery(set.GetTableName(), joinedPropSetters, joinedIds);
                await ctx.Database.ExecuteSqlRawAsync(sql);
            }
        }
    }
}