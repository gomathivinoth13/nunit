using Dapper;
using SEG.LoyaltyDatabase.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LoyaltyDatabase.Core.Utilities
{
    public static class EntityUtility
    {
        public static string GetTableName(Type type)
        {
            return type.GetCustomAttribute<TableAttribute>().Name;
        }

        public static string GetTableName<T>()
        {
            return GetTableName(typeof(T));
        }

        public static string GetKeyName<T1>(T1 entity)
        {
            string keyName = "";
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (Attribute.GetCustomAttribute(property, typeof(KeyAttribute)) is KeyAttribute attribute)
                {
                    keyName = property.Name;
                    break;
                }
            }
            return keyName;
        }

        private static (string Name, object Value) GetTableKey<T>(T item)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                if (Attribute.GetCustomAttribute(property, typeof(KeyAttribute)) is KeyAttribute)
                    return (property.Name, property.GetValue(item));
            }
            return ("", null);
        }

        public static async Task<IEnumerable<T>> PopulateNavigationPropertiesAsync<T>(IDbConnection dbConnection, IEnumerable<T> results, List<PropertyInfo> navProps)
        {
            var tableName = GetTableName<T>();
            foreach (var item in results)
            {
                (string, object) key = GetTableKey(item);
                for (int n = 0; n <= navProps.Count() - 1; ++n)
                {
                    var prop = navProps[n];
                    var methodInfo = prop.GetAccessors().Where(m => m.ReflectedType != typeof(void)).FirstOrDefault();
                    var propertyReturnType = methodInfo.ReturnType.GetGenericArguments().Single();
                    var fkTable = propertyReturnType.Name;
                    var fkColumns = await GetForeignKeyColumnsAsync<T>(dbConnection, tableName, fkTable);
                    var navSql = SqlUtility.BuildForeignKeySqlString(fkTable, fkColumns[n], key.Item2);
                    var navResult = await dbConnection.QueryAsync(propertyReturnType, navSql);
                    var listType = typeof(List<>);
                    var constructedListType = listType.MakeGenericType(propertyReturnType);
                    var instanceList = (ICollection)Activator.CreateInstance(constructedListType);
                    var props = propertyReturnType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    foreach (var nav in navResult)
                    {
                        var instance = Activator.CreateInstance(propertyReturnType);
                        for (var index = 0; index < props.Length; ++index)
                        {
                            var p = props[index];
                            instance.GetType().GetProperty(p.Name).SetValue(instance, p.GetValue(nav));
                        }

                        MethodInfo method = instanceList.GetType().GetMethod("Add");
                        method.Invoke(instanceList, new object[] { instance });
                    }

                    prop.SetValue(item, instanceList);
                }
            }
            return results;
        }

        public static IEnumerable<T> PopulateNavigationProperties<T>(IDbConnection dbConnection, IEnumerable<T> results, List<PropertyInfo> navProps)
        {
            var tableName = GetTableName<T>();
            foreach (var item in results)
            {
                (string, object) key = GetTableKey(item);
                for (int n = 0; n <= navProps.Count() - 1; ++n)
                {
                    var prop = navProps[n];
                    var methodInfo = prop.GetAccessors().Where(m => m.ReflectedType != typeof(void)).FirstOrDefault();
                    var propertyReturnType = methodInfo.ReturnType.GetGenericArguments().Single();
                    var fkTable = propertyReturnType.Name;
                    var fkColumns = GetForeignKeyColumns<T>(dbConnection, tableName, fkTable);
                    var navSql = SqlUtility.BuildForeignKeySqlString(fkTable, fkColumns[n], key.Item2);
                    var navResult = dbConnection.Query(propertyReturnType, navSql);
                    var listType = typeof(List<>);
                    var constructedListType = listType.MakeGenericType(propertyReturnType);
                    var instanceList = (ICollection)Activator.CreateInstance(constructedListType);
                    var props = propertyReturnType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    foreach (var nav in navResult)
                    {
                        var instance = Activator.CreateInstance(propertyReturnType);
                        for (var index = 0; index < props.Length; ++index)
                        {
                            var p = props[index];
                            instance.GetType().GetProperty(p.Name).SetValue(instance, p.GetValue(nav));
                        }

                        MethodInfo method = instanceList.GetType().GetMethod("Add");
                        method.Invoke(instanceList, new object[] { instance });
                    }

                    prop.SetValue(item, instanceList);
                }
            }
            return results;
        }

        private static async Task<List<string>> GetForeignKeyColumnsAsync<T>(IDbConnection dbConnection, string table, string fkTable)
        {
            var key = GetKeyName(typeof(T));
            string sql = SqlUtility.BuildSystemTableSqlString(table, fkTable, key);
            var results = await dbConnection.QueryAsync<string>(sql);
            return results.ToList();
        }

        private static List<string> GetForeignKeyColumns<T>(IDbConnection dbConnection, string table, string fkTable)
        {
            var key = GetKeyName(typeof(T));
            string sql = SqlUtility.BuildSystemTableSqlString(table, fkTable, key);
            var results = dbConnection.Query<string>(sql);
            return results.ToList();
        }

        public static bool TryGetNavigationProperties<T>(out List<PropertyInfo> navProps)
        {
            navProps = typeof(T).GetProperties().Where(x => x.GetAccessors()
            .Any(a => a.IsVirtual)).OrderBy(x => x.Name).ToList();
            return navProps.Any();
        }
    }
}
