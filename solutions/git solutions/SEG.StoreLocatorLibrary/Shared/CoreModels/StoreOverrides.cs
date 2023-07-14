using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.StoreLocatorLibrary.Shared.CoreModels
{
    public class StoreOverrides
    {
        public int StoreCode { get; set; }
        public string StoreInfoMessage { get; set; }
        public bool ClearMessageFlag { get; set; }
        public bool TemporailyClosed { get; set; }
        public string WorkingHours { get; set; }
        public bool ResetStoreHours { get; set; }
        public string PharmacyHours { get; set; }
        public bool ResetPharmacyHours { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
