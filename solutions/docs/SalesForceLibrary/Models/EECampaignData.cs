using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class EECampaignData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PointsMultiplier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int CouponID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EventId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int CouponMinimumQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        
        public String CouponBeginDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String CouponEndDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponDisclaimer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponCategory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponImage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SEGDigitalGroup { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int CouponRedemptionQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Brand { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OfferTargetingNewCustomer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PromoID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Deleted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Decimal DiscountBasketAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Decimal DiscountBasketPercentage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Decimal DiscountProductAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Decimal DiscountProductPercentage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Decimal DiscountProductFixedAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountProductFree { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int DiscountProductIntervalAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Decimal DiscountProductIntervalPercentage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Decimal CouponMinimumProductSpend { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponImageApp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponImageWeb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponOfferType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HiddenOffer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BannerPartnerInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BlIncludedStores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FrIncludedStores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HvIncludedStores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WdIncludedStores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BlExcludedStores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FrExcludedStores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HvExcludedStores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WdExcludedStores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BlIncludedStoreTags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FrIncludedStoreTags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HvIncludedStoreTags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WdIncludedStoreTags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BlExcludedStoreTags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FrExcludedStoreTags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HvExcludedStoreTags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WdExcludedStoreTags { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CouponFixedExpiryDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DigitalSavingsValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RollingExpiryValueType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int RollingExpiryValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PowerUp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int TotalTransactionCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Decimal TotalTransactionSpend { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int TotalTransactionUnits { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BounceBack { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Stamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AppExclusive { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BoosterCongratsDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MarketingRecommendedWelcomeOffer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ThemedDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PromoAltName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PromoTags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String PromoStartDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BrandName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OfferSummary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OfferDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MFGBrandName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MFGProduct { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EcreboImage { get; set; }
    }
}
