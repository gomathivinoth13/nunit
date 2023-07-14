using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using SEG.StoreLocatorLibrary.Shared;
using SEG.StoreLocatorLibrary.Shared.Types;
using SEG.StoreLocatorLibrary.Shared.Interfaces;
using SEG.StoreLocatorLibrary.Shared.ConfigModels;
using SEG.StoreLocatorLibrary.Shared.RequestModels;
using SEG.StoreLocatorLibrary.Shared.ResponseModels;
using SEG.StoreLocatorLibrary.Shared.Extensions;
using SEG.StoreLocatorLibrary.Shared.CoreFunctions;
using MongoDB.Driver.GeoJsonObjectModel;
using SEG.StoreLocatorLibrary.Shared.CoreModels;

namespace SEG.StoreLocatorLibrary.Repository
{
    public class StoreLocatorRepository : IDisposable, IStoreLocatorRepository
    {
        private bool _isDisposed = false;
        private IStoreLocatorDataAccess _dataAccess;
        private StoreLocatorRepoConfig _config;


        public static IStoreLocatorRepository Connect(IStoreLocatorDataAccess dataAccess, Action<StoreLocatorRepoConfig> cfg)
        {
            var configData = new StoreLocatorRepoConfig();
            cfg(configData);
            return new StoreLocatorRepository(dataAccess, configData);
        }


        private StoreLocatorRepository(IStoreLocatorDataAccess dataAccess, StoreLocatorRepoConfig config)
        {
            _dataAccess = dataAccess;
            _config = config;
        }

        public Option<IList<StoreResponse>> GetStores(GetStoreLocationsRequest request)
        {
            try
            {
                //var redis = RedisService<StoreResponse>(request);
                //var response = redis.GetRecordsOrDefault();

                //if (response != null)
                //    return Option<IList<StoreResponse>>.Create(response);

                var stores = GetNearestStores(
                    request.Address.Latitude,
                    request.Address.Longitude,
                    request.Address.Zipcode,
                    request.Address.City,
                    request.Address.State,
                    request.Distance,
                    Functions.GetChainIDFromAppCode(request.AppCode),
                    request.Filter,
                    request.IsFutureStoreFlag);

                if (!stores.HasValue || stores.Value.Count() == 0)
                    return Option<IList<StoreResponse>>.CreateEmpty(10011, "Locations do not exists for given search Criteria");

                var resultStores = MappingSetup.Map<IList<Store>, List<StoreResponse>>(stores.Value);

                resultStores.ForEach(s => s.SetCircularUrl(_config));
                //redis.SaveRecords(resultStores);

                return Option<IList<StoreResponse>>.Create(resultStores);
            }
            catch (Exception ex)
            {
                return Option<IList<StoreResponse>>
                    .CreateEmpty(5000, $"Error in StoreLocatorRepository.GetStores: {ex.Message}");
            }
        }

        public Option<IList<StoreResponse>> GetClosestStores(GetClosestStoresRequest request)
        {
            try
            {
                //var redis = RedisService<StoreResponse>(request);
                //var response = redis.GetRecordsOrDefault();

                //if (response != null)
                //    return Option<IList<StoreResponse>>.Create(response);

                var stores = GetNearestStores(
                    request.latitude,
                    request.longitude,
                    request.zipCode,
                    request.city,
                    request.state,
                    request.radius,
                    Functions.GetChainIDFromAppCode(request.appCode),
                    request.filter,
                    request.isFutureStoreFlag);

                if (!stores.HasValue || stores.Value.Count() == 0)
                    return Option<IList<StoreResponse>>.CreateEmpty(10021, "Locations do not exists for given search Criteria");

                var resultStores = MappingSetup.Map<IList<Store>, List<StoreResponse>>(stores.Value);
                resultStores.ForEach(s => s.SetCircularUrl(_config));

                //redis.SaveRecords(resultStores);
                return Option<IList<StoreResponse>>.Create(resultStores);
            }
            catch (Exception ex)
            {
                return Option<IList<StoreResponse>>
                    .CreateEmpty(5000, $"Error in StoreLocatorRepository.GetClosestStores: {ex.Message}");
            }
        }

