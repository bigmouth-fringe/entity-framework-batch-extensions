using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkBatchExtensions.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkBatchExtensions.Instant
{
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
                // TODO: Move to SQLQueryBuilder (or at least consider)
                // TODO: Figure out how to return DeletedIds after SQL Execution
                var joinedIds = string.Join(", ", idBatch);
                var sql = $@"
                    UPDATE {set.GetTableName()}
                    SET {joinedPropSetters}
                    OUTPUT Updated.Id
                    WHERE Id IS IN({joinedIds});
                ";
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
                // TODO: Move to SQLQueryBuilder (or at least consider)
                // TODO: Figure out how to return DeletedIds after SQL Execution
                var joinedIds = string.Join(", ", idBatch);
                var sql = $@"
                    UPDATE {set.GetTableName()}
                    SET {joinedPropSetters}
                    OUTPUT Updated.Id
                    WHERE Id IS IN({joinedIds});
                ";
                await ctx.Database.ExecuteSqlRawAsync(sql);
            }
        }
    }
}