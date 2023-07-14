using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.SalesForce.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisDatabaseOperations : RedisConnectorHelper
    {
        private StackExchange.Redis.IDatabase _cache;
        private bool _cacheAvailable;

        /// <summary>
        /// 
        /// </summary>
        public RedisDatabaseOperations()
        {
            _cache = CachedRepository();
            _cacheAvailable = RedisAvailable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue getCache(string key)
        {
            if (_cacheAvailable)
                return _cache.StringGet(key);
            else
                return string.Empty;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="serializedObject"></param>
       /// <param name="key"></param>
       /// <param name="expiresIn"></param>
        public void setCache(string serializedObject, string key, int expiresIn)
        {
            try
            {
                TimeSpan ttc = new TimeSpan(0, 0, expiresIn);
                if (ttc.Ticks > 0)
                {
                    if (_cacheAvailable)
                        _cache.StringSetAsync(key, serializedObject, ttc);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
