using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// ***************************************************************
// * This is a data model for getting payload from SIM
// * To be used, it is being mapped into main Store model
// ***************************************************************

namespace SEG.StoreLocatorLibrary.Shared
{
    [DataContract]
    public class SimStore
    {
        [DataMember(Name = "StoreCode", EmitDefaultValue = false)]
        public int StoreCode { get; set; }

        [DataMember(Name = "StoreSize", EmitDefaultValue = false)]
        public int StoreSize { get; set; }

        [DataMember(Name = "StoreOpenDate", EmitDefaultValue = false)]
        public DateTime StoreOpenDate { get; set; }

        [DataMember(Name = "StoreCloseDate", EmitDefaultValue = false)]
        public DateTime StoreCloseDate { get; set; }

        [DataMember(Name = "IsFutureStoreFlag", EmitDefaultValue = false)]
        public bool IsFutureStoreFlag { get; set; }

        [DataMember(Name = "DepartmentList", EmitDefaultValue = false)]
        public string departmentList { get; set; } = null;

        [DataMember(Name = "StoreBannerTypDesc", EmitDefaultValue = false)]
        public string StoreBannerTypDesc { get; set; }

        [DataMember(Name = "StoreName", EmitDefaultValue = false)]
        public string StoreName { get; set; }

        [DataMember(Name = "StoreInformation", EmitDefaultValue = false)]
        public string StoreInformation { get; set; }

        [DataMember(Name = "ParentStore", EmitDefaultValue = false)]
        public string ParentStore { get; set; }

        [DataMember(Name = "ChildStore", EmitDefaultValue = false)]
        public int? ChildStore { get; set; }

        [DataMember(Name = "ChildStoreRelation", EmitDefaultValue = false)]
        public string ChildStoreRelation { get; set; }

        [DataMember(Name = "ChildPhone", EmitDefaultValue = false)]
        public string ChildPhone { get; set; }

        [DataMember(Name = "Str_Trt_Desc", EmitDefaultValue = false)]
        public string Str_Trt_Desc { get; set; }

        [DataMember(Name = "Chain_ID", EmitDefaultValue = false)]
        public string Chain_ID { get; set; }

        [DataMember(Name = "TimeZone", EmitDefaultValue = false)]
        public string TimeZone { get; set; }

        [DataMember(Name = "Address", EmitDefaultValue = false)]
        public SimAddress Address { get; set; }

        [DataMember(Name = "Location", EmitDefaultValue = false)]
        public SimLocation Location { get; set; }

        [DataMember(Name = "Phone", EmitDefaultValue = false)]
        public string Phone { get; set; }

        [DataMember(Name = "Pharmacy", EmitDefaultValue = false)]
        public SimPharmacy Pharmacy { get; set; }

        [DataMember(Name = "MediaLink", EmitDefaultValue = false)]
        public SimMediaLink MediaLink { get; set; }

        [DataMember(Name = "Promotion", EmitDefaultValue = false)]
        public SimPromotion Promotion { get; set; }

        [DataMember(Name = "Timings", EmitDefaultValue = false)]
        public List<SimTimings> Timings { get; set; }

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
