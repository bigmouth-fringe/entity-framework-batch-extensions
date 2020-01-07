using System.Collections.Generic;
using System.Linq;
using EntityFrameworkBatchExtensions.Internal.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkBatchExtensions.Internal
{
    // TODO: Write Tests
    // TODO: Make sure strings wrapped in single quotes
    internal static class SqlQueryBuilder
    {
        internal static string BuildCreateQuery<T>(
            DbSet<T> set, List<T> objs
        ) where T : class {
            // TODO: Test
            var props = typeof(T).GetProperties();
            var propNames = props.Select(p => p.Name);
            var joinedPropNames = string.Join(", ", propNames);
            
            // TODO: Test
            var objWrappedVals = objs.Select(obj => $"({string.Join(", ", props.Select(p => p.GetValue(obj)))})");
            var joinedValues = string.Join(", ", objWrappedVals);
            
            return $@"
                INSERT INTO {set.GetTableName()} ({joinedPropNames})
                VALUES {joinedValues}
                OUTPUT Inserted.Id;
            ";
        }
        
        internal static string BuildUpdateQuery<T, TK>(
            DbSet<T> set, List<TK> ids, T obj
        ) where T : class {
            // TODO: Test
            var props = typeof(T).GetProperties();
            var propSetters = props.Select(p => $"{p.Name} = {p.GetValue(obj)}");
            var joinedPropSetters = string.Join(", ", propSetters);
            var joinedIds = string.Join(", ", ids);
            return $@"
                UPDATE {set.GetTableName()}
                SET {joinedPropSetters}
                OUTPUT Updated.Id
                WHERE Id IN({joinedIds});
            ";
        }
        
        internal static string BuildDeleteQuery<T, TK>(
            DbSet<T> set, List<TK> ids
        ) where T : class {
            // TODO: Test
            var joinedIds = string.Join(", ", ids);
            return $@"
                DELETE FROM {set.GetTableName()}
                OUTPUT Deleted.Id
                WHERE Id IN({joinedIds});
            ";
        }
    }
}