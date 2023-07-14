using System.Collections.Generic;
using SEG.StoreLocatorLibrary.Shared.Types;
using SEG.StoreLocatorLibrary.Shared.RequestModels;
using SEG.StoreLocatorLibrary.Shared.ResponseModels;
using SEG.StoreLocatorLibrary.Shared.CoreModels;
using System.Threading.Tasks;

namespace SEG.StoreLocatorLibrary.Shared.Interfaces
{
    public interface IStoreLocatorRepository
    {
        Option<IList<StoreResponse>> GetStores(GetStoreLocationsRequest request);
        Option<IList<StoreResponse>> GetClosestStores(GetClosestStoresRequest request);
        Option<StoreResponse> GetStore(GetStoreRequest request);
        Option<StoreAddressResponse> GetStoreAddress(GetStoreAddressRequest storeAddressRequest);
        StoreUpdateResult UpdateStoreDatabase(IList<Store> simStores);
        Option<IList<StoreOverrides>> ListStoreOverrides();
        Option<List<StoreOverrides>> SaveStoreOverrides(List<StoreOverrides> storeOverrides);
        Option<bool> ClearAllOverrides();
    }
}