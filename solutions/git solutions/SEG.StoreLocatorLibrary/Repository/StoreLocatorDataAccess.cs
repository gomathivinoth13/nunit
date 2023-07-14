using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SEG.StoreLocatorLibrary.Shared;
using SEG.StoreLocatorLibrary.Shared.ResponseModels;
using SEG.StoreLocatorLibrary.Shared.Interfaces;
using System;
using System.Linq;

namespace SEG.StoreLocatorLibrary.Repository
{
    public class StoreLocatorDataAccess : IStoreLocatorDataAccess
    {
        private readonly IMongoDatabase _db;
        private MongoClient client;

        public static IStoreLocatorDataAccess Connect(string connectionString) => new StoreLocatorDataAccess(connectionString);

        private StoreLocatorDataAccess(string connectionString)
        {
            client = new MongoClient(connectionString);
            _db = client.GetDatabase("stores");
        }

        public async Task<IList<Store>> GetStoresAsync(FilterDefinition<Store> filter)
        {
            var collection = _db.GetCollection<Store>("StoreLocator");

            var builder = new IndexKeysDefinitionBuilder<Store>();

            List<IndexKeysDefinition<Store>> indexes = new List<IndexKeysDefinition<Store>>
                {
                    builder.Geo2DSphere(a => a.Location.Geo),
                    builder.Ascending(a => a.StoreCode),
                    builder.Ascending(a => a.Address.City),
                    builder.Ascending(a => a.Address.State),
                    builder.Ascending(a => new { a.Address.City, a.Address.State })
                };

            indexes.ForEach(i => collection.Indexes.CreateOneAsync(i));

            return await collection.Find(filter).ToListAsync();
        }

        public async Task<IList<ZipcodeDetails>> GetZipCodesAsync(FilterDefinition<ZipcodeDetails> filter)
        {
            var collection = _db.GetCollection<ZipcodeDetails>("ZipCodes");
            var builder2 = new IndexKeysDefinitionBuilder<ZipcodeDetails>();

            List<IndexKeysDefinition<ZipcodeDetails>> indexes = new List<IndexKeysDefinition<ZipcodeDetails>>
                {
                    builder2.Ascending(a => a.CityStateName),
                    builder2.Ascending(a => a.ZipCode),
                    builder2.Ascending(a => a.StateAbrv),
                    builder2.Ascending(a => new { a.CityStateName, a.StateAbrv})
                };

            indexes.ForEach(i => collection.Indexes.CreateOneAsync(i));
            return await collection.Find(filter).ToListAsync();
        }

        public StoreUpdateResult DeleteStores(IList<int> storesToDelete)
        {
            var response = new StoreUpdateResult();

            try
            {
                var collection = _db.GetCollection<Store>("StoreLocator");

                foreach (var storeId in storesToDelete)
                {
                    var filter = Builders<Store>.Filter.Eq("_id", storeId);
                    var result = collection.DeleteManyAsync(filter).GetAwaiter().GetResult();
                    response.StoresDeleted++;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<StoreUpdateResult> UpdateStores(IList<Store> storesToUpdate, IList<Store> cosmosStores)
        {
            var result = new StoreUpdateResult();
            var collection = _db.GetCollection<Store>("StoreLocator");

            foreach (var store in storesToUpdate)
            {
                try
                {
                    var lookupStore = cosmosStores.FirstOrDefault(s => s.StoreCode == store.StoreCode);

                    if (lookupStore == null)
                    {
                        //collection.InsertOneAsync(store).GetAwaiter().GetResult();
                        await collection.InsertOneAsync(store);
                        result.StoresImported++;
                        continue;
                    }

                    if (lookupStore == store)
                        continue;

                    var filter = Builders<Store>.Filter.Eq("_id", store.StoreCode);
                    collection.ReplaceOneAsync(filter, store).GetAwaiter().GetResult();
                    result.StoresUpdated++;
                }
                catch (MongoWriteException mrx)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    result.IsSuccessful = false;
                    result.Message = ex.Message;
                    return result;
                }
            }
            return result;
        }

        public IList<Store> GetExistingStores()
        {
            var collection = _db.GetCollection<Store>("StoreLocator");
            var filter = Builders<Store>.Filter.Empty;
            var stores = collection.Find(filter).ToList();
            return stores;
        }

    }
}