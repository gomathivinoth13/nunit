using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "BIRTH_DATE", NullValueHandling = NullValueHandling.Ignore)]
        public string BIRTH_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Enrollment_Banner", NullValueHandling = NullValueHandling.Ignore)]
        public string Enrollment_Banner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Enrollment_Date", NullValueHandling = NullValueHandling.Ignore)]
        public string Enrollment_Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Insert_Date", NullValueHandling = NullValueHandling.Ignore)]
        public string Insert_Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Last_Modified_Date", NullValueHandling = NullValueHandling.Ignore)]
        public string Last_Modified_Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "ENROLLMENT_STATUS", NullValueHandling = NullValueHandling.Ignore)]
        public string ENROLLMENT_STATUS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "MEMBER_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string MEMBER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "FIRST_NAME", NullValueHandling = NullValueHandling.Ignore)]
        public string FIRST_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "LAST_NAME", NullValueHandling = NullValueHandling.Ignore)]
        public string LAST_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "ADDRESS_1", NullValueHandling = NullValueHandling.Ignore)]
        public string ADDRESS_1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "ADDRESS_2", NullValueHandling = NullValueHandling.Ignore)]
        public string ADDRESS_2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "CITY", NullValueHandling = NullValueHandling.Ignore)]
        public string CITY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "STATE", NullValueHandling = NullValueHandling.Ignore)]
        public string STATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "POSTAL_CODE", NullValueHandling = NullValueHandling.Ignore)]
        public string POSTAL_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "COUNTRY_CODE", NullValueHandling = NullValueHandling.Ignore)]
        public string COUNTRY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "EMAIL", NullValueHandling = NullValueHandling.Ignore)]
        public string EMAIL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "MOBILE_PHONE", NullValueHandling = NullValueHandling.Ignore)]
        public string MOBILE_PHONE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "EME_1", NullValueHandling = NullValueHandling.Ignore)]
        public string EME_1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "EME_2", NullValueHandling = NullValueHandling.Ignore)]
        public string EME_2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Store_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Store_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Wallet_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Wallet_ID { get; set; }

    }
}
