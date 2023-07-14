namespace SEG.StoreLocatorLibrary.Shared.RequestModels
{
    public class GetStoreRequest
    {
        public string appCode { get; set; }
        public string transactionID { get; set; }
        public string appVer { get; set; }
        public int storeId { get; set; }
    }
}
