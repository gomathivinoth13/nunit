using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Identity
    {
        /// </summary>
        [JsonProperty(PropertyName = "identityId", NullValueHandling = NullValueHandling.Ignore)]
        public string IdentityId { get; set; }

        /// </summary>
        [JsonProperty(PropertyName = "walletId", NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }

        /// </summary>
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        /// </summary>
        [JsonProperty(PropertyName = "value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        //{"total":5,"orderBy":[{"name":"identityId","order":"ASC"}],"limit":100,"offset":0,"results":[{"identityId":"38900232","walletId":"42198022","type":"MEMBER_ID","friendlyName":null,"value":"SEGQA0000005314083","safeValue":null,"secret":null,"dates":{"start":"2021-02-12T21:01:12+00:00","end":null},"meta":[],"state":"DEFAULT","status":"ACTIVE","dateCreated":"2021-02-12T21:01:12+00:00","lastUpdated":"2021-02-12T21:01:12+00:00","mobileWallet":"https:\/\/demoapi.podifi.com\/passbook\/add\/43452\/42198022\/38900232\/0d92ac217b9bf451038c1ff293597f389855b67003ee723de398e9f310e95393"},{"identityId":"38900233","walletId":"42198022","type":"SEGR","friendlyName":null,"value":"7221180201198017","safeValue":null,"secret":null,"dates":{"start":"2021-02-12T21:01:12+00:00","end":null},"meta":[],"state":"DEFAULT","status":"ACTIVE","dateCreated":"2021-02-12T21:01:12+00:00","lastUpdated":"2021-02-12T21:01:12+00:00","mobileWallet":"https:\/\/demoapi.podifi.com\/passbook\/add\/43452\/42198022\/38900233\/8188f4e9a9dfaba1b9ae722911a86929f91d8fa340af8cdd221efc6e75e38372"},{"identityId":"38900234","walletId":"42198022","type":"REWARDCARD","friendlyName":null,"value":"48107812902","safeValue":null,"secret":null,"dates":{"start":"2021-02-12T21:01:12+00:00","end":null},"meta":[],"state":"DEFAULT","status":"ACTIVE","dateCreated":"2021-02-12T21:01:12+00:00","lastUpdated":"2021-02-12T21:01:12+00:00","mobileWallet":"https:\/\/demoapi.podifi.com\/passbook\/add\/43452\/42198022\/38900234\/8d8d28ed0aa1b0494c7605e49c77b4652f3a9a58c66d509a8c0402403cc9b304"},{"identityId":"38900235","walletId":"42198022","type":"PHONENUMBER","friendlyName":null,"value":"8082638638","safeValue":null,"secret":null,"dates":{"start":"2021-02-12T21:01:12+00:00","end":null},"meta":[],"state":"DEFAULT","status":"ACTIVE","dateCreated":"2021-02-12T21:01:12+00:00","lastUpdated":"2021-02-12T21:01:12+00:00","mobileWallet":"https:\/\/demoapi.podifi.com\/passbook\/add\/43452\/42198022\/38900235\/de733dfb431c2a076e8c56c73ad083e7918787cb093145add7d7dea004e15086"},{"identityId":"38907591","walletId":"42198022","type":"REWARDCARD","friendlyName":null,"value":"48302539825","safeValue":null,"secret":null,"dates":{"start":"2021-02-15T16:18:28+00:00","end":null},"meta":[],"state":"DEFAULT","status":"ACTIVE","dateCreated":"2021-02-15T16:18:28+00:00","lastUpdated":"2021-02-15T16:18:28+00:00","mobileWallet":"https:\/\/demoapi.podifi.com\/passbook\/add\/43452\/42198022\/38907591\/f77ea6bcf5b51dfdfd9e74038bfdbaf2c6fcb8155d351d73118fae756b7f6366"}]}


    }
}
