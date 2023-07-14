using System;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using SEG.StoreLocatorLibrary.Shared;
using SEG.StoreLocatorLibrary.Shared.Types;
using SEG.StoreLocatorLibrary.Shared.ConfigModels;
using System.Linq;
using SEG.StoreLocatorLibrary.Shared.CoreModels;
using Dapper;
using System.ComponentModel.Design;
using MongoDB.Driver;
using MongoDB.Bson;
using DnsClient.Internal;

namespace SEG.StoreLocatorLibrary.Repository
{
    public class OverrideRepository
    {
        private StoreLocatorRepoConfig _config;
        private string _connectionString;

        public OverrideRepository(StoreLocatorRepoConfig config)
        {
            _config = config;
            _connectionString = _config.OverrideDbConnection;
        }

        public Option<IList<DbOverrideModel>> GetOverrideData()
        {
            var sqlQuery = @"SELECT [StoreCode]
                            ,[StoreSize]
                            ,[DepartmentFlags]
                            ,[StoreName]
                            ,[StoreOpenTime]
                            ,[StoreCloseTime]
                            ,[StoreInformation]
                            ,[StoreBannerTypDesc]
                            ,[TimeZone]
                            ,[AddressLine1]
                            ,[AddressLine2]
                            ,[City]
                            ,[State]
                            ,[ZipCode]
                            ,[County]
                            ,[Country]
                            ,[Latitude]
                            ,[Longitude]
                            ,[LocationTypeCode]
                            ,[LocationTypeDescription]
                            ,[Phone]
                            ,[Email]
                            ,[PharmacyHours]
                            ,[PharmacyPhone]
                            ,[Twitter]
                            ,[FaceBook]
                            ,[Pintrest]
                            ,[Web]
                            ,[PromotionMarketCode]
                            ,[PromotionMarketDescription]
                            ,[PromotionRegionCode]
                            ,[PromotionRegionOffer]
                            ,CASE WHEN [StoreTimingsID] = 0 then NULL ELSE StoreTimingsID END as StoreTimingsID
                            ,[WorkingHours]
                            ,[Chain_ID]
                            ,[StartDate]
                            ,[EndDate]
                            ,[TemporailyClosed]
                            ,[OnlineGrocery]
                            ,[departmentList]
                            ,[StoreInfoMessage]
                        FROM [dbo].[StoreDetails_Override]
                        FOR JSON auto";
            var jsonString = new StringBuilder();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sqlQuery, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jsonString.Append(reader.GetString(0));
                            }
                        }
                    }
                }
                return Option<IList<DbOverrideModel>>
                    .Create(JsonConvert.DeserializeObject<IList<DbOverrideModel>>(jsonString.ToString()));
            }
            catch (Exception ex)
            {
                return Option<IList<DbOverrideModel>>.CreateEmpty(400, ex.Message);
            }
        }

        public Option<IList<StoreOverrides>> ListStoreOverrides()
        {
            IList<StoreOverrides> overrides = null;
            // List<StoreOverrides> overrides = new List<StoreOverrides>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sqlQuery = string.Format(
                        @"SELECT StoreCode, WorkingHours, PharmacyHours, TemporailyClosed, StoreInfoMessage, AddressLine1, AddressLine2, 
                        City, State, ZipCode, County, Country, Latitude, Longitude
                        FROM dbo.StoreDetails_Override
                        WHERE WorkingHours IS NOT NULL OR
                        PharmacyHours IS NOT NULL OR
                        TemporailyClosed IS NOT NULL OR
                        StoreInfoMessage IS NOT NULL 
                        ORDER BY StoreCode;");

                    overrides = (IList<StoreOverrides>)connection.Query<StoreOverrides>(sqlQuery);
                    return Option<IList<StoreOverrides>>.Create(overrides);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Option<List<StoreOverrides>> SaveStoreOverrides(List<StoreOverrides> storeOverrides)
        {
            List<StoreOverrides> overrides = new List<StoreOverrides>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction();

                    foreach (var _storeOverrides in storeOverrides)
                    {
                        var sqlQuery = string.Format(
                        @"IF EXISTS(SELECT StoreCode FROM dbo.StoreDetails_Override WHERE StoreCode = @StoreCode)
                        BEGIN
                        UPDATE dbo.StoreDetails_Override
                        SET WorkingHours = @WorkingHours, PharmacyHours = @PharmacyHours, TemporailyClosed = @TemporailyClosed, 
                        StoreInfoMessage = @StoreInfoMessage, AddressLine1 = @AddressLine1, AddressLine2 = @AddressLine2, 
                        City = @City, State = @State, ZipCode = @ZipCode, County = @County, Country = @Country, Latitude = @Latitude, Longitude = @Longitude
                        WHERE StoreCode = @StoreCode;
                        END 
                        ELSE
                        BEGIN
                        INSERT INTO dbo.StoreDetails_Override
                        (StoreCode, WorkingHours, PharmacyHours, TemporailyClosed, StoreInfoMessage, AddressLine1, AddressLine2, City, State, ZipCode, County, Country, Latitude, Longitude) 
                        Values(@StoreCode, @WorkingHours, @PharmacyHours, @TemporailyClosed, @StoreInfoMessage, @AddressLine1, @AddressLine2, @City, @State, @ZipCode, @County, @Country, @Latitude, @Longitude);
                        END

                        SELECT StoreCode, WorkingHours, PharmacyHours, TemporailyClosed, StoreInfoMessage, AddressLine1, AddressLine2, City, State, ZipCode, County, Country, Latitude, Longitude
                        FROM dbo.StoreDetails_Override WHERE StoreCode = @StoreCode");

                        var result = connection.Query<StoreOverrides>(sqlQuery, _storeOverrides, transaction);

                        overrides.AddRange(result);
                    }
                    transaction.Commit();
                }
                return Option<List<StoreOverrides>>.Create(overrides);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Option<bool> ClearAllOverrides()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sqlQuery = string.Format(
                        @"UPDATE dbo.StoreDetails_Override
                        SET WorkingHours = NULL,
                        PharmacyHours = NULL,
                        temporailyClosed = NULL,
                        storeInfoMessage = NULL");

                    connection.ExecuteScalar<bool>(sqlQuery);
                }
                return Option<bool>.Create(true);               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