        public Option<StoreResponse> GetStore(GetStoreRequest request)
        {
            try
            {
                //var redis = RedisService<StoreResponse>(request);
                //var response = redis.GetRecordsOrDefault();

                //if (response != null)
                //    return Option<StoreResponse>.Create(response.FirstOrDefault());

                var chainId = Functions.GetChainIDFromAppCode(request.appCode);
                var filter = Builders<Store>.Filter
                    .Where(s => s.Chain_ID == $"{chainId}");
                filter &= Builders<Store>.Filter.Where(s => s.StoreCode == request.storeId);

                var stores = _dataAccess.GetStoresAsync(filter).GetAwaiter().GetResult();

                if (stores.Count == 0)
                    return Option<StoreResponse>.CreateEmpty(10031, "Store does not exits for given Banner");

                var resultStores = MappingSetup.Map<IList<Store>, List<StoreResponse>>(stores);
                resultStores.ForEach(s => s.SetCircularUrl(_config));
                //redis.SaveRecords(resultStores);

                return Option<StoreResponse>.Create(resultStores.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return Option<StoreResponse>
                    .CreateEmpty(5000, $"Error in StoreLocatorRepository.GetStore: {ex.Message}");
            }
        }


        public Option<StoreAddressResponse> GetStoreAddress(GetStoreAddressRequest storeAddressRequest)
        {
            try
            {
                //var redis = RedisService<StoreAddressResponse>(storeAddressRequest);
                //var response = redis.GetRecordsOrDefault();

                //if (response != null)
                //    return Option<StoreAddressResponse>.Create(response.FirstOrDefault());

                var filter = Builders<Store>.Filter.Where(s => s.StoreCode == storeAddressRequest.StoreId);

                var stores = _dataAccess.GetStoresAsync(filter).GetAwaiter().GetResult();

                if (stores.Count() == 0)
                    return Option<StoreAddressResponse>.CreateEmpty(10032, $"Store with Id={storeAddressRequest.StoreId} does not exits");

                var resultStores = MappingSetup.Map<IList<Store>, List<StoreAddressResponse>>(stores);
                //redis.SaveRecords(resultStores);

                return Option<StoreAddressResponse>.Create(resultStores.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return Option<StoreAddressResponse>
                    .CreateEmpty(5000, $"Error in StoreLocatorRepository.GetStore: {ex.Message}");
            }
        }


        public StoreUpdateResult UpdateStoreDatabase(IList<Store> simStores)
        {
            var storeUpdates = new List<WriteModel<Store>>();
            var updateResult = new StoreUpdateResult();

            try
            {
                // Override records from SQL DB
                var overrideStores = new OverrideRepository(_config).GetOverrideData().Value;

                // Update list of SIM Stores with data from Override table
                // updateStores list is the one to be imported
                var updatedStores = simStores
                    .Select(s => UpdateStore(s, overrideStores))
                    .Where(s => s.Chain_ID != "5")
                    .ToList();

                // Get all stores from CosmosDB
                var cosmosStores = _dataAccess.GetExistingStores();

                // Get a set of ID's of all existing stores from CosmosDB
                var existingIds = new HashSet<int>(cosmosStores.Select(s=>s.StoreCode));

                // A set of all SIM updated stores IDs from the updated list
                var simIds = new HashSet<int>(updatedStores.Select(s => s.StoreCode).ToList());

                // All stores not in the update set - to be deleted from CosmosDB
                var diff = existingIds.Except(simIds).ToList();

                // Delete records that are not in the current SIM set
                updateResult = _dataAccess.DeleteStores(diff);

                // Update CosmosDB from SIM Records
                var result = _dataAccess.UpdateStores(updatedStores, cosmosStores).GetAwaiter().GetResult();
                updateResult.TotalSimStores = simStores.Count;

                updateResult.IsSuccessful = updateResult.IsSuccessful && result.IsSuccessful;
                updateResult.StoresImported = result.StoresImported;
                updateResult.StoresUpdated = result.StoresUpdated;
                updateResult.Message += string.IsNullOrEmpty(updateResult.Message)
                    ? result.Message
                    : " | " + result.Message;

                // Redis cache was removed - user story 64827
                // Clean all cached data in Redis after updating DB
                //RedisConnection.Connect(_config).CleanCache(); 
                
                return updateResult;
            }
            catch (Exception ex)
            {
                updateResult.IsSuccessful = false;
                updateResult.Message += string.IsNullOrEmpty(updateResult.Message)
                    ? " | Error: " + ex.Message
                    : "Error: " + ex.Message;
                return updateResult;
            }

        }

        public Option<IList<StoreOverrides>> ListStoreOverrides()
        {
            OverrideRepository _overrideRepository = new OverrideRepository(_config);
            try
            {
                var overrideStores = _overrideRepository.ListStoreOverrides();
                return overrideStores;
            }
            catch (Exception ex)
            {
                return Option<IList<StoreOverrides>>.CreateEmpty(5000, $"Error in StoreLocatorRepository.ListStoreOverrides: {ex.Message}");
            }
        }

        public Option<List<StoreOverrides>> SaveStoreOverrides(List<StoreOverrides> storeOverrides)
        {
            OverrideRepository _overrideRepository = new OverrideRepository(_config);
            try
            {
                var result = _overrideRepository.SaveStoreOverrides(storeOverrides);
                return result;
            }
            catch (Exception ex)
            {
                return Option<List<StoreOverrides>>.CreateEmpty(5000, $"Error in StoreLocatorRepository.SaveStoreOverrides: {ex.Message}");
            }
        }

        public Option<bool> ClearAllOverrides()
        {
            OverrideRepository _overrideRepository = new OverrideRepository(_config);
            try
            {
                var result = _overrideRepository.ClearAllOverrides();
                return result;
            }
            catch (Exception ex)
            {
                return Option<bool>.CreateEmpty(5000, $"Error in StoreLocatorRepository.ClearAllOverrides: {ex.Message}");
            }
        }

        #region Helpers

        private Option<IList<Store>> GetNearestStores(double? latitude = null, double? longitude = null, string zipCode = null, string city = null, string state = null, int distance = 10, int? chainId = null, string filter = null, bool IsFutureStoreFlag = false)
        {
            FilterDefinition<ZipcodeDetails> zipCodeFilter = null;

            if (longitude == 0.0 && latitude == 0.0)
            {
                longitude = null;
                latitude = null;
            }

            if (!string.IsNullOrWhiteSpace(zipCode) && !(latitude.HasValue && longitude.HasValue))
            {
                zipCodeFilter = Builders<ZipcodeDetails>.Filter.Where(a => a.ZipCode == zipCode.Trim());
                var coordinates = _dataAccess.GetZipCodesAsync(zipCodeFilter)
                    .GetAwaiter()
                    .GetResult()
                    .Select(s => new double[2] { s.Longitude, s.Latitude })
                    .ToList();

                var centralCoordinate = GeoFunctions.GetCentralGeoCoordinate(coordinates);
                latitude = centralCoordinate[1];
                longitude = centralCoordinate[0];
            }
            else if ((!string.IsNullOrWhiteSpace(city) || !string.IsNullOrWhiteSpace(state)) && !(latitude.HasValue && longitude.HasValue))
            {
                if (!string.IsNullOrWhiteSpace(city))
                {
                    zipCodeFilter = Builders<ZipcodeDetails>.Filter.Where(a => a.CityStateName == city.Trim().Replace(".", "").ToUpper());
                }

                if (!string.IsNullOrWhiteSpace(state))
                {
                    var fil2 = Builders<ZipcodeDetails>.Filter.Where(a => a.StateAbrv == state.Trim().ToUpper());
                    if (zipCodeFilter != null)
                        zipCodeFilter = zipCodeFilter & fil2;
                    else zipCodeFilter = fil2;

                }
                var coordinates = _dataAccess.GetZipCodesAsync(zipCodeFilter)
                    .GetAwaiter()
                    .GetResult()
                    .Select(s => new double[2] { s.Longitude, s.Latitude })
                    .ToList();

                var centralCoordinate = GeoFunctions.GetCentralGeoCoordinate(coordinates);
                latitude = centralCoordinate[1];
                longitude = centralCoordinate[0];
            }

            var point = new GeoJson2DGeographicCoordinates(longitude.Value, latitude.Value);
            var pnt = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(point);
            var dis = distance * 1609.34;
            var fil = Builders<Store>.Filter.NearSphere(p => p.Location.Geo, pnt, dis);

            if (chainId.HasValue)
                fil = fil & Builders<Store>.Filter.Where(a => a.Chain_ID == $"{chainId}");

            var results = _dataAccess.GetStoresAsync(fil).GetAwaiter().GetResult().ToList();

            results.AsParallel().ForAll(r =>
            {
                r.Distance = GeoFunctions.DistanceTo(r.Location.Latitude, r.Location.Longitude, latitude.Value, longitude.Value);
            });

            if (filter != null)
            {
                var filterList = filter.Split(',').ToList();
                foreach (var deptFil in filterList)
                {
                    results = results.Where(a => a.departmentList.Split(',').Contains($"{deptFil}")).ToList();
                }
            }

            if (!IsFutureStoreFlag)
            {
                results = results.Where(x => x.IsFutureStoreFlag != true).ToList();
            }

            results = results.OrderBy(a => a.Distance).ToList();
            return Option<IList<Store>>.Create(results);
        }

        //private StoreLocatorRedisService<T> RedisService<T>(object request)
        //{
        //    try
        //    {
        //        var redisConection = _config.RedisCacheConnection;
        //        return new StoreLocatorRedisService<T>(RedisConnection.Connect(redisConection), request);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Cannot connect to Redis");
        //    }
        //}

        private Store UpdateStore (Store store, IList<DbOverrideModel> corrections)
        {
            if (corrections == null || corrections.Count == 0) return store;

            var correctedStore = corrections.FirstOrDefault(s => s.StoreCode == store.StoreCode);
            if (correctedStore == null)
                return store;

            if (correctedStore.StoreSize > 0) store.StoreSize = correctedStore.StoreSize;
            if (correctedStore.StoreName != null) store.StoreName = correctedStore.StoreName;
            if (correctedStore.StoreInformation != null) store.StoreInformation = correctedStore.StoreInformation;
            if (correctedStore.StoreBannerTypDesc != null) store.StoreBannerTypDesc = correctedStore.StoreBannerTypDesc;
            if (correctedStore.AddressLine1 != null) store.Address.AddressLine1 = correctedStore.AddressLine1;
            if (correctedStore.AddressLine2 != null) store.Address.AddressLine2 = correctedStore.AddressLine2;
            if (correctedStore.City != null) store.Address.City = correctedStore.City;
            if (correctedStore.State != null) store.Address.State = correctedStore.State;
            if (correctedStore.Zipcode != null) store.Address.Zipcode = correctedStore.Zipcode;
            if (correctedStore.County != null) store.Address.County = correctedStore.County;
            if (correctedStore.Country != null) store.Address.Country = correctedStore.Country;
            if (correctedStore.Latitude > 0) store.Location.Latitude = correctedStore.Latitude;
            if (correctedStore.Longitude > 0) store.Location.Longitude = correctedStore.Longitude;
            if (correctedStore.LocationTypeCode > 0) store.Location.LocationTypeCode = correctedStore.LocationTypeCode;
            if (correctedStore.LocationTypeDescription != null) store.Location.LocationTypeDescription = correctedStore.LocationTypeDescription;
            if (correctedStore.Phone != null) store.Phone = correctedStore.Phone;
            if (correctedStore.PharmacyHours != null) store.PharmacyHours = correctedStore.PharmacyHours;
            if (correctedStore.PharmacyPhone != null) store.Pharmacy.PharmacyPhone = correctedStore.PharmacyPhone;
            if (correctedStore.WorkingHours != null) store.WorkingHours = correctedStore.WorkingHours;
            if (correctedStore.Chain_ID != null) store.Chain_ID = correctedStore.Chain_ID;
            if (correctedStore.StartDate != null) store.StoreOpenDate = (DateTime)correctedStore.StartDate;
            if (correctedStore.EndDate != null) store.StoreCloseDate = (DateTime)correctedStore.EndDate;
            //if (correctedStore.TemporarilyClosed != null) store. = correctedStore.StoreSize; /// ***** TODO: 
            store.OnlineGrocery = correctedStore.OnlineGrocery;
            if (correctedStore.departmentList != null) store.departmentList = correctedStore.departmentList;
            if (correctedStore.StoreInfoMessage != null) store.StoreInfoMessage = correctedStore.StoreInfoMessage;

            return store;
        }


        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed || !disposing)
                return;
            _dataAccess = null;
            _isDisposed = true;
        }

        #endregion



    }
}
