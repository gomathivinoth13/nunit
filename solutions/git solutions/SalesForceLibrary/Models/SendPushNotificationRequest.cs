using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SendPushNotificationRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "subscriberKeys", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SubscriberKeys { get; set; }
    }
}
