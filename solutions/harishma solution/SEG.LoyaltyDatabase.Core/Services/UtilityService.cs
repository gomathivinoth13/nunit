////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Utility.cs
//
// summary:	Implements the utility class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using SEG.ApiService.Models;
using SEG.ApiService.Models.CRC;
using SEG.ApiService.Models.Enum;
using SEG.ApiService.Models.Payload;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An utility. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class UtilityService : IUtilityService
    {
        private static ICardRangeService _cardRangeService;

        //static object lockObject = new object();

        public UtilityService(ICardRangeService cardRangeService)
        {
            _cardRangeService = cardRangeService;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Generates a CRC. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="banner">   The banner. </param>
        ///
        /// <returns>   The CRC. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //public static async Task<decimal> GenerateCrcAsync(Banner banner)
        //{

        //    try
        //    {
        //        return await _cardRangeService.GetGeneratedCrcAsync(banner);
        //    }
        //    catch (Exception e)
        //    {

        //        var b = e.ToString();
        //        var c = b;
        //        throw;
        //    }
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Generates a member identifier asynchronous. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        //// <returns>   An asynchronous result that yields the generate member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //public static async Task<string> GenerateMemberIDAsync()
        //{
        //                try
        //                {
        //                    return await _cardRangeService.GetGeneratedMemberIdAsync();
        //                }
        //                catch
        //                {
        //                    throw;
        //                }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public static async Task<string> GenerateSEGCardNumberAsync()
        //{
        //    try
        //    {
        //        return await _cardRangeService.GetGeneratedCardNumberAsync();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


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

        public async Task<ValidationResponse> ValidateAsync(Banner banner, decimal cardNumber, CardRange bannerObj = null)
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

            using var scope = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadUncommitted
                    }, TransactionScopeAsyncFlowOption.Enabled);
            if (bannerObj == null)
                if (banner == 0)
                {
                    var cardRanges = await _cardRangeService.GetAllAsync();
                    var listCardRange = cardRanges.ToList();

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
                    {
                        var bannerEnum = await _cardRangeService.GetAsync(b => b.Banner == banner);
                        bannerObj = bannerEnum.FirstOrDefault();
                    }
                    if (banner == Banner.Bilo || banner == Banner.Harveys)
                    {
                        var bannerEnum = await _cardRangeService.GetAsync(b => b.Banner == banner);
                        bannerObj = bannerEnum.FirstOrDefault();
                        var bannerHarveysEnum = await _cardRangeService.GetAsync(b => b.Banner == Banner.Harveys);
                        bannerObjHarveys = bannerHarveysEnum.FirstOrDefault();
                    }
                }


            resp.CardNumber = cardNumber;

            if (bannerObj == null)
                resp.IsValid = false;
            else
            {
                resp.banner = bannerObj.Banner;

                var bannerRangeObj = bannerObj.CRCRange?.Where(r => r.End >= cardNumber && r.Start <= cardNumber).FirstOrDefault();
                bool isCRC = true;
                if (bannerRangeObj == null) isCRC = false;


                var isTaxExempt = bannerObj.TaxExempt != null && bannerObj.TaxExempt.Where(r => r.End >= cardNumber && r.Start <= cardNumber).Any();
                resp.IsTaxExempt = isTaxExempt;

                var isSupervisor = bannerObj.SupervisorCardRange != null && bannerObj.SupervisorCardRange.Where(r => r.End >= cardNumber && r.Start <= cardNumber).Any();
                resp.IsSupervisorCard = isSupervisor;

                if (bannerObjHarveys != null && !resp.IsSupervisorCard)
                {
                    var isSupervisorharveys = bannerObj.SupervisorCardRange != null && bannerObjHarveys.SupervisorCardRange.Where(r => r.End >= cardNumber && r.Start <= cardNumber).Any();
                    resp.IsSupervisorCard = isSupervisorharveys;
                }

                resp.IsValid = isCRC || isTaxExempt;
                resp.IsGenerated = bannerRangeObj != null && bannerRangeObj.GeneratedRange;
            }

            scope.Complete();
            return resp;
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

        public async Task<bool> IsPlentiCardInSegBinRangeAsync(decimal cardNUmber)
        {
            var result = await _cardRangeService.GetAsync(a => a.PlentiCardRange.Any(b => (b.Start <= cardNUmber && b.End >= cardNUmber)));
            return result.Any();
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

        public async Task<bool> DoesCustomerGetAutomaticDataRightsAsync(Customer customer)
        {
            if (decimal.TryParse(customer.CrcId, out decimal crcDecimal))
            {
                var result = await _cardRangeService.GetAsync(w =>
                    w.CRCRange.Any(a => (a.Start <= crcDecimal && a.End >= crcDecimal) && !a.GeneratedRange));
                return result.Any();
            }

            if (customer.Membership != null && customer.Membership.CustAliasRecords != null)
            {
                bool inSegRange = false;

                customer.Membership.CustAliasRecords.Where(w => w.AliasType == (int)AliasType.PlentiCardNumber).ToList().ForEach(async alias =>
                {
                    if (!inSegRange)
                    {
                        if (decimal.TryParse(alias.AliasNumber, out decimal aliasNumber))
                        {
                            var result = await _cardRangeService.GetAsync(w => w.PlentiCardRange.Any(a => a.Start <= aliasNumber && a.End >= aliasNumber));
                            var inRange = result.Any();
                            if (inRange) inSegRange = true;
                        }
                    }
                });

                if (inSegRange) return true;
            }

            if (decimal.TryParse(customer.CrcId, out crcDecimal))
            {
                var result = await _cardRangeService.GetAsync(w => w.CRCRange.Any(a => a.Start <= crcDecimal && a.End >= crcDecimal && !a.GeneratedRange));
                return result.Any();
            }

            return false;
        }
    }
}
