using System;
using System.Collections.Generic;

namespace SEG.StoreLocatorLibrary.Shared.ConfigModels
{
    public class StoreLocatorRepoConfig
    {
        public string OverrideDbConnection { get; set; }
        public string RedisCacheConnection { get; set; }
        public IDictionary<string, string> WeeklyAdsBaseURL { get; set; } = new Dictionary<string, string>();
    }
}
