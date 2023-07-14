using log4net;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SalesForceLibrary.Models;
using SEG;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Attributes;
using SEG.ApiService.Models.Enum;
using SEG.ApiService.Models.SalesForce;
using SEG.SalesForce;
using SEG.SalesForce.Controllers;
using SEG.SalesForce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SalesForceLibrary.Queue
{
    /// <summary>
    /// 
    /// </summary>
    public class SalesForceQueueProcess
    {
        SalesForceService salesForceService;
        ManageAccessToken manageAccessToken;
        SalesForceRestDAL serviceDAL;

        #region Static Variables
        /// <summary>   The logging. </summary>
        private ILog Logging = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Static Variables  

        const string WinnDixieCouponAliasPrefix = "9800";
        const string OptIn = "Opt-In";
        const string OptOut = "Opt-Out";

        /// <summary>
        /// 
        /// </summary>
        public string WebApiEndPoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OcpApimSubscriptionKeySecret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webApiEndPoint"></param>
        /// <param name="ocpApimSubscriptionKey"></param>
        public SalesForceQueueProcess(string webApiEndPoint, string ocpApimSubscriptionKey)
        {
            WebApiEndPoint = webApiEndPoint;
            OcpApimSubscriptionKeySecret = ocpApimSubscriptionKey;
        }

       
        /// <summary>
        /// 
        /// </summary>
        public async Task ProcessSalesForceRequest(SalesForceQueueRequest salesForceQueueRequest)
        {
            DataExtentionsRequestInsert dataExtentionsRequestInsert = new DataExtentionsRequestInsert();
            DataExtentionsRequest dataExtentionsRequest = new DataExtentionsRequest();
            DataExtentionsBabyClubChildRequest dataExtentionsBabyClubChildRequest = new DataExtentionsBabyClubChildRequest();

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            Boolean Opt_Out = false;

            CustomerAddress address = null;


            try
            {
                if (salesForceQueueRequest.customer != null)
                {
                    List<Item> items = new List<Item>();

                    Item item = new Item();

                    //MEMBER_ID|
                    if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.MemberId))
                    {
                        item.MEMBER_ID = salesForceQueueRequest.customer.MemberId;
                    }
                    else
                    {
                        throw new ApplicationException("Unable to Apply ProcessSalesForceRequest as memberID does not exists");
                    }

                    //FIRST_NAME |
                    if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.FirstName))
                    {
                        item.FIRST_NAME = salesForceQueueRequest.customer.FirstName;
                    }

                    //LAST_NAME|
                    if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.LastName))
                    {
                        item.LAST_NAME = salesForceQueueRequest.customer.LastName;
                    }

                    if (salesForceQueueRequest.customer.CustomerAddress != null)
                    {

                        address = salesForceQueueRequest.customer.CustomerAddress.OrderByDescending(t => t.LastUpdateDate).FirstOrDefault();

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

                    //OPT_OUT and OPT_IN 
                    if (salesForceQueueRequest.customer.EmailOptOutStatus)
                    {
                        //CustomerAttribute attribute = salesForceQueueRequest.customer.CustomerAttributes.FirstOrDefault(a => a.AttributeId == AttributeType.EmailOptOutStatus);
                        //if (attribute != null)
                        //{
                        //    if (attribute.AttributeCode.Trim() == OptOut)
                        //    {
                        Opt_Out = true;
                        //    }
                        //}
                    }
                    queryParams.Add("opt_Out", Opt_Out);

                    if (salesForceQueueRequest.customer.CustomerWallet != null && salesForceQueueRequest.customer.CustomerWallet.Count > 0)
                    {
                        item.Wallet_ID = salesForceQueueRequest.customer.CustomerWallet.FirstOrDefault().WalletId;
                    }

                    //**COUNTRY_CODE|

                    //EMAIL|
                    if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.EmailAddress))
                    {
                        item.EMAIL = salesForceQueueRequest.customer.EmailAddress;
                    }

                    //MOBILE_PHONE |
                    if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.MobilePhone))
                    {
                        item.MOBILE_PHONE = salesForceQueueRequest.customer.MobilePhone;
                    }

                    if (salesForceQueueRequest.customer.CustomerCRC != null && salesForceQueueRequest.customer.CustomerCRC.Count() > 0)
                    {
                        //EME_1 | EME_2 |

                        var crcWD = salesForceQueueRequest.customer.CustomerCRC.FirstOrDefault(c => c.ChainId.Trim() == Banner.WD.GetAttribute<ChainIdAttribute>().Value);
                        if (crcWD != null && !string.IsNullOrEmpty(crcWD.CrcId))
                        {
                            item.EME_1 = crcWD.CrcId;

                            ////Baby_Club_Flag
                            //if (crcWD.BabyClub)
                            //{
                            //    item.Baby_Club_Flag = "Y";
                            //}

                        }

                        var crcBILOHarveys = salesForceQueueRequest.customer.CustomerCRC.FirstOrDefault(c => c.ChainId.Trim() == Banner.Harveys.GetAttribute<ChainIdAttribute>().Value || c.ChainId.Trim() == Banner.Bilo.GetAttribute<ChainIdAttribute>().Value);
                        if (crcBILOHarveys != null && !string.IsNullOrEmpty(crcBILOHarveys.CrcId))
                        {
                            item.EME_2 = crcBILOHarveys.CrcId;

                            ////Baby_Club_Flag
                            //if (crcBILOHarveys.BabyClub)
                            //{
                            //    item.Baby_Club_Flag = "Y";
                            //}
                        }


                        //if (salesForceQueueRequest.customer.ChainId.Trim() == Banner.WD.GetAttribute<ChainIdAttribute>().Value)
                        //{

                        //    if (!String.IsNullOrEmpty(salesForceQueueRequest.customer.CrcId))
                        //    {
                        //        //if (!salesForceQueueRequest.customer.CrcId.StartsWith(WinnDixieCouponAliasPrefix))
                        //        //{
                        //        //    //for winndixie we need to preappend the 9800 to the alias number
                        //        //    //string crcId = string.Format("{0}{1}", WinnDixieCouponAliasPrefix, salesForceQueueRequest.customer.CrcId);
                        //        //    item.EME_1 = crcId;
                        //        //}
                        //        //else
                        //        //{
                        //        item.EME_1 = salesForceQueueRequest.customer.CrcId;
                        //        //}
                        //    }
                        //}
                        //else
                        //{
                        //    item.EME_2 = salesForceQueueRequest.customer.CrcId;
                        //}
                    }

                    //PRIMARY_BANNER |
                    // item.PRIMARY_BANNER = salesForceQueueRequest.customer.ChainId;

                    //***PRIMARY_STORE|

                    //if (salesForceQueueRequest.customer.Membership != null)
                    //{
                    //ENROLLMENT_STATUS|
                    if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.EnrollmentStatus))
                        item.ENROLLMENT_STATUS = salesForceQueueRequest.customer.EnrollmentStatus;
                    //}

                    //GENDER_CODE |
                    if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.GenderCode))
                    {
                        item.GENDER_CODE = salesForceQueueRequest.customer.GenderCode;
                    }

                    //**ASSOCIATE_INDICATOR|
                    //**SEGMENT|
                    //**SUB_SEGMENT|

                    //BIRTH_DATE|
                    if (salesForceQueueRequest.customer.BirthDate != null && salesForceQueueRequest.customer.BirthDate.HasValue)
                    {
                        item.BIRTH_DATE = salesForceQueueRequest.customer.BirthDate.Value.ToString();
                    }


                    ////***EMP_STATUS|




                    //if (salesForceQueueRequest.customer.CustomerAttributes != null && salesForceQueueRequest.customer.CustomerAttributes.Count() > 0)
                    //{

                    ////Enrollment_Banner
                    //if (salesForceQueueRequest.customer.CustomerAttributes.Any(a => a.AttributeId == AttributeType.EnrollmentBanner))
                    //{
                    //    var banner = salesForceQueueRequest.customer.CustomerAttributes.OrderBy(a => a.LastUpdateDate).FirstOrDefault(a => a.AttributeId == AttributeType.EnrollmentBanner);

                    if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.EnrollmentBanner))
                    {
                        item.Enrollment_Banner = salesForceQueueRequest.customer.EnrollmentBanner;
                    }

                    //}

                    ////Enrollment_Date
                    ///
                    if (salesForceQueueRequest.customer.EnrollmentDate != null && salesForceQueueRequest.customer.EnrollmentDate.HasValue)
                    {
                        item.Enrollment_Date = salesForceQueueRequest.customer.EnrollmentDate.ToString();
                    }
                    //if (salesForceQueueRequest.customer.CustomerAttributes.Any(a => a.AttributeId == AttributeType.EnrollmentDate))
                    //{
                    //    var date = salesForceQueueRequest.customer.CustomerAttributes.OrderBy(a => a.LastUpdateDate).FirstOrDefault(a => a.AttributeId == AttributeType.EnrollmentDate);
                    //    item.Enrollment_Date = date.AttributeCode;

                    //}

                    //setPin
                    if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.AccountPin))
                    {
                        item.PIN_Indicator = "Y";
                    }

                    //setPassword
                    if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.AccountPassword))
                    {
                        item.Password_Indicator = "Y";
                    }

                    //}

                    //self slected store WD 
                    if (salesForceQueueRequest.customer.SelfSelectedStoreWD.HasValue)
                    {
                        if (salesForceQueueRequest.customer.SelfSelectedStoreWD != 0)
                            item.Self_Selected_Store_WD = salesForceQueueRequest.customer.SelfSelectedStoreWD.ToString();
                    }


                    //self slected store Bilo 
                    if (salesForceQueueRequest.customer.SelfSelectedStoreBL.HasValue)
                    {
                        if (salesForceQueueRequest.customer.SelfSelectedStoreBL != 0)
                            item.Self_Selected_Store_BL = salesForceQueueRequest.customer.SelfSelectedStoreBL.ToString();
                    }

                    //self slected store Harveys 
                    if (salesForceQueueRequest.customer.SelfSelectedStoreHvy.HasValue)
                    {
                        if (salesForceQueueRequest.customer.SelfSelectedStoreHvy != 0)
                            item.Self_Selected_Store_HVY = salesForceQueueRequest.customer.SelfSelectedStoreHvy.ToString();
                    }

                    //self slected store FYM 
                    if (salesForceQueueRequest.customer.SelfSelectedStoreFYM.HasValue)
                    {
                        if (salesForceQueueRequest.customer.SelfSelectedStoreFYM != 0)
                            item.Self_Selected_Store_FYM = salesForceQueueRequest.customer.SelfSelectedStoreFYM.ToString();
                    }


                    //Insert_Date
                    if (salesForceQueueRequest.customer.CreatedDate != null && salesForceQueueRequest.customer.CreatedDate.HasValue)
                        item.Insert_Date = salesForceQueueRequest.customer.CreatedDate.ToString();

                    //Last_Modified_Date
                    if (salesForceQueueRequest.customer.LastUpdateDate != null && salesForceQueueRequest.customer.LastUpdateDate.HasValue)
                        item.Last_Modified_Date = salesForceQueueRequest.customer.LastUpdateDate.ToString();


                    /////WalletID
                    //if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.WalletId))
                    //{
                    //    salesForceQueueRequest.customer.WalletId;
                    //}

                    //////Baby_Club_Flag
                    //if (salesForceQueueRequest.customer.BabyClubWD)
                    //{
                    //    item.Baby_Club_Flag = "Y";
                    //}

                    //////Baby_Club_Flag
                    //if (salesForceQueueRequest.customer.BabyClubBL)
                    //{
                    //    item.Baby_Club_Flag = "Y";
                    //}

                    //////Baby_Club_Flag
                    //if (salesForceQueueRequest.customer.BabyClubHvy)
                    //{
                    //    item.Baby_Club_Flag = "Y";
                    //}

                    //////Baby_Club_Flag
                    //if (salesForceQueueRequest.customer.BabyClubFYM)
                    //{
                    //    item.Baby_Club_Flag = "Y";
                    //}

                    if (salesForceQueueRequest.customer.AlcoholPromoOptInStatus.HasValue)
                    {
                        if (salesForceQueueRequest.customer.AlcoholPromoOptInStatus.Value)
                        {
                            item.Alcohol_Promo_Opt_In = "Y";
                        }
                        else
                        {
                            item.Alcohol_Promo_Opt_In = "N";
                        }
                    }

                    if (!string.IsNullOrEmpty(salesForceQueueRequest.customer.EnrollmentOnlineVendor))
                    {
                        item.Enrollment_Online_Vendor = salesForceQueueRequest.customer.EnrollmentOnlineVendor;
                    }

                    //add items to list
                    items.Add(item);
                    dataExtentionsRequest.items = items;


                    dataExtentionsRequestInsert.dataExtentionsRequest = dataExtentionsRequest;

                    //////insert baby club Child . 
                    //if (salesForceQueueRequest.customer.CustomerChild != null && salesForceQueueRequest.customer.CustomerChild.Count() > 0)
                    //{

                    //    List<BabyClubChildItem> childItems = new List<BabyClubChildItem>();

                    //    foreach (CustomerChild child in salesForceQueueRequest.customer.CustomerChild)
                    //    {
                    //        BabyClubChildItem babyClubChildItem = new BabyClubChildItem();
                    //        //memberID 
                    //        babyClubChildItem.Member_ID = salesForceQueueRequest.customer.MemberId;

                    //        //child ID - child number .
                    //        if (child.ChildId.HasValue && child.ChildId != null)
                    //        {
                    //            babyClubChildItem.Member_Child_ID = string.Format("{0}_{1}", salesForceQueueRequest.customer.MemberId, child.ChildId.ToString());

                    //            babyClubChildItem.Child_ID = child.ChildId.ToString();
                    //        }

                    //        //baby first name 
                    //        if (!string.IsNullOrEmpty(child.FirstName))
                    //            babyClubChildItem.First_Name = child.FirstName;



                    //        //baby middle intial 
                    //        if (!string.IsNullOrEmpty(child.MiddleInitial))
                    //            babyClubChildItem.Middle_Initial = child.MiddleInitial.ToString();

                    //        //baby last name 
                    //        if (!string.IsNullOrEmpty(child.LastName))
                    //            babyClubChildItem.Last_Name = child.LastName;

                    //        // baby birth date 
                    //        if (child.BirthDate.HasValue && child.BirthDate != null)
                    //            babyClubChildItem.Birth_Date = child.BirthDate.ToString();

                    //        // baby expected indicator 
                    //        if (!string.IsNullOrEmpty(child.Expected))
                    //            babyClubChildItem.Expected_Baby_Indicator = child.Expected.ToString();

                    //        //baby gender code 
                    //        if (!string.IsNullOrEmpty(child.GenderCode))
                    //            babyClubChildItem.Gender_Code = child.GenderCode.ToString();

                    //        //baby deceased indicator
                    //        if (!string.IsNullOrEmpty(child.Deceased))
                    //            babyClubChildItem.Deceased_Indicator = child.Deceased.ToString();

                    //        // baby special needs 
                    //        if (!string.IsNullOrEmpty(child.SpecialNeeds))
                    //            babyClubChildItem.Special_Needs_Indicator = child.SpecialNeeds.ToString();

                    //        // baby aged out 
                    //        if (!string.IsNullOrEmpty(child.AgedOut))
                    //            babyClubChildItem.Aged_Out_Indicator = child.AgedOut.ToString();

                    //        //baby last updated date 
                    //        if (child.LastUpdateDate.HasValue)
                    //            babyClubChildItem.Last_Updated_Date = child.LastUpdateDate.ToString();



                    //        childItems.Add(babyClubChildItem);

                    //    }

                    //    dataExtentionsBabyClubChildRequest.items = childItems;
                    //    dataExtentionsRequestInsert.dataExtentionsBabyClubChildRequest = dataExtentionsBabyClubChildRequest;
                    //}
                    //var json = JsonConvert.SerializeObject(dataExtentionsRequestInsert);
                    DataExtentionsResponse response = (await SEG.Shared.ApiUtility.RestfulPostAsync<DataExtentionsResponse>(dataExtentionsRequestInsert, "UpsertAsync", WebApiEndPoint, QueryParams: queryParams, createHeader(OcpApimSubscriptionKeySecret)).ConfigureAwait(false)).Result;

                }

                else
                {
                    throw new ApplicationException("Unable to Apply ProcessSalesForceRequest as Customer info does not exists");
                }

            }
            catch (Exception ex)
            {
                Logging.Error(String.Format("An exception occurred while trying to ProcessSalesForceRequest. Error {0}", ex.Message), ex);
                throw;
            }

        }

        private Dictionary<string, object> createHeader(string OcpApimSubscriptionKeySecret)
        {
            Dictionary<string, object> headers = new Dictionary<string, object>();

            headers.Add("Ocp-Apim-Subscription-Key", OcpApimSubscriptionKeySecret);
            return headers;
        }
    }
}
