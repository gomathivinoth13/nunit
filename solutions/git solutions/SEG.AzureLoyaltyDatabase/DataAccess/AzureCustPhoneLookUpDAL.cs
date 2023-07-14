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
    /// <summary>
    /// 
    /// </summary>
    public class AzureCustPhoneLookUpDAL : DapperDalBase
    {

        private const string UpsertSQL = @"IF EXISTS (Select * from [CustPhoneLookup] (nolock) where [MobilePhone] = @MobilePhone) 
                                           UPDATE [dbo].[CustPhoneLookup]
                                                SET [TwilioSuccessCheck] = @TwilioSuccessCheck
                                                    ,[PhoneType] = @PhoneType
                                                    ,[LastUpdateDateTime] = @LastUpdateDateTime
                                                    ,[LastUpdateSource] = @LastUpdateSource
                                                WHERE [MobilePhone] = @MobilePhone
                                            
                                            ELSE 
                                            INSERT INTO [dbo].[CustPhoneLookup]
                                                        ([MobilePhone]
                                                        ,[TwilioSuccessCheck]
                                                        ,[LastUpdateDateTime]
                                                        ,[LastUpdateSource]
                                                        ,[PhoneType]
                                                        )
                                                    VALUES
                                                        (@MobilePhone
                                                        ,@TwilioSuccessCheck
                                                        ,@LastUpdateDateTime
                                                        ,@LastUpdateSource
                                                        ,@PhoneType)";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async static Task<CustPhoneLookup> GetCustPhoneLookUp(string phoneNumber, IDbConnection connection, IDbTransaction transaction = null)
        {
            return (await connection.QueryAsync<CustPhoneLookup>("SELECT * from CustPhoneLookup (nolock) where MobilePhone = @MobilePhone", new { MobilePhone = phoneNumber }, transaction).ConfigureAwait(false)).SingleOrDefault();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="custPhoneLookup"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<CustPhoneLookup> SaveCustPhoneLookUp(CustPhoneLookup custPhoneLookup, IDbConnection connection, IDbTransaction transaction = null)
        {
            //new banner, need to be added to existing customer
            if (!string.IsNullOrEmpty(custPhoneLookup.MobilePhone))
            {

                await connection.ExecuteAsync(UpsertSQL, new
                {
                    MobilePhone = custPhoneLookup.MobilePhone,
                    TwilioSuccessCheck = custPhoneLookup.TwilioSuccessCheck,
                    LastUpdateDateTime = custPhoneLookup.LastUpdateDateTime,
                    LastUpdateSource = custPhoneLookup.LastUpdateSource,
                    PhoneType = custPhoneLookup.PhoneType
                }).ConfigureAwait(false);
            }

            return custPhoneLookup;
        }


        internal static async Task<CustPhoneLookup> SaveCustPhoneLookUp(CustPhoneLookup custPhoneLookup)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await SaveCustPhoneLookUp(custPhoneLookup, db);
            }

            return custPhoneLookup;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static async Task<CustPhoneLookup> GetCustPhoneLookup(string phoneNumber)
        {
            CustPhoneLookup custPhoneLookup = null;

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                custPhoneLookup = await GetCustPhoneLookUp(phoneNumber, db).ConfigureAwait(false);
            }
            return custPhoneLookup;
        }
    }
}
