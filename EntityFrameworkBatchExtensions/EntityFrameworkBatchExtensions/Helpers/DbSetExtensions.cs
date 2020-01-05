using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkBatchExtensions.Helpers
{
    public static class DbSetExtensions
    {
        // SOURCE: https://dev.to/j_sakamoto/how-to-get-the-actual-table-name-from-dbset-in-entityframework-core-20-56k0
        public static DbContext GetDbContext<T>(this DbSet<T> set) where T: class
        {
            var infrastructure = set as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            var dbCtx = serviceProvider.GetService(typeof(ICurrentDbContext))
                as ICurrentDbContext;
            return dbCtx?.Context;
        }
        
        // SOURCE: https://dev.to/j_sakamoto/how-to-get-the-actual-table-name-from-dbset-in-entityframework-core-20-56k0
        public static string GetTableName<T>(this DbSet<T> set) where T: class
        {
            var dbCtx = set.GetDbContext();
            var model = dbCtx.Model;
            var entityTypes = model.GetEntityTypes();
            var entityType = entityTypes.First(t => t.ClrType == typeof(T));
            var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
            var tableName = tableNameAnnotation.Value.ToString();
            return tableName;
        }
    }
}