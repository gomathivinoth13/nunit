namespace SEG.StoreLocatorLibrary.Shared.Services
{
    public class EmailModel
    {
        public string subject { get; set; }
        public string toEmail { get; set; }
        public string fromEmail { get; set; }
        public string ccEmail { get; set; }
        public string errorResponse { get; set; }
        public string methodRequest { get; set; }
    }
}
