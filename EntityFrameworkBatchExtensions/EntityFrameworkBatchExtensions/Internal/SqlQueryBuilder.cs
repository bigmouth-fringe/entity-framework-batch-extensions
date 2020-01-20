using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntityFrameworkBatchExtensions.Internal
{
    // TODO: Write Tests
    // NOTE: This builder is not aware of props correspondence to the actual columns
    internal static class SqlQueryBuilder
    {
        public static string BuildCreateQuery<T>(
            string tableName, List<PropertyInfo> props, List<T> objs
        ) where T : class {
            // TODO: Test
            var propNames = props.Select(p => p.Name);
            var joinedPropNames = string.Join(", ", propNames);
            
            // TODO: Test
            var objWrappedVals = objs.Select(obj => $"({string.Join(", ", props.Select(p => MapPropValue(p, obj)))})");
            var joinedValues = string.Join(", ", objWrappedVals);
            
            return $@"
                INSERT INTO {tableName} ({joinedPropNames})
                VALUES {joinedValues}
                OUTPUT Inserted.Id;
            ".Trim();
        }
        
        public static string BuildUpdateQuery<T, TK>(
            string tableName, List<TK> ids, List<PropertyInfo> props, T obj
        ) where T : class {
            // TODO: Test
            var propSetters = props.Select(p => $"{p.Name} = {MapPropValue(p, obj)}");
            var joinedPropSetters = string.Join(", ", propSetters);
            var joinedIds = string.Join(", ", ids);
            return $@"
                UPDATE {tableName}
                SET {joinedPropSetters}
                OUTPUT Updated.Id
                WHERE Id IN({joinedIds});
            ".Trim();
        }
        
        public static string BuildDeleteQuery<TK>(string tableName, List<TK> ids) 
        {
            // TODO: Test
            var joinedIds = string.Join(", ", ids);
            return $@"
                DELETE FROM {tableName}
                OUTPUT Deleted.Id
                WHERE Id IN({joinedIds});
            ".Trim();
        }

        private static object MapPropValue(PropertyInfo prop, object obj)
        {
            switch (Type.GetTypeCode(prop.PropertyType)) { 
                case TypeCode.String:
                case TypeCode.DateTime:
                    return $"'{prop.GetValue(obj)}'";
                default: return prop.GetValue(obj);
            }
        }
    }
}