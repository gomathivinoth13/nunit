using SEG.LoyaltyDatabase.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SEG.LoyaltyDatabase.Core.Extensions
{
    public static class SqlExtensions
    {
        public static string ToSqlString<T>(this Expression expression)
        {
            return ToSqlString(expression, typeof(T));
        }

        public static string ToSqlString(this Expression expression, Type type)
        {
            var tableName = EntityUtility.GetTableName(type);
            var sql = SqlUtility.ConvertExpressionToSqlString(expression, tableName);
            return sql;
        }
    }
}
