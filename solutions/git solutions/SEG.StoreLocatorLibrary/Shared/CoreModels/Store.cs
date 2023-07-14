using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// ***************************************************************
// * This is the main Store model currently being used in CosmosDB
// * Redundant properties below have ben used in the older app
// * but not used by Web of Mobile teams. Will be removed.
// ***************************************************************

namespace SEG.StoreLocatorLibrary.Shared
{
    [DataContract]
    public class Store
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
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

        [DataMember(Name = "StoreInfoMessage", EmitDefaultValue = false)]
        public string StoreInfoMessage { get; set; }

        [DataMember(Name = "Address", EmitDefaultValue = false)]
        public Address Address { get; set; } = new Address();

        [DataMember(Name = "Location", EmitDefaultValue = false)]
        public Location Location { get; set; } = new Location();

        [DataMember(Name = "Phone", EmitDefaultValue = false)]
        public string Phone { get; set; }

        [DataMember(Name = "Pharmacy", EmitDefaultValue = false)]
        public Pharmacy Pharmacy { get; set; } = new Pharmacy();

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

        [DataMember(Name = "LastOverrideTimeStamp", EmitDefaultValue = false)]
        public DateTime? LastOverrideTimeStamp { get; set; }




        // ************* REDUNDANT FROM OLD APP ****************
        // Currently exist in CosmosDB but not being used by app

        [DataMember(Name = "Timings", EmitDefaultValue = false)]
        public List<Timings> Timings { get; set; }

        [DataMember(Name = "MediaLink", EmitDefaultValue = false)]
        public MediaLink MediaLink { get; set; }

        [DataMember(Name = "Promotion", EmitDefaultValue = false)]
        public Promotion Promotion { get; set; }

        [DataMember(Name = "TimeZone", EmitDefaultValue = false)]
        public string TimeZone { get; set; }

        [DataMember(Name = "DepartmentFlags", EmitDefaultValue = false)]
        public List<string> DepartmentFlags { get; set; }

        [DataMember(Name = "StoreOpenTime", EmitDefaultValue = false)]
        public string StoreOpenTime { get; set; }

        [DataMember(Name = "StoreCloseTime", EmitDefaultValue = false)]
        public string StoreCloseTime { get; set; }

        [DataMember(Name = "Email", EmitDefaultValue = false)]
        public string Email { get; set; }

        [DataMember(Name = "PaginationInfo", EmitDefaultValue = false)]
        public PaginationInfo PaginationInfo { get; set; }

        [DataMember(Name = "TransactionId", EmitDefaultValue = false)]
        public string TransactionId { get; set; }

        [DataMember(Name = "StoreSize", EmitDefaultValue = false)]
        public int StoreSize { get; set; }


        [DataMember(Name = "Str_Trt_Desc", EmitDefaultValue = false)]
        public string Str_Trt_Desc { get; set; }



        // ********** From SIM **********

        public string FullText { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }

        #region Helpers

        public override int GetHashCode()
        {
            var hc1 = HashCode.Combine(StoreCode, departmentList, ParentStore, ChildStore, ChildStoreRelation, ChildPhone, StoreName, StoreInformation);
            var hc2 = HashCode.Combine(Phone, WorkingHours, PharmacyHours, OnlineGrocery, Chain_ID, StoreBannerTypDesc, StoreInfoMessage);
            var hc3 = HashCode.Combine(IsFutureStoreFlag, Location.Latitude, Location.Longitude, Location.LocationTypeDescription, Pharmacy.PharmacyPhone);
            var hc4 = HashCode.Combine(Address.AddressLine1, Address.AddressLine2, Address.City, Address.State, Address.Zipcode, Address.Country, Address.Country);
            var hc5 = HashCode.Combine(StoreSize, Instacart, Shipt, Uber, Pickup, Doordash);
            return HashCode.Combine(hc1, hc2, hc3, hc4, hc5);
        }

        public override bool Equals(object obj) => Equals(obj as Store);
        private bool Equals(Store store) => store != null && this.GetHashCode() == store.GetHashCode();

        public static bool operator ==(Store store1, Store store2) => Equals(store1, store2);
        public static bool operator !=(Store store1, Store store2) => !Equals(store1, store2);

        #endregion

    }
}
