using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductSurveyItem
    {
        //
        // Summary:
        //     Member Id
        [JsonProperty(PropertyName = "memberId", NullValueHandling = NullValueHandling.Ignore)]
        public string MemberId { get; set; }
        //
        // Summary:
        //     CRC Id
        [JsonProperty(PropertyName = "crcId", NullValueHandling = NullValueHandling.Ignore)]
        public long CrcId { get; set; }
        //
        // Summary:
        //     LOC Id, NullValueHandling = NullValueHandling.Ignore
        [JsonProperty(PropertyName = "locId")]
        public short LocId { get; set; }
        //
        // Summary:
        //     UPC Code
        [JsonProperty(PropertyName = "upc_Code", NullValueHandling = NullValueHandling.Ignore)]
        public string UpcCode { get; set; }
        //
        // Summary:
        //     Transaction Date and Time
        [JsonProperty(PropertyName = "transactionDateTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime TransactionDateTime { get; set; }
        //
        // Summary:
        //     Banner
        [JsonProperty(PropertyName = "banner", NullValueHandling = NullValueHandling.Ignore)]
        public string Banner { get; set; }
        //
        // Summary:
        //     Satisfaction
        [JsonProperty(PropertyName = "satisfaction", NullValueHandling = NullValueHandling.Ignore)]
        public int Satisfaction { get; set; }
        //
        // Summary:
        //     Taste
        [JsonProperty(PropertyName = "taste", NullValueHandling = NullValueHandling.Ignore)]
        public int Taste { get; set; }
        //
        // Summary:
        //     Packaging
        [JsonProperty(PropertyName = "packaging", NullValueHandling = NullValueHandling.Ignore)]
        public int Packaging { get; set; }
        //
        // Summary:
        //     Size
        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore)]
        public int Size { get; set; }
        //
        // Summary:
        //     Visual Appeal
        [JsonProperty(PropertyName = "visualAppeal", NullValueHandling = NullValueHandling.Ignore)]
        public int VisualAppeal { get; set; }
        //
        // Summary:
        //     Buy It Again
        [JsonProperty(PropertyName = "buyItAgain", NullValueHandling = NullValueHandling.Ignore)]
        public bool BuyItAgain { get; set; }
        //
        // Summary:
        //     Comments
        [JsonProperty(PropertyName = "comments", NullValueHandling = NullValueHandling.Ignore)]
        public string Comments { get; set; }
    }
}
