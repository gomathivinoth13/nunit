using System;
namespace SEG.StoreLocatorLibrary.Shared.Services
{
    // Class used to compose the Email message in case of StoreUpdater Error
    public class EmailErrorLog
    {
        public string Process { get; set; }
        public string ErrorMessage { get; set; }
    }
}
