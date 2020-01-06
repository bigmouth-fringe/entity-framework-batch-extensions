namespace EntityFrameworkBatchExtensions
{
    // TODO: Write Tests
    internal static class SqlQueryBuilder
    {
        internal static string BuildCreateQuery(
            string tableName, string joinedPropNames, string joinedValues
        ) {
            return $@"
                INSERT INTO {tableName} ({joinedPropNames})
                VALUES {joinedValues}
                OUTPUT Inserted.Id;
            ";
        }
        
        internal static string BuildUpdateQuery(
            string tableName, string joinedPropSetters, string joinedIds
        ) {
            return $@"
                UPDATE {tableName}
                SET {joinedPropSetters}
                OUTPUT Updated.Id
                WHERE Id IN({joinedIds});
            ";
        }
        
        internal static string BuildDeleteQuery(
            string tableName, string joinedIds
        ) {
            return $@"
                DELETE FROM {tableName}
                OUTPUT Deleted.Id
                WHERE Id IN({joinedIds});
            ";
        }
    }
}