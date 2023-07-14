using System;
namespace SEG.StoreLocatorLibrary.Shared.ResponseModels
{
    public class StoreUpdateResult
    {
        public bool IsSuccessful { get; set; } = true;
        public int StoresUpdated { get; set; }
        public int StoresDeleted { get; set; }
        public int StoresImported { get; set; }
        public int TotalSimStores { get; set; }
        //public int TotalCosmosDb { get; set; }
        public string Message { get; set; }
    }
}
