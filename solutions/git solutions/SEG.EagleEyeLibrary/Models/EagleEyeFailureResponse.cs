using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class EagleEyeFailureResponse
    {
        //
        // Summary:
        //     Gets or sets the error code.

        [JsonProperty(PropertyName = "errorMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string errorMessage { get; set; }

        [JsonProperty(PropertyName = "errorCode", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }
        //
        // Summary:
        //     Gets or sets information describing the error.

        [JsonProperty(PropertyName = "errorDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorDescription { get; set; }


        [JsonProperty(PropertyName = "details", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Details { get; set; }
    }
}
