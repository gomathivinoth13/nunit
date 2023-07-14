using Dapper;
using SEG.LoyaltyDatabase.Core.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SEG.LoyaltyDatabase.Core.Utilities
{
    public static class SqlUtility
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        public static string BuildSqlInsertString<T>(T entity, bool includeForeignKeys = false, bool includePrimaryKey = false)
        {
            var _tableName = EntityUtility.GetTableName<T>();
            var keyName = EntityUtility.GetKeyName(entity);
            if (string.IsNullOrEmpty(keyName)) throw new Exception($"SQL table [{ _tableName }] has no primary key");

            var sbColumnList = new StringBuilder(null);
            var allProperties = TypePropertiesList(typeof(T));
            var idProps = allProperties.Where(p => (includeForeignKeys && p.Name.EndsWith("Id")) || (includePrimaryKey && p.Name.EndsWith("Id")) || (includePrimaryKey && p.Name == "Id") ||
            (includePrimaryKey && p.Name == keyName.ToString())).ToList();
            var nonIdProps = allProperties.Except(idProps).ToList();

            for (var i = 0; i < nonIdProps.Count; i++)
            {
                var property = nonIdProps[i];
                AppendColumnName(sbColumnList, property.Name);
                if (i < nonIdProps.Count - 1) sbColumnList.Append(", ");
            }

            var sbParameterList = new StringBuilder(null);
            for (var i = 0; i < nonIdProps.Count; i++)
            {
                var property = nonIdProps[i];
                sbParameterList.AppendFormat("@{0}", property.Name);
                if (i < nonIdProps.Count - 1) sbParameterList.Append(", ");
            }

            var sql = $"INSERT INTO { _tableName } ({ sbColumnList }) Values ({ sbParameterList });";
            return sql;
        }

        public static string BuildSqlUpdateString<T>(T entity, bool excludeForeignKeys = false)
        {
            var _tableName = EntityUtility.GetTableName<T>();
            var sb = new StringBuilder();
            var keyName = EntityUtility.GetKeyName(entity);
            if (string.IsNullOrEmpty(keyName)) 
            {
                var modelName = entity.GetType().Name;
                throw new Exception($"SQL table [{ _tableName }] has no primary key or model [{ modelName }] does not have a key attribute on a property");
            }

            sb.AppendFormat("UPDATE {0} SET ", _tableName);
            var allProperties = TypePropertiesList(typeof(T));
            var idProps = allProperties.Where(p => (excludeForeignKeys && p.Name.EndsWith("Id")) || p.Name == "Id" ||
            (p.Name == keyName.ToString())).ToList();
            var nonIdProps = allProperties.Except(idProps).ToList();

            for (var i = 0; i < nonIdProps.Count; i++)
            {
                var property = nonIdProps[i];
                AppendColumnNameEqualsValue(sb, property.Name);
                if (i < nonIdProps.Count - 1)
                    sb.Append(", ");
            }

            sb.Append(" WHERE ");
            if (idProps.Count > 0)
            {
                for (var i = 0; i < idProps.Count; i++)
                {
                    var property = idProps[i];
                    AppendColumnNameEqualsValue(sb, property.Name);
                    if (i < idProps.Count - 1)
                        sb.Append(" and ");
                }
            }
            else
            {
                //Primary key does not contain "id"
                AppendColumnNameEqualsValue(sb, keyName);
            }

            var sql = sb.ToString();
            return sql;
        }

        public static string ConvertExpressionToSqlString(Expression expression, string tableName)
        {
            var convertedExpression = new Visitor().Visit(expression);
            return $"SELECT * FROM { tableName } WHERE { convertedExpression.GetExpressionValue() }";
        }

        public static string BuildForeignKeySqlString(string fkTable, string fkColumn, object keyValue)
        {
            var value = ConvertKeyValue(keyValue);
            return $"SELECT * FROM [{ fkTable }] WHERE [{ fkColumn }] = { value }";
        }

        public static string BuildSystemTableSqlString(string table, string fkTable, string key)
        {
            return $@"select c.name from sys.columns c
                            inner join sys.tables t
                            on t.object_id = c.object_id
                            and t.name = '{ fkTable }' and t.type = 'U'
                            where c.name like '%{ table }_{ key }%'";
        }

        private static object ConvertKeyValue(object keyValue)
        {
            if (keyValue.GetType().BaseType == typeof(string) || keyValue.GetType().BaseType == typeof(Guid))
                return ("N'" + keyValue.ToString().Replace("'", "''") + "'");
            if (keyValue.GetType().BaseType == typeof(bool))
                return (bool)keyValue ? "1" : "0";
            return keyValue.GetType().BaseType == typeof(Enum) ? Convert.ToInt32(Enum.Parse(keyValue.GetType(), keyValue.ToString(), true)) : (object)keyValue.ToString();
        }

        private static List<PropertyInfo> TypePropertiesList(Type type)
        {
            if (TypeProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pis))
            {
                return pis.ToList();
            }

            var properties = type.GetProperties().Where(p => IsWriteable(p)
            && !p.GetAccessors().Any(a => a.IsVirtual)).ToArray();
            TypeProperties[type.TypeHandle] = properties;
            return properties.ToList();
        }

        private static bool IsWriteable(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(typeof(WriteAttribute), false).AsList();
            if (attributes.Count != 1) return true;

            var writeAttribute = (WriteAttribute)attributes[0];
            return writeAttribute.Write;
        }

        private static void AppendColumnName(StringBuilder sb, string columnName)
        {
            sb.AppendFormat("[{0}]", columnName); //Add brackets around each column name because some names are reserved words
        }

        private static void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
        {
            sb.AppendFormat("[{0}] = @{1}", columnName, columnName); //Add brackets around each column name because some names are reserved words
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class WriteAttribute : Attribute
    {
        /// <summary>
        /// Specifies whether a field is writable in the database.
        /// </summary>
        /// <param name="write">Whether a field is writable in the database.</param>
        public WriteAttribute(bool write)
        {
            Write = write;
        }

        /// <summary>
        /// Whether a field is writable in the database.
        /// </summary>
        public bool Write { get; }
    }

    /// <summary>
    /// Specifies that this is a computed column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ComputedAttribute : Attribute
    {
    }
}
