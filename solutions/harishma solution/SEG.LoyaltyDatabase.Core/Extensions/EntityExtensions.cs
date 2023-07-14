using SEG.LoyaltyDatabase.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Dapper;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LoyaltyDatabase.Core.Extensions
{
    public static class EntityExtensions
    {
        public static async Task<IEnumerable<T>> QueryIncludeNavigationPropertiesAsync<T>(this IDbConnection dbConnection, string sql, object sqlParams = null)
        {
            var entityEnumerable = await dbConnection.QueryAsync<T>(sql, sqlParams);
            var hasNavProps = EntityUtility.TryGetNavigationProperties<T>(out var navProps);
            if (hasNavProps) entityEnumerable = await EntityUtility.PopulateNavigationPropertiesAsync(dbConnection, entityEnumerable, navProps);
            return entityEnumerable;
        }

        public static IEnumerable<T> QueryIncludeNavigationProperties<T>(this IDbConnection dbConnection, string sql, object sqlParams = null)
        {
            var entityEnumerable = dbConnection.Query<T>(sql, sqlParams);
            var hasNavProps = EntityUtility.TryGetNavigationProperties<T>(out var navProps);
            if (hasNavProps) entityEnumerable = EntityUtility.PopulateNavigationProperties(dbConnection, entityEnumerable, navProps);
            return entityEnumerable;
        }
    }
}
