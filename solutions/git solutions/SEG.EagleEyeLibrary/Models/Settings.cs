using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class Settings
    {
        //    "tokenProvider": "EES",
        //"tokenFormat": "000000001",
        //"walletEnabled": true,
        //"issuanceMethod": null,
        //public string accountClientType": null,
        //"defaultAccountClientState": null



        [JsonProperty(PropertyName = "accountClientType", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountClientType { get; set; }
    }
}
