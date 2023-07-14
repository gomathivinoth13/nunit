using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SEG.StoreLocatorLibrary.Shared.Types;
using SEG.StoreLocatorLibrary.Shared.ResponseModels;

namespace SEG.StoreLocatorLibrary.Shared.Interfaces
{
    public interface IStoreLocatorDataAccess
    {
        Task<IList<Store>> GetStoresAsync(FilterDefinition<Store> filter);
        Task<IList<ZipcodeDetails>> GetZipCodesAsync(FilterDefinition<ZipcodeDetails> filter);
        //Option<BulkWriteResult<Store>> UpsertRecords(IList<WriteModel<Store>> stores);
        //Task<bool> DeleteRecord(int storeId);
        IList<Store> GetExistingStores();
        StoreUpdateResult DeleteStores(IList<int> storesToDelete);
        Task<StoreUpdateResult> UpdateStores(IList<Store> storesToUpdate, IList<Store> cosmosStores);
    }
}