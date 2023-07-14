using Microsoft.Extensions.Logging;
using PushNotificationSFMCFunctionApp.Interface;
using PushNotificationSFMCFunctionApp.Models;
using SEG.EagleEyeLibrary.Models;
using SEG.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace PushNotificationSFMCFunctionApp.Implementation
{
    public class CampaignCountProcess : AzureCacheApi
    {



        public CampaignCountProcess(string cacheconnectionString)
          : base(cacheconnectionString)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task issuanceCalculator(List<CampaignIssuanceCountRequest> dataList, ILogger log)
        {
            try
            {
                if (dataList != null && dataList.Count > 0)
                {
                    foreach (CampaignIssuanceCountRequest data in dataList)
                    {
                        ICampaignIssuanceCount _campaignIssuanceCount = new CampaignIssuanceCountDAL();
                        var result = await _campaignIssuanceCount.GetCampaignIssuanceCountData(data.CampaignId).ConfigureAwait(false);
                        DateTime? couponFinalDate = null;

                        if (string.IsNullOrEmpty(data.CampaignStatus))
                        {
                            data.CampaignStatus = "Active";
                        }

                        if (result == null || string.IsNullOrEmpty(result.CampaignID))
                        {

                            //insert into DB
                            CampaignIssuanceCount campaignIssuance = new CampaignIssuanceCount();

                            couponFinalDate = CampaignEndDate(data.CampaignId);
                            if (couponFinalDate != null)
                            {
                                campaignIssuance.CampaignEndDate = couponFinalDate;

                                campaignIssuance.CampaignID = data.CampaignId;
                                campaignIssuance.Status = data.CampaignStatus;

                                campaignIssuance.IssuanceCount = 1;

                                campaignIssuance.Created_DT = DateTime.Now;
                                campaignIssuance.Created_Source = "isuuance calcualtor";

                                campaignIssuance.Updated_DT = DateTime.Now;
                                campaignIssuance.Updated_Source = "issuance calculator";

                                var walletInfo = await _campaignIssuanceCount.InsertCampaignIssuanceCountData(campaignIssuance).ConfigureAwait(false);

                                if (walletInfo)
                                {
                                    if (couponFinalDate.HasValue)
                                    {
                                        //add +1 to DateTime
                                        couponFinalDate = couponFinalDate.Value.AddDays(1);
                                        //convert DateTime to TimeSpan
                                        TimeSpan ttc = couponFinalDate.Value - DateTime.UtcNow;
                                        if (ttc.Ticks > 0)
                                        {
                                            _cacheStore.StringSet(campaignIssuance.CampaignID, campaignIssuance.IssuanceCount, ttc); //pass TimeSpan value here to add

                                            log.LogInformation($"campaign with id: {data.CampaignId} count: {campaignIssuance.IssuanceCount} was successfully added to the cache");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {

                            CampaignIssuanceCount campaignIssuance = new CampaignIssuanceCount();

                            campaignIssuance.CampaignID = data.CampaignId;
                            couponFinalDate = result.CampaignEndDate;

                            campaignIssuance.IssuanceCount = result.IssuanceCount + 1;

                            campaignIssuance.Updated_DT = DateTime.Now;
                            campaignIssuance.Updated_Source = "issuance calculator";


                            //update into DB - increment count 
                            var walletInfo = await _campaignIssuanceCount.UpdateCampaignIssuanceCountData(campaignIssuance).ConfigureAwait(false);
                            if (walletInfo)
                            {
                                if (couponFinalDate.HasValue)
                                {
                                    //add +1 to DateTime
                                    couponFinalDate = couponFinalDate.Value.AddDays(1);
                                    //convert DateTime to TimeSpan
                                    TimeSpan ttc = couponFinalDate.Value - DateTime.UtcNow;
                                    if (ttc.Ticks > 0)
                                    {
                                        _cacheStore.StringSet(campaignIssuance.CampaignID, campaignIssuance.IssuanceCount, ttc); //pass TimeSpan value here to add


                                        log.LogInformation($"campaign with id: {data.CampaignId} count: {campaignIssuance.IssuanceCount} was successfully added to the cache");
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Unable to add issuance count  because of the following exception: {ex.Message}");
                throw;
            }
        }

        private DateTime? CampaignEndDate(string CampaignId)
        {
            //get from cache 
            RedisValue couponData = _cache.StringGet(CampaignId);
            DateTime? couponFinalDate = null;
            if (!couponData.IsNullOrEmpty)
            {

                Coupon coupon = Serializer.JsonDeserialize<Coupon>(couponData);
                if (!string.IsNullOrEmpty(coupon.CouponFixedExpiryDate))
                {
                    if (Convert.ToDateTime(coupon.CouponFixedExpiryDate) >= Convert.ToDateTime(coupon.CouponEndDate))
                        couponFinalDate = Convert.ToDateTime(coupon.CouponFixedExpiryDate);
                    else
                        couponFinalDate = Convert.ToDateTime(coupon.CouponEndDate);
                }
                else
                {
                    couponFinalDate = Convert.ToDateTime(coupon.CouponEndDate);
                }
            }

            return couponFinalDate;
        }
    }
}
