using System;
using SEG.StoreLocatorLibrary.Shared.ResponseModels;
using System.Collections;
using SEG.StoreLocatorLibrary.Shared.ConfigModels;

namespace SEG.StoreLocatorLibrary.Shared.Extensions
{
    public static class StoreResponseExtensions
    {
        public static void SetCircularUrl(this StoreResponse store, StoreLocatorRepoConfig config)
        {
            try
            {
                var baseUrl = config.WeeklyAdsBaseURL[store.Chain_ID];
                var storeCode = store.StoreCode.ToString().PadLeft(4, '0');
                store.WeeklyAds = $"{baseUrl}?store_code={storeCode}";
            }
            catch (Exception ex)
            {
                store.WeeklyAds = "";
            }
        }
    }
}
