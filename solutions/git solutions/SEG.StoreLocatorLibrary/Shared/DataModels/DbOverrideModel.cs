using System;
namespace SEG.StoreLocatorLibrary.Shared
{
    // ***************************************************************
    // * Model used to get data from SQL Override Db
    // ***************************************************************
    public class DbOverrideModel
    {
        public int StoreCode { get; set; }
        public int StoreSize { get; set; }
        public string DepartmentFlags { get; set; }
        public string StoreName { get; set; }
        public string StoreOpenTime { get; set; }
        public string StoreCloseTime { get; set; }
        public string StoreInformation { get; set; }
        public string StoreBannerTypDesc { get; set; }
        public string TimeZone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int LocationTypeCode { get; set; }
        public string LocationTypeDescription { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PharmacyHours { get; set; }
        public string PharmacyPhone { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Pintrest { get; set; }
        public string Web { get; set; }
        public string PromotionMarketCode { get; set; }
        public string PromotionMarketDescription { get; set; }
        public string PromotionRegionCode { get; set; }
        public string PromotionRegionOffer { get; set; }
        public int? StoreTimingsID { get; set; }
        public string WorkingHours { get; set; }
        public string Chain_ID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TemporarilyClosed { get; set; }
        public Boolean OnlineGrocery { get; set; }
        public string departmentList { get; set; }
        public string StoreInfoMessage { get; set; }
        public DateTime? LastOverrideTimeStamp { get; set; }
    }
}
