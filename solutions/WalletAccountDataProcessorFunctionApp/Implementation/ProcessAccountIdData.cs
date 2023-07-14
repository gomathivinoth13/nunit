using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletAccountDataProcessorFunctionApp.Interface;
using WalletAccountDataProcessorFunctionApp.Models;

namespace WalletAccountDataProcessorFunctionApp.Implementation
{

    public class ProcessAccountIdData : IProcessAccountIdData
    {
        IWalletAccountIDEventDataDAL walletAccountIDEventData;
        public ProcessAccountIdData(IWalletAccountIDEventDataDAL walletdata)
        {
            walletAccountIDEventData = walletdata;
        }
        public async Task WalletAccountIDEventPush(List<WalletAccountIDEventData> dataList, ILogger log)
        {
            try
            {
                if (dataList != null && dataList.Count > 0)
                {
                    foreach (WalletAccountIDEventData data in dataList)
                    {
                        if (!string.IsNullOrEmpty(data.AccountID))
                        {
                            data.Created_DT = DateTime.UtcNow;
                            data.Created_Source = "received event";
                            data.Updated_DT = DateTime.UtcNow;
                            // insert into database 
                            try
                            {
                                bool result = await walletAccountIDEventData.SetWalletAccountIdEventData(data).ConfigureAwait(false);
                                if (result)
                                {
                                    log.LogInformation("data inserted successfully");
                                }
                                else
                                {
                                    log.LogWarning("Error while inserting data");
                                }


                            }
                            catch (Exception ex)
                            {
                                log.LogError(string.Format("Exception at create insert event : {0}", ex.Message + ex.StackTrace + ex.InnerException));
                               
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.LogError(ex.Message + ex.StackTrace + ex.InnerException);
                
            }

        }

        public async Task UpdateWalletIdAccountEvent(WalletAccountIDEventData data, ILogger log)
        {
            try
            {
                if (!string.IsNullOrEmpty(data.AccountID))
                {
                    data.Created_Source = "processed event";
                    data.Updated_DT = DateTime.UtcNow;
                    try
                    {
                        bool result = await walletAccountIDEventData.UpdateWalletAccountIdEventData(data).ConfigureAwait(false);
                        if (result)
                        {
                            log.LogInformation("data inserted successfully");
                        }
                        else
                        {
                            log.LogWarning("Error while inserting data");
                        }


                    }
                    catch (Exception ex)
                    {
                        log.LogError(string.Format("Exception at create insert event : {0}", ex.Message + ex.StackTrace + ex.InnerException));

                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message + ex.StackTrace + ex.InnerException);
            }


        }
        

    }
}
