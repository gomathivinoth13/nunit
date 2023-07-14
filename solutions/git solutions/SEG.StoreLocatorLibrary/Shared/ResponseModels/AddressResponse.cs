using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.StoreLocatorLibrary.Shared.ResponseModels
{
    public class AddressResponse
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
    }
}
