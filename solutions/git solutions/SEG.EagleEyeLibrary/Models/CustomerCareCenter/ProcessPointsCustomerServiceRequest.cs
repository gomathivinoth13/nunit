using Newtonsoft.Json;
using SEG.ApiService.Models.AuditCustomerServicePoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models.CustomerCareCenter
{
    public class ProcessPointsCustomerServiceRequest
    {
        [JsonProperty(PropertyName = "memberID", NullValueHandling = NullValueHandling.Ignore)]
        public string MemberID { get; set; }


        [JsonProperty(PropertyName = "walletID", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletID { get; set; }


        [JsonProperty(PropertyName = "storeNumber", NullValueHandling = NullValueHandling.Ignore)]
        public int? StoreNumber { get; set; }

        [JsonProperty(PropertyName = "totalPoints", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalPoints { get; set; }


        //[JsonProperty(PropertyName = "createdDateTime", NullValueHandling = NullValueHandling.Ignore)]
        //public DateTime CreatedDateTime { get; set; }


        //[JsonProperty(PropertyName = "lastUpdatedDateTime", NullValueHandling = NullValueHandling.Ignore)]
        //public DateTime LastUpdatedDateTime { get; set; }


        [JsonProperty(PropertyName = "auditCustomerServiceTicket", NullValueHandling = NullValueHandling.Ignore)]
        public virtual AuditCustomerServiceTicket AuditCustomerServiceTicket { get; set; }

        [JsonProperty(PropertyName = "auditCustomerServiceRep", NullValueHandling = NullValueHandling.Ignore)]
        public virtual AuditCustomerServiceRep AuditCustomerServiceRep { get; set; }

    }

}
