using SEG.StoreLocatorLibrary.Shared.Interfaces;
using SEG.StoreLocatorLibrary.Shared.CoreFunctions;
using SEG.StoreLocatorLibrary.Shared;
using System.Collections.Generic;
using System.Linq;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace SEG.StoreLocatorLibrary.Repository
{
    public class SIMRepository
    {

        public IList<Store> GetStores(ISimDataAccess dataAccess)
        {
            var simStores = GetSimStores(dataAccess);
            
            var stores = simStores.Select(s => ConvertStore(s)).ToList();
            return stores;
        }


        #region Helpers

        private IList<SimStore> GetSimStores(ISimDataAccess dataAccess)
        {
            return dataAccess.SimStores().Value ?? new List<SimStore>();
        }

        private Store ConvertStore(SimStore simStore)
        {
            var store = MappingSetup.Map<SimStore, Store>(simStore);

            SetCoordinates(store);
            SetPharmacyHours(simStore, store);
            SetWorkingHours(simStore, store);
            SetLastOverrideTimeStamp(store);

            return store;
        }

        private void SetCoordinates(Store store)
        {
            store.Address.Latitude = store.Location.Latitude;
            store.Address.Longitude = store.Location.Longitude;
        }
       
        private void SetWorkingHours(SimStore simStore, Store store)
        {
            store.WorkingHours = new StoreWorkingHours().GetFormattedHours(simStore.Timings);
        }

        private void SetPharmacyHours(SimStore simStore, Store store)
        {
            store.PharmacyHours = new PharmacyWorkingHours().GetFormattedHours(simStore.Pharmacy);
        }

        private void SetLastOverrideTimeStamp(Store store)
        {
            store.LastOverrideTimeStamp = DateTime.Now;
        }

        #endregion


    }
}
