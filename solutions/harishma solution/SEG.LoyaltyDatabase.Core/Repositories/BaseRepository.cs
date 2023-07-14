using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SEG.LoyaltyDatabase.Core.Interfaces;
using Dapper;
using DapperExtensions;
using System.Transactions;
using System.Linq.Expressions;
using SEG.LoyaltyDatabase.Core.Extensions;
using SEG.LoyaltyDatabase.Core.Utilities;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class BaseRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly string _tableName;
        private readonly IDbConnection _dbConnection;

        public BaseRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _tableName = EntityUtility.GetTableName<T>();
        }

        public async Task<IEnumerable<T>> GetAsync<T>(string sql, object parameter = null)
        {
            var result = await _dbConnection.QueryIncludeNavigationPropertiesAsync<T>(sql, parameter);
            return result;
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression)
        {
            var sql = expression.ToSqlString<T>();
            var result = await _dbConnection.QueryIncludeNavigationPropertiesAsync<T>(sql);
            return result;
        }

        public IEnumerable<T> GetAll()
        {
            var sql = $"SELECT * FROM { _tableName };";
            var result = _dbConnection.QueryIncludeNavigationProperties<T>(sql);
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM { _tableName };";
            var result = await _dbConnection.QueryIncludeNavigationPropertiesAsync<T>(sql);
            return result;
        }

        public T GetById(object id)
        {
            var sql = $"SELECT * FROM { _tableName } WHERE Id = @Id;";
            var result = _dbConnection.QueryIncludeNavigationProperties<T>(sql, new { Id = id });
            return result.FirstOrDefault();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            var sql = $"SELECT * FROM { _tableName } WHERE Id = @Id;";
            var result = await _dbConnection.QueryIncludeNavigationPropertiesAsync<T>(sql, new { Id = id });
            return result.FirstOrDefault();
        }

        public dynamic ExecuteSql(string rawSql, object sqlParams = null, bool isTransaction = false)
        {
            var sqlVerb = rawSql.Split(' ').FirstOrDefault();
            switch (sqlVerb?.ToUpper())
            {
                case "INSERT":
                case "UPDATE":
                case "DELETE":
                    if (isTransaction)
                    {
                        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                        var affectedRowsTransact = _dbConnection.Execute(rawSql, sqlParams);
                        transactionScope.Complete();
                        return affectedRowsTransact > 0;
                    }

                    var affectedRows = _dbConnection.Execute(rawSql, sqlParams);
                    return affectedRows > 0;

                case "SELECT":
                    if (isTransaction)
                    {
                        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                        var resultTransact = _dbConnection.QueryIncludeNavigationProperties<T>(rawSql, sqlParams);
                        transactionScope.Complete();
                        return resultTransact;
                    }

                    return _dbConnection.Query<T>(rawSql);
                default:
                    throw new Exception("Malformed SQL Statement");
            }
        }

        public async Task<dynamic> ExecuteSqlAsync(string rawSql, object sqlParams = null, bool isTransaction = false)
        {
            var sqlVerb = rawSql.Split(' ').FirstOrDefault();
            switch (sqlVerb?.ToUpper())
            {
                case "INSERT":
                case "UPDATE":
                case "DELETE":
                    if (isTransaction)
                    {
                        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                        var affectedRowsTransact = await _dbConnection.ExecuteAsync(rawSql, sqlParams);
                        transactionScope.Complete();
                        return affectedRowsTransact > 0;
                    }

                    var affectedRows = await _dbConnection.ExecuteAsync(rawSql, sqlParams);
                    return affectedRows > 0;

                case "SELECT":
                    if (isTransaction)
                    {
                        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                        var resultTransact = await _dbConnection.QueryIncludeNavigationPropertiesAsync<T>(rawSql, sqlParams);
                        transactionScope.Complete();
                        return resultTransact;
                    }

                    return await _dbConnection.QueryIncludeNavigationPropertiesAsync<T>(rawSql);
                default:
                    throw new Exception("Malformed SQL Statement");
            }
        }

        public async Task<dynamic> ExecuteStoredProcedureAsync(string sprocName, object sqlParams = null, bool isTransaction = false)
        {
            if (!isTransaction) return await _dbConnection.ExecuteAsync(sprocName, sqlParams, commandType: CommandType.StoredProcedure);

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var results = await _dbConnection.ExecuteAsync(sprocName, sqlParams, commandType: CommandType.StoredProcedure);
            transactionScope.Complete();
            return results;
        }

        public async Task<dynamic> ExecuteStoredProcedureAsync(string sprocName, DynamicParameters sqlParams = null, bool isTransaction = false)
        {
            if (!isTransaction) return await _dbConnection.ExecuteAsync(sprocName, sqlParams, commandType: CommandType.StoredProcedure);

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var results = await _dbConnection.ExecuteAsync(sprocName, sqlParams, commandType: CommandType.StoredProcedure);
            transactionScope.Complete();
            return results;
        }

        public bool Insert(ref T entity, bool includeForeignKeys = false, bool includePrimaryKey = false)
        {
            var sql = SqlUtility.BuildSqlInsertString(entity, includeForeignKeys, includePrimaryKey);
            var affectedRows = _dbConnection.Execute(sql, entity);
            return affectedRows > 0;
        }

        public async Task<bool> InsertAsync(T entity, bool includeForeignKeys = false, bool includePrimaryKey = false)
        {
            var sql = SqlUtility.BuildSqlInsertString(entity, includeForeignKeys, includePrimaryKey);
            var affectedRows = await _dbConnection.ExecuteAsync(sql, entity);
            return affectedRows > 0;
        }

        public bool Update(T entity, bool includeForeignKeys = false)
        {
            var sql = SqlUtility.BuildSqlUpdateString(entity, includeForeignKeys);
            var affectedRows = _dbConnection.Execute(sql, entity);
            return affectedRows > 0;
        }

        public async Task<bool> UpdateAsync(T entity, bool includeForeignKeys = false)
        {
            var sql = SqlUtility.BuildSqlUpdateString(entity, includeForeignKeys);
            var affectedRows = await _dbConnection.ExecuteAsync(sql, entity);
            return affectedRows > 0;
        }

        public bool Delete(object id)
        {
            var sql = $"DELETE FROM { _tableName } WHERE Id = @Id;";
            var affectedRows = _dbConnection.Execute(sql, new { Id = id });
            return affectedRows > 0;
        }

        public bool Delete(T entity)
        {
            var sql = $"DELETE FROM { _tableName } WHERE Id = @Id;";
            var affectedRows = _dbConnection.Execute(sql, entity);
            return affectedRows > 0;
        }
    }
}
