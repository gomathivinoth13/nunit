namespace SEG.StoreLocatorLibrary.Shared.RequestModels
{
    public class GetClosestStoresRequest
    {
        public string appCode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public string filter { get; set; }
        public float? latitude { get; set; }
        public float? longitude { get; set; }
        public int radius { get; set; }
        public bool isFutureStoreFlag { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
    }
}
