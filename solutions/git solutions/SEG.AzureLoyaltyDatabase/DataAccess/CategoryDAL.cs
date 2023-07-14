using Dapper;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    public class CategoryDAL : DapperDalBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets SEG categories. </summary>
        ///
        /// <remarks>   Mark Robinson 7/28/2020. </remarks>
        ///
        ///
        /// <returns>
        /// An asynchronous result that yields the SEG categories.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task<List<string>> GetCategories()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))

                return await GetCategories(db).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  Gets SEG categories. </summary>
        ///
        /// <remarks>  Mark Robinson 7/28/22020. </remarks>
        ///
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>
        /// An asynchronous result that yields categories.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task<List<string>> GetCategories(IDbConnection connection, IDbTransaction transaction = null)
        {
            var results = await connection.QueryAsync<Category>("Select * from [dbo].[Category]", transaction).ConfigureAwait(false);
            return results.Select(x => x.Name).ToList();
        }
    }
}
