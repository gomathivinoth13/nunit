using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotificationSFMCFunctionApp.Models
{
    public class MBOIssuancePushFailureResponse
    {

        [JsonProperty(PropertyName = "errorMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }
        [JsonProperty(PropertyName = "errorDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorDescription { get; set; }
        [JsonProperty(PropertyName = "details", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Details { get; set; }
    }
}
