using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using StackExchange.Redis;


namespace PushNotificationSFMCFunctionApp.Implementation
{

    public abstract class AzureCacheApi
    {
        #region members
        protected static string _connectionString;
        protected IDatabase _cache;
        protected IDatabase _cacheStore;

        const int CacheDataBaseIdStore = 2;

        protected static ConnectionMultiplexer _connection
        {
            get { return ConnectionMultiplexer.Connect(_connectionString); }
        }

        #endregion

        public AzureCacheApi(string azcConnectionString)
        {
            SetupAzureCacheConnection(azcConnectionString);
        }

        private void SetupAzureCacheConnection(string azcConnectionString)
        {
            _connectionString = azcConnectionString;
            _cache = _connection.GetDatabase();
            _cacheStore = _connection.GetDatabase(CacheDataBaseIdStore);
        }
    }
}
