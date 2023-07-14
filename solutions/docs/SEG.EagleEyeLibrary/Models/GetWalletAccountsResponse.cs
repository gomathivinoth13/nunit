using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class GetWalletAccountsResponse : EagleEyeFailureResponse
    {


        //{"total":8,"orderBy":[{"name":"accountId","order":"ASC"}],"limit":100,"offset":0,"results":[{"accountId":"360538969","walletId":"25388286","campaignId":"724406","type":"ECOUPON","clientType":null,"status":"ACTIVE","state":"LOADED","dates":{"start":"2020-04-20T23:02:59+01:00","end":"2020-04-30T23:59:59+01:00"},"meta":[],"dateCreated":"2020-04-20T23:02:59+01:00","lastUpdated":"2020-04-20T23:02:59+01:00","overrides":[],"balances":{"available":0,"refundable":0},"relationships":[]},{"accountId":"360538970","walletId":"25388286","campaignId":"725322","type":"ECOUPON","clientType":null,"status":"ACTIVE","state":"LOADED","dates":{"start":"2020-04-20T23:02:59+01:00","end":"2020-04-30T23:59:59+01:00"},"meta":[],"dateCreated":"2020-04-20T23:02:59+01:00","lastUpdated":"2020-04-20T23:02:59+01:00","overrides":[],"balances":{"available":0,"refundable":0},"relationships":[]},{"accountId":"360538990","walletId":"25388286","campaignId":"723247","type":"ECOUPON","clientType":null,"status":"ACTIVE","state":"LOADED","dates":{"start":"2020-04-20T23:08:41+01:00","end":"2020-04-30T23:59:59+01:00"},"meta":[],"dateCreated":"2020-04-20T23:08:41+01:00","lastUpdated":"2020-04-20T23:08:41+01:00","overrides":[],"balances":{"available":0,"refundable":0},"relationships":[]},{"accountId":"360538991","walletId":"25388286","campaignId":"725319","type":"ECOUPON","clientType":null,"status":"ACTIVE","state":"LOADED","dates":{"start":"2020-04-20T23:08:41+01:00","end":"2020-04-30T23:59:59+01:00"},"meta":[],"dateCreated":"2020-04-20T23:08:41+01:00","lastUpdated":"2020-04-20T23:08:41+01:00","overrides":[],"balances":{"available":0,"refundable":0},"relationships":[]},{"accountId":"360539007","walletId":"25388286","campaignId":"725309","type":"ECOUPON","clientType":null,"status":"ACTIVE","state":"LOADED","dates":{"start":"2020-04-20T23:15:29+01:00","end":"2020-04-30T23:59:59+01:00"},"meta":[],"dateCreated":"2020-04-20T23:15:29+01:00","lastUpdated":"2020-04-20T23:15:29+01:00","overrides":[],"balances":{"available":0,"refundable":0},"relationships":[]},{"accountId":"360539008","walletId":"25388286","campaignId":"723252","type":"ECOUPON","clientType":null,"status":"ACTIVE","state":"LOADED","dates":{"start":"2020-04-20T23:15:29+01:00","end":"2020-04-30T23:59:59+01:00"},"meta":[],"dateCreated":"2020-04-20T23:15:29+01:00","lastUpdated":"2020-04-20T23:15:29+01:00","overrides":[],"balances":{"available":0,"refundable":0},"relationships":[]},{"accountId":"360539009","walletId":"25388286","campaignId":"725302","type":"ECOUPON","clientType":null,"status":"ACTIVE","state":"LOADED","dates":{"start":"2020-04-20T23:15:36+01:00","end":"2020-04-30T23:59:59+01:00"},"meta":[],"dateCreated":"2020-04-20T23:15:36+01:00","lastUpdated":"2020-04-20T23:15:36+01:00","overrides":[],"balances":{"available":0,"refundable":0},"relationships":[]},{"accountId":"360539010","walletId":"25388286","campaignId":"725300","type":"ECOUPON","clientType":null,"status":"ACTIVE","state":"LOADED","dates":{"start":"2020-04-20T23:15:36+01:00","end":"2020-04-30T23:59:59+01:00"},"meta":[],"dateCreated":"2020-04-20T23:15:36+01:00","lastUpdated":"2020-04-20T23:15:36+01:00","overrides":[],"balances":{"available":0,"refundable":0},"relationships":[]}]}


        [JsonProperty(PropertyName = "limit", NullValueHandling = NullValueHandling.Ignore)]
        public int Limit { get; set; }

        [JsonProperty(PropertyName = "total", NullValueHandling = NullValueHandling.Ignore)]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Ignore)]

        public int Offset { get; set; }

        [JsonProperty(PropertyName = "filter", NullValueHandling = NullValueHandling.Ignore)]

        public List<string> Filter { get; set; }

        [JsonProperty(PropertyName = "orderBy", NullValueHandling = NullValueHandling.Ignore)]

        public List<OrderBy> OrderBy { get; set; }


        [JsonProperty(PropertyName = "results", NullValueHandling = NullValueHandling.Ignore)]

        public List<Account> Results { get; set; }



    }
}
