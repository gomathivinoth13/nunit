using SalesForceLibrary.Models;
using SalesForceLibrary.SalesForceAPIM;
using SEG;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Attributes;
using SEG.ApiService.Models.SalesForce;
using SEG.SalesForce;
using SEG.SalesForce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesForceLibrary.SendJourney
{
    /// <summary>
    /// 
    /// </summary>
    public class SalesForceJourney
    {

        SalesForceAPIMService salesForceService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseRestUrlAuth"></param>
        /// <param name="baseRestUrl"></param>
        /// <param name="accountId"></param>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        /// <param name="cacheConnectionString"></param>
        /// <param name="ocpApimSubscriptionKey"></param>
        public SalesForceJourney(string baseRestUrlAuth, string baseRestUrl, string accountId, string clientID, string clientSecret, string cacheConnectionString, string ocpApimSubscriptionKey)
        {
            salesForceService = new SalesForceAPIMService(baseRestUrlAuth, baseRestUrl, clientID, clientSecret, cacheConnectionString, ocpApimSubscriptionKey);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task ProcessSalesForceRequest(CustomerV2 customer, string eventDefinitionKey, string storeID)
        {
            CustomerAddress address = null;
            try
            {
                if (customer != null)
                {

                    Models.Data item = new Models.Data();

                    //MEMBER_ID|
                    if (!string.IsNullOrEmpty(customer.MemberId))
                    {
                        item.MEMBER_ID = customer.MemberId;
                    }
                    else
                    {
                        throw new ApplicationException("Unable to Apply ProcessSalesForceRequest as memberID does not exists");
                    }

                    //FIRST_NAME |
                    if (!string.IsNullOrEmpty(customer.FirstName))
                    {
                        item.FIRST_NAME = customer.FirstName;
                    }

                    //LAST_NAME|
                    if (!string.IsNullOrEmpty(customer.LastName))
                    {
                        item.LAST_NAME = customer.LastName;
                    }

                    if (customer.CustomerAddress != null)
                    {

                        address = customer.CustomerAddress.OrderByDescending(t => t.LastUpdateDate).FirstOrDefault();

                        if (address != null)
                        {
                            //ADDRESS_1
                            if (!string.IsNullOrEmpty(address.AddressLine1))
                            {
                                item.ADDRESS_1 = address.AddressLine1;
                            }

                            //ADDRESS_2|
                            if (!string.IsNullOrEmpty(address.AddressLine2))
                            {
                                item.ADDRESS_2 = address.AddressLine2;
                            }

                            //CITY|
                            if (!string.IsNullOrEmpty(address.City))
                            {
                                item.CITY = address.City;
                            }

                            //STATE|
                            if (!string.IsNullOrEmpty(address.State))
                            {
                                item.STATE = address.State;
                            }

                            //POSTAL_CODE|
                            if (!string.IsNullOrEmpty(address.PostalCode))
                            {
                                item.POSTAL_CODE = address.PostalCode;
                            }
                        }
                    }

                    if (customer.CustomerWallet != null && customer.CustomerWallet.Count > 0)
                    {
                        item.Wallet_ID = customer.CustomerWallet.FirstOrDefault().WalletId;
                    }

                    //EMAIL|
                    if (!string.IsNullOrEmpty(customer.EmailAddress))
                    {
                        item.EMAIL = customer.EmailAddress;
                    }
                    else
                    {
                        item.EMAIL = "";
                    }

                    //MOBILE_PHONE |
                    if (!string.IsNullOrEmpty(customer.MobilePhone))
                    {
                        //sfmc always need 1 before phone number 
                        item.MOBILE_PHONE = string.Format("1{0}", customer.MobilePhone);
                    }

                    if (customer.CustomerCRC != null && customer.CustomerCRC.Count() > 0)
                    {
                        //EME_1 | EME_2 |

                        var crcWD = customer.CustomerCRC.FirstOrDefault(c => c.ChainId.Trim() == Banner.WD.GetAttribute<ChainIdAttribute>().Value);
                        if (crcWD != null && !string.IsNullOrEmpty(crcWD.CrcId))
                        {
                            item.EME_1 = crcWD.CrcId;
                        }

                        var crcBILOHarveys = customer.CustomerCRC.FirstOrDefault(c => c.ChainId.Trim() == Banner.Harveys.GetAttribute<ChainIdAttribute>().Value || c.ChainId.Trim() == Banner.Bilo.GetAttribute<ChainIdAttribute>().Value);
                        if (crcBILOHarveys != null && !string.IsNullOrEmpty(crcBILOHarveys.CrcId))
                        {
                            item.EME_2 = crcBILOHarveys.CrcId;
                        }
                    }

                    //ENROLLMENT_STATUS|
                    if (!string.IsNullOrEmpty(customer.EnrollmentStatus))
                        item.ENROLLMENT_STATUS = customer.EnrollmentStatus;


                    ////GENDER_CODE |
                    //if (!string.IsNullOrEmpty(customer.GenderCode))
                    //{
                    //    item.GENDER_CODE = customer.GenderCode;
                    //}

                    //BIRTH_DATE|
                    if (customer.BirthDate != null && customer.BirthDate.HasValue)
                    {
                        item.BIRTH_DATE = customer.BirthDate.Value.ToString();
                    }


                    if (!string.IsNullOrEmpty(customer.EnrollmentBanner))
                    {
                        item.Enrollment_Banner = customer.EnrollmentBanner;
                    }

                    if (!string.IsNullOrEmpty(storeID))
                    {
                        item.Store_ID = storeID;
                    }



                    if (customer.EnrollmentDate != null && customer.EnrollmentDate.HasValue)
                    {
                        item.Enrollment_Date = customer.EnrollmentDate.ToString();
                    }


                    //Insert_Date
                    if (customer.CreatedDate != null && customer.CreatedDate.HasValue)
                        item.Insert_Date = customer.CreatedDate.ToString();

                    //Last_Modified_Date
                    if (customer.LastUpdateDate != null && customer.LastUpdateDate.HasValue)
                        item.Last_Modified_Date = customer.LastUpdateDate.ToString();


                    POSWelcomeJourneyRequest welcomeJourneyRequest = new POSWelcomeJourneyRequest();
                    welcomeJourneyRequest.data = item;
                    welcomeJourneyRequest.ContactKey = item.MEMBER_ID;
                    welcomeJourneyRequest.EventDefinitionKey = eventDefinitionKey;

                    var result = await salesForceService.PosWelcomeJourney(welcomeJourneyRequest).ConfigureAwait(false);

                }
                else
                {
                    throw new ApplicationException("Unable to Apply ProcessSalesForceRequest as Customer info does not exists");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
