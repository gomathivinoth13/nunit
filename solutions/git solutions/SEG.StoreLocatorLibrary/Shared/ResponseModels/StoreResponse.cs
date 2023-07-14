using System;
using System.Runtime.Serialization;

// ***************************************************************
// * This is a "View Model" to be used do deliver payload to
// * the end user.
// ***************************************************************


namespace SEG.StoreLocatorLibrary.Shared.ResponseModels
{
    [DataContract]
    public class StoreResponse
    {
        [DataMember(Name = "StoreCode", EmitDefaultValue = false)]
        public int StoreCode { get; set; }

        [DataMember(Name = "DepartmentList", EmitDefaultValue = false)]
        public string departmentList { get; set; } = null;

        [DataMember(Name = "ParentStore", EmitDefaultValue = false)]
        public string ParentStore { get; set; }

        [DataMember(Name = "ChildStore", EmitDefaultValue = false)]
        public int? ChildStore { get; set; }

        [DataMember(Name = "ChildStoreRelation", EmitDefaultValue = false)]
        public string ChildStoreRelation { get; set; }

        [DataMember(Name = "ChildPhone", EmitDefaultValue = false)]
        public string ChildPhone { get; set; }

        [DataMember(Name = "StoreName", EmitDefaultValue = false)]
        public string StoreName { get; set; }

        [DataMember(Name = "StoreInformation", EmitDefaultValue = false)]
        public string StoreInformation { get; set; }

        [DataMember(Name = "Address", EmitDefaultValue = false)]
        public AddressResponse Address { get; set; } = new AddressResponse();

        [DataMember(Name = "Location", EmitDefaultValue = false)]
        public LocationResponse Location { get; set; } = new LocationResponse();

        [DataMember(Name = "Phone", EmitDefaultValue = false)]
        public string Phone { get; set; }

        [DataMember(Name = "Pharmacy", EmitDefaultValue = false)]
        public PharmacyResponse Pharmacy { get; set; } = new PharmacyResponse();

        [DataMember(Name = "WorkingHours", EmitDefaultValue = false)]
        public string WorkingHours { get; set; }

        [DataMember(Name = "PharmacyHours", EmitDefaultValue = false)]
        public string PharmacyHours { get; set; }

        [DataMember(Name = "Distance", EmitDefaultValue = true)]
        public double Distance { get; set; }

        [DataMember(Name = "WeeklyAds", EmitDefaultValue = false)]
        public string WeeklyAds { get; set; }

        [DataMember(Name = "OnlineGrocery", EmitDefaultValue = true)]
        public Boolean OnlineGrocery { get; set; }

        [DataMember(Name = "Chain_ID", EmitDefaultValue = false)]
        public string Chain_ID { get; set; }

        [DataMember(Name = "StoreBannerTypDesc", EmitDefaultValue = false)]
        public string StoreBannerTypDesc { get; set; }

        [DataMember(Name = "StoreOpenDate", EmitDefaultValue = false)]
        public DateTime StoreOpenDate { get; set; }

        [DataMember(Name = "StoreCloseDate", EmitDefaultValue = false)]
        public DateTime StoreCloseDate { get; set; }

        [DataMember(Name = "IsFutureStoreFlag", EmitDefaultValue = false)]
        public bool IsFutureStoreFlag { get; set; }

        [DataMember(Name = "StoreInfoMessage", EmitDefaultValue = false)]
        public string StoreInfoMessage { get; set; }

        [DataMember(Name = "Instacart", EmitDefaultValue = false)]
        public string Instacart { get; set; }

        [DataMember(Name = "Shipt", EmitDefaultValue = false)]
        public string Shipt { get; set; }

        [DataMember(Name = "Uber", EmitDefaultValue = false)]
        public string Uber { get; set; }

        [DataMember(Name = "Pickup", EmitDefaultValue = false)]
        public string Pickup { get; set; }

        [DataMember(Name = "Doordash", EmitDefaultValue = false)]
        public string Doordash { get; set; }

    }
}
