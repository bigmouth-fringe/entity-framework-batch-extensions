using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkBatchExtensions.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkBatchExtensions.Instant
{
    public static class Create
    {
        // TODO: Add appropriate batch size assertion mechanism
        private const int BatchSize = 1000;
        
        public static void BatchCreate<T>(
            this DbSet<T> set, List<T> objs
        ) where T : class {
            if (objs.Count < 1) return;

            var props = typeof(T).GetProperties();
            var propNames = props.Select(p => p.Name);
            var joinedPropNames = string.Join(", ", propNames);
                
            var objBatches = objs.Batches(BatchSize);
            var ctx = set.GetDbContext();
            foreach (var objBatch in objBatches) {
                // TODO: Move to SQLQueryBuilder (or at least consider)
                // TODO: Figure out how to return DeletedIds after SQL Execution
                var objWrappedVals = objBatch.Select(obj => $"({string.Join(", ", props.Select(p => p.GetValue(obj)))})");
                var joinedValues = string.Join(", ", objWrappedVals);
                var sql = $@"
                    INSERT INTO {set.GetTableName()} ({joinedPropNames})
                    VALUES {joinedValues}
                    OUTPUT Inserted.Id;
                ";
                ctx.Database.ExecuteSqlRaw(sql);
            }
        }
        
        public static async Task BatchCreateAsync<T>(
            this DbSet<T> set, List<T> objs
        ) where T : class {
            if (objs.Count < 1) return;

            var props = typeof(T).GetProperties();
            var propNames = props.Select(p => p.Name);
            var joinedProps = string.Join(", ", propNames);
                
            var batches = objs.Batches(BatchSize);
            var ctx = set.GetDbContext();
            foreach (var batch in batches) {
                // TODO: Move to SQLQueryBuilder (or at least consider)
                // TODO: Figure out how to return DeletedIds after SQL Execution
                var objWrappedVals = batch.Select(obj => $"({string.Join(", ", props.Select(p => p.GetValue(obj)))})");
                var joinedValues = string.Join(", ", objWrappedVals);
                var sql = $@"
                    INSERT INTO {set.GetTableName()} ({joinedProps})
                    VALUES {joinedValues}
                    OUTPUT Inserted.Id;
                ";
                await ctx.Database.ExecuteSqlRawAsync(sql);
            }
        }
    }
}