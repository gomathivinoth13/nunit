using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Coupon
    {
        [JsonProperty(PropertyName = "recommendationGuid", NullValueHandling = NullValueHandling.Ignore)]
        public string RecommendationGuid { get; set; }

        [JsonProperty(PropertyName = "catalogueGuid", NullValueHandling = NullValueHandling.Ignore)]
        public string CatalogueGuid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AccountID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponWeight { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EventId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountBasketAmount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountBasketPercentage { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountProductAmount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountProductPercentage { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountProductFixedAmount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountProductFree { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountProductIntervalAmount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountProductIntervalPercentage { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponMinimumQuantity { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponMinimumProductSpend { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponDescription { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponBeginDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponEndDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponImageApp { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponImageWeb { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponDisclaimer { get; set; }// Terms and Conditions(Character Limit??)

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //(Store or Manufacturer)
        public string CouponOfferType { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> BannerPartnerInfo { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SEGDigitalGroup { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponRedemptionQuantity { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Brand { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OfferTargetingNewCustomer { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PromoID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> BlIncludedStores { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> FrIncludedStores { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> HvIncludedStores { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> WdIncludedStores { get; set; }



        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> BlExcludedStores { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> FrExcludedStores { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> HvExcludedStores { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> WdExcludedStores { get; set; }




        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> BlIncludedStoreTags { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> FrIncludedStoreTags { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> HvIncludedStoreTags { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> WdIncludedStoreTags { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> BlExcludedStoreTags { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> FrExcludedStoreTags { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> HvExcludedStoreTags { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> WdExcludedStoreTags { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Deleted { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HiddenOffer { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponFixedExpiryDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DigitalSavingsValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RollingExpiryValueType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RollingExpiryValue { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ItemUPCInc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ItemGroupsInc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ItemTagsInc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ItemWDCodeInc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ItemGroupsExc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ItemTagsExc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ItemUPCExc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ItemWDCodeExc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PowerUp { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TotalTransactionCount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TotalTransactionSpend { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TotalTransactionUnits { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TransactionCount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TotalSpend { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TotalUnits { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public string BounceBack { get; set; }


        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public string Stamp { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LastUpdatedDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AppExclusive { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TotalIssuanceCount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TotalIssuanceConsumed { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool DealOfTheWeek { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BoosterCongratsDescription { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? MarketingRecommendedWelcomeOffer { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ThemedDescription { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> PromoTags { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ForceRanked { get; set; }
    }
}
