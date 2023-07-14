////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	LoyaltyDBExtensions.cs
//
// summary:	Implements the loyalty database extensions class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace SEG.LoyaltyDatabase.Core
{
   ////////////////////////////////////////////////////////////////////////////////////////////////////
   /// <summary>    A loyalty database extensions. </summary>
   ///
   /// <remarks>    Mcdand, 2/19/2018. </remarks>
   ////////////////////////////////////////////////////////////////////////////////////////////////////

   public static  class LoyaltyDBExtensions
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// An IQueryable&lt;T&gt; extension method that converts a query to a list read uncommitted.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="query">    The query to act on. </param>
        ///
        /// <returns>   Query as a List&lt;T&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static List<T> ToListReadUncommitted<T>(this IQueryable<T> query)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }))
            {
                List<T> toReturn = query.ToList();
                scope.Complete();
                return toReturn;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   An IQueryable&lt;T&gt; extension method that count read uncommitted. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="query">    The query to act on. </param>
        ///
        /// <returns>   The total number of read uncommitted. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int CountReadUncommitted<T>(this IQueryable<T> query)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }))
            {
                int toReturn = query.Count();
                scope.Complete();
                return toReturn;
            }
        }
    }
}
