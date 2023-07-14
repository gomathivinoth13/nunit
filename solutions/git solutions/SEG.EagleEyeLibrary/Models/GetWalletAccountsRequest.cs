using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class GetWalletAccountsRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "walletId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }

        [JsonProperty(PropertyName = "memberId", NullValueHandling = NullValueHandling.Ignore)]
        public string MemberId { get; set; }

        [JsonProperty(PropertyName = "storeId", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreId
        {
            get { return _storeId; }
            set { _storeId = value?.PadLeft(4, '0'); }
        }
        private string _storeId;

        [JsonProperty(PropertyName = "accountId", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountId { get; set; }

        [JsonProperty(PropertyName = "chainId", NullValueHandling = NullValueHandling.Ignore)]
        public int ChainId { get; set; }

        [JsonProperty(PropertyName = "campaignId", NullValueHandling = NullValueHandling.Ignore)]
        public string  CampaignId { get; set; }


        //        include
        //array[string]($array)
        //(query)
        //A list of entities that should be included in the Account response

        //Available values : campaign.rules.redemption.maxRedemptionsPerPeriod, account.statistics.redemption


        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        /// <summary>
        /// Query string for Result filtering by Account Type(s). For single value square brackets could be omitted.
        /// </summary>
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public List<Type> Type { get; set; }

        /// <summary>
        /// This will be True if Customer Opts into Alcohol Promotions, False(or null) if they Opt Out
        /// </summary>
        [JsonProperty(PropertyName = "alcoholOffersAccepted", NullValueHandling = NullValueHandling.Ignore)]
        public bool AlcoholOffersAccepted { get; set; }

        //        clientType[] string ($string)(query)
        //Query string for Result filtering by Account clientType(s). For single value square brackets could be omitted.


        //        accountId[]string ($string)(query)
        //Query string for Result filtering by Wallet Account Id(s). For single value square square brackets could be omitted.


        //        parentAccountId string ($string)(query)
        //Query string allowing to filter out returned results by returning only child Accounts of supplied Account(i.e.Subscription Account)

        //validTo string ($date-time)(query)
        //Query string for Result Filtering by Account Valid To explicit date-time value or date-time range, for range named key attributes needs to be provided (validTo[to]= ... and/or validTo[from]= ...)

        //validFrom string ($date-time)(query)
        //Query string for Result Filtering by Account Valid From explicit date-time value or date-time range, for range named key attributes needs to be provided (validFrom[to]= ... and/or validFrom[from]= ...)

        //dateCreatedstring ($date-time)(query)
        //Query string for Result Filtering by Account Created explicit date-time value or date-time range, for range named key attributes needs to be provided (dateCreated[to]= ... and/or dateCreated[from]= ...)

        //lastUpdatedstring ($date-time)(query)
        //Query string for Result Filtering by Account Last Updated explicit date-time value or date-time range, for range named key attributes needs to be provided (lastUpdated[to]= ... and/or lastUpdated[from]= ...)



        /// <summary>
        ///  Query string for Result Filtering by Campaign Status.Please note enum below contains all possible Campaign statuses
        /// </summary>
        [JsonProperty(PropertyName = "campaign-status", NullValueHandling = NullValueHandling.Ignore)]
        public List<CampaignStatusType> CampaignStatus { get; set; }

        //        tokens string ($boolean)(query)
        //Show up to 100 active Tokens associated with each account.

        /// <summary>
        ///  //Query string Pagination Limit Default value : 20
        /// </summary>
        [JsonProperty(PropertyName = "limit", NullValueHandling = NullValueHandling.Ignore)]
        public int Limit { get; set; }


        /// <summary>
        ///  //Query string Pagination Offset Default value : 0
        /// </summary>
        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Ignore)]
        public int Offset { get; set; }


        /// <summary>
        /// /Query string for controlling Paginated Results Sorting Order.Please note this query string needs to be construted from field name + comma sign and sorting order(ASC/DESC). Currently AIR supports sorting by keys: accountId, validFrom, validTo, dateCreated, lastUpdated
        /// </summary>
        [JsonProperty(PropertyName = "orderBy", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> OrderBy { get; set; }
    }
}
