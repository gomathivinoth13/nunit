using System;
using System.Text.Json.Serialization;

namespace WalletAccountDataProcessorFunctionApp.Models
{
    public class WalletAccountIDEventData
    {
        [JsonPropertyName("EventID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string EventID { get; set; }

        [JsonPropertyName("EventName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string EventName { get; set; }

        [JsonPropertyName("ObjectType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ObjectType { get; set; }

        [JsonPropertyName("Event")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Event { get; set; }

        [JsonPropertyName("AccountID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AccountID { get; set; }

        [JsonPropertyName("WalletID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string WalletID { get; set; }

        [JsonPropertyName("CampaignID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CampaignID { get; set; }

        [JsonPropertyName("State")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string State { get; set; }
        [JsonPropertyName("Type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Type { get; set; }

        [JsonPropertyName("Status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Status { get; set; }

        [JsonPropertyName("ClientType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ClientType { get; set; }

        [JsonPropertyName("Created_DT")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime Created_DT { get; set; }

        [JsonPropertyName("Created_Source")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Created_Source { get; set; }

        [JsonPropertyName("Updated_DT")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime Updated_DT { get; set; }

        public Dates Dates { get; set; }

    }

}