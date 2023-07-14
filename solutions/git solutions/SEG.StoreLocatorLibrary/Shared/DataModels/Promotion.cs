using System.Runtime.Serialization;

namespace SEG.StoreLocatorLibrary.Shared
{
    public class Promotion
    {
        [DataMember(Name = "PromotionMarketCode", EmitDefaultValue = false)]
        public string PromotionMarketCode { get; set; }

        [DataMember(Name = "PromotionMarketDescription", EmitDefaultValue = false)]
        public string PromotionMarketDescription { get; set; }

        [DataMember(Name = "PromotionRegionCode", EmitDefaultValue = false)]
        public string PromotionRegionCode { get; set; }

        [DataMember(Name = "PromotionRegionOffer", EmitDefaultValue = false)]
        public string PromotionRegionOffer { get; set; }
    }
}
