namespace SEG.StoreLocatorLibrary.Shared.RequestModels
{
    public class GetStoreLocationsRequest
    {
        public string AppCode { get; set; }
        public string TransactionID { get; set; }
        public string AppVer { get; set; }
        public Address Address { get; set; } = new Address();
        public StoresPaginationInfo PaginationInfo { get; set; } = new StoresPaginationInfo();
        public int Distance { get; set; }
        public string Filter { get; set; }
        public bool IsFutureStoreFlag { get; set; }
    }
}
