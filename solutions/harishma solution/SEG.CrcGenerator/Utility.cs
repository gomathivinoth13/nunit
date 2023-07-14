////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Utility.cs
//
// summary:	Implements the utility class
////////////////////////////////////////////////////////////////////////////////////////////////////

using SEG;
using Newtonsoft.Json;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Attributes;
using SEG.ApiService.Models.CRC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Transactions;
using SEG.ApiService.Models.Enum;
using System.Text.RegularExpressions;
using SEG.ApiService.Models.Payload;

namespace SEG.CrcGenerator
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An utility. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Utility
    {


        static object lockObject = new object();

        private const Decimal SupervisorCard_WD_Start = 42089999996;
        private const Decimal SupervisorCard_WD_End = 42090000005;
        private const Decimal SupervisorCard_Harveys_Start = 44197777777;
        private const Decimal SupervisorCard_Harveys_End = 44197777787;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Generates a CRC. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="banner">   The banner. </param>
        ///
        /// <returns>   The CRC. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static decimal GenerateCrc(Banner banner)
        {

            try
            {

                using (SEG.LoyaltyDatabase.LoyaltyDatabaseContext dbContext = new SEG.LoyaltyDatabase.LoyaltyDatabaseContext())
                {

                    SqlParameter outParam = new SqlParameter("@GeneratedCrc", System.Data.SqlDbType.Decimal)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };

                    dbContext.Database.ExecuteSqlCommand("EXEC [dbo].[sp_GenerateCRC] @banner, @GeneratedCrc OUTPUT", new SqlParameter("@banner", (int)banner), outParam);

                    return (decimal)outParam.Value;

                }
            }
            catch (Exception e)
            {

                var b = e.ToString();
                var c = b;
                throw;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Generates a member identifier asynchronous. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        //// <returns>   An asynchronous result that yields the generate member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<string> GenerateMemberIDAsync()
        {

            using (SEG.LoyaltyDatabase.LoyaltyDatabaseContext dbContext = new SEG.LoyaltyDatabase.LoyaltyDatabaseContext())
            {
                if (dbContext.Database.Connection.State == System.Data.ConnectionState.Closed)
                    await dbContext.Database.Connection.OpenAsync();

                using (var cmd = dbContext.Database.Connection.CreateCommand())
                {
                    using (var trans = dbContext.Database.Connection.BeginTransaction())
                    {
                        try
                        {
                            SqlParameter outParam = new SqlParameter("@GeneratedMemberID", System.Data.SqlDbType.VarChar, 20)
                            {
                                Direction = System.Data.ParameterDirection.Output
                            };
                            cmd.Transaction = trans;
                            cmd.CommandText = "[dbo].[sp_GenerateMemberID]";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(outParam);
                            await cmd.ExecuteNonQueryAsync();
                            trans.Commit();

                            return (string)outParam.Value;
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GenerateSEGCardNumberAsync()
        {

            using (SEG.LoyaltyDatabase.LoyaltyDatabaseContext dbContext = new SEG.LoyaltyDatabase.LoyaltyDatabaseContext())
            {
                if (dbContext.Database.Connection.State == System.Data.ConnectionState.Closed)
                    await dbContext.Database.Connection.OpenAsync();

                using (var cmd = dbContext.Database.Connection.CreateCommand())
                {
                    using (var trans = dbContext.Database.Connection.BeginTransaction())
                    {
                        try
                        {
                            SqlParameter outParam = new SqlParameter("@GeneratedCardNumber", System.Data.SqlDbType.VarChar, 16)
                            {
                                Direction = System.Data.ParameterDirection.Output
                            };
                            cmd.Transaction = trans;
                            cmd.CommandText = "[dbo].[usp_GenerateCardNumber]";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(outParam);
                            await cmd.ExecuteNonQueryAsync();
                            trans.Commit();

                            return (string)outParam.Value;
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static ValidationResponse ValidateSupervisorCard(decimal cardNumber)
        {
            ValidationResponse response = new ValidationResponse();

            if (cardNumber >= SupervisorCard_WD_Start && cardNumber <= SupervisorCard_WD_End)
            {
                response.IsSupervisorCard = true;
            }

            if (cardNumber >= SupervisorCard_Harveys_Start && cardNumber <= SupervisorCard_Harveys_End)
            {
                response.IsSupervisorCard = true;
            }

            return response;
        }




        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Validates. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="banner">       The banner. </param>
        /// <param name="cardNumber">   The card number. </param>
        /// <param name="bannerObj">    (Optional) The banner object. </param>
        ///
        /// <returns>   A ValidationResponse. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static ValidationResponse Validate(Banner banner, decimal cardNumber, CardRange bannerObj = null)
        {
            CardRange bannerObjHarveys = null;
            var resp = new ValidationResponse();
            //Handle 9800 series CRC's
            if (cardNumber >= 980000000000000)
                cardNumber = cardNumber - 980000000000000;

            if (cardNumber >= 42999990000 && cardNumber <= 42999999999)
            {
                resp.IsValid = false;
                resp.IsRaincheck = true;
                return resp;
            }

            using (new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadUncommitted
                    }))
            {
                using (SEG.LoyaltyDatabase.LoyaltyDatabaseContext dbContext = new SEG.LoyaltyDatabase.LoyaltyDatabaseContext())
                {

                    if (bannerObj == null)
                        if (banner == 0)
                        {
                            List<CardRange> listCardRange = dbContext.LoyaltyCardRanges.ToList();

                            foreach (var i in listCardRange)
                            {
                                var cardRange = i.CRCRange.Where(r => r.End >= cardNumber && r.Start <= cardNumber).FirstOrDefault();
                                if (cardRange != null)
                                {
                                    bannerObj = i;
                                }

                            }
                        }
                        else
                        {
                            if (banner == Banner.WD)
                                bannerObj = dbContext.LoyaltyCardRanges.FirstOrDefault(b => b.Banner == banner);
                            if (banner == Banner.Bilo || banner == Banner.Harveys)
                            {
                                bannerObj = dbContext.LoyaltyCardRanges.FirstOrDefault(b => b.Banner == Banner.Bilo);
                                bannerObjHarveys = dbContext.LoyaltyCardRanges.FirstOrDefault(b => b.Banner == Banner.Harveys);
                            }
                        }


                    resp.CardNumber = cardNumber;

                    if (bannerObj == null)
                        resp.IsValid = false;
                    else
                    {
                        resp.banner = bannerObj.Banner;

                        var bannerRangeObj = bannerObj.CRCRange.Where(r => r.End >= cardNumber && r.Start <= cardNumber).FirstOrDefault();
                        bool isCRC = true;
                        if (bannerRangeObj == null) isCRC = false;


                        var isTaxExempt = bannerObj.TaxExempt.Where(r => r.End >= cardNumber && r.Start <= cardNumber).Any();
                        resp.IsTaxExempt = isTaxExempt;

                        var isSupervisor = bannerObj.SupervisorCardRange.Where(r => r.End >= cardNumber && r.Start <= cardNumber).Any();
                        resp.IsSupervisorCard = isSupervisor;

                        if (bannerObjHarveys != null && !resp.IsSupervisorCard)
                        {
                            var isSupervisorharveys = bannerObjHarveys.SupervisorCardRange.Where(r => r.End >= cardNumber && r.Start <= cardNumber).Any();
                            resp.IsSupervisorCard = isSupervisorharveys;
                        }

                        resp.IsValid = isCRC || isTaxExempt;
                        resp.IsGenerated = bannerRangeObj != null ? bannerRangeObj.GeneratedRange : false;
                    }


                }
                return resp;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Query if 'cardNUmber' is plenti card in segment bin range. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="cardNUmber">   The card n umber. </param>
        ///
        /// <returns>   True if plenti card in segment bin range, false if not. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool IsPlentiCardInSegBinRange(decimal cardNUmber)
        {
            using (SEG.LoyaltyDatabase.LoyaltyDatabaseContext dbContext = new SEG.LoyaltyDatabase.LoyaltyDatabaseContext())
            {
                return dbContext.LoyaltyCardRanges.Any(a => a.PlentiCardRange.Any(b => (b.Start <= cardNUmber && b.End >= cardNUmber)));
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Query if 'customer' does customer get automatic data rights. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="customer"> The customer. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool DoesCustomerGetAutomaticDataRights(Customer customer)
        {
            using (SEG.LoyaltyDatabase.LoyaltyDatabaseContext dbContext = new SEG.LoyaltyDatabase.LoyaltyDatabaseContext())
            {


                if (decimal.TryParse(customer.CrcId, out decimal crcDecimal))
                {
                    return dbContext
                      .LoyaltyCardRanges
                      .Where(w => w.CRCRange.Any(a => ((a.Start <= crcDecimal && a.End >= crcDecimal) && !a.GeneratedRange)))
                      .Any();
                }

                if (customer.Membership != null && customer.Membership.CustAliasRecords != null)
                {
                    bool inSegRange = false;

                    customer.Membership.CustAliasRecords.Where(w => w.AliasType == (int)AliasType.PlentiCardNumber).ToList().ForEach(alias =>
                    {
                        if (!inSegRange)
                        {
                            if (decimal.TryParse(alias.AliasNumber, out decimal aliasNumber))
                            {
                                var inRange = dbContext.LoyaltyCardRanges.Where(w => w.PlentiCardRange.Any(a => ((a.Start <= aliasNumber && a.End >= aliasNumber)))).Any();
                                if (inRange) inSegRange = true;
                            }
                        }
                    });

                    if (inSegRange) return true;
                }

                if (decimal.TryParse(customer.CrcId, out crcDecimal))
                {
                    return dbContext
                      .LoyaltyCardRanges
                      .Where(w => w.CRCRange.Any(a => ((a.Start <= crcDecimal && a.End >= crcDecimal) && !a.GeneratedRange)))
                      .Any();
                }

                return false;

            }
        }
    }
}
