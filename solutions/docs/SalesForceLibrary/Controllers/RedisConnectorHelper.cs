using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SEG.SalesForce.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisConnectorHelper
    {
        static string _localHost;
        private IDatabase redisCache;
        private bool IsInitialized;

        private StackExchange.Redis.IDatabase _cache;
        private bool _cacheAvailable;

        /// <summary>
        /// 
        /// </summary>
        public static string localHost
        {
            get
            {
                return _localHost;
            }

            set
            {
                _localHost = value;
            }
        }
        static RedisConnectorHelper()
        {
            RedisConnectorHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                //ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);
                return ConnectionMultiplexer.Connect(localHost);
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        /// <summary>
        /// 
        /// </summary>
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDatabase CachedRepository()
        {
            if (Connection.IsConnected)
            {
                redisCache = Connection.GetDatabase();
                IsInitialized = true;
            }
            else
            {
                redisCache = null;
                IsInitialized = false;
            }

            return redisCache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RedisAvailable()
        {
            return IsInitialized;
        }
    }
}

