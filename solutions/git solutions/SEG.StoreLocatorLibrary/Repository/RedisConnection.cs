using System;
using StackExchange.Redis;
using SEG.StoreLocatorLibrary.Shared.ConfigModels;

namespace SEG.StoreLocatorLibrary.Repository
{
    //public class RedisConnection
    //{
    //    string _connectionString;
    //    IConnectionMultiplexer _connection;
    //    IDatabase _cacheDb;
    //    static RedisConnection _existingConnection;


    //    public static RedisConnection Connect(StoreLocatorRepoConfig config) =>
    //        Connect(config.RedisCacheConnection);

    //    public static RedisConnection Connect(string connectionString)
    //    {
    //        if (_existingConnection == null)
    //            _existingConnection = new RedisConnection(connectionString);

    //        return _existingConnection;
    //    }

    //    private RedisConnection(string connectionString)
    //    {
    //        _connectionString = connectionString;
    //        _connection = ConnectionMultiplexer.Connect(_connectionString);
    //        _cacheDb = _connection.GetDatabase();
    //    }

    //    private RedisConnection(StoreLocatorRepoConfig config) : this(config.RedisCacheConnection) { }

    //    public string GetValue(string key) => _cacheDb.StringGetAsync(key).GetAwaiter().GetResult();

    //    public void SetValue(string key, string value)
    //    {
    //        var exp = TimeSpan.FromHours(3);
    //        _cacheDb.StringSetAsync(key, value, expiry: exp).GetAwaiter();
    //    }

    //    public bool CleanCache()
    //    {
    //        try
    //        {
    //            var connection = ConnectionMultiplexer.Connect(_connectionString);
    //            var endpoints = connection.GetEndPoints(true);
    //            foreach (var endpoint in endpoints)
    //            {
    //                var _server = connection.GetServer(endpoint);
    //                _server.FlushAllDatabases();
    //            }
    //        }
    //        catch (Exception ex) // TODO: Research error "This operation is not available unless admin mode is enabled: FLUSHALL"
    //        {

    //        }

    //        return true;
    //    }
    //}
}
