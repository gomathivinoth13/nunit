using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync<T>(string sql, object sqlParams = null);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        Task<dynamic> ExecuteSqlAsync(string rawSql, object sqlParams = null, bool isTransaction = false);
        dynamic ExecuteSql(string rawSql, object sqlParams = null, bool isTransaction = false);
        Task<dynamic> ExecuteStoredProcedureAsync(string sprocName, object sqlParams = null, bool isTransaction = false);
        Task<dynamic> ExecuteStoredProcedureAsync(string sprocName, DynamicParameters sqlParams = null, bool isTransaction = false);
        bool Insert(ref T entity, bool includeForeignKeys = false, bool includePrimaryKey = false);
        Task<bool> InsertAsync(T entity, bool includeForeignKeys = false, bool includePrimaryKey = false);
        bool Update(T entity, bool includeForeignKeys = false);
        Task<bool> UpdateAsync(T entity, bool includeForeignKeys = false);
        bool Delete(object id);
        bool Delete(T entity);
    }
}