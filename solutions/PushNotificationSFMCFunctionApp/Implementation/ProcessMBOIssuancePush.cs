
using Microsoft.Extensions.Logging;
using PushNotificationSFMCFunctionApp.Interface;
using PushNotificationSFMCFunctionApp.Models;
using SalesForceLibrary.Models;
using SalesForceLibrary.SalesForceAPIM;
using SalesForceLibrary.SendJourney;
using SEG.ApiService.Models.SalesForce;
using SEG.EagleEyeLibrary;
using SEG.SalesForce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PushNotificationSFMCFunctionApp.Implementation
{
    public class ProcessMBOIssuancePush : ConfigurationDAL, IProcessMBOIssuancePush
    {
        IEagleEyeMBOIssuanceEventData _eagleEyeMBOIssuanceEventData;
        private EagleEyeService serviceEE;

        public ProcessMBOIssuancePush(IEagleEyeMBOIssuanceEventData data)
        {
            _eagleEyeMBOIssuanceEventData = data;
            serviceEE = setService();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task MBOIssuancePush(EagleEyeMBOIssuanceEventData data, ILogger log)
        {
            try
            {
                //if (dataList != null && dataList.Count > 0)
                {
                    //foreach (EagleEyeMBOIssuanceEventData data in dataList)
                    {
                        if (!string.IsNullOrEmpty(data.AccountID))
                        {
                            if (data.EventName.ToLower().Contains(EventNameType.CREATE.ToString().ToLower()))
                            {
                                data.Created_DT = DateTime.UtcNow;
                                data.Created_Source = "Event Create job";
                                data.Updated_DT = DateTime.UtcNow;
                                data.Updated_Source = "Event Create job";
                                data.CouponEndDate = DateTime.UtcNow;
                                data.EventName = EventNameType.CREATE.ToString();

                                if (string.IsNullOrEmpty(data.Merchant_Parent_ID))
                                    data.Merchant_Parent_ID = "W";
                                if (string.IsNullOrEmpty(data.Merchant_Store_ID))
                                    data.Merchant_Store_ID = "2";
                                if (data.CouponEndDate == null || !data.CouponEndDate.HasValue)
                                    data.CouponEndDate = DateTime.Now;

                                // insert into database 
                                try
                                {
                                    bool result = await _eagleEyeMBOIssuanceEventData.SetEagleEyeMBOIssuanceEventData(data).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    log.LogInformation(string.Format("Exception at create insert event : {0}", ex.Message));
                                }


                                log.LogInformation("data inserted successfully - create event");
                            }
                            else
                            {
                                if (data.EventName.ToLower().Contains(EventNameType.REPORT.ToString().ToLower()))
                                {

                                    // get record  database 
                                    var result = await _eagleEyeMBOIssuanceEventData.GetEagleEyeMBOIssuanceEventData(data.AccountID).ConfigureAwait(false);

                                    if (result != null)
                                    {

                                        if (!string.IsNullOrEmpty(result.Merchant_Parent_ID))
                                            data.Merchant_Parent_ID = result.Merchant_Parent_ID;
                                        if (!string.IsNullOrEmpty(result.Merchant_Store_ID))
                                            data.Merchant_Store_ID = result.Merchant_Store_ID;
                                        if (data.CouponEndDate == null || !data.CouponEndDate.HasValue)
                                            data.CouponEndDate = DateTime.Now;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(data.Merchant_Parent_ID))
                                            data.Merchant_Parent_ID = "W";
                                        if (string.IsNullOrEmpty(data.Merchant_Store_ID))
                                            data.Merchant_Store_ID = "2";
                                        if (data.CouponEndDate == null || !data.CouponEndDate.HasValue)
                                            data.CouponEndDate = DateTime.Now;
                                    }


                                    //get memberID from EE 
                                    var walletInfo = await serviceEE.GetWalletIdentities(data.WalletID.ToString()).ConfigureAwait(false);

                                    if (walletInfo != null && walletInfo.Result != null)
                                    {
                                        if (walletInfo.Result.Results != null && walletInfo.Result.Results.Count > 0)
                                        {
                                            var value = walletInfo.Result.Results.Where(x => x.Type.Trim() == "MEMBER_ID").FirstOrDefault();
                                            if (value != null && !string.IsNullOrEmpty(value.Value))
                                            {
                                                string memberID = value.Value;

                                                MBOIssuanceData dataDE = new MBOIssuanceData();
                                                dataDE.Account_ID = data.AccountID;
                                                dataDE.Campaign_ID = data.CampaignID;
                                                dataDE.ClientType = data.ClientType;
                                                dataDE.Member_ID = memberID;
                                                dataDE.Merchant_Parent_ID = data.Merchant_Parent_ID;
                                                dataDE.Merchant_Store_ID = data.Merchant_Store_ID;
                                                dataDE.Wallet_ID = data.WalletID;

                                                //trigger journey 
                                                await WelcomeJourney(dataDE, data.Merchant_Parent_ID, memberID).ConfigureAwait(false);

                                                // insert into database 

                                                data.Created_DT = DateTime.UtcNow;
                                                data.Created_Source = "Event Report job- Success";
                                                data.Updated_DT = DateTime.UtcNow;
                                                data.Updated_Source = "Event Report job- Success";
                                                data.CouponEndDate = DateTime.UtcNow;
                                                data.EventName = EventNameType.REPORT.ToString();
                                                try
                                                {
                                                    bool success = await _eagleEyeMBOIssuanceEventData.SetEagleEyeMBOIssuanceEventData(data).ConfigureAwait(false);
                                                }
                                                catch (Exception ex)
                                                {
                                                    log.LogInformation(string.Format("Exception at create insert event : {0}", ex.Message));
                                                }

                                                log.LogInformation("Report data inserted scuccessfully .");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task MBOIssuancePushArray(List<EagleEyeMBOIssuanceEventData> dataList, ILogger log)
        {
            try
            {
                if (dataList != null && dataList.Count > 0)
                {
                    foreach (EagleEyeMBOIssuanceEventData data in dataList)
                    {
                        if (!string.IsNullOrEmpty(data.AccountID))
                        {
                            if (data.EventName.ToLower().Contains(EventNameType.CREATE.ToString().ToLower()))
                            {
                                data.Created_DT = DateTime.UtcNow;
                                data.Created_Source = "Event Create job";
                                data.Updated_DT = DateTime.UtcNow;
                                data.Updated_Source = "Event Create job";
                                data.CouponEndDate = DateTime.UtcNow;
                                data.EventName = EventNameType.CREATE.ToString();

                                if (string.IsNullOrEmpty(data.Merchant_Parent_ID))
                                    data.Merchant_Parent_ID = "W";
                                if (string.IsNullOrEmpty(data.Merchant_Store_ID))
                                    data.Merchant_Store_ID = "2";
                                if (data.CouponEndDate == null || !data.CouponEndDate.HasValue)
                                    data.CouponEndDate = DateTime.Now;

                                // insert into database 
                                try
                                {
                                    bool result = await _eagleEyeMBOIssuanceEventData.SetEagleEyeMBOIssuanceEventData(data).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    log.LogInformation(string.Format("Exception at create insert event : {0}", ex.Message));
                                }


                                log.LogInformation("data inserted successfully - create event");
                            }
                            else
                            {
                                if (data.EventName.ToLower().Contains(EventNameType.REPORT.ToString().ToLower()))
                                {
                                    // get record  database 
                                    var result = await _eagleEyeMBOIssuanceEventData.GetEagleEyeMBOIssuanceEventData(data.AccountID).ConfigureAwait(false);

                                    if (result != null)
                                    {

                                        if (!string.IsNullOrEmpty(result.Merchant_Parent_ID))
                                            data.Merchant_Parent_ID = result.Merchant_Parent_ID;
                                        if (!string.IsNullOrEmpty(result.Merchant_Store_ID))
                                            data.Merchant_Store_ID = result.Merchant_Store_ID;
                                        if (data.CouponEndDate == null || !data.CouponEndDate.HasValue)
                                            data.CouponEndDate = DateTime.Now;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(data.Merchant_Parent_ID))
                                            data.Merchant_Parent_ID = "W";
                                        if (string.IsNullOrEmpty(data.Merchant_Store_ID))
                                            data.Merchant_Store_ID = "2";
                                        if (data.CouponEndDate == null || !data.CouponEndDate.HasValue)
                                            data.CouponEndDate = DateTime.Now;
                                    }


                                    //get memberID from EE 
                                    var walletInfo = await serviceEE.GetWalletIdentities(data.WalletID.ToString()).ConfigureAwait(false);

                                    if (walletInfo != null && walletInfo.Result != null)
                                    {
                                        if (walletInfo.Result.Results != null && walletInfo.Result.Results.Count > 0)
                                        {
                                            var value = walletInfo.Result.Results.Where(x => x.Type.Trim() == "MEMBER_ID").FirstOrDefault();
                                            if (value != null && !string.IsNullOrEmpty(value.Value))
                                            {
                                                string memberID = value.Value;

                                                MBOIssuanceData dataDE = new MBOIssuanceData();
                                                dataDE.Account_ID = data.AccountID;
                                                dataDE.Campaign_ID = data.CampaignID;
                                                dataDE.ClientType = data.ClientType;
                                                dataDE.Member_ID = memberID;
                                                dataDE.Merchant_Parent_ID = data.Merchant_Parent_ID;
                                                dataDE.Merchant_Store_ID = data.Merchant_Store_ID;
                                                dataDE.Wallet_ID = data.WalletID;

                                                //trigger journey 
                                                await WelcomeJourney(dataDE, data.Merchant_Parent_ID, memberID).ConfigureAwait(false);

                                                // insert into database 
                                                data.Created_DT = DateTime.UtcNow;
                                                data.Created_Source = "Event Report job- Success";
                                                data.Updated_DT = DateTime.UtcNow;
                                                data.Updated_Source = "Event Report job- Success";
                                                data.CouponEndDate = DateTime.UtcNow;
                                                data.EventName = EventNameType.REPORT.ToString();
                                                try
                                                {
                                                    bool success = await _eagleEyeMBOIssuanceEventData.SetEagleEyeMBOIssuanceEventData(data).ConfigureAwait(false);
                                                }
                                                catch (Exception ex)
                                                {
                                                    log.LogInformation(string.Format("Exception at create insert event : {0}", ex.Message));
                                                }

                                                log.LogInformation("Report data inserted scuccessfully .");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private async Task<WelcomeJourneyResponse> WelcomeJourney(MBOIssuanceData data, string merchant_Parent_ID, string memberID)
        {
            try
            {
                WelcomeJourneyResponse welcomeJourneyResponse = null;


                if (data != null)
                {

                    if (!string.IsNullOrEmpty(memberID))
                    {
                        MBOIssuanceJourneyRequest request = new MBOIssuanceJourneyRequest();
                        if (string.IsNullOrEmpty(merchant_Parent_ID))
                        {
                            merchant_Parent_ID = MerchantParentType.W.ToString();
                        }
                        request.EventDefinitionKey = setKey(merchant_Parent_ID);
                        request.ContactKey = memberID;
                        SalesForceAPIMService salesForceService = setServiceWelcomeJourney(merchant_Parent_ID);

                        request.data = data;

                        var result = await salesForceService.SendJourney(request).ConfigureAwait(false);

                    }
                }
                return welcomeJourneyResponse;
            }
            catch (Exception e)
            {
                throw new Exception("Exception in WelcomeJourney :", e);
            }
        }
    }

}
